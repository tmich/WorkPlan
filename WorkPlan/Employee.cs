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

        public string FullName {
            get { return string.Format("{0} {1}", Name, LastName); } 
        }
    }
}
