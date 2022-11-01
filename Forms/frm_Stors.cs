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
    public partial class frm_stors : DevExpress.XtraEditors.XtraForm
    {
        Scr.Stor stor;
        public frm_stors()
        {
            InitializeComponent();
            New();
        }
        public frm_stors(int id)
        {
            InitializeComponent();

            Scr.DBDataContext dB = new Scr.DBDataContext();
            stor = dB.Stors.Where(s => s.id == id).First();
            GetData();
        }

        void save()
        {
            if (txtName.Text.Trim() == String.Empty)
            {
                txtName.ErrorText = "يرجاء ادخال اسم المخزن"; 
                return;
            }
            Scr.DBDataContext dB = new Scr.DBDataContext();
            if (stor.id == 0)
                dB.Stors.InsertOnSubmit(stor);
            else
                dB.Stors.Attach(stor);

                set();
            dB.SubmitChanges();
            XtraMessageBox.Show(text: "تم الحفظ بنجاح");
        }
        void set()
        {
            stor.name = txtName.Text;
        }
        void GetData()
        {
            txtName.Text = stor.name;
        }
        void New()
        {
            stor = new Scr.Stor();
            GetData();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            save();
        }

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            New();

        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Scr.DBDataContext dB = new Scr.DBDataContext();
            DialogResult result = XtraMessageBox.Show(text: "هل تريد الحذف ؟",caption:"تحذير !" ,buttons:MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                dB.Stors.Attach(stor);
                dB.Stors.DeleteOnSubmit(stor);
                dB.SubmitChanges();

                XtraMessageBox.Show(text: "تم الحذف بنجاح");
            }

        }
    }
}