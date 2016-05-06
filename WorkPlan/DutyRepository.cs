using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class DutyRepository
    {
        DutyList duties;

        public DutyRepository()
        {
            duties = new DutyList()
            {
                new Duty()
                {
                    Employee = new Employee()
                    {
                        Name = "Tiziano",
                        LastName = "Michelessi",
                        DateHired = new DateTime(2010, 1, 1)
                    },
                    Id = 1,
                    StartDate = new DateTime(2016, 05, 06, 12, 0, 0),
                    EndDate = new DateTime(2016, 05, 06, 18, 0, 0),
                    Position = "Cassa",
                    Notes = "Occhio che ruba"
                },

                new Duty()
                {
                    Employee = new Employee()
                    {
                        Name = "Tiziano",
                        LastName = "Michelessi",
                        DateHired = new DateTime(2010, 1, 1)
                    },
                    Id = 2,
                    StartDate = new DateTime(2016, 05, 06, 19, 0, 0),
                    EndDate = new DateTime(2016, 05, 06, 23, 0, 0),
                    Position = "Banco",
                    Notes = "Doppio Turno"
                },

                new Duty()
                {
                    Employee = new Employee()
                    {
                        Name = "Mario",
                        LastName = "Rossi",
                        DateHired = new DateTime(2005, 7, 1)
                    },
                    Id = 3,
                    StartDate = new DateTime(2016, 05, 06, 14, 0, 0),
                    EndDate = new DateTime(2016, 05, 06, 19, 0, 0),
                    Position = "Banco",
                    Notes = ""
                }
            };
        }
        
        public List<Duty> GetBy(Employee employee, DateTime startDate)
        {
            var results = duties.FindAll(
                delegate (Duty duty)
                {
                    return duty.Employee.FullName.Equals(employee.FullName) && duty.StartDate.ToShortDateString().Equals(startDate.ToShortDateString());
                }
                );

            results.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));

            return results;
        }

        public DutyList All()
        {
            return duties;
        }

        public void Add(ref Duty duty)
        {
            duties.Add(duty);
            duty.Id = duties.Count + 1;
        }

        public void Update(ref Duty duty)
        {
            int id = duty.Id;
            int idx = duties.FindIndex(
                delegate(Duty d)
                {
                    return id == d.Id;
                }
            );

            if (idx >= 0)
            {
                duties[idx] = duty;
            }

            duty = duties[idx];
        }

        public void Delete(Duty duty)
        {
            int id = duty.Id;
            int idx = duties.FindIndex(
                delegate (Duty d)
                {
                    return id == d.Id;
                }
            );

            duties.RemoveAt(idx);
        }
    }
}
