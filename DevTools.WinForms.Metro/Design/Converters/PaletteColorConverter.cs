using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using DevTools.WinForms.Metro.Framework.Themes;

namespace DevTools.WinForms.Metro.Design.Converters
{
    public class PaletteColorConverter : StringConverter
    {
        public static PaletteColor[] Colors => _colorIndex.Values.ToArray();

        static Dictionary<PaletteColor, string> _nameIndex = InitializeNameIndex();
        static Dictionary<string, PaletteColor> _colorIndex = InitializeColorIndex();

        private static Dictionary<string, PaletteColor> InitializeColorIndex()
        {
            return typeof(Theme)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .ToDictionary(f => f.Name, f => (PaletteColor)f.GetValue(null));
        }

        private static Dictionary<PaletteColor, string> InitializeNameIndex()
        {
            return typeof(Theme)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .ToDictionary(f => (PaletteColor)f.GetValue(null), f => f.Name);
        }

        public static PaletteColor ToPaletteColor(string name)
        {
            PaletteColor result;
            if (_colorIndex.TryGetValue(name, out result))
                return result;
            return null;
        }
        public static PaletteColor ToPaletteColor(Color color)
        {
            return Colors.FirstOrDefault(c => c.Color.Equals(color));
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new System.ComponentModel.TypeConverter.StandardValuesCollection(_nameIndex.Values.ToList());
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(PaletteColor))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                PaletteColor result;
                if (_colorIndex.TryGetValue((string)value, out result))
                    return result;
                //else
                //    return new PaletteColor();
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PaletteColor)
            {
                string result;
                if (_nameIndex.TryGetValue((PaletteColor)value, out result))
                    return result;
                else
                    return String.Empty;
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
