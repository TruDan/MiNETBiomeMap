using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevTools.WinForms.Metro.Design.Converters;
using DevTools.WinForms.Metro.Framework.Themes;

namespace DevTools.WinForms.Metro.Design.Editors
{
    public class PaletteColorEditor : ColorEditor
    {
        private static Color[] Colors;
        static PaletteColorEditor()
        {
            Colors = PaletteColorConverter.Colors.OfType<Color>().ToArray();
        }

        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            var colorEditorObject = this;
            Type colorUiType = typeof(ColorEditor).GetNestedType("ColorUI", BindingFlags.NonPublic);
            var colorUiConstructor = colorUiType.GetConstructors()[0];
            var colorUiField = typeof(ColorEditor).GetField("colorUI", BindingFlags.Instance | BindingFlags.NonPublic);
            var colorUiObject = colorUiConstructor.Invoke(new[] { colorEditorObject });
            colorUiField.SetValue(colorEditorObject, colorUiObject);
            var palField = colorUiObject.GetType().GetField("pal", BindingFlags.Instance | BindingFlags.NonPublic);
            var palObject = palField.GetValue(colorUiObject);
            var palCustomColorsField = palObject.GetType().GetField("customColors", BindingFlags.Instance | BindingFlags.NonPublic);
            palCustomColorsField.SetValue(palObject, Colors);
            var selectedValue = base.EditValue(context, provider, value);
            Colors = palCustomColorsField.GetValue(palObject) as Color[];
            return selectedValue;
        }
    }
}
