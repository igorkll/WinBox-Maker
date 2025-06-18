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
            WinboxDescription = new RichTextBox();
            label1 = new Label();
            label2 = new Label();
            WinboxName = new TextBox();
            WindowsDescription = new RichTextBox();
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
            ProgramType_WebPage = new RadioButton();
            label6 = new Label();
            RawCommand = new TextBox();
            ProgramType_RawCommand = new RadioButton();
            ProgramType_ExecutableFile = new RadioButton();
            label4 = new Label();
            ProgramArgs = new TextBox();
            ProgramName = new Label();
            AppClear = new Button();
            AppSelect = new Button();
            label3 = new Label();
            checkedListBox1 = new CheckedListBox();
            label5 = new Label();
            OpenProjectFolder = new Button();
            label7 = new Label();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // WindowsSelect
            // 
            WindowsSelect.Location = new Point(342, 100);
            WindowsSelect.Name = "WindowsSelect";
            WindowsSelect.Size = new Size(318, 38);
            WindowsSelect.TabIndex = 4;
            WindowsSelect.Text = "Select base windows image";
            WindowsSelect.UseVisualStyleBackColor = true;
            WindowsSelect.Click += WindowsSelect_Click;
            // 
            // WindowsName
            // 
            WindowsName.AutoSize = true;
            WindowsName.Location = new Point(789, 107);
            WindowsName.Name = "WindowsName";
            WindowsName.Size = new Size(178, 25);
            WindowsName.TabIndex = 5;
            WindowsName.Text = "base windows image";
            // 
            // WindowsClear
            // 
            WindowsClear.Location = new Point(666, 100);
            WindowsClear.Name = "WindowsClear";
            WindowsClear.Size = new Size(117, 38);
            WindowsClear.TabIndex = 6;
            WindowsClear.Text = "Clear";
            WindowsClear.UseVisualStyleBackColor = true;
            WindowsClear.Click += WindowsClear_Click;
            // 
            // WindowsVersionSelect
            // 
            WindowsVersionSelect.FormattingEnabled = true;
            WindowsVersionSelect.Location = new Point(342, 144);
            WindowsVersionSelect.Name = "WindowsVersionSelect";
            WindowsVersionSelect.Size = new Size(318, 33);
            WindowsVersionSelect.TabIndex = 7;
            WindowsVersionSelect.TextChanged += WindowsVersionSelect_TextChanged;
            // 
            // WindowsVersionUpdate
            // 
            WindowsVersionUpdate.Location = new Point(789, 144);
            WindowsVersionUpdate.Name = "WindowsVersionUpdate";
            WindowsVersionUpdate.Size = new Size(117, 33);
            WindowsVersionUpdate.TabIndex = 8;
            WindowsVersionUpdate.Text = "Update";
            WindowsVersionUpdate.UseVisualStyleBackColor = true;
            WindowsVersionUpdate.Click += WindowsVersionUpdate_Click;
            // 
            // WindowsVersionClear
            // 
            WindowsVersionClear.Location = new Point(666, 144);
            WindowsVersionClear.Name = "WindowsVersionClear";
            WindowsVersionClear.Size = new Size(117, 33);
            WindowsVersionClear.TabIndex = 9;
            WindowsVersionClear.Text = "Clear";
            WindowsVersionClear.UseVisualStyleBackColor = true;
            WindowsVersionClear.Click += WindowsVersionClear_Click;
            // 
            // ProcessName
            // 
            ProcessName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ProcessName.AutoSize = true;
            ProcessName.Location = new Point(0, 685);
            ProcessName.Name = "ProcessName";
            ProcessName.Size = new Size(122, 25);
            ProcessName.TabIndex = 10;
            ProcessName.Text = "process name";
            // 
            // WinboxDescription
            // 
            WinboxDescription.BackColor = SystemColors.Window;
            WinboxDescription.Location = new Point(15, 300);
            WinboxDescription.Name = "WinboxDescription";
            WinboxDescription.Size = new Size(319, 220);
            WinboxDescription.TabIndex = 11;
            WinboxDescription.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 60);
            label1.Name = "label1";
            label1.Size = new Size(602, 38);
            label1.TabIndex = 12;
            label1.Text = "Base windows (recommended Windows 10 Pro)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 224);
            label2.Name = "label2";
            label2.Size = new Size(188, 38);
            label2.TabIndex = 13;
            label2.Text = "New windows";
            // 
            // WinboxName
            // 
            WinboxName.Location = new Point(16, 264);
            WinboxName.Name = "WinboxName";
            WinboxName.Size = new Size(318, 31);
            WinboxName.TabIndex = 14;
            WinboxName.TextChanged += WinboxName_TextChanged;
            // 
            // WindowsDescription
            // 
            WindowsDescription.Location = new Point(16, 100);
            WindowsDescription.Name = "WindowsDescription";
            WindowsDescription.ReadOnly = true;
            WindowsDescription.Size = new Size(318, 118);
            WindowsDescription.TabIndex = 15;
            WindowsDescription.Text = "";
            WindowsDescription.TextChanged += WinboxDescription_TextChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(1146, 16);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(96, 96);
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
            pictureBox2.Location = new Point(1146, 118);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(96, 96);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 18;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // ExportInstallWim
            // 
            ExportInstallWim.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ExportInstallWim.Location = new Point(1020, 624);
            ExportInstallWim.Name = "ExportInstallWim";
            ExportInstallWim.Size = new Size(222, 38);
            ExportInstallWim.TabIndex = 19;
            ExportInstallWim.Text = "export install.wim";
            ExportInstallWim.UseVisualStyleBackColor = true;
            ExportInstallWim.Click += ExportInstallWim_Click;
            // 
            // ExportIsoInstaller
            // 
            ExportIsoInstaller.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ExportIsoInstaller.Location = new Point(1020, 667);
            ExportIsoInstaller.Name = "ExportIsoInstaller";
            ExportIsoInstaller.Size = new Size(222, 38);
            ExportIsoInstaller.TabIndex = 20;
            ExportIsoInstaller.Text = "export .iso installer";
            ExportIsoInstaller.UseVisualStyleBackColor = true;
            ExportIsoInstaller.Click += ExportIsoInstaller_Click;
            // 
            // ProcessValue
            // 
            ProcessValue.Dock = DockStyle.Bottom;
            ProcessValue.Location = new Point(0, 711);
            ProcessValue.Name = "ProcessValue";
            ProcessValue.Size = new Size(1258, 33);
            ProcessValue.TabIndex = 21;
            // 
            // back
            // 
            back.Location = new Point(18, 12);
            back.Name = "back";
            back.Size = new Size(117, 45);
            back.TabIndex = 22;
            back.Text = "< back";
            back.UseVisualStyleBackColor = true;
            back.Click += back_Click;
            // 
            // README
            // 
            README.Location = new Point(138, 12);
            README.Name = "README";
            README.Size = new Size(117, 45);
            README.TabIndex = 23;
            README.Text = "README";
            README.UseVisualStyleBackColor = true;
            README.Click += README_Click;
            // 
            // LICENSE
            // 
            LICENSE.Location = new Point(261, 12);
            LICENSE.Name = "LICENSE";
            LICENSE.Size = new Size(117, 45);
            LICENSE.TabIndex = 24;
            LICENSE.Text = "LICENSE";
            LICENSE.UseVisualStyleBackColor = true;
            LICENSE.Click += LICENSE_Click;
            // 
            // ExportImgPartition
            // 
            ExportImgPartition.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ExportImgPartition.Location = new Point(1020, 580);
            ExportImgPartition.Name = "ExportImgPartition";
            ExportImgPartition.Size = new Size(222, 38);
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
            panel1.Location = new Point(15, 564);
            panel1.Name = "panel1";
            panel1.Size = new Size(451, 78);
            panel1.TabIndex = 26;
            // 
            // OemKey
            // 
            OemKey.Location = new Point(3, 3);
            OemKey.Name = "OemKey";
            OemKey.Size = new Size(439, 31);
            OemKey.TabIndex = 1;
            OemKey.TextChanged += OemKey_TextChanged;
            // 
            // UseOemKey
            // 
            UseOemKey.AutoSize = true;
            UseOemKey.Location = new Point(3, 44);
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
            panel2.Controls.Add(ProgramType_WebPage);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(RawCommand);
            panel2.Controls.Add(ProgramType_RawCommand);
            panel2.Controls.Add(ProgramType_ExecutableFile);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(ProgramArgs);
            panel2.Controls.Add(ProgramName);
            panel2.Controls.Add(AppClear);
            panel2.Controls.Add(AppSelect);
            panel2.Location = new Point(344, 264);
            panel2.Name = "panel2";
            panel2.Size = new Size(451, 256);
            panel2.TabIndex = 27;
            // 
            // ProgramType_WebPage
            // 
            ProgramType_WebPage.AutoSize = true;
            ProgramType_WebPage.Location = new Point(4, 188);
            ProgramType_WebPage.Margin = new Padding(4);
            ProgramType_WebPage.Name = "ProgramType_WebPage";
            ProgramType_WebPage.Size = new Size(116, 29);
            ProgramType_WebPage.TabIndex = 9;
            ProgramType_WebPage.TabStop = true;
            ProgramType_WebPage.Text = "Web Page";
            ProgramType_WebPage.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(341, 149);
            label6.Name = "label6";
            label6.Size = new Size(96, 25);
            label6.TabIndex = 8;
            label6.Text = "Command";
            // 
            // RawCommand
            // 
            RawCommand.Location = new Point(4, 146);
            RawCommand.Name = "RawCommand";
            RawCommand.Size = new Size(332, 31);
            RawCommand.TabIndex = 7;
            RawCommand.TextChanged += RawCommand_TextChanged;
            // 
            // ProgramType_RawCommand
            // 
            ProgramType_RawCommand.AutoSize = true;
            ProgramType_RawCommand.Location = new Point(4, 117);
            ProgramType_RawCommand.Name = "ProgramType_RawCommand";
            ProgramType_RawCommand.Size = new Size(159, 29);
            ProgramType_RawCommand.TabIndex = 6;
            ProgramType_RawCommand.TabStop = true;
            ProgramType_RawCommand.Text = "Raw Command";
            ProgramType_RawCommand.UseVisualStyleBackColor = true;
            ProgramType_RawCommand.CheckedChanged += ProgramType_RawCommand_CheckedChanged;
            // 
            // ProgramType_ExecutableFile
            // 
            ProgramType_ExecutableFile.AutoSize = true;
            ProgramType_ExecutableFile.Location = new Point(4, 3);
            ProgramType_ExecutableFile.Name = "ProgramType_ExecutableFile";
            ProgramType_ExecutableFile.Size = new Size(151, 29);
            ProgramType_ExecutableFile.TabIndex = 5;
            ProgramType_ExecutableFile.TabStop = true;
            ProgramType_ExecutableFile.Text = "Executable File";
            ProgramType_ExecutableFile.UseVisualStyleBackColor = true;
            ProgramType_ExecutableFile.CheckedChanged += ProgramType_ExecutableFile_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(249, 79);
            label4.Name = "label4";
            label4.Size = new Size(100, 25);
            label4.TabIndex = 4;
            label4.Text = "Arguments";
            // 
            // ProgramArgs
            // 
            ProgramArgs.Location = new Point(4, 76);
            ProgramArgs.Name = "ProgramArgs";
            ProgramArgs.Size = new Size(239, 31);
            ProgramArgs.TabIndex = 3;
            ProgramArgs.TextChanged += ProgramArgs_TextChanged;
            // 
            // ProgramName
            // 
            ProgramName.AutoSize = true;
            ProgramName.Location = new Point(249, 40);
            ProgramName.Name = "ProgramName";
            ProgramName.Size = new Size(113, 25);
            ProgramName.TabIndex = 2;
            ProgramName.Text = "program exe";
            // 
            // AppClear
            // 
            AppClear.Location = new Point(126, 33);
            AppClear.Name = "AppClear";
            AppClear.Size = new Size(117, 38);
            AppClear.TabIndex = 1;
            AppClear.Text = "clear";
            AppClear.UseVisualStyleBackColor = true;
            AppClear.Click += AppClear_Click;
            // 
            // AppSelect
            // 
            AppSelect.Location = new Point(3, 33);
            AppSelect.Name = "AppSelect";
            AppSelect.Size = new Size(117, 38);
            AppSelect.TabIndex = 0;
            AppSelect.Text = "select";
            AppSelect.UseVisualStyleBackColor = true;
            AppSelect.Click += AppSelect_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(342, 225);
            label3.Name = "label3";
            label3.Size = new Size(205, 38);
            label3.TabIndex = 28;
            label3.Text = "You application";
            // 
            // checkedListBox1
            // 
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new Point(802, 264);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(253, 256);
            checkedListBox1.TabIndex = 29;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(802, 224);
            label5.Name = "label5";
            label5.Size = new Size(116, 38);
            label5.TabIndex = 30;
            label5.Text = "Settings";
            // 
            // OpenProjectFolder
            // 
            OpenProjectFolder.Location = new Point(384, 12);
            OpenProjectFolder.Name = "OpenProjectFolder";
            OpenProjectFolder.Size = new Size(246, 45);
            OpenProjectFolder.TabIndex = 31;
            OpenProjectFolder.Text = "Open Project Folder";
            OpenProjectFolder.UseVisualStyleBackColor = true;
            OpenProjectFolder.Click += OpenProjectFolder_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 14F);
            label7.Location = new Point(12, 523);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(139, 38);
            label7.TabIndex = 32;
            label7.Text = "Activation";
            // 
            // button1
            // 
            button1.Location = new Point(342, 184);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 33;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // EditorForm
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScroll = true;
            AutoScrollMargin = new Size(10, 30);
            BackColor = SystemColors.Control;
            ClientSize = new Size(1258, 744);
            Controls.Add(button1);
            Controls.Add(label7);
            Controls.Add(OpenProjectFolder);
            Controls.Add(label5);
            Controls.Add(checkedListBox1);
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
            Controls.Add(WindowsDescription);
            Controls.Add(WinboxName);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(WinboxDescription);
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
            MinimumSize = new Size(836, 356);
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
        private RichTextBox WinboxDescription;
        private Label label1;
        private Label label2;
        private TextBox WinboxName;
        private RichTextBox WindowsDescription;
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
        private CheckedListBox checkedListBox1;
        private Label label5;
        private VScrollBar vScrollBar1;
        private RadioButton ProgramType_ExecutableFile;
        private RadioButton ProgramType_RawCommand;
        private TextBox RawCommand;
        private Label label6;
        private Button OpenProjectFolder;
        private RadioButton ProgramType_WebPage;
        private Label label7;
        private Button button1;
    }
}
