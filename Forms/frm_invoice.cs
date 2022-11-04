using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static Saler_Project.Classes.Master;

namespace Saler_Project.Forms
{
    public partial class frm_invoice : Saler_Project.Forms.frm_master
    {
        Scr.Invoice_Header invoice;
        public frm_invoice()
        {

            InitializeComponent();
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
    }
}
