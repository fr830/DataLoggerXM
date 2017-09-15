using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLogger.Entities
{
    public class data_value
    {
        public int id { get; set; }
        public Double var1 { get; set; }
        public int var1_status { get; set; }
        public Double var2 { get; set; }
        public int var2_status { get; set; }
        public Double var3 { get; set; }
        public int var3_status { get; set; }
        public Double var4 { get; set; }
        public int var4_status { get; set; }
        public Double var5 { get; set; }
        public int var5_status { get; set; }
        public Double var6 { get; set; }
        public int var6_status { get; set; }
        public Double var7 { get; set; }
        public int var7_status { get; set; }
        public Double var8 { get; set; }
        public int var8_status { get; set; }
        public Double var9 { get; set; }
        public int var9_status { get; set; }
        public Double var10 { get; set; }
        public int var10_status { get; set; }
        public Double var11 { get; set; }
        public int var11_status { get; set; }
        public Double var12 { get; set; }
        public int var12_status { get; set; }
        public Double var13 { get; set; }
        public int var13_status { get; set; }
        public Double var14 { get; set; }
        public int var14_status { get; set; }
        public Double var15 { get; set; }
        public int var15_status { get; set; }
        public Double var16 { get; set; }
        public int var16_status { get; set; }
        public Double var17 { get; set; }
        public int var17_status { get; set; }
        public Double var18 { get; set; }
        public int var18_status { get; set; }


        public DateTime created { get; set; }

        public DateTime stored_date { get; set; }
        public int stored_hour { get; set; }
        public int stored_minute { get; set; }

        public int MPS_status { get; set; }

        public int push { get; set; }
        public DateTime push_time { get; set; }
        public data_value()
        {
            id = -1;
            var1 = -1;
            var2 = -1;
            var3 = -1;
            var4 = -1;
            var5 = -1;
            var6 = -1;
            var7 = -1;
            var8 = -1;
            var9 = -1;
            var10 = -1;
            var11 = -1;
            var12 = -1;
            var13 = -1;
            var14 = -1;
            var15 = -1;
            var16 = -1;
            var17 = -1;
            var18 = -1;

            var1_status = -1;
            var2_status = -1;
            var3_status = -1;
            var4_status = -1;
            var5_status = -1;
            var6_status = -1;
            var7_status = -1;
            var8_status = -1;
            var9_status = -1;
            var10_status = -1;
            var11_status = -1;
            var12_status = -1;
            var13_status = -1;
            var14_status = -1;
            var15_status = -1;
            var16_status = -1;
            var17_status = -1;
            var18_status = -1;

            created = DateTime.Now;

            stored_date = DateTime.Now;
            stored_hour = -1;
            stored_minute = -1;


            push = -1;
            push_time = new DateTime();
        }
    }
    public class measured_data
    {
        public Double var1 { get; set; }
        public int var1_status { get; set; }
        public Double var2 { get; set; }
        public int var2_status { get; set; }
        public Double var3 { get; set; }
        public int var3_status { get; set; }
        public Double var4 { get; set; }
        public int var4_status { get; set; }
        public Double var5 { get; set; }
        public int var5_status { get; set; }
        public Double var6 { get; set; }
        public int var6_status { get; set; }
        public Double var7 { get; set; }
        public int var7_status { get; set; }
        public Double var8 { get; set; }
        public int var8_status { get; set; }
        public Double var9 { get; set; }
        public int var9_status { get; set; }
        public Double var10 { get; set; }
        public int var10_status { get; set; }
        public Double var11 { get; set; }
        public int var11_status { get; set; }
        public Double var12 { get; set; }
        public int var12_status { get; set; }
        public Double var13 { get; set; }
        public int var13_status { get; set; }
        public Double var14 { get; set; }
        public int var14_status { get; set; }
        public Double var15 { get; set; }
        public int var15_status { get; set; }
        public Double var16 { get; set; }
        public int var16_status { get; set; }
        public Double var17 { get; set; }
        public int var17_status { get; set; }
        public Double var18 { get; set; }
        public int var18_status { get; set; }
        public int MPS_status { get; set; }

        public DateTime created { get; set; }
        public int push { get; set; }
        public DateTime push_time { get; set; }

        public DateTime latest_update_MPS_communication { get; set; }

        public measured_data()
        {
            var1 = -1;
            var2 = -1;
            var3 = -1;
            var4 = -1;
            var5 = -1;
            var6 = -1;
            var7 = -1;
            var8 = -1;
            var9 = -1;
            var10 = -1;
            var11 = -1;
            var12 = -1;
            var13 = -1;
            var14 = -1;
            var15 = -1;
            var16 = -1;
            var17 = -1;
            var18 = -1;

            var1_status = -1;
            var2_status = -1;
            var3_status = -1;
            var4_status = -1;
            var5_status = -1;
            var6_status = -1;
            var7_status = -1;
            var8_status = -1;
            var9_status = -1;
            var10_status = -1;
            var11_status = -1;
            var12_status = -1;
            var13_status = -1;
            var14_status = -1;
            var15_status = -1;
            var16_status = -1;
            var17_status = -1;
            var18_status = -1;

            MPS_status = -1;

            created = DateTime.Now;

            push = -1;
            push_time = new DateTime();

            latest_update_MPS_communication = DateTime.Now;
        }
    }

}
