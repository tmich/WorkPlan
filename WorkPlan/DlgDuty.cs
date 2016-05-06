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

        protected List<string> mPositions = new List<string>
        {
            "Cassa",
            "Banco",
            "Sala"
        };

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

            cbPositions.Items.AddRange(mPositions.ToArray());
        }

        public DlgDuty(Employee employee, DateTime dutyDate)
            :this()
        {
            mEmployee = employee;
            mDutyDate = dutyDate;
            dtPickerStart.Value = dtPickerEnd.Value = mDutyDate;
            Text = string.Format("Nuovo turno per {0}", mEmployee.FullName);
        }

        public DlgDuty(Duty duty)
            :this()
        {
            mEmployee = duty.Employee;
            dtPickerStart.Value = tmPickerStart.Value = duty.StartDate;
            dtPickerEnd.Value = tmPickerEnd.Value = duty.EndDate;
            cbPositions.SelectedIndex = cbPositions.FindStringExact(duty.Position);
            txNotes.Text = duty.Notes;
            Text = string.Format("Modifica turno per {0}", mEmployee.FullName);
        }
    }
}
