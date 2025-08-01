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
            components = new System.ComponentModel.Container();
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
            label5 = new Label();
            OpenProjectFolder = new Button();
            label7 = new Label();
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
            CustomBootLogo = new Label();
            CustomBootLogo_clear = new Button();
            CustomBootLogo_select = new Button();
            label15 = new Label();
            panel4 = new Panel();
            CustomBootLogo_centering = new CheckBox();
            label13 = new Label();
            ScreenTimeout = new TextBox();
            TweakList = new CheckedListBox();
            ArchitectureSelect = new ComboBox();
            label14 = new Label();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            label16 = new Label();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();
            tabPage5 = new TabPage();
            panel5 = new Panel();
            postinstall_user_reg = new Label();
            label21 = new Label();
            postinstall_user_bat = new Label();
            postinstall_user_bat_sel = new Button();
            postinstall_user_reg_clr = new Button();
            label20 = new Label();
            postinstall_user_reg_sel = new Button();
            postinstall_user_bat_clr = new Button();
            label17 = new Label();
            tabPage6 = new TabPage();
            tabPage7 = new TabPage();
            panel7 = new Panel();
            EmbedDisplayReadme = new Button();
            UseEmbeddedDisplay = new CheckBox();
            panel6 = new Panel();
            label19 = new Label();
            label18 = new Label();
            VirtualDisplayHeight = new TextBox();
            VirtualDisplayWidth = new TextBox();
            AddVirtualDisplay = new CheckBox();
            tabPage8 = new TabPage();
            panel9 = new Panel();
            postbuildEnabled = new CheckBox();
            postbuildEvent = new RichTextBox();
            label23 = new Label();
            panel8 = new Panel();
            prebuildEnabled = new CheckBox();
            prebuildEvent = new RichTextBox();
            label22 = new Label();
            OpenEmbeddedFolder = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage5.SuspendLayout();
            panel5.SuspendLayout();
            tabPage6.SuspendLayout();
            tabPage7.SuspendLayout();
            panel7.SuspendLayout();
            panel6.SuspendLayout();
            tabPage8.SuspendLayout();
            panel9.SuspendLayout();
            panel8.SuspendLayout();
            SuspendLayout();
            // 
            // WindowsSelect
            // 
            WindowsSelect.Location = new Point(6, 44);
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
            WindowsName.Location = new Point(453, 51);
            WindowsName.Name = "WindowsName";
            WindowsName.Size = new Size(178, 25);
            WindowsName.TabIndex = 5;
            WindowsName.Text = "base windows image";
            // 
            // WindowsClear
            // 
            WindowsClear.Location = new Point(330, 44);
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
            WindowsVersionSelect.Location = new Point(6, 88);
            WindowsVersionSelect.Name = "WindowsVersionSelect";
            WindowsVersionSelect.Size = new Size(318, 33);
            WindowsVersionSelect.TabIndex = 7;
            WindowsVersionSelect.TextChanged += WindowsVersionSelect_TextChanged;
            // 
            // WindowsVersionUpdate
            // 
            WindowsVersionUpdate.Location = new Point(453, 87);
            WindowsVersionUpdate.Name = "WindowsVersionUpdate";
            WindowsVersionUpdate.Size = new Size(117, 33);
            WindowsVersionUpdate.TabIndex = 8;
            WindowsVersionUpdate.Text = "Update";
            WindowsVersionUpdate.UseVisualStyleBackColor = true;
            WindowsVersionUpdate.Click += WindowsVersionUpdate_Click;
            // 
            // WindowsVersionClear
            // 
            WindowsVersionClear.Location = new Point(330, 88);
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
            ProcessName.ForeColor = SystemColors.Window;
            ProcessName.Location = new Point(0, 585);
            ProcessName.Margin = new Padding(3, 0, 3, 40);
            ProcessName.Name = "ProcessName";
            ProcessName.Size = new Size(122, 25);
            ProcessName.TabIndex = 10;
            ProcessName.Text = "process name";
            // 
            // WinboxDescription
            // 
            WinboxDescription.BackColor = SystemColors.Window;
            WinboxDescription.Location = new Point(6, 81);
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
            label1.Location = new Point(6, 3);
            label1.Name = "label1";
            label1.Size = new Size(189, 38);
            label1.TabIndex = 12;
            label1.Text = "Base windows";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(6, 3);
            label2.Name = "label2";
            label2.Size = new Size(333, 38);
            label2.TabIndex = 13;
            label2.Text = "New windows description";
            // 
            // WinboxName
            // 
            WinboxName.Location = new Point(6, 44);
            WinboxName.Name = "WinboxName";
            WinboxName.Size = new Size(318, 31);
            WinboxName.TabIndex = 14;
            WinboxName.TextChanged += WinboxName_TextChanged;
            // 
            // WindowsDescription
            // 
            WindowsDescription.BackColor = SystemColors.Window;
            WindowsDescription.Location = new Point(6, 166);
            WindowsDescription.Name = "WindowsDescription";
            WindowsDescription.ReadOnly = true;
            WindowsDescription.Size = new Size(318, 118);
            WindowsDescription.TabIndex = 15;
            WindowsDescription.Text = "";
            WindowsDescription.TextChanged += WindowsDescription_TextChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(1066, 16);
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
            pictureBox2.Location = new Point(1066, 118);
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
            ExportInstallWim.Location = new Point(940, 523);
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
            ExportIsoInstaller.Location = new Point(940, 567);
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
            ProcessValue.Location = new Point(0, 611);
            ProcessValue.Name = "ProcessValue";
            ProcessValue.Size = new Size(1178, 33);
            ProcessValue.TabIndex = 21;
            // 
            // back
            // 
            back.Location = new Point(12, 12);
            back.Name = "back";
            back.Size = new Size(117, 45);
            back.TabIndex = 22;
            back.Text = "< back";
            back.UseVisualStyleBackColor = true;
            back.Click += back_Click;
            // 
            // README
            // 
            README.Location = new Point(135, 12);
            README.Name = "README";
            README.Size = new Size(117, 45);
            README.TabIndex = 23;
            README.Text = "README";
            README.UseVisualStyleBackColor = true;
            README.Click += README_Click;
            // 
            // LICENSE
            // 
            LICENSE.Location = new Point(258, 12);
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
            ExportImgPartition.Location = new Point(940, 479);
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
            panel1.Location = new Point(6, 44);
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
            panel2.Location = new Point(6, 44);
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
            label3.Location = new Point(6, 3);
            label3.Name = "label3";
            label3.Size = new Size(205, 38);
            label3.TabIndex = 28;
            label3.Text = "You application";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(6, 3);
            label5.Name = "label5";
            label5.Size = new Size(116, 38);
            label5.TabIndex = 30;
            label5.Text = "Settings";
            // 
            // OpenProjectFolder
            // 
            OpenProjectFolder.Location = new Point(381, 12);
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
            label7.Location = new Point(5, 3);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(139, 38);
            label7.TabIndex = 32;
            label7.Text = "Activation";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.Location = new Point(3, 3);
            label10.Name = "label10";
            label10.Size = new Size(439, 38);
            label10.TabIndex = 34;
            label10.Text = "System user (SetupComplete.cmd)";
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
            panel3.Location = new Point(6, 44);
            panel3.Name = "panel3";
            panel3.Size = new Size(446, 142);
            panel3.TabIndex = 35;
            // 
            // postinstall_reg
            // 
            postinstall_reg.AutoSize = true;
            postinstall_reg.Location = new Point(179, 103);
            postinstall_reg.Name = "postinstall_reg";
            postinstall_reg.Size = new Size(66, 25);
            postinstall_reg.TabIndex = 7;
            postinstall_reg.Text = "reg file";
            // 
            // postinstall_bat
            // 
            postinstall_bat.AutoSize = true;
            postinstall_bat.Location = new Point(179, 38);
            postinstall_bat.Name = "postinstall_bat";
            postinstall_bat.Size = new Size(86, 25);
            postinstall_bat.TabIndex = 6;
            postinstall_bat.Text = "bat script";
            // 
            // postinstall_reg_clr
            // 
            postinstall_reg_clr.Location = new Point(91, 98);
            postinstall_reg_clr.Name = "postinstall_reg_clr";
            postinstall_reg_clr.Size = new Size(82, 34);
            postinstall_reg_clr.TabIndex = 5;
            postinstall_reg_clr.Text = "clear";
            postinstall_reg_clr.UseVisualStyleBackColor = true;
            postinstall_reg_clr.Click += postinstall_reg_clr_Click;
            // 
            // postinstall_reg_sel
            // 
            postinstall_reg_sel.Location = new Point(3, 98);
            postinstall_reg_sel.Name = "postinstall_reg_sel";
            postinstall_reg_sel.Size = new Size(82, 34);
            postinstall_reg_sel.TabIndex = 4;
            postinstall_reg_sel.Text = "select";
            postinstall_reg_sel.UseVisualStyleBackColor = true;
            postinstall_reg_sel.Click += postinstall_reg_sel_Click;
            // 
            // postinstall_bat_clr
            // 
            postinstall_bat_clr.Location = new Point(91, 33);
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
            label12.Location = new Point(3, 70);
            label12.Name = "label12";
            label12.Size = new Size(70, 25);
            label12.TabIndex = 2;
            label12.Text = "Reg file";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(0, 5);
            label11.Name = "label11";
            label11.Size = new Size(85, 25);
            label11.TabIndex = 1;
            label11.Text = "Bat script";
            // 
            // postinstall_bat_sel
            // 
            postinstall_bat_sel.Location = new Point(3, 33);
            postinstall_bat_sel.Name = "postinstall_bat_sel";
            postinstall_bat_sel.Size = new Size(82, 34);
            postinstall_bat_sel.TabIndex = 0;
            postinstall_bat_sel.Text = "select";
            postinstall_bat_sel.UseVisualStyleBackColor = true;
            postinstall_bat_sel.Click += postinstall_bat_sel_Click;
            // 
            // CustomBootLogo
            // 
            CustomBootLogo.AutoSize = true;
            CustomBootLogo.Location = new Point(176, 213);
            CustomBootLogo.Name = "CustomBootLogo";
            CustomBootLogo.Size = new Size(78, 25);
            CustomBootLogo.TabIndex = 11;
            CustomBootLogo.Text = "bmp file";
            // 
            // CustomBootLogo_clear
            // 
            CustomBootLogo_clear.Location = new Point(88, 208);
            CustomBootLogo_clear.Name = "CustomBootLogo_clear";
            CustomBootLogo_clear.Size = new Size(82, 34);
            CustomBootLogo_clear.TabIndex = 10;
            CustomBootLogo_clear.Text = "clear";
            CustomBootLogo_clear.UseVisualStyleBackColor = true;
            CustomBootLogo_clear.Click += CustomBootLogo_clear_Click;
            // 
            // CustomBootLogo_select
            // 
            CustomBootLogo_select.Location = new Point(3, 208);
            CustomBootLogo_select.Name = "CustomBootLogo_select";
            CustomBootLogo_select.Size = new Size(82, 34);
            CustomBootLogo_select.TabIndex = 9;
            CustomBootLogo_select.Text = "select";
            CustomBootLogo_select.UseVisualStyleBackColor = true;
            CustomBootLogo_select.Click += CustomBootLogo_select_Click;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(3, 180);
            label15.Name = "label15";
            label15.Size = new Size(214, 25);
            label15.TabIndex = 8;
            label15.Text = "Custom boot logo (BETA)";
            // 
            // panel4
            // 
            panel4.BackColor = SystemColors.Window;
            panel4.BorderStyle = BorderStyle.Fixed3D;
            panel4.Controls.Add(CustomBootLogo_centering);
            panel4.Controls.Add(CustomBootLogo);
            panel4.Controls.Add(label15);
            panel4.Controls.Add(label13);
            panel4.Controls.Add(CustomBootLogo_clear);
            panel4.Controls.Add(ScreenTimeout);
            panel4.Controls.Add(CustomBootLogo_select);
            panel4.Location = new Point(505, 44);
            panel4.Name = "panel4";
            panel4.Size = new Size(314, 284);
            panel4.TabIndex = 36;
            // 
            // CustomBootLogo_centering
            // 
            CustomBootLogo_centering.AutoSize = true;
            CustomBootLogo_centering.Location = new Point(3, 248);
            CustomBootLogo_centering.Name = "CustomBootLogo_centering";
            CustomBootLogo_centering.Size = new Size(111, 29);
            CustomBootLogo_centering.TabIndex = 12;
            CustomBootLogo_centering.Text = "centering";
            CustomBootLogo_centering.UseVisualStyleBackColor = true;
            CustomBootLogo_centering.CheckedChanged += CustomBootLogo_centering_CheckedChanged;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(120, 6);
            label13.Name = "label13";
            label13.Size = new Size(134, 25);
            label13.TabIndex = 1;
            label13.Text = "Screen Timeout";
            // 
            // ScreenTimeout
            // 
            ScreenTimeout.Location = new Point(3, 3);
            ScreenTimeout.Name = "ScreenTimeout";
            ScreenTimeout.Size = new Size(111, 31);
            ScreenTimeout.TabIndex = 0;
            ScreenTimeout.TextChanged += ScreenTimeout_TextChanged;
            // 
            // TweakList
            // 
            TweakList.BackColor = SystemColors.Control;
            TweakList.FormattingEnabled = true;
            TweakList.Location = new Point(6, 44);
            TweakList.Name = "TweakList";
            TweakList.Size = new Size(493, 284);
            TweakList.TabIndex = 2;
            TweakList.ItemCheck += TweakList_ItemCheck;
            // 
            // ArchitectureSelect
            // 
            ArchitectureSelect.FormattingEnabled = true;
            ArchitectureSelect.Location = new Point(6, 127);
            ArchitectureSelect.Name = "ArchitectureSelect";
            ArchitectureSelect.Size = new Size(318, 33);
            ArchitectureSelect.TabIndex = 37;
            ArchitectureSelect.TextChanged += ArchitectureSelect_TextChanged;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(330, 130);
            label14.Name = "label14";
            label14.Size = new Size(389, 25);
            label14.TabIndex = 38;
            label14.Text = "select the actual architecture of your image here";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Controls.Add(tabPage7);
            tabControl1.Controls.Add(tabPage8);
            tabControl1.Location = new Point(12, 63);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(959, 385);
            tabControl1.TabIndex = 39;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(label16);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(label14);
            tabPage1.Controls.Add(WindowsDescription);
            tabPage1.Controls.Add(ArchitectureSelect);
            tabPage1.Controls.Add(WindowsSelect);
            tabPage1.Controls.Add(WindowsVersionSelect);
            tabPage1.Controls.Add(WindowsVersionClear);
            tabPage1.Controls.Add(WindowsVersionUpdate);
            tabPage1.Controls.Add(WindowsClear);
            tabPage1.Controls.Add(WindowsName);
            tabPage1.Location = new Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(951, 347);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "base";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(3, 319);
            label16.Name = "label16";
            label16.Size = new Size(582, 25);
            label16.TabIndex = 39;
            label16.Text = "recommended \"Windows 10 Enterprise\" or \"Windows 10 IoT Enterprise\"";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label2);
            tabPage2.Controls.Add(WinboxName);
            tabPage2.Controls.Add(WinboxDescription);
            tabPage2.Location = new Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(951, 347);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "description";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(panel2);
            tabPage3.Controls.Add(label3);
            tabPage3.Location = new Point(4, 34);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(951, 347);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "app";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(TweakList);
            tabPage4.Controls.Add(label5);
            tabPage4.Controls.Add(panel4);
            tabPage4.Location = new Point(4, 34);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(951, 347);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "settings";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(panel5);
            tabPage5.Controls.Add(label17);
            tabPage5.Controls.Add(label10);
            tabPage5.Controls.Add(panel3);
            tabPage5.Location = new Point(4, 34);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(951, 347);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "post install";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            panel5.BorderStyle = BorderStyle.Fixed3D;
            panel5.Controls.Add(postinstall_user_reg);
            panel5.Controls.Add(label21);
            panel5.Controls.Add(postinstall_user_bat);
            panel5.Controls.Add(postinstall_user_bat_sel);
            panel5.Controls.Add(postinstall_user_reg_clr);
            panel5.Controls.Add(label20);
            panel5.Controls.Add(postinstall_user_reg_sel);
            panel5.Controls.Add(postinstall_user_bat_clr);
            panel5.Location = new Point(502, 44);
            panel5.Name = "panel5";
            panel5.Size = new Size(446, 142);
            panel5.TabIndex = 37;
            // 
            // postinstall_user_reg
            // 
            postinstall_user_reg.AutoSize = true;
            postinstall_user_reg.Location = new Point(182, 103);
            postinstall_user_reg.Name = "postinstall_user_reg";
            postinstall_user_reg.Size = new Size(66, 25);
            postinstall_user_reg.TabIndex = 15;
            postinstall_user_reg.Text = "reg file";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(3, 5);
            label21.Name = "label21";
            label21.Size = new Size(85, 25);
            label21.TabIndex = 9;
            label21.Text = "Bat script";
            // 
            // postinstall_user_bat
            // 
            postinstall_user_bat.AutoSize = true;
            postinstall_user_bat.Location = new Point(182, 38);
            postinstall_user_bat.Name = "postinstall_user_bat";
            postinstall_user_bat.Size = new Size(86, 25);
            postinstall_user_bat.TabIndex = 14;
            postinstall_user_bat.Text = "bat script";
            // 
            // postinstall_user_bat_sel
            // 
            postinstall_user_bat_sel.Location = new Point(6, 33);
            postinstall_user_bat_sel.Name = "postinstall_user_bat_sel";
            postinstall_user_bat_sel.Size = new Size(82, 34);
            postinstall_user_bat_sel.TabIndex = 8;
            postinstall_user_bat_sel.Text = "select";
            postinstall_user_bat_sel.UseVisualStyleBackColor = true;
            postinstall_user_bat_sel.Click += postinstall_user_bat_sel_Click;
            // 
            // postinstall_user_reg_clr
            // 
            postinstall_user_reg_clr.Location = new Point(94, 98);
            postinstall_user_reg_clr.Name = "postinstall_user_reg_clr";
            postinstall_user_reg_clr.Size = new Size(82, 34);
            postinstall_user_reg_clr.TabIndex = 13;
            postinstall_user_reg_clr.Text = "clear";
            postinstall_user_reg_clr.UseVisualStyleBackColor = true;
            postinstall_user_reg_clr.Click += postinstall_user_reg_clr_Click;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(6, 70);
            label20.Name = "label20";
            label20.Size = new Size(70, 25);
            label20.TabIndex = 10;
            label20.Text = "Reg file";
            // 
            // postinstall_user_reg_sel
            // 
            postinstall_user_reg_sel.Location = new Point(6, 98);
            postinstall_user_reg_sel.Name = "postinstall_user_reg_sel";
            postinstall_user_reg_sel.Size = new Size(82, 34);
            postinstall_user_reg_sel.TabIndex = 12;
            postinstall_user_reg_sel.Text = "select";
            postinstall_user_reg_sel.UseVisualStyleBackColor = true;
            postinstall_user_reg_sel.Click += postinstall_user_reg_sel_Click;
            // 
            // postinstall_user_bat_clr
            // 
            postinstall_user_bat_clr.Location = new Point(94, 33);
            postinstall_user_bat_clr.Name = "postinstall_user_bat_clr";
            postinstall_user_bat_clr.Size = new Size(82, 34);
            postinstall_user_bat_clr.TabIndex = 11;
            postinstall_user_bat_clr.Text = "clear";
            postinstall_user_bat_clr.UseVisualStyleBackColor = true;
            postinstall_user_bat_clr.Click += postinstall_user_bat_clr_Click;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label17.Location = new Point(773, 3);
            label17.Name = "label17";
            label17.Size = new Size(172, 38);
            label17.TabIndex = 36;
            label17.Text = "Winbox user";
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(label7);
            tabPage6.Controls.Add(panel1);
            tabPage6.Location = new Point(4, 34);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(951, 347);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "activation";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            tabPage7.Controls.Add(panel7);
            tabPage7.Controls.Add(panel6);
            tabPage7.Location = new Point(4, 34);
            tabPage7.Name = "tabPage7";
            tabPage7.Padding = new Padding(3);
            tabPage7.Size = new Size(951, 347);
            tabPage7.TabIndex = 6;
            tabPage7.Text = "winbox service";
            tabPage7.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            panel7.BorderStyle = BorderStyle.Fixed3D;
            panel7.Controls.Add(EmbedDisplayReadme);
            panel7.Controls.Add(UseEmbeddedDisplay);
            panel7.Location = new Point(6, 136);
            panel7.Name = "panel7";
            panel7.Size = new Size(287, 124);
            panel7.TabIndex = 1;
            // 
            // EmbedDisplayReadme
            // 
            EmbedDisplayReadme.Location = new Point(168, 83);
            EmbedDisplayReadme.Name = "EmbedDisplayReadme";
            EmbedDisplayReadme.Size = new Size(112, 34);
            EmbedDisplayReadme.TabIndex = 1;
            EmbedDisplayReadme.Text = "README";
            EmbedDisplayReadme.UseVisualStyleBackColor = true;
            EmbedDisplayReadme.Click += EmbedDisplayReadme_Click;
            // 
            // UseEmbeddedDisplay
            // 
            UseEmbeddedDisplay.AutoSize = true;
            UseEmbeddedDisplay.Location = new Point(3, 3);
            UseEmbeddedDisplay.Name = "UseEmbeddedDisplay";
            UseEmbeddedDisplay.Size = new Size(256, 29);
            UseEmbeddedDisplay.TabIndex = 0;
            UseEmbeddedDisplay.Text = "Support Embedded display";
            UseEmbeddedDisplay.UseVisualStyleBackColor = true;
            UseEmbeddedDisplay.CheckedChanged += UseEmbeddedDisplay_CheckedChanged;
            // 
            // panel6
            // 
            panel6.BorderStyle = BorderStyle.Fixed3D;
            panel6.Controls.Add(label19);
            panel6.Controls.Add(label18);
            panel6.Controls.Add(VirtualDisplayHeight);
            panel6.Controls.Add(VirtualDisplayWidth);
            panel6.Controls.Add(AddVirtualDisplay);
            panel6.Location = new Point(6, 6);
            panel6.Name = "panel6";
            panel6.Size = new Size(287, 124);
            panel6.TabIndex = 0;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(159, 78);
            label19.Name = "label19";
            label19.Size = new Size(65, 25);
            label19.TabIndex = 4;
            label19.Text = "Height";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(159, 41);
            label18.Name = "label18";
            label18.Size = new Size(60, 25);
            label18.TabIndex = 3;
            label18.Text = "Width";
            // 
            // VirtualDisplayHeight
            // 
            VirtualDisplayHeight.Location = new Point(3, 75);
            VirtualDisplayHeight.Name = "VirtualDisplayHeight";
            VirtualDisplayHeight.Size = new Size(150, 31);
            VirtualDisplayHeight.TabIndex = 2;
            VirtualDisplayHeight.TextChanged += VirtualDisplayHeight_TextChanged;
            // 
            // VirtualDisplayWidth
            // 
            VirtualDisplayWidth.Location = new Point(3, 38);
            VirtualDisplayWidth.Name = "VirtualDisplayWidth";
            VirtualDisplayWidth.Size = new Size(150, 31);
            VirtualDisplayWidth.TabIndex = 1;
            VirtualDisplayWidth.TextChanged += VirtualDisplayWidth_TextChanged;
            // 
            // AddVirtualDisplay
            // 
            AddVirtualDisplay.AutoSize = true;
            AddVirtualDisplay.Location = new Point(3, 3);
            AddVirtualDisplay.Name = "AddVirtualDisplay";
            AddVirtualDisplay.Size = new Size(186, 29);
            AddVirtualDisplay.TabIndex = 0;
            AddVirtualDisplay.Text = "Add virtual display";
            AddVirtualDisplay.UseVisualStyleBackColor = true;
            AddVirtualDisplay.CheckedChanged += AddVirtualDisplay_CheckedChanged;
            // 
            // tabPage8
            // 
            tabPage8.Controls.Add(panel9);
            tabPage8.Controls.Add(panel8);
            tabPage8.Location = new Point(4, 34);
            tabPage8.Name = "tabPage8";
            tabPage8.Size = new Size(951, 347);
            tabPage8.TabIndex = 7;
            tabPage8.Text = "events";
            tabPage8.UseVisualStyleBackColor = true;
            // 
            // panel9
            // 
            panel9.BorderStyle = BorderStyle.Fixed3D;
            panel9.Controls.Add(postbuildEnabled);
            panel9.Controls.Add(postbuildEvent);
            panel9.Controls.Add(label23);
            panel9.Location = new Point(3, 172);
            panel9.Name = "panel9";
            panel9.Size = new Size(476, 163);
            panel9.TabIndex = 1;
            // 
            // postbuildEnabled
            // 
            postbuildEnabled.AutoSize = true;
            postbuildEnabled.CheckAlign = ContentAlignment.MiddleRight;
            postbuildEnabled.Location = new Point(368, 3);
            postbuildEnabled.Name = "postbuildEnabled";
            postbuildEnabled.Size = new Size(101, 29);
            postbuildEnabled.TabIndex = 2;
            postbuildEnabled.Text = "enabled";
            postbuildEnabled.UseVisualStyleBackColor = true;
            postbuildEnabled.CheckedChanged += postbuildEnabled_CheckedChanged;
            // 
            // postbuildEvent
            // 
            postbuildEvent.Location = new Point(3, 41);
            postbuildEvent.Name = "postbuildEvent";
            postbuildEvent.Size = new Size(466, 115);
            postbuildEvent.TabIndex = 1;
            postbuildEvent.Text = "";
            postbuildEvent.TextChanged += postbuildEvent_TextChanged;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label23.Location = new Point(-2, 0);
            label23.Name = "label23";
            label23.Size = new Size(219, 38);
            label23.TabIndex = 0;
            label23.Text = "post-build event";
            // 
            // panel8
            // 
            panel8.BorderStyle = BorderStyle.Fixed3D;
            panel8.Controls.Add(prebuildEnabled);
            panel8.Controls.Add(prebuildEvent);
            panel8.Controls.Add(label22);
            panel8.Location = new Point(3, 3);
            panel8.Name = "panel8";
            panel8.Size = new Size(476, 163);
            panel8.TabIndex = 0;
            // 
            // prebuildEnabled
            // 
            prebuildEnabled.AutoSize = true;
            prebuildEnabled.CheckAlign = ContentAlignment.MiddleRight;
            prebuildEnabled.Location = new Point(368, 6);
            prebuildEnabled.Name = "prebuildEnabled";
            prebuildEnabled.Size = new Size(101, 29);
            prebuildEnabled.TabIndex = 2;
            prebuildEnabled.Text = "enabled";
            prebuildEnabled.UseVisualStyleBackColor = true;
            prebuildEnabled.CheckedChanged += prebuildEnabled_CheckedChanged;
            // 
            // prebuildEvent
            // 
            prebuildEvent.Location = new Point(3, 41);
            prebuildEvent.Name = "prebuildEvent";
            prebuildEvent.Size = new Size(466, 115);
            prebuildEvent.TabIndex = 1;
            prebuildEvent.Text = "";
            prebuildEvent.TextChanged += prebuildEvent_TextChanged;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label22.Location = new Point(-2, 0);
            label22.Name = "label22";
            label22.Size = new Size(207, 38);
            label22.TabIndex = 0;
            label22.Text = "pre-build event";
            // 
            // OpenEmbeddedFolder
            // 
            OpenEmbeddedFolder.Location = new Point(633, 12);
            OpenEmbeddedFolder.Name = "OpenEmbeddedFolder";
            OpenEmbeddedFolder.Size = new Size(246, 45);
            OpenEmbeddedFolder.TabIndex = 40;
            OpenEmbeddedFolder.Text = "Open Embedded Folder";
            OpenEmbeddedFolder.UseVisualStyleBackColor = true;
            OpenEmbeddedFolder.Click += OpenEmbeddedFolder_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(24, 24);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // EditorForm
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScroll = true;
            AutoScrollMargin = new Size(10, 30);
            BackColor = Color.DimGray;
            ClientSize = new Size(1178, 644);
            Controls.Add(OpenEmbeddedFolder);
            Controls.Add(tabControl1);
            Controls.Add(OpenProjectFolder);
            Controls.Add(ExportImgPartition);
            Controls.Add(LICENSE);
            Controls.Add(README);
            Controls.Add(back);
            Controls.Add(ExportInstallWim);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(ProcessName);
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
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            tabPage6.ResumeLayout(false);
            tabPage6.PerformLayout();
            tabPage7.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            tabPage8.ResumeLayout(false);
            panel9.ResumeLayout(false);
            panel9.PerformLayout();
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
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
        private Label label5;
        private VScrollBar vScrollBar1;
        private RadioButton ProgramType_ExecutableFile;
        private RadioButton ProgramType_RawCommand;
        private TextBox RawCommand;
        private Label label6;
        private Button OpenProjectFolder;
        private RadioButton ProgramType_WebSite;
        private Label label7;
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
        private Panel panel4;
        private Label label13;
        private TextBox ScreenTimeout;
        private ComboBox ArchitectureSelect;
        private Label label14;
        private CheckedListBox TweakList;
        private Label label15;
        private Button CustomBootLogo_select;
        private Button CustomBootLogo_clear;
        private Label CustomBootLogo;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private Label label16;
        private Label label17;
        private Panel panel5;
        private Label postinstall_user_reg;
        private Label label21;
        private Label postinstall_user_bat;
        private Button postinstall_user_bat_sel;
        private Button postinstall_user_reg_clr;
        private Label label20;
        private Button postinstall_user_reg_sel;
        private Button postinstall_user_bat_clr;
        private TabPage tabPage7;
        private Panel panel6;
        private CheckBox AddVirtualDisplay;
        private TextBox VirtualDisplayHeight;
        private TextBox VirtualDisplayWidth;
        private Label label18;
        private Label label19;
        private Panel panel7;
        private CheckBox UseEmbeddedDisplay;
        private Button OpenEmbeddedFolder;
        private Button EmbedDisplayReadme;
        private CheckBox CustomBootLogo_centering;
        private TabPage tabPage8;
        private ContextMenuStrip contextMenuStrip1;
        private Panel panel8;
        private Panel panel9;
        private Label label22;
        private Label label23;
        private RichTextBox prebuildEvent;
        private RichTextBox postbuildEvent;
        private CheckBox postbuildEnabled;
        private CheckBox prebuildEnabled;
    }
}
