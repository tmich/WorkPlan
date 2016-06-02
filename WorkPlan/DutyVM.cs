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
    public class DutyVM : IShiftVM
    {
        public DutyVM(Duty duty)
        {
            Employee = duty.Employee;
            StartDate = duty.StartDate;
            EndDate = duty.EndDate;
            IsFullDay = duty.FullDay;
            Id = duty.Id;
            Notes = duty.Notes;
            Position = duty.Position;
        }

        public string Position { get; set; }

        public Employee Employee { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsFullDay { get; set; }

        public int Id { get; set; }

        public string Notes { get; set; }

        public DateTime StartDate { get; set; }

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
            int height = IsFullDay ? e.CellBounds.Height - (padding * 3) : 32;
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
            if (IsAfternoon)
            {
                brush = Brushes.LightGoldenrodYellow;
            }

            // cassa ha un'evidenza diversa
            if (IsCassa)
            {
                //stringBrush = Brushes.ForestGreen;
                brush = Brushes.SpringGreen;
            }

            e.Graphics.FillRectangle(brush, rect);
            e.Graphics.DrawString(ToString(), e.CellStyle.Font, stringBrush, rect);
            e.Graphics.DrawString(string.Format("\n{0}", Notes.Truncate(20)), e.CellStyle.Font, Brushes.Chocolate, rect.X, rect.Y + 1);
        }

        public bool IsCassa
        {
            get
            {
                return Position.ToLower().Equals("cassa");
            }
        }

        public bool IsMultipleDays
        {
            get
            {
                return false;
            }
        }

        public void Print(PrintPageEventArgs e, Rectangle cell, int totalPerDay, int order = 0)
        {
            Brush bgBrush = Brushes.LightSkyBlue;
            int height = cell.Height;
            int topMargin = cell.Y; //+ (height * order);
            int width = cell.Width / 2;
            int x = cell.X;
            var str = string.Format("{1}-{2}\n{0}", Position, StartDate.ToShortTimeString(), EndDate.ToShortTimeString());

            if (IsCassa)
            {
                bgBrush = Brushes.SpringGreen;
                str = string.Format("{0}-{1}", StartDate.ToShortTimeString(), EndDate.ToShortTimeString());
            }
            else if (IsAfternoon)
            {
                bgBrush = Brushes.LightGoldenrodYellow;
                x += width;
            }

            if (IsFullDay)
            {
                height = cell.Height;
            }

            // allineamento centrato
            StringFormat stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            Font myFont = new Font("Arial", 8.0f, FontStyle.Regular);
            var cellDuty = new Rectangle(x, topMargin, width, height);
            e.Graphics.FillRectangle(bgBrush, cellDuty);
            e.Graphics.DrawRectangle(Pens.Black, cellDuty);
            e.Graphics.DrawString(str, myFont, Brushes.Black, cellDuty, stringFormat);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}-{2}", Position, StartDate.ToShortTimeString(), EndDate.ToShortTimeString());
        }
    }
}
