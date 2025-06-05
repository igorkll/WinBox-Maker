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
            logichub = new Button();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
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
            tableLayoutPanel1.Controls.Add(logichub, 2, 0);
            tableLayoutPanel1.Controls.Add(pictureBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(pictureBox2, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 66.6666641F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // OpenProject
            // 
            OpenProject.Dock = DockStyle.Fill;
            OpenProject.Location = new Point(30, 329);
            OpenProject.Margin = new Padding(30);
            OpenProject.Name = "OpenProject";
            OpenProject.Size = new Size(272, 91);
            OpenProject.TabIndex = 0;
            OpenProject.Text = "Open Project";
            OpenProject.UseVisualStyleBackColor = true;
            OpenProject.Click += OpenProject_Click;
            // 
            // NewProject
            // 
            NewProject.Dock = DockStyle.Fill;
            NewProject.Location = new Point(362, 329);
            NewProject.Margin = new Padding(30);
            NewProject.Name = "NewProject";
            NewProject.Size = new Size(273, 91);
            NewProject.TabIndex = 1;
            NewProject.Text = "New Project";
            NewProject.UseVisualStyleBackColor = true;
            NewProject.Click += NewProject_Click;
            // 
            // logichub
            // 
            logichub.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            logichub.Image = (Image)resources.GetObject("logichub.Image");
            logichub.Location = new Point(669, 3);
            logichub.Name = "logichub";
            logichub.Size = new Size(128, 128);
            logichub.TabIndex = 2;
            logichub.UseVisualStyleBackColor = true;
            logichub.Click += logichub_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(326, 293);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(335, 3);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(327, 293);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 4;
            pictureBox2.TabStop = false;
            // 
            // OpenProjectForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "OpenProjectForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OpenProject";
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button OpenProject;
        private Button NewProject;
        private Button logichub;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
    }
}