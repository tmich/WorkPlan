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

        public void Print(PrintPageEventArgs e, int x, int y, int width, Font font, int order = 0)
        {
            var cellDuty = new Rectangle(x, y + (20 * order), width, FullDay ? 60 : 20);
            e.Graphics.DrawRectangle(Pens.Black, cellDuty);
            e.Graphics.DrawString(FullDay ? Reason.Value : ToString(), font, Brushes.Black, cellDuty);
        }

        public override string ToString()
        {
            if (FullDay)
            {
                return string.Format("{0} {1}", Reason.Value, "[Giornata Intera]");
            }
            else
            {
                return string.Format("{0} {1}-{2}", Reason.Value, StartDate.ToShortTimeString(), EndDate.ToShortTimeString());
            }

        }
    }
}
