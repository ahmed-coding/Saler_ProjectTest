using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saler_Project.Classes
{
    internal  class Session
    {
        public static class CurrentUser
        {
            private static UserSettingsTemplate _userSettings;
            public static UserSettingsTemplate Settings
            {
                get
                {
                    if (_userSettings == null)
                    {
                        _userSettings = new UserSettingsTemplate();
                    }

                    return _userSettings;
                }
            } 
 
        }


        public static int DefualtDrawer { get => 6; }
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
                    _prodects.ListChanged += _prodects_ListChanged;
                }
                return _prodects;
            }
        }

        private static void _prodects_ListChanged(object sender, ListChangedEventArgs e)
        {
            using (var db = new Scr.DBDataContext())
            {
                var data = from pr in db.Prodects
                           join cg in db.Prodect_Categories on pr.category_id equals cg.id
                           select new ProdectViewClass
                           {
                               id = pr.id,
                               name = pr.name,
                               code = pr.code,
                               CategoryName = cg.name,
                               descreption = pr.descreption,
                               is_active = pr.is_active,
                               type = pr.type,
                               //UOM= db.Prodect_units.Where(x=> x.prodect_id ==pr.id).Select(u=> new
                               Units = (from u in db.Prodect_units
                                        where u.prodect_id == pr.id
                                        join un in db.Units on u.unit_id equals un.id
                                        select new ProdectViewClass.ProdectUMOView
                                        {
                                            UnitName =/* db.Units.Single(un => un.id == u.unit_id).name,*/ un.name,
                                            vactor = u.vactor,
                                            sellPress = u.sellPress,
                                            buyPress = u.buyPress,
                                            barrCode = u.barrCode,
                                            sellDiscount = u.sellDiscount
                                        }).ToList(),
                           };
                prodectViewClasses = new BindingList<ProdectViewClass>(data.ToList());
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
                    DataBaseWatcher.vendor = new  TableDependency.SqlClient.SqlTableDependency<Scr.Customer>(Properties.Settings.Default.SelasConnectionString);
                    DataBaseWatcher.vendor.OnChanged += DataBaseWatcher.Vendor_Changed;
                    //DataBaseWatcher.vendor.Start();
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

        private static BindingList<ProdectViewClass> prodectViewClasses;
        public  BindingList<ProdectViewClass> ProdectView { get
            {
                if (prodectViewClasses == null)
                {
                    using (var db = new Scr.DBDataContext())
                    {
                        var data = from pr in db.Prodects
                                   join cg in db.Prodect_Categories on pr.category_id equals cg.id
                                   select new ProdectViewClass
                                   {
                                       id = pr.id,
                                       name = pr.name,
                                       code = pr.code,
                                       CategoryName = cg.name,
                                       descreption = pr.descreption,
                                       is_active = pr.is_active,
                                       type = pr.type,
                                       //UOM= db.Prodect_units.Where(x=> x.prodect_id ==pr.id).Select(u=> new
                                       Units = (from u in db.Prodect_units
                                                where u.prodect_id == pr.id
                                                join un in db.Units on u.unit_id equals un.id
                                                select new ProdectViewClass.ProdectUMOView
                                                {
                                                    UnitName =/* db.Units.Single(un => un.id == u.unit_id).name,*/ un.name,
                                                    vactor = u.vactor,
                                                    sellPress = u.sellPress,
                                                    buyPress = u.buyPress,
                                                    barrCode = u.barrCode,
                                                    sellDiscount = u.sellDiscount
                                                }).ToList(),
                                   };
                        prodectViewClasses = new BindingList<ProdectViewClass>(data.ToList());
                    }
                }

                return prodectViewClasses;
            } 
        
        }
       

        public  class ProdectViewClass
        {
            public int id { get; set; }
            public string code { get; set; }
            public string name { get; set; }
            public string CategoryName { get; set; }
            public string descreption { get; set; }
            public Boolean is_active { get; set; }
            public int type { get; set; }
            public List<ProdectUMOView> Units { get; set; }

            public class ProdectUMOView
            {
                public int unit_id { get; set; }
                public string UnitName { get; set; }
                public double vactor { get; set; }
                public double sellPress { get; set; }
                public double buyPress { get; set; }
                public double sellDiscount { get; set; }
                public string barrCode { get; set; }


            }





            //using (var db = new Scr.DBDataContext())
            //{
            //   var data = from pr in db.Prodects
            //              join cg in db.Prodect_Categories on pr.category_id equals cg.id
            //              select new
            //              {
            //                  pr.id,
            //                  pr.name,
            //                  pr.code,
            //                  CategoryName = cg.name,
            //                  pr.descreption,
            //                  pr.is_active,
            //                  pr.type,
            //                  //UOM= db.Prodect_units.Where(x=> x.prodect_id ==pr.id).Select(u=> new
            //                  UOM = (from u in db.Prodect_units
            //                         where u.prodect_id == pr.id
            //                         join un in db.Units on u.unit_id equals un.id
            //                         select new
            //                         {
            //                             UnitName =/* db.Units.Single(un => un.id == u.unit_id).name,*/ un.name,
            //                             u.vactor,
            //                             u.sellPress,
            //                             u.buyPress,
            //                             u.sellDiscount,
            //                             u.barrCode,
            //                         }).ToList(),




            //              };

        }
        private static BindingList<Scr.UserSettingsProfilePropertie> _profileProperties;
        public static BindingList<Scr.UserSettingsProfilePropertie> profileProperties { get
            {
                if (_profileProperties == null)
                {
                    using (var db = new Scr.DBDataContext())
                    {
                        _profileProperties = new BindingList<Scr.UserSettingsProfilePropertie>(db.UserSettingsProfileProperties.ToList());

                    }
                }
                return _profileProperties;
            }
        }
    }
}
