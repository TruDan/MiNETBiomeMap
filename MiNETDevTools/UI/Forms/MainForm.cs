using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiNETDevTools.UI.Forms.Tools;
using MiNETDevTools.UI.Framework;
using MiNETDevTools.UI.Theme;
using WeifenLuo.WinFormsUI.Docking;

namespace MiNETDevTools.UI.Forms
{
    public partial class MainForm : CustomThemeForm
    {
        protected UiContext Context { get; private set; }
        
        public MainForm(UiContext context)
        {
            Context = context;
            InitializeComponent();
            customTheme.ApplyToToolStripManager();
            //this.BorderStyle = MetroFormBorderStyle.FixedSingle;
            //this.ShadowType = MetroFormShadowType.AeroShadow;
            this.ApplyDockPanelSkin();
        }

        public MainForm() : this(null)
        {
            if (!DesignMode)
            {
                throw new AccessViolationException();
            }
        }

        public void ShowToolWindow(ToolWindow content)
        {
            ShowToolWindow(content, DockState.DockLeft);
        }
        public void ShowToolWindow(ToolWindow content, DockState state)
        {
            content.Show(this.dockPanel, state);
        }

        public void ShowDocument(ToolWindow content)
        {
            content.Show(this.dockPanel, DockState.Document);
        }


        private void ApplyDockPanelSkin()
        {
            this.dockPanel.Theme = customTheme;
            this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, customTheme);
        }

        private void EnableVSRenderer(VisualStudioToolStripExtender.VsVersion version, ThemeBase theme)
        {
            vsToolStripExtender.SetStyle(mainMenu, version, theme);
            vsToolStripExtender.SetStyle(toolBar, version, theme);
            vsToolStripExtender.SetStyle(statusBar, version, theme);
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            return null;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //string configFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "app.config");

            //if (File.Exists(configFile))
                //dockPanel.LoadFromXml(configFile, GetContentFromPersistString);

            ShowToolWindow(ToolFactory.CreateTool<LevelExplorer>(Context));
            //ShowToolWindow(ToolFactory.CreateTool<MiNetConsole>(Context), DockState.DockBottom);
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //string configFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "app.config");
            //dockPanel.SaveAsXml(configFile);
        }

        private void levelExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowToolWindow(ToolFactory.CreateTool<LevelExplorer>(Context));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
