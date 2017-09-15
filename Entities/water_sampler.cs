using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLogger.Entities
{
    public class water_sampler
    {
        public int id { get; set; }
        public string equipment_name { get; set; }
        public DateTime response_time { get; set; }
        public Double refrigeration_Temperature { get; set; }
        public int bottle_position { get; set; }
        public int door_status { get; set; }
        public int status_info { get; set; }
        public int equipment_status { get; set; }
        public string addInfo { get; set; }
        public string comm_port { get; set; }
        public DateTime created { get; set; }
        public DateTime latest_update_communication { get; set; }
        public water_sampler()
        {
            id = -1;
            equipment_name = "";
            response_time = new DateTime();
            refrigeration_Temperature = -1;
            bottle_position = -1;
            equipment_status = -1;
            door_status = -1;
            comm_port = "";
            addInfo = "";
            status_info = -1;
            created = new DateTime();
            latest_update_communication = DateTime.Now;
        }
    }
}
