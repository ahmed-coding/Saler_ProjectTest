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
    public partial class frm_company : DevExpress.XtraEditors.XtraForm
    {


        public frm_company()
        {
            InitializeComponent();
            this.Load += Frm_company_Load;
        }

        private void Frm_company_Load(object sender, EventArgs e)
        {
            Scr.DBDataContext db;
            Scr.Company company;
            db = new Scr.DBDataContext();
            company = db.Companies.FirstOrDefault();
            if (company == null) return;

            txtAddress.Text = company.address;
            txtName.Text = company.name;
            txtPhone.Text = company.phone;

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            save();

        }

        private void frm_company_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                save();
            }
        }
        void save()
        {
            Scr.DBDataContext db = new Scr.DBDataContext();

            Scr.Company company = db.Companies.FirstOrDefault();
            if (company == null)
            {
                company = new Scr.Company();
                db.Companies.InsertOnSubmit(company);
            }
            if (txtName.Text.Trim() == String.Empty)
            {
                txtName.ErrorText = "يرجاء ادخال اسم الشركة";
                return;
            }
            else if (txtAddress.Text.Trim() == String.Empty)
            {
                txtAddress.ErrorText = "يرجاء ادخال عنوان الشركة";
                return;
            }
            else if (txtPhone.Text.Trim() == String.Empty)
            {
                txtPhone.ErrorText = "يرجاء ادخال هاتف الشركة";
                return;
            }
            company.name = txtName.Text;
            company.phone = txtPhone.Text;
            company.address = txtAddress.Text;

            db.Companies.InsertOnSubmit(company);
            db.SubmitChanges();
            XtraMessageBox.Show(text: "تم الحفظ بنجاح");
        }
    }
}
