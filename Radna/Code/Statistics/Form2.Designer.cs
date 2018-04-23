﻿namespace Statistics
{
    partial class Form2
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.oldCount = new System.Windows.Forms.Label();
            this.oldPredictions = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.newCount = new System.Windows.Forms.Label();
            this.newPredictions = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.purchasesCount = new System.Windows.Forms.Label();
            this.listOfPurchases = new System.Windows.Forms.ListBox();
            this.intersectPredictions = new System.Windows.Forms.GroupBox();
            this.intersectionCount = new System.Windows.Forms.Label();
            this.intersection = new System.Windows.Forms.ListBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.vennDiagram = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.intersectPredictions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vennDiagram)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.oldCount);
            this.groupBox1.Controls.Add(this.oldPredictions);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(228, 303);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OLD VERSION, correct predictions";
            // 
            // oldCount
            // 
            this.oldCount.AutoSize = true;
            this.oldCount.Location = new System.Drawing.Point(16, 29);
            this.oldCount.Name = "oldCount";
            this.oldCount.Size = new System.Drawing.Size(38, 13);
            this.oldCount.TabIndex = 1;
            this.oldCount.Text = "Count:";
            // 
            // oldPredictions
            // 
            this.oldPredictions.FormattingEnabled = true;
            this.oldPredictions.Location = new System.Drawing.Point(19, 56);
            this.oldPredictions.Name = "oldPredictions";
            this.oldPredictions.Size = new System.Drawing.Size(187, 225);
            this.oldPredictions.TabIndex = 0;
            this.oldPredictions.SelectedIndexChanged += new System.EventHandler(this.OldSelected);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.newCount);
            this.groupBox2.Controls.Add(this.newPredictions);
            this.groupBox2.Location = new System.Drawing.Point(275, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(228, 303);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "NEW VERSION, correct predictions";
            // 
            // newCount
            // 
            this.newCount.AutoSize = true;
            this.newCount.Location = new System.Drawing.Point(18, 29);
            this.newCount.Name = "newCount";
            this.newCount.Size = new System.Drawing.Size(38, 13);
            this.newCount.TabIndex = 2;
            this.newCount.Text = "Count:";
            // 
            // newPredictions
            // 
            this.newPredictions.FormattingEnabled = true;
            this.newPredictions.Location = new System.Drawing.Point(21, 56);
            this.newPredictions.Name = "newPredictions";
            this.newPredictions.Size = new System.Drawing.Size(187, 225);
            this.newPredictions.TabIndex = 1;
            this.newPredictions.SelectedIndexChanged += new System.EventHandler(this.newSelected);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.purchasesCount);
            this.groupBox3.Controls.Add(this.listOfPurchases);
            this.groupBox3.Location = new System.Drawing.Point(806, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(228, 303);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "List of Purchases";
            // 
            // purchasesCount
            // 
            this.purchasesCount.AutoSize = true;
            this.purchasesCount.Location = new System.Drawing.Point(19, 29);
            this.purchasesCount.Name = "purchasesCount";
            this.purchasesCount.Size = new System.Drawing.Size(38, 13);
            this.purchasesCount.TabIndex = 2;
            this.purchasesCount.Text = "Count:";
            // 
            // listOfPurchases
            // 
            this.listOfPurchases.FormattingEnabled = true;
            this.listOfPurchases.Location = new System.Drawing.Point(22, 56);
            this.listOfPurchases.Name = "listOfPurchases";
            this.listOfPurchases.Size = new System.Drawing.Size(187, 225);
            this.listOfPurchases.TabIndex = 1;
            // 
            // intersectPredictions
            // 
            this.intersectPredictions.Controls.Add(this.intersectionCount);
            this.intersectPredictions.Controls.Add(this.intersection);
            this.intersectPredictions.Location = new System.Drawing.Point(539, 12);
            this.intersectPredictions.Name = "intersectPredictions";
            this.intersectPredictions.Size = new System.Drawing.Size(228, 303);
            this.intersectPredictions.TabIndex = 3;
            this.intersectPredictions.TabStop = false;
            this.intersectPredictions.Text = "INTERSECTION, correct predictions";
            // 
            // intersectionCount
            // 
            this.intersectionCount.AutoSize = true;
            this.intersectionCount.Location = new System.Drawing.Point(19, 29);
            this.intersectionCount.Name = "intersectionCount";
            this.intersectionCount.Size = new System.Drawing.Size(38, 13);
            this.intersectionCount.TabIndex = 2;
            this.intersectionCount.Text = "Count:";
            // 
            // intersection
            // 
            this.intersection.FormattingEnabled = true;
            this.intersection.Location = new System.Drawing.Point(22, 56);
            this.intersection.Name = "intersection";
            this.intersection.Size = new System.Drawing.Size(187, 225);
            this.intersection.TabIndex = 1;
            this.intersection.SelectedIndexChanged += new System.EventHandler(this.intersection_SelectedIndexChanged);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(31, 345);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "New algorithm";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Old algorithm";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(452, 300);
            this.chart1.TabIndex = 4;
            this.chart1.Text = "chart1";
            title1.Name = "Title1";
            title1.Text = "Common compares";
            this.chart1.Titles.Add(title1);
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // vennDiagram
            // 
            this.vennDiagram.Location = new System.Drawing.Point(561, 345);
            this.vennDiagram.Name = "vennDiagram";
            this.vennDiagram.Size = new System.Drawing.Size(454, 300);
            this.vennDiagram.TabIndex = 5;
            this.vennDiagram.TabStop = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 673);
            this.Controls.Add(this.vennDiagram);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.intersectPredictions);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Shown += new System.EventHandler(this.onShow);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.intersectPredictions.ResumeLayout(false);
            this.intersectPredictions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vennDiagram)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox oldPredictions;
        private System.Windows.Forms.ListBox newPredictions;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listOfPurchases;
        private System.Windows.Forms.GroupBox intersectPredictions;
        private System.Windows.Forms.ListBox intersection;
        private System.Windows.Forms.Label oldCount;
        private System.Windows.Forms.Label newCount;
        private System.Windows.Forms.Label purchasesCount;
        private System.Windows.Forms.Label intersectionCount;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.PictureBox vennDiagram;
    }
}