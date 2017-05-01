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
        protected DateTime mStartDate;
        protected DateTime mEndDate;
        protected NoWorkRepository nwRepo;
        //protected DateTime now;// = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

        public DlgNowork()
        {
            InitializeComponent();
            nwRepo = new NoWorkRepository();
            //SetReasons(nwRepo.GetReasons());
            CanSelectFullDay = true;
            //var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            //dtPickerStart.Value = tmPickerStart.Value = now;
            //dtPickerEnd.Value = tmPickerEnd.Value = now.AddHours(1);

            CheckProfiloUtente();
        }

        private void CheckProfiloUtente()
        {
            List<NoWorkReason> reasons = nwRepo.GetReasons();

            if (!User.CurrentUser.IsAdmin())
            {
                reasons.RemoveAll(r => !r.Code.Equals("GEN"));
            }

            SetReasons(reasons);
        }

        public DlgNowork(Employee employee, DateTime dutyDate)
            : this()
        {
            mEmployee = employee;
            mStartDate = new DateTime(dutyDate.Year, dutyDate.Month, dutyDate.Day, DateTime.Now.Hour, 0, 0);
            mEndDate = mStartDate.AddHours(1);
            //dtPickerStart.Value = dtPickerEnd.Value = mDutyDate;
            dtPickerStart.Value = tmPickerStart.Value = mStartDate;
            dtPickerEnd.Value = tmPickerEnd.Value = mStartDate.AddHours(1);
            Text = string.Format("Nuova assenza per {0}", mEmployee.FullName);
        }

        public DlgNowork(NoworkVM nowork)
            :this()
        {
            mEmployee = nowork.Employee;
            mStartDate = new DateTime(nowork.StartDate.Year, nowork.StartDate.Month, nowork.StartDate.Day, DateTime.Now.Hour, 0, 0);
            mEndDate = new DateTime(nowork.EndDate.Year, nowork.EndDate.Month, nowork.EndDate.Day, DateTime.Now.Hour + 1, 0, 0);
            dtPickerStart.Value = tmPickerStart.Value = nowork.StartDate;
            dtPickerEnd.Value = tmPickerEnd.Value = nowork.EndDate;
            cbReasons.SelectedIndex = cbReasons.FindStringExact(nowork.Reason.Value);
            txNotes.Text = nowork.Notes;
            chkFullDay.Checked = nowork.IsFullDay;
            Text = string.Format("Modifica assenza per {0}", mEmployee.FullName);
            lblUsername.Text = string.Format("Inserita da {0}", nowork.User.Username);
        }

        public bool CanSelectFullDay
        {
            get;
            set;
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

        public new DialogResult ShowDialog(IWin32Window owner)
        {
            chkFullDay.Enabled = CanSelectFullDay;

            return base.ShowDialog(owner);
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

            //dtPickerStart.Enabled = enabled;
            tmPickerStart.Enabled = enabled;
            //dtPickerEnd.Enabled = enabled;
            tmPickerEnd.Enabled = enabled;

            if (chkFullDay.Checked)
            {
                dtStart = new DateTime(dtPickerStart.Value.Year, dtPickerStart.Value.Month, dtPickerStart.Value.Day, 0, 0, 0);
                dtEnd = new DateTime(dtPickerEnd.Value.Year, dtPickerEnd.Value.Month, dtPickerEnd.Value.Day, 23, 59, 59);
            }
            else
            {
                dtStart = mStartDate;
                dtEnd = mEndDate;
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
