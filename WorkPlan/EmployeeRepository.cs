using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class EmployeeRepository
    {
        public EmployeeRepository()
        {
            //TODO: use database
            employees = new List<Employee>();

            Employee emp1 = new Employee()
            {
                Name = "Tiziano",
                LastName = "Michelessi",
                DateHired = new DateTime(2010, 1, 1)
            };
            employees.Add(emp1);

            Employee emp2 = new Employee()
            {
                Name = "Mario",
                LastName = "Rossi",
                DateHired = new DateTime(2005, 7, 1)
            };
            employees.Add(emp2);

            Employee emp3 = new Employee()
            {
                Name = "Giancarlo",
                LastName = "Magalli",
                DateHired = new DateTime(2001, 10, 1)
            };
            employees.Add(emp3);
        }

        public List<Employee> All()
        {
            return employees;
        }

        protected List<Employee> employees;
    }
}
