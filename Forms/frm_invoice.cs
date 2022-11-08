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
using DevExpress.XtraEditors.Repository;
using System.Collections.ObjectModel; 
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;

namespace Saler_Project.Forms
{
    public partial class frm_invoice : Saler_Project.Forms.frm_master
    {


        public static frm_invoice Instance;

        Scr.Invoice_Header invoice;
        InvoiceType type;

        Scr.DBDataContext generalDB;

        RepositoryItemGridLookUpEdit repoItems;
        RepositoryItemGridLookUpEdit repoUMO;
        RepositoryItemGridLookUpEdit repoStor;
        public frm_invoice(InvoiceType _type)
        {
            type = _type;
            InitializeComponent();
            lkpPartType.EditValueChanged += LkpPartType_EditValueChanged;
            refreshData();
            New();
            
        }
        public frm_invoice(InvoiceType _type,int id)
        {
            type = _type;
            InitializeComponent();
            lkpPartType.EditValueChanged += LkpPartType_EditValueChanged;
            refreshData();
            using (var db=new Scr.DBDataContext())
            {
                invoice= db.Invoice_Headers.SingleOrDefault(x=> x.id ==id);
                GetData();
                isNew=false;
                
            }

        }
        public override void Refresh()
        {
            base.Refresh();
        }



        private void frm_invoice_Load(object sender, EventArgs e)
        {
            switch (type)
            {
                case InvoiceType.Purchase:
                    this.Text="فاتورة المشتريات";
                    break;
                case InvoiceType.Sales:
                    break;
                case InvoiceType.PurchaseReturn:
                    break;
                case InvoiceType.SalesReturn:
                    break;
                default:
                    break;
            }

            spnTotal.EditValue = 1000;
            lkpPartType.initData(PartTypeList);
            glkpPartID.ButtonClick += LkpPartType_ButtonClick;

            gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            var datailsIns = new Scr.Invoice_detail();
            gridView1.Columns[nameof(datailsIns.id)].Visible = false;
            gridView1.Columns[nameof(datailsIns.invoice_id)].Visible = false;

           

                //var prodect = Session.ProdectView;
                //repoItems.DataSource = prodect;
                //repoItems.DisplayMember = "name";
                //repoItems.ValueMember = "id";
                //gridControl1.RepositoryItems.Add(repoItems);

                gridView1.Columns[nameof(datailsIns.item_id)].ColumnEdit = repoItems;
            Session session = new Session();
            using ( var db =new Scr.DBDataContext())
            {
                repoItems.initData(session.ProdectView, gridView1.Columns[nameof(datailsIns.item_id)], gridControl1);
                //repoUMO.initData(db.Invoice_details, gridView1.Columns[nameof(datailsIns.itemUnit_id)], gridControl1);
                repoUMO = new RepositoryItemGridLookUpEdit();
                var prodectUnit = db.Prodect_units;
                repoUMO.DataSource = prodectUnit;
                repoUMO.DisplayMember = "name";
                repoUMO.ValueMember = "id";
                gridControl1.RepositoryItems.Add(repoUMO);
                gridView1.Columns[nameof(datailsIns.itemUnit_id)].ColumnEdit = repoUMO;
            }

            




            RepositoryItemSpinEdit spinEdit = new RepositoryItemSpinEdit();
            gridView1.Columns[nameof(datailsIns.totalPrice)].ColumnEdit = spinEdit;
            gridView1.Columns[nameof(datailsIns.price)].ColumnEdit = spinEdit;
            gridView1.Columns[nameof(datailsIns.itemQty)].ColumnEdit = spinEdit;
            gridView1.Columns[nameof(datailsIns.discountValue)].ColumnEdit = spinEdit;


            RepositoryItemSpinEdit spinRatioEdit = new RepositoryItemSpinEdit();
            gridView1.Columns[nameof(datailsIns.discount)].ColumnEdit = spinEdit;


            gridControl1.RepositoryItems.Add(spinEdit);
            gridControl1.RepositoryItems.Add(spinRatioEdit);

            gridView1.Columns[nameof(datailsIns.totalPrice)].OptionsColumn.AllowFocus = false;

            spinRatioEdit.Increment = 0.01m;
            spinRatioEdit.Mask.EditMask = "p";
            spinRatioEdit.Mask.UseMaskAsDisplayFormat = true;
            spinRatioEdit.MaxValue = 1;

            gridView1.Columns.Add(new GridColumn() { VisibleIndex = 0, Name = "clmCode", Caption = " الكود", FieldName = "Code", UnboundType = DevExpress.Data.UnboundColumnType.String });
            gridView1.Columns.Add(new GridColumn() { VisibleIndex = 0, Name = "clmIndex", Caption = " مسلسل", FieldName = "index", UnboundType = DevExpress.Data.UnboundColumnType.Integer });


            gridView1.Columns[nameof(datailsIns.item_id)].Caption = "الصنف";
            gridView1.Columns[nameof(datailsIns.costValue)].Caption = "سعر التكلفة";
            gridView1.Columns[nameof(datailsIns.discount)].Caption = "ن.خصم";
            gridView1.Columns[nameof(datailsIns.discountValue)].Caption = "ق.خصم";
            gridView1.Columns[nameof(datailsIns.itemQty)].Caption = "الكمية";
            gridView1.Columns[nameof(datailsIns.itemUnit_id)].Caption = "الوحدة";
            gridView1.Columns[nameof(datailsIns.price)].Caption = "السعر";
            gridView1.Columns[nameof(datailsIns.totalPrice)].Caption = "المخزن";
            gridView1.Columns[nameof(datailsIns.store_id)].Caption = "اجمالي التكلفة";
            gridView1.Columns[nameof(datailsIns.totalCostValue)].Caption = "اجمالي السعر";


            gridView1.Columns["index"].VisibleIndex = 0;
            gridView1.Columns["Code"].VisibleIndex = 1;

            gridView1.Columns[nameof(datailsIns.item_id)].VisibleIndex =2;
            gridView1.Columns[nameof(datailsIns.itemUnit_id)].VisibleIndex = 3;
            gridView1.Columns[nameof(datailsIns.itemQty)].VisibleIndex = 4;
            gridView1.Columns[nameof(datailsIns.price)].VisibleIndex = 5;
            gridView1.Columns[nameof(datailsIns.discount)].VisibleIndex = 6;
            gridView1.Columns[nameof(datailsIns.discountValue)].VisibleIndex = 7;
            gridView1.Columns[nameof(datailsIns.totalPrice)].VisibleIndex =8;
            gridView1.Columns[nameof(datailsIns.costValue)].VisibleIndex = 9;
            gridView1.Columns[nameof(datailsIns.totalCostValue)].VisibleIndex =10;

            gridView1.Columns[nameof(datailsIns.store_id)].VisibleIndex =11;




            #region Events

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
            spnNet.EditValueChanged += txt_Paid_EditValueChanged;
            spnNet.EditValueChanging += SpnNet_EditValueChanging;
            spnTotal.EditValueChanged += txt_Paid_EditValueChanged;
            spnPaid.EditValueChanged += txt_Paid_EditValueChanged;
            spnNet.DoubleClick += SpnNet_DoubleClick;

            gridView1.CustomRowCellEditForEditing += GridView1_CustomRowCellEditForEditing;
            gridView1.CellValueChanged += GridView1_CellValueChanged;
            gridView1.RowCountChanged += GridView1_RowCountChanged;
            gridView1.RowUpdated += GridView1_RowUpdated;
            #endregion
            moveFocuseToGrid();
        }

        private void SpnNet_DoubleClick(object sender, EventArgs e)
        {
            spnPaid.EditValue = spnNet.EditValue;
        }

        private void SpnNet_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (Convert.ToDouble( e.OldValue) == Convert.ToDouble(spnPaid.EditValue))
                spnPaid.EditValue = e.NewValue;
        }

        private void GridView1_RowUpdated(object sender, RowObjectEventArgs e)
        {
            var items = gridView1.DataSource as Collection<Scr.Invoice_detail>;
            if (items == null)
                spnTotal.EditValue = 0;
            else
            {
                spnTotal.EditValue = items.Sum(x=> x.totalPrice);
            }
        }

        private void GridView1_RowCountChanged(object sender, EventArgs e)
        {
            GridView1_RowUpdated(sender, null);
        }

        private void GridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

                var se = new Session();
                var unit = new Scr.Invoice_detail();
                var row = gridView1.GetRow(e.RowHandle) as Scr.Invoice_detail;
                var itemv = se.ProdectView.SingleOrDefault(x => x.id == row.itemUnit_id);
                if (itemv == null) return;
                if (row.itemUnit_id == 0)
                {
                    row.itemUnit_id = itemv.Units.FirstOrDefault().unit_id;
                    GridView1_CellValueChanged(sender, new CellValueChangedEventArgs(e.RowHandle, gridView1.Columns[nameof(unit.itemUnit_id)], row.itemUnit_id));
                }

                var unitv = itemv.Units.SingleOrDefault(x => x.unit_id == row.itemUnit_id);
                if (unitv == null) return;
                if (row == null) return;

                switch (e.Column.FieldName)
                {
                case nameof(unit.item_id):
                    if (row.store_id == 0 && lkpBranch.isEditValidAndNotZero())
                        row.store_id = Convert.ToInt32(lkpBranch.EditValue);
                    break;
                case nameof(unit.itemUnit_id):
                        if (type == InvoiceType.Purchase || type == InvoiceType.PurchaseReturn)
                            row.price = unitv.buyPress;
                        if (row.itemQty == 0)
                            row.itemQty = 1;
                        GridView1_CellValueChanged(sender, new CellValueChangedEventArgs(e.RowHandle, gridView1.Columns[nameof(unit.price)], row.price));
                        break;
                    case nameof(unit.price):
                    case nameof(unit.discount):
                    case nameof(unit.itemQty):
                        row.discountValue = row.discount * (row.itemQty * row.price);
                        GridView1_CellValueChanged(sender, new CellValueChangedEventArgs(e.RowHandle, gridView1.Columns[nameof(unit.discountValue)], row.discountValue));
                        break;

                    case nameof(unit.discountValue):
                        if (gridView1.FocusedColumn.FieldName == nameof(unit.discountValue))
                            row.discount = row.discountValue / (row.itemQty * row.price);
                        row.totalPrice = (row.itemQty * row.price) - row.discountValue;
                        break;

                    default:
                        break;
                }
            
            

        }

        private void GridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            var unit = new Scr.Invoice_detail();
            if (e.Column.FieldName== nameof(unit.itemUnit_id))
            {
                RepositoryItemLookUpEdit repo = new RepositoryItemLookUpEdit();
                    var row = gridView1.GetRow(e.RowHandle) as Scr.Invoice_detail;
                    if (row == null) return;
                using (var db = new Scr.DBDataContext())
                {
                    var item = db.Prodects.SingleOrDefault(x => x.id == row.item_id);
                    repo.DataSource = item;
                    repo.ValueMember = "unit_id";
                    repo.DisplayMember = "id";
                    repo.NullText = "";
                }

                    e.RepositoryItem = repo;
                
                

            }
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
            var paid = Convert.ToDouble(spnPaid.EditValue);
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
            spnPaid.EditValue = invoice.paid;


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
            invoice.paid = Convert.ToDouble (spnPaid.EditValue);                 

            base.SetData();
        }
        string getNewInvoiceBarrCode()
        {
            string maxCode;
            using (Scr.DBDataContext db = new Scr.DBDataContext())
            {
                maxCode = db.Invoice_Headers.Where(x=> x.invoiceType ==(int)type).Select(x => x.code).Max();
            }

            return getNextCode(maxCode);

        }
        public override void New()
        {
            invoice = new Scr.Invoice_Header()
            {
                darwer = Session.DefualtDrawer,
                date = DateTime.Now,
                postedToStore = true,
                postDate = DateTime.Now,
                code = getNewInvoiceBarrCode()
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
            moveFocuseToGrid();
            base.New();
        }
        void moveFocuseToGrid()
        {
            gridView1.Focus();
            gridView1.FocusedColumn = gridView1.Columns["code"];
            gridView1.AddNewRow();
        }
         
    }
}
