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
            this.button2 = new System.Windows.Forms.Button();
            this.ipField = new System.Windows.Forms.TextBox();
            this.portField = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.clearButton = new System.Windows.Forms.Button();
            this.createCarButton = new System.Windows.Forms.Button();
            this.createBusButton = new System.Windows.Forms.Button();
            this.createBikeButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(211, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Connect";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // ipField
            // 
            this.ipField.Location = new System.Drawing.Point(40, 13);
            this.ipField.Name = "ipField";
            this.ipField.Size = new System.Drawing.Size(100, 20);
            this.ipField.TabIndex = 1;
            // 
            // portField
            // 
            this.portField.Location = new System.Drawing.Point(162, 13);
            this.portField.Name = "portField";
            this.portField.Size = new System.Drawing.Size(43, 20);
            this.portField.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = ":";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "IP:";
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
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.portField);
            this.Controls.Add(this.ipField);
            this.Controls.Add(this.button2);
            this.Name = "ProjectSEN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox ipField;
        private System.Windows.Forms.TextBox portField;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button createCarButton;
        private System.Windows.Forms.Button createBusButton;
        private System.Windows.Forms.Button createBikeButton;
        private System.Windows.Forms.ListBox listBox1;
    }
}

