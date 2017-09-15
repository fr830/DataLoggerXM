using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLogger.Entities
{
    public class data_5minute_value
    {
        public int id { get; set; }
        public Double MPS_pH { get; set; }
        public int MPS_pH_status { get; set; }
        public Double MPS_EC { get; set; }
        public int MPS_EC_status { get; set; }
        public Double MPS_DO { get; set; }
        public int MPS_DO_status { get; set; }
        public Double MPS_Turbidity { get; set; }
        public int MPS_Turbidity_status { get; set; }
        public Double MPS_ORP { get; set; }
        public int MPS_ORP_status { get; set; }
        public Double MPS_Temp { get; set; }
        public int MPS_Temp_status { get; set; }
        public Double MPS_SS { get; set; }
        public int MPS_SS_status { get; set; }
        public Double TN { get; set; }
        public int TN_status { get; set; }
        public Double TP { get; set; }
        public int TP_status { get; set; }
        public Double TOC { get; set; }
        public int TOC_status { get; set; }

        public int module_Power { get; set; }
        public int module_UPS { get; set; }
        public int module_Door { get; set; }
        public int module_Fire { get; set; }
        public int module_Flow { get; set; }
        public int module_PumpLAM { get; set; }
        public int module_PumpLRS { get; set; }
        public int module_PumpLFLT { get; set; }
        public int module_PumpRAM { get; set; }
        public int module_PumpRRS { get; set; }
        public int module_PumpRFLT { get; set; }
        public int module_air1 { get; set; }
        public int module_air2 { get; set; }
        public int module_cleaning { get; set; }
        public Double module_Temperature { get; set; }
        public Double module_Humidity { get; set; }

        public DateTime created { get; set; }

        public DateTime stored_date { get; set; }
        public int stored_hour { get; set; }
        public int stored_minute { get; set; }

        public int MPS_status { get; set; }

        // sampler data
        public Double refrigeration_temperature { get; set; }
        public int bottle_position { get; set; }
        public int door_status { get; set; } // 0->6; 0: close
        public int equipment_status { get; set; } // 0:normal
        // 0:normal; 6: Maintenance; 7: periodic; 8: incident
        public int pumping_system_status { get; set; }
        // 0:normal; 6: Maintenance; 7: periodic; 8: incident
        public int station_status { get; set; }
        public int push { get; set; }
        public DateTime push_time { get; set; }
        public data_5minute_value()
        {
            id = -1;
            MPS_pH = -1;
            MPS_EC = -1;
            MPS_DO = -1;
            MPS_Turbidity = -1;
            MPS_ORP = -1;
            MPS_Temp = -1;
            MPS_SS = -1;
            TN = -1;
            TP = -1;
            TOC = -1;

            MPS_status = -1;
            MPS_pH_status = -1;
            MPS_EC_status = -1;
            MPS_DO_status = -1;
            MPS_Turbidity_status = -1;
            MPS_ORP_status = -1;
            MPS_Temp_status = -1;
            MPS_SS_status = -1;
            TN_status = -1;
            TP_status = -1;
            TOC_status = -1;

            module_Power = -1;
            module_UPS = -1;
            module_Door = -1;
            module_Fire = -1;
            module_Flow = -1;
            module_PumpLAM = -1;
            module_PumpLRS = -1;
            module_PumpLFLT = -1;
            module_PumpRAM = -1;
            module_PumpRRS = -1;
            module_PumpRFLT = -1;
            module_air1 = -1;
            module_air2 = -1;
            module_cleaning = -1;

            module_Temperature = -1;
            module_Humidity = -1;

            created = DateTime.Now;

            stored_date = DateTime.Now;
            stored_hour = -1;
            stored_minute = -1;

            refrigeration_temperature = -1;
            bottle_position = -1;
            door_status = -1;
            equipment_status = -1;
            pumping_system_status = 0;
            station_status = 0;

            push = -1;
            push_time = new DateTime();
        }
    }
}
