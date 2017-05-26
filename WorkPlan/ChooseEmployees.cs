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

        public ChooseEmployees(DateTime? dateRif)
        {
            employees = new List<Employee>();
            m_dateRif = dateRif;
        }
        
        public List<Employee> AskUser()
        {
            if (!m_dateRif.HasValue)
                m_dateRif = DateTime.Now;

            DlgChooseEmployee dlg = new DlgChooseEmployee(m_dateRif.Value);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                employees = dlg.GetSelectedEmployees();
            }

            return employees;
        }

        private DateTime? m_dateRif;
    }
}
