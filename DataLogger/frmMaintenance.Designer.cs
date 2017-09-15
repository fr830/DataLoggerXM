namespace DataLogger
{
    partial class frmMaintenance
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblOperatorHeader = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dtEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtStartTime = new System.Windows.Forms.DateTimePicker();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.txtIDNumber = new System.Windows.Forms.TextBox();
            this.txtOperatorName = new System.Windows.Forms.TextBox();
            this.rbtnIncident = new System.Windows.Forms.RadioButton();
            this.rbtnPeriod = new System.Windows.Forms.RadioButton();
            this.lblMaintenanceReason = new System.Windows.Forms.Label();
            this.lblEndTime = new System.Windows.Forms.Label();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.lblMaintenanceDate = new System.Windows.Forms.Label();
            this.lblIDNumber = new System.Windows.Forms.Label();
            this.lblOperatorName = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblEquipmentsHeader = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rtxtNote = new System.Windows.Forms.RichTextBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.txtOther = new System.Windows.Forms.TextBox();
            this.chkBoxPumpingSystem = new System.Windows.Forms.CheckBox();
            this.chkBoxAutoSampler = new System.Windows.Forms.CheckBox();
            this.chkBoxMPS = new System.Windows.Forms.CheckBox();
            this.chkBoxOther = new System.Windows.Forms.CheckBox();
            this.chkBoxTOC = new System.Windows.Forms.CheckBox();
            this.chkBoxTP = new System.Windows.Forms.CheckBox();
            this.chkBoxTN = new System.Windows.Forms.CheckBox();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(71)))), ((int)(((byte)(117)))));
            this.panel1.Controls.Add(this.lblOperatorHeader);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Location = new System.Drawing.Point(8, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(338, 305);
            this.panel1.TabIndex = 0;
            // 
            // lblOperatorHeader
            // 
            this.lblOperatorHeader.AutoSize = true;
            this.lblOperatorHeader.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperatorHeader.ForeColor = System.Drawing.Color.White;
            this.lblOperatorHeader.Location = new System.Drawing.Point(12, 5);
            this.lblOperatorHeader.Name = "lblOperatorHeader";
            this.lblOperatorHeader.Size = new System.Drawing.Size(128, 25);
            this.lblOperatorHeader.TabIndex = 1;
            this.lblOperatorHeader.Text = "OPERATOR";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.btnStartStop);
            this.panel3.Controls.Add(this.pictureBox2);
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Controls.Add(this.dtEndTime);
            this.panel3.Controls.Add(this.dtStartTime);
            this.panel3.Controls.Add(this.dtDate);
            this.panel3.Controls.Add(this.txtIDNumber);
            this.panel3.Controls.Add(this.txtOperatorName);
            this.panel3.Controls.Add(this.rbtnIncident);
            this.panel3.Controls.Add(this.rbtnPeriod);
            this.panel3.Controls.Add(this.lblMaintenanceReason);
            this.panel3.Controls.Add(this.lblEndTime);
            this.panel3.Controls.Add(this.lblStartTime);
            this.panel3.Controls.Add(this.lblMaintenanceDate);
            this.panel3.Controls.Add(this.lblIDNumber);
            this.panel3.Controls.Add(this.lblOperatorName);
            this.panel3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel3.Location = new System.Drawing.Point(1, 37);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(334, 266);
            this.panel3.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = global::DataLogger.Properties.Resources.Save_Button_1;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.Window;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(160, 205);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(114, 44);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnStartStop
            // 
            this.btnStartStop.BackColor = System.Drawing.Color.Transparent;
            this.btnStartStop.BackgroundImage = global::DataLogger.Properties.Resources.Startbutton;
            this.btnStartStop.FlatAppearance.BorderSize = 0;
            this.btnStartStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStop.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartStop.ForeColor = System.Drawing.SystemColors.Window;
            this.btnStartStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStartStop.Location = new System.Drawing.Point(34, 205);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(105, 46);
            this.btnStartStop.TabIndex = 16;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStartStop.UseVisualStyleBackColor = false;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::DataLogger.Properties.Resources.information;
            this.pictureBox2.InitialImage = global::DataLogger.Properties.Resources.information;
            this.pictureBox2.Location = new System.Drawing.Point(303, 41);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(22, 22);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 14;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DataLogger.Properties.Resources.information;
            this.pictureBox1.InitialImage = global::DataLogger.Properties.Resources.information;
            this.pictureBox1.Location = new System.Drawing.Point(303, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(22, 22);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // dtEndTime
            // 
            this.dtEndTime.CustomFormat = "HH:mm:ss";
            this.dtEndTime.Enabled = false;
            this.dtEndTime.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEndTime.Location = new System.Drawing.Point(161, 131);
            this.dtEndTime.Name = "dtEndTime";
            this.dtEndTime.Size = new System.Drawing.Size(136, 25);
            this.dtEndTime.TabIndex = 12;
            // 
            // dtStartTime
            // 
            this.dtStartTime.CustomFormat = "HH:mm:ss";
            this.dtStartTime.Enabled = false;
            this.dtStartTime.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStartTime.Location = new System.Drawing.Point(161, 101);
            this.dtStartTime.Name = "dtStartTime";
            this.dtStartTime.Size = new System.Drawing.Size(136, 25);
            this.dtStartTime.TabIndex = 11;
            // 
            // dtDate
            // 
            this.dtDate.CustomFormat = "dd/MM/yyyy";
            this.dtDate.Enabled = false;
            this.dtDate.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDate.Location = new System.Drawing.Point(161, 71);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(136, 25);
            this.dtDate.TabIndex = 10;
            // 
            // txtIDNumber
            // 
            this.txtIDNumber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIDNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.txtIDNumber.Location = new System.Drawing.Point(161, 41);
            this.txtIDNumber.Name = "txtIDNumber";
            this.txtIDNumber.Size = new System.Drawing.Size(136, 26);
            this.txtIDNumber.TabIndex = 2;
            // 
            // txtOperatorName
            // 
            this.txtOperatorName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOperatorName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.txtOperatorName.Location = new System.Drawing.Point(161, 11);
            this.txtOperatorName.Name = "txtOperatorName";
            this.txtOperatorName.Size = new System.Drawing.Size(136, 26);
            this.txtOperatorName.TabIndex = 1;
            // 
            // rbtnIncident
            // 
            this.rbtnIncident.BackColor = System.Drawing.Color.Transparent;
            this.rbtnIncident.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnIncident.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.rbtnIncident.Location = new System.Drawing.Point(244, 162);
            this.rbtnIncident.Name = "rbtnIncident";
            this.rbtnIncident.Size = new System.Drawing.Size(89, 23);
            this.rbtnIncident.TabIndex = 4;
            this.rbtnIncident.TabStop = true;
            this.rbtnIncident.Text = "Incident";
            this.rbtnIncident.UseVisualStyleBackColor = false;
            // 
            // rbtnPeriod
            // 
            this.rbtnPeriod.AutoSize = true;
            this.rbtnPeriod.Checked = true;
            this.rbtnPeriod.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnPeriod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.rbtnPeriod.Location = new System.Drawing.Point(160, 162);
            this.rbtnPeriod.Name = "rbtnPeriod";
            this.rbtnPeriod.Size = new System.Drawing.Size(88, 22);
            this.rbtnPeriod.TabIndex = 3;
            this.rbtnPeriod.TabStop = true;
            this.rbtnPeriod.Text = "Periodic";
            this.rbtnPeriod.UseVisualStyleBackColor = true;
            // 
            // lblMaintenanceReason
            // 
            this.lblMaintenanceReason.AutoSize = true;
            this.lblMaintenanceReason.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaintenanceReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.lblMaintenanceReason.Location = new System.Drawing.Point(1, 164);
            this.lblMaintenanceReason.Name = "lblMaintenanceReason";
            this.lblMaintenanceReason.Size = new System.Drawing.Size(163, 18);
            this.lblMaintenanceReason.TabIndex = 5;
            this.lblMaintenanceReason.Text = "Maintenance reason:";
            // 
            // lblEndTime
            // 
            this.lblEndTime.AutoSize = true;
            this.lblEndTime.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.lblEndTime.Location = new System.Drawing.Point(83, 134);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(77, 18);
            this.lblEndTime.TabIndex = 4;
            this.lblEndTime.Text = "End time:";
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.lblStartTime.Location = new System.Drawing.Point(78, 104);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(88, 18);
            this.lblStartTime.TabIndex = 3;
            this.lblStartTime.Text = "Start time:";
            // 
            // lblMaintenanceDate
            // 
            this.lblMaintenanceDate.AutoSize = true;
            this.lblMaintenanceDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaintenanceDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.lblMaintenanceDate.Location = new System.Drawing.Point(20, 74);
            this.lblMaintenanceDate.Name = "lblMaintenanceDate";
            this.lblMaintenanceDate.Size = new System.Drawing.Size(145, 18);
            this.lblMaintenanceDate.TabIndex = 2;
            this.lblMaintenanceDate.Text = "Maintenance date:";
            // 
            // lblIDNumber
            // 
            this.lblIDNumber.AutoSize = true;
            this.lblIDNumber.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIDNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.lblIDNumber.Location = new System.Drawing.Point(73, 44);
            this.lblIDNumber.Name = "lblIDNumber";
            this.lblIDNumber.Size = new System.Drawing.Size(91, 18);
            this.lblIDNumber.TabIndex = 1;
            this.lblIDNumber.Text = "ID number:";
            // 
            // lblOperatorName
            // 
            this.lblOperatorName.AutoSize = true;
            this.lblOperatorName.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperatorName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.lblOperatorName.Location = new System.Drawing.Point(40, 14);
            this.lblOperatorName.Name = "lblOperatorName";
            this.lblOperatorName.Size = new System.Drawing.Size(125, 18);
            this.lblOperatorName.TabIndex = 0;
            this.lblOperatorName.Text = "Operator name:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(71)))), ((int)(((byte)(117)))));
            this.panel2.Controls.Add(this.lblEquipmentsHeader);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Location = new System.Drawing.Point(352, 60);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(276, 305);
            this.panel2.TabIndex = 1;
            // 
            // lblEquipmentsHeader
            // 
            this.lblEquipmentsHeader.AutoSize = true;
            this.lblEquipmentsHeader.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEquipmentsHeader.ForeColor = System.Drawing.Color.White;
            this.lblEquipmentsHeader.Location = new System.Drawing.Point(13, 5);
            this.lblEquipmentsHeader.Name = "lblEquipmentsHeader";
            this.lblEquipmentsHeader.Size = new System.Drawing.Size(155, 25);
            this.lblEquipmentsHeader.TabIndex = 2;
            this.lblEquipmentsHeader.Text = "EQUIPMENTS";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel4.Controls.Add(this.rtxtNote);
            this.panel4.Controls.Add(this.lblNote);
            this.panel4.Controls.Add(this.txtOther);
            this.panel4.Controls.Add(this.chkBoxPumpingSystem);
            this.panel4.Controls.Add(this.chkBoxAutoSampler);
            this.panel4.Controls.Add(this.chkBoxMPS);
            this.panel4.Controls.Add(this.chkBoxOther);
            this.panel4.Controls.Add(this.chkBoxTOC);
            this.panel4.Controls.Add(this.chkBoxTP);
            this.panel4.Controls.Add(this.chkBoxTN);
            this.panel4.Location = new System.Drawing.Point(2, 38);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(273, 265);
            this.panel4.TabIndex = 1;
            // 
            // rtxtNote
            // 
            this.rtxtNote.BackColor = System.Drawing.Color.White;
            this.rtxtNote.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtNote.Location = new System.Drawing.Point(16, 160);
            this.rtxtNote.Name = "rtxtNote";
            this.rtxtNote.Size = new System.Drawing.Size(235, 93);
            this.rtxtNote.TabIndex = 14;
            this.rtxtNote.Text = "";
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.Location = new System.Drawing.Point(16, 133);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(49, 18);
            this.lblNote.TabIndex = 6;
            this.lblNote.Text = "Note:";
            // 
            // txtOther
            // 
            this.txtOther.BackColor = System.Drawing.Color.Snow;
            this.txtOther.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOther.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOther.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.txtOther.Location = new System.Drawing.Point(87, 108);
            this.txtOther.Name = "txtOther";
            this.txtOther.Size = new System.Drawing.Size(120, 17);
            this.txtOther.TabIndex = 13;
            // 
            // chkBoxPumpingSystem
            // 
            this.chkBoxPumpingSystem.AutoSize = true;
            this.chkBoxPumpingSystem.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxPumpingSystem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.chkBoxPumpingSystem.Location = new System.Drawing.Point(125, 74);
            this.chkBoxPumpingSystem.Name = "chkBoxPumpingSystem";
            this.chkBoxPumpingSystem.Size = new System.Drawing.Size(135, 22);
            this.chkBoxPumpingSystem.TabIndex = 11;
            this.chkBoxPumpingSystem.Text = "Pumping system";
            this.chkBoxPumpingSystem.UseVisualStyleBackColor = true;
            // 
            // chkBoxAutoSampler
            // 
            this.chkBoxAutoSampler.AutoSize = true;
            this.chkBoxAutoSampler.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxAutoSampler.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.chkBoxAutoSampler.Location = new System.Drawing.Point(125, 44);
            this.chkBoxAutoSampler.Name = "chkBoxAutoSampler";
            this.chkBoxAutoSampler.Size = new System.Drawing.Size(113, 22);
            this.chkBoxAutoSampler.TabIndex = 10;
            this.chkBoxAutoSampler.Text = "Auto sampler";
            this.chkBoxAutoSampler.UseVisualStyleBackColor = true;
            // 
            // chkBoxMPS
            // 
            this.chkBoxMPS.AutoSize = true;
            this.chkBoxMPS.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxMPS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.chkBoxMPS.Location = new System.Drawing.Point(125, 14);
            this.chkBoxMPS.Name = "chkBoxMPS";
            this.chkBoxMPS.Size = new System.Drawing.Size(55, 22);
            this.chkBoxMPS.TabIndex = 9;
            this.chkBoxMPS.Text = "MPS";
            this.chkBoxMPS.UseVisualStyleBackColor = true;
            // 
            // chkBoxOther
            // 
            this.chkBoxOther.AutoSize = true;
            this.chkBoxOther.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxOther.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.chkBoxOther.Location = new System.Drawing.Point(16, 104);
            this.chkBoxOther.Name = "chkBoxOther";
            this.chkBoxOther.Size = new System.Drawing.Size(64, 22);
            this.chkBoxOther.TabIndex = 12;
            this.chkBoxOther.Text = "Other";
            this.chkBoxOther.UseVisualStyleBackColor = true;
            // 
            // chkBoxTOC
            // 
            this.chkBoxTOC.AutoSize = true;
            this.chkBoxTOC.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxTOC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.chkBoxTOC.Location = new System.Drawing.Point(16, 74);
            this.chkBoxTOC.Name = "chkBoxTOC";
            this.chkBoxTOC.Size = new System.Drawing.Size(57, 22);
            this.chkBoxTOC.TabIndex = 7;
            this.chkBoxTOC.Text = "TOC";
            this.chkBoxTOC.UseVisualStyleBackColor = true;
            // 
            // chkBoxTP
            // 
            this.chkBoxTP.AutoSize = true;
            this.chkBoxTP.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxTP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.chkBoxTP.Location = new System.Drawing.Point(16, 44);
            this.chkBoxTP.Name = "chkBoxTP";
            this.chkBoxTP.Size = new System.Drawing.Size(45, 22);
            this.chkBoxTP.TabIndex = 6;
            this.chkBoxTP.Text = "TP";
            this.chkBoxTP.UseVisualStyleBackColor = true;
            // 
            // chkBoxTN
            // 
            this.chkBoxTN.AutoSize = true;
            this.chkBoxTN.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.chkBoxTN.Location = new System.Drawing.Point(16, 14);
            this.chkBoxTN.Name = "chkBoxTN";
            this.chkBoxTN.Size = new System.Drawing.Size(47, 22);
            this.chkBoxTN.TabIndex = 5;
            this.chkBoxTN.Text = "TN";
            this.chkBoxTN.UseVisualStyleBackColor = true;
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(16)))), ((int)(((byte)(33)))));
            this.lblHeaderTitle.Location = new System.Drawing.Point(163, 11);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(310, 35);
            this.lblHeaderTitle.TabIndex = 2;
            this.lblHeaderTitle.Text = "Maintenance Report";
            // 
            // frmMaintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 371);
            this.Controls.Add(this.lblHeaderTitle);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMaintenance";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Maintenance Report";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMaintenance_FormClosing);
            this.Load += new System.EventHandler(this.frmMaintenance_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblOperatorHeader;
        private System.Windows.Forms.Label lblEquipmentsHeader;
        private System.Windows.Forms.Label lblMaintenanceReason;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.Label lblMaintenanceDate;
        private System.Windows.Forms.Label lblIDNumber;
        private System.Windows.Forms.Label lblOperatorName;
        private System.Windows.Forms.CheckBox chkBoxPumpingSystem;
        private System.Windows.Forms.CheckBox chkBoxAutoSampler;
        private System.Windows.Forms.CheckBox chkBoxMPS;
        private System.Windows.Forms.CheckBox chkBoxOther;
        private System.Windows.Forms.CheckBox chkBoxTOC;
        private System.Windows.Forms.CheckBox chkBoxTP;
        private System.Windows.Forms.CheckBox chkBoxTN;
        private System.Windows.Forms.TextBox txtOther;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.RichTextBox rtxtNote;
        private System.Windows.Forms.RadioButton rbtnIncident;
        private System.Windows.Forms.RadioButton rbtnPeriod;
        private System.Windows.Forms.TextBox txtIDNumber;
        private System.Windows.Forms.TextBox txtOperatorName;
        private System.Windows.Forms.DateTimePicker dtEndTime;
        private System.Windows.Forms.DateTimePicker dtStartTime;
        private System.Windows.Forms.DateTimePicker dtDate;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Button btnSave;

    }
}