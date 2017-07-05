using System.Drawing;
using System.Windows.Forms;

namespace MiNETDevTools.UI.Forms.Tools
{
    partial class LevelExplorer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelExplorer));
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderId1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerLevelName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerPlayers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTPS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.levelItemMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.levelItemMenuStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderId1,
            this.headerLevelName,
            this.headerPlayers,
            this.columnHeaderTPS});
            this.listView1.ContextMenuStrip = this.levelItemMenuStrip;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HotTracking = true;
            this.listView1.HoverSelection = true;
            this.listView1.LabelWrap = false;
            this.listView1.Location = new System.Drawing.Point(0, 28);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(245, 294);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            // 
            // columnHeaderId1
            // 
            this.columnHeaderId1.Text = "ID";
            this.columnHeaderId1.Width = 22;
            // 
            // columnHeaderId
            // 
            this.columnHeaderId.Text = "ID";
            this.columnHeaderId.Width = 56;
            // 
            // headerLevelName
            // 
            this.headerLevelName.Text = "Name";
            this.headerLevelName.Width = 56;
            // 
            // headerPlayers
            // 
            this.headerPlayers.Text = "Players";
            this.headerPlayers.Width = 47;
            // 
            // columnHeaderTPS
            // 
            this.columnHeaderTPS.Text = "Avg Tick";
            this.columnHeaderTPS.Width = 54;
            // 
            // levelItemMenuStrip
            // 
            this.levelItemMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripSeparator1,
            this.toolStripMenuItem3});
            this.levelItemMenuStrip.Name = "levelItemMenuStrip";
            this.levelItemMenuStrip.Size = new System.Drawing.Size(165, 98);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(164, 22);
            this.toolStripMenuItem4.Text = "Generate Chunks";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
            this.toolStripMenuItem1.Text = "Map Visualiser";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(164, 22);
            this.toolStripMenuItem2.Text = "Players";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(161, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(164, 22);
            this.toolStripMenuItem3.Text = "Unload";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemRefresh,
            this.toolStripMenuItemAdd});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(2);
            this.menuStrip1.Size = new System.Drawing.Size(245, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItemRefresh
            // 
            this.toolStripMenuItemRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemRefresh.Image")));
            this.toolStripMenuItemRefresh.Name = "toolStripMenuItemRefresh";
            this.toolStripMenuItemRefresh.Padding = new System.Windows.Forms.Padding(2);
            this.toolStripMenuItemRefresh.Size = new System.Drawing.Size(24, 24);
            this.toolStripMenuItemRefresh.ToolTipText = "Refresh";
            // 
            // toolStripMenuItemAdd
            // 
            this.toolStripMenuItemAdd.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripMenuItemAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemAdd.Image")));
            this.toolStripMenuItemAdd.Name = "toolStripMenuItemAdd";
            this.toolStripMenuItemAdd.Size = new System.Drawing.Size(28, 24);
            // 
            // LevelExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(245, 322);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.menuStrip1);
            this.HideOnClose = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "LevelExplorer";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
            this.TabText = "Level Explorer";
            this.Text = "Level Explorer";
            this.levelItemMenuStrip.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListView listView1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItemRefresh;
        private ColumnHeader columnHeaderId1;
        private ColumnHeader headerLevelName;
        private ColumnHeader headerPlayers;
        private ContextMenuStrip levelItemMenuStrip;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItemAdd;
        private ColumnHeader columnHeaderId;
        private ColumnHeader columnHeaderTPS;
    }
}
