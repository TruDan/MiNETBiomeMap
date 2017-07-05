using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using MiNET.Worlds;
using MiNETDevTools.Plugin;
using MiNETDevToolsPlugin.Interfaces;
using MiNETDevToolsPlugin.Models;
using ServiceWire.TcpIp;
using WeifenLuo.WinFormsUI.Docking;
using Timer = System.Threading.Timer;

namespace MiNETDevTools.UI.Forms.Tools
{
    public partial class LevelExplorer : ToolWindow
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LevelExplorer));
        
        private Timer _updateTimer;

        public LevelExplorer()
        {
            InitializeComponent();
        }

        protected override void OnLoadUiContext(UiContext context)
        {

            if (_updateTimer == null)
            {
                _updateTimer = new Timer(UpdateLevelList, null, 1000, 1000);
            }
            else
            {
                _updateTimer.Change(250, 250);
            }
        }

        private void UpdateLevelList(object state)
        {
            try
            {
                using (var client = new TcpClient<ILevelService>(new IPEndPoint(IPAddress.Loopback, 19137)))
                {
                    var levels = client.Proxy.FetchAllLevels();

                    if (levels.Length == 0)
                        return;

                    this.listView1.SuspendLayout();

                    foreach (var level in levels)
                    {
                        if(string.IsNullOrEmpty(level.Id))
                            continue;

                        var found = false;
                        for (int i = listView1.Items.Count - 1; i >= 0; i--)
                        {
                            var item = listView1.Items[i];
                            if (item.Name.Equals(level.Id, StringComparison.InvariantCultureIgnoreCase))
                            {
                                found = true;

                                item.Text = level.Name;
                                item.Tag = level;
                                //item.SubItems[0].Text = level.Id;
                                item.SubItems[0].Text = level.Name;
                                item.SubItems[1].Text = level.Players.ToString();
                                item.SubItems[2].Text = level.AvarageTickProcessingTime.ToString();
                                break;
                            }
                            Log.InfoFormat("NO MATCH {0} == {1}", level.Id, item.Name);
                        }

                        if(found)
                            continue;

                        Log.InfoFormat("Adding Level {0} {1} {2} {3}", level.Id, level.Name, level.Players, level.AvarageTickProcessingTime);

                        var g = new ListViewItem();
                        g.Tag = level;
                        g.Text = level.Id;
                        g.SubItems.AddRange(new string[]
                        {
                            level.Id,
                            level.Name,
                            level.Players.ToString(),
                            level.AvarageTickProcessingTime.ToString()
                        });

                        this.listView1.Items.Add(level.Id, level.Name, null).SubItems.AddRange(new [] {level.Players.ToString(), level.AvarageTickProcessingTime.ToString()});
                    }

                    this.listView1.ResumeLayout(true);
                }
            }
            catch
            {
                
            }
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            Log.InfoFormat("Checked {0}", e.Item);

            if (!e.Item.Checked)
                return;

            var level = (LevelData) e.Item.Tag;

            if (level == null)
                return;

            var tool = ToolFactory.CreateTool<LevelViewer>(Context);
            tool.Init(level);

            Context.MainForm.ShowDocument(tool);
        }
    }
}
