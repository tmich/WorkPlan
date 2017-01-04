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
    public partial class DlgChooseMonthYear : Form
    {
        public DlgChooseMonthYear()
        {
            InitializeComponent();

            for(int y=2016;y<=DateTime.Now.Year;y++)
            {
                cbYears.Items.Add(y);
            }

            cbYears.SelectedIndex = cbYears.Items.Count - 1;
        }

        public int ChosenYear
        {
            get
            {
                return (int)cbYears.SelectedItem;
            }
            set
            {
                cbYears.SelectedItem = value;
            }
        }

        public int ChosenMonth
        {
            get
            {
                return cbMonths.SelectedIndex + 1;
            }
            set
            {
                cbMonths.SelectedIndex = value;
            }
        }

        public bool Detail
        {
            get
            {
                return chkDettaglio.Checked;
            }
        }
    }
}
