using System;
using System.Collections.Generic;

namespace WorkPlan
{
    public class PeriodService
    {
        protected string connstr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private EmployeeRepository employeeRepository;
        private DutyRepository dutyRepository;
        private NoWorkRepository noworkRepository;

        public PeriodService()
        {
            employeeRepository = new EmployeeRepository();
            dutyRepository = new DutyRepository();
            noworkRepository = new NoWorkRepository();
        }

        public List<IWorkPeriod> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            var results = new List<IWorkPeriod>();
            var employees = employeeRepository.All();

            foreach (var employee in employees)
            {
                foreach (var duty in dutyRepository.GetBy(employee, startDate, endDate))
                {
                    results.Add(duty);
                }

                foreach (var nowork in noworkRepository.GetAssenzeByDipDateRange(employee, startDate, endDate))
                {
                    results.Add(nowork);
                }
            }

            return results;
        }

        public List<IWorkPeriod> GetByEmployeeAndDate(Employee employee, DateTime datetime)
        {
            var results = new List<IWorkPeriod>();
            
            foreach (var duty in dutyRepository.GetBy(employee, datetime))
            {
                results.Add(duty);
            }

            foreach (var nowork in noworkRepository.GetAssenzeByEmployeeAndDate(employee, datetime))
            {
                results.Add(nowork);
            }
            

            return results;
        }
    }
}
