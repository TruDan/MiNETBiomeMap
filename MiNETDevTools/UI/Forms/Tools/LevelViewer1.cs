using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiNET.Worlds;
using MiNETDevTools.Graphics.Biomes;
using MiNETDevTools.Graphics.Internal;
using SharpDX;
using WeifenLuo.WinFormsUI.Docking;
using Timer = System.Threading.Timer;

namespace MiNETDevTools.UI.Forms.Tools
{
    public class LevelViewer1 : ToolWindow
    {
        private VS2015LightTheme vS2015LightTheme1;
        private Timer _updateTimer;

        public LevelViewer1()
        {
            InitializeComponent();
            this.DockAreas = DockAreas.Document | DockAreas.Float;
        }
        
        protected override void OnLoadUiContext(UiContext context)
        {
            var l = "default";
            if (l == null)
            {
                return;
            }

            //biomeMapComponent1.Init(l);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (_updateTimer == null)
            {
                _updateTimer = new Timer(Update, null, 250, 250);
            }
            else
            {
                _updateTimer.Change(250, 250);
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            _updateTimer?.Change(Timeout.Infinite, Timeout.Infinite);
            _updateTimer = null;
            base.OnHandleDestroyed(e);
        }

        private void Update(object state)
        {
           // biomeMapComponent1?.UpdateMap();
        }

        private void InitializeComponent()
        {
            this.vS2015LightTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.SuspendLayout();
            // 
            // LevelViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "LevelViewer";
            this.ResumeLayout(false);

        }
    }
}
