using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLogger.Data;
using System.Windows.Forms.DataVisualization.Charting;
using DataLogger.Utils;

using System.Resources;
using System.Reflection;
using System.Globalization;

namespace DataLogger
{
    public partial class frmHistoryAll : Form
    {
        LanguageService lang;

        private enum DataViewType
        {
            List,
            Graph
        }

        private enum DataViewParameterType
        {
            Analyzer,
            MPS,
            SamplerSystem,
            Station,
            Custom
        }

        private enum DataTimeType
        {
            _5Minute,
            _60Minute
        }

        private readonly data_5minute_value_repository db5m = new data_5minute_value_repository();
        private readonly data_60minute_value_repository db60m = new data_60minute_value_repository();

        private bool isCheckAuto = false;

        // Lưu kiểu hiển thị hiện tại: List/Graph
        private DataViewType currentViewType = DataViewType.List;

        // Lưu kiểu loại tham số hiện tại: Analyzer/MPS/SamplerSystem/Station
        private DataViewParameterType currentViewParameterType = DataViewParameterType.MPS;

        // Lưu inteval hiện tại: 5minute/60minute
        private DataTimeType currentTimeType = DataTimeType._5Minute;

        public frmHistoryAll(LanguageService _lang)
        {
            InitializeComponent();

            lang = _lang;
            switch_language();
        }
        private void switch_language()
        {
            this.Text = lang.getText("form_history_title");
            this.lblGroupSelect.Text = lang.getText("form_history_select_group_box_title");
            this.lblGroupPreview.Text = lang.getText("form_history_preview_group_box_title");
            // 5min, 1hour            
            lang.setText(lbl5MinData, "form_history_select_group_5_min_data", btn5Minute, EAlign.Center);            
            lang.setText(lbl1HourData, "form_history_select_group_1_hour_data", btn60Minute, EAlign.Center);
            // list type, graph type            
            lang.setText(lblListType, "form_history_select_group_list_type", btnListType, EAlign.Center);
            lang.setText(lblGraphType, "form_history_select_group_graph_type", btnGraphType, EAlign.Center);

            // equipments
            lang.setText(lblSelectMPS, "form_history_select_mps", btnMPS, EAlign.Center);
            lang.setText(lblSelectCustom, "form_history_select_custom", btnCustom, EAlign.Center);
            
            lang.setText(grbSelectParameter, "form_history_select_custom");
            this.lblFrom.Text = lang.getText("form_history_select_group_from");
            this.lblTo.Text = lang.getText("form_history_select_group_to");
            this.btnOK.Text = lang.getText("form_history_select_group_button_ok");
            this.btnCancel.Text = lang.getText("form_history_select_group_button_cancel");
            this.chkSelectAll.Text = lang.getText("form_history_select_group_select_all");

            // preview
            lang.setText(lblSamplerSystem, "form_history_select_sampler_system");
            lang.setText(lblAnalyzer, "form_history_select_analyzer", lblSamplerSystem, EAlign.Left);
            lang.setText(lblMPS, "form_history_select_mps", lblSamplerSystem, EAlign.Left);            
            lang.setText(lblStation, "form_history_select_station", lblSamplerSystem, EAlign.Left);
            lang.setText(lblIntevalTimeName, "form_history_preview_interval_time", lblSamplerSystem, EAlign.Left);
            lang.setText(lblViewTypeLabel, "form_history_preview_view_type", lblSamplerSystem, EAlign.Left);
            lang.setText(lblDateFromName, "form_history_select_group_from", lblSamplerSystem, EAlign.Left);
            lang.setText(lblDateToName, "form_history_select_group_to", lblSamplerSystem, EAlign.Left);

            //lblIntevalTimeVal.Text = btn5Minute.IsActive ? "5 Minutes" : "60 Minutes";
            if (btn5Minute.IsActive)
            {
                lang.setText(lblIntevalTimeVal, "form_history_select_group_5_min_data");

            }
            else
            {
                lang.setText(lblIntevalTimeVal, "form_history_select_group_1_hour_data");
            }
            //lblViewTypeVal.Text = btnGraphType.IsActive ? "Graph type" : "List type";
            if (btnGraphType.IsActive)
            {
                lang.setText(lblViewTypeVal, "form_history_select_group_graph_type");
            }
            else
            {
                lang.setText(lblViewTypeVal, "form_history_select_group_list_type");
            }

        }

        private void frmHistoryAll_Load(object sender, EventArgs e)
        {
            dtpDateTo.Value = DateTime.Now;
            dtpDateFrom.Value = DateTime.Now.AddDays(-1);

            GlobalVar.moduleSettings = new module_repository().get_all();
            foreach (var item in GlobalVar.moduleSettings)
            {
                ParamInfo newParam = new ParamInfo();
                Random rand = new Random();
                newParam.HasStatus = true;
                newParam.NameDB = item.value_column;
                newParam.NameDisplay = item.display_name;
                newParam.StatusNameDB = item.status_column;
                newParam.StatusNameDisplay = item.display_name + " status";
                newParam.StatusNameVisible = item.display_name + "_status_Val";
                newParam.Selected = false;
                newParam.GraphColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                DataLoggerParam.PARAMETER_LIST.Add(newParam);
            }

            checkedListBoxParameters.Items.Clear();
            checkedListBoxParameters.Items.AddRange(DataLoggerParam.PARAMETER_LIST.Select(p => p.NameDisplay).ToArray());
        }

        private void btnListType_Click(object sender, EventArgs e)
        {
            currentViewType = DataViewType.List;
        }

        private void btnGraphType_Click(object sender, EventArgs e)
        {
            currentViewType = DataViewType.Graph;
        }

        private void btn5Minute_Click(object sender, EventArgs e)
        {
            currentTimeType = DataTimeType._5Minute;
        }
        private void btn60Minute_Click(object sender, EventArgs e)
        {
            currentTimeType = DataTimeType._60Minute;
        }

        /// <summary>
        /// Hàm hiển thị, tùy vào viewType là List/Graph và timeType là 5Min/60Min để hiển thị cho phù hợp.
        private void reloadViewData(DataViewType _viewType, DataTimeType _timeType, DataViewParameterType _paraType)
        {
            switch (_paraType)
            {
                case DataViewParameterType.MPS:
                    reloadViewDataMPS(_viewType, _timeType);
                    break;
                case DataViewParameterType.Custom:
                    reloadViewDataCustom(_viewType, _timeType);
                    break;
                default:
                    break;
            }
        }
        private void dtpDateFrom_ValueChanged(object sender, EventArgs e)
        {
            // không cho phép DateFrom lớn hơn DateTo
            if (dtpDateFrom.Value > dtpDateTo.Value) dtpDateFrom.Value = dtpDateTo.Value;
        }
        private void dtpDateTo_ValueChanged(object sender, EventArgs e)
        {
            // không cho phép DateFrom lớn hơn DateTo
            if (dtpDateFrom.Value > dtpDateTo.Value) dtpDateTo.Value = dtpDateFrom.Value;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            //lblIntevalTimeVal.Text = btn5Minute.IsActive ? "5 Minutes" : "60 Minutes";
            if (btn5Minute.IsActive)
            {
                lang.setText(lblIntevalTimeVal, "form_history_select_group_5_min_data");

            }
            else
            {
                lang.setText(lblIntevalTimeVal, "form_history_select_group_1_hour_data");
            }
            //lblViewTypeVal.Text = btnGraphType.IsActive ? "Graph type" : "List type";
            if (btnGraphType.IsActive)
            {
                lang.setText(lblViewTypeVal, "form_history_select_group_graph_type");
            }
            else
            {
                lang.setText(lblViewTypeVal, "form_history_select_group_list_type");
            }
            lblDateFromVal.Text = dtpDateFrom.Value.ToString("dd/MM/yyyy hh:mm:ss");
            lblDateToVal.Text = dtpDateTo.Value.ToString("dd/MM/yyyy hh:mm:ss");

            reloadViewData(currentViewType, currentTimeType, currentViewParameterType);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnAnalyzer_Click(object sender, EventArgs e)
        {
            grbSelectParameter.Visible = false;
            
            currentViewParameterType = DataViewParameterType.Analyzer;

        }
        private void btnMPS_Click(object sender, EventArgs e)
        {
            grbSelectParameter.Visible = false;
            
            currentViewParameterType = DataViewParameterType.MPS;
        }

        private void btnSamplerSystem_Click(object sender, EventArgs e)
        {
            grbSelectParameter.Visible = false;

            currentViewParameterType = DataViewParameterType.SamplerSystem;
        }
        private void btnStation_Click(object sender, EventArgs e)
        {
            grbSelectParameter.Visible = false;
            currentViewParameterType = DataViewParameterType.Station;
        }
        private void btnCustom_Click(object sender, EventArgs e)
        {
            grbSelectParameter.Visible = true;

            currentViewParameterType = DataViewParameterType.Custom;
            lang.setText(grbSelectParameter, "form_history_select_custom");
        }

        private void reloadViewDataMPS(DataViewType _viewType, DataTimeType _timeType)
        {
            GlobalVar.moduleSettings = new module_repository().get_all();
            if (_viewType == DataViewType.List)
            {
                chtData.Visible = false;
                dgvData.Columns.Clear();
                dgvData.Visible = true;

                DataTable dt_source = null;
                if (_timeType == DataTimeType._5Minute)
                {
                    dt_source = db5m.get_all_mps(dtpDateFrom.Value, dtpDateTo.Value);
                }
                else //if (_timeType == DataTimeType._60Minute)
                {
                    dt_source = db60m.get_all_mps(dtpDateFrom.Value, dtpDateTo.Value);
                }

                // Do dt_source chứa date,hour,minute riêng nhau nên phải tạo một dt_view mới gộp các thành phần lại hiển thị cho đẹp hơn
                DataTable dt_view = new DataTable();
                dt_view.Columns.Add("Date");
                dt_view.Columns.Add("Time");

                foreach (var item in GlobalVar.moduleSettings)
                {
                    dt_view.Columns.Add(item.display_name);
                }

                dt_view.Columns.Add("Status_Val");

                // dữ liệu dt_source sang dt_view để hiển thị
                DataRow viewrow = null;
                if (dt_source == null)
                {
                    return;
                }
                foreach (DataRow row in dt_source.Rows)
                {
                    viewrow = dt_view.NewRow();
                    //viewrow["Date"] = row["stored_date"].ToString().Substring(0, 10);
                    //viewrow["Date"] = (Convert.ToDateTime(row["stored_date"].ToString().Substring(0, 10))).ToString("dd/MM/yyyy");
                    //viewrow["Date"] = (Convert.ToDateTime(row["stored_date"].ToString().Substring(0, 10))).ToString("dd/MM/yyyy");
                    string created = (Convert.ToDateTime(row["created"].ToString())).ToString("yyyyMMddHHmmss");
                    string time = created.Substring(8, 2) + ":" + created.Substring(10, 2) + ":" + created.Substring(12, 2);
                    viewrow["Date"] = (Convert.ToDateTime(row["created"].ToString())).ToString("dd/MM/yyyy");
                    viewrow["Time"] = time;
                    //viewrow["Date"] = (Convert.ToDateTime((row["stored_date"].ToString().Split(' '))[0])).ToString("dd/MM/yyyy");
                    //viewrow["Time"] = ((int)row["stored_hour"]).ToString("00") + ":" + ((int)row["stored_minute"]).ToString("00") + ":00";

                    foreach (var item in GlobalVar.moduleSettings)
                    {
                        viewrow[item.display_name] = String.Format("{0:0.00}", row[item.value_column]);
                    }

                    viewrow["Status_Val"] = row["mps_status"];

                    dt_view.Rows.Add(viewrow);
                }
                dgvData.DataSource = dt_view;

                // thêm cột Status có màu phù hợp với status
                DataGridViewImageColumn imgColumnStatus = new DataGridViewImageColumn();
                imgColumnStatus.Name = "Status";
                dgvData.Columns.Add(imgColumnStatus);
                dgvData.Columns["Status_Val"].Visible = false; // ẩn cột status bằng số, chỉ để lại cột status có màu

                int status_val = 0;
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Cells["Status_Val"].Value != null)
                    {
                        Int32.TryParse(row.Cells["Status_Val"].Value.ToString(), out status_val);

                        if (status_val == 0)
                        {
                            row.Cells["Status"].Value = (System.Drawing.Image)Properties.Resources.Normal_status_x16;
                        }
                        else if (status_val == 4)
                        {
                            row.Cells["Status"].Value = (System.Drawing.Image)Properties.Resources.bottle_position_18x18;
                        }
                        else
                        {
                            row.Cells["Status"].Value = (System.Drawing.Image)Properties.Resources.Fault_status_x16;
                        }
                    }
                    //if (row.Cells["MPS_pH"].Value != null)
                    //{
                    //    if (Convert.ToDouble(row.Cells["MPS_pH"].Value) < 0)
                    //    {
                    //        row.Cells["MPS_pH"].Value = "---";
                    //    }
                    //}
                    //if (row.Cells["MPS_EC"].Value != null)
                    //{
                    //    if (Convert.ToDouble(row.Cells["MPS_EC"].Value) < 0)
                    //    {
                    //        row.Cells["MPS_EC"].Value = "---";
                    //    }
                    //}
                    //if (row.Cells["MPS_DO"].Value != null)
                    //{
                    //    if (Convert.ToDouble(row.Cells["MPS_DO"].Value) < 0)
                    //    {
                    //        row.Cells["MPS_DO"].Value = "---";
                    //    }
                    //}
                    //if (row.Cells["MPS_TSS"].Value != null)
                    //{
                    //    if (Convert.ToDouble(row.Cells["MPS_TSS"].Value) < 0)
                    //    {
                    //        row.Cells["MPS_TSS"].Value = "---";
                    //    }
                    //}
                    //if (row.Cells["MPS_ORP"].Value != null)
                    //{
                    //    if (Convert.ToDouble(row.Cells["MPS_ORP"].Value) < 0)
                    //    {
                    //        row.Cells["MPS_ORP"].Value = "---";
                    //    }
                    //}
                    //if (row.Cells["MPS_Temp"].Value != null)
                    //{
                    //    if (Convert.ToDouble(row.Cells["MPS_Temp"].Value) < 0)
                    //    {
                    //        row.Cells["MPS_Temp"].Value = "---";
                    //    }
                    //}
                }

            }
            else //if(_viewType == DataViewType.Graph)
            {
                dgvData.Visible = false;
                chtData.Series.Clear();
                chtData.Visible = true;

                DataTable dt_source = null;
                if (_timeType == DataTimeType._5Minute)
                {
                    dt_source = db5m.get_all_mps(dtpDateFrom.Value, dtpDateTo.Value);
                }
                else //if (_timeType == DataTimeType._60Minute)
                {
                    dt_source = db60m.get_all_mps(dtpDateFrom.Value, dtpDateTo.Value);
                }

                DataTable dt_view = new DataTable();
                dt_view.Columns.Add("CreatedDate");

                foreach (var item in GlobalVar.moduleSettings)
                {
                    dt_view.Columns.Add(item.display_name);
                }

                // chuyển dữ liệu dt_source sang dt_view để hiển thị
                DataRow viewrow = null;
                if (dt_source == null)
                {
                    return;
                }
                foreach (DataRow row in dt_source.Rows)
                {
                    // kiểm tra status, chỉ lấy chững status normal
                    int _status = (int)row["mps_status"];
                    //if (_status == 0)
                    //{
                    viewrow = dt_view.NewRow();
                    //DateTime _date = (DateTime)row["stored_date"];
                    //int _hour = (int)row["stored_hour"];
                    //int _minute = (int)row["stored_minute"];
                    //DateTime _rdate = new DateTime(_date.Year, _date.Month, _date.Day, _hour, _minute, 0);
                 
                    string created = (Convert.ToDateTime(row["created"].ToString())).ToString("yyyyMMddHHmmss");
                    DateTime _rdate = new DateTime(Int32.Parse(created.Substring(0, 4)), Int32.Parse(created.Substring(4, 2)), Int32.Parse(created.Substring(6, 2)), Int32.Parse(created.Substring(8, 2)), Int32.Parse(created.Substring(10, 2)), Int32.Parse(created.Substring(12, 2)));
                        
                    viewrow["CreatedDate"] = _rdate;
                    foreach (var item in GlobalVar.moduleSettings)
                    {
                        viewrow[item.display_name] = String.Format("{0:0.00}", row[item.value_column]);
                    }

                    dt_view.Rows.Add(viewrow);
                    //}
                }

                foreach (var item in GlobalVar.moduleSettings)
                {
                    Random rand = new Random();
                    chtData.Series.Add(item.display_name);
                    chtData.Series["MPS_pH"].XValueMember = "CreatedDate";
                    chtData.Series["MPS_pH"].YValueMembers = "item.display_name";
                    chtData.Series["MPS_pH"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                    chtData.Series["MPS_pH"].Color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                    chtData.Series["MPS_pH"].BorderWidth = 3;
                }

                chtData.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                chtData.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
                chtData.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
                chtData.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.WhiteSmoke;
                chtData.ChartAreas[0].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
                chtData.ChartAreas[0].AxisX.Title = "Date";
                chtData.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.Lines;

                chtData.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                chtData.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
                chtData.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
                chtData.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.WhiteSmoke;
                chtData.ChartAreas[0].AxisY.Title = "MPS (mg/L)";
                chtData.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.Lines;

                chtData.DataSource = dt_view;
                chtData.DataBind();

            }
        }
        /// <summary>
        /// Hàm hiển thị, tùy vào viewType là List/Graph và timeType là 5Min/60Min để hiển thị cho phù hợp.
        /// </summary>
        /// <param name="_viewType"></param>
        /// <param name="_timeType"></param>
        private void reloadViewDataCustom(DataViewType _viewType, DataTimeType _timeType)
        {
            DataLoggerParam.PARAMETER_LIST.ForEach(p => p.Selected = false);

            foreach (Object item in checkedListBoxParameters.CheckedItems)
            {
                string _key = item.ToString();
                ParamInfo _pinfo = DataLoggerParam.PARAMETER_LIST.FirstOrDefault(p => p.NameDisplay == _key);
                if (_pinfo != null)
                {
                    _pinfo.Selected = true;
                }
            }

            IEnumerable<string> _statusNameList = DataLoggerParam.PARAMETER_LIST.Where(p => p.Selected && p.HasStatus).Select(p => p.StatusNameDB).ToList();
            IEnumerable<string> _paramNameList = DataLoggerParam.PARAMETER_LIST.Where(p => p.Selected).Select(p => p.NameDB).ToList();
            List<string> _paramListForQuery = _paramNameList.Concat(_statusNameList).ToList();


            if (_viewType == DataViewType.List)
            {
                chtData.Visible = false;
                dgvData.Columns.Clear();
                dgvData.Visible = true;

                DataTable dt_source = null;
                if (_timeType == DataTimeType._5Minute)
                {
                    dt_source = db5m.get_all_custom(dtpDateFrom.Value, dtpDateTo.Value, _paramListForQuery);
                }
                else //if (_timeType == DataTimeType._60Minute)
                {
                    dt_source = db60m.get_all_custom(dtpDateFrom.Value, dtpDateTo.Value, _paramListForQuery);
                }

                // Do dt_source chứa date,hour,minute riêng nhau nên phải tạo một dt_view mới gộp các thành phần lại hiển thị cho đẹp hơn
                DataTable dt_view = new DataTable();
                dt_view.Columns.Add("Date");
                dt_view.Columns.Add("Time");

                foreach (ParamInfo item in DataLoggerParam.PARAMETER_LIST.Where(p => p.Selected))
                {
                    dt_view.Columns.Add(item.NameDisplay);
                    if (item.HasStatus)
                        dt_view.Columns.Add(item.StatusNameVisible);
                }

                // dữ liệu dt_source sang dt_view để hiển thị
                DataRow viewrow = null;
                if (dt_source == null)
                {
                    return;
                }
                foreach (DataRow row in dt_source.Rows)
                {
                    viewrow = dt_view.NewRow();
                    //viewrow["Date"] = row["stored_date"].ToString().Substring(0, 10);
                    //viewrow["Date"] = (Convert.ToDateTime(row["stored_date"].ToString())).ToString("dd/MM/yyyy");
                    //viewrow["Time"] = ((int)row["stored_hour"]).ToString("00") + ":" + ((int)row["stored_minute"]).ToString("00") + ":00";
                    string created = (Convert.ToDateTime(row["created"].ToString())).ToString("yyyyMMddHHmmss");
                    string time = created.Substring(8, 2) + ":" + created.Substring(10, 2) + ":" + created.Substring(12, 2);
                    viewrow["Date"] = (Convert.ToDateTime(row["created"].ToString())).ToString("dd/MM/yyyy");
                    viewrow["Time"] = time;

                    foreach (ParamInfo item in DataLoggerParam.PARAMETER_LIST.Where(p => p.Selected))
                    {
                        viewrow[item.NameDisplay] = String.Format("{0:0.00}", row[item.NameDB]);

                        if (item.HasStatus)
                            viewrow[item.StatusNameVisible] = row[item.StatusNameDB];

                    }

                    dt_view.Rows.Add(viewrow);
                }
                dgvData.DataSource = dt_view;

                // thêm cột Status có màu phù hợp với status
                foreach (ParamInfo item in DataLoggerParam.PARAMETER_LIST.Where(p => p.Selected))
                {
                    if (item.HasStatus)
                    {
                        int cindex = dgvData.Columns[item.StatusNameVisible].Index;

                        DataGridViewImageColumn imgColumnStatus = new DataGridViewImageColumn();
                        imgColumnStatus.Name = item.StatusNameDisplay;
                        dgvData.Columns.Insert(cindex, imgColumnStatus);
                        dgvData.Columns[item.StatusNameVisible].Visible = false; // ẩn cột status bằng số, chỉ để lại cột status có màu
                    }
                }

                // chuẩn hóa dữ liệu hiển thị: chọn màu status, lọc giá trị âm
                int status_val = 0;
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    foreach (ParamInfo item in DataLoggerParam.PARAMETER_LIST.Where(p => p.Selected))
                    {
                        // chọn màu status
                        if (item.HasStatus)
                        {
                            if (row.Cells[item.StatusNameVisible].Value != null)
                            {
                                Int32.TryParse(row.Cells[item.StatusNameVisible].Value.ToString(), out status_val);

                                if (status_val == 0)
                                {
                                    row.Cells[item.StatusNameDisplay].Value = (System.Drawing.Image)Properties.Resources.Normal_status_x16;
                                }
                                else if (status_val == 4)
                                {
                                    row.Cells[item.StatusNameDisplay].Value = (System.Drawing.Image)Properties.Resources.bottle_position_18x18;
                                }
                                else
                                {
                                    row.Cells[item.StatusNameDisplay].Value = (System.Drawing.Image)Properties.Resources.Fault_status_x16;
                                }
                            }
                        }

                        // lọc giá trị âm
                        if (Convert.ToDouble(row.Cells[item.NameDisplay].Value) < 0)
                        {
                            row.Cells[item.NameDisplay].Value = "---";
                        }
                        else
                        {
                            double tempValue = Convert.ToDouble(row.Cells[item.NameDisplay].Value);
                            string displayValueTemp = Convert.ToString(row.Cells[item.NameDisplay].Value);
                            switch (item.NameDB)
                            {
                                case "module_power":
                                    if (tempValue == 1)
                                    {
                                        displayValueTemp = "ON";
                                    }
                                    else
                                    {
                                        displayValueTemp = "OFF";
                                    }
                                    break;
                                case "module_ups":
                                    if (tempValue == 1)
                                    {
                                        displayValueTemp = "ON";
                                    }
                                    else
                                    {
                                        displayValueTemp = "OFF";
                                    }
                                    break;
                                case "module_door":
                                    if (tempValue == 1)
                                    {
                                        displayValueTemp = "CLOSE";
                                    }
                                    else
                                    {
                                        displayValueTemp = "OPEN";
                                    }
                                    break;
                                case "module_fire":
                                    if (tempValue == 1)
                                    {
                                        displayValueTemp = "NORMAL";
                                    }
                                    else
                                    {
                                        displayValueTemp = "FIRE";
                                    }
                                    break;
                                case "module_flow":
                                    if (tempValue == 1)
                                    {
                                        displayValueTemp = "OPEN";
                                    }
                                    else
                                    {
                                        displayValueTemp = "CLOSE";
                                    }
                                    break;
                                case "module_pumplam":
                                    if (tempValue == 1)
                                    {
                                        displayValueTemp = "AUTO";
                                    }
                                    else
                                    {
                                        displayValueTemp = "MANUAL";
                                    }
                                    break;
                                case "module_pumplrs":
                                    if (tempValue == 1)
                                    {
                                        displayValueTemp = "STOP";
                                    }
                                    else
                                    {
                                        displayValueTemp = "RUN";
                                    }
                                    break;
                                case "module_pumplflt":
                                    if (tempValue == 1)
                                    {
                                        displayValueTemp = "FAULT";
                                    }
                                    else
                                    {
                                        displayValueTemp = "NORMAL";
                                    }
                                    break;
                                case "module_pumpram":
                                    if (tempValue == 1)
                                    {
                                        displayValueTemp = "AUTO";
                                    }
                                    else
                                    {
                                        displayValueTemp = "MANUAL";
                                    }
                                    break;
                                case "module_pumprrs":
                                    if (tempValue == 1)
                                    {
                                        displayValueTemp = "STOP";
                                    }
                                    else
                                    {
                                        displayValueTemp = "RUN";
                                    }
                                    break;
                                case "module_pumprflt":
                                    if (tempValue == 1)
                                    {
                                        displayValueTemp = "FAULT";
                                    }
                                    else
                                    {
                                        displayValueTemp = "NORMAL";
                                    }
                                    break;
                                case "module_air1":
                                    if (tempValue == 1)
                                    {
                                        displayValueTemp = "ON";
                                    }
                                    else
                                    {
                                        displayValueTemp = "OFF";
                                    }
                                    break;
                                case "module_air2":
                                    if (tempValue == 1)
                                    {
                                        displayValueTemp = "ON";
                                    }
                                    else
                                    {
                                        displayValueTemp = "OFF";
                                    }
                                    break;
                                case "module_cleaning":
                                    if (tempValue == 1)
                                    {
                                        displayValueTemp = "ON";
                                    }
                                    else
                                    {
                                        displayValueTemp = "OFF";
                                    }
                                    break;
                                default:
                                    break;
                            }
                            row.Cells[item.NameDisplay].Value = displayValueTemp;
                        }
                    }
                }

            }
            else //if(_viewType == DataViewType.Graph)
            {
                dgvData.Visible = false;
                chtData.Series.Clear();
                chtData.Visible = true;

                DataTable dt_source = null;
                if (_timeType == DataTimeType._5Minute)
                {
                    dt_source = db5m.get_all_custom(dtpDateFrom.Value, dtpDateTo.Value, _paramListForQuery);
                }
                else //if (_timeType == DataTimeType._60Minute)
                {
                    dt_source = db60m.get_all_custom(dtpDateFrom.Value, dtpDateTo.Value, _paramListForQuery);
                }

                DataTable dt_view = new DataTable();
                dt_view.Columns.Add("CreatedDate");
                foreach (ParamInfo item in DataLoggerParam.PARAMETER_LIST.Where(p => p.Selected))
                {
                    dt_view.Columns.Add(item.NameDisplay);
                }

                // chuyển dữ liệu dt_source sang dt_view để hiển thị
                DataRow viewrow = null;
                if (dt_source == null)
                {
                    return;
                }
                foreach (DataRow row in dt_source.Rows)
                {
                    bool allowAdd = true;
                    viewrow = dt_view.NewRow();
                    //DateTime _date = (DateTime)row["stored_date"];
                    //int _hour = (int)row["stored_hour"];
                    //int _minute = (int)row["stored_minute"];
                    //DateTime _rdate = new DateTime(_date.Year, _date.Month, _date.Day, _hour, _minute, 0);

                    string created = (Convert.ToDateTime(row["created"].ToString())).ToString("yyyyMMddHHmmss");
                    DateTime _rdate = new DateTime(Int32.Parse(created.Substring(0, 4)), Int32.Parse(created.Substring(4, 2)), Int32.Parse(created.Substring(6, 2)), Int32.Parse(created.Substring(8, 2)), Int32.Parse(created.Substring(10, 2)), Int32.Parse(created.Substring(12, 2)));

                    viewrow["CreatedDate"] = _rdate;
                    foreach (ParamInfo item in DataLoggerParam.PARAMETER_LIST.Where(p => p.Selected))
                    {
                        if (item.HasStatus)
                        {
                            // kiểm tra status, chỉ lấy chững status normal
                            int _status = (int)row[item.StatusNameDB];
                            //if (_status == 0)
                            //{

                            //DateTime _date = (DateTime)row["stored_date"];
                            //int _hour = (int)row["stored_hour"];
                            //int _minute = (int)row["stored_minute"];
                            //DateTime _rdate = new DateTime(_date.Year, _date.Month, _date.Day, _hour, _minute, 0);

                            //viewrow["StoredDate"] = _rdate;
                            viewrow[item.NameDisplay] = String.Format("{0:0.00}", row[item.NameDB]);
                            //}
                            //else
                            //{
                            //    allowAdd = false;
                            //    break;
                            //}

                        }
                    }
                    if (allowAdd)
                    {
                        dt_view.Rows.Add(viewrow);
                    }
                }

                // tạo biểu đồ mới
                foreach (ParamInfo item in DataLoggerParam.PARAMETER_LIST.Where(p => p.Selected))
                {
                    chtData.Series.Add(item.NameDisplay);
                    chtData.Series[item.NameDisplay].XValueMember = "CreatedDate";
                    chtData.Series[item.NameDisplay].YValueMembers = item.NameDisplay;
                    chtData.Series[item.NameDisplay].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                    chtData.Series[item.NameDisplay].Color = item.GraphColor;// Color.Blue;
                    chtData.Series[item.NameDisplay].BorderWidth = 3;
                }

                chtData.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                chtData.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
                chtData.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
                chtData.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.WhiteSmoke;
                chtData.ChartAreas[0].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
                chtData.ChartAreas[0].AxisX.Title = "Date";
                chtData.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.Lines;

                chtData.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                chtData.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
                chtData.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
                chtData.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.WhiteSmoke;
                chtData.ChartAreas[0].AxisY.Title = "Data";
                chtData.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.Lines;

                chtData.DataSource = dt_view;
                chtData.DataBind();

            }
        }



        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (isCheckAuto) return;

            isCheckAuto = true;
            bool state = chkSelectAll.Checked;
            for (int i = 0; i < checkedListBoxParameters.Items.Count; i++)
                checkedListBoxParameters.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
            isCheckAuto = false;
        }

        private void checkedListBoxParameters_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isCheckAuto) return;

            isCheckAuto = true;
            // Get the current number checked.
            int num_checked = checkedListBoxParameters.CheckedItems.Count;

            // See if the item is being checked or unchecked.
            if ((e.CurrentValue != CheckState.Checked) && (e.NewValue == CheckState.Checked)) num_checked++;
            if ((e.CurrentValue == CheckState.Checked) && (e.NewValue != CheckState.Checked)) num_checked--;

            if (num_checked == 0)
            {
                chkSelectAll.CheckState = CheckState.Unchecked;
            }
            else if (num_checked == checkedListBoxParameters.Items.Count)
            {
                chkSelectAll.CheckState = CheckState.Checked;
            }
            else
            {
                chkSelectAll.CheckState = CheckState.Indeterminate;
            }
            isCheckAuto = false;
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
