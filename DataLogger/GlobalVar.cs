using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLogger.Entities;

namespace DataLogger
{
   /// <summary>
   /// Keep global variable for entire application
   /// </summary>
    public static class GlobalVar
    {        
        // USER FOR CHECK LOGIN AND LOGIN INFORMATION
        public static bool isLogin = false;        
        public static user loginUser;
        public static station stationSettings;
        public static IEnumerable<module> moduleSettings { get; set; }
        public static bool isAdmin()
        {
            if (isLogin && loginUser != null)
            {
                if (loginUser.user_groups_id == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool isOperator()
        {
            if (isLogin && loginUser != null)
            {
                if (loginUser.user_groups_id == 2)
                {
                    return true;
                }
            }
            return false;
        }

        // USER FOR MAINTENANCE STATUS PROGRESS
        public static bool isMaintenanceStatus = false;
        public static maintenance_log maintenanceLog { get; set; }

        public static int calibrateTOCStatus = CommonInfo.CALIBRATION_STATUS_STOP;
        public static int calibrateTNStatus = CommonInfo.CALIBRATION_STATUS_STOP;
        public static int calibrateTPStatus = CommonInfo.CALIBRATION_STATUS_STOP;
    }
}
