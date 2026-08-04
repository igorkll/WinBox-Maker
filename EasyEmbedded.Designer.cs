namespace WinBox_Maker
{
    partial class EasyEmbedded
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EasyEmbedded));
            ExportIsoInstaller = new Button();
            ProcessName = new Label();
            ProcessValue = new ProgressBar();
            WindowsName = new TextBox();
            ArchitectureSelect = new ComboBox();
            WindowsSelect = new Button();
            WindowsVersionSelect = new ComboBox();
            label15 = new Label();
            CustomBootLogo_clear = new Button();
            CustomBootLogo_select = new Button();
            CustomBootLogo = new Label();
            panel1 = new Panel();
            ee_onefile = new RadioButton();
            ee_allfiles = new RadioButton();
            label1 = new Label();
            ee_file_clear = new Button();
            ee_file_select = new Button();
            ee_file = new Label();
            richTextBox1 = new RichTextBox();
            back = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // ExportIsoInstaller
            // 
            ExportIsoInstaller.Dock = DockStyle.Bottom;
            ExportIsoInstaller.Location = new Point(0, 733);
            ExportIsoInstaller.Margin = new Padding(4, 4, 4, 47);
            ExportIsoInstaller.Name = "ExportIsoInstaller";
            ExportIsoInstaller.Size = new Size(537, 57);
            ExportIsoInstaller.TabIndex = 21;
            ExportIsoInstaller.Text = "export .iso installer";
            ExportIsoInstaller.UseVisualStyleBackColor = true;
            ExportIsoInstaller.Click += ExportIsoInstaller_Click;
            // 
            // ProcessName
            // 
            ProcessName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ProcessName.AutoSize = true;
            ProcessName.ForeColor = SystemColors.Window;
            ProcessName.Location = new Point(0, 653);
            ProcessName.Margin = new Padding(4, 0, 4, 47);
            ProcessName.Name = "ProcessName";
            ProcessName.Size = new Size(141, 30);
            ProcessName.TabIndex = 22;
            ProcessName.Text = "process name";
            // 
            // ProcessValue
            // 
            ProcessValue.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ProcessValue.Location = new Point(0, 687);
            ProcessValue.Margin = new Padding(4, 4, 4, 4);
            ProcessValue.Name = "ProcessValue";
            ProcessValue.Size = new Size(537, 38);
            ProcessValue.TabIndex = 23;
            // 
            // WindowsName
            // 
            WindowsName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            WindowsName.Location = new Point(125, 14);
            WindowsName.Margin = new Padding(4, 4, 4, 4);
            WindowsName.Name = "WindowsName";
            WindowsName.Size = new Size(259, 35);
            WindowsName.TabIndex = 45;
            WindowsName.TextChanged += WindowsName_TextChanged;
            WindowsName.KeyDown += WindowsName_KeyDown;
            WindowsName.Leave += WindowsName_Leave;
            // 
            // ArchitectureSelect
            // 
            ArchitectureSelect.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ArchitectureSelect.DropDownStyle = ComboBoxStyle.DropDownList;
            ArchitectureSelect.FormattingEnabled = true;
            ArchitectureSelect.Location = new Point(14, 103);
            ArchitectureSelect.Margin = new Padding(4, 4, 4, 4);
            ArchitectureSelect.Name = "ArchitectureSelect";
            ArchitectureSelect.Size = new Size(510, 38);
            ArchitectureSelect.TabIndex = 44;
            ArchitectureSelect.TextChanged += ArchitectureSelect_TextChanged;
            // 
            // WindowsSelect
            // 
            WindowsSelect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            WindowsSelect.Location = new Point(392, 14);
            WindowsSelect.Margin = new Padding(4, 4, 4, 4);
            WindowsSelect.Name = "WindowsSelect";
            WindowsSelect.Size = new Size(133, 36);
            WindowsSelect.TabIndex = 42;
            WindowsSelect.Text = "Select";
            WindowsSelect.UseVisualStyleBackColor = true;
            WindowsSelect.Click += WindowsSelect_Click;
            // 
            // WindowsVersionSelect
            // 
            WindowsVersionSelect.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            WindowsVersionSelect.DropDownStyle = ComboBoxStyle.DropDownList;
            WindowsVersionSelect.FormattingEnabled = true;
            WindowsVersionSelect.Location = new Point(14, 57);
            WindowsVersionSelect.Margin = new Padding(4, 4, 4, 4);
            WindowsVersionSelect.Name = "WindowsVersionSelect";
            WindowsVersionSelect.Size = new Size(510, 38);
            WindowsVersionSelect.TabIndex = 43;
            WindowsVersionSelect.TextChanged += WindowsVersionSelect_TextChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(4, 9);
            label15.Margin = new Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new Size(103, 30);
            label15.TabIndex = 46;
            label15.Text = "Boot logo";
            // 
            // CustomBootLogo_clear
            // 
            CustomBootLogo_clear.Location = new Point(220, 4);
            CustomBootLogo_clear.Margin = new Padding(4, 4, 4, 4);
            CustomBootLogo_clear.Name = "CustomBootLogo_clear";
            CustomBootLogo_clear.Size = new Size(96, 40);
            CustomBootLogo_clear.TabIndex = 48;
            CustomBootLogo_clear.Text = "clear";
            CustomBootLogo_clear.UseVisualStyleBackColor = true;
            CustomBootLogo_clear.Click += CustomBootLogo_clear_Click;
            // 
            // CustomBootLogo_select
            // 
            CustomBootLogo_select.Location = new Point(118, 4);
            CustomBootLogo_select.Margin = new Padding(4, 4, 4, 4);
            CustomBootLogo_select.Name = "CustomBootLogo_select";
            CustomBootLogo_select.Size = new Size(96, 40);
            CustomBootLogo_select.TabIndex = 47;
            CustomBootLogo_select.Text = "select";
            CustomBootLogo_select.UseVisualStyleBackColor = true;
            CustomBootLogo_select.Click += CustomBootLogo_select_Click;
            // 
            // CustomBootLogo
            // 
            CustomBootLogo.AutoSize = true;
            CustomBootLogo.Location = new Point(323, 9);
            CustomBootLogo.Margin = new Padding(4, 0, 4, 0);
            CustomBootLogo.Name = "CustomBootLogo";
            CustomBootLogo.Size = new Size(89, 30);
            CustomBootLogo.TabIndex = 49;
            CustomBootLogo.Text = "bmp file";
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = SystemColors.Window;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(ee_onefile);
            panel1.Controls.Add(ee_allfiles);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(ee_file_clear);
            panel1.Controls.Add(ee_file_select);
            panel1.Controls.Add(ee_file);
            panel1.Controls.Add(label15);
            panel1.Controls.Add(CustomBootLogo_clear);
            panel1.Controls.Add(CustomBootLogo_select);
            panel1.Controls.Add(CustomBootLogo);
            panel1.Location = new Point(14, 148);
            panel1.Margin = new Padding(4, 4, 4, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(510, 150);
            panel1.TabIndex = 50;
            // 
            // ee_onefile
            // 
            ee_onefile.AutoSize = true;
            ee_onefile.Location = new Point(122, 97);
            ee_onefile.Margin = new Padding(4, 4, 4, 4);
            ee_onefile.Name = "ee_onefile";
            ee_onefile.Size = new Size(213, 34);
            ee_onefile.TabIndex = 55;
            ee_onefile.TabStop = true;
            ee_onefile.Text = "one executable file";
            ee_onefile.UseVisualStyleBackColor = true;
            ee_onefile.CheckedChanged += ee_onefile_CheckedChanged;
            // 
            // ee_allfiles
            // 
            ee_allfiles.AutoSize = true;
            ee_allfiles.Location = new Point(8, 97);
            ee_allfiles.Margin = new Padding(4, 4, 4, 4);
            ee_allfiles.Name = "ee_allfiles";
            ee_allfiles.Size = new Size(102, 34);
            ee_allfiles.TabIndex = 54;
            ee_allfiles.TabStop = true;
            ee_allfiles.Text = "all files";
            ee_allfiles.UseVisualStyleBackColor = true;
            ee_allfiles.CheckedChanged += ee_allfiles_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(4, 56);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(113, 30);
            label1.TabIndex = 50;
            label1.Text = "Executable";
            // 
            // ee_file_clear
            // 
            ee_file_clear.Location = new Point(220, 50);
            ee_file_clear.Margin = new Padding(4, 4, 4, 4);
            ee_file_clear.Name = "ee_file_clear";
            ee_file_clear.Size = new Size(96, 40);
            ee_file_clear.TabIndex = 52;
            ee_file_clear.Text = "clear";
            ee_file_clear.UseVisualStyleBackColor = true;
            ee_file_clear.Click += ee_file_clear_Click;
            // 
            // ee_file_select
            // 
            ee_file_select.Location = new Point(118, 50);
            ee_file_select.Margin = new Padding(4, 4, 4, 4);
            ee_file_select.Name = "ee_file_select";
            ee_file_select.Size = new Size(96, 40);
            ee_file_select.TabIndex = 51;
            ee_file_select.Text = "select";
            ee_file_select.UseVisualStyleBackColor = true;
            ee_file_select.Click += ee_file_select_Click;
            // 
            // ee_file
            // 
            ee_file.AutoSize = true;
            ee_file.Location = new Point(323, 56);
            ee_file.Margin = new Padding(4, 0, 4, 0);
            ee_file.Name = "ee_file";
            ee_file.Size = new Size(79, 30);
            ee_file.TabIndex = 53;
            ee_file.Text = "exe file";
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox1.BackColor = SystemColors.Info;
            richTextBox1.Location = new Point(14, 306);
            richTextBox1.Margin = new Padding(4, 4, 4, 4);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(510, 330);
            richTextBox1.TabIndex = 51;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // back
            // 
            back.Location = new Point(14, 14);
            back.Name = "back";
            back.Size = new Size(104, 35);
            back.TabIndex = 52;
            back.Text = "< back";
            back.UseVisualStyleBackColor = true;
            back.Click += back_Click;
            // 
            // EasyEmbedded
            // 
            AutoScaleDimensions = new SizeF(168F, 168F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.DimGray;
            ClientSize = new Size(537, 790);
            Controls.Add(back);
            Controls.Add(richTextBox1);
            Controls.Add(panel1);
            Controls.Add(WindowsName);
            Controls.Add(ArchitectureSelect);
            Controls.Add(WindowsSelect);
            Controls.Add(WindowsVersionSelect);
            Controls.Add(ProcessName);
            Controls.Add(ProcessValue);
            Controls.Add(ExportIsoInstaller);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 4, 4, 4);
            MinimumSize = new Size(558, 844);
            Name = "EasyEmbedded";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EasyEmbedded";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ExportIsoInstaller;
        private Label ProcessName;
        private ProgressBar ProcessValue;
        private TextBox WindowsName;
        private ComboBox ArchitectureSelect;
        private Button WindowsSelect;
        private ComboBox WindowsVersionSelect;
        private Button CustomBootLogo_clear;
        private Button CustomBootLogo_select;
        private Label CustomBootLogo;
        private Label label15;
        private Panel panel1;
        private Label label1;
        private Button ee_file_clear;
        private Button ee_file_select;
        private Label ee_file;
        private RadioButton ee_onefile;
        private RadioButton ee_allfiles;
        private RichTextBox richTextBox1;
        private Button back;
    }
}