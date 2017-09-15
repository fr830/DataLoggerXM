namespace DataLogger
{
    partial class frmHistoryMPS
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
            this.grbSelect = new System.Windows.Forms.GroupBox();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlViewType = new System.Windows.Forms.Panel();
            this.btnGraphType = new DataLogger.FlatButton();
            this.btnListType = new DataLogger.FlatButton();
            this.lblListType = new System.Windows.Forms.Label();
            this.lblGraphType = new System.Windows.Forms.Label();
            this.pnlDataType = new System.Windows.Forms.Panel();
            this.btn60Minute = new DataLogger.FlatButton();
            this.btn5Minute = new DataLogger.FlatButton();
            this.lbl5MinData = new System.Windows.Forms.Label();
            this.lbl1HourData = new System.Windows.Forms.Label();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.grbPreview = new System.Windows.Forms.GroupBox();
            this.pnlDataGrid = new System.Windows.Forms.Panel();
            this.chtData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDateToVal = new System.Windows.Forms.Label();
            this.lblIntevalTimeName = new System.Windows.Forms.Label();
            this.lblDateFromVal = new System.Windows.Forms.Label();
            this.lblViewType = new System.Windows.Forms.Label();
            this.lblIntevalTimeVal = new System.Windows.Forms.Label();
            this.lblDateFromName = new System.Windows.Forms.Label();
            this.lblDateToName = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.grbSelect.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlViewType.SuspendLayout();
            this.pnlDataType.SuspendLayout();
            this.grbPreview.SuspendLayout();
            this.pnlDataGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.pnlInfo.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbSelect
            // 
            this.grbSelect.Controls.Add(this.pnlFooter);
            this.grbSelect.Controls.Add(this.pnlViewType);
            this.grbSelect.Controls.Add(this.pnlDataType);
            this.grbSelect.Controls.Add(this.dtpDateTo);
            this.grbSelect.Controls.Add(this.dtpDateFrom);
            this.grbSelect.Controls.Add(this.lblTo);
            this.grbSelect.Controls.Add(this.lblFrom);
            this.grbSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbSelect.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbSelect.ForeColor = System.Drawing.Color.Gray;
            this.grbSelect.Location = new System.Drawing.Point(0, 0);
            this.grbSelect.Name = "grbSelect";
            this.grbSelect.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.grbSelect.Size = new System.Drawing.Size(193, 493);
            this.grbSelect.TabIndex = 54;
            this.grbSelect.TabStop = false;
            this.grbSelect.Text = "Select";
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.btnOK);
            this.pnlFooter.Controls.Add(this.btnCancel);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(3, 458);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(187, 32);
            this.pnlFooter.TabIndex = 58;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(95)))), ((int)(((byte)(133)))));
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(3, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 55;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(95)))), ((int)(((byte)(133)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(109, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 55;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlViewType
            // 
            this.pnlViewType.Controls.Add(this.btnGraphType);
            this.pnlViewType.Controls.Add(this.btnListType);
            this.pnlViewType.Controls.Add(this.lblListType);
            this.pnlViewType.Controls.Add(this.lblGraphType);
            this.pnlViewType.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlViewType.Location = new System.Drawing.Point(3, 101);
            this.pnlViewType.Name = "pnlViewType";
            this.pnlViewType.Size = new System.Drawing.Size(187, 70);
            this.pnlViewType.TabIndex = 57;
            // 
            // btnGraphType
            // 
            this.btnGraphType.BackgroundImage = global::DataLogger.Properties.Resources.Graph_data__noborder;
            this.btnGraphType.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGraphType.BoderWidthActive = 2;
            this.btnGraphType.BoderWidthNormal = 1;
            this.btnGraphType.ColorBoderActive = System.Drawing.Color.Crimson;
            this.btnGraphType.ColorBoderHover = System.Drawing.Color.Gray;
            this.btnGraphType.ColorBoderNormal = System.Drawing.Color.Silver;
            this.btnGraphType.IsActive = false;
            this.btnGraphType.Location = new System.Drawing.Point(108, 6);
            this.btnGraphType.Margin = new System.Windows.Forms.Padding(6);
            this.btnGraphType.Name = "btnGraphType";
            this.btnGraphType.Size = new System.Drawing.Size(42, 42);
            this.btnGraphType.TabIndex = 4;
            this.btnGraphType.ToolTipHint = "";
            this.btnGraphType.Click += new System.EventHandler(this.btnGraphType_Click);
            // 
            // btnListType
            // 
            this.btnListType.BackgroundImage = global::DataLogger.Properties.Resources.List_data__noborder;
            this.btnListType.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnListType.BoderWidthActive = 2;
            this.btnListType.BoderWidthNormal = 1;
            this.btnListType.ColorBoderActive = System.Drawing.Color.Crimson;
            this.btnListType.ColorBoderHover = System.Drawing.Color.Gray;
            this.btnListType.ColorBoderNormal = System.Drawing.Color.Silver;
            this.btnListType.IsActive = true;
            this.btnListType.Location = new System.Drawing.Point(29, 6);
            this.btnListType.Margin = new System.Windows.Forms.Padding(6);
            this.btnListType.Name = "btnListType";
            this.btnListType.Size = new System.Drawing.Size(42, 42);
            this.btnListType.TabIndex = 3;
            this.btnListType.ToolTipHint = "";
            this.btnListType.Click += new System.EventHandler(this.btnListType_Click);
            // 
            // lblListType
            // 
            this.lblListType.AutoSize = true;
            this.lblListType.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListType.ForeColor = System.Drawing.Color.Black;
            this.lblListType.Location = new System.Drawing.Point(27, 49);
            this.lblListType.Name = "lblListType";
            this.lblListType.Size = new System.Drawing.Size(52, 15);
            this.lblListType.TabIndex = 1;
            this.lblListType.Text = "List type";
            // 
            // lblGraphType
            // 
            this.lblGraphType.AutoSize = true;
            this.lblGraphType.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGraphType.ForeColor = System.Drawing.Color.Black;
            this.lblGraphType.Location = new System.Drawing.Point(100, 50);
            this.lblGraphType.Name = "lblGraphType";
            this.lblGraphType.Size = new System.Drawing.Size(64, 15);
            this.lblGraphType.TabIndex = 2;
            this.lblGraphType.Text = "Graph type";
            // 
            // pnlDataType
            // 
            this.pnlDataType.Controls.Add(this.btn60Minute);
            this.pnlDataType.Controls.Add(this.btn5Minute);
            this.pnlDataType.Controls.Add(this.lbl5MinData);
            this.pnlDataType.Controls.Add(this.lbl1HourData);
            this.pnlDataType.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDataType.Location = new System.Drawing.Point(3, 31);
            this.pnlDataType.Name = "pnlDataType";
            this.pnlDataType.Size = new System.Drawing.Size(187, 70);
            this.pnlDataType.TabIndex = 56;
            // 
            // btn60Minute
            // 
            this.btn60Minute.BackgroundImage = global::DataLogger.Properties.Resources.Icon_1_hour_data__noborder;
            this.btn60Minute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn60Minute.BoderWidthActive = 2;
            this.btn60Minute.BoderWidthNormal = 1;
            this.btn60Minute.ColorBoderActive = System.Drawing.Color.Crimson;
            this.btn60Minute.ColorBoderHover = System.Drawing.Color.Gray;
            this.btn60Minute.ColorBoderNormal = System.Drawing.Color.Silver;
            this.btn60Minute.IsActive = false;
            this.btn60Minute.Location = new System.Drawing.Point(109, 6);
            this.btn60Minute.Margin = new System.Windows.Forms.Padding(12);
            this.btn60Minute.Name = "btn60Minute";
            this.btn60Minute.Size = new System.Drawing.Size(42, 42);
            this.btn60Minute.TabIndex = 5;
            this.btn60Minute.ToolTipHint = "";
            this.btn60Minute.Click += new System.EventHandler(this.btn60Minute_Click);
            // 
            // btn5Minute
            // 
            this.btn5Minute.BackgroundImage = global::DataLogger.Properties.Resources.Icon_5_minutes_data__noborder;
            this.btn5Minute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn5Minute.BoderWidthActive = 2;
            this.btn5Minute.BoderWidthNormal = 1;
            this.btn5Minute.ColorBoderActive = System.Drawing.Color.Crimson;
            this.btn5Minute.ColorBoderHover = System.Drawing.Color.Gray;
            this.btn5Minute.ColorBoderNormal = System.Drawing.Color.Silver;
            this.btn5Minute.IsActive = true;
            this.btn5Minute.Location = new System.Drawing.Point(29, 6);
            this.btn5Minute.Margin = new System.Windows.Forms.Padding(6);
            this.btn5Minute.Name = "btn5Minute";
            this.btn5Minute.Size = new System.Drawing.Size(42, 42);
            this.btn5Minute.TabIndex = 3;
            this.btn5Minute.ToolTipHint = "";
            this.btn5Minute.Click += new System.EventHandler(this.btn5Minute_Click);
            // 
            // lbl5MinData
            // 
            this.lbl5MinData.AutoSize = true;
            this.lbl5MinData.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl5MinData.ForeColor = System.Drawing.Color.Black;
            this.lbl5MinData.Location = new System.Drawing.Point(22, 49);
            this.lbl5MinData.Name = "lbl5MinData";
            this.lbl5MinData.Size = new System.Drawing.Size(60, 15);
            this.lbl5MinData.TabIndex = 1;
            this.lbl5MinData.Text = "5 Min data";
            // 
            // lbl1HourData
            // 
            this.lbl1HourData.AutoSize = true;
            this.lbl1HourData.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl1HourData.ForeColor = System.Drawing.Color.Black;
            this.lbl1HourData.Location = new System.Drawing.Point(98, 50);
            this.lbl1HourData.Name = "lbl1HourData";
            this.lbl1HourData.Size = new System.Drawing.Size(64, 15);
            this.lbl1HourData.TabIndex = 2;
            this.lbl1HourData.Text = "1 Hour data";
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpDateTo.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTo.Location = new System.Drawing.Point(60, 216);
            this.dtpDateTo.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.dtpDateTo.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(127, 21);
            this.dtpDateTo.TabIndex = 54;
            this.dtpDateTo.Value = new System.DateTime(2015, 10, 22, 0, 0, 0, 0);
            this.dtpDateTo.ValueChanged += new System.EventHandler(this.dtpDateTo_ValueChanged);
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpDateFrom.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateFrom.Location = new System.Drawing.Point(60, 184);
            this.dtpDateFrom.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.dtpDateFrom.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(127, 21);
            this.dtpDateFrom.TabIndex = 54;
            this.dtpDateFrom.Value = new System.DateTime(2015, 10, 22, 0, 0, 0, 0);
            this.dtpDateFrom.ValueChanged += new System.EventHandler(this.dtpDateFrom_ValueChanged);
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.ForeColor = System.Drawing.Color.Black;
            this.lblTo.Location = new System.Drawing.Point(6, 217);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(30, 17);
            this.lblTo.TabIndex = 0;
            this.lblTo.Text = "To:";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.ForeColor = System.Drawing.Color.Black;
            this.lblFrom.Location = new System.Drawing.Point(6, 185);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(48, 17);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From:";
            // 
            // grbPreview
            // 
            this.grbPreview.Controls.Add(this.pnlDataGrid);
            this.grbPreview.Controls.Add(this.pnlInfo);
            this.grbPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbPreview.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbPreview.ForeColor = System.Drawing.Color.Gray;
            this.grbPreview.Location = new System.Drawing.Point(5, 0);
            this.grbPreview.Name = "grbPreview";
            this.grbPreview.Size = new System.Drawing.Size(478, 493);
            this.grbPreview.TabIndex = 55;
            this.grbPreview.TabStop = false;
            this.grbPreview.Text = "Preview";
            // 
            // pnlDataGrid
            // 
            this.pnlDataGrid.Controls.Add(this.chtData);
            this.pnlDataGrid.Controls.Add(this.dgvData);
            this.pnlDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDataGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlDataGrid.Location = new System.Drawing.Point(3, 136);
            this.pnlDataGrid.Name = "pnlDataGrid";
            this.pnlDataGrid.Size = new System.Drawing.Size(472, 354);
            this.pnlDataGrid.TabIndex = 12;
            // 
            // chtData
            // 
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.Name = "ChartArea1";
            this.chtData.ChartAreas.Add(chartArea1);
            this.chtData.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chtData.Legends.Add(legend1);
            this.chtData.Location = new System.Drawing.Point(0, 0);
            this.chtData.Name = "chtData";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "ChartSerial";
            this.chtData.Series.Add(series1);
            this.chtData.Size = new System.Drawing.Size(472, 354);
            this.chtData.TabIndex = 1;
            this.chtData.Text = "chart1";
            this.chtData.Visible = false;
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.Location = new System.Drawing.Point(0, 0);
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.Size = new System.Drawing.Size(472, 354);
            this.dgvData.TabIndex = 0;
            this.dgvData.Visible = false;
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.lblName);
            this.pnlInfo.Controls.Add(this.lblDateToVal);
            this.pnlInfo.Controls.Add(this.lblIntevalTimeName);
            this.pnlInfo.Controls.Add(this.lblDateFromVal);
            this.pnlInfo.Controls.Add(this.lblViewType);
            this.pnlInfo.Controls.Add(this.lblIntevalTimeVal);
            this.pnlInfo.Controls.Add(this.lblDateFromName);
            this.pnlInfo.Controls.Add(this.lblDateToName);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.Location = new System.Drawing.Point(3, 31);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(472, 105);
            this.pnlInfo.TabIndex = 11;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblName.Location = new System.Drawing.Point(3, 1);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(88, 17);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "MPS (mg/L)";
            // 
            // lblDateToVal
            // 
            this.lblDateToVal.AutoSize = true;
            this.lblDateToVal.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateToVal.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDateToVal.Location = new System.Drawing.Point(126, 73);
            this.lblDateToVal.Name = "lblDateToVal";
            this.lblDateToVal.Size = new System.Drawing.Size(144, 17);
            this.lblDateToVal.TabIndex = 10;
            this.lblDateToVal.Text = "00-00-0000 00:00:00";
            // 
            // lblIntevalTimeName
            // 
            this.lblIntevalTimeName.AutoSize = true;
            this.lblIntevalTimeName.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIntevalTimeName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblIntevalTimeName.Location = new System.Drawing.Point(3, 19);
            this.lblIntevalTimeName.Name = "lblIntevalTimeName";
            this.lblIntevalTimeName.Size = new System.Drawing.Size(97, 17);
            this.lblIntevalTimeName.TabIndex = 4;
            this.lblIntevalTimeName.Text = "Inteval Time:";
            // 
            // lblDateFromVal
            // 
            this.lblDateFromVal.AutoSize = true;
            this.lblDateFromVal.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateFromVal.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDateFromVal.Location = new System.Drawing.Point(126, 55);
            this.lblDateFromVal.Name = "lblDateFromVal";
            this.lblDateFromVal.Size = new System.Drawing.Size(144, 17);
            this.lblDateFromVal.TabIndex = 9;
            this.lblDateFromVal.Text = "00-00-0000 00:00:00";
            // 
            // lblViewType
            // 
            this.lblViewType.AutoSize = true;
            this.lblViewType.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblViewType.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblViewType.Location = new System.Drawing.Point(3, 37);
            this.lblViewType.Name = "lblViewType";
            this.lblViewType.Size = new System.Drawing.Size(66, 17);
            this.lblViewType.TabIndex = 5;
            this.lblViewType.Text = "List type";
            // 
            // lblIntevalTimeVal
            // 
            this.lblIntevalTimeVal.AutoSize = true;
            this.lblIntevalTimeVal.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIntevalTimeVal.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblIntevalTimeVal.Location = new System.Drawing.Point(126, 19);
            this.lblIntevalTimeVal.Name = "lblIntevalTimeVal";
            this.lblIntevalTimeVal.Size = new System.Drawing.Size(75, 17);
            this.lblIntevalTimeVal.TabIndex = 8;
            this.lblIntevalTimeVal.Text = "5 Minutes";
            // 
            // lblDateFromName
            // 
            this.lblDateFromName.AutoSize = true;
            this.lblDateFromName.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateFromName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDateFromName.Location = new System.Drawing.Point(3, 55);
            this.lblDateFromName.Name = "lblDateFromName";
            this.lblDateFromName.Size = new System.Drawing.Size(48, 17);
            this.lblDateFromName.TabIndex = 6;
            this.lblDateFromName.Text = "From:";
            // 
            // lblDateToName
            // 
            this.lblDateToName.AutoSize = true;
            this.lblDateToName.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateToName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDateToName.Location = new System.Drawing.Point(3, 73);
            this.lblDateToName.Name = "lblDateToName";
            this.lblDateToName.Size = new System.Drawing.Size(30, 17);
            this.lblDateToName.TabIndex = 7;
            this.lblDateToName.Text = "To:";
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.grbSelect);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(5, 5);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(193, 493);
            this.pnlLeft.TabIndex = 56;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.grbPreview);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(198, 5);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.pnlMain.Size = new System.Drawing.Size(483, 493);
            this.pnlMain.TabIndex = 57;
            // 
            // frmHistoryMPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 503);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlLeft);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MinimizeBox = false;
            this.Name = "frmHistoryMPS";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "History data";
            this.Load += new System.EventHandler(this.frmHistoryMPS_Load);
            this.grbSelect.ResumeLayout(false);
            this.grbSelect.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlViewType.ResumeLayout(false);
            this.pnlViewType.PerformLayout();
            this.pnlDataType.ResumeLayout(false);
            this.pnlDataType.PerformLayout();
            this.grbPreview.ResumeLayout(false);
            this.pnlDataGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chtData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbSelect;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.GroupBox grbPreview;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDateToVal;
        private System.Windows.Forms.Label lblDateFromVal;
        private System.Windows.Forms.Label lblIntevalTimeVal;
        private System.Windows.Forms.Label lblDateToName;
        private System.Windows.Forms.Label lblDateFromName;
        private System.Windows.Forms.Label lblViewType;
        private System.Windows.Forms.Label lblIntevalTimeName;
        private System.Windows.Forms.Panel pnlDataType;
        private FlatButton btn5Minute;
        private System.Windows.Forms.Label lbl5MinData;
        private System.Windows.Forms.Label lbl1HourData;
        private System.Windows.Forms.Panel pnlViewType;
        private FlatButton btnGraphType;
        private FlatButton btnListType;
        private System.Windows.Forms.Label lblListType;
        private System.Windows.Forms.Label lblGraphType;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Panel pnlDataGrid;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtData;
        private FlatButton btn60Minute;
    }
}