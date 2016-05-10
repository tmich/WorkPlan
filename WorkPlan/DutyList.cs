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
                    return duty.Employee.Id == employee.Id && duty.StartDate.Date.Equals(startDate.Date);
                }
                );

            return results;
        }
    }
}
