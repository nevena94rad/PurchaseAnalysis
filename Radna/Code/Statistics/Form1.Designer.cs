﻿namespace Statistics
{
    partial class Form1
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
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.NEW_Cp = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.NEW_FPp = new System.Windows.Forms.Label();
            this.NEW_FPtp = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.NEW_CPp = new System.Windows.Forms.Label();
            this.NEW_CPtp = new System.Windows.Forms.Label();
            this.NEW_NoP = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.OLD_Cp = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.OLD_FPp = new System.Windows.Forms.Label();
            this.OLD_FPtp = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.OLD_CPp = new System.Windows.Forms.Label();
            this.OLD_CPtp = new System.Windows.Forms.Label();
            this.OLD_NoP = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.parametersIDs = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(67, 43);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(294, 20);
            this.dateTimePicker1.TabIndex = 0;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(64, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Generate analysis for next week starting from:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox8);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.NEW_NoP);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.groupBox2.Location = new System.Drawing.Point(330, 222);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(294, 387);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "New version";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.NEW_Cp);
            this.groupBox8.Controls.Add(this.label15);
            this.groupBox8.Location = new System.Drawing.Point(22, 274);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(246, 96);
            this.groupBox8.TabIndex = 10;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Coverage:";
            // 
            // NEW_Cp
            // 
            this.NEW_Cp.AutoSize = true;
            this.NEW_Cp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.NEW_Cp.Location = new System.Drawing.Point(30, 41);
            this.NEW_Cp.Name = "NEW_Cp";
            this.NEW_Cp.Size = new System.Drawing.Size(65, 13);
            this.NEW_Cp.TabIndex = 4;
            this.NEW_Cp.Text = "Percentage:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label15.Location = new System.Drawing.Point(30, 28);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(0, 13);
            this.label15.TabIndex = 2;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.NEW_FPp);
            this.groupBox5.Controls.Add(this.NEW_FPtp);
            this.groupBox5.Location = new System.Drawing.Point(22, 176);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(246, 80);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "False predictions:";
            // 
            // NEW_FPp
            // 
            this.NEW_FPp.AutoSize = true;
            this.NEW_FPp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.NEW_FPp.Location = new System.Drawing.Point(30, 58);
            this.NEW_FPp.Name = "NEW_FPp";
            this.NEW_FPp.Size = new System.Drawing.Size(65, 13);
            this.NEW_FPp.TabIndex = 4;
            this.NEW_FPp.Text = "Percentage:";
            // 
            // NEW_FPtp
            // 
            this.NEW_FPtp.AutoSize = true;
            this.NEW_FPtp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.NEW_FPtp.Location = new System.Drawing.Point(30, 30);
            this.NEW_FPtp.Name = "NEW_FPtp";
            this.NEW_FPtp.Size = new System.Drawing.Size(72, 13);
            this.NEW_FPtp.TabIndex = 2;
            this.NEW_FPtp.Text = "Total number:";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.NEW_CPp);
            this.groupBox6.Controls.Add(this.NEW_CPtp);
            this.groupBox6.Location = new System.Drawing.Point(22, 80);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(246, 84);
            this.groupBox6.TabIndex = 8;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Correct predictions:";
            // 
            // NEW_CPp
            // 
            this.NEW_CPp.AutoSize = true;
            this.NEW_CPp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.NEW_CPp.Location = new System.Drawing.Point(30, 51);
            this.NEW_CPp.Name = "NEW_CPp";
            this.NEW_CPp.Size = new System.Drawing.Size(65, 13);
            this.NEW_CPp.TabIndex = 3;
            this.NEW_CPp.Text = "Percentage:";
            // 
            // NEW_CPtp
            // 
            this.NEW_CPtp.AutoSize = true;
            this.NEW_CPtp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.NEW_CPtp.Location = new System.Drawing.Point(30, 26);
            this.NEW_CPtp.Name = "NEW_CPtp";
            this.NEW_CPtp.Size = new System.Drawing.Size(72, 13);
            this.NEW_CPtp.TabIndex = 1;
            this.NEW_CPtp.Text = "Total number:";
            // 
            // NEW_NoP
            // 
            this.NEW_NoP.AutoSize = true;
            this.NEW_NoP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.NEW_NoP.Location = new System.Drawing.Point(19, 39);
            this.NEW_NoP.Name = "NEW_NoP";
            this.NEW_NoP.Size = new System.Drawing.Size(113, 13);
            this.NEW_NoP.TabIndex = 7;
            this.NEW_NoP.Text = "Number of predictions:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(67, 156);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(515, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox7);
            this.groupBox1.Controls.Add(this.OLD_NoP);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.groupBox1.Location = new System.Drawing.Point(12, 222);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 387);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Old version";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.OLD_Cp);
            this.groupBox3.Location = new System.Drawing.Point(22, 274);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(246, 96);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Coverage:";
            // 
            // OLD_Cp
            // 
            this.OLD_Cp.AutoSize = true;
            this.OLD_Cp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.OLD_Cp.Location = new System.Drawing.Point(30, 41);
            this.OLD_Cp.Name = "OLD_Cp";
            this.OLD_Cp.Size = new System.Drawing.Size(65, 13);
            this.OLD_Cp.TabIndex = 4;
            this.OLD_Cp.Text = "Percentage:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.OLD_FPp);
            this.groupBox4.Controls.Add(this.OLD_FPtp);
            this.groupBox4.Location = new System.Drawing.Point(22, 176);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(246, 80);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "False predictions:";
            // 
            // OLD_FPp
            // 
            this.OLD_FPp.AutoSize = true;
            this.OLD_FPp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.OLD_FPp.Location = new System.Drawing.Point(30, 58);
            this.OLD_FPp.Name = "OLD_FPp";
            this.OLD_FPp.Size = new System.Drawing.Size(65, 13);
            this.OLD_FPp.TabIndex = 4;
            this.OLD_FPp.Text = "Percentage:";
            // 
            // OLD_FPtp
            // 
            this.OLD_FPtp.AutoSize = true;
            this.OLD_FPtp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.OLD_FPtp.Location = new System.Drawing.Point(30, 30);
            this.OLD_FPtp.Name = "OLD_FPtp";
            this.OLD_FPtp.Size = new System.Drawing.Size(72, 13);
            this.OLD_FPtp.TabIndex = 2;
            this.OLD_FPtp.Text = "Total number:";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.OLD_CPp);
            this.groupBox7.Controls.Add(this.OLD_CPtp);
            this.groupBox7.Location = new System.Drawing.Point(22, 80);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(246, 84);
            this.groupBox7.TabIndex = 8;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Correct predictions:";
            // 
            // OLD_CPp
            // 
            this.OLD_CPp.AutoSize = true;
            this.OLD_CPp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.OLD_CPp.Location = new System.Drawing.Point(30, 51);
            this.OLD_CPp.Name = "OLD_CPp";
            this.OLD_CPp.Size = new System.Drawing.Size(65, 13);
            this.OLD_CPp.TabIndex = 3;
            this.OLD_CPp.Text = "Percentage:";
            // 
            // OLD_CPtp
            // 
            this.OLD_CPtp.AutoSize = true;
            this.OLD_CPtp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.OLD_CPtp.Location = new System.Drawing.Point(30, 26);
            this.OLD_CPtp.Name = "OLD_CPtp";
            this.OLD_CPtp.Size = new System.Drawing.Size(72, 13);
            this.OLD_CPtp.TabIndex = 1;
            this.OLD_CPtp.Text = "Total number:";
            // 
            // OLD_NoP
            // 
            this.OLD_NoP.AutoSize = true;
            this.OLD_NoP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.OLD_NoP.Location = new System.Drawing.Point(19, 39);
            this.OLD_NoP.Name = "OLD_NoP";
            this.OLD_NoP.Size = new System.Drawing.Size(113, 13);
            this.OLD_NoP.TabIndex = 7;
            this.OLD_NoP.Text = "Number of predictions:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label13.Location = new System.Drawing.Point(64, 81);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(85, 17);
            this.label13.TabIndex = 6;
            this.label13.Text = "Parameters:";
            // 
            // parametersIDs
            // 
            this.parametersIDs.FormattingEnabled = true;
            this.parametersIDs.Location = new System.Drawing.Point(159, 81);
            this.parametersIDs.Name = "parametersIDs";
            this.parametersIDs.Size = new System.Drawing.Size(101, 21);
            this.parametersIDs.TabIndex = 7;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(412, 43);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(100, 13);
            this.label16.TabIndex = 8;
            this.label16.Text = "Customer Recency:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(412, 71);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(98, 13);
            this.label17.TabIndex = 10;
            this.label17.Text = "Percentage CutOff:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(412, 102);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(71, 13);
            this.label18.TabIndex = 12;
            this.label18.Text = "Count CutOff:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 621);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.parametersIDs);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label NEW_Cp;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label NEW_FPp;
        private System.Windows.Forms.Label NEW_FPtp;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label NEW_CPp;
        private System.Windows.Forms.Label NEW_CPtp;
        private System.Windows.Forms.Label NEW_NoP;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label OLD_Cp;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label OLD_FPp;
        private System.Windows.Forms.Label OLD_FPtp;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label OLD_CPp;
        private System.Windows.Forms.Label OLD_CPtp;
        private System.Windows.Forms.Label OLD_NoP;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox parametersIDs;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
    }
}

