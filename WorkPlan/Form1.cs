﻿using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace WorkPlan
{
    public partial class Form1 : Form
    {
        EmployeeListView employeesView;
        ScheduleView scheduleView;

        public Form1()
        {
            
            InitializeComponent();

            EmployeeRepository employeeRepo = new EmployeeRepository();
            employeesView = new EmployeeListView();
            employeesView.SetEmployees(employeeRepo.All());

            scheduleView = new ScheduleView();

            employeesView.Dock = DockStyle.Fill;
            employeesView.Name = "uc";
            tabContainer.TabPages[1].Controls.Add(employeesView);

            scheduleView.Dock = DockStyle.Fill;
            scheduleView.Name = "uc";
            tabContainer.TabPages[0].Controls.Add(scheduleView);
            
            //stampaToolStripMenuItem.Enabled = false;
        }
        
        private void esciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void plannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ScheduleView scheduleView = new ScheduleView();
            //ShowChild(scheduleView);
        }

        private void stampaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var uc = (PrintableUC)tabContainer.SelectedTab.Controls["uc"];
            //uc.Print();
        }

        private void dipendentiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //EmployeeRepository employeeRepo = new EmployeeRepository();
            //EmployeeListView employeesView = new EmployeeListView();
            //employeesView.SetEmployees(employeeRepo.All());

            //ShowChild(employeesView);
        }

        private void tabContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabContainer.SelectedIndex == 0)
            {
                var scheduleView = (ScheduleView)tabContainer.SelectedTab.Controls["uc"];
                scheduleView.UpdateData();
            }
        }

        private void causaliAssenzaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlgCausAss = new DlgCausAss();
            dlgCausAss.ShowDialog(this);
        }
    }
}
