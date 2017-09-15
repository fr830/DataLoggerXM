using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLogger.Entities
{
    public class station
    {
        public int id { get; set; }
        public string station_name { get; set; }
        public string station_id { get; set; }
        public int socket_port { get; set; }        
        public DateTime modified { get; set; }

        public string sampler_comport { get; set; }
        public string tn_comport { get; set; }
        public string tp_comport { get; set; }
        public string toc_comport { get; set; }
        public string mps_comport { get; set; }
        public string module_comport { get; set; }

        // 1: KECO_STD; 2: ANALYZER; 3: MODBUS
        public int mps_protocol { get; set; }
        public int tn_protocol { get; set; }
        public int tp_protocol { get; set; }
        public int toc_protocol { get; set; }
        public string do1_caption { get; set; }
        public string do2_caption { get; set; }
        public string do3_caption { get; set; }
        public string do4_caption { get; set; }
        public string do5_caption { get; set; }
        public string do6_caption { get; set; }
        public string do7_caption { get; set; }
        public string do8_caption { get; set; }

        public string do1_caption_vi { get; set; }
        public string do2_caption_vi { get; set; }
        public string do3_caption_vi { get; set; }
        public string do4_caption_vi { get; set; }
        public string do5_caption_vi { get; set; }
        public string do6_caption_vi { get; set; }
        public string do7_caption_vi { get; set; }
        public string do8_caption_vi { get; set; }

        public string ftpserver { get; set; }
        public string ftpusername { get; set; }
        public string ftppassword { get; set; }
        public string ftpfolder { get; set; }
        public int ftpflag { get; set; }
        public station()
        {
            id = -1;
            station_name = "Station No.1";
            station_id = "BLVTRS0001";
            socket_port = 3001;
            modified = new DateTime();

            sampler_comport = "COM100";
            tn_comport = "COM100";
            tp_comport = "COM100";
            toc_comport = "COM100";
            mps_comport = "COM100";
            module_comport = "COM100";

            mps_protocol = 3; // MODBUS
            tn_protocol = 1; // KECO_STD
            tp_protocol = 1;
            toc_protocol = 1;

            do1_caption = "D/O #1";
            do2_caption = "D/O #2";
            do3_caption = "D/O #3";
            do4_caption = "D/O #4";
            do5_caption = "D/O #5";
            do6_caption = "D/O #6";
            do7_caption = "D/O #7";
            do8_caption = "D/O #8";

            do1_caption_vi = "D/O #1";
            do2_caption_vi = "D/O #2";
            do3_caption_vi = "D/O #3";
            do4_caption_vi = "D/O #4";
            do5_caption_vi = "D/O #5";
            do6_caption_vi = "D/O #6";
            do7_caption_vi = "D/O #7";
            do8_caption_vi = "D/O #8";

            ftpserver = "";
            ftpusername = "";
            ftppassword = "";
            ftpfolder = "";
            ftpflag = -1;

        }
    }
}
