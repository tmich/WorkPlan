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
    public partial class DlgChooseMonth : Form
    {
        public DlgChooseMonth()
        {
            InitializeComponent();
        }

        public int ChosenMonth
        {
            get
            {
                return cbMonths.SelectedIndex;
            }
            set
            {
                cbMonths.SelectedIndex = value;
            }
        }
    }
}
