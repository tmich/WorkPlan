using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WorkPlan
{
    public partial class EmployeeListView : PrintableUC
    {
        private List<Employee> mEmployees;

        public EmployeeListView()
        {
            InitializeComponent();
        }

        public override void Print()
        {
            throw new NotImplementedException();
        }

        public void SetEmployees(List<Employee> employees)
        {
            mEmployees = employees;
            UpdateData();
        }

        protected void UpdateData()
        {
            lvEmployees.Items.Clear();

            foreach (var employee in mEmployees)
            {
                ListViewItem lvi = new ListViewItem(employee.Matr);
                lvi.SubItems.Add(employee.Name);
                lvi.SubItems.Add(employee.LastName);
                //lvi.SubItems.Add(employee.HireDate.ToShortDateString());
                lvi.Tag = employee.Id;
                lvEmployees.Items.Add(lvi);
            }
        }

        private void FromDialog(ref DlgEmployee dlgEmp, ref Employee emp)
        {
            emp.Address = dlgEmp.EmployeeAddress;
            emp.BirthDate = dlgEmp.EmployeeBirthDate;
            emp.City = dlgEmp.EmployeeCity;
            emp.CodFisc = dlgEmp.EmployeeCF;
            emp.Email = dlgEmp.EmployeeEmail;
            emp.HireDate = dlgEmp.EmployeeHireDate;
            emp.LastName = dlgEmp.EmployeeSurname;
            emp.Matr = dlgEmp.EmployeeMatr;
            emp.MobileNo = dlgEmp.EmployeeMobileNo;
            emp.Name = dlgEmp.EmployeeName;
            emp.Qual = dlgEmp.EmployeeQual;
            emp.Telephone = dlgEmp.EmployeeTel;
        }

        private void btNewEmployee_Click(object sender, EventArgs e)
        {
            var dlgEmp = new DlgEmployee();

            if (dlgEmp.ShowDialog() == DialogResult.OK)
            {
                Employee emp = new Employee();
                FromDialog(ref dlgEmp, ref emp);
                EmployeeRepository empRepo = new EmployeeRepository();
                empRepo.Add(ref emp);

                // update list
                SetEmployees(empRepo.All());
            }
        }

        private void lvEmployees_ItemActivate(object sender, EventArgs e)
        {
            ListViewItem item = lvEmployees.SelectedItems[0];
            int id = (int)item.Tag;
            EmployeeRepository repo = new EmployeeRepository();
            Employee emp = repo.GetById(id);

            var dlgEmp = new DlgEmployee()
            {
                EmployeeAddress = emp.Address,
                EmployeeBirthDate = emp.BirthDate,
                EmployeeCity = emp.City,
                EmployeeCF = emp.CodFisc,
                EmployeeEmail = emp.Email,
                EmployeeHireDate = emp.HireDate,
                EmployeeSurname = emp.LastName,
                EmployeeMatr = emp.Matr,
                EmployeeMobileNo = emp.MobileNo,
                EmployeeName = emp.Name,
                EmployeeQual = emp.Qual,
                EmployeeTel = emp.Telephone,
                EmployeeId = emp.Id
            };

            if (dlgEmp.ShowDialog() == DialogResult.OK)
            {
                FromDialog(ref dlgEmp, ref emp);
                EmployeeRepository empRepo = new EmployeeRepository();
                empRepo.Update(emp);

                // update list
                SetEmployees(empRepo.All());
            }
        }
    }
}
