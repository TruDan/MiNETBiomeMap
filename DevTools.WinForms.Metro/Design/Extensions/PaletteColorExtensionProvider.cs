using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevTools.WinForms.Metro.Design.Converters;
using DevTools.WinForms.Metro.Framework.Themes;

namespace DevTools.WinForms.Metro.Design.Extensions
{
    [ProvideProperty("ThemeForeColor", typeof(Control))]
    [ProvideProperty("ThemeBackColor", typeof(Control))]
    public class PaletteColorExtensionProvider : Component, IExtenderProvider
    {
        public PaletteColor GetThemeForeColor(Control control)
        {
            return PaletteColorConverter.ToPaletteColor(control.ForeColor);
        }

        public PaletteColor GetThemeBackColor(Control control)
        {
            return PaletteColorConverter.ToPaletteColor(control.BackColor);
        }

        public void SetThemeBackColor(Control control, PaletteColor value)
        {
            control.BackColor = value.Color;
        }

        public void SetThemeForeColor(Control control, PaletteColor value)
        {
            control.ForeColor = value.Color;
        }

        public bool ShouldSerializeThemeForeColor(Control control)
        {
            return false;
        }

        public bool ShouldSerializeThemeBackColor(Control control)
        {
            return false;
        }

        #region IExtenderProvider Members

        public bool CanExtend(object extendee)
        {
            return (extendee is Control);
        }

        #endregion
    }
}
