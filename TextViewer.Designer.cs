namespace WinBox_Maker
{
    partial class TextViewer
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
            Textbox = new RichTextBox();
            SuspendLayout();
            // 
            // Textbox
            // 
            Textbox.Dock = DockStyle.Fill;
            Textbox.Location = new Point(0, 0);
            Textbox.Name = "Textbox";
            Textbox.ReadOnly = true;
            Textbox.Size = new Size(778, 544);
            Textbox.TabIndex = 0;
            Textbox.Text = "";
            // 
            // TextViewer
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(778, 544);
            Controls.Add(Textbox);
            MinimumSize = new Size(320, 240);
            Name = "TextViewer";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TextViewer";
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox Textbox;
    }
}