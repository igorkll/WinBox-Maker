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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage3 = new TabPage();
            tabPage2 = new TabPage();
            applyBaseSystemBCD = new CheckBox();
            tabControl1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(854, 520);
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
            tabPage3.Location = new Point(4, 34);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(846, 482);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "app";
            tabPage3.UseVisualStyleBackColor = true;
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
            // WinPeModificationsUI
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(878, 544);
            Controls.Add(tabControl1);
            Name = "WinPeModificationsUI";
            Text = "WinPE Modifications";
            tabControl1.ResumeLayout(false);
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
    }
}