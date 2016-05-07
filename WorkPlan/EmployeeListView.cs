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
    public partial class EmployeeListView : ChildForm
    {
        private List<Employee> mEmployees;

        public EmployeeListView()
        {
            InitializeComponent();
        }

        public void SetEmployees(List<Employee> employees)
        {
            mEmployees = employees;
            UpdateData();
        }

        protected void UpdateData()
        {
            foreach (var employee in mEmployees)
            {
                ListViewItem lvi = new ListViewItem(employee.Name);
                lvi.SubItems.Add(employee.LastName);
                lvi.SubItems.Add(employee.DateHired.ToShortDateString());
                lvEmployees.Items.Add(lvi);
            }
        }

        public override void Print()
        {
            //
        }
    }
}
