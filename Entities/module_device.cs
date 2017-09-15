using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLogger.Entities
{
    public class module_device
    {
        public int id { get; set; }
        public int station_id { get; set; }
        public string module_name { get; set; }
        public string module_code { get; set; }        
        public DateTime created { get; set; }

        public module_device()
        {
            id = -1;
            station_id = -1;
            module_name = "";
            module_code = "";            
            
            created = new DateTime();            
        }
    }
}
