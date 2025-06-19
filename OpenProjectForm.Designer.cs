namespace WinBox_Maker
{
    partial class OpenProjectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenProjectForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            OpenProject = new Button();
            NewProject = new Button();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            panel1 = new Panel();
            LICENSE = new Button();
            README = new Button();
            panel2 = new Panel();
            pictureBox4 = new PictureBox();
            pictureBox3 = new PictureBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49.9999924F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0000038F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(OpenProject, 0, 1);
            tableLayoutPanel1.Controls.Add(NewProject, 1, 1);
            tableLayoutPanel1.Controls.Add(pictureBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(pictureBox2, 1, 0);
            tableLayoutPanel1.Controls.Add(panel1, 2, 1);
            tableLayoutPanel1.Controls.Add(panel2, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // OpenProject
            // 
            OpenProject.Dock = DockStyle.Fill;
            OpenProject.Location = new Point(30, 330);
            OpenProject.Margin = new Padding(30);
            OpenProject.Name = "OpenProject";
            OpenProject.Size = new Size(267, 90);
            OpenProject.TabIndex = 0;
            OpenProject.Text = "Open Project";
            OpenProject.UseVisualStyleBackColor = true;
            OpenProject.Click += OpenProject_Click;
            // 
            // NewProject
            // 
            NewProject.Dock = DockStyle.Fill;
            NewProject.Location = new Point(357, 330);
            NewProject.Margin = new Padding(30);
            NewProject.Name = "NewProject";
            NewProject.Size = new Size(268, 90);
            NewProject.TabIndex = 1;
            NewProject.Text = "New Project";
            NewProject.UseVisualStyleBackColor = true;
            NewProject.Click += NewProject_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(321, 294);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(330, 3);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(322, 294);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 4;
            pictureBox2.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(LICENSE);
            panel1.Controls.Add(README);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(658, 303);
            panel1.Name = "panel1";
            panel1.Size = new Size(139, 144);
            panel1.TabIndex = 5;
            // 
            // LICENSE
            // 
            LICENSE.Anchor = AnchorStyles.Bottom;
            LICENSE.Location = new Point(12, 83);
            LICENSE.Name = "LICENSE";
            LICENSE.Size = new Size(112, 34);
            LICENSE.TabIndex = 1;
            LICENSE.Text = "LICENSE";
            LICENSE.UseVisualStyleBackColor = true;
            LICENSE.Click += LICENSE_Click;
            // 
            // README
            // 
            README.Anchor = AnchorStyles.Top;
            README.Location = new Point(12, 27);
            README.Name = "README";
            README.Size = new Size(112, 34);
            README.TabIndex = 0;
            README.Text = "README";
            README.UseVisualStyleBackColor = true;
            README.Click += README_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(pictureBox4);
            panel2.Controls.Add(pictureBox3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(658, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(139, 294);
            panel2.TabIndex = 6;
            // 
            // pictureBox4
            // 
            pictureBox4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox4.Cursor = Cursors.Hand;
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(34, 111);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(96, 96);
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 1;
            pictureBox4.TabStop = false;
            pictureBox4.Click += pictureBox4_Click;
            // 
            // pictureBox3
            // 
            pictureBox3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox3.Cursor = Cursors.Hand;
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(34, 9);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(96, 96);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 0;
            pictureBox3.TabStop = false;
            pictureBox3.Click += pictureBox3_Click;
            // 
            // OpenProjectForm
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(700, 500);
            Name = "OpenProjectForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OpenProject";
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button OpenProject;
        private Button NewProject;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Panel panel1;
        private Button LICENSE;
        private Button README;
        private Panel panel2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
    }
}