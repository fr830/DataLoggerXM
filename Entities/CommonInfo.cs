using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogger.Entities
{
    public static class CommonInfo
    {
        public const int INT_STATUS_MEASURING = 0;

        public const int TRANSACTION_ADD_NEW = 1;
        public const int TRANSACTION_UPDATE = 2;

        public const int PERIOD_CHECK_COMMUNICATION_ERROR = 35;


        public const int INT_ADAM_4017_1 = 1;
        public const int INT_ADAM_4050 = 2;
        public const int INT_ADAM_4051 = 3;
        public const int INT_ADAM_4017_2 = 4;

        public const string ADAM_4017_1 = "ADAM40171";
        public const string ADAM_4017_2 = "ADAM40172";
        public const string ADAM_4050 = "ADAM4050";
        public const string ADAM_4051 = "ADAM4051";
        public const string ADAM_TEMP_HUMIDITY = "TEMP_HUMIDITY";

        public const string CODE_MPS_PH = "CODE_MPS_PH";
        public const string CODE_MPS_EC = "CODE_MPS_EC";
        public const string CODE_MPS_DO = "CODE_MPS_DO";
        public const string CODE_MPS_TURBIDITY = "CODE_MPS_TURBIDITY";
        public const string CODE_MPS_ORP = "CODE_MPS_ORP";
        public const string CODE_MPS_TEMP = "CODE_MPS_TEMP";
        public const string CODE_TN = "CODE_TN";
        public const string CODE_TP = "CODE_TP";
        public const string CODE_TOC = "CODE_TOC";

        public const int INT_STATUS_NORMAL = 0;
        public const int INT_STATUS_MEASURING_STOP = 1;
        public const int INT_STATUS_EMPTY_SAMPLER_RESERVOIR = 2;
        public const int INT_STATUS_CALIBRATING = 3;
        public const int INT_STATUS_MAINTENANCE = 4;
        public const int INT_STATUS_COMMUNICATION_ERROR = 5;
        public const int INT_STATUS_INSTRUMENT_ERROR = 6;       

        public const string STATUS_ERROR = "Error";
        public const string STATUS_Normal = "Normal";
        public const string STATUS_WARNING = "Warning";
        public const string STATUS_MEASURING = "Measuring";

        public const int MAINTENANCE_PERIOD = 0;
        public const int MAINTENANCE_INCIDENT = 1;

        public const int CALIBRATION_STATUS_STOP = 0;
        public const int CALIBRATION_STATUS_START = 1;
        public const int CALIBRATION_STATUS_IN_PROGRESS = 2;
        public const int CALIBRATION_STATUS_DONE = 3;
    }
}
