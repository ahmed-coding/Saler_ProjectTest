using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static Saler_Project.Classes.Master;
using Saler_Project.Classes;
using System.Linq;
using DevExpress.Data.ODataLinq.Helpers;
using DevExpress.Data.WcfLinq.Helpers;

namespace Saler_Project.Forms
{
    public partial class frm_invoice : Saler_Project.Forms.frm_master
    {
        Scr.Invoice_Header invoice;
        InvoiceType type;

        Scr.DBDataContext generalDB;
        public frm_invoice(InvoiceType _type)
        {
            type = _type;
            InitializeComponent();
            lkpPartType.EditValueChanged += LkpPartType_EditValueChanged;
            refreshData();
            New();
            
        }
        

        private void frm_invoice_Load(object sender, EventArgs e)
        {
            spnTotal.EditValue = 1000;
            

            lkpPartType.initData(PartTypeList);
            glkpPartID.ButtonClick += LkpPartType_ButtonClick;


            spnDiscontValue.Leave += SpnDiscontValue_Leave;
            spnDiscontValue.EditValueChanged += SpnDiscontValue_EditValueChanged;
            spnDiscontRation.EditValueChanged += SpnDiscontValue_EditValueChanged;


            spnTaxValue.Leave += SpnTaxValue_Leave; 
            spnTaxValue.Enter += SpnTaxValue_Enter;
            spnTaxValue.EditValueChanged += SpnTaxValue_EditValueChanged;
            spnTax.EditValueChanged += SpnTaxValue_EditValueChanged;



            spnTaxValue.EditValueChanged += Spn_EditValueChanged;
            spnDiscontValue.EditValueChanged += Spn_EditValueChanged;
            spnExpences.EditValueChanged += Spn_EditValueChanged;


            txtPaid.EditValueChanged += txt_Paid_EditValueChanged;
            spnNet.EditValueChanged += txt_Paid_EditValueChanged;

           
        }

        private void LkpPartType_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpPartType.isInt())
            {
                int pars = Convert.ToInt32(lkpPartType.EditValue);

                if (pars == (int)PartType.Customer)
                    glkpPartID.initData(Session.customer);
                else if (pars == (int)PartType.Vendor)
                    glkpPartID.initData(Session.vendors);


            }
        }

        private void Spn_EditValueChanged(object sender, EventArgs e)
        {

            var total = Convert.ToDouble(spnTotal.EditValue);
            var tax = Convert.ToDouble(spnTaxValue.EditValue);
            var discountVal = Convert.ToDouble(spnDiscontValue.EditValue);
            var expences= Convert.ToDouble(spnExpences.EditValue);

            spnNet.EditValue = total + tax - discountVal + expences;

        }
        public override void refreshData()
        {
            lkpBranch.initData(Session.stor);
            lkpDarwer.initData(Session.drawers);


            base.refreshData();
        }

        public override bool isDataValid()
        {
            int numError = 0;
            numError += txtCode.isTextValid() ? 0 : 1;
            numError += lkpPartType.isEditValid() ? 0 : 1;
            numError += lkpDarwer.isEditValid() ? 0 : 1;
            numError += lkpBranch.isEditValid() ? 0 : 1;
            numError += glkpPartID.isEditValidAndNotZero() ? 0 : 1;
            numError += (dtDate.isDateValid() ? 0 : 1);
            if(chkPOstToStor.Checked)
            {
                numError += dtPostIDdate.isDateValid() ? 0 : 1;
                layoutControlGroup2.Expanded = true;
            }
            
            return (numError == 0);
        }

        #region EditCalculations



        private void txt_Paid_EditValueChanged(object sender, EventArgs e)
        {

            var net = Convert.ToDouble(spnNet.EditValue);
            var paid = Convert.ToDouble(txtPaid.EditValue);


            spnRemaing.EditValue = net - paid;

        }
        private void SpnTaxValue_EditValueChanged(object sender, EventArgs e)
        {
            var total = Convert.ToDouble(spnTotal.EditValue);
            var val = Convert.ToDouble(spnTaxValue.EditValue);
            var ratio = Convert.ToDouble(spnTax.EditValue);


            if (isTexValueFoucsed)

                spnTax.EditValue = (val / total);

            else
                spnTaxValue.EditValue = total * ratio;

        }


        bool isTexValueFoucsed;
        private void SpnTaxValue_Enter(object sender, EventArgs e)
        {
            isTexValueFoucsed = true;
        }

        private void SpnTaxValue_Leave(object sender, EventArgs e)
        {
            isTexValueFoucsed = false;
        }

        private void SpnDiscontValue_EditValueChanged(object sender, EventArgs e)
        {
           var total= Convert.ToDouble(spnTotal.EditValue);
            var discountVal = Convert.ToDouble(spnDiscontValue.EditValue);
            var discountRation= Convert.ToDouble(spnDiscontRation.EditValue);


            if (isDiscontValueFoucsed)
            {
                spnDiscontRation.EditValue = (discountVal / total) ;


            }
            else
            {
                spnDiscontValue.EditValue = total * discountRation;

            }
        }

        private void LkpPartType_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if(e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
            {
                using (var frm = new frm_customerVendor(Convert.ToInt32(lkpPartType.EditValue) == (int)PartType.Customer))
                {

                    frm.ShowDialog();
                    refreshData();
                }
            }
           

        }

        Boolean isDiscontValueFoucsed;
        private void spnDiscontValue_Enter(object sender, EventArgs e)
        {

            isDiscontValueFoucsed = true;
        }
        private void SpnDiscontValue_Leave(object sender, EventArgs e)
        {
            isDiscontValueFoucsed = false;
        }
        #endregion
        public override void GetData()
        {
            lkpBranch.EditValue = invoice.branch;
            lkpDarwer.EditValue = invoice.darwer;
            lkpPartType.EditValue = invoice.partType;
            glkpPartID.EditValue = invoice.part_id;
            txtCode.Text = invoice.code;
            dtDate.DateTime = invoice.date;
            dt_DelivaryDate.EditValue = invoice.delivaryDate;
            dtPostIDdate.EditValue = invoice.postDate;
            mesSippingAddress.Text = invoice.shippingAddress;
            meNote.Text = invoice.notes;
            chkPOstToStor.Checked = invoice.postedToStore;
            spnDiscontRation.EditValue = invoice.discountRation;
            spnDiscontValue.EditValue = invoice.discountValue;
            spnExpences.EditValue = invoice.expences;
            spnTax.EditValue = invoice.tax;
            spnTaxValue.EditValue = invoice.taxValue;
            spnRemaing.EditValue = invoice.remaing;
            spnNet.EditValue= invoice.net;
            spnTotal.EditValue = invoice.total;
            txtPaid.Text = invoice.paid.ToString();


            generalDB = new Scr.DBDataContext();
            gridControl1.DataSource = generalDB.Invoice_details.Where(x => x.invoice_id == invoice.id);


            base.GetData();
        }
        public override void SetData()
        {
            invoice.branch = (int)Convert.ToUInt32( lkpBranch.EditValue);
            invoice.darwer = (int)Convert.ToUInt32(lkpDarwer.EditValue);
            invoice.partType = (byte)Convert.ToUInt32(lkpPartType.EditValue);
            invoice.part_id = (int)Convert.ToUInt32(glkpPartID.EditValue);
            invoice.code = txtCode.Text;
            invoice.date = dtDate.DateTime;
            invoice.delivaryDate = dt_DelivaryDate.EditValue as DateTime?;
            invoice.postDate = dtPostIDdate.EditValue as DateTime?;
            invoice.shippingAddress = mesSippingAddress.Text;
            invoice.notes = meNote.Text;
            invoice.postedToStore = chkPOstToStor.Checked;
            invoice.discountRation = Convert.ToDouble(spnDiscontRation.EditValue);
            invoice.discountValue = Convert.ToDouble(spnDiscontValue.EditValue);
            invoice.expences = Convert.ToDouble(spnExpences.EditValue);
            invoice.tax = Convert.ToDouble(spnTax.EditValue);
            invoice.taxValue = Convert.ToDouble(spnTaxValue.EditValue);
            invoice.remaing = Convert.ToDouble(spnRemaing.EditValue);
            invoice.net = Convert.ToDouble(spnNet.EditValue);
            invoice.total = Convert.ToDouble(spnTotal.EditValue);
            invoice.paid = Convert.ToDouble (txtPaid.Text);                 

            base.SetData();
        }
        public override void New()
        {
            invoice = new Scr.Invoice_Header()
            {
                darwer = Session.DefualtDrawer,
                date=DateTime.Now,
                postedToStore=true,
                postDate=DateTime.Now,
               
            };
            switch (type)
            {
                case InvoiceType.PurchaseReturn:
                case InvoiceType.Purchase:
                    invoice.partType = (byte) PartType.Vendor ;
                    invoice.part_id = Session.DefualtVendor;
                    invoice.branch = Session.DefualtRowStor;

                    break;


                case InvoiceType.SalesReturn:
                case InvoiceType.Sales:
                    invoice.partType = (byte)PartType.Customer;
                    invoice.part_id = Session.DefualCustomer;
                    invoice.branch = Session.DefualtStor;
                    break;


                default:
                    throw new NotImplementedException();
            }
            base.New();
        }
         
    }
}
