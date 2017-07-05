using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MiNETDevTools.UI.Forms.Tools
{
    public partial class ToolWindow : DockContent
    {
        protected UiContext Context { get; private set; }
        
        public ToolWindow()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
        }

        internal void LoadUiContext(UiContext context)
        {
            Context = context;
            OnLoadUiContext(context);
        }

        protected virtual void OnLoadUiContext(UiContext context) { }
    }
}
