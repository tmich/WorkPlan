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
        private List<IWorkPeriod> shifts;
        private PeriodService periodService;
        private List<Employee> employees;
        private DateTime startDate, endDate;

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

            updateGrid(monthCalendar1.SelectionStart);
        }

        #region Printing
        public void Print()
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;
            PaperSize ps = new PaperSize();
            ps.RawKind = (int)PaperKind.A4;
            pd.DefaultPageSettings.PaperSize = ps;
            pd.DefaultPageSettings.Landscape = true;
            //Show Print Dialog
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                //Print the page
                pd.Print();
            }

            
        }

        //private void printDutiesCassa(PrintPageEventArgs e)
        //{
        //    DutyService dutyService = new DutyService();
        //    startDate = monthCalendar1.SelectionStart;
        //    endDate = startDate.AddDays(DaysToShow);
        //    var dutiesMap = dutyService.GetDutiesCassaBy(startDate, endDate);

            
        //}

        private void printDuties(PrintPageEventArgs e, IDictionary<Employee, List<IWorkPeriod>> dutiesMap, 
            ref int leftMargin, ref int topMargin, int cellWidth, int cellHeight, Font headerPrintFont, Font printFont)
        {
            foreach (var employee in dutiesMap.Keys)
            {
                // RIGA (DIPENDENTE)
                var cell = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                e.Graphics.DrawRectangle(Pens.Black, cell);
                e.Graphics.DrawString(employee.FullName, headerPrintFont, Brushes.Black, cell);
                leftMargin += cellWidth;

                var duties = dutiesMap[employee];

                var innerStartDate = monthCalendar1.SelectionStart;

                while (!innerStartDate.Date.Equals(endDate.Date))
                {
                    // GIORNI

                    var cellDay = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                    e.Graphics.DrawRectangle(Pens.Black, cellDay);

                    var dutiesOfDay = duties.FindAll(delegate (IWorkPeriod duty)
                    {
                        return duty.StartDate.Date.Equals(innerStartDate.Date);
                    }
                    );

                    int _d = 0;
                    foreach (var dd in dutiesOfDay)
                    {
                        dd.Print(e, leftMargin, cellDay.Top, cellWidth, printFont,  _d);
                        _d++;
                    }

                    innerStartDate = innerStartDate.AddDays(1);
                    leftMargin += cellWidth;
                }

                topMargin += cellHeight;
                leftMargin = e.MarginBounds.Left;
            }
        }

        // The PrintPage event is raised for each page to be printed. 
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            int leftMargin = e.MarginBounds.Left;
            int topMargin = e.MarginBounds.Top;

            int cellWidth = 120;
            int cellHeight = 60;

            int cellHeadHeight = 20;

            //DutyService dutyService = new DutyService();
            startDate = monthCalendar1.SelectionStart;
            endDate = startDate.AddDays(DaysToShow);
            
            using (Font headerPrintFont = new Font("Arial", 9.0f, FontStyle.Bold))
            {
                Font titleFont = new Font("Arial", 14.0f, FontStyle.Bold);
                var title = new Rectangle(leftMargin, topMargin, e.MarginBounds.Width, cellHeight);
                e.Graphics.DrawString(string.Format("Turni dal {0} al {1}", startDate.ToShortDateString(), endDate.ToShortDateString()), titleFont, Brushes.Black, title);
                
                // TESTATA (DATE)
                topMargin += cellHeight;
                leftMargin += cellWidth;
                var idx_date = startDate;
                do
                {
                    var cellHead = new Rectangle(leftMargin, topMargin, cellWidth, cellHeadHeight);
                    e.Graphics.FillRectangle(Brushes.LightGray, cellHead);
                    e.Graphics.DrawRectangle(Pens.Black, cellHead);
                    e.Graphics.DrawString(idx_date.ToString("dddd dd/MM/yy"), headerPrintFont, Brushes.Black, cellHead);
                    leftMargin += cellWidth;
                    idx_date = idx_date.AddDays(1);
                } while (!idx_date.Date.Equals(endDate.Date));

                leftMargin = e.MarginBounds.Left;
                topMargin += cellHeadHeight;

                using (Font printFont = new Font("Arial", 9.0f, FontStyle.Regular))
                {
                    // prima la cassa
                    var dutyCassa = periodService.GetCassaByDateRangeDict(startDate, endDate);
                    printDuties(e, dutyCassa, ref leftMargin, ref topMargin, cellWidth, cellHeight, headerPrintFont, printFont);

                    //e.Graphics.DrawRectangle(Pens.White, new Rectangle(topMargin, 40, 1, 1));
                    topMargin += 50;

                    // poi gli altri turni
                    var otherDuties = periodService.GetNotCassaByDateRangeDict(startDate, endDate);
                    printDuties(e, otherDuties, ref leftMargin, ref topMargin, cellWidth, cellHeight, headerPrintFont, printFont);
                }
            }
        }
        
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
            updateGrid(monthCalendar1.SelectionStart);
        }

        private void updateGrid(DateTime dateStart)
        {
            // presenze
            //duties = dutyService.GetBy(startDate, endDate);

            // presenze - assenze
            shifts = periodService.GetByDateRange(startDate, endDate);

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
        }

        private List<IWorkPeriod> GetDutiesAt(int colIndex, int rowIndex)
        {
            var cell = dataGridView1[colIndex, rowIndex];
            List<IWorkPeriod> duties = (List<IWorkPeriod>)cell.Tag;
            return duties;
        }

        private IWorkPeriod AskUserWhichDuty(List<IWorkPeriod> duties)
        {
            var dlgchoose = new DlgChooseDuty(duties);
            if (dlgchoose.ShowDialog(this) == DialogResult.OK)
            {
                return dlgchoose.GetSelectedDuty();
            }
            else
            {
                return null;
            }
        }

        private void DeleteDutyAt(int colIndex, int rowIndex)
        {
            List<IWorkPeriod> duties = GetDutiesAt(colIndex, rowIndex);
            IWorkPeriod dutyToDelete;

            if (duties.Count > 0)
            {
                dutyToDelete = duties[0];

                if (duties.Count > 1)
                {
                    if ((dutyToDelete = AskUserWhichDuty(duties)) == null)
                    {
                        return;
                    }
                }

                if (MessageBox.Show(this, string.Format("Eliminare il turno?\n{0}\n{1}\n{2}", 
                    dutyToDelete.Employee.FullName, dutyToDelete.StartDate.ToLongDateString(), dutyToDelete.ToString()), 
                    "Conferma", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    dutyRepo.Delete(dutyToDelete.Id);
                }
            }
        }
        
        private void EditDutyAt(int colIndex, int rowIndex)
        {
            List<IWorkPeriod> duties = GetDutiesAt(colIndex, rowIndex);
            IWorkPeriod dutyToEdit = null;

            if (duties.Count > 0)
            {
                dutyToEdit = duties[0];

                if (duties.Count > 1)
                {
                    if ((dutyToEdit = AskUserWhichDuty(duties)) == null)
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
                dataGridView1.Refresh();
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
                dataGridView1.Refresh();
            }
        }

        private void nuovoTurnoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void modificaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditDutyAt(dataGridView1.SelectedCells[0].ColumnIndex,
                dataGridView1.SelectedCells[0].RowIndex);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            List<IWorkPeriod> dutiesOfTheDay = (List<IWorkPeriod>)dataGridView1.SelectedCells[0].Tag;
            if (dutiesOfTheDay.Count > 0)
            {
                nuovoToolStripMenuItem.Enabled = !dutiesOfTheDay[0].FullDay;
            }
            
            modificaToolStripMenuItem.Enabled = (dutiesOfTheDay.Count > 0);
            eliminaToolStripMenuItem.Enabled = (dutiesOfTheDay.Count > 0);
        }

        private void draw(DataGridViewCellPaintingEventArgs e, Duty duty)
        {

        }

        private void draw(DataGridViewCellPaintingEventArgs e, NoWork nowork)
        {

        }

        private void drawPeriods(DataGridViewCellPaintingEventArgs e, List<IWorkPeriod> duties)
        {
            //List<Brush> brushes = new List<Brush>()
            //{
            //    Brushes.AliceBlue,
            //    Brushes.Beige,
            //    Brushes.BlanchedAlmond,
            //    Brushes.LightPink
            //};

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

                    for (int d = 0; d < duties.Count; d++)
                    {
                        // Draw the inset highlight box.
                        IWorkPeriod period = duties[d];
                        period.Draw(e, d);
                    }
                }
            }

            e.Handled = true;
        }

        

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    Employee employee = (Employee)dataGridView1.Rows[e.RowIndex].Tag;
                    DateTime datetime = (DateTime)dataGridView1.Columns[e.ColumnIndex].Tag;

                    //var duties_ = dutyRepo.GetBy(employee, datetime);
                    List<IWorkPeriod> periods = periodService.GetByEmployeeAndDate(employee, datetime);

                    if (periods.Count > 0)
                    {
                        drawPeriods(e, periods);
                    }

                    var cell = dataGridView1[e.ColumnIndex, e.RowIndex];
                    cell.Tag = periods;
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
                List<IWorkPeriod> periods = (List<IWorkPeriod>)cell.Tag;

                if (periods.Count > 0)
                {
                    EditDutyAt(e.ColumnIndex, e.RowIndex);
                }
                else
                {
                    AddNewDutyAt(e.ColumnIndex, e.RowIndex);
                }
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.Button == MouseButtons.Right ||
                    e.Button == MouseButtons.Left)
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Selected = true;
                }
            }
        }

        private void eliminaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteDutyAt(dataGridView1.SelectedCells[0].ColumnIndex,
                dataGridView1.SelectedCells[0].RowIndex);
        }

        private void modificaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            EditDutyAt(dataGridView1.SelectedCells[0].ColumnIndex,
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
