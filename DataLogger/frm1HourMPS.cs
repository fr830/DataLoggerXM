using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLogger.Entities;
using System.Resources;
using System.Globalization;
using DataLogger.Utils;
using DataLogger.Data;
using System.Reflection;

namespace DataLogger
{
    public partial class frm1HourMPS : Form
    {

        LanguageService lang;

        public data_value obj_data_value { get; set; }
        public frm1HourMPS()
        {
            InitializeComponent();
        }

        public frm1HourMPS(data_value obj, LanguageService _lang)
        {
            InitializeComponent();
            obj_data_value = obj;
            //res_man = obj_res_man;
            //cul = obj_cul;
            lang = _lang;
            switch_language();
        }
        private void switch_language()
        {
            this.lblHeaderTitle.Text = lang.getText("form_1_hour_title");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm1HourMPS_Load(object sender, EventArgs e)
        {
            GlobalVar.stationSettings = new station_repository().get_info();
            GlobalVar.moduleSettings = new module_repository().get_all();
            for (int i = 1; i <= GlobalVar.moduleSettings.Count(); i++)
            {
                foreach (var item in GlobalVar.moduleSettings)
                {
                    string currentvar = "var" + i.ToString();
                    string currentlabelname = "txt" + currentvar;
                    string currentlabelunit = "txt" + currentvar + "Unit";
                    string currentlabelvalue = "txt" + currentvar + "Value";
                    if (item.item_name.Equals(currentvar))
                    {

                        frmNewMain.ClearLabel(this, item.display_name, currentlabelname);
                        frmNewMain.ClearLabel(this, item.unit, currentlabelunit);

                        string param = currentvar;  //VD : var1
                        Type paramType = typeof(data_value);
                        PropertyInfo prop = paramType.GetProperty(param);
                        double value = (double)prop.GetValue(obj_data_value); //VD : lay display_var = data.var1
                        frmNewMain.ClearTextbox(this, value.ToString("##0.00"), currentlabelvalue);
                    }
                }
            }
        }
    }
}
