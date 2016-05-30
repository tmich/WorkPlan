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
    public partial class DlgNowork : Form
    {
        protected Employee mEmployee;
        protected DateTime mDutyDate;
        protected NoWorkRepository nwRepo;

        public DlgNowork()
        {
            InitializeComponent();
            nwRepo = new NoWorkRepository();
            SetReasons(nwRepo.GetReasons());
        }

        public DlgNowork(Employee employee, DateTime dutyDate)
            : this()
        {
            mEmployee = employee;
            mDutyDate = dutyDate;
            dtPickerStart.Value = dtPickerEnd.Value = mDutyDate;
            Text = string.Format("Nuova assenza per {0}", mEmployee.FullName);
        }

        public DlgNowork(NoWork nowork)
            :this()
        {
            mEmployee = nowork.Employee;
            dtPickerStart.Value = tmPickerStart.Value = nowork.StartDate;
            dtPickerEnd.Value = tmPickerEnd.Value = nowork.EndDate;
            cbReasons.SelectedIndex = cbReasons.FindStringExact(nowork.Reason.Value);
            txNotes.Text = nowork.Notes;
            chkFullDay.Checked = nowork.FullDay;
            Text = string.Format("Modifica assenza per {0}", mEmployee.FullName);
        }

        protected void SetReasons(List<NoWorkReason> reasons)
        {
            foreach (var reason in reasons)
            {
                cbReasons.Items.Add(reason);
            }
        }

        public DateTime NoWorkStart
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

        public DateTime NoWorkEnd
        {
            get
            {
                return new DateTime(dtPickerEnd.Value.Year, dtPickerEnd.Value.Month, dtPickerEnd.Value.Day, tmPickerEnd.Value.Hour, tmPickerEnd.Value.Minute, 0);
            }
        }

        public NoWorkReason Reason
        {
            get
            {
                return (NoWorkReason)cbReasons.SelectedItem;
            }
        }

        public string Notes
        {
            get
            {
                return txNotes.Text;
            }
        }

        public bool FullDay
        {
            get
            {
                return chkFullDay.Checked;
            }
        }

        private void chkFullDay_CheckedChanged(object sender, EventArgs e)
        {
            DateTime dtStart = dtPickerStart.Value;
            DateTime dtEnd = dtPickerStart.Value;

            bool enabled = !chkFullDay.Checked;

            dtPickerStart.Enabled = enabled;
            tmPickerStart.Enabled = enabled;
            dtPickerEnd.Enabled = enabled;
            tmPickerEnd.Enabled = enabled;

            if (chkFullDay.Checked)
            {
                dtStart = new DateTime(dtPickerStart.Value.Year, dtPickerStart.Value.Month, dtPickerStart.Value.Day, 0, 0, 0);
                dtEnd = new DateTime(dtPickerStart.Value.Year, dtPickerStart.Value.Month, dtPickerStart.Value.Day, 23, 59, 59);
            }

            dtPickerStart.Value = tmPickerStart.Value = dtStart;
            dtPickerEnd.Value = tmPickerEnd.Value = dtEnd;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this, message, "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void DlgNowork_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                e.Cancel = true;

                if (NoWorkEnd <= NoWorkStart)
                {
                    ShowError("La data finale deve essere successiva all'inizio");
                    tmPickerEnd.Focus();
                    return;
                }

                if (cbReasons.SelectedIndex < 0)
                {
                    ShowError("Selezionare una motivazione");
                    cbReasons.Focus();
                    return;
                }

                e.Cancel = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
