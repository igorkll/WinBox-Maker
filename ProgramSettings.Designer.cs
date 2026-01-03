namespace WinBox_Maker
{
    partial class ProgramSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramSettings));
            msbuildPath = new TextBox();
            selectQemu = new Button();
            selectCargo = new Button();
            qemuPath = new TextBox();
            cargoPath = new TextBox();
            selectPip = new Button();
            selectCmake = new Button();
            pipPath = new TextBox();
            cmakePath = new TextBox();
            selectMsbuild = new Button();
            AutoDetect = new Button();
            openProgramData = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            richTextBox1 = new RichTextBox();
            label1 = new Label();
            telemetry_policy = new ComboBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // msbuildPath
            // 
            msbuildPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            msbuildPath.Location = new Point(6, 6);
            msbuildPath.Name = "msbuildPath";
            msbuildPath.Size = new Size(722, 31);
            msbuildPath.TabIndex = 1;
            msbuildPath.TextChanged += msbuildPath_TextChanged;
            // 
            // selectQemu
            // 
            selectQemu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectQemu.Location = new Point(734, 158);
            selectQemu.Name = "selectQemu";
            selectQemu.Size = new Size(195, 31);
            selectQemu.TabIndex = 13;
            selectQemu.Text = "Select qemu folder";
            selectQemu.UseVisualStyleBackColor = true;
            selectQemu.Click += selectQemu_Click;
            // 
            // selectCargo
            // 
            selectCargo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectCargo.Location = new Point(734, 121);
            selectCargo.Name = "selectCargo";
            selectCargo.Size = new Size(195, 31);
            selectCargo.TabIndex = 12;
            selectCargo.Text = "Select cargo";
            selectCargo.UseVisualStyleBackColor = true;
            selectCargo.Click += selectCargo_Click;
            // 
            // qemuPath
            // 
            qemuPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            qemuPath.Location = new Point(6, 158);
            qemuPath.Name = "qemuPath";
            qemuPath.Size = new Size(722, 31);
            qemuPath.TabIndex = 11;
            qemuPath.TextChanged += qemuPath_TextChanged;
            // 
            // cargoPath
            // 
            cargoPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cargoPath.Location = new Point(6, 121);
            cargoPath.Name = "cargoPath";
            cargoPath.Size = new Size(722, 31);
            cargoPath.TabIndex = 10;
            cargoPath.TextChanged += cargoPath_TextChanged;
            // 
            // selectPip
            // 
            selectPip.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectPip.Location = new Point(734, 80);
            selectPip.Name = "selectPip";
            selectPip.Size = new Size(195, 31);
            selectPip.TabIndex = 9;
            selectPip.Text = "Select pip";
            selectPip.UseVisualStyleBackColor = true;
            selectPip.Click += selectPip_Click;
            // 
            // selectCmake
            // 
            selectCmake.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectCmake.Location = new Point(734, 43);
            selectCmake.Name = "selectCmake";
            selectCmake.Size = new Size(195, 31);
            selectCmake.TabIndex = 8;
            selectCmake.Text = "Select cmake";
            selectCmake.UseVisualStyleBackColor = true;
            selectCmake.Click += selectCmake_Click;
            // 
            // pipPath
            // 
            pipPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pipPath.Location = new Point(6, 80);
            pipPath.Name = "pipPath";
            pipPath.Size = new Size(722, 31);
            pipPath.TabIndex = 7;
            pipPath.TextChanged += pipPath_TextChanged;
            // 
            // cmakePath
            // 
            cmakePath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmakePath.Location = new Point(6, 43);
            cmakePath.Name = "cmakePath";
            cmakePath.Size = new Size(722, 31);
            cmakePath.TabIndex = 6;
            cmakePath.TextChanged += cmakePath_TextChanged;
            // 
            // selectMsbuild
            // 
            selectMsbuild.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectMsbuild.Location = new Point(734, 6);
            selectMsbuild.Name = "selectMsbuild";
            selectMsbuild.Size = new Size(195, 31);
            selectMsbuild.TabIndex = 5;
            selectMsbuild.Text = "Select msbuild";
            selectMsbuild.UseVisualStyleBackColor = true;
            selectMsbuild.Click += selectMsbuild_Click;
            // 
            // AutoDetect
            // 
            AutoDetect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            AutoDetect.Location = new Point(745, 372);
            AutoDetect.Name = "AutoDetect";
            AutoDetect.Size = new Size(195, 34);
            AutoDetect.TabIndex = 4;
            AutoDetect.Text = "Auto search";
            AutoDetect.UseVisualStyleBackColor = true;
            AutoDetect.Click += AutoDetect_Click;
            // 
            // openProgramData
            // 
            openProgramData.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            openProgramData.Location = new Point(755, 468);
            openProgramData.Name = "openProgramData";
            openProgramData.Size = new Size(211, 64);
            openProgramData.TabIndex = 4;
            openProgramData.Text = "Open Program Data";
            openProgramData.UseVisualStyleBackColor = true;
            openProgramData.Click += openProgramData_Click;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(954, 450);
            tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(AutoDetect);
            tabPage1.Controls.Add(selectQemu);
            tabPage1.Controls.Add(msbuildPath);
            tabPage1.Controls.Add(selectCargo);
            tabPage1.Controls.Add(selectMsbuild);
            tabPage1.Controls.Add(qemuPath);
            tabPage1.Controls.Add(cargoPath);
            tabPage1.Controls.Add(cmakePath);
            tabPage1.Controls.Add(selectPip);
            tabPage1.Controls.Add(pipPath);
            tabPage1.Controls.Add(selectCmake);
            tabPage1.Location = new Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(946, 412);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "paths";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(richTextBox1);
            tabPage2.Controls.Add(label1);
            tabPage2.Controls.Add(telemetry_policy);
            tabPage2.Location = new Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(946, 412);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "telemetry";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox1.BackColor = SystemColors.Info;
            richTextBox1.Location = new Point(6, 213);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(934, 193);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(783, 9);
            label1.Name = "label1";
            label1.Size = new Size(138, 25);
            label1.TabIndex = 1;
            label1.Text = "telemetry policy";
            // 
            // telemetry_policy
            // 
            telemetry_policy.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            telemetry_policy.DropDownStyle = ComboBoxStyle.DropDownList;
            telemetry_policy.FormattingEnabled = true;
            telemetry_policy.Items.AddRange(new object[] { "do not send telemetry", "send only the build time (and whether it was successful)", "send the build time and project description with build logs" });
            telemetry_policy.Location = new Point(6, 6);
            telemetry_policy.Name = "telemetry_policy";
            telemetry_policy.Size = new Size(771, 33);
            telemetry_policy.TabIndex = 0;
            telemetry_policy.SelectedIndexChanged += telemetry_policy_SelectedIndexChanged;
            // 
            // ProgramSettings
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = SystemColors.Control;
            ClientSize = new Size(978, 544);
            Controls.Add(tabControl1);
            Controls.Add(openProgramData);
            MinimumSize = new Size(700, 500);
            Name = "ProgramSettings";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Winbox-Maker Settings";
            Load += ProgramSettings_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private TextBox msbuildPath;
        private Button AutoDetect;
        private Button selectMsbuild;
        private TextBox pipPath;
        private TextBox cmakePath;
        private Button selectPip;
        private Button selectCmake;
        private Button openProgramData;
        private TextBox qemuPath;
        private TextBox cargoPath;
        private Button selectQemu;
        private Button selectCargo;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label1;
        private ComboBox telemetry_policy;
        private RichTextBox richTextBox1;
    }
}