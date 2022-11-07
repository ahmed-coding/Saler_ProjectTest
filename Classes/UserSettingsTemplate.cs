using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saler_Project.Classes
{
    internal class UserSettingsTemplate
    {

        public class UserGeneralSettings
        {
            int profile_id { get; set; }
            public UserGeneralSettings(int profile_id)
            {
                this.profile_id = profile_id;
                genral = new GeneralSettings(this.profile_id);
                invoices = new InvoicesSettings(this.profile_id);
                purchase = new PurchaseSettings(this.profile_id);
                sales = new SalesSettings(this.profile_id);
            }
            public GeneralSettings genral;
            public InvoicesSettings invoices;
            public PurchaseSettings purchase;
            public SalesSettings sales;



        }
        public class GeneralSettings
        {
            int profile_id { get; set; }
            public GeneralSettings(int profile_id)
            {
                this.profile_id = profile_id;
            }
            public bool CanChangeStore { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public int DefualtRawStor { get { return Master.FromByteArray<int>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public int DefualtStor { get { return Master.FromByteArray<int>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public bool CanChangeDrawer { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public int DefualtDrawer { get { return Master.FromByteArray<int>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public int DefualtCustomer { get { return Master.FromByteArray<int>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public bool CanChangeCustomer { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public bool CanChangeVendor { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public bool CanViewDocumentHistory { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public int DefualtVendor { get { return Master.FromByteArray<int>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
        }
        public class SalesSettings
        {
            int profile_id { get; set; }
            public SalesSettings(int profile_id)
            {
                this.profile_id = profile_id;
            }
            public bool CanChangePaidInSales { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public bool CanNotPostToStoreInSales { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public Master.WarningLevels DefualtPayMethodInSales { get { return Master.FromByteArray<Master.WarningLevels>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public Master.WarningLevels WhenSalingToACustomerExcededMaxCredit { get { return Master.FromByteArray<Master.WarningLevels>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public bool CanCangeItemPriceInSales { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }

            public Master.WarningLevels WhenSellingItemReachedReOrderLevel { get { return Master.FromByteArray<Master.WarningLevels>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public Master.WarningLevels WhenSellingItemWithQtyMoreThanAvailableQty { get { return Master.FromByteArray<Master.WarningLevels>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public Master.WarningLevels WhenSellingItemWithPriceLowerThanCostPrice { get { return Master.FromByteArray<Master.WarningLevels>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }

            public bool HideCostInSales { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public bool CanChangeQtyInSales { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public bool CanChangeSalesInvoiceDate { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public bool CanSallToVendors { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public double MaxDiscountPerItem { get { return Master.FromByteArray<double>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public double MaxDiscountInInvoice { get { return Master.FromByteArray<double>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }




        }

        public class InvoicesSettings
        {
            int profile_id { get; set; }
            public InvoicesSettings(int profile_id)
            {
                this.profile_id = profile_id;
            }
            public bool CanDeleteItemsInInvoices { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public bool CanChangeTax { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }

        }
        public class PurchaseSettings
        {
            int profile_id { get; set; }
            public PurchaseSettings(int profile_id)
            {
                this.profile_id = profile_id;
            }
            public bool CanChangeItemPriceInPurchase { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public bool CanBuyFromCustomers { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
            public bool CanChangePurchaseInvoiceDate { get { return Master.FromByteArray<bool>(Master.GetPropertyValue(Master.GetLineNumber(), profile_id)); } }
        }
        }
}
