using Liphsoft.Crypto.Argon2;
using Saler_Project.Classes;
using System;
using System.Data;
using System.Linq;
using static Saler_Project.Classes.Master;

namespace Saler_Project.Forms
{
    public partial class frm_user : Saler_Project.Forms.frm_master
    {
        Scr.User user;
        public frm_user()
        {
            InitializeComponent();
            refreshData();
            New();
        }
        public frm_user(int id)
        {
            InitializeComponent();
            using (var db = new Scr.DBDataContext())
            {
                user = db.Users.SingleOrDefault(x => x.id == id);
                refreshData();
                GetData();
            }


        }
        public override void refreshData()
        {
            using (var db = new Scr.DBDataContext())
            {
                //lkpScreenProfile_id.initData(db.us)
                lkpSettingsProfile_id.initData(db.UserSettingsProfiles.Select(x => new { x.id, x.name }).ToList());
                lkpUserType.initData(UserTypeList);
            }

            base.refreshData();
        }

        public override void New()
        {
            user = new Scr.User();
            user.isActive = true;
            base.New();
        }
        public override void GetData()
        {
            txtName.Text = user.name;
            txtUsername.Text = user.username;
            txtPassword.Text = user.password;

            lkpScreenProfile_id.EditValue = user.screenProfile_id;
            lkpSettingsProfile_id.EditValue=user.settingsProfile_id;
            lkpUserType.EditValue = user.userType;
            toggleSwitch1.IsOn = user.isActive;
            base.GetData();
        }

        public override void SetData()
        {
            if(user.password == txtPassword.Text)
            {
                var hasher = new PasswordHasher();
                string myhasher = hasher.Hash(txtPassword.Text);
                txtPassword.Text = myhasher ;
            }

            user.name = txtName.Text;
            user.username = txtUsername.Text;
            user.password = txtPassword.Text;
            user.userType= Convert.ToByte(lkpUserType.EditValue);
            user.screenProfile_id= Convert.ToInt32(lkpScreenProfile_id.EditValue);
            user.settingsProfile_id = Convert.ToInt32(lkpSettingsProfile_id.EditValue);
            user.isActive = toggleSwitch1.IsOn;

            base.SetData();
        }
        public override void Save()
        {
            using (Scr.DBDataContext db = new Scr.DBDataContext())
            {


                if (user.id == 0)
                {
                    db.Users.InsertOnSubmit(user);
                }
                else
                {
                    db.Users.Attach(user);
                }
                SetData();
                db.SubmitChanges();
            }
            base.Save();

        }
        public override bool isDataValid()
        {
            int numberErrors=0;

            using (Scr.DBDataContext db = new Scr.DBDataContext())
            {
                if(db.Users.Where(x=> x.username.Trim() == txtUsername.Text.Trim() && x.id != user.id).Count() >0)
                {
                    numberErrors += 1;
                    txtPassword.ErrorText = "هذا الاسم مستخدم بالفعل";
                }
                if (db.Users.Where(x => x.name.Trim() == txtName.Text.Trim() && x.id != user.id).Count() > 0)
                {
                    numberErrors += 1;
                    txtName.ErrorText = "هذا الاسم مستخدم بالفعل";
                }

            }
            numberErrors += txtName.isTextValid() ? 0 : 1;
            numberErrors += txtPassword.isTextValid() ? 0 : 1;
            numberErrors += txtUsername.isTextValid() ? 0 : 1;

            //numberErrors += lkpScreenProfile_id.isEditValidAndNotZero() ? 0 : 1;
            //numberErrors += lkpSettingsProfile_id.isEditValidAndNotZero() ? 0 : 1;
            //numberErrors += lkpUserType.isEditValidAndNotZero() ? 0 : 1;

           return (numberErrors == 0);
        }


    }

}
