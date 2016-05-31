using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkPlan
{
    public class NoworkVM : IWorkPeriod
    {
        public NoworkVM(NoWork nowork)
        {
            Employee = nowork.Employee;
            StartDate = nowork.StartDate;
            EndDate = nowork.EndDate;
            FullDay = nowork.FullDay;
            Id = nowork.Id;
            Notes = nowork.Notes;
            Reason = nowork.Reason;
        }

        public Employee Employee { get; set; }

        public DateTime EndDate { get; set; }

        public bool FullDay { get; set; }

        public int Id { get; set; }

        public string Notes { get; set; }

        public DateTime StartDate { get; set; }

        public NoWorkReason Reason { get; set; }

        public bool IsAfternoon
        {
            // almeno la metà del turno si svolge dopo mezzogiorno
            get
            {
                var duration = EndDate.TimeOfDay.Subtract(StartDate.TimeOfDay);
                var duration_in_hours = (int)duration.TotalHours;
                return (StartDate.TimeOfDay.Add(new TimeSpan(duration_in_hours / 2, 0, 0)) >= new TimeSpan(12, 0, 0));
            }
        }

        public void Draw(DataGridViewCellPaintingEventArgs e, int order = 0)
        {
            int padding = 5;
            int spacing = 5;
            int height = FullDay ? e.CellBounds.Height - (padding * 3) : 32;
            int X = e.CellBounds.X + (2 * padding);
            int Y = e.CellBounds.Y + (height * order) + (spacing * order) + padding;
            int width = e.CellBounds.Width - (padding * 3);
            Pen bordPen = new Pen(Color.Crimson, 2);
            Pen leftBorderPen = new Pen(Color.Crimson, 8);
            Rectangle rect = new Rectangle(X, Y, width, height);
            e.Graphics.DrawRectangle(bordPen, rect);
            Point p2 = rect.Location;
            p2.Offset(0, rect.Height);
            e.Graphics.DrawLine(leftBorderPen, rect.Location, p2);
            Brush brush = Brushes.LightPink;
            Brush stringBrush = Brushes.Black;
            
            e.Graphics.FillRectangle(brush, rect);
            e.Graphics.DrawString(ToString(), e.CellStyle.Font, stringBrush, rect);
            e.Graphics.DrawString(string.Format("\n{0}", Notes.Truncate(20)), e.CellStyle.Font, Brushes.Chocolate, rect.X, rect.Y + 1);
        }

        public void Print(PrintPageEventArgs e, Rectangle cell, int totalPerDay, int order = 0)
        {
            int height = cell.Height;// / totalPerDay;
            int topMargin = cell.Y; // + (height) * order;
            int width = cell.Width / 2;
            int x = cell.X;
            var str = string.Format("{1}-{2}\n{0}", Reason.Value, StartDate.ToShortTimeString(), EndDate.ToShortTimeString());

            if (FullDay)
            {
                height = cell.Height;
                str = Reason.Value;
                width = cell.Width;
            }
            else if (IsAfternoon)
            {
                x += width;
            }

            Font myFont = new Font("Arial", 8.0f, FontStyle.Bold);
            var cellDuty = new Rectangle(x, topMargin, width, height);
            e.Graphics.FillRectangle(Brushes.LightCoral, cellDuty);
            e.Graphics.DrawRectangle(Pens.Black, cellDuty);
            e.Graphics.DrawString(str, myFont, Brushes.FloralWhite, cellDuty);
        }

        public override string ToString()
        {
            if (FullDay)
            {
                return string.Format("{0} {1}", Reason.Value, "[Giornata Intera]");
            }
            else
            {
                return string.Format("{0} [{1}-{2}]", Reason.Value, StartDate.ToShortTimeString(), EndDate.ToShortTimeString());
            }

        }
    }
}
