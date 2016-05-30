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
    public partial class DlgChooseDuty : Form
    {
        protected List<IWorkPeriod> mDuties;
        protected IWorkPeriod mSelected;

        protected DlgChooseDuty()
        {
            InitializeComponent();
        }

        public DlgChooseDuty(List<IWorkPeriod> duties)
            :this()
        {
            mDuties = duties;

            lbTurni.Items.AddRange(duties.ToArray());
            lbTurni.SelectedIndex = 0;
        }

        public IWorkPeriod GetSelectedDuty()
        {
            return mSelected;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lbTurni.SelectedIndex >= 0)
            {
                mSelected = (IWorkPeriod)lbTurni.SelectedItem;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
