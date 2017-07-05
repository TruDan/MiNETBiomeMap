using System;
using System.Threading;
using System.Windows.Forms;
using log4net;
using NetCoreEx.Geometry;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using SharpDX.Windows;
using WinApi.DxUtils.Component;
using WinApi.DxUtils.Composition;
using WinApi.DxUtils.D2D1;
using WinApi.Windows;

namespace MiNETDevTools.Graphics.Components
{
    public abstract class ViewportComponent : UserControl, IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ViewportComponent));
        
        public Matrix3x2 Transform { get; set; } = Matrix3x2.Identity;
        
        private System.ComponentModel.IContainer components = null;
        private readonly Dx11Component m_dx = new Dx11Component();

        private bool _sizeChanged;
        private Thread _thread;

        protected ViewportComponent()
        {
            this.components = new System.ComponentModel.Container();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (!DesignMode)
            {
                _thread = new Thread(StartRender);
                _thread.Start();
            }
        }

        protected void StartRender()
        {
            this.m_dx.Initialize(this.Handle, new Size(this.ClientSize.Width, this.ClientSize.Height));
            RenderLoop.Run(this, Render);
        }

        Random rand = new Random();

        protected void Render()
        {
            if (_sizeChanged)
            {
                _sizeChanged = false;
                this.m_dx.Resize(new Size(this.ClientSize.Width, this.ClientSize.Height));
            }

            this.m_dx.EnsureInitialized();
            try
            {
                //var context = this.m_dx.D2D.Context;
                var dx = m_dx.D2D.Context;
                dx.BeginDraw();
                dx.Clear(new RawColor4(0, 0, 0, 255f));
                
                dx.Transform = Transform;

                OnPaintView(dx);
                
                dx.EndDraw();
                
                this.m_dx.D3D.SwapChain.Present(1, 0);
                //this.Validate();
            }
            catch (SharpDXException ex)
            {
                Log.InfoFormat("Exception during render {0}x{1}: {2}", ClientSize.Width, ClientSize.Height, ex.Message);
                if (!this.m_dx.PerformResetOnException(ex)) throw;
            }
        }

        protected abstract void OnPaintView(DeviceContext component);

        protected override void OnClientSizeChanged(EventArgs e)
        {
            _sizeChanged = true;
            Log.InfoFormat("Resize {0}x{1}", ClientSize.Width, ClientSize.Height);
            base.OnClientSizeChanged(e);
        }
        
        protected override void Dispose(bool disposing)
        {
            _thread?.Abort();
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (!DesignMode)
                this.m_dx.Dispose();
            base.Dispose(disposing);
        }

        public void AttachCamera(ViewportCameraComponent cameraComponent)
        {
            this.components.Add(cameraComponent);
        }
        
    }
    /*
    public event Action CameraPositionChanged;
        public event Action ViewportSizeChanged;
        public event Action CameraZoomChanged;

        private ViewportF _cameraBounds;
        



        public Vector2 CameraPosition
        {
            get { return _cameraPosition; }
            private set {
                _cameraPosition = value;
                CameraPositionChanged?.Invoke();
            }
        }

        public Size2 ViewportSize
        {
            get { return _viewportSize; }
            private set {
                _viewportSize = value;
                ViewportSizeChanged?.Invoke();
            }
        }

        public double CameraZoom
        {
            get { return _cameraZoom; }
            private set
            {
                _cameraZoom = Math.Max(MinZoom, Math.Min(MaxZoom, value < MinZoom ? 100d / value : value));
                CameraZoomChanged?.Invoke();
            }
        }
        
        public float[] ZoomSteps = {0.25f, 0.5f, 0.75f, 1f, 1.5f, 2f, 2.5f, 3f, 4f, 5f, 7.5f, 10f};

        private ToolStripButton tSrefresh;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton tSinteractiveToggle;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton toolStripButton7;
        private ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem zoomInToolStripMenuItem;
        private ToolStripMenuItem zoomOutToolStripMenuItem;
        private ToolStripButton toolStripButton1;
        private ToolStrip toolStrip2;
        private ToolStripLabel sScursorPosition;
        private ToolStripLabel sScontentDimensions;
        private ToolStripComboBox sSzoomLevel;
        private ToolStripProgressBar toolStripProgressBar1;
        private ToolStripLabel toolStripLabel1;
        private Matrix3x2 _cameraTransformMatrix;

        public ViewportComponent()
        {
            InitializeComponent();
            this.sSzoomLevel.Items.AddRange(ZoomSteps.Select(f => f.ToString("P")).ToArray());
            this.sSzoomLevel.Sorted = true;

            this.sSzoomLevel.SelectedIndexChanged += (sender, args) =>
            {
                string val = this.sSzoomLevel.Items[this.sSzoomLevel.SelectedIndex].ToString();

                decimal dVal;
                if (decimal.TryParse(val.TrimEnd(new char[] {'%', ' '}), out dVal))
                {
                    CameraZoom = (double) (dVal / 100M);
                }
            };

            this.zoomInToolStripMenuItem.Click += (sender, args) => ZoomCamera(0.5f);

            this.CameraZoomChanged += () =>
            {
                this.zoomInToolStripMenuItem.Enabled = CameraZoom > MinZoom;
                this.zoomOutToolStripMenuItem.Enabled = CameraZoom < MaxZoom;
                this.tSzoomIn.Enabled = this.zoomInToolStripMenuItem.Enabled;
                this.tSzoomOut.Enabled = this.zoomOutToolStripMenuItem.Enabled;

                this.sSzoomLevel.SelectedItem = this.GetClosestZoomStep(CameraZoom);
            };
            this.ViewportSizeChanged += () =>
            {
                this.sScontentDimensions.Text = $"{ViewportSize.Width} x {ViewportSize.Height}";
            };

            _cameraBounds = new ViewportF(0, 0, 0, 0);
            CameraPosition = Vector2.Zero;
            ViewportSize = Size2.Zero;
            CameraZoom = 1.0f;
        }

        #region Control Events

        private ToolStrip toolStrip1;
        private ToolStripButton tSzoomOut;
        private ToolStripButton tSzoomIn;
        private ToolStripButton tSZoom100;
        private ToolStripButton tSzoomFit;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tSinspect;
        private ToolStripContainer toolStripContainer1;
        private RenderControl renderControl;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewportComponent));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tSrefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tSzoomOut = new System.Windows.Forms.ToolStripButton();
            this.tSzoomIn = new System.Windows.Forms.ToolStripButton();
            this.tSZoom100 = new System.Windows.Forms.ToolStripButton();
            this.tSzoomFit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tSinspect = new System.Windows.Forms.ToolStripButton();
            this.tSinteractiveToggle = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.renderControl = new SharpDX.Windows.RenderControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.sSzoomLevel = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.sScontentDimensions = new System.Windows.Forms.ToolStripLabel();
            this.sScursorPosition = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSrefresh,
            this.toolStripSeparator3,
            this.tSzoomOut,
            this.tSzoomIn,
            this.tSZoom100,
            this.tSzoomFit,
            this.toolStripSeparator1,
            this.tSinspect,
            this.tSinteractiveToggle,
            this.toolStripSeparator2,
            this.toolStripButton7,
            this.toolStripButton1});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(1);
            this.toolStrip1.Size = new System.Drawing.Size(640, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tSrefresh
            // 
            this.tSrefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSrefresh.Image = ((System.Drawing.Image)(resources.GetObject("tSrefresh.Image")));
            this.tSrefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSrefresh.Name = "tSrefresh";
            this.tSrefresh.Size = new System.Drawing.Size(23, 20);
            this.tSrefresh.Text = "toolStripButton8";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tSzoomOut
            // 
            this.tSzoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSzoomOut.Enabled = false;
            this.tSzoomOut.Image = ((System.Drawing.Image)(resources.GetObject("tSzoomOut.Image")));
            this.tSzoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSzoomOut.Name = "tSzoomOut";
            this.tSzoomOut.Size = new System.Drawing.Size(23, 22);
            this.tSzoomOut.Text = "toolStripButton1";
            // 
            // tSzoomIn
            // 
            this.tSzoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSzoomIn.Enabled = false;
            this.tSzoomIn.Image = ((System.Drawing.Image)(resources.GetObject("tSzoomIn.Image")));
            this.tSzoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSzoomIn.Name = "tSzoomIn";
            this.tSzoomIn.Size = new System.Drawing.Size(23, 22);
            this.tSzoomIn.Text = "toolStripButton2";
            // 
            // tSZoom100
            // 
            this.tSZoom100.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSZoom100.Enabled = false;
            this.tSZoom100.Image = ((System.Drawing.Image)(resources.GetObject("tSZoom100.Image")));
            this.tSZoom100.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSZoom100.Name = "tSZoom100";
            this.tSZoom100.Size = new System.Drawing.Size(23, 22);
            this.tSZoom100.Text = "toolStripButton3";
            // 
            // tSzoomFit
            // 
            this.tSzoomFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSzoomFit.Enabled = false;
            this.tSzoomFit.Image = ((System.Drawing.Image)(resources.GetObject("tSzoomFit.Image")));
            this.tSzoomFit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSzoomFit.Name = "tSzoomFit";
            this.tSzoomFit.Size = new System.Drawing.Size(23, 22);
            this.tSzoomFit.Text = "toolStripButton4";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tSinspect
            // 
            this.tSinspect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSinspect.Enabled = false;
            this.tSinspect.Image = ((System.Drawing.Image)(resources.GetObject("tSinspect.Image")));
            this.tSinspect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSinspect.Name = "tSinspect";
            this.tSinspect.Size = new System.Drawing.Size(23, 22);
            this.tSinspect.Text = "toolStripButton5";
            // 
            // tSinteractiveToggle
            // 
            this.tSinteractiveToggle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSinteractiveToggle.Image = ((System.Drawing.Image)(resources.GetObject("tSinteractiveToggle.Image")));
            this.tSinteractiveToggle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSinteractiveToggle.Name = "tSinteractiveToggle";
            this.tSinteractiveToggle.Size = new System.Drawing.Size(23, 22);
            this.tSinteractiveToggle.Text = "toolStripButton6";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton7.Image")));
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton7.Text = "toolStripButton7";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.toolStrip2);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.renderControl);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(640, 430);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(640, 480);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // renderControl
            // 
            this.renderControl.AutoSize = true;
            this.renderControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.renderControl.ContextMenuStrip = this.contextMenuStrip1;
            this.renderControl.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.renderControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderControl.Location = new System.Drawing.Point(0, 0);
            this.renderControl.Name = "renderControl";
            this.renderControl.Size = new System.Drawing.Size(640, 430);
            this.renderControl.TabIndex = 0;
            this.renderControl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.renderControl_KeyPress);
            this.renderControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.renderControl_MouseDoubleClick);
            this.renderControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.renderControl_MouseDown);
            this.renderControl.MouseEnter += new System.EventHandler(this.renderControl_MouseEnter);
            this.renderControl.MouseLeave += new System.EventHandler(this.renderControl_MouseLeave);
            this.renderControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.renderControl_MouseMove);
            this.renderControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.renderControl_MouseUp);
            this.renderControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.renderControl_MouseWheel);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 70);
            // 
            // zoomInToolStripMenuItem
            // 
            this.zoomInToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("zoomInToolStripMenuItem.Image")));
            this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
            this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.zoomInToolStripMenuItem.Text = "Zoom In";
            this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
            // 
            // zoomOutToolStripMenuItem
            // 
            this.zoomOutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("zoomOutToolStripMenuItem.Image")));
            this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
            this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.zoomOutToolStripMenuItem.Text = "Zoom Out";
            this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip2.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sScursorPosition,
            this.sScontentDimensions,
            this.sSzoomLevel,
            this.toolStripProgressBar1,
            this.toolStripLabel1});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Padding = new System.Windows.Forms.Padding(1);
            this.toolStrip2.Size = new System.Drawing.Size(640, 25);
            this.toolStrip2.Stretch = true;
            this.toolStrip2.TabIndex = 0;
            // 
            // sSzoomLevel
            // 
            this.sSzoomLevel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.sSzoomLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sSzoomLevel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sSzoomLevel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.sSzoomLevel.Items.AddRange(new object[] {
            "25%",
            "50%",
            "75%",
            "100%",
            "150%",
            "200%"});
            this.sSzoomLevel.MaxDropDownItems = 5;
            this.sSzoomLevel.Name = "sSzoomLevel";
            this.sSzoomLevel.Size = new System.Drawing.Size(75, 28);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.toolStripProgressBar1.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 23);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(38, 25);
            this.toolStripLabel1.Text = "Ready";
            // 
            // sScontentDimensions
            // 
            this.sScontentDimensions.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.sScontentDimensions.Image = ((System.Drawing.Image)(resources.GetObject("sScontentDimensions.Image")));
            this.sScontentDimensions.Margin = new System.Windows.Forms.Padding(0);
            this.sScontentDimensions.Name = "sScontentDimensions";
            this.sScontentDimensions.Size = new System.Drawing.Size(46, 25);
            this.sScontentDimensions.Text = "0 x 0";
            // 
            // sScursorPosition
            // 
            this.sScursorPosition.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.sScursorPosition.Image = ((System.Drawing.Image)(resources.GetObject("sScursorPosition.Image")));
            this.sScursorPosition.Margin = new System.Windows.Forms.Padding(0);
            this.sScursorPosition.Name = "sScursorPosition";
            this.sScursorPosition.Size = new System.Drawing.Size(44, 25);
            this.sScursorPosition.Text = "0 , 0";
            // 
            // ViewportComponent
            // 
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "ViewportComponent";
            this.Size = new System.Drawing.Size(640, 480);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected void MoveCamera(Vector2 offset)
        {
            CameraPosition += offset;
            UpdateCamera();
        }
        protected void MoveCamera(int x, int y)
        { 
            MoveCamera(new Point(-x, -y));
        }

        protected void SetCameraPosition(int x, int y)
        {
            CameraPosition = new Vector2(x, y);
            UpdateCamera();
        }

        protected void ZoomCamera(double offsetZoom)
        {
            CameraZoom += offsetZoom;
            UpdateCamera();
        }

        protected void ZoomCameraTo(double targetZoom)
        {
            CameraZoom = targetZoom;
            UpdateCamera();
        }

        protected void UpdateCamera()
        {
            //var mousePos = PointToClient(MousePosition);
            //_cameraTransformMatrix = Matrix3x2.Add(Matrix3x2.Scaling((float)CameraZoom, (float)CameraZoom, Vector2.Clamp(new Vector2(Width-mousePos.X, Height-mousePos.Y), new Vector2(0, 0), new Vector2(Width, Height))), Matrix3x2.Translation(CameraPosition));
            //_cameraTransformMatrix = Matrix3x2.Transformation((float)CameraZoom, (float)CameraZoom, 0, CameraPosition.X, CameraPosition.Y);

            var translate = Matrix3x2.Translation(CameraPosition);
            //var scale = Matrix3x2.Scaling((float) CameraZoom, (float) CameraZoom, new Vector2(Width/2f, Height/2f));

            _cameraTransformMatrix = translate * scale;
            
            var cameraMax = Matrix3x2.TransformPoint(_cameraTransformMatrix, new Vector2(ViewportSize.Width, ViewportSize.Height));
            var cameraPos = Matrix3x2.TransformPoint(_cameraTransformMatrix, CameraPosition);

            _cameraBounds = new ViewportF(cameraPos.X, cameraPos.Y, cameraMax.X, cameraMax.Y);
            //var inverse = Matrix3x2.Invert(_cameraTransformMatrix);

            //Matrix3x2.TransformPoint(ref inverse, ref cameraPos, out cameraMin);
            //Matrix3x2.TransformPoint(ref inverse, ref cameraPos, out cameraMax);
            //_cameraBounds = new Rectangle((int)cameraMin.X, (int)cameraMin.Y, (int)(cameraMax.X - cameraMin.X), (int)(cameraMax.Y - cameraMin.Y));
        }

        public void InitialiseGraphics(GraphicsDevice device)
        {
            ViewportSize = device.WindowSize;

        }

        public void CreateResources(GraphicsDevice device)
        {

        }

        public void DrawFrame(GraphicsDevice device)
        {

            //device.D2Context.Transform = _cameraTransformMatrix;
            RenderView(device, _cameraBounds);
        }

        protected virtual void OnInitialiseGraphics(GraphicsDevice device)
        {
            
        }

        protected abstract void RenderView(GraphicsDevice device, ViewportF viewBounds);

        private void renderControl_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {

            }
            else
            {
                MoveCamera(0, e.OldValue - e.NewValue);
            }
        }

        private void renderControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void ZoomStepIn()
        {
            var cur = CameraZoom;
            double target = CameraZoom;

            foreach (var v in ZoomSteps)
            {
                target = v;
                
                if (target > cur)
                {
                    break;
                }
            }
            
            ZoomCameraTo(target);
        }
        private void ZoomStepOut()
        {
            var cur = CameraZoom;
            double target = CameraZoom;

            for(var i = ZoomSteps.Length-1; i >= 0; i--)
            {
                target = ZoomSteps[i];

                if (target < cur)
                {
                    break;
                }
            }

            ZoomCameraTo(target);
        }

        private float GetClosestZoomStep(double zoomDelta)
        {
            return ZoomSteps.OrderByDescending(i => Math.Abs(zoomDelta - i)).FirstOrDefault();
        }

        private void renderControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                ZoomStepIn();
            }
            else if (e.Delta < 0)
            {
                ZoomStepOut();
            }
        }

        private bool _mouseDown;

        private Point _lastPoint;

        private void renderControl_MouseEnter(object sender, EventArgs e)
        {
            // set cursor to drag handle
        }

        private void renderControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (DesignMode) return;
            if (_mouseDown) return;

            _mouseDown = true;
            _lastPoint = new Point(e.Location.X, e.Location.Y);
            Cursor = Cursors.SizeAll;
            //Cursor = new Cursor(ResourceManager.GetStream("PanCursor"));
        }

        private void renderControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (DesignMode) return;
            var newPoint = new Point(e.Location.X, e.Location.Y);

            var offset = new Point(newPoint.X - _lastPoint.X, newPoint.Y -_lastPoint.Y);

            this.sScursorPosition.Text = $"{(newPoint.X - renderControl.Left) / CameraZoom:##,###} , {(newPoint.Y - renderControl.Top) / CameraZoom:##,###}";
            if (!_mouseDown) return;

            if (offset.X == 0 && offset.Y == 0) return;
            
            _lastPoint = newPoint;
            MoveCamera(offset);
        }

        private void renderControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (DesignMode) return;
            _mouseDown = false;
            Cursor = Cursors.Default;
        }

        private void renderControl_MouseLeave(object sender, EventArgs e)
        {
            if (DesignMode) return;
            _mouseDown = false;
        }

        private void renderControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (DesignMode) return;

        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

        }

        private GraphicsDevice _graphics;

        private Size2 _newSize = Size2.Empty;
        private bool _sizeChanged = false;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (DesignMode) return;
            if (_graphics == null)
            {
                _graphics = new GraphicsDevice(renderControl);
                _graphics.OnInitialize += InitialiseGraphics;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (DesignMode) return;
            var newSize = new Size2(Width, Height);

            if ((newSize.Width == 0 && newSize.Height != 0) ||
                (newSize.Width != 0 && newSize.Height == 0))
                return;

            _newSize = newSize;
            _sizeChanged = true;
        }

        protected override void DestroyHandle()
        {
            base.DestroyHandle();
            if (DesignMode) return;
            Stop();
            Dispose();
            _graphics = null;
        }


        private Control _dummyControl;
        private Thread _thread;
        private Vector2 _cameraPosition;
        private Size2 _viewportSize;
        private double _cameraZoom;
        private double _maxZoom = 25d;
        private double _minZoom = 0.25d;
        private bool _zoomEnabled;
        private bool _cameraDragabble;
        public bool IsRunning { get; private set; }

        public void Start()
        {
            if (DesignMode) return;
            IsRunning = true;
            _dummyControl = new Control(this, "");
            _thread = new Thread(RunInternal);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
        }


        [STAThread]
        private void RunInternal()
        {
            _graphics.Initialise();
            RenderLoop.Run(_dummyControl, () =>
            {
                if (_sizeChanged)
                {
                    _sizeChanged = false;
                    _graphics.Resize(_newSize);
                }

                DrawFrame(_graphics);

                _graphics.Present();
            });
        }

        public void Pause()
        {
            IsRunning = false;

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;
            Start();
        }
        
        public void Stop()
        {
            if (DesignMode) return;
            IsRunning = false;
            _dummyControl?.Dispose();
            _thread = null;
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomStepIn();
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomStepOut();
        }
    }*/
}
