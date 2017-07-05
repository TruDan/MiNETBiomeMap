using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security;
using System.Windows.Forms;

namespace DevTools.WinForms.Metro.Framework.Shadows
{
    public class MetroFlatDropShadow : MetroShadowBase
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
}