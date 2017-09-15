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
using System.Resources;
using System.Reflection;
using System.Globalization;
using DataLogger.Utils;

namespace DataLogger
{
    public partial class frmLogin : Form
    {
        //ResourceManager res_man;    // declare Resource manager to access to specific cultureinfo
        //CultureInfo cul;            //declare culture info

        LanguageService lang;
        public frmLogin(LanguageService _lang)
        {
            InitializeComponent();
            lblLoginInfo.Text = "";
            //res_man = obj_res_man;
            //cul = obj_cul;
            lang = _lang;
            switch_language();
        }
        private void switch_language()
        {

            this.Text = lang.getText("form_login_title");
            this.lblLogin.Text = lang.getText("form_login_title");
            this.lblLoginInfo.Text = lang.getText("form_login_info_wrong_data");
            this.lblPassword.Text = lang.getText("form_login_password");
            this.lblUsername.Text = lang.getText("form_login_user_name");
            this.btnLogin.Text = lang.getText("form_login_title");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.Length == 0 || txtUsername.Text.Length == 0)
            {
                lblLoginInfo.Text = "Please insert your username or password.";
            }
            user objUser = new user_repository().validateUser(txtUsername.Text,txtPassword.Text);
            if (objUser == null)
            {
                lblLoginInfo.Text = "Incorrect username or password.";
            }
            else
            {
                lblLoginInfo.Text = "Login successfully!";

                GlobalVar.loginUser = objUser;
                GlobalVar.isLogin = true;
                this.Close();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            frmRegister frm = new frmRegister(lang);
            frm.ShowDialog();
        }
    }
}
