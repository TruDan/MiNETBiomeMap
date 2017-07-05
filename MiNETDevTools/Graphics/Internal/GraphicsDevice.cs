using System;
using System.Numerics;
using System.Threading;
using System.Windows.Forms;
using MiNETDevTools.Graphics.Components;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device = SharpDX.Direct3D11.Device;
using DeviceContext = SharpDX.Direct2D1.DeviceContext;
using Factory = SharpDX.DXGI.Factory;
using FeatureLevel = SharpDX.Direct3D.FeatureLevel;
using Filter = SharpDX.Direct3D11.Filter;
using Resource = SharpDX.Direct3D11.Resource;
using Vector2 = SharpDX.Vector2;

namespace MiNETDevTools.Graphics.Internal
{
    public class GraphicsDevice : DisposeWrapper
    {
        public event Action<GraphicsDevice> OnInitialize;

        public bool FPSCounter { get; set; } = true;
        public bool VSync { get; set; }

        public SharpDX.Direct3D11.Device1 D3Device { get { return _d3dDevice; } }
        public SharpDX.Direct3D11.DeviceContext1 D3Context { get { return _d3dContext; } }

        public SharpDX.Direct2D1.Device D2Device { get { return _d2dDevice; } }
        public DeviceContext D2Context { get { return _d2dContext; } }
        public SharpDX.Direct2D1.Factory D2Factory { get { return _d2dFactory; } }

        public Size2 WindowSize { get; private set; }
        public Vector2 WindowCenter => new Vector2(WindowSize.Width/2f, WindowSize.Height/2f);

        public Bitmap1 Target2D { get { return _target; } }

        public bool IsRunning { get; private set; }

        public SharpDX.DirectWrite.Factory DwFactory { get { return _dwFactory; } }
        
        public Rectangle RenderTargetBounds { get; private set; }

        protected SharpDX.DirectWrite.Factory _dwFactory;

        private IntPtr _outputHandle { get; }
        private Control _dummyControl;

        protected SharpDX.Direct3D11.Device1 _d3dDevice;
        protected SharpDX.Direct3D11.DeviceContext1 _d3dContext;

        protected SharpDX.Direct2D1.Factory1 _d2dFactory;
        protected SharpDX.Direct2D1.Device _d2dDevice;
        protected DeviceContext _d2dContext;

        private SwapChain1 _swapChain;

        private Texture2D _backBuffer;
        private DepthStencilView _depthStencilView;
        private RenderTargetView _renderTargetView;
        private Surface _surface;

        private FpsCounter _fpsCounter;

        private Bitmap1 _target;

        internal GraphicsDevice(Control control)
        {
            _outputHandle = control.Handle;
            WindowSize = new Size2(control.Width, control.Height);
    }
        /*
        protected SwapChainDescription CreateSwapChainDescription()
        {
            return new SwapChainDescription()
            {
                ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), Format.B8G8R8A8_UNorm),
                OutputHandle = _outputHandle,
                IsWindowed = true,
                //Width = WindowSize.Width,
                //Height = WindowSize.Height,
                //Format = Format.R8G8B8A8_UNorm,
                //Stereo = false,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = 2,
                SwapEffect = SwapEffect.Discard,
                //Scaling = SharpDX.DXGI.Scaling.Stretch,
                Flags = SwapChainFlags.AllowModeSwitch,
            };
        }*/

        protected SwapChainDescription1 CreateSwapChainDescription()
        {
            var desc = new SwapChainDescription1()
            {
                // Automatic sizing
                Width = WindowSize.Width,
                Height = WindowSize.Height,
                Format = Format.B8G8R8A8_UNorm,
                Stereo = false,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.BackBuffer | Usage.RenderTargetOutput,
                // Use two buffers to enable flip effect.
                BufferCount = 2,
                Scaling = Scaling.Stretch,
                //Scaling = SharpDX.DXGI.Scaling.None,
                SwapEffect = SwapEffect.Discard,
            };
            return desc;
        }

        internal void Initialise()
        {
            RemoveAndDispose(ref _d2dFactory);
            RemoveAndDispose(ref _dwFactory);

            _d2dFactory = ToDispose(new SharpDX.Direct2D1.Factory1(FactoryType.SingleThreaded));
            _dwFactory = ToDispose(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared));
            
            RemoveAndDispose(ref _d3dDevice);
            RemoveAndDispose(ref _d3dContext);
            RemoveAndDispose(ref _d2dDevice);
            RemoveAndDispose(ref _d2dContext);
            
            
            //var desc = CreateSwapChainDescription();

            using (var device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport)) // | DeviceCreationFlags.Debug,
            {
                _d3dDevice = ToDispose(device.QueryInterface<SharpDX.Direct3D11.Device1>());
            }

            _d3dContext = ToDispose(_d3dDevice.ImmediateContext.QueryInterface<SharpDX.Direct3D11.DeviceContext1>());



            using (var dxgiDevice = _d3dDevice.QueryInterface<SharpDX.DXGI.Device>())
            {
                _d2dDevice = ToDispose(new SharpDX.Direct2D1.Device(_d2dFactory, dxgiDevice));
            }
            _d2dContext = ToDispose(new DeviceContext(_d2dDevice, DeviceContextOptions.None));
            /*
            D2Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, new [] { FeatureLevel.Level_10_0 }, desc, out _device, out _swapChain);

            ToDispose(_device);
            ToDispose(_swapChain);

            var d2dFactory = ToDispose(new SharpDX.Direct2D1.Factory());

            Factory factory = ToDispose(_swapChain.GetParent<Factory>());
            factory.MakeWindowAssociation(_outputHandle, WindowAssociationFlags.IgnoreAll);

            _backBuffer = ToDispose(Texture2D.FromSwapChain<Texture2D>(_swapChain, 0));
            _renderTargetView = ToDispose(new RenderTargetView(_device, _backBuffer));
            _surface = ToDispose(_backBuffer.QueryInterface<Surface>());

            _target = ToDispose(new RenderTarget(d2dFactory, _surface, new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied))));
            */

            InitialiseResources();

            RemoveAndDispose(ref _fpsCounter);

            _fpsCounter = new FpsCounter();
            _fpsCounter.InitialiseGraphics(this);

            OnInitialize?.Invoke(this);
        }

        protected void InitialiseResources()
        {
            RemoveAndDispose(ref _backBuffer);
            RemoveAndDispose(ref _renderTargetView);
            RemoveAndDispose(ref _surface);
            RemoveAndDispose(ref _target);
            _d2dContext.Target = null;

            var desc = CreateSwapChainDescription();

            if (_swapChain != null)
            {
                _swapChain.ResizeBuffers(
                    _swapChain.Description.BufferCount,
                    WindowSize.Width,
                    WindowSize.Height,
                    _swapChain.Description.ModeDescription.Format,
                    _swapChain.Description.Flags);
            }
            else
            {
                using (var dxgiDevice2 = _d3dDevice.QueryInterface<SharpDX.DXGI.Device2>())
                using (var dxgiAdapter = dxgiDevice2.Adapter)
                using (var dxgiFactory2 = dxgiAdapter.GetParent<SharpDX.DXGI.Factory2>())
                {
                    _swapChain = ToDispose(new SwapChain1(dxgiFactory2, _d3dDevice, _outputHandle, ref desc));
                    //_swapChain = ToDispose(new SwapChain1(dxgiFactory2, _d3dDevice, _outputHandle, ref desc));
                }
            }

            _backBuffer = ToDispose(Resource.FromSwapChain<Texture2D>(_swapChain, 0));
            {
                // Create a view interface on the rendertarget to use on bind.
                _renderTargetView = new RenderTargetView(_d3dDevice, _backBuffer);

                // Cache the rendertarget dimensions in our helper class for convenient use.
                var backBufferDesc = _backBuffer.Description;
                RenderTargetBounds = new Rectangle(0, 0, backBufferDesc.Width, backBufferDesc.Height);
            }

            using (var depthBuffer = new Texture2D(_d3dDevice,
                new Texture2DDescription()
                {
                    Format = Format.D24_UNorm_S8_UInt,
                    ArraySize = 1,
                    MipLevels = 1,
                    Width = (int)WindowSize.Width,
                    Height = (int)WindowSize.Height,
                    SampleDescription = new SampleDescription(1, 0),
                    BindFlags = BindFlags.DepthStencil,
                }))
                _depthStencilView = new DepthStencilView(_d3dDevice, depthBuffer, new DepthStencilViewDescription() { Dimension = DepthStencilViewDimension.Texture2D });

            var viewport = new ViewportF((float)RenderTargetBounds.X, (float)RenderTargetBounds.Y, (float)RenderTargetBounds.Width, (float)RenderTargetBounds.Height, 0.0f, 1.0f);

            _d3dContext.Rasterizer.SetViewport(viewport);

            var bitmapProperties = new BitmapProperties1(
                new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
                _d2dFactory.DesktopDpi.Width,
                _d2dFactory.DesktopDpi.Height,
                BitmapOptions.Target | BitmapOptions.CannotDraw);


            using (var dxgiBackBuffer = _swapChain.GetBackBuffer<Surface>(0))
                _target = new Bitmap1(_d2dContext, dxgiBackBuffer, bitmapProperties);

            _d2dContext.Target = _target;
            _d2dContext.TextAntialiasMode = TextAntialiasMode.Grayscale;

            /*_backBuffer = ToDispose(Resource.FromSwapChain<Texture2D>(_swapChain, 0));
            _renderTargetView = ToDispose(new RenderTargetView(_d3dDevice, _backBuffer));
            _surface = ToDispose(_backBuffer.QueryInterface<Surface>());

            using (var dxgiBackBuffer = _swapChain.GetBackBuffer<Surface>(0))
                _target = ToDispose(new RenderTarget(_d2dFactory, dxgiBackBuffer, new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied))));
            */
            OnInitialize?.Invoke(this);
        }
        
        public void Resize(Size2 newSize)
        {
            if (WindowSize.Width == newSize.Width && WindowSize.Height == newSize.Height) return;


            WindowSize = newSize;
            InitialiseResources();
        }


        internal void Present()
        {
            if (FPSCounter)
            {
                
                _fpsCounter.DrawFrame(this);
            }

            try
            {
                _swapChain.Present(VSync ? 1 : 0, PresentFlags.None);
            }
            catch (SharpDXException ex)
            {
                if (ex.ResultCode == SharpDX.DXGI.ResultCode.DeviceRemoved
                    || ex.ResultCode == SharpDX.DXGI.ResultCode.DeviceReset)
                {
                    Initialise();
                }
                else
                    throw;
            }
        }

    }
}
