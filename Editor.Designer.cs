namespace WinBox_Maker
{
    partial class Editor
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
            BuildProcess = new ProgressBar();
            StartBuild = new Button();
            splitContainer1 = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // BuildProcess
            // 
            BuildProcess.Dock = DockStyle.Fill;
            BuildProcess.Location = new Point(0, 0);
            BuildProcess.Margin = new Padding(0);
            BuildProcess.Name = "BuildProcess";
            BuildProcess.Size = new Size(634, 37);
            BuildProcess.TabIndex = 0;
            // 
            // StartBuild
            // 
            StartBuild.Dock = DockStyle.Fill;
            StartBuild.Location = new Point(0, 0);
            StartBuild.Name = "StartBuild";
            StartBuild.Size = new Size(138, 37);
            StartBuild.TabIndex = 1;
            StartBuild.Text = "build";
            StartBuild.UseVisualStyleBackColor = true;
            StartBuild.Click += StartBuild_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(12, 398);
            splitContainer1.Margin = new Padding(0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(BuildProcess);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(StartBuild);
            splitContainer1.Size = new Size(776, 37);
            splitContainer1.SplitterDistance = 634;
            splitContainer1.TabIndex = 3;
            // 
            // Mainform
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer1);
            Name = "Mainform";
            Text = "Editor";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ProgressBar BuildProcess;
        private Button StartBuild;
        private SplitContainer splitContainer1;
    }
}
