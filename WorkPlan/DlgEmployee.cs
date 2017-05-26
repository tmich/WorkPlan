using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WorkPlan
{
    public partial class DlgEmployee : Form
    {
        private bool busteCaricate = false;
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

        public List<EmploymentRelationship> Relationships
        {
            get
            {
                List<EmploymentRelationship> relationships = new List<EmploymentRelationship>();
                foreach (var lvi in lvRelationships.Items)
                {
                    var li = (ListViewItem)lvi;

                    EmploymentRelationship rel = (EmploymentRelationship)li.Tag;
                    relationships.Add(rel);
                }

                return relationships;
            }

            set
            {
                lvRelationships.Items.Clear();

                foreach (var rel in value)
                {
                    AddRelationshipToListView(rel);
                }
            }
        }

        private void AddRelationshipToListView(EmploymentRelationship rel)
        {
            ListViewItem lvi = new ListViewItem(rel.Id.ToString());
            lvi.Tag = rel;
            lvi.SubItems.Add(rel.HiringDate.ToShortDateString());
            string sub2 = "IN ESSERE";
            if (!rel.IsOpen())
                sub2 = rel.FiringDate.ToShortDateString();
            lvi.SubItems.Add(sub2);
            lvRelationships.Items.Add(lvi);
        }

        public decimal EmployeeSalary
        {
            get
            {
                return decimal.Parse(txtEmpSalary.Text.Remove(txtEmpSalary.Text.Length - 1));
            }

            set
            {
                if (value == 0)
                {
                    txtEmpSalary.Text = "0000,00";
                }
                else
                {
                    txtEmpSalary.Text = value.ToString();
                }
            }
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
                this.Text = txEmpName.Text + " " + txEmpSurname.Text;
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
                this.Text = txEmpName.Text + " " + txEmpSurname.Text;
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

        public TimeSpan EmployeeDailyHours
        {
            get
            {
                TimeSpan ts = new TimeSpan(0, 0, 0);
                try
                {
                    TimeSpan.TryParse(txtEmpDailyHours.Text, out ts);
                }
                catch (Exception)
                {
                    throw;
                }

                return ts;
            }

            set
            {
                txtEmpDailyHours.Text = string.Format("{0:hh\\:mm}", value);
            }
        }

        public TimeSpan EmployeeMonthlyHours
        {
            get
            {
                string span = txtEmpMonthlyHours.Text;
                TimeSpan ts = new TimeSpan(0, 0, 0);
                try
                {
                    ts = new TimeSpan(int.Parse(span.Split(':')[0]),    // hours
                           int.Parse(span.Split(':')[1]),    // minutes
                           0);          // seconds
                }
                catch (Exception)
                {

                    throw;
                }

                return ts;
            }

            set
            {
                string dd = value.ToString("%d");
                string hh = value.ToString("%h");
                int h = (int.Parse(dd) * 24) + int.Parse(hh);
                string mm = value.ToString("mm");
                txtEmpMonthlyHours.Text = string.Format("{0}:{1}", h, mm);
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

        public bool EmployeeHasBusta
        {
            get
            {
                return chkBusta.Checked;
            }

            set
            {
                chkBusta.Checked = value;
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
                if (txtEmpSalary.Enabled)
                {
                    e.Cancel = !ValidateDecimal(txtEmpSalary, "Valore non corretto"); if (e.Cancel) return;
                }
                e.Cancel = !ValidateTimeSpan(txtEmpDailyHours, "Valore non corretto"); if (e.Cancel) return;
                //e.Cancel = !ValidateTimeSpan(txtEmpMonthlyHours, "Valore non corretto"); if (e.Cancel) return;
            }
        }

        private bool ValidateTimeSpan(TextBoxBase tx, string message = null)
        {
            if (ValidateNonEmpty(tx, message))
            {
                try
                {
                    string span = tx.Text;
                    TimeSpan ts = new TimeSpan(int.Parse(span.Split(':')[0]),    // hours
                           int.Parse(span.Split(':')[1]),    // minutes
                           0);
                }
                catch (FormatException)
                {
                    tx.Focus();

                    if (message != null)
                    {
                        GuiUtils.Warning(message, this);
                    }

                    return false;
                }
                catch (OverflowException)
                {
                    tx.Focus();

                    if (message != null)
                    {
                        GuiUtils.Warning(message, this);
                    }

                    return false;
                }
            }

            return true;
        }

        private bool ValidateDecimal(TextBoxBase tx, string message = null)
        {
            if (ValidateNonEmpty(tx, message))
            {
                try
                {
                    decimal.Parse(txtEmpSalary.Text.Remove(txtEmpSalary.Text.Length - 1));
                }
                catch (FormatException)
                {
                    tx.Focus();

                    if (message != null)
                    {
                        GuiUtils.Warning(message, this);
                    }

                    return false;
                }
            }

            return true;
        }

        private bool ValidateDate(TextBoxBase tx, string message = null)
        {
            if (ValidateNonEmpty(tx, message))
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
                        GuiUtils.Warning(message, this);
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
                    GuiUtils.Warning(message, this);
                }

                return false;
            }

            return true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void chkBusta_CheckedChanged(object sender, EventArgs e)
        {
            //txtEmpSalary.Enabled = !chkBusta.Checked;
        }

        private void CaricaBustePaga()
        {
            lvBuste.Items.Clear();
            EconomicsRepository ecorep = new EconomicsRepository();
            var buste = ecorep.GetBuste(EmployeeId);
            buste.Sort();

            foreach (var b in buste)
            {
                AddToListView(b);
            }

            busteCaricate = true;
        }

        private ListViewItem CreateListViewItem(BustaPaga busta)
        {
            ListViewItem lv = new ListViewItem(busta.ToString());
            lv.SubItems.Add(busta.Importo.ToString("N2"));
            lv.Tag = busta;

            return lv;
        }

        private void AddToListView(BustaPaga busta)
        {
            ListViewItem lv = CreateListViewItem(busta);

            lvBuste.Items.Add(lv);
        }

        private void InsertToListView(BustaPaga busta, int index)
        {
            ListViewItem lv = CreateListViewItem(busta);

            lvBuste.Items.RemoveAt(index);
            lvBuste.Items.Insert(index, lv);
        }

        private void btnAggRetrib_Click(object sender, EventArgs e)
        {
            DlgBusta dlgb = new DlgBusta(EmployeeId);
            if (dlgb.ShowDialog() == DialogResult.OK)
            {
                EconomicsRepository ecorep = new EconomicsRepository();
                BustaPaga busta = dlgb.BustaPaga;
                try
                {
                    ecorep.AddBusta(ref busta);
                    AddToListView(busta);
                }
                catch (Exception exc)
                {
                    GuiUtils.Error(exc.Message, this);
                }

            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {
                if (!busteCaricate)
                {
                    CaricaBustePaga();
                }
            }
        }

        private void btnRimRetrib_Click(object sender, EventArgs e)
        {
            if (lvBuste.SelectedIndices.Count == 1)
            {
                ListViewItem lvitem = lvBuste.SelectedItems[0];
                BustaPaga busta = (BustaPaga)lvitem.Tag;

                if (GuiUtils.Confirm(String.Format("Eliminare {0}?", busta.ToString()), this) == DialogResult.Yes)
                {

                    EconomicsRepository ecorep = new EconomicsRepository();
                    try
                    {
                        ecorep.DeleteBusta(busta);
                        lvBuste.Items.Remove(lvitem);
                    }
                    catch (Exception ex)
                    {
                        GuiUtils.Error(ex.Message, this);
                        throw;
                    }
                }
            }
        }

        private void lvBuste_DoubleClick(object sender, EventArgs e)
        {
            if (lvBuste.SelectedIndices.Count == 1)
            {
                ListViewItem lvitem = lvBuste.SelectedItems[0];
                BustaPaga busta = (BustaPaga)lvitem.Tag;
                BustaPaga busDaMod = new BustaPaga(busta.Id, busta.IdDip, busta.Mese, busta.Anno, busta.Importo);
                foreach (var vp in busta.Voci)
                    busDaMod.Voci.Add(vp);

                DlgBusta dlgb = new DlgBusta(busDaMod);

                if (dlgb.ShowDialog() == DialogResult.OK)
                {
                    EconomicsRepository ecorep = new EconomicsRepository();
                    try
                    {
                        busta = dlgb.BustaPaga;
                        ecorep.UpdateBusta(busta);
                        InsertToListView(busta, lvitem.Index);
                    }
                    catch (Exception ex)
                    {
                        GuiUtils.Error(ex.Message, this);
                        throw;
                    }
                }
            }
        }

        private void btnNewRelationship_Click(object sender, EventArgs e)
        {
            DlgRelationship dlg = new DlgRelationship();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var rel = new EmploymentRelationship()
                {
                    HiringDate = dlg.HiringDate,
                    FiringDate = dlg.FiringDate
                };

                //Relationships.Add(rel);
                AddRelationshipToListView(rel);
            }
        }

        private void lvRelationships_DoubleClick(object sender, EventArgs e)
        {
            if (lvRelationships.SelectedIndices.Count == 1)
            {
                var idx = lvRelationships.SelectedIndices[0];
                ListViewItem lvitem = lvRelationships.SelectedItems[0];
                EmploymentRelationship rel = (EmploymentRelationship)lvitem.Tag;

                DlgRelationship dlg = new DlgRelationship(rel);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    RemoveRelationshipFromListView(idx);

                    rel = new EmploymentRelationship()
                    {
                        Id = dlg.Id,
                        HiringDate = dlg.HiringDate,
                        FiringDate = dlg.FiringDate
                    };

                    AddRelationshipToListView(rel);
                }

            }
        }

        private void RemoveRelationshipFromListView(int idx)
        {
            lvRelationships.Items.RemoveAt(idx);
        }

        private void btnDelRelationship_Click(object sender, EventArgs e)
        {
            if (lvRelationships.SelectedIndices.Count == 1)
            {
                var idx = lvRelationships.SelectedIndices[0];

                if(lvRelationships.Items.Count == 1)
                {
                    GuiUtils.Warning("Impossibile eliminare il rapporto di lavoro", this, "Attenzione");
                    return;
                }
                
                if(GuiUtils.Confirm("Sicuro di voler eliminare il rapporto?", this, "Conferma") == DialogResult.Yes)
                {
                    EmploymentRelationship r = (EmploymentRelationship)lvRelationships.Items[idx].Tag;
                    EmployeeRepository repo = new EmployeeRepository();
                    if (repo.DeleteRelationship(r))
                        RemoveRelationshipFromListView(idx);
                    else
                        GuiUtils.Error("Si è verificato un errore", this);
                }
            }
        }
    }
}
