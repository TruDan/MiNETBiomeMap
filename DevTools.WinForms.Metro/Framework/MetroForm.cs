using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security;
using System.Windows.Forms;
using DevTools.WinForms.Metro.Framework.Native;
using DevTools.WinForms.Metro.Framework.Shadows;
using DevTools.WinForms.Metro.Framework.Themes;
using WeifenLuo.WinFormsUI.Docking;

namespace DevTools.WinForms.Metro.Framework
{
    [DesignTimeVisible]
    public partial class MetroForm : Form
    {

        #region Fields

        private MetroFormTextAlign textAlign = MetroFormTextAlign.Left;
        [Browsable(true)]
        [Category(MetroPropertyCategory.Appearance)]
        public MetroFormTextAlign TextAlign
        {
            get { return textAlign; }
            set { textAlign = value; }
        }

        [Browsable(false)]
        public override Color BackColor
        {
            get { return Theme.WindowBackgroundColor; }
        }

        private MetroFormBorderStyle formBorderStyle = MetroFormBorderStyle.None;
        [DefaultValue(MetroFormBorderStyle.None)]
        [Browsable(true)]
        [Category(MetroPropertyCategory.Appearance)]
        public MetroFormBorderStyle BorderStyle
        {
            get { return formBorderStyle; }
            set { formBorderStyle = value; }
        }

        private bool isMovable = true;
        [Category(MetroPropertyCategory.Appearance)]
        public bool Movable
        {
            get { return isMovable; }
            set { isMovable = value; }
        }

        public new Padding Padding
        {
            get { return base.Padding; }
            set
            {
                value.Top = Math.Max(value.Top, DisplayHeader ? 32 : BorderStyle == MetroFormBorderStyle.FixedSingle ? borderWidth : 0);
                value.Left = Math.Max(value.Left, BorderStyle == MetroFormBorderStyle.FixedSingle ? borderWidth : 0);
                value.Right = Math.Max(value.Left, BorderStyle == MetroFormBorderStyle.FixedSingle ? borderWidth : 0);
                value.Bottom = Math.Max(value.Left, BorderStyle == MetroFormBorderStyle.FixedSingle ? borderWidth : 0);
                base.Padding = value;
            }
        }

        protected override Padding DefaultPadding
        {
            get { return new Padding(BorderStyle == MetroFormBorderStyle.FixedSingle ? borderWidth : 0, DisplayHeader ? (backMaxSize + backImagePadding.Vertical + BorderStyle == MetroFormBorderStyle.FixedSingle ? borderWidth : 0) : BorderStyle == MetroFormBorderStyle.FixedSingle ? borderWidth : 0, BorderStyle == MetroFormBorderStyle.FixedSingle ? borderWidth : 0, BorderStyle == MetroFormBorderStyle.FixedSingle ? borderWidth : 0); }
        }

        private bool displayHeader = true;
        [Category(MetroPropertyCategory.Appearance)]
        [DefaultValue(true)]
        public bool DisplayHeader
        {
            get { return displayHeader; }
            set
            {
                if (value != displayHeader)
                {
                    Padding p = base.Padding;
                    p.Top += value ? (backMaxSize + backImagePadding.Vertical + BorderStyle == MetroFormBorderStyle.FixedSingle ? borderWidth : 0) : -(backMaxSize + backImagePadding.Vertical + BorderStyle == MetroFormBorderStyle.FixedSingle ? borderWidth : 0);
                    base.Padding = p;
                }
                displayHeader = value;
            }
        }

        private bool isResizable = true;
        [Category(MetroPropertyCategory.Appearance)]
        public bool Resizable
        {
            get { return isResizable; }
            set { isResizable = value; }
        }

        private MetroFormShadowType shadowType = MetroFormShadowType.Flat;
        [Category(MetroPropertyCategory.Appearance)]
        [DefaultValue(MetroFormShadowType.Flat)]
        public MetroFormShadowType ShadowType
        {
            get { return IsMdiChild ? MetroFormShadowType.None : shadowType; }
            set { shadowType = value; }
        }

        [Browsable(false)]
        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
            set { base.FormBorderStyle = value; }
        }

        public new Form MdiParent
        {
            get { return base.MdiParent; }
            set
            {
                if (value != null)
                {
                    RemoveShadow();
                    shadowType = MetroFormShadowType.None;
                }

                base.MdiParent = value;
            }
        }

        private Bitmap _image = null;
        private Image backImage;
        [Category(MetroPropertyCategory.Appearance)]
        [DefaultValue(null)]
        public Image BackImage
        {
            get { return backImage; }
            set
            {
                backImage = value;
                if (value != null) _image = ApplyInvert(new Bitmap(value));
                Refresh();
            }
        }

        private Padding backImagePadding;
        [Category(MetroPropertyCategory.Appearance)]
        public Padding BackImagePadding
        {
            get { return backImagePadding; }
            set
            {
                backImagePadding = value;
                Refresh();
            }
        }

        private int backMaxSize;
        [Category(MetroPropertyCategory.Appearance)]
        public int BackMaxSize
        {
            get { return backMaxSize; }
            set
            {
                backMaxSize = value;
                Refresh();
            }
        }

        private BackLocation backLocation;
        [Category(MetroPropertyCategory.Appearance)]
        [DefaultValue(BackLocation.TopLeft)]
        public BackLocation BackLocation
        {
            get { return backLocation; }
            set
            {
                backLocation = value;
                Refresh();
            }
        }

        private bool _imageinvert;
        [Category(MetroPropertyCategory.Appearance)]
        [DefaultValue(true)]
        public bool ApplyImageInvert
        {
            get { return _imageinvert; }
            set
            {
                _imageinvert = value;
                Refresh();
            }
        }

        private int borderWidth = 1;
        [Category(MetroPropertyCategory.Appearance)]
        [DefaultValue(1)]
        public int BorderWidth
        {
            get { return borderWidth; }
            set
            {
                borderWidth = value;
                Refresh();
            }
        }
        #endregion
        public MetroForm()
        {
            SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                     System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer |
                     System.Windows.Forms.ControlStyles.ResizeRedraw |
                     System.Windows.Forms.ControlStyles.UserPaint, true);
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                RemoveShadow();
            }
            base.Dispose(disposing);
        }

        public Bitmap ApplyInvert(Bitmap bitmapImage)
        {
            byte A, R, G, B;
            Color pixelColor;

            for (int y = 0; y < bitmapImage.Height; y++)
            {
                for (int x = 0; x < bitmapImage.Width; x++)
                {
                    pixelColor = bitmapImage.GetPixel(x, y);
                    A = pixelColor.A;
                    R = (byte)(255 - pixelColor.R);
                    G = (byte)(255 - pixelColor.G);
                    B = (byte)(255 - pixelColor.B);

                    if (R <= 0) R = 17;
                    if (G <= 0) G = 17;
                    if (B <= 0) B = 17;
                    //bitmapImage.SetPixel(x, y, Color.FromArgb((int)A, (int)R, (int)G, (int)B));
                    bitmapImage.SetPixel(x, y, Color.FromArgb((int)R, (int)G, (int)B));
                }
            }

            return bitmapImage;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Themes.Theme.WindowBackgroundColor);

            using (SolidBrush b = new SolidBrush(Themes.Theme.WindowBorderColor))
            {
                Rectangle topRect = new Rectangle(0, 0, Width, borderWidth);
                e.Graphics.FillRectangle(b, topRect);
            }

            if (BorderStyle != MetroFormBorderStyle.None)
            {
                using (Pen pen = new Pen(Themes.Theme.WindowBorderColor))
                {
                    e.Graphics.DrawLines(pen, new[]
                    {
                        new Point(0, borderWidth),
                        new Point(0, Height - 1),
                        new Point(Width - 1, Height - 1),
                        new Point(Width - 1, borderWidth)
                    });
                }
            }

            Rectangle titleBounds = new Rectangle();

            if (DisplayHeader)
            {
                titleBounds = new Rectangle(borderWidth, borderWidth,
                    ClientRectangle.Width - (2 * borderWidth), (int)Math.Ceiling(Font.SizeInPoints));
            }


            if (backImage != null && backMaxSize != 0)
            {
                Image img = MetroImage.ResizeImage(backImage, new Rectangle(0, 0, backMaxSize, backMaxSize));
                if (_imageinvert)
                {
                    img = MetroImage.ResizeImage(backImage, new Rectangle(0, 0, backMaxSize, backMaxSize));
                }

                int backImageX = 0, backImageY = 0;


                switch (backLocation)
                {
                    case BackLocation.TopLeft:
                        backImageX = borderWidth + backImagePadding.Left;
                        backImageY = borderWidth + backImagePadding.Top;

                        titleBounds.X += img.Width + backImageX + backImagePadding.Right;
                        titleBounds.Y += backImageY;
                        titleBounds.Width -= img.Width + backImagePadding.Left + backImagePadding.Right;
                        titleBounds.Height = Math.Max(titleBounds.Height, img.Height);
                        break;
                    case BackLocation.TopRight:
                        backImageX = -(backImagePadding.Right + img.Width + borderWidth);
                        backImageY = borderWidth + backImagePadding.Top;

                        titleBounds.X += backImageX - backImagePadding.Left;
                        titleBounds.Y += backImageY;
                        titleBounds.Width -= img.Width + backImagePadding.Left + backImagePadding.Right;
                        titleBounds.Height = Math.Max(titleBounds.Height, img.Height);
                        break;
                    case BackLocation.BottomLeft:
                        backImageX = borderWidth + backImagePadding.Left;
                        backImageY = -(img.Height + backImagePadding.Bottom + borderWidth);
                        break;
                    case BackLocation.BottomRight:
                        backImageX = -(backImagePadding.Right + img.Width + borderWidth);
                        backImageY = -(img.Height + backImagePadding.Bottom + borderWidth);
                        break;
                }
                e.Graphics.DrawImage(img, backImageX < 0 ? ClientRectangle.Right - backImageX : backImageX, backImageY < 0 ? ClientRectangle.Bottom - backImageY : backImageY);
            }

            if (DisplayHeader)
            {
                TextFormatFlags flags = TextFormatFlags.EndEllipsis | GetTextFormatFlags() | TextFormatFlags.VerticalCenter;
                TextRenderer.DrawText(e.Graphics, Text, Font, titleBounds, Themes.Theme.WindowTitleForegroundColor, flags);
            }

            if (Resizable && (SizeGripStyle == SizeGripStyle.Auto || SizeGripStyle == SizeGripStyle.Show))
            {
                using (SolidBrush b = new SolidBrush(Themes.Theme.WindowResizeGripColor))
                {
                    Size resizeHandleSize = new Size(2, 2);
                    e.Graphics.FillRectangles(b, new Rectangle[] {
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-10,ClientRectangle.Height-10), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-10,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-10), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-14,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-14), resizeHandleSize)
                    });
                }
            }
        }

        private TextFormatFlags GetTextFormatFlags()
        {
            switch (this.TextAlign)
            {
                case MetroFormTextAlign.Left:
                    return TextFormatFlags.Default;
                case MetroFormTextAlign.Center:
                    return TextFormatFlags.HorizontalCenter;
                case MetroFormTextAlign.Right:
                    return TextFormatFlags.Right;
                default:
                    throw new InvalidOperationException();
            }
        }

        #region Management Methods

        protected override void OnClosed(EventArgs e)
        {
            if (this.Owner != null) this.Owner = null;

            RemoveShadow();

            base.OnClosed(e);
        }

        [SecuritySafeCritical]
        public bool FocusMe()
        {
            return Native.WinApi.SetForegroundWindow(Handle);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!DesignMode)
            {

                switch (StartPosition)
                {
                    case FormStartPosition.CenterParent:
                        CenterToParent();
                        break;
                    case FormStartPosition.CenterScreen:
                        if (IsMdiChild)
                        {
                            CenterToParent();
                        }
                        else
                        {
                            CenterToScreen();
                        }
                        break;
                }
            }

            //RemoveCloseButton();

            // if (ControlBox)
            if (false)
            {
                AddWindowButton(WindowButtons.Close);

                if (MaximizeBox)
                    AddWindowButton(WindowButtons.Maximize);

                if (MinimizeBox)
                    AddWindowButton(WindowButtons.Minimize);

                UpdateWindowButtonPosition();
            }

            CreateShadow();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (shadowType == MetroFormShadowType.AeroShadow &&
                IsAeroThemeEnabled() && IsDropShadowSupported())
            {
                int val = 2;
                DwmApi.DwmSetWindowAttribute(Handle, 2, ref val, 4);
                var m = new DwmApi.MARGINS
                {
                    cyBottomHeight = 1,
                    cxLeftWidth = 0,
                    cxRightWidth = 0,
                    cyTopHeight = 0
                };

                DwmApi.DwmExtendFrameIntoClientArea(Handle, ref m);
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            Invalidate();
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            UpdateWindowButtonPosition();
        }

        protected override void WndProc(ref Message m)
        {
            if (DesignMode)
            {
                base.WndProc(ref m);
                return;
            }

            switch (m.Msg)
            {
                case (int)Native.WinApi.Messages.WM_SYSCOMMAND:
                    int sc = m.WParam.ToInt32() & 0xFFF0;
                    switch (sc)
                    {
                        case (int)Native.WinApi.Messages.SC_MOVE:
                            if (!Movable) return;
                            break;
                        case (int)Native.WinApi.Messages.SC_MAXIMIZE:
                            break;
                        case (int)Native.WinApi.Messages.SC_RESTORE:
                            break;
                    }
                    break;

                case (int)Native.WinApi.Messages.WM_NCLBUTTONDBLCLK:
                case (int)Native.WinApi.Messages.WM_LBUTTONDBLCLK:
                    if (!MaximizeBox) return;
                    break;

                case (int)Native.WinApi.Messages.WM_NCHITTEST:
                    Native.WinApi.HitTest ht = HitTestNCA(m.HWnd, m.WParam, m.LParam);
                    if (ht != Native.WinApi.HitTest.HTCLIENT)
                    {
                        m.Result = (IntPtr)ht;
                        return;
                    }
                    break;

                case (int)Native.WinApi.Messages.WM_DWMCOMPOSITIONCHANGED:
                    break;
            }

            base.WndProc(ref m);

            switch (m.Msg)
            {
                case (int)Native.WinApi.Messages.WM_GETMINMAXINFO:
                    OnGetMinMaxInfo(m.HWnd, m.LParam);
                    break;
                case (int)Native.WinApi.Messages.WM_SIZE:
                    if (windowButtonList != null)
                    {
                        MetroFormButton btn;
                        windowButtonList.TryGetValue(WindowButtons.Maximize, out btn);
                        if (btn == null) return;
                        if (WindowState == FormWindowState.Normal)
                        {
                            if (_shadowForm != null) _shadowForm.Visible = true;
                            btn.Text = "1";
                        }
                        if (WindowState == FormWindowState.Maximized) btn.Text = "2";
                    }
                    break;
            }
        }

        [SecuritySafeCritical]
        private unsafe void OnGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            Native.WinApi.MINMAXINFO* pmmi = (Native.WinApi.MINMAXINFO*)lParam;

            //YOROCA MDI PARENT
            Screen s = Screen.FromHandle(hwnd);
            //if (IsMdiChild)
            if (this.Parent != null)
            {
                pmmi->ptMaxSize.x = this.Parent.ClientRectangle.Size.Width;
                pmmi->ptMaxSize.y = this.Parent.ClientRectangle.Size.Height;
            }
            else
            {
                pmmi->ptMaxSize.x = s.WorkingArea.Width;
                pmmi->ptMaxSize.y = s.WorkingArea.Height;
            }
            pmmi->ptMaxPosition.x = Math.Abs(s.WorkingArea.Left - s.Bounds.Left);
            pmmi->ptMaxPosition.y = Math.Abs(s.WorkingArea.Top - s.Bounds.Top);

            //if (MinimumSize.Width > 0) pmmi->ptMinTrackSize.x = MinimumSize.Width;
            //if (MinimumSize.Height > 0) pmmi->ptMinTrackSize.y = MinimumSize.Height;
            //if (MaximumSize.Width > 0) pmmi->ptMaxTrackSize.x = MaximumSize.Width;
            //if (MaximumSize.Height > 0) pmmi->ptMaxTrackSize.y = MaximumSize.Height;
        }

        private Native.WinApi.HitTest HitTestNCA(IntPtr hwnd, IntPtr wparam, IntPtr lparam)
        {
            //Point vPoint = PointToClient(new Point((int)lparam & 0xFFFF, (int)lparam >> 16 & 0xFFFF));
            //Point vPoint = PointToClient(new Point((Int16)lparam, (Int16)((int)lparam >> 16)));
            Point vPoint = new Point((Int16)lparam, (Int16)((int)lparam >> 16));
            int vPadding = Math.Max(20, Math.Max(Padding.Right, Padding.Bottom));

            if (Resizable)
            {
                if (RectangleToScreen(new Rectangle(0, ClientRectangle.Height - vPadding, vPadding, vPadding)).Contains(vPoint))
                    return Native.WinApi.HitTest.HTBOTTOMLEFT;

                if (RectangleToScreen(new Rectangle(ClientRectangle.Width - vPadding, ClientRectangle.Height - vPadding, vPadding, vPadding)).Contains(vPoint))
                    return Native.WinApi.HitTest.HTBOTTOMRIGHT;

                if (RectangleToScreen(new Rectangle(0, ClientRectangle.Height - vPadding, ClientRectangle.Width, vPadding)).Contains(vPoint))
                    return Native.WinApi.HitTest.HTBOTTOM;

                if (RectangleToScreen(new Rectangle(0, 0, vPadding, ClientRectangle.Height)).Contains(vPoint))
                    return Native.WinApi.HitTest.HTLEFT;

                if (RectangleToScreen(new Rectangle(ClientRectangle.Width - vPadding, 0, vPadding, ClientRectangle.Height)).Contains(vPoint))
                    return Native.WinApi.HitTest.HTRIGHT;
            }

            if (RectangleToScreen(new Rectangle(borderWidth, borderWidth, ClientRectangle.Width - 2 * (BorderStyle == MetroFormBorderStyle.FixedSingle ? borderWidth : 0), (backMaxSize + backImagePadding.Vertical + BorderStyle == MetroFormBorderStyle.FixedSingle ? borderWidth : 0))).Contains(vPoint))
                return Native.WinApi.HitTest.HTCAPTION;

            return Native.WinApi.HitTest.HTCLIENT;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left && Movable)
            {
                if (WindowState == FormWindowState.Maximized) return;
                if (Width - borderWidth > e.Location.X && e.Location.X > borderWidth && e.Location.Y > borderWidth)
                {
                    MoveControl();
                }
            }

        }

        [SecuritySafeCritical]
        private void MoveControl()
        {
            Native.WinApi.ReleaseCapture();
            Native.WinApi.SendMessage(Handle, (int)Native.WinApi.Messages.WM_NCLBUTTONDOWN, (int)Native.WinApi.HitTest.HTCAPTION, 0);
        }

        [SecuritySafeCritical]
        private static bool IsAeroThemeEnabled()
        {
            if (Environment.OSVersion.Version.Major <= 5) return false;

            bool aeroEnabled;
            DwmApi.DwmIsCompositionEnabled(out aeroEnabled);
            return aeroEnabled;
        }

        private static bool IsDropShadowSupported()
        {
            return Environment.OSVersion.Version.Major > 5 && SystemInformation.IsDropShadowEnabled;
        }

        #endregion


        #region Window Buttons

        private enum WindowButtons
        {
            Minimize,
            Maximize,
            Close
        }

        private Dictionary<WindowButtons, MetroFormButton> windowButtonList;

        private void AddWindowButton(WindowButtons button)
        {
            if (windowButtonList == null)
                windowButtonList = new Dictionary<WindowButtons, MetroFormButton>();

            if (windowButtonList.ContainsKey(button))
                return;

            MetroFormButton newButton = new MetroFormButton();

            if (button == WindowButtons.Close)
            {
                newButton.Text = "r";
            }
            else if (button == WindowButtons.Minimize)
            {
                newButton.Text = "0";
            }
            else if (button == WindowButtons.Maximize)
            {
                if (WindowState == FormWindowState.Normal)
                    newButton.Text = "1";
                else
                    newButton.Text = "2";
            }

            newButton.BackColor = Theme.WindowBackgroundColor;
            newButton.ForeColor = Theme.WindowForegroundColor;
            newButton.Tag = button;
            newButton.Size = new Size(25, 20);
            newButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            newButton.TabStop = false; //remove the form controls from the tab stop
            newButton.Click += WindowButton_Click;
            Controls.Add(newButton);

            windowButtonList.Add(button, newButton);
        }

        private void WindowButton_Click(object sender, EventArgs e)
        {
            var btn = sender as MetroFormButton;
            if (btn != null)
            {
                var btnFlag = (WindowButtons)btn.Tag;
                if (btnFlag == WindowButtons.Close)
                {
                    Close();
                }
                else if (btnFlag == WindowButtons.Minimize)
                {
                    WindowState = FormWindowState.Minimized;
                }
                else if (btnFlag == WindowButtons.Maximize)
                {
                    if (WindowState == FormWindowState.Normal)
                    {
                        WindowState = FormWindowState.Maximized;
                        btn.Text = "2";
                    }
                    else
                    {
                        WindowState = FormWindowState.Normal;
                        btn.Text = "1";
                    }
                }
            }
        }

        private void UpdateWindowButtonPosition()
        {
            //if (DesignMode) return;
            if (!ControlBox) return;

            Dictionary<int, WindowButtons> priorityOrder = new Dictionary<int, WindowButtons>(3) { { 0, WindowButtons.Close }, { 1, WindowButtons.Maximize }, { 2, WindowButtons.Minimize } };

            Point firstButtonLocation = new Point(ClientRectangle.Width - borderWidth - 25, borderWidth);
            int lastDrawedButtonPosition = firstButtonLocation.X - 25;

            MetroFormButton firstButton = null;

            if (windowButtonList.Count == 1)
            {
                foreach (KeyValuePair<WindowButtons, MetroFormButton> button in windowButtonList)
                {
                    button.Value.Location = firstButtonLocation;
                }
            }
            else
            {
                foreach (KeyValuePair<int, WindowButtons> button in priorityOrder)
                {
                    bool buttonExists = windowButtonList.ContainsKey(button.Value);

                    if (firstButton == null && buttonExists)
                    {
                        firstButton = windowButtonList[button.Value];
                        firstButton.Location = firstButtonLocation;
                        continue;
                    }

                    if (firstButton == null || !buttonExists) continue;

                    windowButtonList[button.Value].Location = new Point(lastDrawedButtonPosition, borderWidth);
                    lastDrawedButtonPosition = lastDrawedButtonPosition - 25;
                }
            }

            Refresh();
        }

        #endregion

        #region Shadows

        private const int CS_DROPSHADOW = 0x20000;
        const int WS_MINIMIZEBOX = 0x20000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                if (ShadowType == MetroFormShadowType.SystemShadow)
                    cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private Form _shadowForm;
        private ThemeBase _theme = new CustomTheme();

        private void CreateShadow()
        {
            if (DesignMode) return;
            switch (ShadowType)
            {
                case MetroFormShadowType.Flat:
                    _shadowForm = new MetroFlatDropShadow(this);
                    return;

                case MetroFormShadowType.Glow:
                    _shadowForm = new MetroGlowShadow(this);
                    return;

                case MetroFormShadowType.DropShadow:
                    _shadowForm = new MetroRealisticDropShadow(this);
                    return;

                case MetroFormShadowType.None:
                    return;

                    //default:
                    //    shadowForm = new MetroFlatDropShadow(this);
                    //    return;
            }
        }
        private void RemoveShadow()
        {
            if (_shadowForm == null || _shadowForm.IsDisposed) return;

            _shadowForm.Visible = false;
            Owner = _shadowForm.Owner;
            _shadowForm.Owner = null;
            _shadowForm.Dispose();
            _shadowForm = null;
        }

        #endregion


        #region Helper Methods

        [SecuritySafeCritical]
        public void RemoveCloseButton()
        {
            IntPtr hMenu = Native.WinApi.GetSystemMenu(Handle, false);
            if (hMenu == IntPtr.Zero) return;

            int n = Native.WinApi.GetMenuItemCount(hMenu);
            if (n <= 0) return;

            Native.WinApi.RemoveMenu(hMenu, (uint)(n - 1), Native.WinApi.MfByposition | Native.WinApi.MfRemove);
            Native.WinApi.RemoveMenu(hMenu, (uint)(n - 2), Native.WinApi.MfByposition | Native.WinApi.MfRemove);
            Native.WinApi.DrawMenuBar(Handle);
        }

        private Rectangle MeasureText(System.Drawing.Graphics g, Rectangle clientRectangle, Font font, string text, TextFormatFlags flags)
        {
            var proposedSize = new Size(int.MaxValue, int.MinValue);
            var actualSize = TextRenderer.MeasureText(g, text, font, proposedSize, flags);
            return new Rectangle(clientRectangle.X, clientRectangle.Y, actualSize.Width, actualSize.Height);
        }

        #endregion

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
