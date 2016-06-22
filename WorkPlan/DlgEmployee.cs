using System;
using System.Windows.Forms;

namespace WorkPlan
{
    public partial class DlgEmployee : Form
    {
        public DlgEmployee()
        {
            InitializeComponent();

            PositionDao posdao = new PositionDao();
            foreach (var item in posdao.GetAll())
            {
                cbReparti.Items.Add(item);
            }
            cbReparti.SelectedIndex = 0;
        }

        public string EmployeeName
        {
            get
            {
                return txEmpName.Text;
            }
            set
            {
                txEmpName.Text = value;
            }
        }

        public string EmployeeSurname
        {
            get
            {
                return txEmpSurname.Text;
            }
            set
            {
                txEmpSurname.Text = value;
            }
        }

        public string EmployeeCF
        {
            get
            {
                return txEmpCF.Text;
            }
            set
            {
                txEmpCF.Text = value;
            }
        }

        public string EmployeeAddress
        {
            get
            {
                return txEmpAddress.Text;
            }
            set
            {
                txEmpAddress.Text = value;
            }
        }

        public string EmployeeMobileNo
        {
            get
            {
                return txEmpMobile.Text;
            }
            set
            {
                txEmpMobile.Text = value;
            }
        }

        public string EmployeeCity
        {
            get
            {
                return txEmpCity.Text;
            }
            set
            {
                txEmpCity.Text = value;
            }
        }

        public string EmployeeEmail
        {
            get
            {
                return txEmpEmail.Text;
            }
            set
            {
                txEmpEmail.Text = value;
            }
        }

        public string EmployeeMatr
        {
            get
            {
                return txEmpMatr.Text;
            }
            set
            {
                txEmpMatr.Text = value;
            }
        }

        public string EmployeeQual
        {
            get
            {
                return txEmpQual.Text;
            }
            set
            {
                txEmpQual.Text = value;
            }
        }

        public DateTime EmployeeHireDate
        {
            get
            {
                return DateTime.Parse(txEmpHireDate.Text);
            }

            set
            {
                txEmpHireDate.Text = value.ToString("dd/MM/yyyy");
            }
        }

        public string EmployeeTel
        {
            get
            {
                return txEmpTel.Text;
            }

            set
            {
                txEmpTel.Text = value;
            }
        }

        public DateTime EmployeeBirthDate
        {
            get
            {
                return DateTime.Parse(txEmpDob.Text);
            }

            set
            {
                txEmpDob.Text = value.ToString("dd/MM/yyyy");
            }
        }

        public int EmployeeId
        {
            get
            {
                int id = 0;
                int.TryParse(txEmpId.Text, out id);
                return id;
            }

            set
            {
                txEmpId.Text = value.ToString();
            }
        }

        public Position EmployeeDefaultPosition
        {
            get
            {
                return (Position)cbReparti.SelectedItem;
            }
            set
            {
                if (value != null)
                {
                    for (int i = 0; i < cbReparti.Items.Count; i++)
                    {
                        if (((Position)cbReparti.Items[i]).Id == value.Id)
                        {
                            cbReparti.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        public string EmployeeAddressDom
        {
            get
            {
                return txEmpAddressDom.Text;
            }
            set
            {
                txEmpAddressDom.Text = value;
            }
        }

        public string EmployeeCityDom
        {
            get
            {
                return txEmpCityDom.Text;
            }
            set
            {
                txEmpCityDom.Text = value;
            }
        }

        public string EmployeeNationality
        {
            get
            {
                return txEmpNationality.Text;
            }
            set
            {
                txEmpNationality.Text = value;
            }
        }

        public string EmployeeBirthCity
        {
            get
            {
                return txEmpLNasc.Text;
            }
            set
            {
                txEmpLNasc.Text = value;
            }
        }

        public string EmployeeMobileNo2
        {
            get
            {
                return txEmpMobile2.Text;
            }
            set
            {
                txEmpMobile2.Text = value;
            }
        }

        private void DlgEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                // validazione
                e.Cancel = !ValidateNonEmpty(txEmpName, "Inserire il nome"); if (e.Cancel) return;
                e.Cancel = !ValidateNonEmpty(txEmpSurname, "Inserire il cognome"); if (e.Cancel) return;
                e.Cancel = !ValidateDate(txEmpDob, "Data di nascita non valida"); if (e.Cancel) return;
                e.Cancel = !ValidateNonEmpty(txEmpCF, "Inserire il codice fiscale"); if (e.Cancel) return;
                e.Cancel = !ValidateNonEmpty(txEmpAddress, "Inserire l'indirizzo"); if (e.Cancel) return;
                e.Cancel = !ValidateNonEmpty(txEmpCity, "Inserire la città"); if (e.Cancel) return;
                e.Cancel = !ValidateNonEmpty(txEmpQual, "Inserire la qualifica"); if (e.Cancel) return;
                e.Cancel = !ValidateDate(txEmpHireDate, "Data di assunzione non valida"); if (e.Cancel) return;
            }
        }

        private bool ValidateDate(TextBoxBase tx, string message = null)
        {
            if(ValidateNonEmpty(tx, message))
            {
                try
                {
                    DateTime.Parse(tx.Text);
                }
                catch (FormatException)
                {
                    tx.Focus();

                    if (message != null)
                    {
                        MessageBox.Show(message, "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    return false;
                }
            }

            return true;
        }

        private bool ValidateNonEmpty(TextBoxBase tx, string message = null)
        {
            if (string.IsNullOrWhiteSpace(tx.Text))
            {
                tx.Focus();

                if (message != null)
                {
                    MessageBox.Show(message, "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                return false;
            }

            return true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
