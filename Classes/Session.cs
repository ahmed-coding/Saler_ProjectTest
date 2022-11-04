using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saler_Project.Classes
{
    internal static class Session
    {
        public static int DefualtDrawer { get => 3; }
        public static int DefualCustomer { get => 1; }
        public static int DefualtVendor { get => 3; }
        public static int DefualtStor { get => 3; }
        public static int DefualtRowStor { get => 4; }


        private static BindingList<Scr.Prodect> _prodects;
        public static BindingList<Scr.Prodect> prodects
        {
            get
            {
                if (_prodects == null)
                {
                    using (var db = new Scr.DBDataContext())
                    {
                        _prodects = new BindingList<Scr.Prodect>(db.Prodects.ToList());
                    }
                    DataBaseWatcher.prodects = new TableDependency.SqlClient.SqlTableDependency<Scr.Prodect>(Properties.Settings.Default.SelasConnectionString);
                    DataBaseWatcher.prodects.OnChanged += DataBaseWatcher.Prodect_Changed;
                    //DataBaseWatcher.prodects.Start();
                }
                return _prodects;
            }
        }

        private static BindingList<Scr.Customer> _vendor;
        public static BindingList<Scr.Customer> vendors
        {
            get
            {
                if (_vendor == null)
                {
                    using (var db = new Scr.DBDataContext())
                    {
                        _vendor = new BindingList<Scr.Customer>(db.Customers.Where(x=> x.isCustomer ==false).ToList());
                    }
                    //DataBaseWatcher.prodects = new TableDependency.SqlClient.SqlTableDependency<Scr.Prodect>(Properties.Settings.Default.SelasConnectionString);
                    //DataBaseWatcher.prodects.OnChanged += DataBaseWatcher.Prodect_Changed;
                    //DataBaseWatcher.prodects.Start();
                }
                return _vendor;
            }
        }

        private static BindingList<Scr.Customer> _customer;
        public static BindingList<Scr.Customer> customer
        {
            get
            {
                if (_customer == null)
                {
                    using (var db = new Scr.DBDataContext())
                    {
                        _customer = new BindingList<Scr.Customer>(db.Customers.Where(x => x.isCustomer == true).ToList());
                    }
                    //DataBaseWatcher.prodects = new TableDependency.SqlClient.SqlTableDependency<Scr.Prodect>(Properties.Settings.Default.SelasConnectionString);
                    //DataBaseWatcher.prodects.OnChanged += DataBaseWatcher.Prodect_Changed;
                    //DataBaseWatcher.prodects.Start();
                }
                return _customer;
            }
        }
        private static BindingList<Scr.Drawer> _drawer;
        public static BindingList<Scr.Drawer> drawers
        {
            get
            {
                if (_drawer == null)
                {
                    using (var db = new Scr.DBDataContext())
                    {
                        _drawer = new BindingList<Scr.Drawer>(db.Drawers.ToList());
                    }
                    //DataBaseWatcher.prodects = new TableDependency.SqlClient.SqlTableDependency<Scr.Prodect>(Properties.Settings.Default.SelasConnectionString);
                    //DataBaseWatcher.prodects.OnChanged += DataBaseWatcher.Prodect_Changed;
                    //DataBaseWatcher.prodects.Start();
                }
                return _drawer;
            }
        }
        private static BindingList<Scr.Stor> _stor;
        public static BindingList<Scr.Stor> stor
        {
            get
            {
                if (_stor == null)
                {
                    using (var db = new Scr.DBDataContext())
                    {
                        _stor = new BindingList<Scr.Stor>(db.Stors.ToList());
                    }
                    //DataBaseWatcher.prodects = new TableDependency.SqlClient.SqlTableDependency<Scr.Prodect>(Properties.Settings.Default.SelasConnectionString);
                    //DataBaseWatcher.prodects.OnChanged += DataBaseWatcher.Prodect_Changed;
                    //DataBaseWatcher.prodects.Start();
                }
                return _stor;
            }
        }
    }
}
