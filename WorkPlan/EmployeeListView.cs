﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WorkPlan
{
    public partial class EmployeeListView : UserControl
    {
        private List<Employee> mEmployees;

        public EmployeeListView()
        {
            InitializeComponent();
        }

        public void Print()
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
                lvi.SubItems.Add(employee.LastName);
                lvi.SubItems.Add(employee.Name);
                lvi.SubItems.Add(employee.DefaultPosition.Desc);
                if(!employee.HasOpenRelationship(DateTime.Now))
                {
                    lvi.BackColor = System.Drawing.Color.Coral;
                    lvi.SubItems.Add(String.Format("RAPPORTO TERMINATO IL {0}", employee.CurrentRelationship.FiringDate.ToShortDateString()));
                }
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
            emp.DefaultPosition = dlgEmp.EmployeeDefaultPosition;
            emp.BirthCity = dlgEmp.EmployeeBirthCity;
            emp.Nationality = dlgEmp.EmployeeNationality;
            emp.CityDom = dlgEmp.EmployeeCityDom;
            emp.AddressDom = dlgEmp.EmployeeAddressDom;
            emp.MobileNo2 = dlgEmp.EmployeeMobileNo2;

            emp.Relationships = dlgEmp.Relationships;
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

        private void EditEmployee()
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
                EmployeeId = emp.Id,
                EmployeeDefaultPosition = emp.DefaultPosition,
                EmployeeBirthCity = emp.BirthCity,
                EmployeeNationality = emp.Nationality,
                EmployeeCityDom = emp.CityDom,
                EmployeeAddressDom = emp.AddressDom,
                EmployeeMobileNo2 = emp.MobileNo2
            };

            dlgEmp.Relationships = emp.Relationships;

            // leggo il salario e le ore lavorative pattuite
            EconomicsRepository econRepo = new EconomicsRepository();
            decimal salary = econRepo.GetAgreedSalary(emp.Id);
            dlgEmp.EmployeeSalary = salary;

            TimeSpan dailyHh = econRepo.GetDailyHours(emp.Id);
            dlgEmp.EmployeeDailyHours = dailyHh;

            TimeSpan monthlyHh = econRepo.GetMonthlyHours(emp.Id);
            dlgEmp.EmployeeMonthlyHours = monthlyHh;

            dlgEmp.EmployeeHasBusta = econRepo.HasBusta(emp.Id);

            if (dlgEmp.ShowDialog() == DialogResult.OK)
            {
                FromDialog(ref dlgEmp, ref emp);
                EmployeeRepository empRepo = new EmployeeRepository();
                empRepo.Update(emp);

                // scrivo il salario e le ore lavorative pattuite
                econRepo.SetAgreedSalary(emp.Id, dlgEmp.EmployeeSalary);
                if (dlgEmp.EmployeeDailyHours != TimeSpan.Zero)
                {
                    econRepo.SetDailyHours(emp.Id, dlgEmp.EmployeeDailyHours);
                }

                econRepo.SetBusta(emp.Id, dlgEmp.EmployeeHasBusta);

                // update list
                SetEmployees(empRepo.All());
            }
        }

        private void lvEmployees_ItemActivate(object sender, EventArgs e)
        {
            EditEmployee();
        }

        private void tbNuovo_Click(object sender, EventArgs e)
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

        private void tbEdit_Click(object sender, EventArgs e)
        {
            if (lvEmployees.SelectedItems.Count > 0)
            {
                EditEmployee();
            }
        }

        private void DeleteEmployee()
        {
            ListViewItem item = lvEmployees.SelectedItems[0];
            int EmployeeId = (int)item.Tag;
            
            if (GuiUtils.Confirm("Eliminare il dipendente?", this) == DialogResult.Yes)
            {
                EmployeeRepository empRepo = new EmployeeRepository();
                empRepo.Delete(EmployeeId);

                // update list
                SetEmployees(empRepo.All());
            }
        }

        private void tbDelete_Click(object sender, EventArgs e)
        {
            if (lvEmployees.SelectedItems.Count > 0)
            {
                DeleteEmployee();
            }
        }
    }
}
