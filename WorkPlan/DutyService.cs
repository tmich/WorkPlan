using MySql.Data.MySqlClient;
using System;

namespace WorkPlan
{
    public class DutyService
    {
        protected string connstr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private EmployeeRepository employeeRepository;
        private DutyRepository dutyRepository;

        public DutyService()
        {
            employeeRepository = new EmployeeRepository();
            dutyRepository = new DutyRepository();
        }
        
        public DutyList GetBy(DateTime startDate, DateTime endDate)
        {
            var results = new DutyList();
            var employees = employeeRepository.All();

            foreach (var employee in employees)
            {
                foreach(var duty in dutyRepository.GetBy(employee, startDate, endDate))
                {
                    results.Add(duty);
                }
            }

            return results;
        }
    }
}
