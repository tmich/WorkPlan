using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class DateUtils
    {
        public static DateTime FirstDayOfWeek(DateTime date)
        {
            DateTime lunedi = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            while (lunedi.DayOfWeek != DayOfWeek.Monday)
                lunedi = lunedi.AddDays(-1);

            return lunedi;
        }
    }
}
