using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class Duty
    {
        public int Id { get; set; }
        
        private Employee mEmployee;
        public Employee Employee
        {
            get { return mEmployee; }
            set { mEmployee = value; }
        }

        public User User { get; set; }

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

        public bool FullDay
        {
            get
            {
                return false;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}-{2}", Position, StartDate.ToShortTimeString(), EndDate.ToShortTimeString());
        }

        public override bool Equals(object obj)
        {
            Duty d = obj as Duty;
            if ((object)d == null)
            {
                return false;
            }

            return base.Equals(obj) && Id == d.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Id;
        }

        public TimeSpan GetDuration()
        {
            return mEndDate.Subtract(mStartDate);
        }
    }
}
