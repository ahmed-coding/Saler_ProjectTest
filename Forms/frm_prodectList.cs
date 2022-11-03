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
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using static Saler_Project.Classes.Master;


namespace Saler_Project.Forms
{
    public partial class frm_prodectList : Saler_Project.Forms.frm_master
    {
        public frm_prodectList()
        {
            InitializeComponent();
        }

        private void frm_prodectList_Load(object sender, EventArgs e)
        {
            refreshData();
            btnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            gridView1.CustomColumnDisplayText += GridView1_CustomColumnDisplayText;
            gridView1.OptionsBehavior.Editable = false;

            gridView1.DoubleClick += GridView1_DoubleClick;
            this.Text = "قائمة الاصناف";
            gridControl1.ViewRegistered += GridControl1_ViewRegistered;
            gridView1.OptionsDetail.ShowDetailTabs = false;
        }

        private void GridControl1_ViewRegistered(object sender, DevExpress.XtraGrid.ViewOperationEventArgs e)
        {

            if(e.View.LevelName == "UOM")
            {
                GridView view = e.View as GridView;
                view.OptionsView.ShowViewCaption = true;
                view.ViewCaption = "وحدات القياس ";
                view.Columns["UnitName"].Caption = "الوحدة";
                view.Columns["vactor"].Caption = "المعامل";
                view.Columns["sellPress"].Caption = "سعر البيع";
                view.Columns["buyPress"].Caption = "سعر الشرء";
                view.Columns["sellDiscount"].Caption = "الخصم";
                view.Columns["barrCode"].Caption = "الباركود";

            }
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            int id = 0;
            if(int.TryParse(gridView1.GetFocusedRowCellValue("id").ToString(),out id)&& id>0)
            {
                var frm = new frm_prodect(id);
                frm.ShowDialog();
                refreshData();
            }
        }

        private void GridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "type")
            {
                e.DisplayText = prodectTypeList.Single(x => x.id == Convert.ToInt32(e.Value)).name;
            }
        }
        public override void New()
        {
            var frm = new frm_prodect();
            frm.ShowDialog();
            refreshData();

            base.New();
        }

        public override void refreshData()
        {
            var frm = new frm_prodect();

            using (var db = new Scr.DBDataContext())
            {
               var data= from pr in db.Prodects 
                                          join cg in db.Prodect_Categories on pr.category_id equals cg.id
                                          select new {
                    pr.id,
                    pr.name,
                    pr.code,
                   CategoryName= cg.name,
                   pr.descreption,
                   pr.is_active,
                   pr.type,
                   UOM= db.Prodect_units.Where(x=> x.prodect_id ==pr.id).Select(u=> new
                   {
                       UnitName=db.Units.Single(un=> un.id == u.unit_id).name,
                       u.vactor,
                       u.sellPress,
                       u.buyPress,
                       u.sellDiscount,
                       u.barrCode,
                   }).ToList(),
                

                   
                } ;

                gridControl1.DataSource = data;
                var ins = data.FirstOrDefault();
                gridView1.Columns[nameof(ins.CategoryName)].Caption = "الفئة";
                gridView1.Columns[nameof(ins.code)].Caption = "الكود";
                gridView1.Columns[nameof(ins.is_active)].Caption = "نشط";
                gridView1.Columns[nameof(ins.descreption)].Caption = "الوصف";
                gridView1.Columns[nameof(ins.name)].Caption = "الاسم";
                gridView1.Columns[nameof(ins.type)].Caption = "النوع";
                gridView1.Columns[nameof(ins.id)].Visible = false;


            }

            base.refreshData();
        }
    }
}
