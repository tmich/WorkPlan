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
    public class DutyVM : IWorkPeriod
    {
        public DutyVM(Duty duty)
        {
            Employee = duty.Employee;
            StartDate = duty.StartDate;
            EndDate = duty.EndDate;
            FullDay = duty.FullDay;
            Id = duty.Id;
            Notes = duty.Notes;
            Position = duty.Position;
        }

        public string Position { get; set; }

        public Employee Employee { get; set; }

        public DateTime EndDate { get; set; }

        public bool FullDay { get; set; }

        public int Id { get; set; }

        public string Notes { get; set; }

        public DateTime StartDate { get; set; }

        public void Draw(DataGridViewCellPaintingEventArgs e, int order = 0)
        {
            int padding = 5;
            int spacing = 5;
            int height = FullDay ? e.CellBounds.Height - (padding * 3) : 32;
            int X = e.CellBounds.X + (2 * padding);
            int Y = e.CellBounds.Y + (height * order) + (spacing * order) + padding;
            int width = e.CellBounds.Width - (padding * 3);
            Pen navyPen = new Pen(Color.Navy, 2);
            Pen leftBorderPen = new Pen(Color.Navy, 8);
            Rectangle rect = new Rectangle(X, Y, width, height);
            e.Graphics.DrawRectangle(navyPen, rect);
            Point p2 = rect.Location;
            p2.Offset(0, rect.Height);
            e.Graphics.DrawLine(leftBorderPen, rect.Location, p2);
            Brush brush = Brushes.AliceBlue;
            Brush stringBrush = Brushes.Black;

            // pomeriggio di un altro colore
            // almeno la metà del turno si svolge dopo mezzogiorno
            var duration = EndDate.TimeOfDay.Subtract(StartDate.TimeOfDay);
            var duration_in_hours = (int)duration.TotalHours;
            if (StartDate.TimeOfDay.Add(new TimeSpan(duration_in_hours / 2, 0, 0)) >= new TimeSpan(12, 0, 0))
            {
                brush = Brushes.LightGoldenrodYellow;
            }

            // cassa ha un'evidenza diversa
            if (Position.ToLower().Equals("cassa"))
            {
                stringBrush = Brushes.ForestGreen;
            }

            e.Graphics.FillRectangle(brush, rect);
            e.Graphics.DrawString(ToString(), e.CellStyle.Font, stringBrush, rect);
            e.Graphics.DrawString(string.Format("\n{0}", Notes.Truncate(20)), e.CellStyle.Font, Brushes.Chocolate, rect.X, rect.Y + 1);
        }

        public void Print(PrintPageEventArgs e, int order = 0)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}-{2}", Position, StartDate.ToShortTimeString(), EndDate.ToShortTimeString());
        }
    }
}
