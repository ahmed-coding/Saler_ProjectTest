using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Saler_Project.Forms
{
    public partial class frm_Stors : DevExpress.XtraEditors.XtraForm
    {
        Scr.Stor stor;
        public frm_Stors()
        {
            InitializeComponent();
            New();
        }
        public frm_Stors(int id)
        {
            InitializeComponent();

            var db = new Scr.DBDataContext();
            stor = db.Stors.Where(st => st.id == id).First();
            get_data();

        }
        void save_data()
        {
            if (textEdit1.Text.Trim() == string.Empty)
            {
                XtraMessageBox.Show("يرجئ ادخال اسم الفرع", "خطاء");
                return;
            }
            var db = new Scr.DBDataContext();
            if (stor.id== 0)
                db.Stors.InsertOnSubmit(stor);
            else
                db.Stors.Attach(stor);
            set_data();
            db.SubmitChanges();
            XtraMessageBox.Show("تمت العملية بنجاح");
        }
        void get_data()
        {
            textEdit1.Text = stor.name;
        }
        void set_data()
        {

            stor.name = textEdit1.Text;
        }
        void New()
        {
            stor = new Scr.Stor();
            get_data();

        }
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            save_data();
        }

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            New();
            save_data();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Scr.DBDataContext db = new Scr.DBDataContext();


            if(XtraMessageBox.Show(caption:"تاكيد الحذف ",text:"هل تريد الحذف ",buttons:MessageBoxButtons.YesNo ,icon:MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                db.Stors.Attach(stor);
                db.Stors.DeleteOnSubmit(stor);
                db.SubmitChanges();
                XtraMessageBox.Show("تم الحذف بنجاح");
                this.Close();
            }


        }
    }
}