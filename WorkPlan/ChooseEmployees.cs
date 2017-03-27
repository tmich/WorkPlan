using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkPlan
{
    class ChooseEmployees
    {
        private List<Employee> employees;

        public ChooseEmployees()
        {
            employees = new List<Employee>();
        }
        
        public List<Employee> AskUser()
        {
            DlgChooseEmployee dlg = new DlgChooseEmployee();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                employees = dlg.GetSelectedEmployees();
            }

            return employees;
        }
    }
}
