using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
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
    public partial class frm_userList : frm_master
    {
        public frm_userList()
        {
            InitializeComponent();
            gridView1.OptionsBehavior.Editable = false;
            gridView1.DoubleClick += GridView1_DoubleClick;
            refreshData();
            gridView1.Columns["id"].Visible = false;
            btnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        public override void refreshData()
        {
            using (var db = new Scr.DBDataContext())
            {
                gridControl1.DataSource = db.Users.Select(x=> new {x.id,x.name , x.isActive}).ToList();
            }
            base.refreshData();
        }
        public override void New()
        { frm_main.openForm(nameof(frm_user));
            base.New();
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                var frm=new frm_user( Convert.ToInt32(view.GetFocusedRowCellValue("id")));
                frm.ShowDialog();
                refreshData();
            }
        }
    }
}
