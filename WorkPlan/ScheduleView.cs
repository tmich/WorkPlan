using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Reflection;
using System.Data.SqlClient;

namespace WorkPlan
{
    public partial class ScheduleView : UserControl
    {
        class SelectedCellInfo
        {
            public int RowIndex
            {
                get;
                set;
            }

            public int ColIndex
            {
                get;
                set;
            }

            public SelectedCellInfo() { RowIndex = 0; ColIndex = 0; }
        }

        SelectedCellInfo selectedCellInfo = new SelectedCellInfo();
        int firstDisplayedScrollingRowIndex = 0;
        const int DaysToShow = 7;
        const int ColumnWidth = 170;
        private EmployeeRepository repo;
        private DutyRepository dutyRepo;
        private NoWorkRepository noworkRepo;
        //private List<IWorkPeriod> shifts;
        private PeriodService periodService;
        private List<Employee> employees;
        private DateTime startDate, endDate;

        private IDictionary<Employee, List<IShiftVM>> dutyCassa;
        private IDictionary<Employee, List<IShiftVM>> dutiesMap;
        private IDictionary<Employee, List<IShiftVM>> allDuties;
        //private int rowsPrinted = 0;
        //private int currentPage = 0;

        public ScheduleView()
        {
            InitializeComponent();
            repo = new EmployeeRepository();
            dutyRepo = new DutyRepository();
            noworkRepo = new NoWorkRepository();
            //duties = dutyRepo.All
            employees = repo.All();
            //dutiesToDraw = new List<Duty>[DaysToShow, employees.Count];
            //dutyService = new DutyService();
            periodService = new PeriodService();
            
            startDate = monthCalendar1.SelectionStart.AddDays(-15);
            endDate = monthCalendar1.SelectionStart.AddDays(15);

            UpdateData();

            CreateGrid(monthCalendar1.SelectionStart);
        }
        
        public void Print()
        {
            SchedulePrint pd = new SchedulePrint(monthCalendar1.SelectionStart,
                monthCalendar1.SelectionStart.AddDays(DaysToShow));

            pd.Print();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            startDate = e.Start.AddDays(-15);
            endDate = e.Start.AddDays(15);
            CreateGrid(e.Start);

            UpdateData();
        }

        public void UpdateData()
        {
            try
            {
                employees = repo.All();

                allDuties = periodService.GetByDateRangeDict(startDate, endDate);
                dutyCassa = periodService.GetCassaByDateRangeDict(startDate, endDate);
                dutiesMap = periodService.GetNotCassaByDateRangeDict(startDate, endDate);
            }
            catch (SqlException exc)
            {
                MessageBox.Show(exc.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Dispose();
                Application.Exit();
            }
            catch (Exception)
            {

                throw;
            }
            

            //CreateGrid(monthCalendar1.SelectionStart);
        }

        private void CreateGrid(DateTime dateStart)
        {
            //Set Double buffering on the Grid using reflection and the bindingflags enum.
            //typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic |
            //BindingFlags.Instance | BindingFlags.SetProperty, null,
            //dataGridView1, new object[] { true });

            Type dgvType = dataGridView1.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dataGridView1, true, null);

            // create columns
            dataGridView1.Columns.Clear();
            for (int i = 0; i < DaysToShow; i++)
            {
                int idx = dataGridView1.Columns.Add("", dateStart.ToString("dddd d MMMM"));
                dataGridView1.Columns[idx].Tag = dateStart;
                dataGridView1.Columns[idx].Width = ColumnWidth;
                dataGridView1.Columns[idx].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                dateStart = dateStart.AddDays(1);
            }

            // create rows
            dataGridView1.Rows.Clear();
            if (employees.Count > 0)
            {
                dataGridView1.Rows.Add(employees.Count);
            }

            int e = 0;
            foreach (var emp in employees)
            {
                dataGridView1.Rows[e].HeaderCell.Value = emp.FullName;
                dataGridView1.Rows[e++].Tag = emp;
            }

            // restore selected cell and offset
            dataGridView1.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
            dataGridView1[selectedCellInfo.ColIndex, selectedCellInfo.RowIndex].Selected = true;
        }

        private List<IShiftVM> GetShiftsAt(int colIndex, int rowIndex)
        {
            var cell = dataGridView1[colIndex, rowIndex];
            List<IShiftVM> duties = (List<IShiftVM>)cell.Tag;
            return duties;
        }

        private IShiftVM AskUserWhichShift(List<IShiftVM> shifts)
        {
            var dlgchoose = new DlgChooseShift(shifts);
            if (dlgchoose.ShowDialog(this) == DialogResult.OK)
            {
                return dlgchoose.GetSelectedShift();
            }
            else
            {
                return null;
            }
        }

        private void DeleteShiftAt(int colIndex, int rowIndex)
        {
            List<IShiftVM> shifts = GetShiftsAt(colIndex, rowIndex);
            IShiftVM shiftToDelete;

            if (shifts.Count > 0)
            {
                shiftToDelete = shifts[0];

                if (shifts.Count > 1)
                {
                    if ((shiftToDelete = AskUserWhichShift(shifts)) == null)
                    {
                        return;
                    }
                }

                if (AskUserToDelete(shiftToDelete))
                {
                    periodService.DeleteShift(shiftToDelete);
                    UpdateData();
                    //dataGridView1.InvalidateCell(dataGridView1.CurrentCell);
                    for (int d = 0; d < DaysToShow; d++)
                        dataGridView1.InvalidateCell(d, dataGridView1.CurrentCell.RowIndex);
                }
            }
        }

        private bool AskUserToDelete(IShiftVM shiftToDelete)
        {
            return (MessageBox.Show(this, string.Format("Eliminare?\n\n{0}\n{1}{2}\n{3}",
                    shiftToDelete.Employee.FullName,
                    shiftToDelete.StartDate.ToLongDateString(),
                    (shiftToDelete.IsMultipleDays ? string.Format("-{0}", shiftToDelete.EndDate.ToLongDateString()) : ""),
                    shiftToDelete.ToString()),
                    "Conferma", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
        }
        
        private void EditShiftAt(int colIndex, int rowIndex)
        {
            var cell = dataGridView1[colIndex, rowIndex];
            List<IShiftVM> duties = GetShiftsAt(colIndex, rowIndex);
            IShiftVM dutyToEdit = null;
            
            if (duties.Count > 0)
            {
                dutyToEdit = duties[0];

                if (duties.Count > 1)
                {
                    if ((dutyToEdit = AskUserWhichShift(duties)) == null)
                    {
                        return;
                    }
                }

                DutyVM dutyVM = dutyToEdit as DutyVM;

                if (dutyVM != null)
                {
                    // turno
                    DlgDuty dlg = new DlgDuty(dutyVM);

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        Duty duty = new Duty();
                        duty.Id = dutyVM.Id;
                        duty.StartDate = dlg.DutyStart;
                        duty.EndDate = dlg.DutyEnd;
                        duty.Notes = dlg.Notes;
                        duty.Position = dlg.Position;

                        dutyRepo.Update(ref duty);
                        dataGridView1.Refresh();
                    }
                }
                else
                {
                    // assenza
                    NoworkVM nwvm = dutyToEdit as NoworkVM;
                    DlgNowork dlg = new DlgNowork(nwvm);

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        NoWork nw = new NoWork();
                        nw.Id = nwvm.Id;
                        nw.StartDate = dlg.NoWorkStart;
                        nw.EndDate = dlg.NoWorkEnd;
                        nw.Notes = dlg.Notes;
                        nw.Reason = dlg.Reason;
                        nw.FullDay = dlg.FullDay;

                        noworkRepo.Update(ref nw);
                        dataGridView1.Refresh();
                    }
                }
            }
            
            UpdateData();
            //dataGridView1.InvalidateCell(dataGridView1.CurrentCell);
            //var days = dutyToEdit.EndDate.Subtract(dutyToEdit.StartDate).Days;
            for (int d = 0; d < DaysToShow; d++)
                dataGridView1.InvalidateCell(d, dataGridView1.CurrentCell.RowIndex);
        }

        private void AddNewDutyAt(int colIndex, int rowIndex)
        {
            var employee = (Employee)dataGridView1.Rows[rowIndex].Tag;
            DateTime dutyDate = (DateTime)dataGridView1.Columns[colIndex].Tag;
            var dlgDuty = new DlgDuty(employee, dutyDate);

            if (dlgDuty.ShowDialog(this) == DialogResult.OK)
            {
                Duty duty = new Duty();
                duty.Employee = employee;
                duty.StartDate = dlgDuty.DutyStart;
                duty.EndDate = dlgDuty.DutyEnd;
                duty.Position = dlgDuty.Position;
                duty.Notes = dlgDuty.Notes;
                
                dutyRepo.Add(ref duty);
                UpdateData();

                dataGridView1.InvalidateCell(dataGridView1.CurrentCell);
            }
        }

        private void AddNewNoWorkAt(int colIndex, int rowIndex)
        {
            var employee = (Employee)dataGridView1.Rows[rowIndex].Tag;
            DateTime dutyDate = (DateTime)dataGridView1.Columns[colIndex].Tag;
            var dlgNw = new DlgNowork(employee, dutyDate);
            
            if (dlgNw.ShowDialog(this) == DialogResult.OK)
            {
                NoWork nowork = new NoWork();
                nowork.Employee = employee;
                nowork.StartDate = dlgNw.NoWorkStart;
                nowork.EndDate = dlgNw.NoWorkEnd;
                nowork.Reason = dlgNw.Reason;
                nowork.Notes = dlgNw.Notes;
                nowork.FullDay = dlgNw.FullDay;

                noworkRepo.Add(ref nowork);
                UpdateData();

                var days = nowork.EndDate.Subtract(nowork.StartDate).Days;
                for(int d = dataGridView1.CurrentCell.ColumnIndex; d <= dataGridView1.CurrentCell.ColumnIndex + days; d++)
                    dataGridView1.InvalidateCell(d, dataGridView1.CurrentCell.RowIndex);
            }
        }
        
        private void modificaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditShiftAt(dataGridView1.SelectedCells[0].ColumnIndex,
                dataGridView1.SelectedCells[0].RowIndex);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            List<IShiftVM> dutiesOfTheDay = (List<IShiftVM>)dataGridView1.SelectedCells[0].Tag;
            nuovoToolStripMenuItem.Enabled = true;
            if (dutiesOfTheDay.Count > 0)
            {
                nuovoToolStripMenuItem.Enabled = !dutiesOfTheDay[0].IsFullDay;
            }
            
            modificaToolStripMenuItem.Enabled = (dutiesOfTheDay.Count > 0);
            eliminaToolStripMenuItem.Enabled = (dutiesOfTheDay.Count > 0);
        }
        
        // qui è dove vengono effettivamente disegnati i rettangoli che rappresentano
        // i turni o le assenze
        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (!e.Handled)
                {
                    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                    {
                        var cell = dataGridView1[e.ColumnIndex, e.RowIndex];
                        Debug.WriteLine(string.Format("Painting cell {0}, {1}", e.RowIndex, e.ColumnIndex));
                        Employee employee = (Employee)dataGridView1.Rows[e.RowIndex].Tag;
                        DateTime datetime = (DateTime)dataGridView1.Columns[e.ColumnIndex].Tag;

                        List<IShiftVM> shifts = allDuties[employee].FindAll(delegate (IShiftVM shift)
                        {
                            return shift.StartDate.Date.Equals(datetime.Date) || (shift.StartDate.Date < datetime.Date && shift.EndDate.Date >= datetime.Date);
                        });

                        if (shifts.Count > 0)
                        {
                            shifts.Sort(new ShiftComparer());

                            using (Brush gridBrush = new SolidBrush(this.dataGridView1.GridColor),
                                backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                            {
                                using (Pen gridLinePen = new Pen(gridBrush))
                                {
                                    // Erase the cell.
                                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

                                    if (dataGridView1[e.ColumnIndex, e.RowIndex].Selected)
                                    {
                                        e.PaintBackground(e.CellBounds, true);
                                    }

                                    // Draw the grid lines (only the right and bottom lines;
                                    // DataGridView takes care of the others).
                                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
                                        e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
                                        e.CellBounds.Bottom - 1);
                                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                                        e.CellBounds.Top, e.CellBounds.Right - 1,
                                        e.CellBounds.Bottom);

                                    for (int d = 0; d < shifts.Count; d++)
                                    {
                                        // Draw the inset highlight box.
                                        IShiftVM shift = shifts[d];
                                        shift.Draw(e, d);
                                    }
                                }
                            }
                            e.Handled = true;
                        }

                        cell.Tag = shifts;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                ;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = dataGridView1[e.ColumnIndex, e.RowIndex];
                List<IShiftVM> periods = (List<IShiftVM>)cell.Tag;

                if (periods.Count > 0)
                {
                    EditShiftAt(e.ColumnIndex, e.RowIndex);
                }
                else
                {
                    AddNewDutyAt(e.ColumnIndex, e.RowIndex);
                }                
            }
        }

        private void SelectRangeOfColumns(int RowIndex, DateTime start, DateTime end)
        {
            for (int c = 0; c < DaysToShow; c++)
            {
                var cell = dataGridView1[c, RowIndex];

                if (cell.Tag != null)
                {
                    DateTime dtTag = (DateTime)cell.Tag;

                    if ((dtTag.Date >= start || dtTag.Date <= end))
                    {
                        cell.Selected = true;
                    }
                }
            }
        }

        private void SelectShift(int RowIndex, IShiftVM shift)
        {
            for (int c = 0; c < DaysToShow; c++)
            {
                var cell = dataGridView1[c, RowIndex];

                if (cell.Tag != null)
                {
                    List<IShiftVM> shiftsInCell = (List<IShiftVM>)cell.Tag;

                    if (shiftsInCell.Count == 1 && (shiftsInCell[0] == shift))
                    {
                        cell.Selected = true;
                    }
                }
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1.MultiSelect = false;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.Button == MouseButtons.Right ||
                    e.Button == MouseButtons.Left)
                { 
                    dataGridView1[e.ColumnIndex, e.RowIndex].Selected = true;

                    // store selected cell information and offset
                    selectedCellInfo.RowIndex = e.RowIndex;
                    selectedCellInfo.ColIndex = e.ColumnIndex;
                    firstDisplayedScrollingRowIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
                }

                var shifts = GetShiftsAt(e.ColumnIndex, e.RowIndex);
                if (shifts != null)
                {
                    if (shifts.Count == 1)
                    {
                        if (shifts[0].IsMultipleDays)
                        {
                            dataGridView1.MultiSelect = true;
                            SelectShift(e.RowIndex, shifts[0]);
                        }
                    }
                }
            }
        }

        private void eliminaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteShiftAt(dataGridView1.SelectedCells[0].ColumnIndex,
                dataGridView1.SelectedCells[0].RowIndex);
        }

        private void modificaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            EditShiftAt(dataGridView1.SelectedCells[0].ColumnIndex,
                dataGridView1.SelectedCells[0].RowIndex);
        }

        private void turnoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewDutyAt(dataGridView1.SelectedCells[0].ColumnIndex,
                dataGridView1.SelectedCells[0].RowIndex);
        }

        private void assenzaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewNoWorkAt(dataGridView1.SelectedCells[0].ColumnIndex,
                dataGridView1.SelectedCells[0].RowIndex);
        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DlgChooseMonth d = new DlgChooseMonth();
            d.ChosenMonth = monthCalendar1.SelectionStart.Month - 2;
            var r = d.ShowDialog();
            if(r == DialogResult.OK)
            {
                int month = d.ChosenMonth;
                MonthlyReport report = new MonthlyReport(month);
                report.Print();
            }
        }

        private void tbPrint_Click(object sender, EventArgs e)
        {
            Print();
        }
    }

    public class ShiftComparer : IComparer<IShiftVM>
    {
        public int Compare(IShiftVM x, IShiftVM y)
        {
            return x.StartDate.CompareTo(y.StartDate);
        }
    }
}
