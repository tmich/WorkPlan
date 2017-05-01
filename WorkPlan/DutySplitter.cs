using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class DutySplitter
    {
        protected Duty mDuty;
        public DutySplitter(Duty duty)
        {
            mDuty = duty;
        }

        public List<Duty> Split()
        {
            List<Duty> l = new List<Duty>();
            if(mDuty.IsMultipleDays)
            {
                DateTime startDate = mDuty.StartDate.Date;
                while (startDate <= mDuty.EndDate.Date)
                {
                    Duty dayDuty = new Duty()
                    {
                        Employee = mDuty.Employee,
                        EndDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 
                                  mDuty.EndDate.Hour, mDuty.EndDate.Minute, mDuty.EndDate.Second),
                        Id = mDuty.Id,
                        Notes = mDuty.Notes,
                        Position = mDuty.Position,
                        StartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day,
                                    mDuty.StartDate.Hour, mDuty.StartDate.Minute, mDuty.StartDate.Second),
                        User = mDuty.User
                    };
                    l.Add(dayDuty);

                    startDate = startDate.AddDays(1);
                }
            }
            else
            {
                l.Add(mDuty);
            }

            return l;
        }
    }
}
