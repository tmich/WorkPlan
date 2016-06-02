using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;

namespace WorkPlan
{
    public class SchedulePrint
    {
        protected PrintDocument pd;
        protected IDictionary<Employee, List<IShiftVM>> shiftsCassa;
        protected IDictionary<Employee, List<IShiftVM>> shifts;
        protected DateTime StartDate, EndDate;
        protected int PageNumber = 0;
        protected int PrintedRows = 0;
        protected int RowsPerPage = 12;

        int leftMargin = 0;
        int topMargin = 0;
        int cellWidth = 140;
        int cellHeight = 50;
        int cellHeadHeight = 20;

        // allineamento centrato
        StringFormat centerAlignedFormat = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        // allineamento a sinistra
        StringFormat leftAlignedFormat = new StringFormat()
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center
        };

        public SchedulePrint(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;

            pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(this.PrintPage);
            PaperSize ps = new PaperSize();
            ps.RawKind = (int)PaperKind.A4;

            pd.DefaultPageSettings.PaperSize = ps;
            pd.DefaultPageSettings.Landscape = true;
            pd.DefaultPageSettings.Margins.Top = 20;
            pd.DefaultPageSettings.Margins.Bottom = 20;
            pd.DefaultPageSettings.Margins.Left = 20;
            pd.DefaultPageSettings.Margins.Right = 20;
        }

        public void Print()
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                var pserv = new PeriodService();

                shiftsCassa = pserv.GetCassaByDateRangeDict(StartDate, EndDate);
                shifts = pserv.GetNotCassaByDateRangeDict(StartDate, EndDate);

                PageNumber = 1;
                pd.Print();
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            leftMargin = e.MarginBounds.Left;
            topMargin = e.MarginBounds.Top;
            Graphics g = e.Graphics;

            if (PageNumber == 1)
            {
                PrintTestata(ref e);
                topMargin += cellHeight * 2;
            }

            PrintColonne(ref e);

            if (shiftsCassa.Count > 0)
            {
                g.DrawString(string.Format("Cassa"), new Font("Arial", 14.0f, FontStyle.Bold), Brushes.Black, new Point(leftMargin, topMargin - 30));
                PrintCassa(ref e);
                topMargin += 50;
            }

            if (shifts.Count > 0)
            {
                //PrintColonne(ref e);
                g.DrawString(string.Format("Turni"), new Font("Arial", 14.0f, FontStyle.Bold), Brushes.Black, new Point(leftMargin, topMargin - 30));
                PrintTurni(ref e);
            }
        }

        private void PrintColonne(ref PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            // COLONNE (DATE)
            //topMargin += cellHeight * 2;
            leftMargin += cellWidth;
            var idx_date = StartDate;

            do
            {
                var cellHead = new Rectangle(leftMargin, topMargin, cellWidth, cellHeadHeight);
                g.FillRectangle(Brushes.LightGray, cellHead);
                g.DrawRectangle(Pens.Black, cellHead);
                g.DrawString(idx_date.ToString("dddd dd/MM/yy"), new Font("Arial", 8.0f, FontStyle.Bold), 
                    Brushes.Black, cellHead, centerAlignedFormat);
                leftMargin += cellWidth;
                idx_date = idx_date.AddDays(1);
            } while (!idx_date.Date.Equals(EndDate.Date));

            leftMargin = e.MarginBounds.Left;
            topMargin += cellHeadHeight;

            // matt/ pom
            idx_date = StartDate;
            leftMargin += cellWidth;

            while (!idx_date.Date.Equals(EndDate.Date))
            {
                var cellMat = new Rectangle(leftMargin, topMargin, cellWidth / 2, cellHeadHeight);
                g.FillRectangle(Brushes.LightGray, cellMat);
                g.DrawRectangle(Pens.Black, cellMat);
                g.DrawString("MAT", new Font("Arial", 8.0f, FontStyle.Bold), Brushes.Black, cellMat, centerAlignedFormat);
                leftMargin += cellMat.Width;

                var cellPom = new Rectangle(leftMargin, topMargin, cellWidth / 2, cellHeadHeight);
                g.FillRectangle(Brushes.LightGray, cellPom);
                g.DrawRectangle(Pens.Black, cellPom);
                g.DrawString("POM", new Font("Arial", 8.0f, FontStyle.Bold), Brushes.Black, cellPom, centerAlignedFormat);
                leftMargin += cellPom.Width;

                idx_date = idx_date.AddDays(1);
            }

            leftMargin = e.MarginBounds.Left;
            topMargin += cellHeadHeight;
        }

        private void PrintTestata(ref PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            
            // INTESTAZIONE
            Font titleFont = new Font("Arial", 18.0f, FontStyle.Bold | FontStyle.Underline);
            //Font headerPrintFont = new Font("Arial", 8.0f, FontStyle.Bold);

            var title = new Rectangle(leftMargin, topMargin, e.MarginBounds.Width, cellHeight);
            //g.FillRectangle(Brushes.LightSlateGray, title);
            g.DrawString(string.Format("Situazione turni dal {0} al {1}", 
                StartDate.ToShortDateString(), EndDate.ToShortDateString()), titleFont, Brushes.Black, title);
        }

        private void PrintCassa(ref PrintPageEventArgs e)
        {
            var employees = shiftsCassa.Keys.ToList();
            employees.Sort(new EmployeeFullNameComparer());

            foreach (var employee in employees)
            {
                if (e.HasMorePages)
                    return;

                PrintRow(ref e, employee, shiftsCassa[employee]);
                shiftsCassa.Remove(employee);
            }
        }

        private void PrintTurni(ref PrintPageEventArgs e)
        {
            var employees = shifts.Keys.ToList();
            employees.Sort(new EmployeeFullNameComparer());

            foreach (var employee in employees)
            {
                if(e.HasMorePages)
                    return;

                PrintRow(ref e, employee, shifts[employee]);
                shifts.Remove(employee);
            }
        }

        private void PrintRow(ref PrintPageEventArgs e, Employee employee, List<IShiftVM> duties)
        {
            Graphics g = e.Graphics;

            // RIGA (DIPENDENTE
            var cellRowHead = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
            g.FillRectangle(Brushes.LightGray, cellRowHead);
            g.DrawRectangle(Pens.Black, cellRowHead);
            g.DrawString(employee.FullName, new Font("Arial", 10), Brushes.Black, cellRowHead, leftAlignedFormat);
            leftMargin += cellWidth;
            var innerStartDate = StartDate;

            while (!innerStartDate.Date.Equals(EndDate.Date))
            {
                // GIORNI
                var cellDay = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                g.DrawRectangle(Pens.Black, cellDay);

                // linea divisoria mattina/pomeriggio
                g.DrawLine(Pens.DarkGray, new Point(leftMargin + (cellDay.Width / 2), topMargin), 
                    new Point(leftMargin + (cellDay.Width / 2), topMargin + cellHeight));

                // turni del giorno
                var dutiesOfDay = duties.FindAll(delegate (IShiftVM shift)
                {
                    return shift.StartDate.Date.Equals(innerStartDate.Date) || 
                        (shift.StartDate.Date < innerStartDate.Date && shift.EndDate.Date >= innerStartDate.Date);
                }
                );

                int d = 0;
                foreach (var dd in dutiesOfDay)
                {
                    dd.Print(e, cellDay, dutiesOfDay.Count, d);
                    d++;
                }

                innerStartDate = innerStartDate.AddDays(1);
                leftMargin += cellWidth;
            }

            leftMargin = e.MarginBounds.Left;
            topMargin += cellHeight;
            PrintedRows++;

            if (PrintedRows == RowsPerPage)
            {
                e.HasMorePages = true;
                PageNumber++;
                PrintedRows = 0;
            }
        }
    }
}
