using System.Windows.Forms;
using MiNETDevTools.Graphics.Biomes;
using MiNETDevTools.Graphics.Components;
using WeifenLuo.WinFormsUI.Docking;

namespace MiNETDevTools.UI.Forms.Tools
{
    partial class LevelViewer
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
            this._viewportCameraComponent = new ViewportCameraComponent();
            this._viewportCameraComponent.SuspendLayout();
            this.SuspendLayout();
            // 
            // _viewportCameraComponent
            // 
            this._viewportCameraComponent.AutoSize = true;
            this._viewportCameraComponent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._viewportCameraComponent.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewportCameraComponent.Location = new System.Drawing.Point(0, 0);
            this._viewportCameraComponent.Name = "_viewportCameraComponent";
            this._viewportCameraComponent.Size = new System.Drawing.Size(292, 266);
            this._viewportCameraComponent.TabIndex = 0;
            // 
            // LevelViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this._viewportCameraComponent);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Name = "LevelViewer";
            this.Text = "LevelViewer";
            this._viewportCameraComponent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        private ViewportCameraComponent _viewportCameraComponent;
        #endregion
    }
}