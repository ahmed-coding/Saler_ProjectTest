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
    public partial class frm_master : DevExpress.XtraEditors.XtraForm
    {
        public string errorText = "هذا الحقل مطلوب";
        public frm_master()
        {
            InitializeComponent();
        }

       virtual public  void New()
        {
            GetData();
        }
        virtual public  void Save()
        {
            XtraMessageBox.Show(text: "تم الحفظ بنجاح", caption: "تمت العملية", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
            refreshData();
        }
        virtual public void Delete()
        {

        }
        virtual public void GetData()
        {

        }
        virtual public void SetData()
        {

        }
        virtual public void refreshData()
        {

        }
        virtual public bool isDataValid()
        {
            return true;
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (isDataValid())
                //btnSave.PerformClick();
                Save();

            else return;
        }

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            New();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Delete();

        }

        private void frm_master_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                Save();
            }
           else if (e.KeyCode == Keys.F2)
            {
                New();
            }
           else if (e.KeyCode == Keys.F3)
            {
                Delete();
            }
        }
    }
}