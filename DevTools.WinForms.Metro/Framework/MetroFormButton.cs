using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevTools.WinForms.Metro.Framework.Themes;
using WeifenLuo.WinFormsUI.Docking;

namespace DevTools.WinForms.Metro.Framework
{
    internal class MetroFormButton : Button
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

        private ThemeBase _theme = new CustomTheme();

        [DefaultValue(typeof(CustomTheme))]
        [Category(MetroPropertyCategory.Appearance)]
        public ThemeBase Theme
        {
            get { return _theme; }
            set { _theme = value; }
        }

        protected DockPanelColorPalette Palette => Theme.ColorPalette;
        protected IPaintingService Painting => Theme.PaintingService;

        #region Paint Methods

        protected override void OnPaint(PaintEventArgs pevent)
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

            pevent.Graphics.Clear(backColor);
            Font buttonFont = new Font("Webdings", 11f);
            TextRenderer.DrawText(pevent.Graphics, Text, buttonFont, ClientRectangle, foreColor, backColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        #endregion

        #region Mouse Methods

        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }

            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            isPressed = false;
            Invalidate();

            base.OnMouseUp(mevent);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isHovered = false;
            Invalidate();

            base.OnMouseLeave(e);
        }

        #endregion
    }
}