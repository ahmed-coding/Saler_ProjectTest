using DevExpress.Utils;
using DevExpress.XtraEditors;
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
    public partial class frm_masterList : frm_master
    {
        public frm_masterList()
        {
            InitializeComponent();
            this.Load += Frm_masterList_Load;
        }

        private void Frm_masterList_Load(object sender, EventArgs e)
        {
            refreshData();  
            btnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.DoubleClick += GridView1_DoubleClick;
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea=e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if(info.InRow || info.InRowCell)
            {
                OpenForm(Convert.ToInt32(view.GetFocusedRowCellValue("id")));
            }
        }
        public virtual void OpenForm(int id)
        {


        }

        public override void Save()
        {

            
        }
        public override void Delete()
        {
        }
    }
}