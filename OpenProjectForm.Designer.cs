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
            tableLayoutPanel1 = new TableLayoutPanel();
            OpenProject = new Button();
            NewProject = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(OpenProject, 0, 1);
            tableLayoutPanel1.Controls.Add(NewProject, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // OpenProject
            // 
            OpenProject.Dock = DockStyle.Fill;
            OpenProject.Location = new Point(30, 330);
            OpenProject.Margin = new Padding(30);
            OpenProject.Name = "OpenProject";
            OpenProject.Size = new Size(340, 90);
            OpenProject.TabIndex = 0;
            OpenProject.Text = "Open Project";
            OpenProject.UseVisualStyleBackColor = true;
            OpenProject.Click += this.OpenProject_Click;
            // 
            // NewProject
            // 
            NewProject.Dock = DockStyle.Fill;
            NewProject.Location = new Point(430, 330);
            NewProject.Margin = new Padding(30);
            NewProject.Name = "NewProject";
            NewProject.Size = new Size(340, 90);
            NewProject.TabIndex = 1;
            NewProject.Text = "New Project";
            NewProject.UseVisualStyleBackColor = true;
            NewProject.Click += this.NewProject_Click;
            // 
            // OpenProjectForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "OpenProjectForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OpenProject";
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button OpenProject;
        private Button NewProject;
    }
}