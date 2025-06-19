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
            OpenLocalHtml = new Button();
            label9 = new Label();
            WebSessionTimeout = new TextBox();
            label8 = new Label();
            WebSite = new TextBox();
            ProgramType_WebSite = new RadioButton();
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
            label10 = new Label();
            panel3 = new Panel();
            postinstall_reg = new Label();
            postinstall_bat = new Label();
            postinstall_reg_clr = new Button();
            postinstall_reg_sel = new Button();
            postinstall_bat_clr = new Button();
            label12 = new Label();
            label11 = new Label();
            postinstall_bat_sel = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
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
            WindowsName.Location = new Point(789, 106);
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
            ProcessName.Location = new Point(0, 886);
            ProcessName.Margin = new Padding(3, 0, 3, 40);
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
            WinboxDescription.Size = new Size(319, 248);
            WinboxDescription.TabIndex = 11;
            WinboxDescription.Text = "";
            WinboxDescription.TextChanged += WinboxDescription_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 60);
            label1.Name = "label1";
            label1.Size = new Size(684, 38);
            label1.TabIndex = 12;
            label1.Text = "Base windows (recommended Windows 10 Enterprise)";
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
            ExportInstallWim.Location = new Point(1020, 824);
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
            ExportIsoInstaller.Location = new Point(1020, 868);
            ExportIsoInstaller.Margin = new Padding(3, 3, 3, 40);
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
            ProcessValue.Location = new Point(0, 911);
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
            ExportImgPartition.Location = new Point(1020, 780);
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
            panel1.Location = new Point(340, 591);
            panel1.Name = "panel1";
            panel1.Size = new Size(455, 78);
            panel1.TabIndex = 26;
            // 
            // OemKey
            // 
            OemKey.Location = new Point(3, 3);
            OemKey.Name = "OemKey";
            OemKey.Size = new Size(445, 31);
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
            panel2.Controls.Add(OpenLocalHtml);
            panel2.Controls.Add(label9);
            panel2.Controls.Add(WebSessionTimeout);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(WebSite);
            panel2.Controls.Add(ProgramType_WebSite);
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
            panel2.Size = new Size(451, 284);
            panel2.TabIndex = 27;
            // 
            // OpenLocalHtml
            // 
            OpenLocalHtml.Location = new Point(274, 214);
            OpenLocalHtml.Name = "OpenLocalHtml";
            OpenLocalHtml.Size = new Size(62, 31);
            OpenLocalHtml.TabIndex = 14;
            OpenLocalHtml.Text = "html";
            OpenLocalHtml.UseVisualStyleBackColor = true;
            OpenLocalHtml.Click += OpenLocalHtml_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(135, 249);
            label9.Name = "label9";
            label9.Size = new Size(298, 25);
            label9.TabIndex = 13;
            label9.Text = "Session timeout (in minutes 0-1440)";
            // 
            // WebSessionTimeout
            // 
            WebSessionTimeout.Location = new Point(4, 246);
            WebSessionTimeout.Name = "WebSessionTimeout";
            WebSessionTimeout.Size = new Size(125, 31);
            WebSessionTimeout.TabIndex = 12;
            WebSessionTimeout.TextChanged += WebSessionTimeout_TextChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(340, 220);
            label8.Name = "label8";
            label8.Size = new Size(34, 25);
            label8.TabIndex = 11;
            label8.Text = "Url";
            // 
            // WebSite
            // 
            WebSite.Location = new Point(4, 214);
            WebSite.Name = "WebSite";
            WebSite.Size = new Size(264, 31);
            WebSite.TabIndex = 10;
            WebSite.TextChanged += WebSite_TextChanged;
            // 
            // ProgramType_WebSite
            // 
            ProgramType_WebSite.AutoSize = true;
            ProgramType_WebSite.Location = new Point(3, 182);
            ProgramType_WebSite.Margin = new Padding(4);
            ProgramType_WebSite.Name = "ProgramType_WebSite";
            ProgramType_WebSite.Size = new Size(107, 29);
            ProgramType_WebSite.TabIndex = 9;
            ProgramType_WebSite.TabStop = true;
            ProgramType_WebSite.Text = "Web Site";
            ProgramType_WebSite.UseVisualStyleBackColor = true;
            ProgramType_WebSite.CheckedChanged += ProgramType_WebSite_CheckedChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(340, 149);
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
            ProgramType_RawCommand.Location = new Point(4, 111);
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
            ProgramType_ExecutableFile.Location = new Point(3, 3);
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
            label4.Location = new Point(249, 80);
            label4.Name = "label4";
            label4.Size = new Size(100, 25);
            label4.TabIndex = 4;
            label4.Text = "Arguments";
            // 
            // ProgramArgs
            // 
            ProgramArgs.Location = new Point(4, 76);
            ProgramArgs.Name = "ProgramArgs";
            ProgramArgs.Size = new Size(240, 31);
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
            checkedListBox1.Size = new Size(253, 284);
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
            label7.Location = new Point(342, 551);
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
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.Location = new Point(16, 551);
            label10.Name = "label10";
            label10.Size = new Size(149, 38);
            label10.TabIndex = 34;
            label10.Text = "Post install";
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.Window;
            panel3.BorderStyle = BorderStyle.Fixed3D;
            panel3.Controls.Add(postinstall_reg);
            panel3.Controls.Add(postinstall_bat);
            panel3.Controls.Add(postinstall_reg_clr);
            panel3.Controls.Add(postinstall_reg_sel);
            panel3.Controls.Add(postinstall_bat_clr);
            panel3.Controls.Add(label12);
            panel3.Controls.Add(label11);
            panel3.Controls.Add(postinstall_bat_sel);
            panel3.Location = new Point(16, 591);
            panel3.Name = "panel3";
            panel3.Size = new Size(318, 145);
            panel3.TabIndex = 35;
            // 
            // postinstall_reg
            // 
            postinstall_reg.AutoSize = true;
            postinstall_reg.Location = new Point(185, 103);
            postinstall_reg.Name = "postinstall_reg";
            postinstall_reg.Size = new Size(66, 25);
            postinstall_reg.TabIndex = 7;
            postinstall_reg.Text = "reg file";
            // 
            // postinstall_bat
            // 
            postinstall_bat.AutoSize = true;
            postinstall_bat.Location = new Point(185, 38);
            postinstall_bat.Name = "postinstall_bat";
            postinstall_bat.Size = new Size(86, 25);
            postinstall_bat.TabIndex = 6;
            postinstall_bat.Text = "bat script";
            // 
            // postinstall_reg_clr
            // 
            postinstall_reg_clr.Location = new Point(97, 98);
            postinstall_reg_clr.Name = "postinstall_reg_clr";
            postinstall_reg_clr.Size = new Size(82, 34);
            postinstall_reg_clr.TabIndex = 5;
            postinstall_reg_clr.Text = "clear";
            postinstall_reg_clr.UseVisualStyleBackColor = true;
            postinstall_reg_clr.Click += postinstall_reg_clr_Click;
            // 
            // postinstall_reg_sel
            // 
            postinstall_reg_sel.Location = new Point(9, 98);
            postinstall_reg_sel.Name = "postinstall_reg_sel";
            postinstall_reg_sel.Size = new Size(82, 34);
            postinstall_reg_sel.TabIndex = 4;
            postinstall_reg_sel.Text = "select";
            postinstall_reg_sel.UseVisualStyleBackColor = true;
            postinstall_reg_sel.Click += postinstall_reg_sel_Click;
            // 
            // postinstall_bat_clr
            // 
            postinstall_bat_clr.Location = new Point(97, 33);
            postinstall_bat_clr.Name = "postinstall_bat_clr";
            postinstall_bat_clr.Size = new Size(82, 34);
            postinstall_bat_clr.TabIndex = 3;
            postinstall_bat_clr.Text = "clear";
            postinstall_bat_clr.UseVisualStyleBackColor = true;
            postinstall_bat_clr.Click += postinstall_bat_clr_Click;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(9, 70);
            label12.Name = "label12";
            label12.Size = new Size(70, 25);
            label12.TabIndex = 2;
            label12.Text = "Reg file";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(6, 5);
            label11.Name = "label11";
            label11.Size = new Size(85, 25);
            label11.TabIndex = 1;
            label11.Text = "Bat script";
            // 
            // postinstall_bat_sel
            // 
            postinstall_bat_sel.Location = new Point(9, 33);
            postinstall_bat_sel.Name = "postinstall_bat_sel";
            postinstall_bat_sel.Size = new Size(82, 34);
            postinstall_bat_sel.TabIndex = 0;
            postinstall_bat_sel.Text = "select";
            postinstall_bat_sel.UseVisualStyleBackColor = true;
            postinstall_bat_sel.Click += postinstall_bat_sel_Click;
            // 
            // EditorForm
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScroll = true;
            AutoScrollMargin = new Size(10, 30);
            BackColor = SystemColors.Control;
            ClientSize = new Size(1258, 944);
            Controls.Add(panel3);
            Controls.Add(label10);
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
            MinimumSize = new Size(834, 347);
            Name = "EditorForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Editor";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
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
        private RadioButton ProgramType_WebSite;
        private Label label7;
        private Button button1;
        private TextBox WebSite;
        private Label label8;
        private TextBox WebSessionTimeout;
        private Label label9;
        private Button OpenLocalHtml;
        private Label label10;
        private Panel panel3;
        private Button postinstall_bat_sel;
        private Label label11;
        private Label label12;
        private Button postinstall_bat_clr;
        private Button postinstall_reg_sel;
        private Button postinstall_reg_clr;
        private Label postinstall_reg;
        private Label postinstall_bat;
    }
}
