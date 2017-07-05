using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET;
using MiNETDevTools.Graphics;
using MiNETDevTools.UI.Forms;
using MiNETDevTools.UI.Forms.Tools;
using Process = System.Diagnostics.Process;

namespace MiNETDevTools.UI
{
    public class UiContext
    {
        public MainForm MainForm { get; }

        public UiContext()
        {
            MainForm = new MainForm(this);
        }
    }
}
