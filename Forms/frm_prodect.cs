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

namespace Saler_Project.Forms
{
   
    public partial class frm_prodect : Saler_Project.Forms.frm_master
    {
       public enum ProdectType
        {
            Inventory,
            Servise,
        }
        Scr.Prodect prodect;
        RepositoryItemLookUpEdit lookUpEdit = new RepositoryItemLookUpEdit();
        public frm_prodect()
        {
            InitializeComponent();
            New();
            this.Load += Frm_prodect_Load;
        }

        private void Frm_prodect_Load(object sender, EventArgs e)
        {
            refreshData();
            lookUpCatedory.Properties.DisplayMember = "name";
            lookUpCatedory.Properties.ValueMember = "id";
            
            lookUpCatedory.ProcessNewValue += LookUpCatedory_ProcessNewValue;
            lookUpCatedory.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            lookUpType.Properties.DataSource = new List<ValueAndID>() { new ValueAndID() { id = 0, name = "مخزني" }, new ValueAndID() { id = 1, name = "خدمي" } };
            lookUpType.Properties.DisplayMember = "name";
            lookUpType.Properties.ValueMember = "id";


            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            Scr.Prodect_unit unit = new Scr.Prodect_unit();
            gridView1.Columns[nameof(unit.prodect_id)].Visible = false;
            gridView1.Columns[nameof(unit.id)].Visible = false;

            


            RepositoryItemCalcEdit calcEdit = new RepositoryItemCalcEdit();
            gridControl1.RepositoryItems.Add(calcEdit);
            gridControl1.RepositoryItems.Add(lookUpEdit);


            gridView1.Columns[nameof(unit.sellPress)].ColumnEdit = calcEdit;
            gridView1.Columns[nameof(unit.buyPress)].ColumnEdit = calcEdit;
            gridView1.Columns[nameof(unit.sellDiscount)].ColumnEdit = calcEdit;
            gridView1.Columns[nameof(unit.unit_id)].ColumnEdit = lookUpEdit;

            lookUpEdit.NullText = "";
            lookUpEdit.ValueMember = "id";
            lookUpEdit.DisplayMember = "name";
            refreshData();

            lookUpEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

            lookUpEdit.ProcessNewValue += LookUpEdit_ProcessNewValue;
        }

        private void LookUpEdit_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            if (e.DisplayValue is string s && s.Trim() != string.Empty)
            {
                var newobject = new Scr.Unit() { name = s.Trim() };
                using (Scr.DBDataContext db= new Scr.DBDataContext())
                {
                    db.Units.InsertOnSubmit(newobject);
                    db.SubmitChanges();
                }
                ((List<Scr.Unit>)lookUpEdit.DataSource).Add(newobject);
                e.Handled = true;
            }
        }

        class ValueAndID
        {
            public int id  { get; set; }
            public string name { get; set; }
        }

        private void LookUpCatedory_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
           
        }

        public override void New()
        {
            prodect = new Scr.Prodect();

            base.New();
        }
        Byte[] getByteFromImage(Image img)
        {
            using (MemoryStream stream = new MemoryStream() )
            {
               

                try
                {
                    img.Save(stream, ImageFormat.Jpeg);
                    return stream.ToArray();
                }
                catch {

                    return stream.ToArray();
                }
            }
        }
        Scr.DBDataContext dBs = new Scr.DBDataContext();
        public override void GetData()
        {
            txtCode.Text = prodect.code;
            txtName.Text = prodect.name;
            lookUpCatedory.EditValue = prodect.category_id;
            lookUpType.EditValue = prodect.type;
            memoEdit1.Text = prodect.descreption;
            checkEdit1.Checked = prodect.is_active;

            

            gridControl1.DataSource = dBs.Prodect_units.Where(x => x.prodect_id == prodect.id);
            base.GetData();
        }
        public override void SetData()
        {
            prodect.is_active = checkEdit1.Checked;
            prodect.code = txtCode.Text;
            prodect.name = txtName.Text;
            prodect.category_id = Convert.ToInt32(lookUpCatedory.EditValue);
            prodect.descreption = memoEdit1.Text;
            prodect.type = Convert.ToByte(lookUpType.EditValue);
            prodect.image=getByteFromImage( pictureEdit1.Image); 
            base.SetData();
        }
        bool ValidataData()
        {

            if (txtName.Text.Trim() == String.Empty)
            {
                txtName.ErrorText = errorText;
                return false;
            }
            else if (txtCode.Text.Trim() == string.Empty)
            {
                txtCode.ErrorText = errorText;
                return false;
            }

            else if(lookUpCatedory.EditValue is int == false)
            {
                lookUpCatedory.ErrorText = errorText;
                return false;

            }
            else if(lookUpType.EditValue is byte == false)
            {
                lookUpType.ErrorText = errorText;
            }

            Scr.DBDataContext db = new Scr.DBDataContext();


            var obj = db.Prodects.Where(x => x.name.Trim() == txtName.Text.Trim() && x.id != prodect.id);
            if (obj.Count() > 0)
            {
                txtName.ErrorText = "هذا الاسم موجود مسبقآ";
                return false;
            }
             obj = db.Prodects.Where(x => x.code.Trim() == txtCode.Text.Trim() && x.id != prodect.id);
            if (obj.Count() > 0)
            {
                txtCode.ErrorText = "هذا الكود موجود مسبقآ";
                return false;
            }
            return true;
        }
        public override void Save()
        {
            if (ValidataData() == false) return;

            Scr.DBDataContext db = new Scr.DBDataContext();

            if (prodect.id == 0)
                db.Prodects.InsertOnSubmit(prodect);
            else
                db.Prodects.Attach(prodect);

            SetData();
            db.SubmitChanges();

            var data = gridView1.DataSource as BindingList<Scr.Prodect_unit>;
            foreach (var item in data)
            {
                item.prodect_id = prodect.id;
                if (string.IsNullOrEmpty(item.barrCode))
                {
                    item.barrCode = "";
                }
            }

            dBs.SubmitChanges();
            base.Save();
        }
        public override void refreshData()
        {
            using (Scr.DBDataContext db = new Scr.DBDataContext()){
                //lookUpCatedory.Properties.DataSource = db.Prodect_Categories.Where(x => db.Prodect_Categories.Where(y => y.parent_id == x.id).Count() == 0).ToList();
                lookUpCatedory.Properties.DataSource = db.Prodect_Categories.ToList();
                lookUpEdit.DataSource = db.Units.ToList();

            }


            base.refreshData();
        }
    }
}
