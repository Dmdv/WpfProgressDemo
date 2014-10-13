namespace ProgressDemo
{
	partial class ProgressWindowTester
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
			this.bnStartIndef = new System.Windows.Forms.Button();
			this.bnStartDef = new System.Windows.Forms.Button();
			this.bnIndefWithReport = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// bnStartIndef
			// 
			this.bnStartIndef.Location = new System.Drawing.Point(12, 170);
			this.bnStartIndef.Name = "bnStartIndef";
			this.bnStartIndef.Size = new System.Drawing.Size(143, 34);
			this.bnStartIndef.TabIndex = 0;
			this.bnStartIndef.Text = "Start indefinitely";
			this.bnStartIndef.UseVisualStyleBackColor = true;
			this.bnStartIndef.Click += new System.EventHandler(this.bnStartIndef_Click);
			// 
			// bnStartDef
			// 
			this.bnStartDef.Location = new System.Drawing.Point(161, 170);
			this.bnStartDef.Name = "bnStartDef";
			this.bnStartDef.Size = new System.Drawing.Size(143, 34);
			this.bnStartDef.TabIndex = 1;
			this.bnStartDef.Text = "Start 10 times with report";
			this.bnStartDef.UseVisualStyleBackColor = true;
			this.bnStartDef.Click += new System.EventHandler(this.bnStartDef_Click);
			// 
			// bnIndefWithReport
			// 
			this.bnIndefWithReport.Location = new System.Drawing.Point(310, 170);
			this.bnIndefWithReport.Name = "bnIndefWithReport";
			this.bnIndefWithReport.Size = new System.Drawing.Size(143, 34);
			this.bnIndefWithReport.TabIndex = 2;
			this.bnIndefWithReport.Text = "Start indefinitely with report";
			this.bnIndefWithReport.UseVisualStyleBackColor = true;
			this.bnIndefWithReport.Click += new System.EventHandler(this.bnIndefWithReport_Click);
			// 
			// ProgressWindowTester
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(636, 216);
			this.Controls.Add(this.bnIndefWithReport);
			this.Controls.Add(this.bnStartDef);
			this.Controls.Add(this.bnStartIndef);
			this.Name = "ProgressWindowTester";
			this.Text = "ProgressWindowTester";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button bnStartIndef;
		private System.Windows.Forms.Button bnStartDef;
		private System.Windows.Forms.Button bnIndefWithReport;
	}
}