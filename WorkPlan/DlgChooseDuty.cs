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
        protected List<Duty> mDuties;
        protected Duty mSelected;

        protected DlgChooseDuty()
        {
            InitializeComponent();
        }

        public DlgChooseDuty(List<Duty> duties)
            :this()
        {
            mDuties = duties;

            lbTurni.Items.AddRange(duties.ToArray());
            lbTurni.SelectedIndex = 0;
        }

        public Duty GetSelectedDuty()
        {
            return mSelected;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lbTurni.SelectedIndex >= 0)
            {
                mSelected = (Duty)lbTurni.SelectedItem;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
