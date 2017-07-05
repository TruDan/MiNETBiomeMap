using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DevTools.WinForms.Metro.Framework.Themes;

namespace DevTools.WinForms.Metro.Framework.Themes
{
    public static class Theme
    {
        public static PaletteColor AccentColor { get; }


        public static PaletteColor WindowBackgroundColor { get; } = 0xFF2D2D30.ToColor();
        public static PaletteColor WindowForegroundColor { get; } = 0xF1F1F1.ToColor();
        public static PaletteColor WindowBorderColor { get; } = 0x007ACC.ToColor();
        public static PaletteColor WindowGlowColor { get; } = 0x007ACC.ToColor();
        public static PaletteColor WindowTitleBackroundColor { get; } = Color.Transparent.ToColor();
        public static PaletteColor WindowTitleForegroundColor { get; } = 0x999999.ToColor();
        public static PaletteColor WindowResizeGripColor { get; } = 0x7FBCE5.ToColor();
        public static PaletteColor WindowResizeGripShadowColor { get; } = 0x005C99.ToColor();

    }

    internal static class ThemeUtils
    {
        internal static PaletteColor ToColor(this uint hex, [CallerMemberName] string name = "")
        {
            return new PaletteColor((int)hex, name);
        }

        internal static PaletteColor ToColor(this int hex, [CallerMemberName] string name = "")
        {
            return new PaletteColor(hex, name);
        }

        internal static PaletteColor ToColor(this Color color, [CallerMemberName] string name = "")
        {
            return new PaletteColor(color, name);
        }

    }
}
