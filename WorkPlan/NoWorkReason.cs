using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class NoWorkReason
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
