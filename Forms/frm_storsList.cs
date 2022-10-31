using DevExpress.XtraEditors;
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
    public partial class frm_storsList : DevExpress.XtraEditors.XtraForm
    {
        public frm_storsList()
        {
            InitializeComponent();
        }

        private void frm_storsList_Load(object sender, EventArgs e)
        {
            refresh();

            gridView1.DoubleClick += GridView1_DoubleClick;

        }
        void refresh()
        {
            Scr.DBDataContext dB = new Scr.DBDataContext();
            gridControl1.DataSource = dB.Stors;
            gridView1.Columns[0].Visible = false;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns[1].Caption = "الاسم";
        }
        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            int id = 0;
            id =Convert.ToInt32( gridView1.GetFocusedRowCellValue("id"));
            frm_stors frm = new frm_stors(id);

            frm.Show();
            refresh();
        }
    }
}