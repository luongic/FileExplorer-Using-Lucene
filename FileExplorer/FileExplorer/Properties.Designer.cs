
namespace FileExplorer
{
    partial class PropertiesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertiesForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.containLB = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.readCheckBox = new System.Windows.Forms.CheckBox();
            this.hiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.type = new System.Windows.Forms.Label();
            this.location = new System.Windows.Forms.Label();
            this.size = new System.Windows.Forms.Label();
            this.date = new System.Windows.Forms.Label();
            this.containtxt = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(33, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(90, 87);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(168, 56);
            this.nameBox.Name = "nameBox";
            this.nameBox.ReadOnly = true;
            this.nameBox.Size = new System.Drawing.Size(263, 27);
            this.nameBox.TabIndex = 1;
            this.nameBox.TextChanged += new System.EventHandler(this.nameBox_TextChanged);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(-7, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(463, 16);
            this.label1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Type :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 205);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Location :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 247);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Size :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 289);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "Date Modify :";
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Location = new System.Drawing.Point(-7, 364);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(463, 11);
            this.label6.TabIndex = 7;
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(229, 507);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 36);
            this.button1.TabIndex = 8;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button2_Click);
            // 
            // containLB
            // 
            this.containLB.AutoSize = true;
            this.containLB.Location = new System.Drawing.Point(33, 331);
            this.containLB.Name = "containLB";
            this.containLB.Size = new System.Drawing.Size(73, 20);
            this.containLB.TabIndex = 9;
            this.containLB.Text = "Contains :";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Red;
            this.button2.ForeColor = System.Drawing.SystemColors.Control;
            this.button2.Location = new System.Drawing.Point(337, 507);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 36);
            this.button2.TabIndex = 10;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 395);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 20);
            this.label8.TabIndex = 11;
            this.label8.Text = "Attributes :";
            // 
            // readCheckBox
            // 
            this.readCheckBox.AutoSize = true;
            this.readCheckBox.Location = new System.Drawing.Point(168, 393);
            this.readCheckBox.Name = "readCheckBox";
            this.readCheckBox.Size = new System.Drawing.Size(99, 24);
            this.readCheckBox.TabIndex = 12;
            this.readCheckBox.Text = "Read Only";
            this.readCheckBox.UseVisualStyleBackColor = true;
            this.readCheckBox.CheckedChanged += new System.EventHandler(this.readCheckBox_CheckedChanged);
            // 
            // hiddenCheckBox
            // 
            this.hiddenCheckBox.AutoSize = true;
            this.hiddenCheckBox.Location = new System.Drawing.Point(168, 435);
            this.hiddenCheckBox.Name = "hiddenCheckBox";
            this.hiddenCheckBox.Size = new System.Drawing.Size(80, 24);
            this.hiddenCheckBox.TabIndex = 13;
            this.hiddenCheckBox.Text = "Hidden";
            this.hiddenCheckBox.UseVisualStyleBackColor = true;
            this.hiddenCheckBox.CheckedChanged += new System.EventHandler(this.hiddenCheckBox_CheckedChanged);
            // 
            // type
            // 
            this.type.AutoSize = true;
            this.type.Location = new System.Drawing.Point(168, 167);
            this.type.Name = "type";
            this.type.Size = new System.Drawing.Size(50, 20);
            this.type.TabIndex = 14;
            this.type.Text = "label9";
            // 
            // location
            // 
            this.location.AutoSize = true;
            this.location.Location = new System.Drawing.Point(168, 205);
            this.location.Name = "location";
            this.location.Size = new System.Drawing.Size(58, 20);
            this.location.TabIndex = 15;
            this.location.Text = "label10";
            // 
            // size
            // 
            this.size.AutoSize = true;
            this.size.Location = new System.Drawing.Point(168, 247);
            this.size.Name = "size";
            this.size.Size = new System.Drawing.Size(58, 20);
            this.size.TabIndex = 16;
            this.size.Text = "label11";
            // 
            // date
            // 
            this.date.AutoSize = true;
            this.date.Location = new System.Drawing.Point(168, 289);
            this.date.Name = "date";
            this.date.Size = new System.Drawing.Size(58, 20);
            this.date.TabIndex = 17;
            this.date.Text = "label12";
            // 
            // containtxt
            // 
            this.containtxt.AutoSize = true;
            this.containtxt.Location = new System.Drawing.Point(168, 331);
            this.containtxt.Name = "containtxt";
            this.containtxt.Size = new System.Drawing.Size(58, 20);
            this.containtxt.TabIndex = 18;
            this.containtxt.Text = "label13";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(168, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 20);
            this.label7.TabIndex = 19;
            this.label7.Text = "Name:";
            // 
            // PropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 555);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.containtxt);
            this.Controls.Add(this.date);
            this.Controls.Add(this.size);
            this.Controls.Add(this.location);
            this.Controls.Add(this.type);
            this.Controls.Add(this.hiddenCheckBox);
            this.Controls.Add(this.readCheckBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.containLB);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PropertiesForm";
            this.Text = "Properties";
            this.Load += new System.EventHandler(this.PropertiesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label containLB;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox readCheckBox;
        private System.Windows.Forms.CheckBox hiddenCheckBox;
        private System.Windows.Forms.Label type;
        private System.Windows.Forms.Label location;
        private System.Windows.Forms.Label size;
        private System.Windows.Forms.Label date;
        private System.Windows.Forms.Label containtxt;
        private System.Windows.Forms.Label label7;
    }
}