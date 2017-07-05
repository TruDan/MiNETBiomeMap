using System.ComponentModel;
using System.Windows.Forms;

namespace MiNETDevTools.UI.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbarsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.levelExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.customTheme = new MiNETDevTools.UI.Theme.CustomTheme();
            this.vsToolStripExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.serverToolStripMenuItem,
            this.windowToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(1, 38);
            this.mainMenu.Margin = new System.Windows.Forms.Padding(0, 42, 0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(931, 24);
            this.mainMenu.TabIndex = 7;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // serverToolStripMenuItem
            // 
            this.serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stopToolStripMenuItem});
            this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            this.serverToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.serverToolStripMenuItem.Text = "Server";
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbarsToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // toolbarsToolStripMenuItem
            // 
            this.toolbarsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.levelExplorerToolStripMenuItem});
            this.toolbarsToolStripMenuItem.Name = "toolbarsToolStripMenuItem";
            this.toolbarsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.toolbarsToolStripMenuItem.Text = "Toolbars";
            // 
            // levelExplorerToolStripMenuItem
            // 
            this.levelExplorerToolStripMenuItem.Name = "levelExplorerToolStripMenuItem";
            this.levelExplorerToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.levelExplorerToolStripMenuItem.Text = "Level Explorer";
            this.levelExplorerToolStripMenuItem.Click += new System.EventHandler(this.levelExplorerToolStripMenuItem_Click);
            // 
            // statusBar
            // 
            this.statusBar.GripMargin = new System.Windows.Forms.Padding(7, 0, 7, 7);
            this.statusBar.Location = new System.Drawing.Point(1, 531);
            this.statusBar.Name = "statusBar";
            this.statusBar.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.statusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusBar.Size = new System.Drawing.Size(931, 22);
            this.statusBar.TabIndex = 4;
            // 
            // toolBar
            // 
            this.toolBar.Location = new System.Drawing.Point(1, 62);
            this.toolBar.Name = "toolBar";
            this.toolBar.Padding = new System.Windows.Forms.Padding(13, 0, 1, 0);
            this.toolBar.Size = new System.Drawing.Size(931, 25);
            this.toolBar.TabIndex = 6;
            // 
            // dockPanel
            // 
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DockBottomPortion = 150D;
            this.dockPanel.DockLeftPortion = 200D;
            this.dockPanel.DockRightPortion = 200D;
            this.dockPanel.DockTopPortion = 150D;
            this.dockPanel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(0)));
            this.dockPanel.Location = new System.Drawing.Point(1, 87);
            this.dockPanel.Margin = new System.Windows.Forms.Padding(0, 98, 0, 27);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.RightToLeftLayout = true;
            this.dockPanel.Size = new System.Drawing.Size(931, 444);
            this.dockPanel.SupportDeeplyNestedContent = true;
            this.dockPanel.TabIndex = 0;
            // 
            // vsToolStripExtender
            // 
            this.vsToolStripExtender.DefaultRenderer = null;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackImage = global::MiNETDevTools.Resources.ic_pikachu_dev_grey;
            this.BackImagePadding = new System.Windows.Forms.Padding(12, 7, 7, 7);
            this.BackMaxSize = 20;
            this.BorderStyle = MiNETDevTools.UI.Framework.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(933, 554);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.statusBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(373, 185);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(1, 38, 1, 1);
            this.ShadowType = MiNETDevTools.UI.Framework.MetroFormShadowType.DropShadow;
            this.Text = "MiNET Dev Tools";
            this.TransparencyKey = System.Drawing.Color.Empty;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.MenuStrip mainMenu;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.StatusStrip statusBar;
        private MiNETDevTools.UI.Theme.CustomTheme customTheme;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem windowToolStripMenuItem;
        private ToolStripMenuItem toolbarsToolStripMenuItem;
        private ToolStripMenuItem levelExplorerToolStripMenuItem;
        private ToolStripMenuItem serverToolStripMenuItem;
        private ToolStripMenuItem stopToolStripMenuItem;
    }
}