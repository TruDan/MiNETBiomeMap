using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security;
using System.Windows.Forms;
using log4net;
using MiNETDevTools.UI.Framework.Native;
using MiNETDevTools.UI.Theme;
using WeifenLuo.WinFormsUI.Docking;
using WinApi = MiNETDevTools.UI.Framework.Native.WinApi;

namespace MiNETDevTools.UI.Framework
{
    public static class MetroPropertyCategory
    {
        public const string Appearance = "Metro Appearance";
        public const string Behaviour = "Metro Behaviour";
    }

    class MetroImage
    {
        public static Image ResizeImage(Image imgToResize, Rectangle maxOffset)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = (float)maxOffset.Width / sourceWidth;
            nPercentH = (float)maxOffset.Height / sourceHeight;

            nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            return imgToResize.GetThumbnailImage(destWidth, destHeight, null, IntPtr.Zero);
        }
    }
    #region Enums

    public enum MetroFormTextAlign
    {
        Left,
        Center,
        Right
    }

    public enum MetroFormShadowType
    {
        None,
        Flat,
        DropShadow,
        SystemShadow,
        AeroShadow
    }

    public enum MetroFormBorderStyle
    {
        None,
        FixedSingle
    }

    public enum BackLocation
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    #endregion
    public partial class CustomThemeForm : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CustomThemeForm));

        #region Fields

        public ThemeBase Theme { get; set; } = new CustomTheme();
        protected DockPanelColorPalette Palette => Theme.ColorPalette;
        protected IPaintingService Painting => Theme.PaintingService;

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
            get { return Palette.MainWindowActive.Background; }
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

        public CustomThemeForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            FormBorderStyle = FormBorderStyle.None;
            Name = "MetroForm";
            StartPosition = FormStartPosition.CenterScreen;
            TransparencyKey = Color.Lavender;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
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
            Color backColor = Palette.MainWindowActive.Background;
            Color foreColor = Color.FromArgb(0x7F999999);

            e.Graphics.Clear(backColor);

            using (SolidBrush b = new SolidBrush(Palette.MainWindowStatusBarDefault.Background))
            {
                Rectangle topRect = new Rectangle(0, 0, Width, borderWidth);
                e.Graphics.FillRectangle(b, topRect);
            }

            Color c = Palette.MainWindowStatusBarDefault.Background;

            if (BorderStyle != MetroFormBorderStyle.None)
            {
                using (Pen pen = new Pen(c))
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
                    ClientRectangle.Width - (2 * borderWidth), (int) Math.Ceiling(Font.SizeInPoints));
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
                TextRenderer.DrawText(e.Graphics, Text, Font, titleBounds, foreColor, flags);
            }

            if (Resizable && (SizeGripStyle == SizeGripStyle.Auto || SizeGripStyle == SizeGripStyle.Show))
            {
                using (SolidBrush b = new SolidBrush(Palette.CommandBarMenuDefault.Background))
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

            if (DesignMode) return;

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

            RemoveCloseButton();

            if (ControlBox)
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
                            if (shadowForm != null) shadowForm.Visible = true;
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

            newButton.BackColor = Palette.CommandBarMenuDefault.Background;
            newButton.ForeColor = Palette.CommandBarMenuDefault.Text;
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
        private class MetroFormButton : Button
        {
            #region Interface
            
            private bool useCustomBackColor = false;
            [Category(MetroPropertyCategory.Appearance)]
            [DefaultValue(false)]
            public bool UseCustomBackColor
            {
                get { return useCustomBackColor; }
                set { useCustomBackColor = value; }
            }

            private bool useCustomForeColor = false;
            [Category(MetroPropertyCategory.Appearance)]
            [DefaultValue(false)]
            public bool UseCustomForeColor
            {
                get { return useCustomForeColor; }
                set { useCustomForeColor = value; }
            }

            private bool useStyleColors = false;
            [Category(MetroPropertyCategory.Appearance)]
            [DefaultValue(false)]
            public bool UseStyleColors
            {
                get { return useStyleColors; }
                set { useStyleColors = value; }
            }

            [Browsable(false)]
            [DefaultValue(false)]
            public bool UseSelectable
            {
                get { return GetStyle(ControlStyles.Selectable); }
                set { SetStyle(ControlStyles.Selectable, value); }
            }

            #endregion

            #region Fields

            private bool isHovered = false;
            private bool isPressed = false;

            #endregion

            #region Constructor

            public MetroFormButton()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw |
                         ControlStyles.UserPaint, true);
            }

            #endregion

            public ThemeBase Theme { get; set; } = new CustomTheme();
            protected DockPanelColorPalette Palette => Theme.ColorPalette;
            protected IPaintingService Painting => Theme.PaintingService;
            #region Paint Methods

            protected override void OnPaint(PaintEventArgs e)
            {
                Color backColor, foreColor;
                
                if (Parent != null)
                {
                    backColor = Parent.BackColor;
                }
                else
                {
                    backColor = Palette.CommandBarMenuDefault.Background;
                }

                if (isHovered && !isPressed && Enabled)
                {
                    foreColor = Palette.CommandBarMenuTopLevelHeaderHovered.Text;
                    backColor = Palette.CommandBarMenuTopLevelHeaderHovered.Background;
                }
                else if (isHovered && isPressed && Enabled)
                {
                    foreColor = Palette.CommandBarToolbarButtonPressed.Text;
                    backColor = Palette.CommandBarToolbarButtonPressed.Background;
                }
                else if (!Enabled)
                {
                    foreColor = Palette.CommandBarMenuPopupDisabled.Text;
                    backColor = Palette.CommandBarMenuDefault.Background;
                }
                else
                {
                    foreColor = Palette.CommandBarMenuDefault.Text;
                }

                e.Graphics.Clear(backColor);
                Font buttonFont = new Font("Webdings", 9.25f);
                TextRenderer.DrawText(e.Graphics, Text, buttonFont, ClientRectangle, foreColor, backColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }

            #endregion

            #region Mouse Methods

            protected override void OnMouseEnter(EventArgs e)
            {
                isHovered = true;
                Invalidate();

                base.OnMouseEnter(e);
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    isPressed = true;
                    Invalidate();
                }

                base.OnMouseDown(e);
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
                isPressed = false;
                Invalidate();

                base.OnMouseUp(e);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                isHovered = false;
                Invalidate();

                base.OnMouseLeave(e);
            }

            #endregion
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

        private Form shadowForm;

        private void CreateShadow()
        {
            switch (ShadowType)
            {
                case MetroFormShadowType.Flat:
                    shadowForm = new MetroFlatDropShadow(this);
                    return;

                case MetroFormShadowType.DropShadow:
                    shadowForm = new MetroRealisticDropShadow(this);
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
            if (shadowForm == null || shadowForm.IsDisposed) return;

            shadowForm.Visible = false;
            Owner = shadowForm.Owner;
            shadowForm.Owner = null;
            shadowForm.Dispose();
            shadowForm = null;
        }

        #region MetroShadowBase

        protected abstract class MetroShadowBase : Form
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

        #endregion

        #region Aero DropShadow

        protected class MetroAeroDropShadow : MetroShadowBase
        {
            public MetroAeroDropShadow(Form targetForm)
                : base(targetForm, 0, WS_EX_TRANSPARENT | WS_EX_NOACTIVATE)
            {
                FormBorderStyle = FormBorderStyle.SizableToolWindow;
            }

            protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
            {
                if (specified == BoundsSpecified.Size) return;
                base.SetBoundsCore(x, y, width, height, specified);
            }

            protected override void PaintShadow() { Visible = true; }

            protected override void ClearShadow() { }

        }

        #endregion

        #region Flat DropShadow

        protected class MetroFlatDropShadow : MetroShadowBase
        {
            private Point Offset = new Point(-6, -6);

            public MetroFlatDropShadow(Form targetForm)
                : base(targetForm, 6, WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_NOACTIVATE)
            {
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);
                PaintShadow();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                Visible = true;
                PaintShadow();
            }

            protected override void PaintShadow()
            {
                using (Bitmap getShadow = DrawBlurBorder())
                    SetBitmap(getShadow, 255);
            }

            protected override void ClearShadow()
            {
                Bitmap img = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);
                g.Clear(Color.Transparent);
                g.Flush();
                g.Dispose();
                SetBitmap(img, 255);
                img.Dispose();
            }

            #region Drawing methods

            [SecuritySafeCritical]
            private void SetBitmap(Bitmap bitmap, byte opacity)
            {
                if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                    throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

                IntPtr screenDc = Native.WinApi.GetDC(IntPtr.Zero);
                IntPtr memDc = Native.WinApi.CreateCompatibleDC(screenDc);
                IntPtr hBitmap = IntPtr.Zero;
                IntPtr oldBitmap = IntPtr.Zero;

                try
                {
                    hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                    oldBitmap = Native.WinApi.SelectObject(memDc, hBitmap);

                    Native.WinApi.SIZE size = new Native.WinApi.SIZE(bitmap.Width, bitmap.Height);
                    Native.WinApi.POINT pointSource = new Native.WinApi.POINT(0, 0);
                    Native.WinApi.POINT topPos = new Native.WinApi.POINT(Left, Top);
                    Native.WinApi.BLENDFUNCTION blend = new Native.WinApi.BLENDFUNCTION();
                    blend.BlendOp = Native.WinApi.AC_SRC_OVER;
                    blend.BlendFlags = 0;
                    blend.SourceConstantAlpha = opacity;
                    blend.AlphaFormat = Native.WinApi.AC_SRC_ALPHA;

                    Native.WinApi.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, Native.WinApi.ULW_ALPHA);
                }
                finally
                {
                    Native.WinApi.ReleaseDC(IntPtr.Zero, screenDc);
                    if (hBitmap != IntPtr.Zero)
                    {
                        Native.WinApi.SelectObject(memDc, oldBitmap);
                        Native.WinApi.DeleteObject(hBitmap);
                    }
                    Native.WinApi.DeleteDC(memDc);
                }
            }

            private Bitmap DrawBlurBorder()
            {
                return (Bitmap)DrawOutsetShadow(Color.Black, new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height));
            }

            private Image DrawOutsetShadow(Color color, Rectangle shadowCanvasArea)
            {
                Rectangle rOuter = shadowCanvasArea;
                Rectangle rInner = new Rectangle(shadowCanvasArea.X + (-Offset.X - 1), shadowCanvasArea.Y + (-Offset.Y - 1), shadowCanvasArea.Width - (-Offset.X * 2 - 1), shadowCanvasArea.Height - (-Offset.Y * 2 - 1));

                Bitmap img = new Bitmap(rOuter.Width, rOuter.Height, PixelFormat.Format32bppArgb);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                using (Brush bgBrush = new SolidBrush(Color.FromArgb(30, Color.Black)))
                {
                    g.FillRectangle(bgBrush, rOuter);
                }
                using (Brush bgBrush = new SolidBrush(Color.FromArgb(60, Color.Black)))
                {
                    g.FillRectangle(bgBrush, rInner);
                }

                g.Flush();
                g.Dispose();

                return img;
            }

            #endregion
        }

        #endregion

        #region Realistic DropShadow

        protected class MetroRealisticDropShadow : MetroShadowBase
        {
            public MetroRealisticDropShadow(Form targetForm)
                : base(targetForm, 15, WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_NOACTIVATE)
            {
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);
                PaintShadow();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                Visible = true;
                PaintShadow();
            }

            protected override void PaintShadow()
            {
                using (Bitmap getShadow = DrawBlurBorder())
                    SetBitmap(getShadow, 255);
            }

            protected override void ClearShadow()
            {
                Bitmap img = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);
                g.Clear(Color.Transparent);
                g.Flush();
                g.Dispose();
                SetBitmap(img, 255);
                img.Dispose();
            }

            #region Drawing methods

            [SecuritySafeCritical]
            private void SetBitmap(Bitmap bitmap, byte opacity)
            {
                if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                    throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

                IntPtr screenDc = Native.WinApi.GetDC(IntPtr.Zero);
                IntPtr memDc = Native.WinApi.CreateCompatibleDC(screenDc);
                IntPtr hBitmap = IntPtr.Zero;
                IntPtr oldBitmap = IntPtr.Zero;

                try
                {
                    hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                    oldBitmap = Native.WinApi.SelectObject(memDc, hBitmap);

                    Native.WinApi.SIZE size = new Native.WinApi.SIZE(bitmap.Width, bitmap.Height);
                    Native.WinApi.POINT pointSource = new Native.WinApi.POINT(0, 0);
                    Native.WinApi.POINT topPos = new Native.WinApi.POINT(Left, Top);
                    Native.WinApi.BLENDFUNCTION blend = new Native.WinApi.BLENDFUNCTION
                    {
                        BlendOp = Native.WinApi.AC_SRC_OVER,
                        BlendFlags = 0,
                        SourceConstantAlpha = opacity,
                        AlphaFormat = Native.WinApi.AC_SRC_ALPHA
                    };

                    Native.WinApi.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, Native.WinApi.ULW_ALPHA);
                }
                finally
                {
                    Native.WinApi.ReleaseDC(IntPtr.Zero, screenDc);
                    if (hBitmap != IntPtr.Zero)
                    {
                        Native.WinApi.SelectObject(memDc, oldBitmap);
                        Native.WinApi.DeleteObject(hBitmap);
                    }
                    Native.WinApi.DeleteDC(memDc);
                }
            }

            private Bitmap DrawBlurBorder()
            {
                return (Bitmap)DrawOutsetShadow(0, 0, 40, 1, Color.Black, new Rectangle(1, 1, ClientRectangle.Width, ClientRectangle.Height));
            }

            private Image DrawOutsetShadow(int hShadow, int vShadow, int blur, int spread, Color color, Rectangle shadowCanvasArea)
            {
                Rectangle rOuter = shadowCanvasArea;
                Rectangle rInner = shadowCanvasArea;
                rInner.Offset(hShadow, vShadow);
                rInner.Inflate(-blur, -blur);
                rOuter.Inflate(spread, spread);
                rOuter.Offset(hShadow, vShadow);

                Rectangle originalOuter = rOuter;

                Bitmap img = new Bitmap(originalOuter.Width, originalOuter.Height, PixelFormat.Format32bppArgb);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                var currentBlur = 0;
                do
                {
                    var transparency = (rOuter.Height - rInner.Height) / (double)(blur * 2 + spread * 2);
                    var shadowColor = Color.FromArgb(((int)(200 * (transparency * transparency))), color);
                    var rOutput = rInner;
                    rOutput.Offset(-originalOuter.Left, -originalOuter.Top);

                    DrawRoundedRectangle(g, rOutput, currentBlur, Pens.Transparent, shadowColor);
                    rInner.Inflate(1, 1);
                    currentBlur = (int)((double)blur * (1 - (transparency * transparency)));

                } while (rOuter.Contains(rInner));

                g.Flush();
                g.Dispose();

                return img;
            }

            private void DrawRoundedRectangle(System.Drawing.Graphics g, Rectangle bounds, int cornerRadius, Pen drawPen, Color fillColor)
            {
                int strokeOffset = Convert.ToInt32(Math.Ceiling(drawPen.Width));
                bounds = Rectangle.Inflate(bounds, -strokeOffset, -strokeOffset);

                var gfxPath = new GraphicsPath();

                if (cornerRadius > 0)
                {
                    gfxPath.AddArc(bounds.X, bounds.Y, cornerRadius, cornerRadius, 180, 90);
                    gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y, cornerRadius, cornerRadius, 270, 90);
                    gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
                    gfxPath.AddArc(bounds.X, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
                }
                else
                {
                    gfxPath.AddRectangle(bounds);
                }

                gfxPath.CloseAllFigures();

                if (cornerRadius > 5)
                {
                    using (SolidBrush b = new SolidBrush(fillColor))
                    {
                        g.FillPath(b, gfxPath);
                    }
                }
                if (drawPen != Pens.Transparent)
                {
                    using (Pen p = new Pen(drawPen.Color))
                    {
                        p.EndCap = p.StartCap = LineCap.Round;
                        g.DrawPath(p, gfxPath);
                    }
                }
            }

            #endregion
        }

        #endregion

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
    }
}
