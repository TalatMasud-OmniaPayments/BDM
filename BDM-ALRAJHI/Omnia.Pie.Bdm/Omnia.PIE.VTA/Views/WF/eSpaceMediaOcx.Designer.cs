namespace Omnia.PIE.VTA.Views.WF
{
    partial class eSpaceMediaOcx
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(eSpaceMediaOcx));
            this.axeSpaceMedia = new AxeSpaceMediaLib.AxeSpaceMedia();
            this.axVRCControl1 = new AxVRCCONTROLLib.AxVRCControl();
            ((System.ComponentModel.ISupportInitialize)(this.axeSpaceMedia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axVRCControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axeSpaceMedia
            // 
            this.axeSpaceMedia.Enabled = true;
            this.axeSpaceMedia.Location = new System.Drawing.Point(82, 82);
            this.axeSpaceMedia.Name = "axeSpaceMedia";
            this.axeSpaceMedia.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axeSpaceMedia.OcxState")));
            this.axeSpaceMedia.Size = new System.Drawing.Size(100, 50);
            this.axeSpaceMedia.TabIndex = 0;
            // 
            // axVRCControl1
            // 
            this.axVRCControl1.Enabled = true;
            this.axVRCControl1.Location = new System.Drawing.Point(82, 201);
            this.axVRCControl1.Name = "axVRCControl1";
            this.axVRCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVRCControl1.OcxState")));
            this.axVRCControl1.Size = new System.Drawing.Size(100, 50);
            this.axVRCControl1.TabIndex = 1;
            // 
            // eSpaceMediaOcx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.axVRCControl1);
            this.Controls.Add(this.axeSpaceMedia);
            this.Name = "eSpaceMediaOcx";
            this.Text = "eSpaceMediaOcx";
            ((System.ComponentModel.ISupportInitialize)(this.axeSpaceMedia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axVRCControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxeSpaceMediaLib.AxeSpaceMedia axeSpaceMedia;
        private AxVRCCONTROLLib.AxVRCControl axVRCControl1;
    }
}