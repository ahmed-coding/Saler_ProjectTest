using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Saler_Project.Forms
{
    public partial class frm_prodectCategory : Saler_Project.Forms.frm_master
    {
        Scr.Prodect_Category prodect;
        public frm_prodectCategory()
        {
            InitializeComponent();
            New();
            refreshData();
        }
        public override void New()
        {
            prodect = new Scr.Prodect_Category();
            base.New();
        }
        public override void GetData()
        {
            txtName.Text = prodect.name;
            lookUp.EditValue = prodect.parent_id;
            base.GetData();
        }
        public override void SetData()
        {
            prodect.name = txtName.Text;
            prodect.parent_id = (lookUp.EditValue as int?) ?? 0;
            prodect.number = "0";
            base.SetData();
        }
        bool ValidataData()
        {

            if (txtName.Text.Trim() == String.Empty)
            {
                txtName.ErrorText = "هذا الحقل مطلوب";
                return false;
            }
           
            Scr.DBDataContext db = new Scr.DBDataContext();
            

            var obj = db.Prodect_Categories.Where(x => x.name.Trim() == txtName.Text.Trim() && x.id != prodect.id);
            if (obj.Count() > 0)
            {
                txtName.ErrorText = "هذا الاسم موجود مسبقآ";
                return false;
            }


            return true;
        }
        public override void Save()
        {
            if (ValidataData() == false)
                return;

            Scr.DBDataContext db = new Scr.DBDataContext();
            

            if (prodect.id == 0)
            {
                db.Prodect_Categories.InsertOnSubmit(prodect);
            }

            else
            {
                db.Prodect_Categories.Attach(prodect);
                
            }


            SetData();
            db.SubmitChanges();
           
            base.Save();
            this.Load += Frm_prodectCategory_Load;
        }

        private void Frm_prodectCategory_Load(object sender, EventArgs e)
        {
            refreshData();

        }

        private void TreeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            int id = 0;
            if (int.TryParse(e.Node.GetValue("id").ToString(),out id))
            {
                var db = new Scr.DBDataContext();
                prodect = db.Prodect_Categories.Single(x => x.id == id);
                GetData();

            }
        }

        public override void refreshData()
        {
            var db = new Scr.DBDataContext();
            var group = db.Prodect_Categories;
            lookUp.Properties.DataSource = group;

            treeList1.DataSource = group;
            base.refreshData();
            lookUp.Properties.DisplayMember = "name";
            lookUp.Properties.ValueMember = "id";
            treeList1.ParentFieldName = "parent_id";
            treeList1.KeyFieldName = "id";

            treeList1.OptionsBehavior.Editable = false;
            treeList1.Columns[nameof(prodect.number)].Visible = false;
            treeList1.Columns[nameof(prodect.name)].Caption = "الاسم";
            treeList1.FocusedNodeChanged += TreeList1_FocusedNodeChanged;
        }
    }
}
