namespace DataLogger
{
    partial class frmReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReport));
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.dteFinish = new System.Windows.Forms.DateTimePicker();
            this.dteStart = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.opt1Hour = new System.Windows.Forms.RadioButton();
            this.opt5min = new System.Windows.Forms.RadioButton();
            this.btnDataValue = new System.Windows.Forms.Button();
            this.btnTrend = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "pH",
            "EC",
            "DO",
            "Turbidity",
            "ORP",
            "Temp",
            "TN",
            "TP",
            "TOC",
            "SAM00(Temp)",
            "SAM01(Bottle)",
            "SAM02(Door)",
            "Power",
            "UPS",
            "Door"});
            this.checkedListBox1.Location = new System.Drawing.Point(2, 202);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(249, 372);
            this.checkedListBox1.TabIndex = 0;
            // 
            // dteFinish
            // 
            this.dteFinish.CustomFormat = "yyyy-MM-dd   HH:mm:ss";
            this.dteFinish.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dteFinish.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dteFinish.Location = new System.Drawing.Point(69, 39);
            this.dteFinish.Name = "dteFinish";
            this.dteFinish.Size = new System.Drawing.Size(174, 21);
            this.dteFinish.TabIndex = 1;
            this.dteFinish.Value = new System.DateTime(2015, 11, 18, 0, 0, 0, 0);
            // 
            // dteStart
            // 
            this.dteStart.CustomFormat = "yyyy-MM-dd   HH:mm:ss";
            this.dteStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dteStart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dteStart.Location = new System.Drawing.Point(69, 8);
            this.dteStart.Name = "dteStart";
            this.dteStart.Size = new System.Drawing.Size(174, 21);
            this.dteStart.TabIndex = 1;
            this.dteStart.Value = new System.DateTime(2015, 11, 18, 0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Start";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Finish";
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAll.Location = new System.Drawing.Point(23, 175);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(71, 19);
            this.chkAll.TabIndex = 1;
            this.chkAll.Text = "All Items";
            this.chkAll.UseVisualStyleBackColor = true;
            // 
            // opt1Hour
            // 
            this.opt1Hour.AutoSize = true;
            this.opt1Hour.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opt1Hour.Location = new System.Drawing.Point(124, 149);
            this.opt1Hour.Name = "opt1Hour";
            this.opt1Hour.Size = new System.Drawing.Size(85, 19);
            this.opt1Hour.TabIndex = 0;
            this.opt1Hour.Text = "1 Hour Data";
            this.opt1Hour.UseVisualStyleBackColor = true;
            // 
            // opt5min
            // 
            this.opt5min.AutoSize = true;
            this.opt5min.Checked = true;
            this.opt5min.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opt5min.Location = new System.Drawing.Point(23, 149);
            this.opt5min.Name = "opt5min";
            this.opt5min.Size = new System.Drawing.Size(84, 19);
            this.opt5min.TabIndex = 0;
            this.opt5min.TabStop = true;
            this.opt5min.Text = "5 Min. Data";
            this.opt5min.UseVisualStyleBackColor = true;
            // 
            // btnDataValue
            // 
            this.btnDataValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(95)))), ((int)(((byte)(133)))));
            this.btnDataValue.FlatAppearance.BorderSize = 0;
            this.btnDataValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDataValue.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDataValue.ForeColor = System.Drawing.SystemColors.Window;
            this.btnDataValue.Location = new System.Drawing.Point(1, 592);
            this.btnDataValue.Name = "btnDataValue";
            this.btnDataValue.Size = new System.Drawing.Size(122, 27);
            this.btnDataValue.TabIndex = 5;
            this.btnDataValue.Text = "Data Value";
            this.btnDataValue.UseVisualStyleBackColor = false;
            // 
            // btnTrend
            // 
            this.btnTrend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(95)))), ((int)(((byte)(133)))));
            this.btnTrend.FlatAppearance.BorderSize = 0;
            this.btnTrend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrend.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrend.ForeColor = System.Drawing.SystemColors.Window;
            this.btnTrend.Location = new System.Drawing.Point(128, 592);
            this.btnTrend.Name = "btnTrend";
            this.btnTrend.Size = new System.Drawing.Size(122, 27);
            this.btnTrend.TabIndex = 5;
            this.btnTrend.Text = "Trend";
            this.btnTrend.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(95)))), ((int)(((byte)(133)))));
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.Window;
            this.btnSave.Location = new System.Drawing.Point(1, 620);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(250, 27);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(71)))), ((int)(((byte)(117)))));
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(1, 1);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(17, 6, 17, 6);
            this.label3.Size = new System.Drawing.Size(251, 32);
            this.label3.TabIndex = 2;
            this.label3.Text = "Inquiry of Station Data";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkedListBox1);
            this.panel2.Controls.Add(this.chkAll);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.opt1Hour);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.btnTrend);
            this.panel2.Controls.Add(this.opt5min);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnDataValue);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(1);
            this.panel2.Size = new System.Drawing.Size(252, 648);
            this.panel2.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(71)))), ((int)(((byte)(117)))));
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(1, 108);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(17, 6, 17, 6);
            this.label4.Size = new System.Drawing.Size(251, 32);
            this.label4.TabIndex = 7;
            this.label4.Text = "Inquiry of Station Data";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dteStart);
            this.panel1.Controls.Add(this.dteFinish);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(1, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(251, 75);
            this.panel1.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(71)))), ((int)(((byte)(117)))));
            this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(253, 0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(17, 6, 17, 6);
            this.label5.Size = new System.Drawing.Size(660, 32);
            this.label5.TabIndex = 8;
            this.label5.Text = "Data";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel3.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel3.Location = new System.Drawing.Point(252, 33);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(661, 615);
            this.panel3.TabIndex = 9;
            // 
            // frmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 648);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.DateTimePicker dteFinish;
        private System.Windows.Forms.DateTimePicker dteStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.RadioButton opt1Hour;
        private System.Windows.Forms.RadioButton opt5min;
        private System.Windows.Forms.Button btnDataValue;
        private System.Windows.Forms.Button btnTrend;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel3;
    }
}