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
            override_application = new CheckBox();
            tabPage2 = new TabPage();
            applyBaseSystemBCD = new CheckBox();
            override_application_tab = new TabControl();
            tabPage4 = new TabPage();
            tabPage5 = new TabPage();
            textBox1 = new TextBox();
            richTextBox1 = new RichTextBox();
            richTextBox2 = new RichTextBox();
            label1 = new Label();
            textBox2 = new TextBox();
            label2 = new Label();
            tabControl1.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage2.SuspendLayout();
            override_application_tab.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage5.SuspendLayout();
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
            tabPage1.Location = new Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(846, 482);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "main";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(override_application_tab);
            tabPage3.Controls.Add(override_application);
            tabPage3.Location = new Point(4, 34);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1146, 582);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "app";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // override_application
            // 
            override_application.AutoSize = true;
            override_application.Location = new Point(3, 3);
            override_application.Name = "override_application";
            override_application.Size = new Size(195, 29);
            override_application.TabIndex = 0;
            override_application.Text = "override application";
            override_application.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(applyBaseSystemBCD);
            tabPage2.Location = new Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(846, 482);
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
            // override_application_tab
            // 
            override_application_tab.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            override_application_tab.Controls.Add(tabPage4);
            override_application_tab.Controls.Add(tabPage5);
            override_application_tab.Location = new Point(3, 38);
            override_application_tab.Name = "override_application_tab";
            override_application_tab.SelectedIndex = 0;
            override_application_tab.Size = new Size(1140, 541);
            override_application_tab.TabIndex = 1;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(richTextBox2);
            tabPage4.Location = new Point(4, 34);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(1132, 503);
            tabPage4.TabIndex = 0;
            tabPage4.Text = "winbox maker recovery";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(label2);
            tabPage5.Controls.Add(textBox2);
            tabPage5.Controls.Add(label1);
            tabPage5.Controls.Add(richTextBox1);
            tabPage5.Controls.Add(textBox1);
            tabPage5.Location = new Point(4, 34);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(1132, 503);
            tabPage5.TabIndex = 1;
            tabPage5.Text = "custom";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(6, 6);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(375, 31);
            textBox1.TabIndex = 0;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = SystemColors.Info;
            richTextBox1.Location = new Point(598, 6);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(528, 491);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // richTextBox2
            // 
            richTextBox2.BackColor = SystemColors.Info;
            richTextBox2.Location = new Point(598, 6);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.ReadOnly = true;
            richTextBox2.Size = new Size(528, 491);
            richTextBox2.TabIndex = 2;
            richTextBox2.Text = resources.GetString("richTextBox2.Text");
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(387, 9);
            label1.Name = "label1";
            label1.Size = new Size(48, 25);
            label1.TabIndex = 2;
            label1.Text = "path";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(6, 43);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(375, 31);
            textBox2.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(387, 46);
            label2.Name = "label2";
            label2.Size = new Size(46, 25);
            label2.TabIndex = 4;
            label2.Text = "args";
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
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            override_application_tab.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private CheckBox applyBaseSystemBCD;
        private CheckBox override_application;
        private TabControl override_application_tab;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TextBox textBox1;
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;
        private Label label1;
        private Label label2;
        private TextBox textBox2;
    }
}