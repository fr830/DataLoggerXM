using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogger.Entities
{
    public class maintenance_log
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public virtual string user_name { get; set; }
        public virtual string name { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }        
        public int maintenance_reason { get; set; }
        public int tn { get; set; }
        public int tp { get; set; }
        public int toc { get; set; }
        public int mps { get; set; }
        public int auto_sampler { get; set; }
        public int pumping_system { get; set; }
        public int other { get; set; }
        public string other_para { get; set; }
        public string note { get; set; }
        public maintenance_log()
        {
            id = -1;
            user_name = "";
            user_id = -1;
            start_time = DateTime.Now;
            end_time = DateTime.Now;
            maintenance_reason = 0;
            tn = 0;
            tp = 0;
            toc = 0;
            mps = 0;
            auto_sampler = 0;
            pumping_system = 0;
            other = 0;
            other_para = "";
            note = "";
        }
    }
}
