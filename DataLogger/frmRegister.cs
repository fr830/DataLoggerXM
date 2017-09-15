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
    public partial class frmRegister : Form
    {
        //ResourceManager res_man;    // declare Resource manager to access to specific cultureinfo
        //CultureInfo cul;            //declare culture info

        LanguageService lang;
        public frmRegister(LanguageService _lang)
        {
            InitializeComponent();
            //res_man = obj_res_man;
            //cul = obj_cul;
            lang = _lang;
            switch_language();
        }
        private void switch_language()
        {
            this.Text = lang.getText("form_register_title");
            this.lblRegisterUser.Text = lang.getText("form_register_title");
            this.lblUsername.Text = lang.getText("form_register_user_name");
            this.lblPassword.Text = lang.getText("form_register_password");
            this.lblReEnterPassword.Text = lang.getText("form_register_re_enter_password");
            this.lblIDNumber.Text = lang.getText("form_register_id_number");
            this.lblName.Text = lang.getText("form_register_name");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text)
                || string.IsNullOrEmpty(txtName.Text)
                || string.IsNullOrEmpty(txtIDNumber.Text)
                || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show(lang.getText("please_check_pass"));
                return;
            }
            if (txtPassword.Text.Trim() != txtReEnterPassword.Text.Trim())
            {
                MessageBox.Show(lang.getText("check_same_password_error"));
                return;
            }
            user objUser = new user();
            objUser.user_groups_id = 2;
            objUser.user_name = txtUsername.Text;
            objUser.password = Crypto.HashPassword(txtPassword.Text);
            objUser.name = txtName.Text;
            objUser.id_number = txtIDNumber.Text;

            if (new user_repository().add(ref objUser) > 0)
            {
                // ok
                MessageBox.Show(lang.getText("successfully"));
            }
            else
            {
                MessageBox.Show(lang.getText("fail"));
            }
            this.Close();
        }

        private void txtIDNumber_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
