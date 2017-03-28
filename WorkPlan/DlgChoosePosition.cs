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
    public partial class DlgChoosePosition : Form
    {
        private List<Position> repartiSelezionati;

        public DlgChoosePosition()
        {
            InitializeComponent();

            repartiSelezionati = new List<Position>();

            PositionDao posdao = new PositionDao();
            foreach(Position pos in posdao.GetAll())
            {
                lstPositions.Items.Add(pos);
            }
        }

        private void lstPositions_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if(e.NewValue == CheckState.Checked)
            {
                repartiSelezionati.Add((Position)lstPositions.Items[e.Index]);
            }
            else
            {
                repartiSelezionati.Remove((Position)lstPositions.Items[e.Index]);
            }
        }

        public IEnumerable<Position> Positions
        {
            get { return repartiSelezionati; }
        }

        private void DlgChoosePosition_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (repartiSelezionati.Count == 0)
                {
                    MessageBox.Show("Selezionare almeno un reparto dalla lista!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                }
            }
        }
    }
}
