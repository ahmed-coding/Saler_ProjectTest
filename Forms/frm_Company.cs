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
    public partial class frm_Company : DevExpress.XtraEditors.XtraForm
    {
        public frm_Company()
        {
            InitializeComponent();
            this.Load += Frm_Company_Load;
        }

        private void Frm_Company_Load(object sender, EventArgs e)
        {

            getData();
            //company.name.ge
            
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            saveData();
            
        }

        private void frm_Company_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                saveData();
            }
        }
        void getData()
        {
            Scr.DBDataContext dbData = new Scr.DBDataContext();
            Scr.Company company = new Scr.Company();
            company = dbData.Companies.FirstOrDefault();
            if (company == null) return;
            txtName.Text = company.name;
            txtPhone.Text = company.phone;
            txtAddress.Text = company.address;
        }
        void saveData()
        {
                if (txtName.Text.Trim() == string.Empty || txtAddress.Text.Trim() == string.Empty || txtPhone.Text.Trim() == string.Empty)
                {
                    XtraMessageBox.Show("هناك حقل فارغ يرجئ تعبئة الحقول", "خطاء");
                    return;
                }
                Scr.DBDataContext dbData = new Scr.DBDataContext();
                Scr.Company company = dbData.Companies.FirstOrDefault();
            if (company == null) {
                company = new Scr.Company();

                dbData.Companies.InsertOnSubmit(company);
            }

                company.name = txtName.Text;
                company.phone = txtPhone.Text;
                company.address = txtAddress.Text;
                dbData.SubmitChanges();
                XtraMessageBox.Show("تم الحفظ بنجاح");
        }
    }
    
}