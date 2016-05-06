using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkPlan
{
    struct CellTag
    {
        Employee employee;
        DateTime dutyDate;
    }

    public partial class Form1 : Form
    {
        private EmployeeRepository repo;

        public Form1()
        {
            InitializeComponent();
            repo = new EmployeeRepository();
            updateGrid(monthCalendar1.SelectionStart);
        }

        private void esciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            updateGrid(e.Start);
        }

        private void updateGrid(DateTime dateStart)
        {
            List<Employee> employees = repo.All();
            dataGridView1.Columns.Clear();
            for (int i = 0; i < 14; i++)
            {
                int idx = dataGridView1.Columns.Add("", dateStart.ToShortDateString());
                dataGridView1.Columns[idx].Tag = dateStart;
                dateStart = dateStart.AddDays(1);
            }

            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(employees.Count);

            int e= 0;
            foreach (var emp in employees)
            {
                dataGridView1.Rows[e].HeaderCell.Value = emp.FullName;
                dataGridView1.Rows[e++].Tag = emp;
            }
        }

        private void EditDutyAt(int colIndex, int rowIndex)
        {
            if (dataGridView1[colIndex, rowIndex].Tag != null)
            {
                Duty duty = (Duty)dataGridView1[colIndex, rowIndex].Tag;
                var employee = duty.Employee;
                var dlgDuty = new DlgDuty(duty);

                if (dlgDuty.ShowDialog(this) == DialogResult.OK)
                {
                    duty.StartDate = dlgDuty.DutyStart;
                    duty.EndDate = dlgDuty.DutyEnd;
                    duty.Position = dlgDuty.Position;
                    duty.Notes = dlgDuty.Notes;

                    dataGridView1[colIndex, rowIndex].Tag = duty;
                    dataGridView1[colIndex, rowIndex].Value = duty.Position;
                }
            }
            
        }

        private void AddNewDutyAt(int colIndex, int rowIndex)
        {
            var employee = (Employee)dataGridView1.Rows[rowIndex].Tag;
            DateTime dutyDate = (DateTime)dataGridView1.Columns[colIndex].Tag;
            var dlgDuty = new DlgDuty(employee, dutyDate);

            if (dlgDuty.ShowDialog(this) == DialogResult.OK)
            {
                Duty duty = new Duty();
                duty.Employee = employee;
                duty.StartDate = dlgDuty.DutyStart;
                duty.EndDate = dlgDuty.DutyEnd;
                duty.Position = dlgDuty.Position;
                duty.Notes = dlgDuty.Notes;

                dataGridView1[colIndex, rowIndex].Tag = duty;
                dataGridView1[colIndex, rowIndex].Value = dataGridView1[colIndex, rowIndex].Value + 
                    "\n" + duty.Position;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1[e.ColumnIndex, e.RowIndex].Tag == null)
            {
                AddNewDutyAt(e.ColumnIndex, e.RowIndex);
            }
            else
            {
                EditDutyAt(e.ColumnIndex, e.RowIndex);
            }            
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void nuovoTurnoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewDutyAt(dataGridView1.SelectedCells[0].ColumnIndex,
                dataGridView1.SelectedCells[0].RowIndex);
        }

        private void modificaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditDutyAt(dataGridView1.SelectedCells[0].ColumnIndex,
                dataGridView1.SelectedCells[0].RowIndex);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            modificaToolStripMenuItem.Enabled = (dataGridView1.SelectedCells[0].Tag != null);
            eliminaToolStripMenuItem.Enabled = (dataGridView1.SelectedCells[0].Tag != null);
        }
    }
}
