using DevExpress.XtraEditors;
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
    }

}
