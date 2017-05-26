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
    public partial class DlgChooseEmployee : Form
    {
        private List<Employee> employees;

        private void UpdatePagesNumber()
        {
            lblTotPagine.Text = string.Format("Totale pagine da stampare: {0}", (employees.Count > 0 ? employees.Count.ToString() : "nessuna"));
        }

        public DlgChooseEmployee(DateTime dateRif)
        {
            InitializeComponent();
            employees = new List<Employee>();
            EmployeeRepository repo = new EmployeeRepository();
            foreach(Employee e in repo.All())
            {
                if(e.HasOpenRelationship(dateRif))
                    lstEmployees.Items.Add(e);
            }

            UpdatePagesNumber();
        }

        public List<Employee> GetSelectedEmployees()
        {
            return employees;
        }

        private void lstEmployees_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if(e.NewValue == CheckState.Checked)
            {
                employees.Add((Employee)lstEmployees.Items[e.Index]);
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                employees.Remove((Employee)lstEmployees.Items[e.Index]);
            }

            UpdatePagesNumber();
        }

        private void DlgChooseEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(DialogResult == DialogResult.OK)
            {
                if (employees.Count == 0)
                {
                    MessageBox.Show("Selezionare almeno un dipendente dalla lista!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                }
            }
        }

        private void DlgChooseEmployee_Load(object sender, EventArgs e)
        {

        }
    }
}
