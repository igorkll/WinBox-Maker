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
            textBox1 = new TextBox();
            panel1 = new Panel();
            label1 = new Label();
            AutoDetect = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(3, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(393, 31);
            textBox1.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(textBox1);
            panel1.Location = new Point(12, 50);
            panel1.Name = "panel1";
            panel1.Size = new Size(776, 348);
            panel1.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(116, 38);
            label1.TabIndex = 2;
            label1.Text = "Settings";
            // 
            // AutoDetect
            // 
            AutoDetect.Location = new Point(12, 404);
            AutoDetect.Name = "AutoDetect";
            AutoDetect.Size = new Size(776, 34);
            AutoDetect.TabIndex = 4;
            AutoDetect.Text = "Auto search";
            AutoDetect.UseVisualStyleBackColor = true;
            // 
            // ProgramSettings
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = SystemColors.Control;
            ClientSize = new Size(800, 450);
            Controls.Add(AutoDetect);
            Controls.Add(panel1);
            Controls.Add(label1);
            Name = "ProgramSettings";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Winbox Maker Settings";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textBox1;
        private Panel panel1;
        private Label label1;
        private Button AutoDetect;
    }
}