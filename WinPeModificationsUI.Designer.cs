namespace WinBox_Maker
{
    partial class WinPeModificationsUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinPeModificationsUI));
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            remove_cmd_exe = new CheckBox();
            tabPage3 = new TabPage();
            richTextBox3 = new RichTextBox();
            app_lowlevel = new CheckBox();
            label2 = new Label();
            app_tab = new TabControl();
            tabPage4 = new TabPage();
            recovery_allowFactoryReset = new CheckBox();
            recovery_allowFlashWithFactoryReset = new CheckBox();
            recovery_allowFlashWithoutFactoryReset = new CheckBox();
            label4 = new Label();
            recovery_textOnInfoPage_en = new CheckBox();
            recovery_textOnInfoPage = new RichTextBox();
            recovery_ffuName = new TextBox();
            recovery_imgName = new TextBox();
            recovery_wimName = new TextBox();
            recovery_allowFlashFfu = new CheckBox();
            recovery_allowFlashImg = new CheckBox();
            recovery_allowFlashWim = new CheckBox();
            label3 = new Label();
            recovery_title = new TextBox();
            recovery_dataPaths = new RichTextBox();
            richTextBox2 = new RichTextBox();
            tabPage5 = new TabPage();
            label1 = new Label();
            richTextBox1 = new RichTextBox();
            app_custom_cmdline = new TextBox();
            app_override = new CheckBox();
            tabPage2 = new TabPage();
            applyBaseSystemBCD = new CheckBox();
            tabControl2 = new TabControl();
            tabPage6 = new TabPage();
            tabPage7 = new TabPage();
            tabPage8 = new TabPage();
            winboxRecoveryLogoType = new ComboBox();
            label5 = new Label();
            customRecoveryLogoPath = new TextBox();
            customRecoveryLogoPath_sel = new Button();
            customRecoveryLogoPath_clr = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage3.SuspendLayout();
            app_tab.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage5.SuspendLayout();
            tabPage2.SuspendLayout();
            tabControl2.SuspendLayout();
            tabPage6.SuspendLayout();
            tabPage7.SuspendLayout();
            tabPage8.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1154, 620);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(remove_cmd_exe);
            tabPage1.Location = new Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1146, 582);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "main";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // remove_cmd_exe
            // 
            remove_cmd_exe.AutoSize = true;
            remove_cmd_exe.Location = new Point(6, 6);
            remove_cmd_exe.Name = "remove_cmd_exe";
            remove_cmd_exe.Size = new Size(614, 29);
            remove_cmd_exe.TabIndex = 0;
            remove_cmd_exe.Text = "delete cmd.exe (ensures that it is not possible to open cmd via shift+f10)";
            remove_cmd_exe.UseVisualStyleBackColor = true;
            remove_cmd_exe.CheckedChanged += remove_cmd_exe_CheckedChanged;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(richTextBox3);
            tabPage3.Controls.Add(app_lowlevel);
            tabPage3.Controls.Add(label2);
            tabPage3.Controls.Add(app_tab);
            tabPage3.Controls.Add(app_override);
            tabPage3.Location = new Point(4, 34);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1146, 582);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "app";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBox3
            // 
            richTextBox3.BackColor = SystemColors.Info;
            richTextBox3.Location = new Point(810, 3);
            richTextBox3.Name = "richTextBox3";
            richTextBox3.ReadOnly = true;
            richTextBox3.Size = new Size(333, 130);
            richTextBox3.TabIndex = 4;
            richTextBox3.Text = resources.GetString("richTextBox3.Text");
            // 
            // app_lowlevel
            // 
            app_lowlevel.AutoSize = true;
            app_lowlevel.Location = new Point(3, 38);
            app_lowlevel.Name = "app_lowlevel";
            app_lowlevel.Size = new Size(808, 29);
            app_lowlevel.TabIndex = 3;
            app_lowlevel.Text = "low-level shell initialization (prevents the console window from flashing when logging into WinPE)";
            app_lowlevel.UseVisualStyleBackColor = true;
            app_lowlevel.CheckedChanged += app_lowlevel_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 85);
            label2.Name = "label2";
            label2.Size = new Size(657, 25);
            label2.TabIndex = 2;
            label2.Text = "Please note that the application replacement option is changed by switching tabs.";
            // 
            // app_tab
            // 
            app_tab.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            app_tab.Controls.Add(tabPage4);
            app_tab.Controls.Add(tabPage5);
            app_tab.Location = new Point(3, 113);
            app_tab.Name = "app_tab";
            app_tab.SelectedIndex = 0;
            app_tab.Size = new Size(1140, 466);
            app_tab.TabIndex = 1;
            app_tab.SelectedIndexChanged += app_tab_SelectedIndexChanged;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(tabControl2);
            tabPage4.Controls.Add(richTextBox2);
            tabPage4.Location = new Point(4, 34);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(1132, 428);
            tabPage4.TabIndex = 0;
            tabPage4.Text = "winbox maker recovery";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // recovery_allowFactoryReset
            // 
            recovery_allowFactoryReset.AutoSize = true;
            recovery_allowFactoryReset.Location = new Point(3, 3);
            recovery_allowFactoryReset.Name = "recovery_allowFactoryReset";
            recovery_allowFactoryReset.Size = new Size(182, 29);
            recovery_allowFactoryReset.TabIndex = 18;
            recovery_allowFactoryReset.Text = "allow factory reset";
            recovery_allowFactoryReset.UseVisualStyleBackColor = true;
            // 
            // recovery_allowFlashWithFactoryReset
            // 
            recovery_allowFlashWithFactoryReset.AutoSize = true;
            recovery_allowFlashWithFactoryReset.Location = new Point(6, 152);
            recovery_allowFlashWithFactoryReset.Name = "recovery_allowFlashWithFactoryReset";
            recovery_allowFlashWithFactoryReset.Size = new Size(262, 29);
            recovery_allowFlashWithFactoryReset.TabIndex = 17;
            recovery_allowFlashWithFactoryReset.Text = "allow flash with factory reset";
            recovery_allowFlashWithFactoryReset.UseVisualStyleBackColor = true;
            // 
            // recovery_allowFlashWithoutFactoryReset
            // 
            recovery_allowFlashWithoutFactoryReset.AutoSize = true;
            recovery_allowFlashWithoutFactoryReset.Location = new Point(6, 117);
            recovery_allowFlashWithoutFactoryReset.Name = "recovery_allowFlashWithoutFactoryReset";
            recovery_allowFlashWithoutFactoryReset.Size = new Size(289, 29);
            recovery_allowFlashWithoutFactoryReset.TabIndex = 16;
            recovery_allowFlashWithoutFactoryReset.Text = "allow flash without factory reset";
            recovery_allowFlashWithoutFactoryReset.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 58);
            label4.Name = "label4";
            label4.Size = new Size(220, 25);
            label4.TabIndex = 15;
            label4.Text = "use application data paths";
            // 
            // recovery_textOnInfoPage_en
            // 
            recovery_textOnInfoPage_en.AutoSize = true;
            recovery_textOnInfoPage_en.Location = new Point(6, 186);
            recovery_textOnInfoPage_en.Name = "recovery_textOnInfoPage_en";
            recovery_textOnInfoPage_en.Size = new Size(180, 29);
            recovery_textOnInfoPage_en.TabIndex = 14;
            recovery_textOnInfoPage_en.Text = "use info page text";
            recovery_textOnInfoPage_en.UseVisualStyleBackColor = true;
            // 
            // recovery_textOnInfoPage
            // 
            recovery_textOnInfoPage.Location = new Point(6, 221);
            recovery_textOnInfoPage.Name = "recovery_textOnInfoPage";
            recovery_textOnInfoPage.Size = new Size(576, 151);
            recovery_textOnInfoPage.TabIndex = 12;
            recovery_textOnInfoPage.Text = "";
            // 
            // recovery_ffuName
            // 
            recovery_ffuName.Location = new Point(183, 80);
            recovery_ffuName.Name = "recovery_ffuName";
            recovery_ffuName.Size = new Size(328, 31);
            recovery_ffuName.TabIndex = 11;
            // 
            // recovery_imgName
            // 
            recovery_imgName.Location = new Point(183, 43);
            recovery_imgName.Name = "recovery_imgName";
            recovery_imgName.Size = new Size(328, 31);
            recovery_imgName.TabIndex = 10;
            // 
            // recovery_wimName
            // 
            recovery_wimName.Location = new Point(183, 6);
            recovery_wimName.Name = "recovery_wimName";
            recovery_wimName.Size = new Size(328, 31);
            recovery_wimName.TabIndex = 9;
            // 
            // recovery_allowFlashFfu
            // 
            recovery_allowFlashFfu.AutoSize = true;
            recovery_allowFlashFfu.Location = new Point(6, 76);
            recovery_allowFlashFfu.Name = "recovery_allowFlashFfu";
            recovery_allowFlashFfu.Size = new Size(160, 29);
            recovery_allowFlashFfu.TabIndex = 8;
            recovery_allowFlashFfu.Text = "allow flash *.ffu";
            recovery_allowFlashFfu.UseVisualStyleBackColor = true;
            // 
            // recovery_allowFlashImg
            // 
            recovery_allowFlashImg.AutoSize = true;
            recovery_allowFlashImg.Location = new Point(6, 41);
            recovery_allowFlashImg.Name = "recovery_allowFlashImg";
            recovery_allowFlashImg.Size = new Size(169, 29);
            recovery_allowFlashImg.TabIndex = 7;
            recovery_allowFlashImg.Text = "allow flash *.img";
            recovery_allowFlashImg.UseVisualStyleBackColor = true;
            // 
            // recovery_allowFlashWim
            // 
            recovery_allowFlashWim.AutoSize = true;
            recovery_allowFlashWim.Location = new Point(6, 6);
            recovery_allowFlashWim.Name = "recovery_allowFlashWim";
            recovery_allowFlashWim.Size = new Size(171, 29);
            recovery_allowFlashWim.TabIndex = 6;
            recovery_allowFlashWim.Text = "allow flash *.wim";
            recovery_allowFlashWim.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(469, 9);
            label3.Name = "label3";
            label3.Size = new Size(113, 25);
            label3.TabIndex = 5;
            label3.Text = "recovery title";
            // 
            // recovery_title
            // 
            recovery_title.Location = new Point(6, 6);
            recovery_title.Name = "recovery_title";
            recovery_title.Size = new Size(457, 31);
            recovery_title.TabIndex = 4;
            // 
            // recovery_dataPaths
            // 
            recovery_dataPaths.Location = new Point(3, 86);
            recovery_dataPaths.Name = "recovery_dataPaths";
            recovery_dataPaths.Size = new Size(587, 289);
            recovery_dataPaths.TabIndex = 3;
            recovery_dataPaths.Text = "";
            // 
            // richTextBox2
            // 
            richTextBox2.BackColor = SystemColors.Info;
            richTextBox2.Location = new Point(613, 6);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.ReadOnly = true;
            richTextBox2.Size = new Size(513, 416);
            richTextBox2.TabIndex = 2;
            richTextBox2.Text = resources.GetString("richTextBox2.Text");
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(label1);
            tabPage5.Controls.Add(richTextBox1);
            tabPage5.Controls.Add(app_custom_cmdline);
            tabPage5.Location = new Point(4, 34);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(1132, 428);
            tabPage5.TabIndex = 1;
            tabPage5.Text = "custom";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(387, 9);
            label1.Name = "label1";
            label1.Size = new Size(74, 25);
            label1.TabIndex = 2;
            label1.Text = "cmdline";
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = SystemColors.Info;
            richTextBox1.Location = new Point(598, 6);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(528, 416);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // app_custom_cmdline
            // 
            app_custom_cmdline.Location = new Point(6, 6);
            app_custom_cmdline.Name = "app_custom_cmdline";
            app_custom_cmdline.Size = new Size(375, 31);
            app_custom_cmdline.TabIndex = 0;
            app_custom_cmdline.TextChanged += app_custom_cmdline_TextChanged;
            // 
            // app_override
            // 
            app_override.AutoSize = true;
            app_override.Location = new Point(3, 3);
            app_override.Name = "app_override";
            app_override.Size = new Size(195, 29);
            app_override.TabIndex = 0;
            app_override.Text = "override application";
            app_override.UseVisualStyleBackColor = true;
            app_override.CheckedChanged += app_override_CheckedChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(applyBaseSystemBCD);
            tabPage2.Location = new Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1146, 582);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "bcd";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // applyBaseSystemBCD
            // 
            applyBaseSystemBCD.AutoSize = true;
            applyBaseSystemBCD.Location = new Point(6, 6);
            applyBaseSystemBCD.Name = "applyBaseSystemBCD";
            applyBaseSystemBCD.Size = new Size(368, 29);
            applyBaseSystemBCD.TabIndex = 0;
            applyBaseSystemBCD.Text = "apply BCD changes from the base system";
            applyBaseSystemBCD.UseVisualStyleBackColor = true;
            applyBaseSystemBCD.CheckedChanged += applyBaseSystemBCD_CheckedChanged;
            // 
            // tabControl2
            // 
            tabControl2.Controls.Add(tabPage6);
            tabControl2.Controls.Add(tabPage8);
            tabControl2.Controls.Add(tabPage7);
            tabControl2.Location = new Point(6, 6);
            tabControl2.Name = "tabControl2";
            tabControl2.SelectedIndex = 0;
            tabControl2.Size = new Size(601, 416);
            tabControl2.TabIndex = 19;
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(customRecoveryLogoPath_clr);
            tabPage6.Controls.Add(customRecoveryLogoPath_sel);
            tabPage6.Controls.Add(customRecoveryLogoPath);
            tabPage6.Controls.Add(label5);
            tabPage6.Controls.Add(winboxRecoveryLogoType);
            tabPage6.Controls.Add(recovery_title);
            tabPage6.Controls.Add(recovery_textOnInfoPage_en);
            tabPage6.Controls.Add(recovery_textOnInfoPage);
            tabPage6.Controls.Add(label3);
            tabPage6.Location = new Point(4, 34);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(593, 378);
            tabPage6.TabIndex = 0;
            tabPage6.Text = "look";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            tabPage7.Controls.Add(recovery_allowFlashWim);
            tabPage7.Controls.Add(recovery_imgName);
            tabPage7.Controls.Add(recovery_wimName);
            tabPage7.Controls.Add(recovery_ffuName);
            tabPage7.Controls.Add(recovery_allowFlashWithFactoryReset);
            tabPage7.Controls.Add(recovery_allowFlashWithoutFactoryReset);
            tabPage7.Controls.Add(recovery_allowFlashFfu);
            tabPage7.Controls.Add(recovery_allowFlashImg);
            tabPage7.Location = new Point(4, 34);
            tabPage7.Name = "tabPage7";
            tabPage7.Padding = new Padding(3);
            tabPage7.Size = new Size(593, 378);
            tabPage7.TabIndex = 1;
            tabPage7.Text = "flash";
            tabPage7.UseVisualStyleBackColor = true;
            // 
            // tabPage8
            // 
            tabPage8.Controls.Add(label4);
            tabPage8.Controls.Add(recovery_allowFactoryReset);
            tabPage8.Controls.Add(recovery_dataPaths);
            tabPage8.Location = new Point(4, 34);
            tabPage8.Name = "tabPage8";
            tabPage8.Size = new Size(593, 378);
            tabPage8.TabIndex = 2;
            tabPage8.Text = "factory reset";
            tabPage8.UseVisualStyleBackColor = true;
            // 
            // winboxRecoveryLogoType
            // 
            winboxRecoveryLogoType.FormattingEnabled = true;
            winboxRecoveryLogoType.Location = new Point(6, 43);
            winboxRecoveryLogoType.Name = "winboxRecoveryLogoType";
            winboxRecoveryLogoType.Size = new Size(457, 33);
            winboxRecoveryLogoType.TabIndex = 15;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(469, 51);
            label5.Name = "label5";
            label5.Size = new Size(49, 25);
            label5.TabIndex = 16;
            label5.Text = "logo";
            // 
            // customRecoveryLogoPath
            // 
            customRecoveryLogoPath.Location = new Point(6, 82);
            customRecoveryLogoPath.Name = "customRecoveryLogoPath";
            customRecoveryLogoPath.Size = new Size(332, 31);
            customRecoveryLogoPath.TabIndex = 17;
            // 
            // customRecoveryLogoPath_sel
            // 
            customRecoveryLogoPath_sel.Location = new Point(351, 80);
            customRecoveryLogoPath_sel.Name = "customRecoveryLogoPath_sel";
            customRecoveryLogoPath_sel.Size = new Size(112, 34);
            customRecoveryLogoPath_sel.TabIndex = 18;
            customRecoveryLogoPath_sel.Text = "select";
            customRecoveryLogoPath_sel.UseVisualStyleBackColor = true;
            // 
            // customRecoveryLogoPath_clr
            // 
            customRecoveryLogoPath_clr.Location = new Point(470, 82);
            customRecoveryLogoPath_clr.Name = "customRecoveryLogoPath_clr";
            customRecoveryLogoPath_clr.Size = new Size(112, 34);
            customRecoveryLogoPath_clr.TabIndex = 19;
            customRecoveryLogoPath_clr.Text = "clear";
            customRecoveryLogoPath_clr.UseVisualStyleBackColor = true;
            // 
            // WinPeModificationsUI
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1178, 644);
            Controls.Add(tabControl1);
            Name = "WinPeModificationsUI";
            Text = "WinPE Modifications";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            app_tab.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabControl2.ResumeLayout(false);
            tabPage6.ResumeLayout(false);
            tabPage6.PerformLayout();
            tabPage7.ResumeLayout(false);
            tabPage7.PerformLayout();
            tabPage8.ResumeLayout(false);
            tabPage8.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private CheckBox applyBaseSystemBCD;
        private CheckBox app_override;
        private TabControl app_tab;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TextBox app_custom_cmdline;
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;
        private Label label1;
        private Label label2;
        private CheckBox app_lowlevel;
        private CheckBox remove_cmd_exe;
        private RichTextBox richTextBox3;
        private TextBox recovery_title;
        private RichTextBox recovery_dataPaths;
        private Label label3;
        private TextBox recovery_ffuName;
        private TextBox recovery_imgName;
        private TextBox recovery_wimName;
        private CheckBox recovery_allowFlashFfu;
        private CheckBox recovery_allowFlashImg;
        private CheckBox recovery_allowFlashWim;
        private RichTextBox recovery_textOnInfoPage;
        private CheckBox recovery_textOnInfoPage_en;
        private Label label4;
        private CheckBox recovery_allowFlashWithFactoryReset;
        private CheckBox recovery_allowFlashWithoutFactoryReset;
        private CheckBox recovery_allowFactoryReset;
        private TabControl tabControl2;
        private TabPage tabPage6;
        private TabPage tabPage7;
        private TabPage tabPage8;
        private ComboBox winboxRecoveryLogoType;
        private Label label5;
        private TextBox customRecoveryLogoPath;
        private Button customRecoveryLogoPath_clr;
        private Button customRecoveryLogoPath_sel;
    }
}