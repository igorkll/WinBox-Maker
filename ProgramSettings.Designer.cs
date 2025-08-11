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
            msbuildPath = new TextBox();
            panel1 = new Panel();
            selectPip = new Button();
            selectCmake = new Button();
            pipPath = new TextBox();
            cmakePath = new TextBox();
            selectMsbuild = new Button();
            AutoDetect = new Button();
            label1 = new Label();
            openProgramData = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // msbuildPath
            // 
            msbuildPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            msbuildPath.Location = new Point(3, 6);
            msbuildPath.Name = "msbuildPath";
            msbuildPath.Size = new Size(748, 31);
            msbuildPath.TabIndex = 1;
            msbuildPath.TextChanged += msbuildPath_TextChanged;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(selectPip);
            panel1.Controls.Add(selectCmake);
            panel1.Controls.Add(pipPath);
            panel1.Controls.Add(cmakePath);
            panel1.Controls.Add(selectMsbuild);
            panel1.Controls.Add(AutoDetect);
            panel1.Controls.Add(msbuildPath);
            panel1.Location = new Point(12, 50);
            panel1.Name = "panel1";
            panel1.Size = new Size(959, 258);
            panel1.TabIndex = 3;
            // 
            // selectPip
            // 
            selectPip.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectPip.Location = new Point(757, 80);
            selectPip.Name = "selectPip";
            selectPip.Size = new Size(195, 31);
            selectPip.TabIndex = 9;
            selectPip.Text = "Select pip";
            selectPip.UseVisualStyleBackColor = true;
            // 
            // selectCmake
            // 
            selectCmake.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectCmake.Location = new Point(757, 43);
            selectCmake.Name = "selectCmake";
            selectCmake.Size = new Size(195, 31);
            selectCmake.TabIndex = 8;
            selectCmake.Text = "Select cmake";
            selectCmake.UseVisualStyleBackColor = true;
            // 
            // pipPath
            // 
            pipPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pipPath.Location = new Point(3, 80);
            pipPath.Name = "pipPath";
            pipPath.Size = new Size(748, 31);
            pipPath.TabIndex = 7;
            pipPath.TextChanged += pipPath_TextChanged;
            // 
            // cmakePath
            // 
            cmakePath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmakePath.Location = new Point(3, 43);
            cmakePath.Name = "cmakePath";
            cmakePath.Size = new Size(748, 31);
            cmakePath.TabIndex = 6;
            cmakePath.TextChanged += cmakePath_TextChanged;
            // 
            // selectMsbuild
            // 
            selectMsbuild.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectMsbuild.Location = new Point(757, 6);
            selectMsbuild.Name = "selectMsbuild";
            selectMsbuild.Size = new Size(195, 31);
            selectMsbuild.TabIndex = 5;
            selectMsbuild.Text = "Select msbuild";
            selectMsbuild.UseVisualStyleBackColor = true;
            // 
            // AutoDetect
            // 
            AutoDetect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            AutoDetect.Location = new Point(757, 217);
            AutoDetect.Name = "AutoDetect";
            AutoDetect.Size = new Size(195, 34);
            AutoDetect.TabIndex = 4;
            AutoDetect.Text = "Auto search";
            AutoDetect.UseVisualStyleBackColor = true;
            AutoDetect.Click += AutoDetect_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(83, 38);
            label1.TabIndex = 2;
            label1.Text = "Paths";
            // 
            // openProgramData
            // 
            openProgramData.Location = new Point(755, 468);
            openProgramData.Name = "openProgramData";
            openProgramData.Size = new Size(211, 64);
            openProgramData.TabIndex = 4;
            openProgramData.Text = "Open Program Data";
            openProgramData.UseVisualStyleBackColor = true;
            openProgramData.Click += openProgramData_Click;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(3, 117);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(748, 31);
            textBox1.TabIndex = 10;
            // 
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox2.Location = new Point(3, 154);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(748, 31);
            textBox2.TabIndex = 11;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Location = new Point(757, 117);
            button1.Name = "button1";
            button1.Size = new Size(195, 31);
            button1.TabIndex = 12;
            button1.Text = "Select pip";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.Location = new Point(757, 154);
            button2.Name = "button2";
            button2.Size = new Size(195, 31);
            button2.TabIndex = 13;
            button2.Text = "Select pip";
            button2.UseVisualStyleBackColor = true;
            // 
            // ProgramSettings
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = SystemColors.Control;
            ClientSize = new Size(978, 544);
            Controls.Add(openProgramData);
            Controls.Add(panel1);
            Controls.Add(label1);
            MinimumSize = new Size(700, 500);
            Name = "ProgramSettings";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Winbox-Maker Settings";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox msbuildPath;
        private Panel panel1;
        private Label label1;
        private Button AutoDetect;
        private Button selectMsbuild;
        private TextBox pipPath;
        private TextBox cmakePath;
        private Button selectPip;
        private Button selectCmake;
        private Button openProgramData;
        private TextBox textBox2;
        private TextBox textBox1;
        private Button button2;
        private Button button1;
    }
}