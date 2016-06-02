using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace WorkPlan
{
    public partial class ScheduleView : UserControl
    {
        const int DaysToShow = 7;
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

            //updateGrid(monthCalendar1.SelectionStart);
        }

        #region Printing
        public void Print()
        {
            //PrintDocument pd = new PrintDocument();
            //pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);

            //PrintDialog printDialog = new PrintDialog();
            //printDialog.Document = pd;
            //PaperSize ps = new PaperSize();
            //ps.RawKind = (int)PaperKind.A4;
            //pd.DefaultPageSettings.PaperSize = ps;
            //pd.DefaultPageSettings.Landscape = true;
            //pd.DefaultPageSettings.Margins.Top = 20;
            //pd.DefaultPageSettings.Margins.Bottom = 20;
            //pd.DefaultPageSettings.Margins.Left = 20;
            //pd.DefaultPageSettings.Margins.Right = 20;

            //Show Print Dialog
            //if (printDialog.ShowDialog() == DialogResult.OK)
            //{
            //    //Print the page
            //    //currentPage = 1;
            //    pd.Print();
            //}

            SchedulePrint pd = new SchedulePrint(monthCalendar1.SelectionStart,
                monthCalendar1.SelectionStart.AddDays(DaysToShow));

            pd.Print();
        }

        ////private void printDuties(PrintPageEventArgs e, IDictionary<Employee, List<IWorkPeriod>> dutiesMap, 
        ////    ref int leftMargin, ref int topMargin, int cellWidth, int cellHeight, Font headerPrintFont, Font printFont)
        ////{
        ////    Graphics g = e.Graphics;
        ////    foreach (var employee in dutiesMap.Keys)
        ////    {
        ////        // RIGA (DIPENDENTE)
        ////        var cellRowHead = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        ////        g.FillRectangle(Brushes.LightGray, cellRowHead);
        ////        g.DrawRectangle(Pens.Black, cellRowHead);
        ////        g.DrawString(employee.FullName, new Font("Arial", 10), Brushes.Black, cellRowHead);
        ////        leftMargin += cellWidth;

        ////        var duties = dutiesMap[employee];

        ////        var innerStartDate = monthCalendar1.SelectionStart;

        ////        while (!innerStartDate.Date.Equals(endDate.Date))
        ////        {
        ////            // GIORNI

        ////            var cellDay = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        ////            g.DrawRectangle(Pens.Black, cellDay);

        ////            // linea divisoria mattina/pomeriggio
        ////            g.DrawLine(Pens.DarkGray, new Point(leftMargin + (cellDay.Width / 2), topMargin), new Point(leftMargin + (cellDay.Width / 2), topMargin + cellHeight));

        ////            // turni del giorno
        ////            var dutiesOfDay = duties.FindAll(delegate (IWorkPeriod duty)
        ////            {
        ////                return duty.StartDate.Date.Equals(innerStartDate.Date);
        ////            }
        ////            );

        ////            int _d = 0;
        ////            foreach (var dd in dutiesOfDay)
        ////            {
        ////                dd.Print(e, cellDay, dutiesOfDay.Count, _d);
        ////                _d++;
        ////            }

        ////            innerStartDate = innerStartDate.AddDays(1);
        ////            leftMargin += cellWidth;
        ////        }

        ////        leftMargin = e.MarginBounds.Left;
        ////        topMargin += cellHeight;

        ////        //if (topMargin > e.MarginBounds.Bottom)
        ////        //{
        ////        //    e.HasMorePages = true;
        ////        //    topMargin = e.MarginBounds.Top;
        ////        //}
        ////    }
        ////}

        //private void PrintRow(PrintPageEventArgs e, Employee employee, List<IShiftVM> duties, ref int leftMargin, ref int topMargin, int cellWidth, int cellHeight)
        //{
        //    Graphics g = e.Graphics;

        //    // RIGA (DIPENDENTE)
        //    // allineamento centrato
        //    StringFormat stringFormat = new StringFormat()
        //    {
        //        Alignment = StringAlignment.Near,
        //        LineAlignment = StringAlignment.Center
        //    };
        //    var cellRowHead = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //    g.FillRectangle(Brushes.LightGray, cellRowHead);
        //    g.DrawRectangle(Pens.Black, cellRowHead);
        //    g.DrawString(employee.FullName, new Font("Arial", 10), Brushes.Black, cellRowHead, stringFormat);
        //    leftMargin += cellWidth;

        //    //var duties = dutiesMap[employee];

        //    var innerStartDate = monthCalendar1.SelectionStart;

        //    while (!innerStartDate.Date.Equals(endDate.Date))
        //    {
        //        // GIORNI

        //        var cellDay = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //        g.DrawRectangle(Pens.Black, cellDay);

        //        // linea divisoria mattina/pomeriggio
        //        g.DrawLine(Pens.DarkGray, new Point(leftMargin + (cellDay.Width / 2), topMargin), new Point(leftMargin + (cellDay.Width / 2), topMargin + cellHeight));

        //        // turni del giorno
        //        var dutiesOfDay = duties.FindAll(delegate (IShiftVM shift)
        //        {
        //            //return shift.StartDate.Date.Equals(innerStartDate.Date);
        //            return shift.StartDate.Date.Equals(innerStartDate.Date) || (shift.StartDate.Date < innerStartDate.Date && shift.EndDate.Date >= innerStartDate.Date);
        //        }
        //        );

        //        int _d = 0;
        //        foreach (var dd in dutiesOfDay)
        //        {
        //            dd.Print(e, cellDay, dutiesOfDay.Count, _d);
        //            _d++;
        //        }

        //        innerStartDate = innerStartDate.AddDays(1);
        //        leftMargin += cellWidth;
        //    }

        //    leftMargin = e.MarginBounds.Left;
        //    topMargin += cellHeight;
        //}

        //// The PrintPage event is raised for each page to be printed. 
        //private void pd_PrintPage(object sender, PrintPageEventArgs e)
        //{
        //    Graphics g = e.Graphics;
        //    int leftMargin = e.MarginBounds.Left;
        //    int topMargin = e.MarginBounds.Top;
        //    int cellWidth = 140;
        //    int cellHeight = 32;
        //    int cellHeadHeight = 20;

        //    Font titleFont = new Font("Arial", 14.0f, FontStyle.Bold);

        //    startDate = monthCalendar1.SelectionStart;
        //    endDate = startDate.AddDays(DaysToShow);


        //    // INTESTAZIONE
        //    Font headerPrintFont = new Font("Arial", 8.0f, FontStyle.Bold);

        //    var title = new Rectangle(leftMargin, topMargin, e.MarginBounds.Width, cellHeight);
        //    g.FillRectangle(Brushes.LightSlateGray, title);
        //    g.DrawString(string.Format("Turni dal {0} al {1}", startDate.ToShortDateString(), endDate.ToShortDateString()), titleFont, Brushes.AntiqueWhite, title);

        //    // COLONNE (DATE)
        //    topMargin += cellHeight * 2;
        //    leftMargin += cellWidth;
        //    var idx_date = startDate;

        //    do
        //    {
        //        var cellHead = new Rectangle(leftMargin, topMargin, cellWidth, cellHeadHeight);
        //        g.FillRectangle(Brushes.LightGray, cellHead);
        //        g.DrawRectangle(Pens.Black, cellHead);
        //        g.DrawString(idx_date.ToString("dddd dd/MM/yy"), headerPrintFont, Brushes.Black, cellHead);
        //        leftMargin += cellWidth;
        //        idx_date = idx_date.AddDays(1);
        //    } while (!idx_date.Date.Equals(endDate.Date));

        //    leftMargin = e.MarginBounds.Left;
        //    topMargin += cellHeadHeight;

        //    // matt/ pom
        //    idx_date = startDate;
        //    leftMargin += cellWidth;

        //    while (!idx_date.Date.Equals(endDate.Date))
        //    {
        //        var cellMat = new Rectangle(leftMargin, topMargin, cellWidth / 2, cellHeadHeight);
        //        g.FillRectangle(Brushes.LightGray, cellMat);
        //        g.DrawRectangle(Pens.Black, cellMat);
        //        g.DrawString("MAT", headerPrintFont, Brushes.Black, cellMat);
        //        leftMargin += cellMat.Width;

        //        var cellPom = new Rectangle(leftMargin, topMargin, cellWidth / 2, cellHeadHeight);
        //        g.FillRectangle(Brushes.LightGray, cellPom);
        //        g.DrawRectangle(Pens.Black, cellPom);
        //        g.DrawString("POM", headerPrintFont, Brushes.Black, cellPom);
        //        leftMargin += cellPom.Width;

        //        idx_date = idx_date.AddDays(1);
        //    }

        //    leftMargin = e.MarginBounds.Left;
        //    topMargin += cellHeadHeight;

        //    // prima la cassa
        //    Font printFont = new Font("Arial", 8.0f, FontStyle.Regular);
        //    g.DrawString(string.Format("Cassa"), titleFont, Brushes.Black, new Point(leftMargin, topMargin - 30));

        //    dutyCassa = periodService.GetCassaByDateRangeDict(startDate, endDate);
        //    foreach (var employee in dutyCassa.Keys)
        //    {
        //        PrintRow(e, employee, dutyCassa[employee], ref leftMargin, ref topMargin, cellWidth, cellHeight);
        //    }
        //    //printDuties(e, dutyCassa, ref leftMargin, ref topMargin, cellWidth, cellHeight, headerPrintFont, printFont);
        //    //rowsPrinted += dutyCassa.Count;

        //    topMargin += 50;


        //    // poi gli altri turni
        //    g.DrawString(string.Format("Turni"), titleFont, Brushes.Black, new Point(leftMargin, topMargin - 30));
        //    dutiesMap = periodService.GetNotCassaByDateRangeDict(startDate, endDate);
        //    //printDuties(e, otherDuties, ref leftMargin, ref topMargin, cellWidth, cellHeight, headerPrintFont, printFont);
        //    foreach (var employee in dutiesMap.Keys)
        //    {
        //        PrintRow(e, employee, dutiesMap[employee], ref leftMargin, ref topMargin, cellWidth, cellHeight);
        //        //rowsPrinted++;

        //        //if (rowsPrinted >= 12)
        //        //{
        //        //    //    e.HasMorePages = true;
        //        //    //    currentPage++;
        //        //    //return;
        //        //}
        //    }
        //}

        #endregion

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            startDate = e.Start.AddDays(-15);
            endDate = e.Start.AddDays(15);
            updateGrid(e.Start);
        }

        public void UpdateData()
        {
            employees = repo.All();

            allDuties = periodService.GetByDateRangeDict(startDate, endDate);
            dutyCassa = periodService.GetCassaByDateRangeDict(startDate, endDate);
            dutiesMap = periodService.GetNotCassaByDateRangeDict(startDate, endDate);

            updateGrid(monthCalendar1.SelectionStart);
        }

        private void updateGrid(DateTime dateStart)
        {
            // presenze
            //duties = dutyService.GetBy(startDate, endDate);

            // presenze - assenze
            //shifts = periodService.GetByDateRange(startDate, endDate);

            // create columns
            dataGridView1.Columns.Clear();
            for (int i = 0; i < DaysToShow; i++)
            {
                int idx = dataGridView1.Columns.Add("", dateStart.ToLongDateString());
                dataGridView1.Columns[idx].Tag = dateStart;
                dataGridView1.Columns[idx].Width = 180;
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

            //dataGridView1.Refresh();
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
        
        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    var cell = dataGridView1[e.ColumnIndex, e.RowIndex];
                    
                    Employee employee = (Employee)dataGridView1.Rows[e.RowIndex].Tag;
                    DateTime datetime = (DateTime)dataGridView1.Columns[e.ColumnIndex].Tag;

                    List<IShiftVM> shifts = allDuties[employee].FindAll(delegate (IShiftVM shift)
                    {
                        return shift.StartDate.Date.Equals(datetime.Date) || (shift.StartDate.Date < datetime.Date && shift.EndDate.Date >= datetime.Date);
                    });
                    
                    if (shifts.Count > 0)
                    {
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
            catch(IndexOutOfRangeException)
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
                }

                var shifts = GetShiftsAt(e.ColumnIndex, e.RowIndex);
                if (shifts.Count == 1)
                {
                    if (shifts[0].IsMultipleDays)
                    {
                        dataGridView1.MultiSelect = true;
                        //SelectRangeOfColumns(e.RowIndex, shifts[0].StartDate, shifts[0].EndDate);
                        SelectShift(e.RowIndex, shifts[0]);
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
        

        private void tbPrint_Click(object sender, EventArgs e)
        {
            Print();
        }
    }
}
