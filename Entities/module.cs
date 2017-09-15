using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DataLogger.Entities
{   
    public partial class module
    {
        
        public int id { get; set; }        
        public string item_name { get; set; }
        public int module_id { get; set; }        
        public int channel_number { get; set; }
        public string on_value { get; set; }
        public string off_value { get; set; }
        public int input_min { get; set; }
        public int input_max { get; set; }
        public int output_min { get; set; }
        public int output_max { get; set; }
        public double off_set { get; set; }
        public int error_min { get; set; }
        public int error_max { get; set; }
        public string unit { get; set; }
        public string value_column { get; set; }
        public string status_column { get; set; }
        public string display_name { get; set; }
        public int type_value { get; set; }
        public module()
        {
            id = -1;
            item_name = "";
            module_id = 1;
            channel_number = 1;
            on_value = "FAULT";
            off_value = "NORMAL";
            input_min = 0;
            input_max = 0;
            output_max = 0;
            output_min = 0;
            off_set = 0;
            error_min = 0;
            error_max = 1000;
            unit = "";
            value_column = "";
            status_column = "";
            display_name = "";
            type_value = 0;
        }
    }
}
