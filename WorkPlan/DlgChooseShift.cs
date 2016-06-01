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
    public partial class DlgChooseShift : Form
    {
        protected List<IShiftVM> mShifts;
        protected IShiftVM mSelected;

        protected DlgChooseShift()
        {
            InitializeComponent();
        }

        public DlgChooseShift(List<IShiftVM> duties)
            :this()
        {
            mShifts = duties;

            lbTurni.Items.AddRange(duties.ToArray());
            lbTurni.SelectedIndex = 0;
        }

        public IShiftVM GetSelectedShift()
        {
            return mSelected;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lbTurni.SelectedIndex >= 0)
            {
                mSelected = (IShiftVM)lbTurni.SelectedItem;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
