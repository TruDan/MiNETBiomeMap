using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;
using MiNETDevTools.Graphics.Components;
using MiNETDevTools.Graphics.Internal;
using MiNETDevTools.Plugin;
using MiNETDevToolsPlugin.Interfaces;
using MiNETDevToolsPlugin.Models;
using Newtonsoft.Json;
using SharpDX;
using SharpDX.Direct2D1;
using WinApi.DxUtils.Component;
using WinApi.Windows;
using Color = System.Drawing.Color;
using Point = SharpDX.Point;

namespace MiNETDevTools.Graphics.Biomes
{
    public class BiomeMapComponent : ViewportComponent
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BiomeMapComponent));

        IBiomeMapRenderer[] Chunks
        {
            get
            {
                lock (_renderSync)
                {
                    return _cachedChunks.Values.ToArray();
                }
            }
        }
        
        private Point _minChunk = Point.Zero;
        private Point _maxChunk = Point.Zero;
        
        private LevelData _level;
        private BiomeUtils _biomeUtils;

        private readonly object _renderSync = new object();
        private bool _fetchAll = true;

        public BiomeMapComponent()
        {
            _biomeUtils = new BiomeUtils();
            _biomeUtils.PrecomputeBiomeColors();
        }
        
        public void Init(LevelData level)
        {
            Name = level.Id;
            Text = level.Name;
            _level = level;
        }

        private Dictionary<ChunkCoordinates, IBiomeMapRenderer> _cachedChunks = new Dictionary<ChunkCoordinates, IBiomeMapRenderer>();
        
        internal void UpdateMap()
        {
            if (_level == null) return;

            using (var client = PluginClient.LevelService)
            {
                ChunkData[] chunks;
                if (_fetchAll)
                {
                    chunks = client.Proxy.FetchAllChunksForLevel(_level.Id);
                }
                else
                {
                    chunks = client.Proxy.FetchUpdatedChunksForLevel(_level.Id);
                }

                if (chunks == null || chunks.Length == 0) return;

                foreach (var chunk in chunks)
                {
                    InitChunk(chunk);
                }
            }
            
        }

        private void InitChunk(ChunkData chunk)
        {
            var k = new ChunkCoordinates(chunk.X, chunk.Z);

            lock (_renderSync)
            {
                IBiomeMapRenderer renderer;
                if (!_cachedChunks.TryGetValue(k, out renderer))
                {
                    renderer = new BiomeHeightMapRenderer(this, chunk);
                    _cachedChunks.Add(k, renderer);
                    Log.InfoFormat("Cached chunk {0},{1} ({2}) {3}", chunk.X, chunk.Z, Chunks.Length, JsonConvert.SerializeObject(chunk.ColumnInfo));
                }
                else
                {
                    renderer.Update(chunk);
                }
            }
        }

        internal Color GetBiomeColor(byte biomeId)
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

        protected override void OnPaintView(DeviceContext context)
        {
            if (_level == null) return;

            foreach (var chunk in Chunks)
            {
                chunk.Render(context);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }

    class BiomeMapChunk
    {
        public int X { get; }
        public int Y { get; }
        public byte[] BitmapCache { get; set; }
    }

    interface IBiomeMapRenderer : IDisposable
    {
        int X { get; }
        int Y { get; }
        void Update(ChunkData data);
        
        void Render(DeviceContext context);
    }
}
