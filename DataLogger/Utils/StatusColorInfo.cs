using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogger.Utils
{
    public class StatusColorInfo
    {
        //0
        public static Color COL_STATUS_NORMAL = Color.FromArgb(0xffffff);


        //4
        public static Color COL_STATUS_MAINTENANCE = Color.FromArgb(0x0033cc);

        //5
        public static Color COL_STATUS_COMMUNICATION_ERROR = Color.FromArgb(0xffc000);

        //6
        public static Color COL_STATUS_INSTRUMENT_ERROR = Color.FromArgb(0xff0000);

        //7
        public static Color COL_STATUS_MAINTENANCE_PERIODIC = Color.FromArgb(0xffffff);

        //8
        public static Color COL_STATUS_MAINTENANCE_INCIDENT = Color.FromArgb(0x66ff33);
    }
}
