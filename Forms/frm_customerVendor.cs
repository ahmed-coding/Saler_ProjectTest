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
        private bool isCustomer;

        public bool IsCoustomer
        {
            get { return isCustomer; }
            set { isCustomer = value; }
        }

        Scr.Customer customer;

        public frm_customerVendor()
        {
            InitializeComponent();
        }
        public frm_customerVendor(bool isCustomer)
        {
            InitializeComponent();
            IsCoustomer = this.isCustomer;
        }

        private void frm_customerVendor_Load(object sender, EventArgs e)
        {
            this.Text = (IsCoustomer) ? "عميل" : "مورد";
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
        public override void Save()
        {
            if(txtName.Text.Trim() == String.Empty)
            {
                txtName.ErrorText = "هذا الحقل مطلوب";
                return;
            }else if (txtAddress.Text.Trim() == string.Empty)
            {
                txtAddress.ErrorText = "هذا الحقل مطلوب";
                return;
            }
            else if (txtPhone.Text.Trim()== string.Empty)
            {
                txtPhone.ErrorText = "هذا الحقل مطلوب";
                return ;
            }

            base.Save();
        }
        public override void SetData()
        {
            customer.name = txtName.Text;
            customer.phone = txtPhone.Text;
            customer.address = txtAddress.Text;
            
            base.SetData();
        }

    }
}
