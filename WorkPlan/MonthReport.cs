using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    class MonthReportDay
    {
        private DutyList presenze;
        private List<NoWork> assenze;
        private DateTime m_dt;

        public MonthReportDay(DateTime date)
        {
            presenze = new DutyList();
            assenze = new List<NoWork>();
            m_dt = date;
        }

        public void Add(Duty duty)
        {
            presenze.Add(duty);
        }

        public void Add(NoWork nowork)
        {
            assenze.Add(nowork);
        }

        public TimeSpan GetDutyTime()
        {
            TimeSpan ts = new TimeSpan();
            foreach (var p in presenze)
            {
                ts = ts.Add(p.GetDuration());
            }

            return ts;
        }

        public DateTime Date
        {
            get
            {
                return m_dt;
            }
        }

        public TimeSpan GetNoWorkTime(NoWorkReason reason)
        {
            TimeSpan ts = new TimeSpan();
            var filtered = assenze.FindAll(delegate (NoWork nw) { return nw.Reason.Id == reason.Id; });
            foreach (var f in filtered)
            {
                ts = ts.Add(f.GetDuration());
            }
            return ts;
        }
        
        public List<NoWork> GetNoWork()
        {
            return assenze;
        }
    }


    public class MonthReport
    {
        private Employee m_employee;
        private int m_month, m_year;
        private List<MonthReportDay> m_days;
        private List<NoWork> assenzeMensili;
        //private Dictionary<NoWorkReason, TimeSpan> dictTotAssenze = new Dictionary<NoWorkReason, TimeSpan>();
        private List<NoWorkReason> lstAssenze;
        //private List<TimeSpan> lstLavorate;
        TimeSpan orePattuite;
        DutyRepository dutyRepo;
        NoWorkRepository noRepo;
        int cellWidth = 80;
        int cellHeight = 20;
        TimeSpan oreLavorate, oreTotaliGiustificate;

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

        // allineamento a destra
        StringFormat rightAlignedFormat = new StringFormat()
        {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Center
        };

        public MonthReport(Employee e, int month, int year)
        {
            m_employee = e;
            m_month = month;
            m_year = year;
        }

        public DateTime StartDate
        {
            get
            {
                return new DateTime(m_year, m_month, 1);
            }
        }

        public DateTime EndDate
        {
            get
            {
                return StartDate.AddMonths(1).Subtract(new TimeSpan(1, 0, 0, 0));
            }
        }

        public void Calculate()
        {
            m_days = new List<MonthReportDay>();
            lstAssenze = new List<NoWorkReason>();
            assenzeMensili = new List<NoWork>();
            dutyRepo = new DutyRepository();
            noRepo = new NoWorkRepository();

            // inizializzo la lista delle causali assenza
            NoWorkRepository repo = new NoWorkRepository();
            lstAssenze = repo.GetReasons();

            //foreach(var nwr in lstAssenze)
            //{
            //    if(nwr.Code != "RIP")
            //        dictTotAssenze.Add(nwr, new TimeSpan());
            //}

            // ore giornaliere pattuite
            EconomicsRepository ecoRepo = new EconomicsRepository();
            orePattuite = ecoRepo.GetDailyHours(m_employee.Id);
            oreLavorate = new TimeSpan();
            oreTotaliGiustificate = new TimeSpan();

            // scorro i giorni
            DateTime dx = StartDate;
            while (dx <= EndDate)
            {
                var day = new MonthReportDay(dx);

                // presenze
                List<Duty> turniGiornalieri = dutyRepo.GetBy(m_employee, dx);
                foreach (var du in turniGiornalieri)
                {
                    day.Add(du);
                    oreLavorate = oreLavorate.Add(du.GetDuration());
                    oreTotaliGiustificate = oreTotaliGiustificate.Add(oreLavorate);
                }

                //assenze
                List<NoWork> assenze = noRepo.GetAssenzeByEmployeeAndDate(m_employee, dx);
                foreach (var ass in assenze)
                {
                    day.Add(ass);
                    assenzeMensili.Add(ass);

                    // il riposo non è conteggiato
                    if (ass.Reason.Code != "RIP")
                    {
                        oreTotaliGiustificate = oreTotaliGiustificate.Add(ass.FullDay ? orePattuite : ass.GetDuration());
                    }
                }

                m_days.Add(day);
                dx = dx.AddDays(1);
            }
        }

        private Rectangle CreateCell(int x, int y)
        {
            return new Rectangle(x, y, cellWidth, cellHeight);
        }

        private Rectangle CreateCell(int x, int y, int width)
        {
            return new Rectangle(x, y, width, cellHeight);
        }

        private string FormatTimeSpan(TimeSpan ts)
        {
            string dd = ts.ToString("%d");
            string hh = ts.ToString("%h");
            int h = (int.Parse(dd) * 24) + int.Parse(hh);
            string mm = ts.ToString("mm");
            return string.Format("{0}:{1}", h, mm);
        }

        public void Draw(Graphics g)
        {
            int y = 0;
            int x = 0;

            // totali
            //TimeSpan totOreGiustificate = new TimeSpan();
            //TimeSpan totOreLavorate = new TimeSpan();

            // Create font and brush.
            Font drawFont = new Font("Arial", 9);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            Pen blackPen = new Pen(Color.Black);
            
            // nominativo
            RectangleF r1 = new RectangleF(x, y, 400.0F, cellHeight);
            g.DrawString(m_employee.FullName, drawFont, drawBrush, r1);

            // intestazioni celle
            y += cellHeight;
            x = 0;

            Rectangle r2 = CreateCell(x, y, 120);
            g.DrawRectangle(blackPen, r2);
            g.DrawString(StartDate.ToString("MMMM yyy"), drawFont, drawBrush, r2);
            x += 120;

            Rectangle r3 = CreateCell(x, y);
            g.DrawRectangle(blackPen, r3);
            g.DrawString("ore lav.", drawFont, drawBrush, r3);
            x += cellWidth;

            foreach (var item in lstAssenze)
            {
                Rectangle r = CreateCell(x, y);
                g.DrawString(item.Value, drawFont, drawBrush, r, centerAlignedFormat);
                g.DrawRectangle(blackPen, r);
                x += cellWidth;
                //dictTotAssenze.Add(item, new TimeSpan());
            }

            foreach (var d in m_days)
            {
                x = 0;
                y += cellHeight;

                // (1) data
                Rectangle drawRect = CreateCell(x, y, 120);
                g.DrawRectangle(blackPen, drawRect);
                g.DrawString(d.Date.ToString("dddd dd/MM/yy"), drawFont, drawBrush, drawRect, rightAlignedFormat);

                // (2) ore lavorate
                x += 120;
                var c1 = CreateCell(x, y);
                TimeSpan tsDuty = d.GetDutyTime();
                string s1 = FormatTimeSpan(tsDuty);
                if (tsDuty.CompareTo(TimeSpan.Zero) == 0)
                {
                    s1 = "";
                }
                g.DrawString(s1, drawFont, drawBrush, c1, centerAlignedFormat);
                g.DrawRectangle(blackPen, c1);
                //totOreLavorate = totOreLavorate.Add(tsDuty);
                //totOreGiustificate = totOreGiustificate.Add(totOreLavorate);

                // (3...) assenze
                var assenze = d.GetNoWork();
                foreach(var reason in lstAssenze)
                {
                    TimeSpan tsAss = new TimeSpan();
                    x += cellWidth;
                    Rectangle r = CreateCell(x, y);
                    var filtered = assenze.FindAll(delegate (NoWork nw) { return nw.Reason.Equals(reason); });
                    foreach (var f in filtered)
                    {
                        tsAss = tsAss.Add(f.FullDay ? orePattuite : f.GetDuration());
                    }
                    
                    String s = FormatTimeSpan(tsAss);
                    if (tsAss.CompareTo(TimeSpan.Zero) == 0)
                    {
                        s = "";
                    }
                    g.DrawString(s, drawFont, drawBrush, r, centerAlignedFormat);
                    g.DrawRectangle(blackPen, r);
                }
            }

            // totali
            x = 0;
            y += cellHeight;
            Rectangle t0 = CreateCell(x, y, 120);
            g.DrawString("TOTALI", drawFont, drawBrush, t0, rightAlignedFormat);
            g.DrawRectangle(blackPen, t0);

            // ore lavorate
            x += 120;
            Rectangle t1 = CreateCell(x, y);
            g.DrawString(FormatTimeSpan(oreLavorate), drawFont, drawBrush, t1, centerAlignedFormat);
            g.DrawRectangle(blackPen, t1);

            // assenze
            foreach (var reason in lstAssenze)
            {
                x += cellWidth;
                Rectangle t = CreateCell(x, y);
                var filtered = assenzeMensili.FindAll(delegate (NoWork nw) { return nw.Reason.Equals(reason); });
                TimeSpan ts = new TimeSpan();
                foreach (var f in filtered)
                {
                    ts = ts.Add(f.FullDay ? orePattuite : f.GetDuration());
                }
                g.DrawString(FormatTimeSpan(ts), drawFont, drawBrush, t, centerAlignedFormat);
                g.DrawRectangle(blackPen, t);
            }

            // differenza
            x = 0;
            y += cellHeight;
            int ggAccordi = 26;
            TimeSpan tsAccordi = new TimeSpan();
            for(int i=0; i < ggAccordi; i++)
            {
                tsAccordi = tsAccordi.Add(orePattuite);
            }
            TimeSpan tsTotale = oreLavorate;
            foreach (var item in assenzeMensili)
            {
                if(item.Reason.Code != "RIP")
                    tsTotale = tsTotale.Add(item.FullDay ? orePattuite : item.GetDuration());
            }

            TimeSpan tsExtra = tsTotale.Subtract(tsAccordi).Duration();
            Rectangle d0 = CreateCell(x, y, 120);
            g.DrawString("DIFF.", drawFont, drawBrush, d0, rightAlignedFormat);
            g.DrawRectangle(blackPen, d0);

            x += 120;
            Rectangle d1 = CreateCell(x, y);
            g.DrawString(FormatTimeSpan(tsExtra), drawFont, drawBrush, d1, centerAlignedFormat);
            g.DrawRectangle(blackPen, d1);
        }
    }
}
