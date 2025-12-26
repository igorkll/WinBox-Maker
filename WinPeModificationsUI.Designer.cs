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
            tabPage3 = new TabPage();
            app_lowlevel = new CheckBox();
            label2 = new Label();
            app_tab = new TabControl();
            tabPage4 = new TabPage();
            richTextBox2 = new RichTextBox();
            tabPage5 = new TabPage();
            label1 = new Label();
            richTextBox1 = new RichTextBox();
            app_custom_cmdline = new TextBox();
            app_override = new CheckBox();
            tabPage2 = new TabPage();
            applyBaseSystemBCD = new CheckBox();
            remove_cmd_exe = new CheckBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage3.SuspendLayout();
            app_tab.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage5.SuspendLayout();
            tabPage2.SuspendLayout();
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
            // tabPage3
            // 
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
            // app_lowlevel
            // 
            app_lowlevel.AutoSize = true;
            app_lowlevel.Location = new Point(3, 38);
            app_lowlevel.Name = "app_lowlevel";
            app_lowlevel.Size = new Size(924, 29);
            app_lowlevel.TabIndex = 3;
            app_lowlevel.Text = "low-level shell initialization (prevents the console window from flashing, does not support arguments in custom)";
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
            tabPage4.Controls.Add(richTextBox2);
            tabPage4.Location = new Point(4, 34);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(1132, 428);
            tabPage4.TabIndex = 0;
            tabPage4.Text = "winbox maker recovery";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // richTextBox2
            // 
            richTextBox2.BackColor = SystemColors.Info;
            richTextBox2.Location = new Point(598, 6);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.ReadOnly = true;
            richTextBox2.Size = new Size(528, 416);
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
    }
}