using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLogger.Entities
{
    public class measured_device
    {
        public int id { get; set; }
        public int station_id { get; set; }
        public string device_name { get; set; }
        public string device_code { get; set; }
        public int automatic_status { get; set; }
        public int manual_status { get; set; }
        public string comm_port { get; set; }
        public DateTime created { get; set; }

        public measured_device()
        {
            id = -1;
            station_id = -1;
            device_name = "";
            device_code = "";            
            
            created = new DateTime();            
        }
    }
}
