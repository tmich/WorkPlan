using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private List<NoWorkReason> lstCausaliAssenza;
        //private List<TimeSpan> lstLavorate;
        TimeSpan orePattuite;
        DutyRepository dutyRepo;
        NoWorkRepository noRepo;
        int firstCellHeight = 40;
        int firstCellWidth = 140;
        int cellWidth = 60;
        int cellHeight = 20;
        int marginLeft = 50;

        internal void DrawFooter(Graphics g, int x, int y, int width, int height)
        {
            // Piè di pagina
            var cellFooter = new Rectangle(x, y, width, height);
            g.DrawString(string.Format("Data stampa: {0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToShortTimeString()),
                drawFont, Brushes.Black, cellFooter, rightAlignedFormat);
        }
        

        int marginTop = 10;
        TimeSpan oreLavorate, oreTotaliGiustificate;
        Font drawFont, boldFont, italicFont, smallBoldFont, smallItalicFont;
        Dictionary<string, TimeSpan> totals;
        SolidBrush drawBrush;
        Pen blackPen;

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

            // Create font and brush.
            smallBoldFont = new Font("Arial", 8, FontStyle.Bold);
            drawFont = new Font("Arial", 9);
            boldFont = new Font("Arial", 9, FontStyle.Bold);
            italicFont = new Font("Arial", 9, FontStyle.Italic);
            smallItalicFont = new Font("Arial", 8, FontStyle.Italic);

            drawBrush = new SolidBrush(Color.Black);
            blackPen = new Pen(Color.Black);
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

        public Dictionary<string, TimeSpan> Totals
        {
            get
            {
                return totals;
            }
        }

        public void Calculate()
        {
            m_days = new List<MonthReportDay>();
            lstCausaliAssenza = new List<NoWorkReason>();
            assenzeMensili = new List<NoWork>();
            dutyRepo = new DutyRepository();
            noRepo = new NoWorkRepository();
            totals = new Dictionary<string, TimeSpan>();

            // inizializzo la lista delle causali assenza
            NoWorkRepository repo = new NoWorkRepository();
            lstCausaliAssenza = repo.GetReasons();
            lstCausaliAssenza.Sort(delegate (NoWorkReason r1, NoWorkReason r2) { return r1.Id.CompareTo(r2.Id); });

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

                // totali
                TimeSpan ts;
                if(!totals.TryGetValue("Ore Lav.", out ts))
                {
                    totals.Add("Ore Lav.", new TimeSpan());
                }
                totals["Ore Lav."] = ts.Add(oreLavorate);

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

                    // totali
                    TimeSpan ts2;
                    if(!totals.TryGetValue(ass.Reason.Value, out ts2))
                    {
                        totals.Add(ass.Reason.Value, new TimeSpan());
                    }
                    
                    totals[ass.Reason.Value] = ts2.Add(ass.FullDay ? orePattuite : ass.GetDuration());
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

        private Rectangle CreateCell(int x, int y, int width, int height)
        {
            return new Rectangle(x, y, width, height);
        }

        private string FormatTimeSpan(TimeSpan ts, bool signed = false)
        {
            if (ts.CompareTo(TimeSpan.Zero) == 0)
            {
                return "";
            }
            else
            {
                string dd = ts.ToString("%d");
                string hh = ts.ToString("%h");
                int h = (int.Parse(dd) * 24) + int.Parse(hh);
                string mm = ts.ToString("mm");
                string segno = "";
                if (signed)
                {
                    if (ts.CompareTo(TimeSpan.Zero) != 0)
                    {
                        segno = ts.CompareTo(TimeSpan.Zero) > 0 ? "+" : "-";
                    }
                }

                return string.Format("{0}{1}:{2}", segno, h, mm);
            }
        }

        private void NewLine(ref int x, ref int y)
        {
            x = marginLeft;
            y += cellHeight;
        }

        public void Draw(Graphics g)
        {
            // Inizio
            int y = marginTop;
            int x = marginLeft;
            
            // nominativo
            RectangleF r1 = new RectangleF(x, y, 600.0F, cellHeight);
            g.DrawString(m_employee.FullName, drawFont, drawBrush, r1);

            // intestazioni celle
            NewLine(ref x, ref y);

            Rectangle r2 = CreateCell(x, y, firstCellWidth);
            g.DrawRectangle(blackPen, r2);
            g.DrawString(StartDate.ToString("MMMM yyy"), smallBoldFont, drawBrush, r2, centerAlignedFormat);
            x += firstCellWidth;

            Rectangle r3 = CreateCell(x, y);
            g.DrawRectangle(blackPen, r3);
            g.DrawString("Ore lav.", smallBoldFont, drawBrush, r3, centerAlignedFormat);
            x += cellWidth;

            foreach (var item in lstCausaliAssenza)
            {
                Rectangle r = CreateCell(x, y);
                g.DrawString(item.Value, item.Code.Equals("RIP") ? smallItalicFont : smallBoldFont, drawBrush, r, centerAlignedFormat);
                g.DrawRectangle(blackPen, r);
                x += cellWidth;
            }

            foreach (var d in m_days)
            {
                NewLine(ref x, ref y);

                // (1) data
                Rectangle drawRect = CreateCell(x, y, firstCellWidth);
                g.DrawRectangle(blackPen, drawRect);
                g.DrawString(d.Date.ToString("dddd dd/MM/yy"), drawFont, drawBrush, drawRect, rightAlignedFormat);

                // (2) ore lavorate
                x += firstCellWidth;
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
                foreach(var reason in lstCausaliAssenza)
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
                    g.DrawString(s, reason.Code.Equals("RIP") ? italicFont : drawFont, drawBrush, r, centerAlignedFormat);
                    g.DrawRectangle(blackPen, r);
                }

                // (4) segnalazione se non c'è neanche una giustificazione
                if(assenze.Count == 0 && tsDuty.CompareTo(TimeSpan.Zero) == 0)
                {
                    x += cellWidth;
                    Rectangle r = CreateCell(x, y);
                    g.DrawString("←", boldFont, Brushes.Red, r, centerAlignedFormat);
                }
            }

            // totali
            NewLine(ref x, ref y);
            Rectangle t0 = CreateCell(x, y, firstCellWidth);
            g.DrawString("TOTALI", boldFont, drawBrush, t0, rightAlignedFormat);
            g.DrawRectangle(blackPen, t0);

            // ore lavorate
            x += firstCellWidth;
            Rectangle t1 = CreateCell(x, y);
            g.DrawString(FormatTimeSpan(oreLavorate), drawFont, drawBrush, t1, centerAlignedFormat);
            g.DrawRectangle(blackPen, t1);

            // assenze
            foreach (var reason in lstCausaliAssenza)
            {
                x += cellWidth;
                Rectangle t = CreateCell(x, y);
                var filtered = assenzeMensili.FindAll(delegate (NoWork nw) { return nw.Reason.Equals(reason); });
                TimeSpan ts = new TimeSpan();
                foreach (var f in filtered)
                {
                    ts = ts.Add(f.FullDay ? orePattuite : f.GetDuration());
                }
                g.DrawString(FormatTimeSpan(ts), reason.Code.Equals("RIP") ? italicFont : drawFont, drawBrush, t, centerAlignedFormat);
                g.DrawRectangle(blackPen, t);
            }

            // cella del totale
            TimeSpan tsTotale = oreLavorate;
            foreach (var item in assenzeMensili)
            {
                if (item.Reason.Code != "RIP")
                    tsTotale = tsTotale.Add(item.FullDay ? orePattuite : item.GetDuration());
            }
            x += cellWidth;
            Rectangle tot1 = CreateCell(x, y);
            g.DrawString(FormatTimeSpan(tsTotale), boldFont, drawBrush, tot1, rightAlignedFormat);
            g.DrawRectangle(blackPen, tot1);

            // Accordi
            NewLine(ref x, ref y);
            int ggAccordi = 26;
            TimeSpan tsAccordi = new TimeSpan();
            for (int i = 0; i < ggAccordi; i++)
            {
                tsAccordi = tsAccordi.Add(orePattuite);
            }
            Rectangle acc0 = CreateCell(x, y, firstCellWidth);
            g.DrawString("Da accordi", italicFont, drawBrush, acc0, rightAlignedFormat);
            g.DrawRectangle(blackPen, acc0);
            x += firstCellWidth;
            Rectangle acc1 = CreateCell(x, y, cellWidth * 7);
            g.DrawString(FormatTimeSpan(tsAccordi), italicFont, drawBrush, acc1, rightAlignedFormat);
            g.DrawRectangle(blackPen, acc1);

            // differenza
            NewLine(ref x, ref y);
            TimeSpan tsExtra = tsTotale.Subtract(tsAccordi);
            Rectangle d0 = CreateCell(x, y, firstCellWidth);
            g.DrawString("DIFF.", boldFont, drawBrush, d0, rightAlignedFormat);
            g.DrawRectangle(blackPen, d0);
            x += d0.Width;
            Rectangle d1 = CreateCell(x, y, cellWidth * 7);
            g.DrawString(FormatTimeSpan(tsExtra, signed: true), boldFont, drawBrush, d1, rightAlignedFormat);
            g.DrawRectangle(blackPen, d1);

            // Contatori
            NewLine(ref x, ref y);
            NewLine(ref x, ref y);
            Rectangle cn0 = CreateCell(x, y, firstCellWidth + cellWidth);
            g.DrawString(string.Format("Contatori {0}", m_year), boldFont, drawBrush, cn0, leftAlignedFormat);
            g.DrawRectangle(blackPen, cn0);
            x += cn0.Width;

            foreach (var reason in lstCausaliAssenza)
            {
                var absCounter = new AbsenceCounter(reason, m_employee, m_year);
                
                Rectangle cn1 = CreateCell(x, y);
                g.DrawString(FormatTimeSpan(absCounter.Duration), drawFont, drawBrush, cn1, centerAlignedFormat);
                g.DrawRectangle(blackPen, cn1);
                x += cn1.Width;
            }
        }

        internal void DrawTotalsHeaders(Graphics g, int x, int y)
        {
            // intestazioni celle:
            //      nominativo
            Rectangle r2 = CreateCell(x, y, 250);
            g.DrawRectangle(blackPen, r2);
            g.DrawString("Nominativo", smallBoldFont, drawBrush, r2, centerAlignedFormat);

            //      ore lavorate
            x += r2.Width;
            Rectangle r3 = CreateCell(x, y);
            g.DrawRectangle(blackPen, r3);
            g.DrawString("Ore lav.", smallBoldFont, drawBrush, r3, centerAlignedFormat);
            x += r3.Width;

            //      assenze
            foreach (var item in lstCausaliAssenza)
            {
                Rectangle r = CreateCell(x, y);
                g.DrawString(item.Value, item.Code.Equals("RIP") ? smallItalicFont : smallBoldFont, drawBrush, r, centerAlignedFormat);
                g.DrawRectangle(blackPen, r);
                x += r.Width;
            }

            NewLine(ref x, ref y);
        }

        internal void DrawTotals(Graphics g, int x, int y)
        {
            // DATI
            //      nominativo
            Rectangle rNom = CreateCell(x, y, 250);
            g.DrawRectangle(blackPen, rNom);
            g.DrawString(m_employee.FullName.Length > 20 ? m_employee.FullName.Substring(0, 20) : m_employee.FullName, smallBoldFont, drawBrush, rNom, leftAlignedFormat);
            x += rNom.Width;

            //      ore lavorate
            Rectangle t1 = CreateCell(x, y);
            g.DrawString(FormatTimeSpan(oreLavorate), drawFont, drawBrush, t1, centerAlignedFormat);
            g.DrawRectangle(blackPen, t1);
            x += t1.Width;

            // assenze
            foreach (var reason in lstCausaliAssenza)
            {    
                Rectangle t = CreateCell(x, y);
                var filtered = assenzeMensili.FindAll(delegate (NoWork nw) { return nw.Reason.Equals(reason); });
                TimeSpan ts = new TimeSpan();
                foreach (var f in filtered)
                {
                    ts = ts.Add(f.FullDay ? orePattuite : f.GetDuration());
                }
                g.DrawString(FormatTimeSpan(ts), reason.Code.Equals("RIP") ? italicFont : drawFont, drawBrush, t, centerAlignedFormat);
                g.DrawRectangle(blackPen, t);
                x += t.Width;
            }
        }
    }
}
