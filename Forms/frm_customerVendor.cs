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
    public partial class frm_customerVendor : frm_master
    {
         bool IsCustomer;


        Scr.Customer customer;

        public frm_customerVendor(bool isCustomer)
        {
            InitializeComponent();
             this.IsCustomer =isCustomer;
            New();
        }

        private void frm_customerVendor_Load(object sender, EventArgs e)
        {
            this.Text = (IsCustomer) ? "عميل" : "مورد";
        }
        public override void New()
        {
            customer = new Scr.Customer();
            base.New();
        }
        public override void GetData()
        {
            txtName.Text = customer.name;
            txtPhone.Text = customer.phone;
            txtAcount.Text = customer.account_id.ToString();
            txtAddress.Text = customer.address;
            base.GetData();
        }
        bool ValidataData()
        {

            if (txtName.Text.Trim() == String.Empty)
            {
                txtName.ErrorText = "هذا الحقل مطلوب";
                return false;
            }
            else if (txtAddress.Text.Trim() == string.Empty)
            {
                txtAddress.ErrorText = "هذا الحقل مطلوب";
                return false;
            }
            else if (txtPhone.Text.Trim() == string.Empty)
            {
                txtPhone.ErrorText = "هذا الحقل مطلوب";
                return false;
            }
            Scr.DBDataContext db = new Scr.DBDataContext();
            
            var obj = db.Customers.Where(x => x.name.Trim() == txtName.Text.Trim() && x.isCustomer == this.IsCustomer && x.id != customer.id);
            if (obj.Count() > 0)
            {
                txtName.ErrorText = "هذا الاسم موجود مسبقآ";
                return false;
            }

            
            return true;
        }


        public override void Save()
        {


            if (ValidataData() == false)    
                return;

            Scr.DBDataContext db = new Scr.DBDataContext();
            Scr.Account account;

            if (customer.id == 0)
            {
                db.Customers.InsertOnSubmit(customer);
                account = new Scr.Account();
                db.Accounts.InsertOnSubmit(account);
            }

            else
            {
                db.Customers.Attach(customer);
                account = db.Accounts.Single(a => a.id == customer.account_id);
            }


            SetData();
            account.name = customer.name;
            db.SubmitChanges();
            customer.account_id = account.id;
            db.SubmitChanges();
            base.Save();
        }
        public override void SetData()
        {
            customer.name = txtName.Text;
            customer.phone = txtPhone.Text;
            customer.address = txtAddress.Text;
            customer.isCustomer = IsCustomer;
            
            base.SetData();
        }
    }
}
