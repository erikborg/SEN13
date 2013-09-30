namespace SEN
{
    partial class ProjectSEN
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
            this.serverStartButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.createCarButton = new System.Windows.Forms.Button();
            this.createBusButton = new System.Windows.Forms.Button();
            this.createBikeButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // serverStartButton
            // 
            this.serverStartButton.Location = new System.Drawing.Point(15, 10);
            this.serverStartButton.Name = "serverStartButton";
            this.serverStartButton.Size = new System.Drawing.Size(75, 23);
            this.serverStartButton.TabIndex = 0;
            this.serverStartButton.Text = "Start server";
            this.serverStartButton.UseVisualStyleBackColor = true;
            this.serverStartButton.Click += new System.EventHandler(this.serverStart_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(310, 11);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 5;
            this.clearButton.Text = "Clear XML";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // createCarButton
            // 
            this.createCarButton.Location = new System.Drawing.Point(310, 58);
            this.createCarButton.Name = "createCarButton";
            this.createCarButton.Size = new System.Drawing.Size(75, 23);
            this.createCarButton.TabIndex = 7;
            this.createCarButton.Text = "Car";
            this.createCarButton.UseVisualStyleBackColor = true;
            this.createCarButton.Click += new System.EventHandler(this.createCarButton_Click);
            // 
            // createBusButton
            // 
            this.createBusButton.Location = new System.Drawing.Point(310, 116);
            this.createBusButton.Name = "createBusButton";
            this.createBusButton.Size = new System.Drawing.Size(75, 23);
            this.createBusButton.TabIndex = 8;
            this.createBusButton.Text = "Bus";
            this.createBusButton.UseVisualStyleBackColor = true;
            this.createBusButton.Click += new System.EventHandler(this.createBusButton_Click);
            // 
            // createBikeButton
            // 
            this.createBikeButton.Location = new System.Drawing.Point(310, 87);
            this.createBikeButton.Name = "createBikeButton";
            this.createBikeButton.Size = new System.Drawing.Size(75, 23);
            this.createBikeButton.TabIndex = 9;
            this.createBikeButton.Text = "Bike";
            this.createBikeButton.UseVisualStyleBackColor = true;
            this.createBikeButton.Click += new System.EventHandler(this.createBikeButton_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(15, 39);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(271, 108);
            this.listBox1.TabIndex = 10;
            // 
            // ProjectSEN
            // 
            this.ClientSize = new System.Drawing.Size(401, 167);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.createBikeButton);
            this.Controls.Add(this.createBusButton);
            this.Controls.Add(this.createCarButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.serverStartButton);
            this.Name = "ProjectSEN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button serverStartButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button createCarButton;
        private System.Windows.Forms.Button createBusButton;
        private System.Windows.Forms.Button createBikeButton;
        private System.Windows.Forms.ListBox listBox1;
    }
}

