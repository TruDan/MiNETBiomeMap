using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DevTools.WinForms.Metro.Design.Editors;
using Microsoft.VisualStudio.Shell;

namespace DevTools.WinForms.Metro.Design
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideAutoLoad(Microsoft.VisualStudio.Shell.Interop.UIContextGuids80.SolutionExists)]

    public sealed class DevToolsPackage : Package
    {
        public DevToolsPackage() { }
        protected override void Initialize()
        {
            base.Initialize();
            TypeDescriptor.AddAttributes(typeof(Color), new EditorAttribute(typeof(PaletteColorEditor), typeof(UITypeEditor)));
        }
    }
}
