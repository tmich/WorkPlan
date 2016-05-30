using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public interface IWorkPeriod
    {
        int Id { get; set; }
        Employee Employee { get; }
        DateTime StartDate { get; }
        DateTime EndDate { get; }
        bool FullDay { get; }
        
        string Notes { get; set; }
    }
}
