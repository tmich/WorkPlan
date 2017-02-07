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
            mBusta = new BustaPaga(busta, busta.Id);

            txtImporto.Text = mBusta.Importo.ToString("N2");
            cmbAnno.SelectedItem = mBusta.Anno;
            cmbAnno.Enabled = false;
            cmbMese.SelectedIndex = mBusta.Mese - 1;
            cmbMese.Enabled = false;

            foreach (var vp in mBusta.Voci)
            {
                AddVoce(vp);
            }
        }

        public DlgBusta(int IdDipendente)
            :this()
        {
            mIdDipendente = IdDipendente;
            mBusta = new BustaPaga(mIdDipendente, Mese, Anno, 0);
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
                    //return new BustaPaga(mBusta.Id, mBusta.IdDip, Mese, Anno, Importo);
                    mBusta.Mese = Mese;
                    mBusta.Anno = Anno;
                    return mBusta;
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
                txtImporto.Text = value.ToString("N2");
            }
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
            else
            {
                //foreach (var vp in mBusta.Voci.ToArray())
                //{
                //    if (vp.Id == 0)
                //    {
                //        mBusta.Voci.Remove(vp);
                //    }
                //}
            }
        }

        private void AddVoce(VocePaga vp)
        {
            ListViewItem li = new ListViewItem();
            li.Text = vp.Descrizione;
            li.SubItems.Add(vp.Importo.ToString("N2"));
            li.Tag = vp;

            lvVoci.Items.Add(li);
        }
        

        private void btnNuovaVoce_Click(object sender, EventArgs e)
        {
            var dlgvoce = new DlgVoceBusta();
            if (dlgvoce.ShowDialog() == DialogResult.OK)
            {
                VocePaga vp = new VocePaga(dlgvoce.Descrizione, dlgvoce.Importo);
                mBusta.Voci.Add(vp);
                txtImporto.Text = mBusta.Importo.ToString("N2");

                AddVoce(vp);
            }
        }

        private void lvVoci_DoubleClick(object sender, EventArgs e)
        {
            if (lvVoci.SelectedItems.Count == 0)
            {
                return;
            }

            VocePaga vp = (VocePaga)(lvVoci.SelectedItems[0].Tag);
            var dlgvoce = new DlgVoceBusta(vp);
            if (dlgvoce.ShowDialog() == DialogResult.OK)
            {
                mBusta.Voci.Remove(vp);
                lvVoci.Items.RemoveAt(lvVoci.SelectedIndices[0]);
                
                vp = new VocePaga(dlgvoce.Descrizione, dlgvoce.Importo);
                mBusta.Voci.Add(vp);
                txtImporto.Text = mBusta.Importo.ToString("N2");

                AddVoce(vp);
            }
        }

        private void btnEliminaVoce_Click(object sender, EventArgs e)
        {
            if (lvVoci.SelectedItems.Count == 0)
            {
                return;
            }

            VocePaga vp = (VocePaga)(lvVoci.SelectedItems[0].Tag);
            mBusta.Voci.Remove(vp);
            lvVoci.Items.RemoveAt(lvVoci.SelectedIndices[0]);
            txtImporto.Text = mBusta.Importo.ToString("N2");
        }
    }
}
