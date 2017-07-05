using DevTools.WinForms.Metro.Framework.Shadows;

namespace DevTools.WinForms
{
    partial class Form1
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
            this.paletteColorExtensionProvider1 = new DevTools.WinForms.Metro.Design.Extensions.PaletteColorExtensionProvider();
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackImage = global::DevTools.WinForms.Properties.Resources.ic_pikachu_dev_grey;
            this.BackImagePadding = new System.Windows.Forms.Padding(12, 7, 7, 7);
            this.BackLocation = DevTools.WinForms.Metro.Framework.BackLocation.TopLeft;
            this.BackMaxSize = 20;
            this.BorderStyle = DevTools.WinForms.Metro.Framework.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(560, 480);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(1, 32, 1, 1);
            this.ShadowType = DevTools.WinForms.Metro.Framework.Shadows.MetroFormShadowType.Glow;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "MiNET Dev Tools";
            this.TransparencyKey = System.Drawing.Color.Empty;
            this.ResumeLayout(false);

        }

        #endregion

        private Metro.Design.Extensions.PaletteColorExtensionProvider paletteColorExtensionProvider1;
    }
}

