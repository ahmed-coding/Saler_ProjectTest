using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Saler_Project.Classes;
using static Saler_Project.Classes.Master;
namespace Saler_Project.Forms
{
    public partial class frm_customerVendorList : Saler_Project.Forms.frm_master
    {
        bool isCustomer;
      
        public frm_customerVendorList(bool sCustomer)
        {
            InitializeComponent();
            this.isCustomer = sCustomer;
            this.Text = isCustomer ? "قائمة العملاء" : "قائمة الموردين";
            refreshData();
            this.Load += Frm_customerVendorList_Load;    
        }

            private void Frm_customerVendorList_Load(object sender, EventArgs e)
        {
        
            gridView1.Columns["id"].Visible = false;
            gridView1.Columns["name"].Caption = "الاسم";
            gridView1.Columns["phone"].Caption = "الهاتف";
            gridView1.Columns["address"].Caption = "العنوان";
            gridView1.Columns["account_id"].Caption = "رقم الحساب";
            gridView1.Columns["isCustomer"].Visible = false;

            gridView1.OptionsBehavior.Editable = false;
            gridView1.DoubleClick += GridView1_DoubleClick;
            Session.vendors.ListChanged += Vendors_ListChanged;
        }
        

        private void Vendors_ListChanged(object sender, ListChangedEventArgs e)
        {
            refreshData();
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRowCell || info.InRow)
            {
                var frm = new frm_customerVendor(Convert.ToInt32(view.GetFocusedRowCellValue("id")));
                frm.ShowDialog();
                refreshData();
            }
        }

        public override void refreshData()
        {
            using (var db = new Scr.DBDataContext())
            {
                gridControl1.DataSource =db.Customers.Where(x=> x.isCustomer == isCustomer).ToList();


            }
            base.refreshData();
        }



    }
}
