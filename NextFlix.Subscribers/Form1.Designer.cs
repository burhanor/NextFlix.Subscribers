namespace NextFlix.Subscribers
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			btnStart = new Button();
			lblRedisStatus = new Label();
			btnStop = new Button();
			SuspendLayout();
			// 
			// btnStart
			// 
			btnStart.Location = new Point(42, 12);
			btnStart.Name = "btnStart";
			btnStart.Size = new Size(75, 23);
			btnStart.TabIndex = 0;
			btnStart.Text = "Başlat";
			btnStart.UseVisualStyleBackColor = true;
			btnStart.Click += btnStart_Click;
			// 
			// lblRedisStatus
			// 
			lblRedisStatus.AutoSize = true;
			lblRedisStatus.Location = new Point(345, 22);
			lblRedisStatus.Name = "lblRedisStatus";
			lblRedisStatus.Size = new Size(35, 15);
			lblRedisStatus.TabIndex = 1;
			lblRedisStatus.Text = "Redis";
			// 
			// btnStop
			// 
			btnStop.Enabled = false;
			btnStop.Location = new Point(42, 41);
			btnStop.Name = "btnStop";
			btnStop.Size = new Size(75, 23);
			btnStop.TabIndex = 2;
			btnStop.Text = "Durdur";
			btnStop.UseVisualStyleBackColor = true;
			btnStop.Click += btnStop_Click;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1422, 622);
			Controls.Add(btnStop);
			Controls.Add(lblRedisStatus);
			Controls.Add(btnStart);
			Name = "Form1";
			Text = "Form1";
			Load += Form1_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button btnStart;
		private Label lblRedisStatus;
		private Button btnStop;
	}
}
