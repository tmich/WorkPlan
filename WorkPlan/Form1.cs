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
        ChildForm child = null;

        public Form1()
        {
            InitializeComponent();

            stampaToolStripMenuItem.Enabled = false;
        }

        private Panel ContainerPanel
        {
            get
            {
                return (Panel)Controls["containerPanel"];
            }
        }

        private void ShowChild(ChildForm childForm)
        {
            ContainerPanel.Controls.Clear();
            childForm.Dock = DockStyle.Fill;
            childForm.TopLevel = false;
            child = childForm;
            ContainerPanel.Controls.Add(childForm);
            stampaToolStripMenuItem.Enabled = true;
            childForm.Show();
        }

        private void esciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void plannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScheduleView scheduleView = new ScheduleView();
            ShowChild(scheduleView);
        }

        private void stampaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (child != null)
            {
                child.Print();
            }
        }

        private void dipendentiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmployeeRepository employeeRepo = new EmployeeRepository();
            EmployeeListView employeesView = new EmployeeListView();
            employeesView.SetEmployees(employeeRepo.All());

            ShowChild(employeesView);
        }
    }
}
