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
            ProcessValue = new ProgressBar();
            StartBuild = new Button();
            splitContainer1 = new SplitContainer();
            WindowsSelect = new Button();
            WindowsName = new Label();
            WindowsClear = new Button();
            WindowsVersionSelect = new ComboBox();
            WindowsVersionUpdate = new Button();
            WindowsVersionClear = new Button();
            ProcessName = new Label();
            WindowsDescription = new RichTextBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // ProcessValue
            // 
            ProcessValue.Dock = DockStyle.Fill;
            ProcessValue.Location = new Point(0, 0);
            ProcessValue.Margin = new Padding(0);
            ProcessValue.Name = "ProcessValue";
            ProcessValue.Size = new Size(799, 37);
            ProcessValue.TabIndex = 0;
            // 
            // StartBuild
            // 
            StartBuild.Dock = DockStyle.Fill;
            StartBuild.Location = new Point(0, 0);
            StartBuild.Name = "StartBuild";
            StartBuild.Size = new Size(175, 37);
            StartBuild.TabIndex = 1;
            StartBuild.Text = "Build";
            StartBuild.UseVisualStyleBackColor = true;
            StartBuild.Click += StartBuild_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(12, 660);
            splitContainer1.Margin = new Padding(0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(ProcessValue);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(StartBuild);
            splitContainer1.Size = new Size(978, 37);
            splitContainer1.SplitterDistance = 799;
            splitContainer1.TabIndex = 3;
            // 
            // WindowsSelect
            // 
            WindowsSelect.Location = new Point(12, 50);
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
            WindowsName.Location = new Point(439, 55);
            WindowsName.Name = "WindowsName";
            WindowsName.Size = new Size(178, 25);
            WindowsName.TabIndex = 5;
            WindowsName.Text = "base windows image";
            // 
            // WindowsClear
            // 
            WindowsClear.Location = new Point(321, 50);
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
            WindowsVersionSelect.Location = new Point(12, 90);
            WindowsVersionSelect.Name = "WindowsVersionSelect";
            WindowsVersionSelect.Size = new Size(303, 33);
            WindowsVersionSelect.TabIndex = 7;
            WindowsVersionSelect.TextChanged += WindowsVersionSelect_TextChanged;
            // 
            // WindowsVersionUpdate
            // 
            WindowsVersionUpdate.Location = new Point(439, 90);
            WindowsVersionUpdate.Name = "WindowsVersionUpdate";
            WindowsVersionUpdate.Size = new Size(112, 34);
            WindowsVersionUpdate.TabIndex = 8;
            WindowsVersionUpdate.Text = "Update";
            WindowsVersionUpdate.UseVisualStyleBackColor = true;
            WindowsVersionUpdate.Click += WindowsVersionUpdate_Click;
            // 
            // WindowsVersionClear
            // 
            WindowsVersionClear.Location = new Point(321, 90);
            WindowsVersionClear.Name = "WindowsVersionClear";
            WindowsVersionClear.Size = new Size(112, 34);
            WindowsVersionClear.TabIndex = 9;
            WindowsVersionClear.Text = "Clear";
            WindowsVersionClear.UseVisualStyleBackColor = true;
            WindowsVersionClear.Click += WindowsVersionClear_Click;
            // 
            // ProcessName
            // 
            ProcessName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ProcessName.AutoSize = true;
            ProcessName.Location = new Point(12, 625);
            ProcessName.Name = "ProcessName";
            ProcessName.Size = new Size(122, 25);
            ProcessName.TabIndex = 10;
            ProcessName.Text = "process name";
            // 
            // WindowsDescription
            // 
            WindowsDescription.BackColor = Color.Silver;
            WindowsDescription.Location = new Point(11, 129);
            WindowsDescription.Name = "WindowsDescription";
            WindowsDescription.ReadOnly = true;
            WindowsDescription.Size = new Size(304, 178);
            WindowsDescription.TabIndex = 11;
            WindowsDescription.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(189, 38);
            label1.TabIndex = 12;
            label1.Text = "Base windows";
            // 
            // EditorForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1002, 712);
            Controls.Add(label1);
            Controls.Add(WindowsDescription);
            Controls.Add(ProcessName);
            Controls.Add(WindowsVersionClear);
            Controls.Add(WindowsVersionUpdate);
            Controls.Add(WindowsVersionSelect);
            Controls.Add(WindowsClear);
            Controls.Add(WindowsName);
            Controls.Add(WindowsSelect);
            Controls.Add(splitContainer1);
            MinimumSize = new Size(800, 400);
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

        private ProgressBar ProcessValue;
        private Button StartBuild;
        private SplitContainer splitContainer1;
        private Button WindowsSelect;
        private Label WindowsName;
        private Button WindowsClear;
        private ComboBox WindowsVersionSelect;
        private Button WindowsVersionUpdate;
        private Button WindowsVersionClear;
        private Label ProcessName;
        private RichTextBox WindowsDescription;
        private Label label1;
    }
}
