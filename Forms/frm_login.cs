using DevExpress.XtraEditors; 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Liphsoft.Crypto.Argon2;
using Liphsoft.Crypto;
using DevExpress.XtraSplashScreen;

namespace Saler_Project.Forms
{
    public partial class frm_login : DevExpress.XtraEditors.XtraForm
    {
        public bool isLogin { get; set; }
        public frm_login()
        {
            InitializeComponent();
            isLogin = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            using(var db = new Scr.DBDataContext())
            {



                var username=txtName.Text;
                string password=txtPassword.Text;

                var user=db.Users.SingleOrDefault(x=> x.username == username );
                if (user == null)
                {
                    XtraMessageBox.Show(text: "اسم المستخدم او كلمة المرور غير صحيحة",caption:"", icon: MessageBoxIcon.Error, buttons: MessageBoxButtons.OK);
                }
                else
                {
                    if(user.isActive == false)
                    {
                        XtraMessageBox.Show(text: "هذا الحساب تم تعطيلة !", caption: "", icon: MessageBoxIcon.Exclamation, buttons: MessageBoxButtons.OK);
                    }
                    var passwordhash = user.password;
                    PasswordHasher passwordHasher = new PasswordHasher();
                    if (password== user.password)
                    {
                        isLogin = true;
                        this.Hide();
                        //Splash Start
                        SplashScreenManager.ShowForm(parentForm:frm_main.Instance,typeof(StartSplash));
                        System.Threading.Thread.Sleep(5000);
                        frm_main.Instance.Show();
                        this.Close();
                        SplashScreenManager.CloseForm();
                        //End Splash Start
                        return;
                    }
                    else
                        XtraMessageBox.Show(text: "اسم المستخدم او كلمة المرور غير صحيحة", caption: "", icon: MessageBoxIcon.Error, buttons: MessageBoxButtons.OK);

                }
            }
        }
    }
}