using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security;
using System.Windows.Forms;

namespace DevTools.WinForms.Metro.Framework.Shadows
{
    public class MetroGlowShadow : MetroShadowBase
    {

        public Color GlowColor
        {
            get { return Themes.Theme.WindowGlowColor; }
        }

        public MetroGlowShadow(Form targetForm)
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
            return (Bitmap)DrawOutsetShadow(0, 0, 40, 1, GlowColor, new Rectangle(1, 1, ClientRectangle.Width, ClientRectangle.Height));
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
}