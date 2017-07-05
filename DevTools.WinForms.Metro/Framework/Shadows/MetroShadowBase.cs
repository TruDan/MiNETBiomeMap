using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevTools.WinForms.Metro.Framework.Shadows
{

    [DesignerCategory("")]
    public abstract class MetroShadowBase : Form
    {
        protected Form TargetForm { get; private set; }

        private readonly int shadowSize;
        private readonly int wsExStyle;

        protected MetroShadowBase(Form targetForm, int shadowSize, int wsExStyle)
        {
            TargetForm = targetForm;
            this.shadowSize = shadowSize;
            this.wsExStyle = wsExStyle;

            TargetForm.Activated += OnTargetFormActivated;
            TargetForm.ResizeBegin += OnTargetFormResizeBegin;
            TargetForm.ResizeEnd += OnTargetFormResizeEnd;
            TargetForm.VisibleChanged += OnTargetFormVisibleChanged;
            TargetForm.SizeChanged += OnTargetFormSizeChanged;

            TargetForm.Move += OnTargetFormMove;
            TargetForm.Resize += OnTargetFormResize;

            if (TargetForm.Owner != null)
                Owner = TargetForm.Owner;

            TargetForm.Owner = this;

            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ShowIcon = false;
            FormBorderStyle = FormBorderStyle.None;

            Bounds = GetShadowBounds();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= wsExStyle;
                return cp;
            }
        }

        private Rectangle GetShadowBounds()
        {
            Rectangle r = TargetForm.Bounds;
            r.Inflate(shadowSize, shadowSize);
            return r;
        }

        protected abstract void PaintShadow();

        protected abstract void ClearShadow();

        #region Event Handlers

        private bool isBringingToFront;

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            isBringingToFront = true;
        }

        private void OnTargetFormActivated(object sender, EventArgs e)
        {
            if (Visible) Update();
            if (isBringingToFront)
            {
                Visible = true;
                isBringingToFront = false;
                return;
            }
            BringToFront();
        }

        private void OnTargetFormVisibleChanged(object sender, EventArgs e)
        {
            Visible = TargetForm.Visible && TargetForm.WindowState != FormWindowState.Minimized;
            Update();
        }

        private long lastResizedOn;

        private bool IsResizing { get { return lastResizedOn > 0; } }

        private void OnTargetFormResizeBegin(object sender, EventArgs e)
        {
            lastResizedOn = DateTime.Now.Ticks;
        }

        private void OnTargetFormMove(object sender, EventArgs e)
        {
            if (!TargetForm.Visible || TargetForm.WindowState != FormWindowState.Normal)
            {
                Visible = false;
            }
            else
            {
                Bounds = GetShadowBounds();
            }
        }

        private void OnTargetFormResize(object sender, EventArgs e)
        {
            ClearShadow();
        }

        private void OnTargetFormSizeChanged(object sender, EventArgs e)
        {
            Bounds = GetShadowBounds();

            if (IsResizing)
            {
                return;
            }

            PaintShadowIfVisible();
        }

        private void OnTargetFormResizeEnd(object sender, EventArgs e)
        {
            lastResizedOn = 0;
            PaintShadowIfVisible();
        }

        private void PaintShadowIfVisible()
        {
            if (TargetForm.Visible && TargetForm.WindowState != FormWindowState.Minimized)
                PaintShadow();
        }

        #endregion

        #region Constants

        protected const int WS_EX_TRANSPARENT = 0x20;
        protected const int WS_EX_LAYERED = 0x80000;
        protected const int WS_EX_NOACTIVATE = 0x8000000;

        private const int TICKS_PER_MS = 10000;
        private const long RESIZE_REDRAW_INTERVAL = 1000 * TICKS_PER_MS;

        #endregion
    }
}