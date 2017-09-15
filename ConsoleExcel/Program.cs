using DataLogger.Data;
using DataLogger.Entities;
using DataLogger.Utils;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleExcel
{
    class Program
    {
        HSSFWorkbook hssfwb;
        private readonly data_60minute_value_repository db60m = new data_60minute_value_repository();
        private readonly maintenance_log_repository _maintenance_logs = new maintenance_log_repository();
        private void backgroundWorkerMonthlyReport_DoWork()
        {
            //string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var appPath = System.IO.Path.GetDirectoryName(path);
            string dataFolderName = "data";

            string tempFileName = "monthly_report_template.xlsx";
            string newFileName = "MonthlyReport_" + DateTime.Now.ToString("yyyy (MMddHHmmssfff)");

            string tempFilePath = Path.Combine(appPath, dataFolderName, tempFileName); //thu muc chua temple xls
            string newFilePath = Path.Combine(appPath, dataFolderName, newFileName);   // chua file xls moi

            if (File.Exists(tempFilePath))
            {
                int year = DateTime.Now.Year;
                double dayOfYearTotal = (new DateTime(year, 12, 31)).DayOfYear;
                double dayOfYear = 0;
                int percent = 0;

                IEnumerable<data_value> allData = db60m.get_all_for_monthly_report(year); //lay het dulieu 60min cua nam hien tai
                if (allData != null)
                {
                    //Microsoft.Office.Interop.Excel.Application oExcelApp = new Microsoft.Office.Interop.Excel.Application();
                    //Microsoft.Office.Interop.Excel.Workbook oExcelWorkbook = oExcelApp.Workbooks.Open(tempFilePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                        const int startRow = 5;
                        int row;

                        List<MonthlyReportInfo> mps_ph = new List<MonthlyReportInfo>();
                        List<MonthlyReportInfo> mps_orp = new List<MonthlyReportInfo>();
                        List<MonthlyReportInfo> mps_do = new List<MonthlyReportInfo>();
                        List<MonthlyReportInfo> mps_turbidity = new List<MonthlyReportInfo>();
                        List<MonthlyReportInfo> mps_ec = new List<MonthlyReportInfo>();
                        List<MonthlyReportInfo> mps_temp = new List<MonthlyReportInfo>();
                        List<MonthlyReportInfo> tn = new List<MonthlyReportInfo>();
                        List<MonthlyReportInfo> tp = new List<MonthlyReportInfo>();
                        List<MonthlyReportInfo> toc = new List<MonthlyReportInfo>();
                        List<MonthlyReportInfo> refrigeration_temperature = new List<MonthlyReportInfo>();
                        List<MonthlyReportInfo> bottle_position = new List<MonthlyReportInfo>();

                    //    for (int month = 1; month <= 12; month++)
                    //    {
                    //        Excel.Worksheet oExcelWorksheet = oExcelWorkbook.Worksheets[month] as Excel.Worksheet;

                    //        //rename the Sheet name
                    //        oExcelWorksheet.Name = (new DateTime(year, month, 1)).ToString("MMM-yy");
                    //        oExcelWorksheet.Cells[2, 1] = "'" + (new DateTime(year, month, 1)).ToString("MM.");
                    //        oExcelWorksheet.Cells[2, 17] = (new DateTime(year, month, 1)).ToString("MMM-yy");

                    //        // calculate average value
                    //        for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
                    //        {
                    //            // get maintenance by date (year, month, day)
                    //            string strDate = year + "-" + month + "-" + day;
                    //            IEnumerable<maintenance_log> onDateMaintenanceLogs = _maintenance_logs.get_all_by_date(strDate);
                    //            // prepare data for maintenance
                    //            string maintenance_operator_name = "";
                    //            string maintenance_start_time = "";
                    //            string maintenance_end_time = "";
                    //            string maintenance_equipments = "";

                    //            Color maintenance_color = StatusColorInfo.COL_STATUS_MAINTENANCE_PERIODIC;
                    //            if (onDateMaintenanceLogs != null && onDateMaintenanceLogs.Count() > 0)
                    //            {
                    //                foreach (maintenance_log itemMaintenanceLog in onDateMaintenanceLogs)
                    //                {
                    //                    maintenance_operator_name += itemMaintenanceLog.name + ";";
                    //                    maintenance_start_time += itemMaintenanceLog.start_time.ToString("HH")
                    //                                                + "h" + itemMaintenanceLog.start_time.ToString("mm") + ";";
                    //                    maintenance_end_time += itemMaintenanceLog.end_time.ToString("HH")
                    //                                                + "h" + itemMaintenanceLog.end_time.ToString("mm") + ";";
                    //                    if (itemMaintenanceLog.tn == 1)
                    //                    {
                    //                        maintenance_equipments += "TN;";
                    //                    }
                    //                    if (itemMaintenanceLog.tp == 1)
                    //                    {
                    //                        maintenance_equipments += "TP;";
                    //                    }
                    //                    if (itemMaintenanceLog.toc == 1)
                    //                    {
                    //                        maintenance_equipments += "TOC;";
                    //                    }
                    //                    if (itemMaintenanceLog.mps == 1)
                    //                    {
                    //                        maintenance_equipments += "MPS;";
                    //                    }
                    //                    if (itemMaintenanceLog.pumping_system == 1)
                    //                    {
                    //                        maintenance_equipments += "Pumping;";
                    //                    }
                    //                    if (itemMaintenanceLog.auto_sampler == 1)
                    //                    {
                    //                        maintenance_equipments += "AutoSampler;";
                    //                    }
                    //                    if (itemMaintenanceLog.other == 1)
                    //                    {
                    //                        maintenance_equipments += itemMaintenanceLog.other_para + ";";
                    //                    }
                    //                    if (itemMaintenanceLog.maintenance_reason == 1)
                    //                    {
                    //                        maintenance_color = StatusColorInfo.COL_STATUS_MAINTENANCE_INCIDENT;
                    //                    }
                    //                }
                    //                maintenance_operator_name = maintenance_operator_name.Substring(0, maintenance_operator_name.Length - 1);
                    //                maintenance_start_time = maintenance_start_time.Substring(0, maintenance_start_time.Length - 1);
                    //                maintenance_end_time = maintenance_end_time.Substring(0, maintenance_end_time.Length - 1);
                    //                maintenance_equipments = maintenance_equipments.Substring(0, maintenance_equipments.Length - 1);
                    //            }

                    //            IEnumerable<data_value> dayData = allData.Where(t => t.stored_date.Month == month && t.stored_date.Day == day);
                    //            mps_ph.Clear();
                    //            mps_orp.Clear();
                    //            mps_do.Clear();
                    //            mps_turbidity.Clear();
                    //            mps_ec.Clear();
                    //            mps_temp.Clear();
                    //            tn.Clear();
                    //            tp.Clear();
                    //            toc.Clear();
                    //            refrigeration_temperature.Clear();
                    //            bottle_position.Clear();
                    //            foreach (data_value item in dayData)
                    //            {
                    //                mps_ph.AddNewDataValue(item.MPS_pH_status, item.MPS_pH);
                    //                mps_orp.AddNewDataValue(item.MPS_ORP_status, item.MPS_ORP);
                    //                mps_do.AddNewDataValue(item.MPS_DO_status, item.MPS_DO);
                    //                mps_turbidity.AddNewDataValue(item.MPS_Turbidity_status, item.MPS_Turbidity);
                    //                mps_ec.AddNewDataValue(item.MPS_EC_status, item.MPS_EC);
                    //                mps_temp.AddNewDataValue(item.MPS_Temp_status, item.MPS_Temp);
                    //                tn.AddNewDataValue(item.TN_status, item.TN);
                    //                tp.AddNewDataValue(item.TP_status, item.TP);
                    //                toc.AddNewDataValue(item.TOC_status, item.TOC);
                    //                refrigeration_temperature.AddNewDataValue(0, item.refrigeration_temperature);
                    //                bottle_position.AddNewDataValue(0, item.bottle_position);
                    //            }

                    //            // update to excel worksheet
                    //            row = startRow + day;

                    //            oExcelWorksheet.Cells[row, 2] = mps_ph.GetAverageOfMaxCountAsString();
                    //            oExcelWorksheet.Cells[row, 3] = mps_orp.GetAverageOfMaxCountAsString();
                    //            oExcelWorksheet.Cells[row, 4] = mps_do.GetAverageOfMaxCountAsString();
                    //            oExcelWorksheet.Cells[row, 5] = mps_turbidity.GetAverageOfMaxCountAsString();
                    //            oExcelWorksheet.Cells[row, 6] = mps_ec.GetAverageOfMaxCountAsString();
                    //            oExcelWorksheet.Cells[row, 7] = mps_temp.GetAverageOfMaxCountAsString();
                    //            oExcelWorksheet.Cells[row, 8] = tn.GetAverageOfMaxCountAsString();
                    //            oExcelWorksheet.Cells[row, 9] = tp.GetAverageOfMaxCountAsString();
                    //            oExcelWorksheet.Cells[row, 10] = toc.GetAverageOfMaxCountAsString();
                    //            oExcelWorksheet.Cells[row, 11] = refrigeration_temperature.GetAverageOfMaxCountAsString();
                    //            oExcelWorksheet.Cells[row, 12] = bottle_position.GetAverageOfMaxCountAsString();
                    //            oExcelWorksheet.Cells[row, 14] = maintenance_operator_name;
                    //            oExcelWorksheet.Cells[row, 15] = maintenance_start_time;
                    //            oExcelWorksheet.Cells[row, 16] = maintenance_end_time;
                    //            oExcelWorksheet.Cells[row, 17] = maintenance_equipments;


                    //            oExcelWorksheet.get_Range("b" + row).Interior.Color = ColorTranslator.ToOle(mps_ph.GetStatusColor());
                    //            oExcelWorksheet.get_Range("c" + row).Interior.Color = ColorTranslator.ToOle(mps_orp.GetStatusColor());
                    //            oExcelWorksheet.get_Range("d" + row).Interior.Color = ColorTranslator.ToOle(mps_do.GetStatusColor());
                    //            oExcelWorksheet.get_Range("e" + row).Interior.Color = ColorTranslator.ToOle(mps_turbidity.GetStatusColor());
                    //            oExcelWorksheet.get_Range("f" + row).Interior.Color = ColorTranslator.ToOle(mps_ec.GetStatusColor());
                    //            oExcelWorksheet.get_Range("g" + row).Interior.Color = ColorTranslator.ToOle(mps_temp.GetStatusColor());
                    //            oExcelWorksheet.get_Range("h" + row).Interior.Color = ColorTranslator.ToOle(tn.GetStatusColor());
                    //            oExcelWorksheet.get_Range("i" + row).Interior.Color = ColorTranslator.ToOle(tp.GetStatusColor());
                    //            oExcelWorksheet.get_Range("j" + row).Interior.Color = ColorTranslator.ToOle(toc.GetStatusColor());
                    //            oExcelWorksheet.get_Range("k" + row).Interior.Color = ColorTranslator.ToOle(refrigeration_temperature.GetStatusColor());
                    //            oExcelWorksheet.get_Range("l" + row).Interior.Color = ColorTranslator.ToOle(bottle_position.GetStatusColor());

                    //            oExcelWorksheet.get_Range("n" + row).Interior.Color = ColorTranslator.ToOle(maintenance_color);
                    //            oExcelWorksheet.get_Range("o" + row).Interior.Color = ColorTranslator.ToOle(maintenance_color);
                    //            oExcelWorksheet.get_Range("p" + row).Interior.Color = ColorTranslator.ToOle(maintenance_color);
                    //            oExcelWorksheet.get_Range("q" + row).Interior.Color = ColorTranslator.ToOle(maintenance_color);

                    //            dayOfYear = (new DateTime(year, month, day)).DayOfYear;
                    //            percent = (int)(dayOfYear * 100d / dayOfYearTotal);
                    //            //bgwMonthlyReport.ReportProgress(percent);

                    //            //Thread.Sleep(1);
                    //        }
                    //    }
                    //    oExcelWorkbook.SaveAs(newFilePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlShared, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    }
                }
            FileInfo fi = new FileInfo(newFilePath + ".xlsx");
            if (fi.Exists)
            {
                System.Diagnostics.Process.Start(newFilePath + ".xlsx");
            }
            else
            {
                //file doesn't exist
            }
            Console.ReadLine();
        }
        static void Main(string[] args)
        {
            Program ne = new Program();
            ne.backgroundWorkerMonthlyReport_DoWork();
        }
    }
}
