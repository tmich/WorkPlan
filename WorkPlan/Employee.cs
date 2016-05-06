using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class Employee
    {
        private string mName;
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        private string mLastName;
        public string LastName
        {
            get { return mLastName; }
            set { mLastName = value; }
        }

        private DateTime mDateHired;
        public DateTime DateHired
        {
            get { return mDateHired; }
            set { mDateHired = value; }
        }

        public string FullName {
            get { return string.Format("{0} {1}", mName, mLastName); } 
        }
    }
}
