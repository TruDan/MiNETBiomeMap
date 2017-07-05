using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNETDevTools.UI.Util
{
    public static class ColorExtensions
    {

        public static Color ToColor(this uint argb)
        {
            if (argb < 0x01000000)
            {
                return Color.FromArgb(255,
                    (byte)((argb & 0xff0000) >> 0x10),
                    (byte)((argb & 0xff00) >> 8),
                    (byte)(argb & 0xff));
            }
            else
            {
                return Color.FromArgb((byte)((argb & -16777216) >> 0x18),
                    (byte)((argb & 0xff0000) >> 0x10),
                    (byte)((argb & 0xff00) >> 8),
                    (byte)(argb & 0xff));
            }
        }

    }
}
