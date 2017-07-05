using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DevTools.WinForms.Metro.Design.Converters;

namespace DevTools.WinForms.Metro.Framework.Themes
{
    [TypeConverter(typeof(PaletteColorConverter))]
    public class PaletteColor
    {
        public string Name { get; protected set; }

        public Color Color { get; protected set; }

        public PaletteColor(Color color, string name)
        {
            Name = name;
            Color = color;
        }

        public PaletteColor(int color, [CallerMemberName] string name = "") : this(Color.FromArgb(color), name)
        {
        }


        public static implicit operator Color(PaletteColor v)
        {
            return v.Color;
        }

        //public static implicit operator string(PaletteColor v)
        //{
        //    return v.Name;
        //}
    }
}
