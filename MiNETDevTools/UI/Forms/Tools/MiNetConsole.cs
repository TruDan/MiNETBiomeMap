using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiNETDevTools.UI.Forms.Tools
{
    public partial class MiNetConsole : ToolWindow, IDisposable
    {
        public MiNetConsole()
        {
            InitializeComponent();
            /*Console.SetOut(TextWriter.Synchronized((TextWriter)new StreamWriter(new MemoryStream(), Console.OutputEncoding)));

            ThreadPool.QueueUserWorkItem(state =>
            {
                var p = System.Diagnostics.Process.GetCurrentProcess();
                
                using (var s = p.StandardOutput)
                {
                    while (!this.IsDisposed && !p.HasExited)
                    {
                        var line = s.ReadLine();
                        this.consoleControl1.WriteOutput(line, SystemColors.GrayText);
                    }
                }
            });*/
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
        }
    }
}
