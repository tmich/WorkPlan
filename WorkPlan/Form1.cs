using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkPlan
{
    public partial class Form1 : Form
    {
        EmployeeListView employeesView;
        ScheduleView scheduleView;

        public Form1()
        {
            InitializeComponent();

            EmployeeRepository employeeRepo = new EmployeeRepository();
            employeesView = new EmployeeListView();
            employeesView.SetEmployees(employeeRepo.All());

            scheduleView = new ScheduleView();
            
            employeesView.Dock = DockStyle.Fill;
            employeesView.Name = "uc";
            tabContainer.TabPages[1].Controls.Add(employeesView);

            scheduleView.Dock = DockStyle.Fill;
            scheduleView.Name = "uc";
            tabContainer.TabPages[0].Controls.Add(scheduleView);

            //stampaToolStripMenuItem.Enabled = false;
        }

        //private TabControl TabContainer
        //{
        //    get
        //    {
        //        return (TabControl)Controls["tabContainer"];
        //    }
        //}

        //private Panel ContainerPanel
        //{
        //    get
        //    {
        //        return (Panel)Controls["containerPanel"];
        //    }
        //}

        //private void ShowChild(ChildForm childForm)
        //{
        //    ContainerPanel.Controls.Clear();
        //    childForm.Dock = DockStyle.Fill;
        //    childForm.TopLevel = false;
        //    child = childForm;
        //    ContainerPanel.Controls.Add(childForm);
        //    stampaToolStripMenuItem.Enabled = true;
        //    childForm.Show();
        //}

        private void esciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void plannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ScheduleView scheduleView = new ScheduleView();
            //ShowChild(scheduleView);
        }

        private void stampaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var uc = (PrintableUserControl)tabContainer.SelectedTab.Controls["uc"];
            uc.Print();
        }

        private void dipendentiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //EmployeeRepository employeeRepo = new EmployeeRepository();
            //EmployeeListView employeesView = new EmployeeListView();
            //employeesView.SetEmployees(employeeRepo.All());

            //ShowChild(employeesView);
        }
    }
}
