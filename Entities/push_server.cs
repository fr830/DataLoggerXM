using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogger.Entities
{
    public class  push_server
    {
        public int id { get; set; }
        public string ftp_ip { get; set; }
        public string ftp_username { get; set; }
        public string ftp_pwd { get; set; }
        public string ftp_folder { get; set; }
        public int ftp_flag { get; set; }
        public DateTime ftp_lasted { get; set; }

        public push_server()
        {
            id = -1;
            ftp_ip = "";
            ftp_username = "";
            ftp_pwd = "";
            ftp_folder = "";
            ftp_flag = -1;
            ftp_lasted = new DateTime();
        }
    }
}
