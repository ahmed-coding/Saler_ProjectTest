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

namespace Saler_Project.Forms
{    
    public partial class frm_drawer : frm_master
    {
        Scr.Drawer drawer;
        public frm_drawer()
        {
            InitializeComponent();
            New();
        }
        public frm_drawer(int id)
        {
            InitializeComponent();
            loadObject(id);
        }
        void loadObject(int id)
        {
            using (var db = new Scr.DBDataContext()) 
            {
                drawer = db.Drawers.Single(x => x.id == id);
                GetData();

            }
        }
        private void frm_drawer_Load(object sender, EventArgs e)
        {

        }

        public override void New()
        {
            // XtraMessageBox.Show
            drawer = new Scr.Drawer();
            base.New();
        }
        public override void Delete()
        {
            base.Delete();
        }
        public override void Save()
        {
            if (txtName.Text.Trim() == String.Empty)
            {
                txtName.ErrorText = "يرجاء ادخال اسم الخزنة";
                return;
            }
            Scr.DBDataContext dB = new Scr.DBDataContext();
            Scr.Account account ;

            if (drawer.id == 0)
            {
                account = new Scr.Account();
                dB.Drawers.InsertOnSubmit(drawer);
                dB.Accounts.InsertOnSubmit(account);
            }
            else
            {
                dB.Drawers.Attach(drawer);
                account = dB.Accounts.Single(a => a.id == drawer.account_id);
            }

            SetData();
            account.name = drawer.name;
            dB.SubmitChanges();

            drawer.account_id = account.id;
            dB.SubmitChanges();
            base.Save();
        }
        public override void GetData()
        {
            txtName.Text = drawer.name;
            base.GetData(); 
        }
        public override void SetData()
        {
            drawer.name = txtName.Text;
            base.SetData();
        }
    }
}