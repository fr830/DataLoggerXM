using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLogger.Data;
using System.Windows.Forms.DataVisualization.Charting;
using DataLogger.Utils;
using DataLogger.Entities;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace DataLogger
{
    public partial class frmMaintenance : Form
    {
        //ResourceManager res_man;    // declare Resource manager to access to specific cultureinfo
        //CultureInfo cul;            //declare culture info
        LanguageService lang;

        public static maintenance_log objMaintenanceLog = new maintenance_log();
        /// <summary>
        /// contructor
        /// </summary>
        public frmMaintenance(LanguageService _lang)
        {
            InitializeComponent();
            //res_man = obj_res_man;
            //cul = obj_cul;
            lang = _lang;
            switch_language();
        }
        private void switch_language()
        {
            this.Text = lang.getText("form_maintenance_title");
            this.lblHeaderTitle.Text = lang.getText("form_maintenance_title");
            this.lblOperatorHeader.Text = lang.getText("form_maintenance_operator_group");
            this.lblOperatorName.Text = lang.getText("form_maintenance_operator_group_operator_name");
            this.lblIDNumber.Text = lang.getText("form_maintenance_operator_id_number");
            this.lblMaintenanceDate.Text = lang.getText("form_maintenance_operator_maintenance_date");
            this.lblStartTime.Text = lang.getText("form_maintenance_operator_start_time");
            this.lblEndTime.Text = lang.getText("form_maintenance_operator_end_time");
            this.lblMaintenanceReason.Text = lang.getText("form_maintenance_operator_maintenance_reason");
            this.rbtnPeriod.Text = lang.getText("form_maintenance_operator_periodic");
            this.rbtnIncident.Text = lang.getText("form_maintenance_operator_incident");            
            //this.lblEquipmentsHeader.Text = lang.getText("form_maintenance_equipments_group");
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (btnStartStop.Text == "Start")
            {
                btnSave.Enabled = false;
                btnStartStop.Text = "Stop";
                this.btnStartStop.Image = global::DataLogger.Properties.Resources.Stop_maintenance_48x48;

                GlobalVar.isMaintenanceStatus = true;
                GlobalVar.maintenanceLog.auto_sampler = (chkBoxAutoSampler.Checked) ? 1 : 0;
                GlobalVar.maintenanceLog.end_time = DateTime.Now;
                GlobalVar.maintenanceLog.maintenance_reason = (rbtnPeriod.Checked) ? 0 : 1;// 0: Period, 1: Incident
                GlobalVar.maintenanceLog.mps = (chkBoxMPS.Checked) ? 1 : 0;
                GlobalVar.maintenanceLog.note = rtxtNote.Text;
                GlobalVar.maintenanceLog.other = (chkBoxOther.Checked) ? 1 : 0;
                GlobalVar.maintenanceLog.other_para = txtOther.Text;
                GlobalVar.maintenanceLog.pumping_system = (chkBoxPumpingSystem.Checked) ? 1 : 0;
                GlobalVar.maintenanceLog.start_time = DateTime.Now;
                GlobalVar.maintenanceLog.tn = (chkBoxTN.Checked) ? 1 : 0;
                GlobalVar.maintenanceLog.toc = (chkBoxTOC.Checked) ? 1 : 0;
                GlobalVar.maintenanceLog.tp = (chkBoxTP.Checked) ? 1 : 0;
                GlobalVar.maintenanceLog.user_id = GlobalVar.loginUser.id;
                GlobalVar.maintenanceLog.user_name = GlobalVar.loginUser.user_name;

                objMaintenanceLog.auto_sampler = (chkBoxAutoSampler.Checked) ? 1 : 0;
                objMaintenanceLog.end_time = DateTime.Now;
                objMaintenanceLog.maintenance_reason = (rbtnPeriod.Checked) ? 0 : 1;// 0: Period, 1: Incident
                objMaintenanceLog.mps = (chkBoxMPS.Checked) ? 1 : 0;
                objMaintenanceLog.note = rtxtNote.Text;
                objMaintenanceLog.other = (chkBoxOther.Checked) ? 1 : 0;
                objMaintenanceLog.other_para = txtOther.Text;
                objMaintenanceLog.pumping_system = (chkBoxPumpingSystem.Checked) ? 1 : 0;
                objMaintenanceLog.start_time = DateTime.Now;
                objMaintenanceLog.tn = (chkBoxTN.Checked) ? 1 : 0;
                objMaintenanceLog.toc = (chkBoxTOC.Checked) ? 1 : 0;
                objMaintenanceLog.tp = (chkBoxTP.Checked) ? 1 : 0;
                objMaintenanceLog.user_id = GlobalVar.loginUser.id;
                objMaintenanceLog.user_name = GlobalVar.loginUser.user_name;

                dtStartTime.Value = DateTime.Now;
            }
            else
            {
                btnSave.Enabled = true;
                btnStartStop.Text = "Start";
                this.btnStartStop.Image = global::DataLogger.Properties.Resources.Play_48x48;
                GlobalVar.isMaintenanceStatus = false;
                //GlobalVar.maintenanceLog = null;
                objMaintenanceLog.end_time = DateTime.Now;
                dtEndTime.Value = DateTime.Now;
            }
        }

        private void frmMaintenance_Load(object sender, EventArgs e)
        {
            txtIDNumber.Text = GlobalVar.loginUser.id_number;
            txtOperatorName.Text = GlobalVar.loginUser.name;

            txtIDNumber.ReadOnly = true;
            txtOperatorName.ReadOnly = true;
        }

        private void frmMaintenance_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalVar.isMaintenanceStatus = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (new maintenance_log_repository().add(ref objMaintenanceLog) >= 0)
            {
                // ok
                MessageBox.Show(lang.getText("successfully"));
            }
            else
            {
                //
                MessageBox.Show(lang.getText("fail"));
            }
            this.Close();
        }

    }
}
