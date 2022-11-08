using DevExpress.XtraEditors;
using Saler_Project.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Saler_Project.Classes.Master;

namespace Saler_Project.Forms
{
    public partial class frm_invoiceList : frm_masterList
    {
        public Master.InvoiceType type;
       
        public frm_invoiceList(Master.InvoiceType _Type)
        {
            InitializeComponent();
            type = _Type;
           
            this.Load += Frm_invoiceList_Load;
        }
        
        public override void OpenForm(int id)
        {
            frm_invoice.Instance = new frm_invoice(type,id);
        }

        public override void refreshData()
        {
            using (var db = new Scr.DBDataContext())
            {
                var query = from inv in db.Invoice_Headers.Where(x=> x.invoiceType ==(byte) type)
                            from prt in db.Customers.Where(x => x.id == inv.part_id).DefaultIfEmpty()
                            from br in db.Stors.Where(x => x.id == inv.branch).DefaultIfEmpty()
                            from drw in db.Drawers.Where(x => x.id == inv.darwer).DefaultIfEmpty()
                            select new
                            {
                                inv.id,
                                inv.code,
                                inv.date,
                                partName = prt.name,
                                branch = br.name,
                                itemCount = db.Invoice_details.Where(x => x.invoice_id == inv.id).Count(),
                                inv.postedToStore,
                                inv.total,
                                inv.discountValue,
                                inv.discountRation,
                                inv.tax,
                                inv.taxValue,
                                inv.expences,
                                inv.net,
                                inv.paid,
                                inv.remaing,
                                payStaus = "",
                                prodect = (from itm in db.Invoice_details.Where(x => x.invoice_id == inv.id)
                                           from pro in db.Prodects.Where(x => x.id == itm.item_id).DefaultIfEmpty()
                                           from unt in db.Units.Where(x => x.id == itm.itemUnit_id).DefaultIfEmpty()
                                           select new
                                           {
                                               prodect = pro.name,
                                               unit = unt.name,
                                               itm.price,
                                               itm.itemQty,
                                               itm.discountValue,
                                               itm.totalPrice

                                           }
                                         ).ToList()
                            };

                gridControl1.DataSource = query;
            }
            
            base.refreshData();
        }

        private void Frm_invoiceList_Load(object sender, EventArgs e)
        {
            switch (type)
            {
                case InvoiceType.Purchase:
                    this.Text = "فواتير المشتريات";
                    break;
                    
                case InvoiceType.Sales:
                    this.Text = "فواتير مبيعات";
                    break;
                //case InvoiceType.PurchaseReturn:
                //    this.Text = " مرتجع فاتورة المشتريات";
                //    break;
                //case InvoiceType.SalesReturn:
                //    this.Text = " مرتجع فاتورة مبيعات";
                //    break;
                default:
                    break;
            }
        }
    }
}