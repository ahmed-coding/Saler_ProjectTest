using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static Saler_Project.Classes.Master;
using Saler_Project.Classes;

namespace Saler_Project.Forms
{
    public partial class frm_user : Saler_Project.Forms.frm_master
    {
        Scr.User user;
        public frm_user()
        {
            InitializeComponent();
            New();
        }
        public frm_user(int id)
        {
            InitializeComponent();
            using (var db = new Scr.DBDataContext())
            {
                user = db.Users.SingleOrDefault(x => x.id == id);
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



    }     
    
}
