using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class NoWork : IWorkPeriod
    {
        public int Id { get; set; }

        private NoWorkReason mReason;
        public NoWorkReason Reason
        {
            get { return mReason; }
            set { mReason = value; }
        }

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
        
        private string mNotes;
        public string Notes
        {
            get { return mNotes; }
            set { mNotes = value; }
        }

        public bool FullDay { get; set; }

        public override string ToString()
        {
            if (FullDay)
            {
                return string.Format("{0} {1}", Reason.Value, "[Giornata Intera]");
            }
            else
            {
                return string.Format("{0} {1}-{2}", Reason.Value, StartDate.ToShortTimeString(), EndDate.ToShortTimeString());
            }
            
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
    }
}
