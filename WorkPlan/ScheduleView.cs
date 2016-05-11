using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace WorkPlan
{
    public partial class ScheduleView : PrintableUC
    {
        const int DaysToShow = 7;
        private EmployeeRepository repo;
        private DutyRepository dutyRepo;
        private DutyList duties;
        private DutyService dutyService;
        List<Employee> employees;
        private List<Duty>[,] dutiesToDraw;

        private DateTime startDate, endDate;
             
        public ScheduleView()
        {
            InitializeComponent();
            repo = new EmployeeRepository();
            dutyRepo = new DutyRepository();
            //duties = dutyRepo.All
            employees = repo.All();
            dutiesToDraw = new List<Duty>[DaysToShow, employees.Count];
            dutyService = new DutyService();

            startDate = monthCalendar1.SelectionStart.AddDays(-15);
            endDate = monthCalendar1.SelectionStart.AddDays(15);

            updateGrid(monthCalendar1.SelectionStart);
        }

        #region Printing
        public override void Print()
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

        // The PrintPage event is raised for each page to be printed. 
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            int leftMargin = e.MarginBounds.Left;
            int topMargin = e.MarginBounds.Top;

            int cellWidth = 120;
            int cellHeight = 60;

            int cellHeadHeight = 20;

            DutyService dutyService = new DutyService();
            startDate = monthCalendar1.SelectionStart;
            endDate = startDate.AddDays(DaysToShow);
            Dictionary<Employee, List<Duty>> dutiesMap = dutyService.GetDutiesBy(startDate, endDate);

            using (Font headerPrintFont = new Font("Arial", 9.0f, FontStyle.Bold))
            {
                Font titleFont = new Font("Arial", 14.0f, FontStyle.Bold);
                var title = new Rectangle(leftMargin, topMargin, e.MarginBounds.Width, cellHeight);
                e.Graphics.DrawString(string.Format("Turni dal {0} al {1}", startDate.ToShortDateString(), endDate.ToShortDateString()), titleFont, Brushes.Black, title);
                
                // TESTATA (DATE)
                topMargin += cellHeight;
                leftMargin += cellWidth;
                do
                {
                    var cellHead = new Rectangle(leftMargin, topMargin, cellWidth, cellHeadHeight);
                    e.Graphics.FillRectangle(Brushes.LightGray, cellHead);
                    e.Graphics.DrawRectangle(Pens.Black, cellHead);
                    e.Graphics.DrawString(startDate.ToString("dddd dd/MM/yy"), headerPrintFont, Brushes.Black, cellHead);
                    leftMargin += cellWidth;
                    startDate = startDate.AddDays(1);
                } while (!startDate.Date.Equals(endDate.Date));

                leftMargin = e.MarginBounds.Left;
                topMargin += cellHeadHeight;

                using (Font printFont = new Font("Arial", 9.0f, FontStyle.Regular))
                {
                    foreach (var employee in dutiesMap.Keys)
                    {
                        // RIGA (DIPENDENTE)
                        var cell = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                        e.Graphics.DrawRectangle(Pens.Black, cell);
                        e.Graphics.DrawString(employee.FullName, headerPrintFont, Brushes.Black, cell);
                        leftMargin += cellWidth;

                        var duties = dutiesMap[employee];

                        startDate = monthCalendar1.SelectionStart;

                        while (!startDate.Date.Equals(endDate.Date))
                        {
                            // GIORNI

                            var cellDay = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                            e.Graphics.DrawRectangle(Pens.Black, cellDay);
                            //e.Graphics.DrawString(dd.ToString(), this.Font, Brushes.Black, cell2);

                            var dutiesOfDay = duties.FindAll(delegate (Duty duty)
                                {
                                    return duty.StartDate.Date.Equals(startDate.Date);
                                }
                            );

                            int _d = 0;
                            foreach (var dd in dutiesOfDay)
                            {
                                var cellDuty = new Rectangle(leftMargin, cellDay.Top + (20 * _d), cellWidth, 20);
                                e.Graphics.DrawRectangle(Pens.Black, cellDuty);
                                e.Graphics.DrawString(dd.ToString(), printFont, Brushes.Black, cellDuty);
                                _d++;
                            }

                            startDate = startDate.AddDays(1);
                            leftMargin += cellWidth;
                        }

                        topMargin += cellHeight;
                        leftMargin = e.MarginBounds.Left;
                    }
                }
            }
        }

        // The PrintPage event is raised for each page to be printed. 
        private void pd_PrintPage___(object sender, PrintPageEventArgs e)
        {
            //int yPos = 0;
            
            int leftMargin = e.MarginBounds.Left;
            int topMargin = e.MarginBounds.Top;
            //Image img = Image.FromFile("logo.bmp");
            //Rectangle logo = new Rectangle(40, 40, 200, 100);
            using (Font printFont = new Font("Arial", 10.0f))
            {
                //e.Graphics.DrawImage(img, logo);
                //e.Graphics.DrawString("Testing!", printFont, Brushes.Black, leftMargin, yPos, new StringFormat());

                //testata
                for (int c = -1; c < dataGridView1.Columns.Count; c++)
                {
                    var cell = new Rectangle(leftMargin, topMargin, 100, 30);
                    e.Graphics.DrawRectangle(Pens.Black, cell);
                    //e.Graphics.FillRectangle(Brushes.Azure, cell);
                    if (c >= 0)
                    {
                        e.Graphics.DrawString(dataGridView1.Columns[c].HeaderText, this.Font, Brushes.Black, cell);
                    }
                    else
                    {
                        e.Graphics.DrawString("Dipendente", this.Font, Brushes.Black, cell);
                    }
                    
                    leftMargin += cell.Width + 2;
                }
                leftMargin = e.MarginBounds.Left;

                int i = 0;
                foreach (var employee in repo.All())
                {
                    topMargin += 60;
                    leftMargin += 100;
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        var cell = new Rectangle(leftMargin, topMargin, 100, 60);
                        e.Graphics.DrawRectangle(Pens.Black, cell);
                        if (i == 0)
                        {
                            e.Graphics.DrawString(employee.FullName, this.Font, Brushes.Black, cell);
                        }
                        else
                        {
                            DateTime datetime = (DateTime)column.Tag;
                            var duties = dutyRepo.GetBy(employee, datetime);
                            int d = duties.Count;
                            foreach (Duty duty in duties)
                            {
                                e.Graphics.DrawRectangle(Pens.Black, cell);

                                int spacing = 0;
                                int padding = 0;
                                int height = 30;
                                int X = leftMargin;
                                int Y = topMargin + (height * d) + (spacing * d) + padding;
                                int width = 100 - (spacing * 2);


                                Rectangle rect = new Rectangle(X, Y, width, height);
                                e.Graphics.DrawRectangle(Pens.White, rect);
                                //e.Graphics.FillRectangle(brushes[d], rect);
                                e.Graphics.DrawString(duty.ToString(), this.Font, Brushes.Crimson, rect);
                                e.Graphics.DrawString(string.Format("\n{0}", duty.Notes.Truncate(20)), this.Font, Brushes.BlueViolet, rect.X, rect.Y + 1);

                            }
                        }
                        i++;
                    }
//                    leftMargin += 50;
                    //topMargin += 60;
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
            // get last month
            duties = dutyService.GetBy(startDate, endDate);

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

        private List<Duty> GetDutiesAt(int colIndex, int rowIndex)
        {
            var cell = dataGridView1[colIndex, rowIndex];
            List<Duty> duties = (List<Duty>)cell.Tag;
            return duties;
        }

        private Duty AskUserWhichDuty(List<Duty> duties)
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
            List<Duty> duties = GetDutiesAt(colIndex, rowIndex);
            Duty dutyToDelete;

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

                if (MessageBox.Show(this, string.Format("Eliminare il turno?\n{0}\n{1}", 
                    dutyToDelete.Employee.FullName, dutyToDelete.ToString()), 
                    "Conferma", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    dutyRepo.Delete(dutyToDelete.Id);
                }
            }
        }
        
        private void EditDutyAt(int colIndex, int rowIndex)
        {
            List<Duty> duties = GetDutiesAt(colIndex, rowIndex);
            DlgDuty dlg;
            Duty dutyToEdit = null;

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

                dlg = new DlgDuty(dutyToEdit);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    dutyToEdit.StartDate = dlg.DutyStart;
                    dutyToEdit.EndDate = dlg.DutyEnd;
                    dutyToEdit.Notes = dlg.Notes;
                    dutyToEdit.Position = dlg.Position;

                    dutyRepo.Update(ref dutyToEdit);
                    dataGridView1.Refresh();
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
        
        private void nuovoTurnoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewDutyAt(dataGridView1.SelectedCells[0].ColumnIndex,
                dataGridView1.SelectedCells[0].RowIndex);
        }

        private void modificaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditDutyAt(dataGridView1.SelectedCells[0].ColumnIndex,
                dataGridView1.SelectedCells[0].RowIndex);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            List<Duty> dutiesOfTheDay = (List<Duty>)dataGridView1.SelectedCells[0].Tag;

            modificaToolStripMenuItem.Enabled = (dutiesOfTheDay.Count > 0);
            eliminaToolStripMenuItem.Enabled = (dutiesOfTheDay.Count > 0);
        }

        private void drawDutiesToCell(DataGridViewCellPaintingEventArgs e, List<Duty> duties)
        {
            List<Brush> brushes = new List<Brush>()
            {
                Brushes.AliceBlue
                //Brushes.Beige,
                //Brushes.BlanchedAlmond,
                //Brushes.DimGray
            };

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
                        Duty duty = duties[d];

                        int padding = 5;
                        int spacing = 5;
                        //int width = e.CellBounds.Width - (padding * 2);
                        //int height = (e.CellBounds.Height / duties.Count) - (padding * 2);
                        //int X = e.CellBounds.X + padding;
                        //int Y = (e.CellBounds.Y) + (height * d) + padding;

                        int height = 32;
                        int X = e.CellBounds.X + (2 * padding);
                        int Y = e.CellBounds.Y + (height * d) + (spacing * d) + padding;
                        int width = e.CellBounds.Width - (padding * 3);

                        Pen navyPen = new Pen(Color.Navy, 2);
                        Pen leftBorderPen = new Pen(Color.Navy, 8);
                        //navyPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        Rectangle rect = new Rectangle(X, Y, width, height);
                        e.Graphics.DrawRectangle(navyPen, rect);
                        Point p2 = rect.Location;
                        p2.Offset(0, rect.Height);
                        e.Graphics.DrawLine(leftBorderPen, rect.Location, p2);
                        e.Graphics.FillRectangle(brushes[0], rect);
                        e.Graphics.DrawString(duty.ToString(), e.CellStyle.Font, Brushes.Crimson, rect);
                        e.Graphics.DrawString(string.Format("\n{0}", duty.Notes.Truncate(20)), e.CellStyle.Font, Brushes.BlueViolet, rect.X, rect.Y + 1);
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

                    var duties_ = dutyRepo.GetBy(employee, datetime);
                    //var duties_ = duties.GetBy(employee, datetime);
                    
                    if (duties_.Count > 0)
                    {
                        drawDutiesToCell(e, duties_);
                    }

                    var cell = dataGridView1[e.ColumnIndex, e.RowIndex];
                    cell.Tag = duties_;
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
                List<Duty> duties = (List<Duty>)cell.Tag;

                if (duties.Count > 0)
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

        private void tbPrint_Click(object sender, EventArgs e)
        {
            Print();
        }
    }
}
