using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using MiNET;
using MiNET.Utils;
using MiNET.Worlds;
using Timer = System.Threading.Timer;

namespace MiNETBiomeMap
{
    public partial class BiomeMapForm : Form
    {
        private static readonly TimeSpan ShowUpdateProgressThreshold = TimeSpan.FromSeconds(3);

        private static readonly ILog Log = LogManager.GetLogger(typeof(BiomeMapForm));
        
        private Level _level;

        private Bitmap _bitmap;

        private BiomeUtils _biomeUtils;

        private bool _validSelection;

        private Point _currentPoint;
        private ChunkCoordinates _currentChunk;

        private ChunkCoordinates _minChunk;
        private ChunkCoordinates _maxChunk;

        private object _chunkSync = new object();
        private IList<ChunkCoordinates> _validChunks = new List<ChunkCoordinates>();

        private DisplayMode Mode = DisplayMode.BiomeGrass;

        protected IReadOnlyList<ChunkCoordinates> ValidChunks
        {
            get
            {
                lock (_chunkSync)
                {
                    return _validChunks.ToArray();
                }
            }
        }

        private Image _baseImage;

        private bool _isBusy;
        private object _busy = new object();
        private bool _cancelProgress;
        private int _lastChunkCount;

        private TimeSpan _lastUpdateDuration = TimeSpan.Zero;

        public BiomeMapForm(Level level)
        {
            _level = level;

            _biomeUtils = new BiomeUtils();
            _biomeUtils.PrecomputeBiomeColors();

            InitializeComponent();

            Closing += (sender, args) =>
            {
                _cancelProgress = true;
            };
        }
        
        public void UpdateLevel()
        {
            Invoke(new MethodInvoker(delegate ()
            {
                var chunks = _level.GetLoadedChunks();
                if (chunks.Length == 0) return;

                if (!Monitor.TryEnter(_busy)) return;

                var showProgress = _lastUpdateDuration > ShowUpdateProgressThreshold;

                try
                {
                    Busy();

                    var sw = Stopwatch.StartNew();

                    var minChunkX = chunks.Min(c => c.x);
                    var minChunkZ = chunks.Min(c => c.z);

                    var maxChunkX = chunks.Max(c => c.x);
                    var maxChunkZ = chunks.Max(c => c.z);

                    var imageWidth = (maxChunkX - minChunkX + 1) * 16;
                    var imageHeight = (maxChunkZ - minChunkZ + 1) * 16;

                    var offsetX = Math.Abs(minChunkX * 16);
                    var offsetZ = Math.Abs(minChunkZ * 16);

                    var tot = chunks.Length;

                    Bitmap image;
                    bool usingBase = false;

                    if (_baseImage != null &&
                        imageWidth == _baseImage.Width && imageHeight == _baseImage.Height
                        && minChunkX == _minChunk.X && minChunkZ == _minChunk.Z
                        && maxChunkX == _maxChunk.X && maxChunkZ == _maxChunk.Z
                    )
                    {
                        image = (Bitmap) _baseImage.Clone();
                        usingBase = true;

                        if (tot == _lastChunkCount)
                        {
                            return;
                        }
                    }
                    else
                    {
                        image = new Bitmap(imageWidth, imageHeight);
                    }

                    _lastChunkCount = tot;


                    if (showProgress)
                        ShowProgress(tot);

                    var i = 0;
                    foreach (var chunk in chunks)
                    {
                        lock (_chunkSync)
                        {
                            var coords = new ChunkCoordinates(chunk.x, chunk.z);
                            if (_validChunks.Contains(coords))
                            {
                                if(!chunk.isDirty && usingBase)
                                    continue;
                            }
                            else
                            {
                                _validChunks.Add(coords);
                            }
                        }

                        if (showProgress)
                            UpdateProgress(i, "Drawing Bitmap");
                        i++;
                        //Debug.WriteLine("Processing Chunk {0}/{1}", i, tot);
                        for (int x = 0; x < 16; x++)
                        {
                            for (int z = 0; z < 16; z++)
                            {
                                var iX = offsetX + chunk.x * 16 + x;
                                var iZ = offsetZ + chunk.z * 16 + z;

                                //Debug.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", iX, iZ, imageWidth, imageHeight, offsetX, offsetZ, chunk.x, chunk.z, x, z));
                                var biomeId = chunk.GetBiome(x, z);
                                var color = GetBiomeColor(biomeId);

                                if (Mode == DisplayMode.Rainfall)
                                {
                                    color = HeatMap.GetColor(0.33M, 0.66M, 1M,
                                        (decimal) (_biomeUtils.GetBiome(biomeId).Downfall /
                                                   BiomeMapUtil.MaxBiomeDownfall));
                                }
                                else if (Mode == DisplayMode.Temperature)
                                {
                                    color = HeatMap.GetColor(0.33M, 0.66M, 1M,
                                        (decimal) (_biomeUtils.GetBiome(biomeId).Temperature /
                                                   BiomeMapUtil.MaxBiomeTemperature));
                                }

                                image.SetPixel(iX, iZ, color);
                            }
                        }
                    }

                    _baseImage = (Bitmap) image.Clone();

                    foreach (var player in _level.Players.Values.ToArray())
                    {
                        var pX = offsetX + (int) player.KnownPosition.X;
                        var pZ = offsetX + (int) player.KnownPosition.Z;

                        //Debug.WriteLine("Player at " + pX + "," + pZ);
                        var r = 2;

                        for (int x = -r; x <= r; x++)
                        {
                            for (int z = -r; z <= r; z++)
                            {
                                image.SetPixel(pX + x, pZ + z, Color.Black);
                            }
                        }
                    }

                    sw.Stop();
                    _lastUpdateDuration = sw.Elapsed;

                    _minChunk = new ChunkCoordinates(minChunkX, minChunkZ);
                    _maxChunk = new ChunkCoordinates(maxChunkX, maxChunkZ);

                    UpdateImage();

                    //Debug.WriteLine("Drawing Image");
                    statusChunks.Text = tot + " Chunks";

                }
                catch (Exception ex)
                {
                    Log.Error("Exception during bitmap update", ex);
                }
                finally
                {
                    Monitor.Exit(_busy);

                    Busy(false);

                    if (showProgress)
                        HideProgress();
                }
            }));
        }

        private void UpdateImage()
        {
            if (_baseImage == null) return;

            var img = (Bitmap) _baseImage.Clone();

            if (_validSelection)
            {
                var cX = (_currentChunk.X - _minChunk.X) * 16;
                var cZ = (_currentChunk.Z - _minChunk.Z) * 16;

                for (int x = 0; x < 16; x++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        var pX = cX + x;
                        var pZ = cZ + z;

                        var color = img.GetPixel(pX, pZ);

                        if (x == 0 || z == 0 || x == 15 || z == 15)
                        {
                            color = Darken(color, 0.2f);
                        }
                        else
                        {
                            color = Lighten(color, 0.2f);
                        }

                        img.SetPixel(pX, pZ, color);
                    }
                }
            }

            Invoke(new MethodInvoker(delegate ()
            {
                if (_validSelection)
                    statusPosition.Text = $"{_currentChunk.X},{_currentChunk.Z}";
                else
                    statusPosition.Text = "";

                statusDuration.Text = Math.Round(_lastUpdateDuration.TotalMilliseconds, 2) + "ms";

                pictureBox1.Image = img;
            }));
        }

        private Color GetBiomeColor(byte biomeId)
        {
            var biome = _biomeUtils.GetBiome(biomeId);
            var c = biome.Grass;

            if (Mode == DisplayMode.BiomeFoilage)
            {
                c = biome.Foliage;
            }

            int r = (int)((c >> 16) & 0xff);
            int g = (int)((c >> 8) & 0xff);
            int b = (int)c & 0xff;

            //Debug.WriteLine("Biome {0}: {1} {2}", biomeId, biome.Grass, biome.Foliage);

            return Color.FromArgb(r, g, b);
        }

        private void BiomeMapForm_Load(object sender, EventArgs e)
        {
            statusLevel.Text = _level.LevelName;
            statusProvider.Text = _level._worldProvider.GetType().Name;
            statusChunks.Text = "0 Chunks";

            toolStripComboBox1.SelectedIndex = 0;
            toolStripComboBox2.SelectedIndex = 0;
        }
        
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            var img = pictureBox1.Image;
            var box = pictureBox1;

            if (img == null) return;

            float imgRatio = img.Width / (float)img.Height;
            float boxRatio = box.Width / (float) box.Height;

            int locX = 0, locY = 0;

            if (imgRatio >= boxRatio)
            {
                float sf = box.Width / (float) img.Width;
                float sh = img.Height * sf;

                float offset = Math.Abs(box.Height - sh) / 2;
                locX = (int) (e.Location.X / sf);
                locY = (int) ((e.Location.Y - offset) / sf);
            }
            else
            {
                float sf = box.Height / (float) img.Height;
                float sw = img.Width * sf;

                float offset = Math.Abs(box.Width - sw) / 2;

                locX = (int) ((e.Location.X - offset) / sf);
                locY = (int) (e.Location.Y / sf);
            }
            
            var x = (int) Math.Floor(locX / 16f) + _minChunk.X;
            var y = (int)Math.Floor(locY / 16f) + _minChunk.Z;
            
            if (ValidChunks.ToArray().Contains(new ChunkCoordinates(x, y)))
            {
                //statusPosition.Text = x + "," + y + "(" + offsetX + "," + offsetY + ")";
                _currentPoint = new Point(locX  + _minChunk.X * 16, locY + _minChunk.Z*16);
                var biome = _biomeUtils.GetBiome(_level.GetBlock(_currentPoint.X, 0, _currentPoint.Y).BiomeId);

                statusBiome.Text = biome.Name;
                statusBiomeTemp.Text = $"Temp {biome.Temperature}";
                statusBiomeDownfall.Text = $"Rain {biome.Downfall}";

                statusBiome.Visible = true;
                statusBiomeTemp.Visible = true;
                statusBiomeDownfall.Visible = true;

                _currentChunk = new ChunkCoordinates(x, y);
                _validSelection = true;
            }
            else
            {
                statusBiome.Text = "";
                statusBiomeTemp.Text = "";
                statusBiomeDownfall.Text = "";

                statusBiome.Visible = false;
                statusBiomeTemp.Visible = false;
                statusBiomeDownfall.Visible = false;

                _validSelection = false;
            }

            UpdateImage();
        }
        
        public static Color Lighten(Color inColor, double inAmount)
        {
            return Color.FromArgb(
                inColor.A,
                (int)Math.Min(255, inColor.R + 255 * inAmount),
                (int)Math.Min(255, inColor.G + 255 * inAmount),
                (int)Math.Min(255, inColor.B + 255 * inAmount));
        }

        public static Color Darken(Color inColor, double inAmount)
        {
            return Color.FromArgb(
                inColor.A,
                (int)Math.Max(0, inColor.R - 255 * inAmount),
                (int)Math.Max(0, inColor.G - 255 * inAmount),
                (int)Math.Max(0, inColor.B - 255 * inAmount));
        }

        private void ShowProgress(int total)
        {
            Invoke(new MethodInvoker(delegate
            {
                progress.Value = 0;
                progressStatus.Text = "";

                progress.Visible = true;
                progressCancel.Visible = true;
                progressStatus.Visible = true;

                toolStripButton1.Enabled = false;
                
                progress.Minimum = 0;
                progress.Maximum = total;
            }));
        }

        private void HideProgress()
        {
            Invoke(new MethodInvoker(delegate
            {
                progress.Visible = false;
                progressCancel.Visible = false;
                progressStatus.Visible = false;

                toolStripButton1.Enabled = true;
            }));
        }

        private void UpdateProgress(int current, string prefix)
        {
            Invoke(new MethodInvoker(delegate
            {
                progress.Value = Math.Min(progress.Maximum, current);

                var pct = Math.Floor((progress.Value / (float)progress.Maximum) * 100);
                progressStatus.Text = $"{prefix} ({progress.Value}/{progress.Maximum}) - {pct}%";
            }));
        }

        private void GenerateChunks(int rX, int rZ)
        {
            var tot = rX * rZ;
            ThreadPool.QueueUserWorkItem(delegate
            {
                Monitor.Enter(_busy);

                try
                {
                    Busy();

                    _cancelProgress = false;
                    ShowProgress(tot);
                    var i = 0;

                    var hX = (int) Math.Floor(rX / 2f);
                    var hZ = (int) Math.Floor(rZ / 2f);

                    var chunks = _validChunks.ToArray();

                    for (int x = -hX; x <= hX; x++)
                    {
                        for (int z = -hZ; z <= hZ; z++)
                        {
                            if (_cancelProgress)
                            {
                                HideProgress();
                                return;
                            }

                            var co = new ChunkCoordinates(x, z);

                            if (!chunks.Contains(co))
                            {
                                UpdateProgress(i, "Generating Chunks");
                                _level.GenerateChunk(co);
                            }

                            i++;
                        }
                    }
                }
                finally
                {
                    Monitor.Exit(_busy);
                    HideProgress();
                    Busy(false);
                }
            });
        }

        private void Busy(bool busy = true)
        {
            _isBusy = busy;

            Invoke(new MethodInvoker(delegate
            {
                statusGeneric.Text = _isBusy ? "Working..." : "Ready";
            }));
        }
        
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            GenerateChunks(int.Parse(chunkGenX.Text), int.Parse(chunkGenZ.Text));
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var text = toolStripComboBox1.Items[toolStripComboBox1.SelectedIndex].ToString();
            //Debug.WriteLine(text + " - " + toolStripComboBox1.SelectedIndex + " - " + toolStripComboBox1.Items[toolStripComboBox1.SelectedIndex]);
            if (!text.Contains('x')) return;

            var split = text.Split(new[] { 'x' }, StringSplitOptions.RemoveEmptyEntries);
            var x = split[0].Trim();
            var z = split[1].Trim();

            chunkGenX.Text = x;
            chunkGenZ.Text = z;
        }

        private void progressCancel_Click(object sender, EventArgs e)
        {
            _cancelProgress = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // teleport all players there
            var y = _level.GetHeight(new BlockCoordinates(_currentPoint.X, 0, _currentPoint.Y)) + 2;
            var loc = new PlayerLocation(_currentPoint.X, y, _currentPoint.Y);

            foreach (var player in _level.Players.Values.ToArray())
            {
                player.Teleport(loc);
            }
        }

        enum DisplayMode : byte
        {
            BiomeGrass = 0,
            BiomeFoilage = 1,
            Rainfall = 2,
            Temperature = 3
        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedIndex = toolStripComboBox2.SelectedIndex;
            Mode = (DisplayMode) selectedIndex;
            UpdateLevel();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateLevel();
        }

        private void keepOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopMost = !TopMost;
            keepOnTopToolStripMenuItem.Checked = TopMost;
        }
    }
}
