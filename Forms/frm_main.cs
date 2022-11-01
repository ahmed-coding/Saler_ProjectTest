using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Saler_Project.Forms
{
    public partial class frm_main : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        public frm_main()
        {
            InitializeComponent();
            accordionControl1.ElementClick += AccordionControl1_ElementClick;
        }

        private void AccordionControl1_ElementClick(object sender, DevExpress.XtraBars.Navigation.ElementClickEventArgs e)
        {
            var tag = e.Element.Tag as string;
            if(tag != string.Empty)
            {
                openForm(tag);
            }
        }

        public void openForm(string name)
        {

            var ins = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.Name == name);

            if (ins != null)
            {

                var frm = Activator.CreateInstance(ins) as Form;
                if (Application.OpenForms[frm.Name] != null)
                {
                    frm = Application.OpenForms[frm.Name];
                }
                else 
                    frm.Show();

            }
        }

        private void accordionControlElement1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
