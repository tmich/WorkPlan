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
    public partial class DlgBusta : Form
    {
        private int mIdDipendente;
        private BustaPaga mBusta;

        private DlgBusta()
        {
            InitializeComponent();

            DateTime now = DateTime.Now;
            cmbAnno.Items.Clear();
            int y = now.Year;
            while (y >= 2016)
            {
                int a = cmbAnno.Items.Add(y--);
            }
            cmbAnno.SelectedIndex = 0;
            cmbMese.SelectedIndex = now.Month - 1;
        }

        public DlgBusta(BustaPaga busta)
            :this(busta.IdDip)
        {
            mBusta = busta;

            txtImporto.Text = mBusta.Importo.ToString("N2");
            cmbAnno.SelectedItem = mBusta.Anno;
            cmbMese.SelectedIndex = mBusta.Mese - 1;
        }

        public DlgBusta(int IdDipendente)
            :this()
        {
            mIdDipendente = IdDipendente;
            
        }
        
        public BustaPaga BustaPaga
        {
            get
            {
                if (mBusta == null)
                {
                    return new BustaPaga(mIdDipendente, Mese, Anno, Importo);
                }
                else
                {
                    return new BustaPaga(mBusta.Id, mBusta.IdDip, Mese, Anno, Importo);
                }
            }
        }

        public int Mese
        {
            get
            {
                return cmbMese.SelectedIndex + 1;
            }
            set
            {
                cmbMese.SelectedIndex = value - 1;
            }
        }

        public int Anno
        {
            get
            {
                return int.Parse(cmbAnno.SelectedItem.ToString());
            }
            set
            {
                cmbAnno.SelectedItem = value.ToString();
            }
        }

        public double Importo
        {
            get
            {
                double imp = 0.00;
                string text = txtImporto.Text;
                text = text.Replace(" €", "");
                double.TryParse(text, out imp);
                return imp;
            }

            set
            {
                txtImporto.Text = value.ToString();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            
        }

        private void DlgBusta_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(DialogResult == DialogResult.OK)
            {
                if (Importo == 0)
                {
                    GuiUtils.Warning("Importo non valido", this);
                    txtImporto.Focus();
                    e.Cancel = true;
                }
            }
        }
    }
}
