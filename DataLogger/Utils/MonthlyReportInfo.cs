using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogger.Utils
{
    public class MonthlyReportInfo
    {
        public int status { get; set; }
        public double sum { get; set; }
        public int count { get; set; }
    }


    public static class MonthlyReportInfoUtil
    {
        public static string GetAverageOfMaxCountAsString(this List<MonthlyReportInfo> monthlyReportInfoList)
        {
            string rv = "---";
            if (monthlyReportInfoList == null) return rv;
            MonthlyReportInfo mrinfo = monthlyReportInfoList.FirstOrDefault(m => m.count == monthlyReportInfoList.Max(mm => mm.count));
            double avg = -1;

            if (mrinfo != null)
                avg = mrinfo.sum / (double)mrinfo.count;

            if (avg >= 0)
                return avg.ToString("###,0.00");

            return rv;
        }

        public static void AddNewDataValue(this List<MonthlyReportInfo> monthlyReportInfoList, int status, double value)
        {
            if (monthlyReportInfoList == null) return;

            MonthlyReportInfo mrinfo = monthlyReportInfoList.FirstOrDefault(m => m.status == status);

            if (mrinfo != null)
            {
                mrinfo.sum += value;
                mrinfo.count++;
            }
            else
            {
                MonthlyReportInfo newmrInfo = new MonthlyReportInfo();

                newmrInfo.count = 1;
                newmrInfo.sum = value;
                newmrInfo.status = status;

                monthlyReportInfoList.Add(newmrInfo);
            }

        }

        public static Color GetStatusColor(this List<MonthlyReportInfo> monthlyReportInfoList)
        {
            if (monthlyReportInfoList != null)
            {
                MonthlyReportInfo mrinfo = monthlyReportInfoList.FirstOrDefault(m => m.count == monthlyReportInfoList.Max(mm => mm.count));
                if (mrinfo != null)
                {
                    switch (mrinfo.status)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3: return StatusColorInfo.COL_STATUS_NORMAL;
                        case 4: return StatusColorInfo.COL_STATUS_MAINTENANCE;
                        case 5: return StatusColorInfo.COL_STATUS_COMMUNICATION_ERROR;
                        case 6: return StatusColorInfo.COL_STATUS_INSTRUMENT_ERROR;
                        case 7: return StatusColorInfo.COL_STATUS_MAINTENANCE_PERIODIC;
                        case 8: return StatusColorInfo.COL_STATUS_MAINTENANCE_INCIDENT;
                        default:
                            break;
                    }
                }
            }
            return Color.White;
        }
    }
}
