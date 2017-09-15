using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DataLogger.Entities
{
    public partial class setting
    {

        public int id { get; set; }
        public string setting_key { get; set; }
        public string setting_value { get; set; }
        public string setting_type { get; set; }
        public string note { get; set; }
        public DateTime setting_datetime { get; set; }

        public setting()
        {
            id = -1;
            setting_key = "";
            setting_value = "";
            setting_type = "";
            note = "";
            setting_datetime = new DateTime();
        }
    }
}
