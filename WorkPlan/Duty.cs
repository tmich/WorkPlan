using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class Duty
    {
        private Employee mEmployee;
        public Employee Employee
        {
            get { return mEmployee; }
            set { mEmployee = value; }
        }

        private DateTime mStartDate;
        public DateTime StartDate
        {
            get { return mStartDate; }
            set { mStartDate = value; }
        }

        private DateTime mEndDate;
        public DateTime EndDate
        {
            get { return mEndDate; }
            set { mEndDate = value; }
        }

        private string mPosition;
        public string Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        private string mNotes;
        public string Notes
        {
            get { return mNotes; }
            set { mNotes = value; }
        }

    }
}
