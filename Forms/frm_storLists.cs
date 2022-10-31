using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Saler_Project.Forms
{
    public partial class frm_storLists : DevExpress.XtraEditors.XtraForm
    {
        public frm_storLists()
        {
            InitializeComponent();
        }

        private void frm_storLists_Load(object sender, EventArgs e)
        {
            refresh();
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns[0].Visible = false;
            gridView1.Columns[1].Caption = "الاسماء";
            gridView1.DoubleClick += GridView1_DoubleClick;
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            int id = 0;
            id =Convert.ToInt32(gridView1.GetFocusedRowCellValue("id"));

            frm_Stors frm = new frm_Stors(id);
            frm.Show();
        }
        void refresh()
        {
            Scr.DBDataContext db = new Scr.DBDataContext();
            gridControl1.DataSource = db.Stors;

        }
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            refresh();
        }

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm_Stors frm = new frm_Stors();
            frm.Show();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
} 