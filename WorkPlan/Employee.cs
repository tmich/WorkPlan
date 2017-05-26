using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }
        
        public DateTime HireDate { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public DateTime BirthDate { get; set; }

        public string CodFisc { get; set; }

        public string Telephone { get; set; }

        public string MobileNo { get; set; }

        public string Matr { get; set; }

        public string Qual { get; set; }

        public string Email { get; set; }
        
        public string AddressDom { get; set; }

        public string CityDom { get; set; }

        public string Nationality { get; set; }

        public string BirthCity { get; set; }

        public string MobileNo2 { get; set; }

        //public bool HasBusta { get; set; }

        public Employee()
        {
            DefaultPosition = new Position(0, "");
            Relationships = new List<EmploymentRelationship>();
        }

        public void Fire(DateTime FiringDate)
        {
            CurrentRelationship.FiringDate = FiringDate;
        }

        public void Hire(DateTime HiringDate)
        {
            Relationships.Add(new EmploymentRelationship()
            {
                HiringDate = HiringDate,
                FiringDate = new DateTime(9999, 12, 31)
            });
        }

        public string FullName {
            get { return string.Format("{0} {1}", LastName, Name); } 
        }

        public Position DefaultPosition { get; set; }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Employee p = obj as Employee;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            bool match = (Id == p.Id);
            return match;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return FullName;
        }

        public List<EmploymentRelationship> Relationships { get; set; }

        public EmploymentRelationship CurrentRelationship
        {
            get
            {
                return Relationships.Last();
            }
        }

        public bool HasOpenRelationship(DateTime daterif)
        {
            bool ret = false;

            foreach(var rel in Relationships)
            {
                ret = (daterif >= rel.HiringDate && daterif <= rel.FiringDate);
            }

            return ret;
        }
    }
}
