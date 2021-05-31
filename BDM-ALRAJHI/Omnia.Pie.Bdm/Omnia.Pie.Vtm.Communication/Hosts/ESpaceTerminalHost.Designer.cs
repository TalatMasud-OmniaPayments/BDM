namespace Omnia.Pie.Vtm.Communication
{
	partial class ESpaceTerminalHost
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ESpaceTerminalHost));
			this.axeSpaceMedia1 = new AxeSpaceMediaLib.AxeSpaceMedia();
			((System.ComponentModel.ISupportInitialize)(this.axeSpaceMedia1)).BeginInit();
			this.SuspendLayout();
			// 
			// axeSpaceMedia1
			// 
			this.axeSpaceMedia1.Enabled = true;
			this.axeSpaceMedia1.Location = new System.Drawing.Point(89, 120);
			this.axeSpaceMedia1.Name = "axeSpaceMedia1";
			this.axeSpaceMedia1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axeSpaceMedia1.OcxState")));
			this.axeSpaceMedia1.Size = new System.Drawing.Size(100, 50);
			this.axeSpaceMedia1.TabIndex = 0;
			// 
			// ESpaceTerminalHost
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(282, 253);
			this.Controls.Add(this.axeSpaceMedia1);
			this.Name = "ESpaceTerminalHost";
			this.Text = "ESpaceTerminalHost";
			((System.ComponentModel.ISupportInitialize)(this.axeSpaceMedia1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private AxeSpaceMediaLib.AxeSpaceMedia axeSpaceMedia1;
	}
}