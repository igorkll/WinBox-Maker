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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            WindowsSelect = new Button();
            WindowsName = new Label();
            WindowsClear = new Button();
            WindowsVersionSelect = new ComboBox();
            WindowsVersionUpdate = new Button();
            WindowsVersionClear = new Button();
            ProcessName = new Label();
            WindowsDescription = new RichTextBox();
            label1 = new Label();
            label2 = new Label();
            WinboxName = new TextBox();
            WinboxDescription = new RichTextBox();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            ExportInstallWim = new Button();
            ExportIsoInstaller = new Button();
            ProcessValue = new ProgressBar();
            back = new Button();
            README = new Button();
            LICENSE = new Button();
            ExportImgPartition = new Button();
            panel1 = new Panel();
            OemKey = new TextBox();
            UseOemKey = new CheckBox();
            panel2 = new Panel();
            ProgramAsAdmin = new CheckBox();
            label4 = new Label();
            ProgramArgs = new TextBox();
            ProgramName = new Label();
            AppClear = new Button();
            AppSelect = new Button();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // WindowsSelect
            // 
            WindowsSelect.Location = new Point(12, 90);
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
            WindowsName.Location = new Point(439, 95);
            WindowsName.Name = "WindowsName";
            WindowsName.Size = new Size(178, 25);
            WindowsName.TabIndex = 5;
            WindowsName.Text = "base windows image";
            // 
            // WindowsClear
            // 
            WindowsClear.Location = new Point(321, 90);
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
            WindowsVersionSelect.Location = new Point(12, 130);
            WindowsVersionSelect.Name = "WindowsVersionSelect";
            WindowsVersionSelect.Size = new Size(303, 33);
            WindowsVersionSelect.TabIndex = 7;
            WindowsVersionSelect.TextChanged += WindowsVersionSelect_TextChanged;
            // 
            // WindowsVersionUpdate
            // 
            WindowsVersionUpdate.Location = new Point(439, 130);
            WindowsVersionUpdate.Name = "WindowsVersionUpdate";
            WindowsVersionUpdate.Size = new Size(112, 34);
            WindowsVersionUpdate.TabIndex = 8;
            WindowsVersionUpdate.Text = "Update";
            WindowsVersionUpdate.UseVisualStyleBackColor = true;
            WindowsVersionUpdate.Click += WindowsVersionUpdate_Click;
            // 
            // WindowsVersionClear
            // 
            WindowsVersionClear.Location = new Point(321, 130);
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
            ProcessName.Location = new Point(0, 647);
            ProcessName.Name = "ProcessName";
            ProcessName.Size = new Size(122, 25);
            ProcessName.TabIndex = 10;
            ProcessName.Text = "process name";
            // 
            // WindowsDescription
            // 
            WindowsDescription.BackColor = SystemColors.Window;
            WindowsDescription.Location = new Point(11, 169);
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
            label1.Location = new Point(12, 49);
            label1.Name = "label1";
            label1.Size = new Size(602, 38);
            label1.TabIndex = 12;
            label1.Text = "Base windows (recommended Windows 10 Pro)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 350);
            label2.Name = "label2";
            label2.Size = new Size(188, 38);
            label2.TabIndex = 13;
            label2.Text = "New windows";
            // 
            // WinboxName
            // 
            WinboxName.Location = new Point(12, 391);
            WinboxName.Name = "WinboxName";
            WinboxName.Size = new Size(303, 31);
            WinboxName.TabIndex = 14;
            WinboxName.TextChanged += WinboxName_TextChanged;
            // 
            // WinboxDescription
            // 
            WinboxDescription.Location = new Point(11, 428);
            WinboxDescription.Name = "WinboxDescription";
            WinboxDescription.Size = new Size(303, 178);
            WinboxDescription.TabIndex = 15;
            WinboxDescription.Text = "";
            WinboxDescription.TextChanged += WinboxDescription_TextChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(1118, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(128, 128);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 17;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(1118, 146);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(128, 128);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 18;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // ExportInstallWim
            // 
            ExportInstallWim.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ExportInstallWim.Location = new Point(1023, 589);
            ExportInstallWim.Name = "ExportInstallWim";
            ExportInstallWim.Size = new Size(223, 37);
            ExportInstallWim.TabIndex = 19;
            ExportInstallWim.Text = "export install.wim";
            ExportInstallWim.UseVisualStyleBackColor = true;
            ExportInstallWim.Click += ExportInstallWim_Click;
            // 
            // ExportIsoInstaller
            // 
            ExportIsoInstaller.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ExportIsoInstaller.Location = new Point(1023, 632);
            ExportIsoInstaller.Name = "ExportIsoInstaller";
            ExportIsoInstaller.Size = new Size(223, 37);
            ExportIsoInstaller.TabIndex = 20;
            ExportIsoInstaller.Text = "export .iso installer";
            ExportIsoInstaller.UseVisualStyleBackColor = true;
            ExportIsoInstaller.Click += ExportIsoInstaller_Click;
            // 
            // ProcessValue
            // 
            ProcessValue.Dock = DockStyle.Bottom;
            ProcessValue.Location = new Point(0, 675);
            ProcessValue.Name = "ProcessValue";
            ProcessValue.Size = new Size(1258, 37);
            ProcessValue.TabIndex = 21;
            // 
            // back
            // 
            back.Location = new Point(12, 12);
            back.Name = "back";
            back.Size = new Size(112, 34);
            back.TabIndex = 22;
            back.Text = "< back";
            back.UseVisualStyleBackColor = true;
            back.Click += back_Click;
            // 
            // README
            // 
            README.Location = new Point(130, 12);
            README.Name = "README";
            README.Size = new Size(112, 34);
            README.TabIndex = 23;
            README.Text = "README";
            README.UseVisualStyleBackColor = true;
            README.Click += README_Click;
            // 
            // LICENSE
            // 
            LICENSE.Location = new Point(248, 12);
            LICENSE.Name = "LICENSE";
            LICENSE.Size = new Size(112, 34);
            LICENSE.TabIndex = 24;
            LICENSE.Text = "LICENSE";
            LICENSE.UseVisualStyleBackColor = true;
            LICENSE.Click += LICENSE_Click;
            // 
            // ExportImgPartition
            // 
            ExportImgPartition.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ExportImgPartition.Location = new Point(1023, 549);
            ExportImgPartition.Name = "ExportImgPartition";
            ExportImgPartition.Size = new Size(223, 34);
            ExportImgPartition.TabIndex = 25;
            ExportImgPartition.Text = "export .img partition";
            ExportImgPartition.UseVisualStyleBackColor = true;
            ExportImgPartition.Click += ExportImgPartition_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Window;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(OemKey);
            panel1.Controls.Add(UseOemKey);
            panel1.Location = new Point(321, 169);
            panel1.Name = "panel1";
            panel1.Size = new Size(434, 75);
            panel1.TabIndex = 26;
            // 
            // OemKey
            // 
            OemKey.Location = new Point(3, 3);
            OemKey.Name = "OemKey";
            OemKey.Size = new Size(424, 31);
            OemKey.TabIndex = 1;
            OemKey.TextChanged += OemKey_TextChanged;
            // 
            // UseOemKey
            // 
            UseOemKey.AutoSize = true;
            UseOemKey.Location = new Point(3, 40);
            UseOemKey.Name = "UseOemKey";
            UseOemKey.Size = new Size(347, 29);
            UseOemKey.TabIndex = 0;
            UseOemKey.Text = "Activate windows with this product key";
            UseOemKey.UseVisualStyleBackColor = true;
            UseOemKey.CheckedChanged += UseOemKey_CheckedChanged;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.Window;
            panel2.BorderStyle = BorderStyle.Fixed3D;
            panel2.Controls.Add(ProgramAsAdmin);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(ProgramArgs);
            panel2.Controls.Add(ProgramName);
            panel2.Controls.Add(AppClear);
            panel2.Controls.Add(AppSelect);
            panel2.Location = new Point(326, 391);
            panel2.Name = "panel2";
            panel2.Size = new Size(429, 215);
            panel2.TabIndex = 27;
            // 
            // ProgramAsAdmin
            // 
            ProgramAsAdmin.AutoSize = true;
            ProgramAsAdmin.Location = new Point(3, 80);
            ProgramAsAdmin.Name = "ProgramAsAdmin";
            ProgramAsAdmin.Size = new Size(226, 29);
            ProgramAsAdmin.TabIndex = 5;
            ProgramAsAdmin.Text = "Run as an administrator";
            ProgramAsAdmin.UseVisualStyleBackColor = true;
            ProgramAsAdmin.CheckedChanged += ProgramAsAdmin_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(239, 49);
            label4.Name = "label4";
            label4.Size = new Size(100, 25);
            label4.TabIndex = 4;
            label4.Text = "Arguments";
            // 
            // ProgramArgs
            // 
            ProgramArgs.Location = new Point(3, 43);
            ProgramArgs.Name = "ProgramArgs";
            ProgramArgs.Size = new Size(230, 31);
            ProgramArgs.TabIndex = 3;
            ProgramArgs.TextChanged += ProgramArgs_TextChanged;
            // 
            // ProgramName
            // 
            ProgramName.AutoSize = true;
            ProgramName.Location = new Point(239, 8);
            ProgramName.Name = "ProgramName";
            ProgramName.Size = new Size(113, 25);
            ProgramName.TabIndex = 2;
            ProgramName.Text = "program exe";
            // 
            // AppClear
            // 
            AppClear.Location = new Point(121, 3);
            AppClear.Name = "AppClear";
            AppClear.Size = new Size(112, 34);
            AppClear.TabIndex = 1;
            AppClear.Text = "clear";
            AppClear.UseVisualStyleBackColor = true;
            // 
            // AppSelect
            // 
            AppSelect.Location = new Point(3, 3);
            AppSelect.Name = "AppSelect";
            AppSelect.Size = new Size(112, 34);
            AppSelect.TabIndex = 0;
            AppSelect.Text = "select";
            AppSelect.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(326, 350);
            label3.Name = "label3";
            label3.Size = new Size(205, 38);
            label3.TabIndex = 28;
            label3.Text = "You application";
            // 
            // EditorForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1258, 712);
            Controls.Add(label3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(ExportImgPartition);
            Controls.Add(LICENSE);
            Controls.Add(README);
            Controls.Add(back);
            Controls.Add(ExportInstallWim);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(WinboxDescription);
            Controls.Add(WinboxName);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(WindowsDescription);
            Controls.Add(ProcessName);
            Controls.Add(WindowsVersionClear);
            Controls.Add(WindowsVersionUpdate);
            Controls.Add(WindowsVersionSelect);
            Controls.Add(WindowsClear);
            Controls.Add(WindowsName);
            Controls.Add(WindowsSelect);
            Controls.Add(ProcessValue);
            Controls.Add(ExportIsoInstaller);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(800, 400);
            Name = "EditorForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Editor";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button WindowsSelect;
        private Label WindowsName;
        private Button WindowsClear;
        private ComboBox WindowsVersionSelect;
        private Button WindowsVersionUpdate;
        private Button WindowsVersionClear;
        private Label ProcessName;
        private RichTextBox WindowsDescription;
        private Label label1;
        private Label label2;
        private TextBox WinboxName;
        private RichTextBox WinboxDescription;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Button ExportInstallWim;
        private Button ExportIsoInstaller;
        private ProgressBar ProcessValue;
        private Button back;
        private Button README;
        private Button LICENSE;
        private Button ExportImgPartition;
        private Panel panel1;
        private CheckBox UseOemKey;
        private TextBox OemKey;
        private Panel panel2;
        private Label label3;
        private Button AppClear;
        private Button AppSelect;
        private Label ProgramName;
        private Label label4;
        private TextBox ProgramArgs;
        private CheckBox ProgramAsAdmin;
    }
}
