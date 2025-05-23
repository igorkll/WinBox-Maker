namespace WinBox_Maker
{
    partial class EditorForm
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
            WindowsSelect = new Button();
            WindowsName = new Label();
            WindowsClear = new Button();
            WindowsVersionSelect = new ComboBox();
            WindowsVersionClear = new Button();
            WindowsVersionDetect = new Button();
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
            StartBuild.Text = "Build";
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
            // WindowsSelect
            // 
            WindowsSelect.Location = new Point(12, 12);
            WindowsSelect.Name = "WindowsSelect";
            WindowsSelect.Size = new Size(303, 34);
            WindowsSelect.TabIndex = 4;
            WindowsSelect.Text = "Select base windows image";
            WindowsSelect.UseVisualStyleBackColor = true;
            WindowsSelect.Click += WindowsSelect_Click;
            // 
            // WindowsName
            // 
            WindowsName.AutoSize = true;
            WindowsName.Location = new Point(439, 17);
            WindowsName.Name = "WindowsName";
            WindowsName.Size = new Size(178, 25);
            WindowsName.TabIndex = 5;
            WindowsName.Text = "base windows image";
            // 
            // WindowsClear
            // 
            WindowsClear.Location = new Point(321, 12);
            WindowsClear.Name = "WindowsClear";
            WindowsClear.Size = new Size(112, 34);
            WindowsClear.TabIndex = 6;
            WindowsClear.Text = "Clear";
            WindowsClear.UseVisualStyleBackColor = true;
            WindowsClear.Click += WindowsClear_Click;
            // 
            // WindowsVersionSelect
            // 
            WindowsVersionSelect.FormattingEnabled = true;
            WindowsVersionSelect.Items.AddRange(new object[] { "test1", "test2", "test3", "test4" });
            WindowsVersionSelect.Location = new Point(12, 52);
            WindowsVersionSelect.Name = "WindowsVersionSelect";
            WindowsVersionSelect.Size = new Size(303, 33);
            WindowsVersionSelect.TabIndex = 7;
            WindowsVersionSelect.TextChanged += WindowsVersionSelect_TextChanged;
            // 
            // WindowsVersionClear
            // 
            WindowsVersionClear.Location = new Point(321, 52);
            WindowsVersionClear.Name = "WindowsVersionClear";
            WindowsVersionClear.Size = new Size(112, 34);
            WindowsVersionClear.TabIndex = 8;
            WindowsVersionClear.Text = "Clear";
            WindowsVersionClear.UseVisualStyleBackColor = true;
            WindowsVersionClear.Click += WindowsVersionClear_Click;
            // 
            // WindowsVersionDetect
            // 
            WindowsVersionDetect.Location = new Point(439, 52);
            WindowsVersionDetect.Name = "WindowsVersionDetect";
            WindowsVersionDetect.Size = new Size(112, 34);
            WindowsVersionDetect.TabIndex = 9;
            WindowsVersionDetect.Text = "Detect";
            WindowsVersionDetect.UseVisualStyleBackColor = true;
            WindowsVersionDetect.Click += WindowsVersionDetect_Click;
            // 
            // EditorForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(800, 450);
            Controls.Add(WindowsVersionDetect);
            Controls.Add(WindowsVersionClear);
            Controls.Add(WindowsVersionSelect);
            Controls.Add(WindowsClear);
            Controls.Add(WindowsName);
            Controls.Add(WindowsSelect);
            Controls.Add(splitContainer1);
            Name = "EditorForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Editor";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar BuildProcess;
        private Button StartBuild;
        private SplitContainer splitContainer1;
        private Button WindowsSelect;
        private Label WindowsName;
        private Button WindowsClear;
        private ComboBox WindowsVersionSelect;
        private Button WindowsVersionClear;
        private Button WindowsVersionDetect;
    }
}
