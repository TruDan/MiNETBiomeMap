using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using log4net;
using MiNET.Utils;
using MiNET.Worlds;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using SharpDX.Windows;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.WIC.Bitmap;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Timer = System.Threading.Timer;

namespace MiNETDevTools.Graphics.Biomes
{
    public class BiomeMapControl : GraphicControlBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BiomeMapControl));
        
        public Level Level { get; }

        private readonly BiomeUtils _biomeUtils;

        private Size2 _imageSize;

        private ChunkCoordinates _minChunk = ChunkCoordinates.Zero;
        private ChunkCoordinates _maxChunk = ChunkCoordinates.Zero;

        private int _lastChunkCount;
        
        private readonly object _sync = new object();
        private readonly object _chunkColoursSync = new object();
        
        private Matrix3x2 _scaleMatrix;
        private float _scale;

        private bool _isDirty, _isCacheDirty;
        private Timer _updateTimer;
        
        public BiomeMapControl(Level level) : base(null)
        {
            Level = level;
            _biomeUtils = new BiomeUtils();
            _biomeUtils.PrecomputeBiomeColors();
        }

        public void UpdateLevel()
        {

            var chunks = Level.GetLoadedChunks();
            if (chunks.Length == 0) return;
                
            int minX = _minChunk.X, maxX = _maxChunk.X, minZ = _minChunk.Z, maxZ = _maxChunk.Z;
            
            foreach (var chunk in chunks)
            {
                var c = new ChunkCoordinates(chunk.x, chunk.z);

                if (chunk.x < minX) minX = chunk.x;
                if (chunk.z < minZ) minZ = chunk.z;

                if (chunk.x > maxX) maxX = chunk.x;
                if (chunk.z > maxZ) maxZ = chunk.z;

                lock (_renderSync)
                {
                    if (_chunkCache.ContainsKey(c) && !_isCacheDirty)
                    {
                        continue;
                    }
                }
                    
                var bitmap = ProcessChunk(chunk);

                lock (_renderSync)
                {
                    _imageSize = new Size2((maxX - minX + 1) * 16, (maxZ - minZ + 1) * 16);
                    _minChunk = new ChunkCoordinates(minX, minZ);
                    _maxChunk = new ChunkCoordinates(maxX, maxZ);
                    _isDirty = true;

                    _chunkCache[c] = bitmap;
                    Log.InfoFormat("Cached chunk {0},{1} ({2})", chunk.x, chunk.z, _chunkCache.Count);
                }
            }
            _isCacheDirty = false;
                
        }

        private readonly object _renderSync = new object();

        private byte[] _bitmapMemory;

        private void RenderBitmap(GraphicsContext graphics)
        {
            Log.InfoFormat("Rendering Bitmap");

            int xOffset = 0, zOffset = 0;
            Size2 size = Size2.Empty;
            ChunkCacheBitmap[] cache;

            lock (_renderSync)
            {
                _isDirty = false;
                xOffset = Math.Min(0, _minChunk.X) * -1;
                zOffset = Math.Min(0, _minChunk.Z) * -1;
                size = _imageSize;
                cache = _chunkCache.Values.ToArray();
            }
            

            var arrSize = size.Width * size.Height * 4;
            if (_bitmap == null || _bitmapMemory == null || _bitmapMemory.Length != arrSize)
            {
                _bitmapMemory = new byte[arrSize];
                _bitmap = new SharpDX.Direct2D1.Bitmap(graphics.Target2D, size, new BitmapProperties(graphics.Target2D.PixelFormat));
                
            }
            _scale = Math.Min(graphics.ViewportSize.Width / (float)size.Width,
                graphics.ViewportSize.Height / (float)size.Height);

            _scaleMatrix = Matrix3x2.Scaling(_scale);

            if (cache.Length == 0) return;

            foreach (var chunk in cache)
            {
                for (int z = 0; z < 16; z++)
                {
                    Array.Copy(chunk.BitmapMemory, z * 16 * 4, _bitmapMemory, (size.Width * ((chunk.Z + zOffset) * 16 + z) * 4) + ((chunk.X + xOffset) * 16) * 4, 16 * 4);
                }
                //chunk.BitmapMemory.CopyTo(_bitmapMemory, (int)((_imageSize.Width * ((chunk.Z + _zOffset) * 16)) + ((chunk.X + _xOffset)* 16))*4);
            }

            _bitmap.CopyFromMemory(_bitmapMemory, size.Width * 4);
            Log.InfoFormat("Rendering Bitmap Complete | Size: {0}", _bitmapMemory.Length);
        }
        
        private ChunkCacheBitmap ProcessChunk(ChunkColumn chunk)
        {
            var items = new List<ChunkCacheBlockItem>();

            byte[] bitmapMemory = new byte[16 * 16 * 4];

            for (int z = 0; z < 16; z++)
            {
                for (int x = 0; x < 16; x++)
                {
                    //var y = chunk.GetHeight(x, z);
                    var y = GetHeightFix(chunk, x, z);
                    var biomeId = chunk.GetBiome(x, z);

                    var i = (int) ((16 * z * 4) + (4 * x));

                    var biomeColor = GetBiomeColor(biomeId);

                    //var c = SharpDX.Color.FromRgba(((255 << 24) & 0xFF) | ((y << 16) & 0xFF) | ((y << 8) & 0xFF) | (y & 0xFF));
                    var c = new SharpDX.Color(biomeColor.R, biomeColor.G, biomeColor.B, (byte)255);
                    //c = SharpDX.Color.Scale(c, ((y / 256f) * 0.6f) + 0.4f);

                    c *= (y / 255f);
                    //c = SharpDX.Color.Clamp(c, _minColor, _maxColor); 

                    bitmapMemory[i] = c.B;
                    bitmapMemory[i + 1] = c.G;
                    bitmapMemory[i + 2] = c.R;
                    bitmapMemory[i + 3] = 255;
                }
            }

            return new ChunkCacheBitmap
            {
                X = chunk.x,
                Z = chunk.z,
                BitmapMemory = bitmapMemory
            };


            var v = items.GroupBy(ks => ks.BiomeId).Select(vs => new ChunkCacheItem()
            {
                BiomeId = vs.Key,
                Items = vs.ToArray().ToArray(),
                X = chunk.x,
                Z = chunk.z
            });
        }

        private byte GetHeightFix(ChunkColumn column, int x, int z)
        {
            for(byte y = 255; y > 0; y--)
            {
                if (column.GetBlock(x, y, z) > 0)
                {
                    return y;
                }
            }
            return 0;
        }
        
        
        private IDictionary<ChunkCoordinates, ChunkCacheBitmap> _chunkCache =
            new Dictionary<ChunkCoordinates, ChunkCacheBitmap>();

        private struct ChunkCacheBitmap
        {
            public int X { get; set; }
            public int Z { get; set; }
            public byte[] BitmapMemory { get; set; }
        }

        private struct ChunkCacheBlockItem
        {
            public int X { get; set; }
            public int Z { get; set; }
            public int Height { get; set; }
            public int BiomeId { get; set; }
        }

        private struct ChunkCacheItem
        {
            public int X { get; set; }
            public int Z { get; set; }
            public int BiomeId { get; set; }
            public ChunkCacheBlockItem[] Items { get; set; }
        }

        private Color GetBiomeColor(byte biomeId)
        {
            var biome = _biomeUtils.GetBiome(biomeId);
            var c = biome.Grass;

            //if (Mode == DisplayMode.BiomeFoilage)
            {
             //   c = biome.Foliage;
            }

            int r = (int)((c >> 16) & 0xff);
            int g = (int)((c >> 8) & 0xff);
            int b = (int)c & 0xff;

            //Debug.WriteLine("Biome {0}: {1} {2}", biomeId, biome.Grass, biome.Foliage);

            return Color.FromArgb(r, g, b);
        }
        
        private void Update(object state)
        {
            if (!Monitor.TryEnter(_sync)) return;
            try
            {
                //Console.WriteLine("Update Level");

                UpdateLevel();
                //Console.WriteLine("Draw Size: " + _chunkColours.Count);
            }
            finally
            {
                Monitor.Exit(_sync);
            }
        }

        public void InitGraphics(GraphicsContext graphics)
        {
            _brush = new SolidColorBrush(graphics.Target2D, SharpDX.Color.DodgerBlue);

            RenderBitmap(graphics);
            _isDirty = true;
            _isCacheDirty = true;

            if (_updateTimer == null)
            {
                _updateTimer = new Timer(Update, null, 250, 250);
            }
        }

        private SharpDX.Direct2D1.Bitmap _bitmap;
        private SolidColorBrush _brush;

        public void Draw(GraphicsContext graphics)
        {
            if (_isDirty)
            {
                RenderBitmap(graphics);
            }

            var t = graphics.Target2D.Transform;
            
            //graphics.Target.DrawRectangle(new RectangleF(0, 0, graphics.ViewportSize.Width, graphics.ViewportSize.Height), _brush);

            graphics.Target2D.Transform = _scaleMatrix;
            //graphics.Target2D.Transform = Matrix3x2.Add(t, _scaleMatrix);
            //_blockGeometry = new RectangleGeometry(graphics.Target.Factory, new RectangleF(0, 0, 1 * _scale, 1 * _scale));
            //foreach (var kvp in ChunkColours)
            //{
            graphics.Target2D.DrawBitmap(_bitmap, 1f, BitmapInterpolationMode.NearestNeighbor);
            //_renderTarget.DrawLine(kvp.Position, kvp.Position, kvp.Brush);
            //graphics.Target.FillRectangle(kvp.Rect, kvp.Brush);
            //}
            graphics.Target2D.Transform = t;
        }

        public void DisposeResources(GraphicsContext graphics)
        {
            _updateTimer?.Change(Timeout.Infinite, Timeout.Infinite);
            _updateTimer = null;
            _bitmap?.Dispose();
            _bitmap = null;
            _brush?.Dispose();
            _bitmapMemory = null;
        }

        enum DisplayMode : byte
        {
            BiomeGrass = 0,
            BiomeFoilage = 1,
            Rainfall = 2,
            Temperature = 3
        }
    }
}
