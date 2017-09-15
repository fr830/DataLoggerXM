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
using DataLogger.Data;
using System.Web.Helpers;
using System.Resources;
using System.Reflection;
using System.Globalization;
using DataLogger.Utils;
namespace DataLogger
{
    public partial class frmUserManagement : Form
    {
        //ResourceManager res_man;    // declare Resource manager to access to specific cultureinfo
        //CultureInfo cul;            //declare culture info

        LanguageService lang;

        DataTable dt = new DataTable();
        public static int selected_row_index;
        public static int selected_column_index;
        public static int current_user_id = 0;
        public frmUserManagement(LanguageService _lang)
        {
            InitializeComponent();
            //res_man = obj_res_man;
            //cul = obj_cul;
            lang = _lang;
            switch_language();
        }
        private void switch_language()
        {
            this.Text = lang.getText("form_user_management_title");
            this.lblHeaderTitle.Text = lang.getText("form_user_management_title");
            this.lblHeaderTitle.Text = lang.getText("form_user_management_title");
            this.lblHeaderTitle.Text = lang.getText("form_user_management_title");
            this.lblHeaderTitle.Text = lang.getText("form_user_management_title");
            this.lblHeaderTitle.Text = lang.getText("form_user_management_title");
            this.lblHeaderTitle.Text = lang.getText("form_user_management_title");
            this.lblHeaderTitle.Text = lang.getText("form_user_management_title");
        }

        private void frmUserManagement_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("user_name", typeof(string));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("id_number", typeof(string));
            dt.Columns.Add("user_group_name", typeof(string));
            dt.Columns.Add("user_group_id", typeof(int));

            loadDataAll();

            btnDelete.Enabled = false;
            btnSetDefaultPassword.Enabled = false;
            btnChangePassword.Enabled = false;
            btnSave.Enabled = false;
        }

        private void loadDataAll()
        {
            try
            {
                dt.Clear();
                // load all
                IEnumerable<user> listUser = new user_repository().get_all();
                foreach (user item in listUser)
                {
                    dt.Rows.Add(item.id, item.user_name, item.name, item.id_number,
                        item.user_group_name, item.user_groups_id);
                }
                dgvData.DataSource = dt;

                dgvData.Columns["user_group_id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmRegister frm = new frmRegister(lang);
            frm.ShowDialog();
            loadDataAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                btnDelete.Enabled = false;
                btnSetDefaultPassword.Enabled = false;
                btnChangePassword.Enabled = false;
                btnSave.Enabled = false;

                if (new user_repository().delete(current_user_id))
                {
                    // ok
                }
                else
                {

                    // fail
                    MessageBox.Show(lang.getText("fail"));
                    return;
                }
                dt.Rows.RemoveAt(dgvData.CurrentCell.RowIndex);
                dgvData.DataSource = dt;
                MessageBox.Show(lang.getText("successfully"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                current_user_id = Convert.ToInt32(dgvData.CurrentRow.Cells[0].Value);

                txtUserName.Text = Convert.ToString(dgvData.CurrentRow.Cells[1].Value);
                txtName.Text = Convert.ToString(dgvData.CurrentRow.Cells[2].Value);
                txtIDNumber.Text = Convert.ToString(dgvData.CurrentRow.Cells[3].Value);
                cbUserGroup.SelectedIndex = Convert.ToInt32(dgvData.CurrentRow.Cells[5].Value);

                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                btnSetDefaultPassword.Enabled = true;
                btnChangePassword.Enabled = true;
            }
            catch (Exception)
            {

                //throw;
            }

        }

        private void btnSetDefaultPassword_Click(object sender, EventArgs e)
        {
            if (current_user_id > 0)
            {
                user objUser = new user_repository().get_info_by_id(current_user_id);
                objUser.password = Crypto.HashPassword("123456");

                if (new user_repository().update(ref objUser) > 0)
                {
                    // ok


                }
                else
                {
                    // fail
                }
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (current_user_id > 0)
            {
                user objUser = new user_repository().get_info_by_id(current_user_id);
                objUser.user_groups_id = cbUserGroup.SelectedIndex + 1 ;
                objUser.name = txtName.Text;
                objUser.user_name = txtUserName.Text;
                objUser.id_number = txtIDNumber.Text;

                if (new user_repository().update(ref objUser) > 0)
                {
                    // ok
                    dt.Rows[dgvData.CurrentCell.RowIndex]["user_name"] = txtUserName.Text;
                    dt.Rows[dgvData.CurrentCell.RowIndex]["name"] = txtName.Text;
                    dt.Rows[dgvData.CurrentCell.RowIndex]["id_number"] = txtIDNumber.Text;
                    dt.Rows[dgvData.CurrentCell.RowIndex]["user_group_id"] = cbUserGroup.SelectedIndex ;
                    dt.Rows[dgvData.CurrentCell.RowIndex]["user_group_name"] = cbUserGroup.Text;

                    dgvData.DataSource = dt;
                }
                else
                {
                    // fail
                }
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (current_user_id > 0)
            {
                frmChangePassword frm = new frmChangePassword(current_user_id, lang);
                frm.ShowDialog();
                loadDataAll();
            }

        }
    }
}
