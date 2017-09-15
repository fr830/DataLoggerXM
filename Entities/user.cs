using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogger.Entities
{
    public class user
    {
        public int id { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string id_number { get; set; }
        public int user_groups_id { get; set; }
        public string user_group_name { get; set; }
        public user()
        {
            id = -1;
            user_name = "";
            password = "";
            name = "";
            id_number = "";
            user_groups_id = -1;
            user_group_name = "";
        }
    }
}
