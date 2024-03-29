﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient.Base.Enums;
using System.Windows.Forms;
namespace Saler_Project.Classes
{
    internal static class DataBaseWatcher
    {
        public static SqlTableDependency<Scr.Prodect> prodects;

      public  static void Prodect_Changed(object sender , RecordChangedEventArgs<Scr.Prodect> e)
        {
            Application.OpenForms[0].Invoke( new Action(() =>
            {
                if (e.ChangeType == ChangeType.Insert)
                {
                    Session.prodects.Add(e.Entity);
                }
                else if (e.ChangeType == ChangeType.Update)
                {
                    var index = Session.prodects.IndexOf(Session.prodects.Single(x => x.id == e.Entity.id));

                    Session.prodects.Remove(Session.prodects.Single(x => x.id == e.Entity.id));
                    Session.prodects.Insert(index,e.Entity);

                }
                else if (e.ChangeType == ChangeType.Delete)
                {
                    Session.prodects.Remove(Session.prodects.Single(x => x.id == e.Entity.id));
                }
            }));
           
       }

        public static SqlTableDependency<Scr.Customer> vendor;
        public static void Vendor_Changed(object sender, RecordChangedEventArgs<Scr.Customer> e)
        {
            Application.OpenForms[0].Invoke(new Action(() =>
            {
                switch (e.ChangeType)
                {
                    case ChangeType.None:
                        break;
                    case ChangeType.Delete:
                        Session.vendors.Remove(Session.vendors.Single(x => x.id == e.Entity.id));
                        break;
                    case ChangeType.Insert:
                        Session.vendors.Add(e.Entity);
                        break;
                    case ChangeType.Update:
                        var index = Session.vendors.IndexOf(Session.vendors.Single(x => x.id == e.Entity.id));
                        Session.vendors.Remove(Session.vendors.Single(x => x.id == e.Entity.id));
                        Session.vendors.Add(e.Entity);
                        break;
                    default:
                        break;
                }
            }));

        }


    }
}
