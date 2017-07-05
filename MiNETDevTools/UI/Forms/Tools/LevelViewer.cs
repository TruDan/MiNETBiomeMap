using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiNETDevTools.Graphics.Biomes;
using MiNETDevTools.Graphics.Components;
using MiNETDevToolsPlugin.Models;
using WeifenLuo.WinFormsUI.Docking;
using Timer = System.Threading.Timer;

namespace MiNETDevTools.UI.Forms.Tools
{
    public partial class LevelViewer : ToolWindow
    {
        private Timer _updateTimer;


        private LevelData _level;
        private BiomeMapComponent _component;

        public LevelViewer()
        {
            this._component = new BiomeMapComponent();
            this._viewportCameraComponent = new ViewportCameraComponent(_component);
            InitializeComponent();
        }

        public void Init(LevelData level)
        {
            _level = level;
            TabText = level.Name;
            _component.Init(_level);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (_updateTimer == null)
            {
                _updateTimer = new Timer(Update, null, 250, 250);
            }
            else
            {
                _updateTimer.Change(250, 250);
            }
        }

        private void Update(object state)
        {
            try
            {
                _component?.UpdateMap();
            }
            catch
            {
                
            }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            _updateTimer?.Change(Timeout.Infinite, Timeout.Infinite);
            _updateTimer = null;
            base.OnDeactivate(e);
        }

        
    }
}
