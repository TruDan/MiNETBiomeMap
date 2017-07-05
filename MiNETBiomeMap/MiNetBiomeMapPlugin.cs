using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MiNET;
using MiNET.Plugins;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using MiNET.Effects;
using MiNET.Plugins.Attributes;
using MiNET.Worlds;
using Timer = System.Threading.Timer;

namespace MiNETBiomeMap
{
    [Plugin(PluginName = "MiNETBiomeMap")]
    public class MiNetBiomeMapPlugin : Plugin
    {
        private BiomeMapForm _biomeMapForm;

        private Timer _timer;

        private readonly object _sync = new object();

        public MiNetBiomeMapPlugin()
        {
        }

        [STAThread]
        protected override void OnEnable()
        {
            Application.EnableVisualStyles();
            base.OnEnable();
            
            Context.Server.LevelManager.LevelCreated += (sender, args) =>
            {
                if (_biomeMapForm != null) return;
                CreateForm(args.Level);
            };

            Context.Server.PlayerFactory.PlayerCreated += (sender, args) =>
            {
                args.Player.PlayerJoin += (o, eventArgs) =>
                {
                    _biomeMapForm.UpdateLevel();
                    args.Player.SetEffect(new Speed()
                    {
                        Duration = Effect.MaxDuration,
                        Level = 10,
                        Particles = false
                    });
                };
            };
            
        }

        private void CreateForm(Level level)
        {
            _biomeMapForm?.Hide();

            _biomeMapForm = new BiomeMapForm(level);
            _biomeMapForm.FormClosing += (sender, args) =>
            {
                _timer?.Change(Timeout.Infinite, Timeout.Infinite);
                _biomeMapForm = null;
            };

            _biomeMapForm.Shown += (sender, args) =>
            {
                _timer = new Timer(Update, null, 1000, 1000);
            };

            ThreadPool.QueueUserWorkItem(delegate
            {
                Application.Run(_biomeMapForm);
            });

        }

        private void Update(object state)
        {
            if (_biomeMapForm == null) return;

            if (!Monitor.TryEnter(_sync)) return;
            try
            {
                Console.WriteLine("Update Level");

                _biomeMapForm?.UpdateLevel();
            }
            finally
            {
                Monitor.Exit(_sync);
            }
        }

        public override void OnDisable()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _biomeMapForm.Hide();
            _biomeMapForm = null;

            base.OnDisable();
        }

        [Command(Name = "map", Description = "Show Biome Map")]
        public void ShowMap(Player player)
        {
            if (_biomeMapForm != null)
            {
                _biomeMapForm.Show();
            }
            else
            {
                CreateForm(player.Level);
            }
        }
    }
}
