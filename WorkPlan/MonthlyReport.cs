using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    class MonthlyReport
    {
        //protected PrintDocument pd;
        protected int m_month;

        public MonthlyReport(int month)
        {
            m_month = month;
        }

        public void Print()
        {
            Debug.Print("Ciao " + m_month);
            EmployeeRepository repo = new EmployeeRepository();
            var employees = repo.All();
            //DutyService dutyService = new DutyService();
            //DutyRepository dutyRepo = new DutyRepository();
            foreach (var emp in employees)
            {
                
            }
        }
    }
}
