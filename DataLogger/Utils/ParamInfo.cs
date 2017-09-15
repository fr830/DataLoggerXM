using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogger.Utils
{
    public class ParamInfo
    {
        public string NameDB { get; set; }
        public string  NameDisplay { get; set; }
        public bool HasStatus { get; set; }
        public string StatusNameDB { get; set; }
        public string StatusNameDisplay { get; set; }
        public string StatusNameVisible { get; set; }
        public bool Selected { get; set; }
        public Color GraphColor { get; set; }
    }

    public static class DataLoggerParam
    {
        public static List<ParamInfo> PARAMETER_LIST = new List<ParamInfo>()
        {
        };

    }
}
