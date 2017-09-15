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
    public partial class frmChangePassword : Form
    {
        //ResourceManager res_man;    // declare Resource manager to access to specific cultureinfo
        //CultureInfo cul;            //declare culture info

        LanguageService lang;

        public static int current_user_id = -1;
        public frmChangePassword()
        {
            InitializeComponent();
        }

        public frmChangePassword(int user_id, LanguageService _lang)
        {
            InitializeComponent();
            current_user_id = user_id;
            //res_man = obj_res_man;
            //cul = obj_cul;
            lang = _lang;
            switch_language();
        }
        private void switch_language()
        {
            this.Text = lang.getText("form_change_password_title");
            this.lblHeaderTitle.Text = lang.getText("form_change_password_title");
            this.lblNewPassword.Text = lang.getText("form_change_password_new_password");
            this.lblReEnterNewPassword.Text = lang.getText("form_change_password_re_enter_new_password");
            this.btnSave.Text = lang.getText("form_change_password_button_save");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {

                MessageBox.Show(lang.getText("please_check_pass"));
                return;
            }
            if (txtPassword.Text.Trim() != txtRePassword.Text.Trim())
            {
                MessageBox.Show(lang.getText("check_same_password_error"));
                return;
            }
            if (current_user_id > 0)
            {
                user objUser = new user_repository().get_info_by_id(current_user_id);

                objUser.password = Crypto.HashPassword(txtPassword.Text);
                if (new user_repository().update(ref objUser) > 0)
                {
                    // ok
                    MessageBox.Show(lang.getText("change_password_successfully"));
                    this.Close();
                }
                else
                {
                    // fail
                    MessageBox.Show(lang.getText("change_password_fail"));
                    this.Close();
                }
            }

        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {

        }
    }
}
