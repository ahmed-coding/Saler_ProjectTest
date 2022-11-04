using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using static Saler_Project.Classes.Master;

namespace Saler_Project.Forms
{
   
    public partial class frm_prodect : Saler_Project.Forms.frm_master
    {
        //start Mempers
      
        Scr.Prodect prodect;
        RepositoryItemLookUpEdit lookUpEdit = new RepositoryItemLookUpEdit();
        Scr.DBDataContext dBs = new Scr.DBDataContext();
        Scr.Prodect_unit unit = new Scr.Prodect_unit();




        //End Mempers

        //End Events
        public frm_prodect()
        {
            InitializeComponent();
            //refreshData();
            New();
            this.Load += Frm_prodect_Load;
        }

        public frm_prodect(int id)
        {
            InitializeComponent();
            //refreshData();
            loadProdect(id);
            
        }
        private void Frm_prodect_Load(object sender, EventArgs e)
        {
            refreshData();
            lookUpCatedory.Properties.DisplayMember = "name";
            lookUpCatedory.Properties.ValueMember = "id";
            
            lookUpCatedory.ProcessNewValue += LookUpCatedory_ProcessNewValue;
            lookUpCatedory.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            lookUpType.Properties.DataSource =prodectTypeList;
            lookUpType.Properties.DisplayMember = "name";
            lookUpType.Properties.ValueMember = "id";


            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            
            gridView1.Columns[nameof(unit.prodect_id)].Visible = false;
            gridView1.Columns[nameof(unit.id)].Visible = false;

            


            RepositoryItemCalcEdit calcEdit = new RepositoryItemCalcEdit();
            gridControl1.RepositoryItems.Add(calcEdit);
            gridControl1.RepositoryItems.Add(lookUpEdit);


            gridView1.Columns[nameof(unit.sellPress)].ColumnEdit = calcEdit;
            gridView1.Columns[nameof(unit.buyPress)].ColumnEdit = calcEdit;
            gridView1.Columns[nameof(unit.sellDiscount)].ColumnEdit = calcEdit;
            gridView1.Columns[nameof(unit.unit_id)].ColumnEdit = lookUpEdit;

            gridView1.Columns[nameof(unit.sellPress)].Caption = "سعر البيع ";
            gridView1.Columns[nameof(unit.sellDiscount)].Caption = "الخصم";
            gridView1.Columns[nameof(unit.vactor)].Caption = "الكمية ";
            gridView1.Columns[nameof(unit.barrCode)].Caption = "الكود";
            gridView1.Columns[nameof(unit.buyPress)].Caption = "سعر الشراء";
            gridView1.Columns[nameof(unit.unit_id)].Caption = "الوحدة";

            lookUpEdit.NullText = "";
            lookUpEdit.ValueMember = "id";
            lookUpEdit.DisplayMember = "name";            

            lookUpEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            lookUpEdit.ProcessNewValue += LookUpEdit_ProcessNewValue;

            gridView1.ValidateRow += GridView1_ValidateRow;
            gridView1.InvalidRowException += GridView1_InvalidRowException;
            gridView1.CellValueChanged += GridView1_CellValueChanged;
            gridView1.FocusedRowChanged += GridView1_FocusedRowChanged;

            gridView1.CustomRowCellEditForEditing += GridView1_CustomRowCellEditForEditing;
        }

        private void GridView1_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.Column.FieldName == nameof(unit.unit_id))
            {
                var ids = (((Collection<Scr.Prodect_unit>)gridView1.DataSource).Select(x => x.unit_id).ToList());
                RepositoryItemLookUpEdit repo = new RepositoryItemLookUpEdit();
                using (var db=new Scr.DBDataContext())
                {
                    var currentID = (Int32?)e.CellValue;
                    ids.Remove(currentID ?? 0);
                    repo.DataSource = db.Units.Where(x => ids.Contains(x.id) == false).ToList();
                    repo.ValueMember = "id";
                    repo.DisplayMember = "name";
                    repo.PopulateColumns();
                    repo.Columns["id"].Visible = false;
                    repo.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                    repo.ProcessNewValue += LookUpEdit_ProcessNewValue;

                    e.RepositoryItem = repo;
                }


            }
        }

        private void GridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
           
            
                gridView1.Columns[nameof(unit.vactor)].OptionsColumn.AllowEdit = !(e.FocusedRowHandle ==0);
            
        }

        private void GridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

        }

        private void GridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }

        private void GridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            var row = e.Row as Scr.Prodect_unit;
            var view = sender as GridView;
            if (row == null) return;

            if(row.vactor <= 1 && e.RowHandle !=0)
            {
                e.Valid = false;
                gridView1.SetColumnError(gridView1.Columns[nameof(row.vactor)],"يجب ان تكون القيمة اكبر من واحد ");
            }
            if (row.unit_id <=0)
            {
                e.Valid = false;
                gridView1.SetColumnError(gridView1.Columns[nameof(row.unit_id)], errorText);
            }
            if (checkBarrcode(row.barrCode, prodect.id))
            {
                e.Valid = false;
                gridView1.SetColumnError(gridView1.Columns[nameof(row.barrCode)], "هذا الكود موجود بافعل");
            }


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
                ((List<Scr.Unit>) (((LookUpEdit)sender).Properties.DataSource)).Add(newobject);
                e.Handled = true;

            }
        }


        private void LookUpCatedory_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
           
        }
        //End Events


        //start methods
        void loadProdect(int id)
        {
            using (var db= new Scr.DBDataContext())
            {
                prodect = db.Prodects.Single(x => x.id == id);

            }
            this.Text = string.Format("بيانات صنف : {0}", prodect.name);
            GetData();
        }
        public override void New()
        {
            prodect = new Scr.Prodect()
            {
                code= getNewCode()
            } ;

            base.New();
            this.Text ="اضافة صنف جديد";
           var data = gridView1.DataSource as BindingList<Scr.Prodect_unit>;
            var db =new Scr.DBDataContext();
            if (db.Units.Count() == 0)
            {
                db.Units.InsertOnSubmit(new Scr.Unit() { name = "قطعه" } );
                db.SubmitChanges();
                refreshData();
            }
            data.Add(new Scr.Prodect_unit() { vactor = 1, unit_id = db.Units.First().id,barrCode =getNewBarrCode() });
        }
        Image getImageFromByte(Byte[] bytArryay)
        {
            Image img;
            try
            {
                Byte[] imgByte = bytArryay;
                MemoryStream stream = new MemoryStream(imgByte,false);

                img = Image.FromStream(stream);
            }
            catch (Exception)
            {

                img = null;
            }
            return img;
        }
        Byte[] getByteFromImage(Image img)
        {
            using (MemoryStream stream = new MemoryStream() )
            {

                if (img == null)
                    return stream.ToArray();

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

       
        
        public override void GetData()
        {
            txtCode.Text = prodect.code;
            txtName.Text = prodect.name;
            lookUpCatedory.EditValue = prodect.category_id;
            lookUpType.EditValue = prodect.type;
            memoEdit1.Text = prodect.descreption;
            checkEdit1.Checked = prodect.is_active;
            if (prodect.image != null)
                pictureEdit1.Image = getImageFromByte(prodect.image.ToArray());
            else
                pictureEdit1.Image = null;



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
                if (item.barrCode == string.Empty || item.barrCode == null)
                {
                    item.barrCode = "";
                }
            }

            dBs.SubmitChanges();
            base.Save();
            this.Text = string.Format("بيانات صنف : {0}", prodect.name);
        }
        public override void refreshData()
        {
            using (Scr.DBDataContext db = new Scr.DBDataContext()){
               
                lookUpCatedory.Properties.DataSource = db.Prodect_Categories.ToList();
                lookUpEdit.DataSource = db.Units.ToList();

            }


            base.refreshData();
        }
        string getNewCode()
        {
            string maxCode;
            using (Scr.DBDataContext db = new Scr.DBDataContext())
            {
                maxCode = db.Prodects.Select(x => x.code).Max();
            }

            return getNextCode(maxCode);

        }
        string getNewBarrCode()
        {
            string maxCode;
            using (Scr.DBDataContext db = new Scr.DBDataContext())
            {
                maxCode = db.Prodect_units.Select(x => x.barrCode).Max();
            }

            return getNextCode(maxCode);

        }

        string getNextCode( string code)
        {
            if (code == string.Empty || code == null) return "1";

            string str1 = "";
            foreach (char item in code)
            {
                str1 = char.IsDigit(item) ? str1 + item.ToString() : "";
            }

            if (str1 == string.Empty) return code + "1";


            string str2 = str1.Insert(0, "1");
            str2 = (Convert.ToUInt32(str2)+1).ToString();

         string  str3 = str2[0] == '1' ? str2.Remove(0, 1) : str2.Remove(0, 1).Insert(0,"1");

            int index = code.LastIndexOf(str1);
            code = code.Remove(index);

            code = code.Insert(index, str3);

            return code;
        }

        Boolean checkBarrcode(string barrCode  ,int id)
        {
            using (Scr.DBDataContext db = new Scr.DBDataContext())
            {
              return  db.Prodect_units.Select(x => x.barrCode == barrCode && x.prodect_id != id ).Count() > 0;
            }

        }

        //End methods
    }
}
