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
    public partial class DlgRelationship : Form
    {
        public DlgRelationship()
        {
            InitializeComponent();
        }

        public DlgRelationship(EmploymentRelationship rel)
            : this()
        {
            Id = rel.Id;
            HiringDate = rel.HiringDate;
            FiringDate = rel.FiringDate;
        }

        public long Id
        {
            get
            {
                long id = 0;
                if (long.TryParse(txtId.Text, out id))
                    return id;
                return 0;
            }

            set
            {
                txtId.Text = value.ToString();
            }
        }

        public DateTime HiringDate
        {
            get
            {
                DateTime hDate;
                if (DateTime.TryParse(txtHiringDate.Text, out hDate))
                    return hDate;
                return DateTime.MaxValue;
            }

            set
            {
                txtHiringDate.Text = value.ToShortDateString();
            }
        }

        public DateTime FiringDate
        {
            get
            {
                DateTime fDate;
                if (DateTime.TryParse(txtFiringDate.Text, out fDate))
                    return fDate;
                return DateTime.MaxValue;
            }

            set
            {
                if(value.Year < 9999)
                    txtFiringDate.Text = value.ToShortDateString();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            
        }

        private void DlgRelationship_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (HiringDate > DateTime.Now || HiringDate == DateTime.MaxValue)
                {
                    GuiUtils.Error("Controllare la data inizio rapporto", this, "Errore");
                    e.Cancel = true;
                }

                if(HiringDate >= FiringDate)
                {
                    GuiUtils.Error("Controllare le date", this, "Errore");
                    e.Cancel = true;
                }
            }
        }
    }
}
