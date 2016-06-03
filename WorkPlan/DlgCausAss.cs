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
    public partial class DlgCausAss : Form
    {
        protected NoWorkRepository nwRepo;
        protected bool updating = false;

        public DlgCausAss()
        {
            InitializeComponent();

            nwRepo = new NoWorkRepository();
            foreach (var nwr in nwRepo.GetReasons())
            {
                lbCausali.Items.Add(nwr);
            }
        }

        private void cmdNuovo_Click(object sender, EventArgs e)
        {
            toggle(true);
        }

        private void toggle(bool enabled)
        {
            txtCodice.Enabled = enabled;
            txtDescrizione.Enabled = enabled;
            cmdSalva.Enabled = enabled;
            cmdAnnulla.Enabled = enabled;

            txtId.Text = String.Empty;
            txtCodice.Text = String.Empty;
            txtDescrizione.Text = String.Empty;
        }

        private void cmdAnnulla_Click(object sender, EventArgs e)
        {
            updating = false;
            toggle(false);
        }

        private int findElementById(int id)
        {
            for (int i=0; i<lbCausali.Items.Count; i++)
            {
                if (((NoWorkReason)lbCausali.Items[i]).Id == id)
                {
                    return i;
                }
            }
            return -1;
        }

        private void cmdSalva_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtCodice.Text))
            {
                MessageBox.Show("Inserire un codice", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCodice.Focus();
                return;
            }

            if (String.IsNullOrWhiteSpace(txtDescrizione.Text))
            {
                MessageBox.Show("Inserire una descrizione", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDescrizione.Focus();
                return;
            }

            if (updating)
            {
                int idx = findElementById(int.Parse(txtId.Text));
                if (idx >= 0)
                {
                    NoWorkReason nwr = (NoWorkReason)lbCausali.Items[idx];
                    nwr.Code = txtCodice.Text;
                    nwr.Value = txtDescrizione.Text;

                    try
                    {
                        nwRepo.UpdateReason(ref nwr);

                        lbCausali.Items[idx] = nwr;
                        toggle(false);
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Si è verificato un errore", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        updating = false;
                    }
                }
            }
            else
            {
                NoWorkReason nwr = new NoWorkReason();
                nwr.Code = txtCodice.Text;
                nwr.Value = txtDescrizione.Text;

                try
                {
                    nwRepo.AddReason(ref nwr);
                    lbCausali.Items.Add(nwr);
                    toggle(false);
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Si è verificato un errore", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    updating = false;
                }
            }
        }

        private void lbCausali_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            toggle(true);
            updating = true;
            NoWorkReason nwr = (NoWorkReason)lbCausali.SelectedItem;
            txtId.Text = nwr.Id.ToString();
            txtCodice.Text = nwr.Code;
            txtDescrizione.Text = nwr.Value;
        }
    }
}
