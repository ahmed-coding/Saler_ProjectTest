using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Saler_Project.Forms
{
    public partial class frm_main : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        public frm_main()
        {
            InitializeComponent();
            accordionControl1.ElementClick += AccordionControl1_ElementClick;
        }

        private void AccordionControl1_ElementClick(object sender, DevExpress.XtraBars.Navigation.ElementClickEventArgs e)
        {
            var tag = e.Element.Tag as string;
            if(tag != string.Empty)
            {
                openForm(tag);
            }
        }

        public static void openForm(string name)
        {
            Form frm = null;
            switch (name)
            {
                case "frm_customer":
                    frm = new frm_customerVendor(true);
                    frm.Show();
                    break;
                case "frm_vendor":
                    frm = new frm_customerVendor(false);
                    frm.Show();
                    break;
                case "frm_vendorList":
                    frm = new frm_customerVendorList(false);
                    frm.Show();
                    break;
                case "frm_customerList":
                    frm = new frm_customerVendorList(true);
                    frm.Show();
                    break;
                case "frm_PurchaseInvoice":
                    frm = new frm_invoice(Classes.Master.InvoiceType.Purchase);
                    frm.Show();
                    break;
                default:

                    var ins = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.Name == name);

                    if (ins != null)
                    {

                        frm = Activator.CreateInstance(ins) as Form;
                        if (Application.OpenForms[frm.Name] != null)
                        {
                            frm = Application.OpenForms[frm.Name];
                        }
                        else
                            frm.Show();

                        frm.BringToFront();
                    }
                        break;
            }

            if (frm != null)
                frm.Show();

            
        }

        private void accordionControlElement1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
