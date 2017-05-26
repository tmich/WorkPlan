using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class EmploymentRelationship
    {
        public long Id { get; set; }
        public DateTime HiringDate { get; set; }
        public DateTime FiringDate { get; set; }

        public EmploymentRelationship()
        {
            Id = 0;
            HiringDate = new DateTime(2000, 1, 1);
            FiringDate = new DateTime(9999, 12, 31);
        }

        public bool IsOpen()
        {
            return FiringDate.Year == 9999;
        }
    }
}
