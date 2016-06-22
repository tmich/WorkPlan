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
    public partial class DlgDuty : Form
    {
        protected Employee mEmployee;
        protected DateTime mDutyDate;

        //protected List<string> mPositions = new List<string>
        //{
        //    "Cassa",
        //    "Banco",
        //    "Sala"
        //};

        public DateTime DutyStart
        {
            get
            {
                return new DateTime(dtPickerStart.Value.Year,
                    dtPickerStart.Value.Month,
                    dtPickerStart.Value.Day,
                    tmPickerStart.Value.Hour,
                    tmPickerStart.Value.Minute,
                    0);
            }
        }

        public DateTime DutyEnd
        {
            get
            {
                return new DateTime(dtPickerEnd.Value.Year,
                    dtPickerEnd.Value.Month,
                    dtPickerEnd.Value.Day,
                    tmPickerEnd.Value.Hour,
                    tmPickerEnd.Value.Minute,
                    0);
            }
        }

        public string Position
        {
            get
            {
                return cbPositions.Text;
            }
        }

        public string Notes
        {
            get
            {
                return txNotes.Text;
            }
        }

        protected DlgDuty()
        {
            InitializeComponent();

            PositionDao posdao = new PositionDao();
            cbPositions.Items.AddRange(posdao.GetAll().ToArray());
            //cbPositions.Items.AddRange(mPositions.ToArray());

            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            dtPickerStart.Value = tmPickerStart.Value = now;
            dtPickerEnd.Value = tmPickerEnd.Value = now.AddHours(1);
        }

        public DlgDuty(Employee employee, DateTime dutyDate)
            :this()
        {
            mEmployee = employee;
            mDutyDate = dutyDate;
            dtPickerStart.Value = dtPickerEnd.Value = mDutyDate;
            Text = string.Format("Nuovo turno per {0}", mEmployee.FullName);

            cbPositions.SelectedIndex = FindPosition(employee.DefaultPosition);
        }

        private int FindPosition(Position pos)
        {
            for(int i =0; i< cbPositions.Items.Count; i++)
            {
                var pit = cbPositions.Items[i] as Position;
                if (pit.Id == pos.Id)
                    return i;
            }

            return 0;
        }

        public DlgDuty(DutyVM duty)
            :this()
        {
            mEmployee = duty.Employee;
            dtPickerStart.Value = tmPickerStart.Value = duty.StartDate;
            dtPickerEnd.Value = tmPickerEnd.Value = duty.EndDate;
            cbPositions.SelectedIndex = cbPositions.FindStringExact(duty.Position);
            txNotes.Text = duty.Notes;
            Text = string.Format("Modifica turno per {0}", mEmployee.FullName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this, message, "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void DlgDuty_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                e.Cancel = true;

                if (DutyEnd <= DutyStart)
                {
                    ShowError("La data finale deve essere successiva all'inizio");
                    tmPickerEnd.Focus();
                    return;
                }

                if (cbPositions.SelectedIndex < 0)
                {
                    ShowError("Selezionare una posizione");
                    cbPositions.Focus();
                    return;
                }

                e.Cancel = false;
            }
        }

        private void dtPickerStart_ValueChanged(object sender, EventArgs e)
        {
            dtPickerEnd.Value = dtPickerStart.Value;
        }
    }
}
