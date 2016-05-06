using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class DutyList : List<Duty>
    {
        public List<Duty> GetBy(Employee employee, DateTime startDate)
        {
            var results = FindAll(
                delegate (Duty duty)
                {
                    return duty.Employee.FullName.Equals(employee.FullName) && duty.StartDate.ToShortDateString().Equals(startDate.ToShortDateString());
                }
                );

            return results;
        }
    }
}
