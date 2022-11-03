using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

}
