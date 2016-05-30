using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

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

        public Dictionary<Employee, List<Duty>> GetDutiesBy(DateTime startDate, DateTime endDate)
        {
            var results = new Dictionary<Employee, List<Duty>>();
            var employees = employeeRepository.All();

            foreach (var employee in employees)
            {
                results.Add(employee, dutyRepository.GetBy(employee, startDate, endDate));
            }

            return results;
        }

        public Dictionary<Employee, List<Duty>> GetDutiesCassaBy(DateTime startDate, DateTime endDate)
        {
            var results = new Dictionary<Employee, List<Duty>>();
            var employees = employeeRepository.All();

            foreach (var employee in employees)
            {
                results.Add(employee, dutyRepository.GetCassaBy(employee, startDate, endDate));
            }

            return results;
        }

        public List<IWorkPeriod> GetBy(DateTime startDate, DateTime endDate)
        {
            var results = new List<IWorkPeriod>();
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
