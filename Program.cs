using Saler_Project.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Saler_Project
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var frm= new frm_login();
            frm.ShowDialog();

            //Application.Run(new frm_invoice(Classes.Master.InvoiceType.Purchase));
            Application.Run();
            //Application.Run(frm_main.Instance);



        }
    }
}
