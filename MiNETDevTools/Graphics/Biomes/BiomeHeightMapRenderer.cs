using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using MiNET.Worlds;
using MiNETDevTools.Graphics.Internal;
using MiNETDevToolsPlugin.Models;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using WinApi.DxUtils.Component;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;

namespace MiNETDevTools.Graphics.Biomes
{
    class BiomeHeightMapRenderer : IBiomeMapRenderer
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BiomeHeightMapRenderer));
        
        private static readonly Size2 Size = new Size2(Width, Height);

        public const int Width = 16;
        public const int Height = 16;

        public int X { get; private set; }
        public int Y { get; private set; }

        public byte[] BitmapCache { get; set; }

        private ChunkData _chunk;
        private RectangleF _bounds;
        private Bitmap1 _bitmap;
        private BiomeMapComponent _component;

        private DateTime _lastUpdate = DateTime.MinValue;

        private readonly object _renderSync = new object();
        private bool _isDirty = false;

        internal BiomeHeightMapRenderer(BiomeMapComponent component, ChunkData chunk)
        {
            _component = component;
            SetData(chunk);
            BitmapCache = new byte[16 * 16 * 4];
        }

        private void SetData(ChunkData data)
        {
            _chunk = data;
            X = _chunk.X;
            Y = _chunk.Z;
            _bounds = new RectangleF(X * Width, Y * Height, Width, Height);
        }

        public void Update(ChunkData data)
        {
            if (_lastUpdate == DateTime.MinValue || (DateTime.UtcNow - _lastUpdate).TotalMilliseconds > 2500)
            {
                SetData(data);
                foreach (var column in _chunk.ColumnInfo)
                {

                    var biomeColor = _component.GetBiomeColor(column.BiomeId);

                    //var c = SharpDX.Color.FromRgba(((255 << 24) & 0xFF) | ((y << 16) & 0xFF) | ((y << 8) & 0xFF) | (y & 0xFF));
                    var c = new Color(biomeColor.R, biomeColor.G, biomeColor.B, (byte) 255);
                    c *= (column.Height / 255f);

                    var i = (int) ((16 * column.Z * 4) + (4 * column.X));
                    lock (_renderSync)
                    {
                        BitmapCache[i] = c.B;
                        BitmapCache[i + 1] = c.G;
                        BitmapCache[i + 2] = c.R;
                        BitmapCache[i + 3] = 255;
                        _isDirty = true;
                    }
                }
                _lastUpdate = DateTime.UtcNow;
            }
        }

        protected void UpdateBitmap()
        {
            lock (_renderSync)
            {
                _isDirty = false;
                _bitmap.CopyFromMemory(BitmapCache, 16 * 4);
                //Log.InfoFormat("Bitmap Updated.");
            }
        }

        public void Render(DeviceContext context)
        {
            if (_bitmap == null)
            {
                _bitmap = new Bitmap1(context, Size, new BitmapProperties1(context.PixelFormat, context.DotsPerInch.Width, context.DotsPerInch.Height));
                UpdateBitmap();
            }
            
            lock (_renderSync)
            {
                if (_isDirty)
                {
                    UpdateBitmap();
                }
            }

            context.DrawBitmap(_bitmap, _bounds, 1.0f, BitmapInterpolationMode.NearestNeighbor);
            //context.D2Context.DrawBitmap(_bitmap, _bounds, 1.0f);
        }

        public void Dispose()
        {
            _bitmap?.Dispose();
            _component?.Dispose();
        }
    }
}
