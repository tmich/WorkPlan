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
        public string Code { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(Object rhs)
        {
            // If parameter is null return false:
            if ((object)rhs == null)
            {
                return false;
            }

            NoWorkReason nwr = rhs as NoWorkReason;

            return (Id == nwr.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
