using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saler_Project.Classes
{
    public static class Master
    {
       public class ValueAndID
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public static  List<ValueAndID> prodectTypeList = new List<ValueAndID> {
           new ValueAndID() { id = 0, name = "مخزني" },
           new ValueAndID() { id = 1, name = "خدمي" }
       };
        public enum ProdectType
        {
            Inventory,
            Servise,
        }

        public static List<ValueAndID> PartTypeList = new List<ValueAndID> {
           new ValueAndID() { id = 0, name = "مورد" },
           new ValueAndID() { id = 1, name = "عميل" }
       };
        public enum PartType
        {
            Vendor,
            Customer,

        }

        public static List<ValueAndID> InvoiceTypeList = new List<ValueAndID> {
           new ValueAndID() { id = (int)InvoiceType.Purchase , name = "مشتريات" },
           new ValueAndID() { id = (int)InvoiceType.Sales , name = "مبيعات" },
            new ValueAndID() { id = (int)InvoiceType.PurchaseReturn , name = "مردود مشتريات" },
            new ValueAndID() { id = (int)InvoiceType.SalesReturn , name = "مردود مبيعات" },
        };

        public enum InvoiceType
        {
            Purchase,
            Sales,
            PurchaseReturn,
            SalesReturn,

        }


        public static void initData(this RepositoryItemGridLookUpEditBase repo, object dataSours,GridColumn column ,GridControl grid)
        {
            initData(repo:repo, dataSours:dataSours, column:column, grid:grid,"name", "id");
        }
        public static void initData(RepositoryItemGridLookUpEditBase repo, object dataSours, GridColumn column, GridControl grid, string DisplayMember, string ValueMember)
        {
            if (repo == null)
                repo = new RepositoryItemGridLookUpEdit();
            repo.DataSource = dataSours;
            repo.DisplayMember = DisplayMember;
            repo.ValueMember = ValueMember;

            column.ColumnEdit = repo;

            if (grid != null)
                grid.RepositoryItems.Add(repo);
        }
        public  static void initData(this LookUpEdit lkp, object dataSours)
        {
            initData(lkp, dataSours, "name", "id");
        }
       public static void initData(LookUpEdit lkp, object dataSours, string DisplayMember, string ValueMember)
        {
            lkp.Properties.DataSource = dataSours;
            lkp.Properties.DisplayMember = DisplayMember;
            lkp.Properties.ValueMember = ValueMember;
            //lkp.Properties.PopulateColumns();
            //lkp.Properties.Columns[ValueMember].Visible = false;
        }

        public static void initData(this GridLookUpEdit lkp, object dataSours)
        {
            initData(lkp, dataSours, "name", "id");
        }
        public static void initData(GridLookUpEdit lkp, object dataSours, string DisplayMember, string ValueMember)
        {
            lkp.Properties.DataSource = dataSours;
            lkp.Properties.DisplayMember = DisplayMember;
            lkp.Properties.ValueMember = ValueMember;
        }
        public static string errorText
        {
            get { return "هذا الحقب مطلوب"; }
        }
            
                
                
        public static bool isTextValid(this TextEdit txt )
        {
            if(txt.Text.Trim() == string.Empty)
            {
                txt.ErrorText = errorText;
                return false;
            }
            return true;

        }
        public static bool isEditValid(this LookUpEditBase lkp)
        {
            if (lkp.isInt() == false)
            {
                lkp.ErrorText = errorText;
                return false;
            }
            return true;

        }
        public static bool isEditValidAndNotZero(this LookUpEditBase lkp)
        {
            if (lkp.isInt() == false || Convert.ToInt32(lkp.EditValue) == 0)
            {
                lkp.ErrorText = errorText;
                return false;
            }
            return true;

        }

        public static bool isInt(this LookUpEditBase edit)
        {
            var v = edit.EditValue;
            return (v is int || v is byte);
        }
        public static bool isDateValid(this DateEdit dt)
        {
            if(dt.DateTime.Year < 1950)
            {
                dt.ErrorText = errorText;
                return false;

            }
            return true;
        }
       public static string getNextCode(string code)
        {
            if (code == string.Empty || code == null) return "1";

            string str1 = "";
            foreach (char item in code)
            {
                str1 = char.IsDigit(item) ? str1 + item.ToString() : "";
            }

            if (str1 == string.Empty) return code + "1";


            string str2 = str1.Insert(0, "1");
            str2 = (Convert.ToUInt32(str2) + 1).ToString();

            string str3 = str2[0] == '1' ? str2.Remove(0, 1) : str2.Remove(0, 1).Insert(0, "1");

            int index = code.LastIndexOf(str1);
            code = code.Remove(index);

            code = code.Insert(index, str3);

            return code;
        }
    }

}
