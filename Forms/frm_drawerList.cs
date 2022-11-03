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
using static Saler_Project.Classes.Master;

namespace Saler_Project.Forms
{
    public partial class frm_drawerList : Saler_Project.Forms.frm_master
    {
        public frm_drawerList()
        {
            InitializeComponent();
            refreshData();

        }

        private void frm_drawerList_Load(object sender, EventArgs e)
        {
            btnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            gridView1.Columns["id"].Visible = false;
            gridView1.Columns["name"].Caption = "الاسم";
            gridView1.OptionsBehavior.Editable = false;
            gridView1.DoubleClick += GridView1_DoubleClick;
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea= e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if(info.InRowCell || info.InRow)
            {
                var frm = new frm_drawer(Convert.ToInt32(view.GetFocusedRowCellValue("id")));
                frm.ShowDialog();
                refreshData();
            }

        }

        public override void New()
        {
            var frm = new frm_drawer();
            frm.ShowDialog();
            refreshData();
            base.New();
        }
        public override void refreshData()
        {
            using (var db =new Scr.DBDataContext())
            {
                gridControl1.DataSource = db.Drawers.ToList();



            }
            base.refreshData();
        }
    }
}
