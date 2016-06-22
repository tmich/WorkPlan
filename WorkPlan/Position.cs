using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class Position
    {
        public int Id { get; private set; }
        public string Desc { get; private set; }

        public Position(int id, string desc)
        {
            Id = id;
            Desc = desc;
        }

        public override string ToString()
        {
            return Desc;
        }
    }
}
