using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX;
using Point = System.Drawing.Point;

namespace MiNETDevTools.Graphics.Components
{
    public partial class ViewportCameraComponent : UserControl
    {
        private bool _cameraDragabble = true;
        private bool _zoomEnabled = true;
        private double _minZoom = 0.25d;
        private double _maxZoom = 25d;

        private ViewportComponent _viewport;

        [DefaultValue(true)]
        [Category("Custom")]
        public bool CameraDragabble
        {
            get { return _cameraDragabble; }
            set { _cameraDragabble = value; }
        }

        [DefaultValue(true)]
        [Category("Custom")]
        public bool ZoomEnabled
        {
            get { return _zoomEnabled; }
            set { _zoomEnabled = value; }
        }

        [DefaultValue(0.25d)]
        [Category("Custom")]
        public double MinZoom
        {
            get { return _minZoom; }
            set { _minZoom = value; }
        }

        [DefaultValue(25d)]
        [Category("Custom")]
        public double MaxZoom
        {
            get { return _maxZoom; }
            set { _maxZoom = value; }
        }

        public Vector2 CameraPosition
        {
            get { return _cameraPosition; }
            private set
            {
                _cameraPosition = value;
                CameraPositionChanged?.Invoke();
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

        public event Action CameraZoomChanged;
        public event Action ViewportSizeChanged;
        public event Action CameraPositionChanged;

        private Matrix3x2 _cameraTransformMatrix;

        public ViewportCameraComponent()
        {
            InitializeComponent();
        }

        public ViewportCameraComponent(ViewportComponent viewport)
        {
            InitializeComponent();
            _viewport = viewport;

            _viewport.Dock = DockStyle.Fill;
            _viewport.Size = ClientSize;
            this.toolStripContainer1.ContentPanel.Controls.Add(_viewport);

            RegisterEvents(_viewport);
            RegisterEvents(this);
        }

        private void RegisterEvents(Control control)
        {
            control.MouseMove += OnMouseMove;
            control.MouseUp += OnMouseUp;
            control.MouseDown += OnMouseDown;
            control.MouseEnter += OnMouseEnter;
            control.MouseLeave += OnMouseLeave;
        }

        public bool CanExtend(object target)
        {
            return (target is ViewportComponent);
        }

        protected void MoveCamera(Vector2 offset)
        {
            CameraPosition += offset;
            UpdateCamera();
        }
        protected void MoveCamera(int x, int y)
        {
            MoveCamera(new Vector2(-x, -y));
        }

        protected void SetCameraPosition(int x, int y)
        {
            txtPos.Text = string.Format("{0} x {1}", x, y);
            CameraPosition = new Vector2(x, y);
            UpdateCamera();
        }

        protected void ZoomCamera(double offsetZoom)
        {
            CameraZoom += offsetZoom;
            txtZoom.Text = string.Format("{0:P}", CameraZoom);
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
            var scale = Matrix3x2.Scaling((float) CameraZoom, (float) CameraZoom, new Vector2(ClientRectangle.Width/2f, ClientRectangle.Height /2f));

            _cameraTransformMatrix = translate * scale;

            var cameraMax = Matrix3x2.TransformPoint(_cameraTransformMatrix, new Vector2(ClientRectangle.Width, ClientRectangle.Height));
            var cameraPos = Matrix3x2.TransformPoint(_cameraTransformMatrix, CameraPosition);

            //_cameraBounds = new ViewportF(cameraPos.X, cameraPos.Y, cameraMax.X, cameraMax.Y);
            //var inverse = Matrix3x2.Invert(_cameraTransformMatrix);

            //Matrix3x2.TransformPoint(ref inverse, ref cameraPos, out cameraMin);
            //Matrix3x2.TransformPoint(ref inverse, ref cameraPos, out cameraMax);
            //_cameraBounds = new Rectangle((int)cameraMin.X, (int)cameraMin.Y, (int)(cameraMax.X - cameraMin.X), (int)(cameraMax.Y - cameraMin.Y));

            _viewport.Transform = _cameraTransformMatrix;
        }

        #region Event Handlers 

        private bool _mouseDown;
        private Point _lastPoint = Point.Empty;
        private Vector2 _cameraPosition = Vector2.Zero;
        private double _cameraZoom = 1f;

        protected void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (DesignMode) return;
            var newPoint = new Point(e.Location.X, e.Location.Y);

            var offset = new Point(newPoint.X - _lastPoint.X, newPoint.Y - _lastPoint.Y);

            if (!_mouseDown) return;

            if (offset.X == 0 && offset.Y == 0) return;

            _lastPoint = newPoint;
            MoveCamera(offset.X, offset.Y);

            var n = PointToClient(newPoint);
            ;
            var o2 = CameraPosition - new Vector2(n.X, n.Y);

            txtPos.Text = string.Format("{0} , {1}", o2.X, o2.Y);
        }

        protected void OnMouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }

        protected void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (_mouseDown) return;
            _mouseDown = true;
            _lastPoint = new Point(e.Location.X, e.Location.Y);

        }
        
        protected void OnMouseEnter(object sender, EventArgs e)
        {
            var parent = this.Container as Control;
            if(parent == null) return;

            parent.Cursor = new Cursor(typeof(ViewportComponent), "cursor_pan.cur");
        }

        protected void OnMouseLeave(object sender, EventArgs e)
        {
            _mouseDown = false;
            var parent = this.Container as Control;
            if (parent == null) return;

            parent.Cursor = Cursors.Default;
        }

        #endregion

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            ZoomCamera(-0.25f);
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            ZoomCamera(0.25f);
        }
    }
}
