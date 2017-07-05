namespace MiNETDevTools.Graphics.Components
{
    partial class ViewportCameraComponent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewportCameraComponent));
            this.headerToolStrip = new System.Windows.Forms.ToolStrip();
            this.statusToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.txtPos = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtSize = new System.Windows.Forms.ToolStripLabel();
            this.txtZoom = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.headerToolStrip.SuspendLayout();
            this.statusToolStrip.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerToolStrip
            // 
            this.headerToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.headerToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtZoom,
            this.btnZoomIn,
            this.btnZoomOut,
            this.toolStripSeparator2});
            this.headerToolStrip.Location = new System.Drawing.Point(0, 0);
            this.headerToolStrip.Name = "headerToolStrip";
            this.headerToolStrip.Size = new System.Drawing.Size(150, 25);
            this.headerToolStrip.Stretch = true;
            this.headerToolStrip.TabIndex = 0;
            this.headerToolStrip.Text = "toolStrip1";
            // 
            // statusToolStrip
            // 
            this.statusToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.statusToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtPos,
            this.toolStripSeparator1,
            this.txtSize,
            this.toolStripSeparator3});
            this.statusToolStrip.Location = new System.Drawing.Point(0, 0);
            this.statusToolStrip.Name = "statusToolStrip";
            this.statusToolStrip.Size = new System.Drawing.Size(150, 25);
            this.statusToolStrip.Stretch = true;
            this.statusToolStrip.TabIndex = 0;
            this.statusToolStrip.Text = "toolStrip2";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusToolStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(150, 100);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(150, 150);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.headerToolStrip);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
            this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(23, 22);
            this.btnZoomIn.Text = "toolStripButton1";
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
            this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(23, 22);
            this.btnZoomOut.Text = "toolStripButton2";
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // txtPos
            // 
            this.txtPos.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.txtPos.Image = ((System.Drawing.Image)(resources.GetObject("txtPos.Image")));
            this.txtPos.Name = "txtPos";
            this.txtPos.Size = new System.Drawing.Size(44, 22);
            this.txtPos.Text = "0 , 0";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // txtSize
            // 
            this.txtSize.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.txtSize.Image = ((System.Drawing.Image)(resources.GetObject("txtSize.Image")));
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(46, 22);
            this.txtSize.Text = "0 x 0";
            // 
            // txtZoom
            // 
            this.txtZoom.Image = ((System.Drawing.Image)(resources.GetObject("txtZoom.Image")));
            this.txtZoom.Name = "txtZoom";
            this.txtZoom.Size = new System.Drawing.Size(51, 22);
            this.txtZoom.Text = "100%";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ViewportCameraComponent
            // 
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "ViewportCameraComponent";
            this.headerToolStrip.ResumeLayout(false);
            this.headerToolStrip.PerformLayout();
            this.statusToolStrip.ResumeLayout(false);
            this.statusToolStrip.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip headerToolStrip;
        private System.Windows.Forms.ToolStrip statusToolStrip;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripLabel txtZoom;
        private System.Windows.Forms.ToolStripButton btnZoomIn;
        private System.Windows.Forms.ToolStripButton btnZoomOut;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel txtPos;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel txtSize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}
