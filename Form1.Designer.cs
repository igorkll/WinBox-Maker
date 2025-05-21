namespace WinBox_Maker
{
    partial class mainform
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
            progressBar1 = new ProgressBar();
            build = new Button();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            progressBar1.Location = new Point(12, 404);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(642, 34);
            progressBar1.TabIndex = 0;
            // 
            // build
            // 
            build.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            build.Location = new Point(660, 404);
            build.Name = "build";
            build.Size = new Size(128, 34);
            build.TabIndex = 1;
            build.Text = "button1";
            build.UseVisualStyleBackColor = true;
            build.Click += button1_Click;
            // 
            // mainform
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(800, 450);
            Controls.Add(build);
            Controls.Add(progressBar1);
            Name = "mainform";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private ProgressBar progressBar1;
        private Button build;
    }
}
