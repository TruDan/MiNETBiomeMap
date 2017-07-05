using System.Windows.Forms;

namespace DevTools.WinForms.Metro.Framework.Shadows
{
    public class MetroAeroDropShadow : MetroShadowBase
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
}