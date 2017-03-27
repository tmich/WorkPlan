using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkPlan
{
    internal class TotaliMensili
    {
        public TotaliMensili(TimeSpan oreLavorate, TimeSpan oreAssenza, int giorniAssenza)
        {
            OreLavorate = oreLavorate;
            OreAssenza = oreAssenza;
            GiorniAssenza = giorniAssenza;
        }

        public TimeSpan OreLavorate { get; private set; }
        public TimeSpan OreAssenza { get; private set; }
        public int GiorniAssenza { get; private set; }
    }

    class MonthlySummaryReport : IMonthlyReport
    {
        protected PrintDocument pd;
        protected int PageNumber = 0;
        protected int PrintedRows = 0;
        protected int RowsPerPage = 16;
        int m_mese, m_anno;
        int firstCellWidth = 400;
        int leftMargin = 50;
        int topMargin = 50;
        int cellWidth = 80;
        int cellHeight = 20;
        Font baseFont = new Font("Arial", 10.0f);

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

        //protected DataTable table;

        public MonthlySummaryReport(int mese, int anno)
        {
            m_mese = mese;
            m_anno = anno;

            //MonthlyReportCmd mrep = new MonthlyReportCmd(m_mese, m_anno);
            
            //DataView dv = mrep.Sintesi.DefaultView;
            //dv.Sort = "cognome";
            //table = dv.ToTable();

            pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintPage);
            PaperSize ps = new PaperSize();
            ps.RawKind = (int)PaperKind.A4;

            pd.DefaultPageSettings.PaperSize = ps;
            pd.DefaultPageSettings.Landscape = false;
            pd.DefaultPageSettings.Margins.Top = 30;
            pd.DefaultPageSettings.Margins.Bottom = 30;
            pd.DefaultPageSettings.Margins.Left = 30;
            pd.DefaultPageSettings.Margins.Right = 30;
        }

        public void Print()
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                PageNumber = 1;
                pd.Print();
            }
        }

        private void PrintHeaders(ref PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            // INTESTAZIONE
            Font titleFont = new Font("Arial", 18.0f, FontStyle.Bold);
            //Font headerPrintFont = new Font("Arial", 8.0f, FontStyle.Bold);

            var title = new Rectangle(leftMargin, topMargin, e.MarginBounds.Width, cellHeight*2);
            //g.FillRectangle(Brushes.LightSlateGray, title);
            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            var FirstDay = new DateTime(m_anno, m_mese, 1);
            var LastDay = new DateTime(m_anno, m_mese, DateTime.DaysInMonth(m_anno, m_mese));

            g.DrawString(string.Format("Resoconto Mensile Presenze dal {0} al {1}",
                FirstDay.ToShortDateString(), LastDay.ToShortDateString()), titleFont, Brushes.Black, title);

            topMargin += (cellHeight * 2);

            // stampa testata della tabella
            var head0 = new Rectangle(leftMargin, topMargin, firstCellWidth, cellHeight);
            g.DrawRectangle(Pens.DarkGray, head0);
            g.FillRectangle(Brushes.LightGray, head0);
            g.DrawString("DIPENDENTE", baseFont, Brushes.Black, head0);
            leftMargin += head0.Width;
            
            var head1 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
            g.DrawRectangle(Pens.DarkGray, head1);
            g.FillRectangle(Brushes.LightGray, head1);
            g.DrawString("HH LAV.", baseFont, Brushes.Black, head1, centerAlignedFormat);
            leftMargin += head1.Width;

            var head2 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
            g.DrawRectangle(Pens.DarkGray, head2);
            g.FillRectangle(Brushes.LightGray, head2);
            g.DrawString("HH ASS.", baseFont, Brushes.Black, head2, centerAlignedFormat);
            leftMargin += head2.Width;

            var head3 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
            g.DrawRectangle(Pens.DarkGray, head3);
            g.FillRectangle(Brushes.LightGray, head3);
            g.DrawString("GG ASS.", baseFont, Brushes.Black, head3, centerAlignedFormat);
            leftMargin += head3.Width;

            //var head4 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
            //g.DrawRectangle(Pens.DarkGray, head4);
            //g.FillRectangle(Brushes.LightGray, head4);
            //g.DrawString("DIFF.", baseFont, Brushes.Black, head4, centerAlignedFormat);

            leftMargin = 50;
            topMargin += cellHeight;
        }

        protected void PrintFooter(ref PrintPageEventArgs e)
        {
            topMargin = e.PageBounds.Height - cellHeight;
            Graphics g = e.Graphics;
            var footer = new Rectangle(leftMargin, topMargin, e.PageBounds.Width, cellHeight);
            g.DrawString(string.Format("Stampato il {0}", DateTime.Now.ToShortDateString()), baseFont, Brushes.Black, footer, centerAlignedFormat);
        }

        public DateTime StartDate()
        {
            return new DateTime(m_anno, m_mese, 1);
        }

        public DateTime EndDate()
        {
            return StartDate().AddMonths(1).Subtract(new TimeSpan(1, 0, 0, 0));
        }

        protected void PrintTotal(Graphics g, Dictionary<Employee, TotaliMensili> total)
        {
            // Stampa
            foreach (var emp in total.Keys)
            {
                TotaliMensili tot = total[emp];

                // nominativo
                var rect = new Rectangle(leftMargin, topMargin, firstCellWidth, cellHeight);
                g.DrawRectangle(Pens.DarkGray, rect);
                g.DrawString(emp.FullName, baseFont, Brushes.Black, rect);
                leftMargin += rect.Width;

                // ore lavorate
                string ol = "";
                var cell1 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                if (tot.OreLavorate.CompareTo(TimeSpan.Zero) != 0)
                {
                    ol = string.Format("{0:00}:{1:00}", Math.Floor(tot.OreLavorate.TotalHours), tot.OreLavorate.Minutes);
                }
                g.DrawRectangle(Pens.DarkGray, cell1);
                g.DrawString(ol, baseFont, Brushes.Black, cell1, centerAlignedFormat);
                leftMargin += cell1.Width;

                // ore di assenza
                string oa = "";
                var cell2 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                if (tot.OreAssenza.CompareTo(TimeSpan.Zero) != 0)
                {
                    oa = string.Format("{0:00}:{1:00}", Math.Floor(tot.OreAssenza.TotalHours), tot.OreAssenza.Minutes);
                }
                g.DrawRectangle(Pens.DarkGray, cell2);
                g.DrawString(oa, baseFont, Brushes.Black, cell2, centerAlignedFormat);
                leftMargin += cell2.Width;

                // gg assenza
                string ga = "";
                var cell3 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                if (tot.GiorniAssenza != 0)
                {
                    ga = tot.GiorniAssenza.ToString();
                }
                g.DrawRectangle(Pens.DarkGray, cell3);
                g.DrawString(ga, baseFont, Brushes.Black, cell3, centerAlignedFormat);

                leftMargin = 50;
                topMargin += cellHeight;
            }
        }

        protected void PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Dictionary<Employee, TotaliMensili> m_totali = new Dictionary<Employee, TotaliMensili>();

            if (PageNumber == 1)
            {
                PrintHeaders(ref e);
            }

            // per tutti i dipendenti...
            EmployeeRepository empRepo = new EmployeeRepository();
            List<Employee> employees = empRepo.All();
            employees.Sort((emp1, emp2) => emp1.FullName.CompareTo(emp2.FullName));
            DutyRepository dutyRepo = new DutyRepository();
            NoWorkRepository noRepo = new NoWorkRepository();
            foreach (Employee emp in employees)
            {
                // Totali MENSILI
                TimeSpan ore_lavorate = new TimeSpan();
                TimeSpan ore_assenza = new TimeSpan();
                int giorni_assenza = 0;

                // per tutti i giorni...
                DateTime dx = StartDate();
                while (dx <= EndDate())
                {
                    // presenze...
                    List<Duty> dutiesOfTheDay = dutyRepo.GetBy(emp, dx);
                    dutiesOfTheDay.ForEach(delegate (Duty d)
                    {
                        ore_lavorate += d.GetDuration();
                    });

                    // hh assenze...
                    List<NoWork> assenze = noRepo.GetAssenzeByEmployeeAndDate(emp, dx);
                    List<NoWork> assenzeHH = assenze.FindAll(delegate (NoWork nw) { return nw.FullDay == false; });
                    assenzeHH.ForEach(delegate (NoWork n)
                    {
                        ore_assenza += n.GetDuration();
                    });

                    // gg. assenza
                    List<NoWork> assenzeGG = assenze.FindAll(delegate (NoWork nw) { return nw.FullDay == true; });
                    giorni_assenza += assenzeGG.Count;
                    
                    dx = dx.AddDays(1);
                }

                // totali mensili
                TotaliMensili tot = new TotaliMensili(ore_lavorate, ore_assenza, giorni_assenza);
                m_totali.Add(emp, tot);
            }

            // stampa dei totali
            PrintTotal(g, m_totali);
        }

        //protected void OLDPrintPage(object sender, PrintPageEventArgs e)
        //{
        //    Graphics g = e.Graphics;

        //    if (PageNumber == 1)
        //    {
        //        PrintHeaders(ref e);
        //    }

        //    EconomicsRepository econ = new EconomicsRepository();
            
        //    foreach (DataRow row in table.Rows)
        //    {
        //        string nominativo = String.Format("{0} {1}", row["cognome"], row["nome"]);

        //        var rect = new Rectangle(leftMargin, topMargin, firstCellWidth, cellHeight);
        //        g.DrawRectangle(Pens.DarkGray, rect);
        //        g.DrawString(nominativo, baseFont, Brushes.Black, rect);
        //        leftMargin += rect.Width;

        //        // ore lavorate
        //        TimeSpan tsOreLavorate = new TimeSpan();
        //        string ol = "";
        //        var cell1 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //        if (!row.IsNull("tot_ore_lavorate"))
        //        {
        //            tsOreLavorate = (TimeSpan)row["tot_ore_lavorate"];
        //            ol= string.Format("{0:00}:{1:00}", Math.Floor(tsOreLavorate.TotalHours), tsOreLavorate.Minutes);
        //        }
        //        g.DrawRectangle(Pens.DarkGray, cell1);
        //        g.DrawString(ol, baseFont, Brushes.Black, cell1, centerAlignedFormat);
        //        leftMargin += cell1.Width;

        //        // ore di assenza
        //        TimeSpan ore_assenza = new TimeSpan();
        //        string oa = "";
        //        var cell2 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //        if (!row.IsNull("ore_assenza"))
        //        {
        //            ore_assenza = (TimeSpan)row["ore_assenza"];
        //            oa = string.Format("{0:00}:{1:00}", Math.Floor(ore_assenza.TotalHours), ore_assenza.Minutes);
        //        }
        //        g.DrawRectangle(Pens.DarkGray, cell2);
        //        g.DrawString(oa, baseFont, Brushes.Black, cell2, centerAlignedFormat);
        //        leftMargin += cell2.Width;

        //        // giorni di assenza
        //        int gg_assenza = int.Parse(row["giorni_assenza"].ToString());
        //        string ga = "";
        //        var cell3 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //        if (gg_assenza != 0)
        //        {
        //            ga = gg_assenza.ToString();
        //        }
        //        g.DrawRectangle(Pens.DarkGray, cell3);
        //        g.DrawString(ga, baseFont, Brushes.Black, cell3, centerAlignedFormat);
        //        //leftMargin += cell3.Width;

        //        // calcolo la differenza
        //        //int dip_id = (int)row["dipendente_id"];
        //        //TimeSpan tsContratto = econ.GetMonthlyHours(dip_id);    // ore mensili da contratto
        //        //TimeSpan tsOreGiornaliere = econ.GetDailyHours(dip_id); // ore giornaliere da contratto
        //        //TimeSpan tsAssenza = ore_assenza;                       // ore di assenza
                
        //        //for (int i = 0; i < gg_assenza; i++)                    // aggiungo i giorni interi di assenza
        //        //{
        //        //    tsAssenza = tsAssenza.Add(tsOreGiornaliere);
        //        //}
                
        //        //TimeSpan tsGiustificato = tsOreLavorate.Add(tsAssenza); // ore giustificate (lavorato + assenza)
        //        //TimeSpan tsDiff = tsGiustificato.Subtract(tsContratto); // differenza tra giustificato e ore da contratto
                
        //        //var cell4 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //        //g.DrawRectangle(Pens.DarkGray, cell4);

        //        //if (TimeSpan.Zero.CompareTo(tsDiff) != 0)
        //        //{
        //        //    bool isNegative = tsDiff < TimeSpan.Zero;

        //        //    string dd = tsDiff.ToString("%d");
        //        //    string hh = tsDiff.ToString("%h");
        //        //    int h = (int.Parse(dd) * 24) + int.Parse(hh);
        //        //    string mm = tsDiff.ToString("mm");
        //        //    string s_diff = string.Format("{0}{1}", (isNegative ? "-" : "+"), string.Format("{0}:{1}", h, mm));
        //        //    g.DrawString(s_diff, baseFont, (isNegative ? Brushes.Red : Brushes.Green), cell4, centerAlignedFormat);
        //        //}

        //        leftMargin = 50;
        //        topMargin += cellHeight;
        //    }

        //    PrintFooter(ref e);
        //}
    }
}
