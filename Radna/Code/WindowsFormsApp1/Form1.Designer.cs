namespace WindowsFormsApp1
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
            this.StartB = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.recency = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.percentage = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.count = new System.Windows.Forms.TextBox();
            this.StopB = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.selectPreparer = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.calculator = new System.Windows.Forms.ComboBox();
            this.preparer = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.useGPI = new System.Windows.Forms.RadioButton();
            this.regularItems = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.gpiDigits = new System.Windows.Forms.Label();
            this.selectGPIdigits = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.GPIresult = new System.Windows.Forms.ComboBox();
            this.selectGPIresult = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.recency)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectGPIdigits)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartB
            // 
            this.StartB.Location = new System.Drawing.Point(173, 429);
            this.StartB.Name = "StartB";
            this.StartB.Size = new System.Drawing.Size(75, 23);
            this.StartB.TabIndex = 0;
            this.StartB.Text = "Start!";
            this.StartB.UseVisualStyleBackColor = true;
            this.StartB.Click += new System.EventHandler(this.button1_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Done: 0/0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Percentage 0%";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Start: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Last Write:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Total Writes:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(173, 384);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(232, 20);
            this.dateTimePicker1.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Customer Recency:";
            // 
            // recency
            // 
            this.recency.Location = new System.Drawing.Point(141, 49);
            this.recency.Name = "recency";
            this.recency.Size = new System.Drawing.Size(52, 20);
            this.recency.TabIndex = 8;
            this.recency.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.recency.ValueChanged += new System.EventHandler(this.recency_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Percentage CutOff:";
            // 
            // percentage
            // 
            this.percentage.Location = new System.Drawing.Point(146, 44);
            this.percentage.Name = "percentage";
            this.percentage.Size = new System.Drawing.Size(84, 20);
            this.percentage.TabIndex = 10;
            this.percentage.Text = "10";
            this.percentage.TextChanged += new System.EventHandler(this.percentage_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Count CutOff:";
            // 
            // count
            // 
            this.count.Location = new System.Drawing.Point(146, 115);
            this.count.Name = "count";
            this.count.Size = new System.Drawing.Size(84, 20);
            this.count.TabIndex = 12;
            this.count.Text = "30";
            this.count.TextChanged += new System.EventHandler(this.count_TextChanged);
            // 
            // StopB
            // 
            this.StopB.Enabled = false;
            this.StopB.Location = new System.Drawing.Point(330, 429);
            this.StopB.Name = "StopB";
            this.StopB.Size = new System.Drawing.Size(75, 23);
            this.StopB.TabIndex = 13;
            this.StopB.Text = "Cancel!";
            this.StopB.UseVisualStyleBackColor = true;
            this.StopB.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(16, 576);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(546, 23);
            this.progressBar1.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(29, 72);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(217, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "(How far back we should look for customers)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(31, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(174, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "(Bottom limit of prediction accuracy)";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(31, 138);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(217, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "(Upper limit of prediction count per customer)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(170, 356);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(235, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "Generate predictions for next week starting from:";
            // 
            // selectPreparer
            // 
            this.selectPreparer.AutoSize = true;
            this.selectPreparer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.selectPreparer.Location = new System.Drawing.Point(26, 182);
            this.selectPreparer.Name = "selectPreparer";
            this.selectPreparer.Size = new System.Drawing.Size(91, 13);
            this.selectPreparer.TabIndex = 19;
            this.selectPreparer.Text = "Select a preparer:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(27, 44);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(98, 13);
            this.label14.TabIndex = 20;
            this.label14.Text = "Select a calculator:";
            // 
            // calculator
            // 
            this.calculator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.calculator.FormattingEnabled = true;
            this.calculator.Location = new System.Drawing.Point(140, 41);
            this.calculator.Name = "calculator";
            this.calculator.Size = new System.Drawing.Size(86, 21);
            this.calculator.TabIndex = 21;
            this.calculator.SelectedIndexChanged += new System.EventHandler(this.calculator_SelectedIndexChanged);
            // 
            // preparer
            // 
            this.preparer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.preparer.FormattingEnabled = true;
            this.preparer.Location = new System.Drawing.Point(141, 179);
            this.preparer.Name = "preparer";
            this.preparer.Size = new System.Drawing.Size(105, 21);
            this.preparer.TabIndex = 22;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(16, 474);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 95);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Completion";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(370, 474);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 95);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Times";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.useGPI);
            this.groupBox3.Controls.Add(this.regularItems);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.calculator);
            this.groupBox3.Location = new System.Drawing.Point(115, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(368, 90);
            this.groupBox3.TabIndex = 25;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Stage ONE: choose algorithm";
            // 
            // useGPI
            // 
            this.useGPI.AutoSize = true;
            this.useGPI.Location = new System.Drawing.Point(242, 54);
            this.useGPI.Name = "useGPI";
            this.useGPI.Size = new System.Drawing.Size(43, 17);
            this.useGPI.TabIndex = 23;
            this.useGPI.TabStop = true;
            this.useGPI.Text = "GPI";
            this.useGPI.UseVisualStyleBackColor = true;
            this.useGPI.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // regularItems
            // 
            this.regularItems.AutoSize = true;
            this.regularItems.Location = new System.Drawing.Point(242, 31);
            this.regularItems.Name = "regularItems";
            this.regularItems.Size = new System.Drawing.Size(89, 17);
            this.regularItems.TabIndex = 22;
            this.regularItems.TabStop = true;
            this.regularItems.Text = "Regular items";
            this.regularItems.UseVisualStyleBackColor = true;
            this.regularItems.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.gpiDigits);
            this.groupBox4.Controls.Add(this.selectGPIdigits);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.recency);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.preparer);
            this.groupBox4.Controls.Add(this.selectPreparer);
            this.groupBox4.Location = new System.Drawing.Point(16, 116);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(261, 218);
            this.groupBox4.TabIndex = 26;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Stage TWO: prepare data";
            // 
            // gpiDigits
            // 
            this.gpiDigits.AutoSize = true;
            this.gpiDigits.Location = new System.Drawing.Point(26, 118);
            this.gpiDigits.Name = "gpiDigits";
            this.gpiDigits.Size = new System.Drawing.Size(55, 13);
            this.gpiDigits.TabIndex = 23;
            this.gpiDigits.Text = "GPI digits:";
            // 
            // selectGPIdigits
            // 
            this.selectGPIdigits.Location = new System.Drawing.Point(141, 116);
            this.selectGPIdigits.Name = "selectGPIdigits";
            this.selectGPIdigits.Size = new System.Drawing.Size(105, 20);
            this.selectGPIdigits.TabIndex = 24;
            this.selectGPIdigits.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(199, 51);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(47, 13);
            this.label15.TabIndex = 16;
            this.label15.Text = "month(s)";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.GPIresult);
            this.groupBox5.Controls.Add(this.selectGPIresult);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.percentage);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.count);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Location = new System.Drawing.Point(303, 116);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(259, 218);
            this.groupBox5.TabIndex = 27;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Stage THREE: store data";
            // 
            // GPIresult
            // 
            this.GPIresult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GPIresult.FormattingEnabled = true;
            this.GPIresult.Location = new System.Drawing.Point(144, 179);
            this.GPIresult.Name = "GPIresult";
            this.GPIresult.Size = new System.Drawing.Size(86, 21);
            this.GPIresult.TabIndex = 24;
            // 
            // selectGPIresult
            // 
            this.selectGPIresult.AutoSize = true;
            this.selectGPIresult.Location = new System.Drawing.Point(29, 182);
            this.selectGPIresult.Name = "selectGPIresult";
            this.selectGPIresult.Size = new System.Drawing.Size(89, 13);
            this.selectGPIresult.TabIndex = 23;
            this.selectGPIresult.Text = "Select GPI result:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 619);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.StopB);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.StartB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.recency)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectGPIdigits)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartB;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown recency;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox percentage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox count;
        private System.Windows.Forms.Button StopB;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label selectPreparer;
        private System.Windows.Forms.Label label14;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.ComboBox calculator;
        private System.Windows.Forms.ComboBox preparer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton useGPI;
        private System.Windows.Forms.RadioButton regularItems;
        private System.Windows.Forms.Label gpiDigits;
        private System.Windows.Forms.NumericUpDown selectGPIdigits;
        private System.Windows.Forms.ComboBox GPIresult;
        private System.Windows.Forms.Label selectGPIresult;
    }
}

