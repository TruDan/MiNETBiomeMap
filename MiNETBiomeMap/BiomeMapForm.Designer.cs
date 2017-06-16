namespace MiNETBiomeMap
{
    partial class BiomeMapForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BiomeMapForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLevel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProvider = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusChunks = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBiomeTemp = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBiomeDownfall = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBiome = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.progress = new System.Windows.Forms.ToolStripProgressBar();
            this.progressCancel = new System.Windows.Forms.ToolStripDropDownButton();
            this.progressStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.progressStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.chunkGenX = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.chunkGenZ = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBox2 = new System.Windows.Forms.ToolStripComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusGeneric = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusDuration = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.progressStatusStrip.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(614, 419);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLevel,
            this.statusProvider,
            this.statusChunks,
            this.toolStripStatusLabel1,
            this.statusBiomeTemp,
            this.statusBiomeDownfall,
            this.statusBiome,
            this.statusPosition});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(614, 24);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLevel
            // 
            this.statusLevel.Name = "statusLevel";
            this.statusLevel.Size = new System.Drawing.Size(34, 19);
            this.statusLevel.Text = "Level";
            // 
            // statusProvider
            // 
            this.statusProvider.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusProvider.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusProvider.Name = "statusProvider";
            this.statusProvider.Size = new System.Drawing.Size(55, 19);
            this.statusProvider.Text = "Provider";
            // 
            // statusChunks
            // 
            this.statusChunks.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.statusChunks.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusChunks.Name = "statusChunks";
            this.statusChunks.Size = new System.Drawing.Size(60, 19);
            this.statusChunks.Text = "0 Chunks";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(286, 19);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // statusBiomeTemp
            // 
            this.statusBiomeTemp.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusBiomeTemp.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusBiomeTemp.Name = "statusBiomeTemp";
            this.statusBiomeTemp.Size = new System.Drawing.Size(50, 19);
            this.statusBiomeTemp.Text = "Temp 0";
            // 
            // statusBiomeDownfall
            // 
            this.statusBiomeDownfall.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusBiomeDownfall.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusBiomeDownfall.Name = "statusBiomeDownfall";
            this.statusBiomeDownfall.Size = new System.Drawing.Size(43, 19);
            this.statusBiomeDownfall.Text = "Rain 0";
            // 
            // statusBiome
            // 
            this.statusBiome.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusBiome.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusBiome.Name = "statusBiome";
            this.statusBiome.Size = new System.Drawing.Size(45, 19);
            this.statusBiome.Text = "Biome";
            // 
            // statusPosition
            // 
            this.statusPosition.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusPosition.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusPosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusPosition.Name = "statusPosition";
            this.statusPosition.Size = new System.Drawing.Size(26, 19);
            this.statusPosition.Text = "0,0";
            this.statusPosition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.statusPosition.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            // 
            // progress
            // 
            this.progress.AutoSize = false;
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(100, 18);
            this.progress.Visible = false;
            // 
            // progressCancel
            // 
            this.progressCancel.AutoSize = false;
            this.progressCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.progressCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.progressCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.progressCancel.Name = "progressCancel";
            this.progressCancel.ShowDropDownArrow = false;
            this.progressCancel.Size = new System.Drawing.Size(18, 22);
            this.progressCancel.Text = "X";
            this.progressCancel.Visible = false;
            this.progressCancel.Click += new System.EventHandler(this.progressCancel_Click);
            // 
            // progressStatus
            // 
            this.progressStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.progressStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.progressStatus.Name = "progressStatus";
            this.progressStatus.Size = new System.Drawing.Size(56, 19);
            this.progressStatus.Text = "Progress";
            this.progressStatus.Visible = false;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.progressStatusStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.pictureBox1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(614, 419);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(614, 517);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip3);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // progressStatusStrip
            // 
            this.progressStatusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.progressStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusDuration,
            this.toolStripStatusLabel2,
            this.progressStatus,
            this.progress,
            this.progressCancel,
            this.statusGeneric});
            this.progressStatusStrip.Location = new System.Drawing.Point(0, 24);
            this.progressStatusStrip.Name = "progressStatusStrip";
            this.progressStatusStrip.Size = new System.Drawing.Size(614, 24);
            this.progressStatusStrip.SizingGrip = false;
            this.progressStatusStrip.TabIndex = 2;
            // 
            // toolStrip3
            // 
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.toolStripTextBox1});
            this.toolStrip3.Location = new System.Drawing.Point(3, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(114, 25);
            this.toolStrip3.TabIndex = 2;
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(60, 22);
            this.toolStripLabel3.Text = "Teleport Y";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripTextBox1.AutoSize = false;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripTextBox1.Size = new System.Drawing.Size(40, 25);
            this.toolStripTextBox1.Text = "128";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel4,
            this.toolStripComboBox1,
            this.toolStripSeparator1,
            this.chunkGenX,
            this.toolStripLabel1,
            this.chunkGenZ,
            this.toolStripSeparator2,
            this.toolStripButton1});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(3, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(440, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(96, 22);
            this.toolStripLabel4.Text = "Generation Tools";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "128 x 128",
            "256 x 256",
            "512 x 512",
            "1024 x 1024",
            "2048 x 2048",
            "4096  x 4096"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // chunkGenX
            // 
            this.chunkGenX.Name = "chunkGenX";
            this.chunkGenX.Size = new System.Drawing.Size(40, 25);
            this.chunkGenX.Text = "128";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(12, 22);
            this.toolStripLabel1.Text = "x";
            // 
            // chunkGenZ
            // 
            this.chunkGenZ.Name = "chunkGenZ";
            this.chunkGenZ.Size = new System.Drawing.Size(40, 25);
            this.chunkGenZ.Text = "128";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(101, 22);
            this.toolStripButton1.Text = "Generate Chunks";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.toolStripComboBox2});
            this.toolStrip2.Location = new System.Drawing.Point(116, 24);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(214, 25);
            this.toolStrip2.TabIndex = 1;
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(79, 22);
            this.toolStripLabel2.Text = "Display Mode";
            // 
            // toolStripComboBox2
            // 
            this.toolStripComboBox2.Items.AddRange(new object[] {
            "Biome (Grass)",
            "Biome (Foilage)",
            "Rainfall",
            "Temperature"});
            this.toolStripComboBox2.Name = "toolStripComboBox2";
            this.toolStripComboBox2.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBox2.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox2_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(614, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keepOnTopToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // keepOnTopToolStripMenuItem
            // 
            this.keepOnTopToolStripMenuItem.Name = "keepOnTopToolStripMenuItem";
            this.keepOnTopToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.keepOnTopToolStripMenuItem.Text = "Always on top";
            this.keepOnTopToolStripMenuItem.Click += new System.EventHandler(this.keepOnTopToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(316, 19);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // statusGeneric
            // 
            this.statusGeneric.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusGeneric.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusGeneric.Name = "statusGeneric";
            this.statusGeneric.Size = new System.Drawing.Size(43, 19);
            this.statusGeneric.Text = "Ready";
            // 
            // statusDuration
            // 
            this.statusDuration.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.statusDuration.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusDuration.Name = "statusDuration";
            this.statusDuration.Size = new System.Drawing.Size(33, 19);
            this.statusDuration.Text = "0ms";
            // 
            // BiomeMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 541);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(630, 580);
            this.Name = "BiomeMapForm";
            this.Text = "MiNET Biome Map";
            this.Load += new System.EventHandler(this.BiomeMapForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.progressStatusStrip.ResumeLayout(false);
            this.progressStatusStrip.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLevel;
        private System.Windows.Forms.ToolStripStatusLabel statusChunks;
        private System.Windows.Forms.ToolStripStatusLabel statusPosition;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel statusBiome;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox chunkGenX;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox chunkGenZ;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripProgressBar progress;
        private System.Windows.Forms.ToolStripStatusLabel progressStatus;
        private System.Windows.Forms.ToolStripDropDownButton progressCancel;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox2;
        private System.Windows.Forms.ToolStripStatusLabel statusBiomeTemp;
        private System.Windows.Forms.ToolStripStatusLabel statusBiomeDownfall;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepOnTopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel statusProvider;
        private System.Windows.Forms.StatusStrip progressStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel statusGeneric;
        private System.Windows.Forms.ToolStripStatusLabel statusDuration;
    }
}