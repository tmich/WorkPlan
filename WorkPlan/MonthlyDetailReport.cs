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
    class MonthlyDetailReport : IMonthlyReport
    {
        protected PrintDocument pd;
        protected int PageNumber = 0;
        protected int PrintedRows = 0;
        protected int RowsPerPage = 16;

        int m_mese, m_anno;

        int firstCellWidth = 140;
        int leftMargin = 50;
        int topMargin = 50;
        int cellWidth = 100;
        int cellHeight = 30;
        Font bigboldFont = new Font("Arial", 12.0f, FontStyle.Bold);
        Font baseFont = new Font("Arial", 10.0f);
        Font bigFont = new Font("Arial", 12.0f, FontStyle.Regular);
        Font italicFont = new Font("Arial", 10.0f, FontStyle.Italic);
        Font boldFont = new Font("Arial", 10.0f, FontStyle.Bold);
        Font miniFont = new Font("Arial", 7.0f, FontStyle.Regular);
        List<String> dipendenti = new List<string>();
        List<Employee> employees = new List<Employee>();

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

        //protected DataTable table;

        public MonthlyDetailReport(int mese, int anno)
        {
            m_mese = mese;
            m_anno = anno;

            //MonthlyReportCmd mrep = new MonthlyReportCmd(m_mese, m_anno);

            //DataView dv = mrep.Dettaglio.DefaultView;
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

        //public void Print()
        //{
        //    PrintPreviewDialog printDialog = new PrintPreviewDialog();

        //    var dv = table.DefaultView;
        //    DataTable ids = dv.ToTable(true, "dipendente_id", "cognome", "nome");
        //    foreach (var id in ids.AsEnumerable())
        //    {
        //        dipendenti.Add(id["dipendente_id"].ToString());
        //    }

        //    PageNumber = 1;

        //    printDialog.Document = pd;

        //    printDialog.ShowDialog();
        //}

        public DateTime StartDate()
        {
            return new DateTime(m_anno, m_mese, 1);
        }

        public DateTime EndDate()
        {
            return StartDate().AddMonths(1).Subtract(new TimeSpan(1, 0, 0, 0));
        }

        public void Print()   // questa funziona!!!
        {
            // 27/03/2017: nuova procedura di stampa per singolo dipendente
            // scelta dei dipendenti
            ChooseEmployees choose = new ChooseEmployees();
            employees = choose.AskUser();
            employees.Sort((emp1, emp2) => emp1.FullName.CompareTo(emp2.FullName));
            if (employees.Count > 0)
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = pd;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    //var dv = table.DefaultView;
                    //DataTable ids = dv.ToTable(true, "dipendente_id", "cognome", "nome");
                    //foreach (var id in ids.AsEnumerable())
                    //{
                    //    dipendenti.Add(id["dipendente_id"].ToString());
                    //}

                    PageNumber = 1;
                    pd.Print();
                }
            }
        }

        //protected void PrintDipendente(ref Graphics g, DataRow[] rows, string dip_id)
        //{
        //    var rowNome = table.Select("dipendente_id=" + dip_id).First();

        //    string nominativo = String.Format("{0} {1}", rowNome["cognome"], rowNome["nome"]);
        //    var rcNome = new Rectangle(leftMargin, topMargin, firstCellWidth, cellHeight);
        //    g.DrawString(nominativo, baseFont, Brushes.Black, rcNome);
        //    topMargin += cellHeight;

        //    foreach (var row in table.Select("dipendente_id=" + dip_id))
        //    {
        //        DateTime giorno = (DateTime)row["giorno"];
        //        var rect = new Rectangle(leftMargin, topMargin, firstCellWidth, cellHeight);
        //        g.DrawRectangle(Pens.DarkGray, rect);
        //        g.DrawString(giorno.ToShortDateString(), baseFont, Brushes.Black, rect);
        //        //leftMargin += rect.Width;
        //        topMargin += cellHeight;
        //    }
        //}

        private void PrintDocTitle(ref PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            // INTESTAZIONE
            Font titleFont = new Font("Arial", 18.0f, FontStyle.Bold);
            //Font headerPrintFont = new Font("Arial", 8.0f, FontStyle.Bold);
            leftMargin = e.MarginBounds.Left;
            topMargin = e.MarginBounds.Top;

            var title = new Rectangle(leftMargin, topMargin, e.MarginBounds.Width, cellHeight * 2);
            //g.FillRectangle(Brushes.LightSlateGray, title);
            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            var FirstDay = new DateTime(m_anno, m_mese, 1);
            var LastDay = new DateTime(m_anno, m_mese, DateTime.DaysInMonth(m_anno, m_mese));

            g.DrawString(string.Format("Dettaglio Mensile Presenze dal {0} al {1}",
                FirstDay.ToShortDateString(), LastDay.ToShortDateString()), titleFont, Brushes.Black, title);

            //topMargin += (cellHeight * 2);

            

            //leftMargin = 50;
            //topMargin += cellHeight;
        }

        protected void PrintPage(object sender, PrintPageEventArgs e)
        {
            int pn = PageNumber - 1;
            int leftMargin = e.MarginBounds.Left;
            int topMargin = e.MarginBounds.Top;
            Graphics g = e.Graphics;
            bool mancanoOrePattuite = false;

            List<NoWork> assenzeMensili = new List<NoWork>();

            //if (PageNumber == 1)
            //{
            //    PrintDocTitle(ref e);
            //    topMargin += 50;
            //}

            Employee emp = employees[pn];
            var rcTitle = new Rectangle(leftMargin, topMargin, 700, cellHeight);
            g.DrawString(emp.FullName, bigboldFont, Brushes.Black, rcTitle);
            topMargin += (cellHeight);
            var rcSubTitle = new Rectangle(leftMargin, topMargin, 700, cellHeight);
            g.DrawString(string.Format("Situazione mese di {0} {1}", StartDate().ToString("MMMM"), StartDate().ToString("yyyy")), boldFont, Brushes.Black, rcSubTitle);
            topMargin += (cellHeight);

            PrintHeaders(g, leftMargin, topMargin, firstCellWidth, cellHeight);
            topMargin += cellHeight;
            leftMargin = e.MarginBounds.Left;

            // tutte le giornate lavorate
            DutyRepository dutyRepo = new DutyRepository();

            // Totali per dipendente
            TimeSpan tsTotaleOreLavorate = new TimeSpan();
            TimeSpan tsTotaleOreAssenza = new TimeSpan();
            TimeSpan tsTotaleDifferenza = new TimeSpan();
            //int iTotaleGgAssenza = 0;

            // ore giornaliere
            EconomicsRepository ecoRepo = new EconomicsRepository();
            TimeSpan orePattuite = ecoRepo.GetDailyHours(emp.Id);
            mancanoOrePattuite = (TimeSpan.Zero.CompareTo(orePattuite) == 0);
            
            // scorro i giorni
            DateTime dx = StartDate();
            while(dx <= EndDate())
            {
                // Totali della giornata
                TimeSpan ore_lavorate = new TimeSpan();
                TimeSpan ore_assenza = new TimeSpan();
                int giorno_assenza = 0;

                // (1) GIORNO
                leftMargin = e.MarginBounds.Left;
                var rect = new Rectangle(leftMargin, topMargin, firstCellWidth, cellHeight);
                g.DrawRectangle(Pens.DarkGray, rect);
                g.DrawString(dx.ToString("ddd dd-MM-yyyy", CultureInfo.CurrentCulture), italicFont, Brushes.Black, rect);
                leftMargin += rect.Width;

                // (2) ORE LAVORATE DELLA GIORNATA
                string ol = "";
                var cell1 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                List<Duty> dutiesOfTheDay = dutyRepo.GetBy(emp, dx);
                if (dutiesOfTheDay.Count > 0)
                {
                    dutiesOfTheDay.ForEach(delegate (Duty d) 
                    {
                        ore_lavorate += d.GetDuration();
                    });
                    
                    if (ore_lavorate != TimeSpan.Zero)
                    {
                        ol = string.Format("{0:00}:{1:00}", Math.Floor(ore_lavorate.TotalHours), ore_lavorate.Minutes);
                        tsTotaleOreLavorate = tsTotaleOreLavorate.Add(ore_lavorate);
                    }
                }
                g.DrawRectangle(Pens.DarkGray, cell1);
                g.DrawString(ol, baseFont, Brushes.Black, cell1, centerAlignedFormat);
                leftMargin += cell1.Width;

                // (3) ORE ASSENZA DELLA GIORNATA
                string causale = "";
                string oa = "";
                var cell2 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                NoWorkRepository noRepo = new NoWorkRepository();
                List<NoWork> assenze = noRepo.GetAssenzeByEmployeeAndDate(emp, dx);
                List<NoWork> assenzeHH = assenze.FindAll(delegate (NoWork nw) { return nw.FullDay == false; });
                if(assenzeHH.Count > 0)
                {
                    assenzeHH.ForEach(delegate (NoWork n)
                    {
                        ore_assenza += n.GetDuration();
                    });
                    
                    //ore_assenza = assenzeHH[0].GetDuration();
                    if (ore_assenza != TimeSpan.Zero)
                    {
                        oa = string.Format("{0:00}:{1:00}", Math.Floor(ore_assenza.TotalHours), ore_assenza.Minutes);
                        tsTotaleOreAssenza = tsTotaleOreAssenza.Add(ore_assenza);
                        causale = assenzeHH[0].Reason.ToString();
                    }

                    assenzeMensili.AddRange(assenze);
                }
                g.DrawRectangle(Pens.DarkGray, cell2);
                g.DrawString(oa, baseFont, Brushes.Black, cell2, centerAlignedFormat);
                leftMargin += cell2.Width;

                // (4) GIORNO ASSENZA
                string ga = "";
                var cell3 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                List<NoWork> assenzeGG = assenze.FindAll(delegate (NoWork nw) 
                {
                    return nw.FullDay == true;
                });

                if (assenzeGG.Count > 0)
                {
                    giorno_assenza++;
                    ga = "1";

                    if(assenzeGG[0].Reason.Code.Equals("RIP"))
                    {
                        // il riposo è figurativo
                        ga = "(1)";
                    }
                    
                    //iTotaleGgAssenza++;

                    causale = assenzeGG[0].Reason.ToString();

                    assenzeMensili.AddRange(assenzeGG);
                }

                g.DrawRectangle(Pens.DarkGray, cell3);
                g.DrawString(ga, baseFont, Brushes.Black, cell3, centerAlignedFormat);
                leftMargin += cell3.Width;

                // (5) CAUSALE ASSENZA
                var cell4 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                g.DrawRectangle(Pens.DarkGray, cell4);
                g.DrawString(causale, baseFont, Brushes.Black, cell4, centerAlignedFormat);
                leftMargin += cell4.Width;

                // (6) DIFFERENZA
                string diff = "";
                bool isNegative = false;
                var cell5 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                g.DrawRectangle(Pens.DarkGray, cell5);
                if (giorno_assenza == 0)
                {
                    if(mancanoOrePattuite)
                    {
                        g.DrawString("*", baseFont, Brushes.Red, cell5, centerAlignedFormat);
                    }
                    else
                    {
                        TimeSpan tsDiff = new TimeSpan();
                        tsDiff = (ore_lavorate + ore_assenza) - orePattuite;
                        tsTotaleDifferenza = tsTotaleDifferenza.Add(tsDiff);

                        if (TimeSpan.Zero.CompareTo(tsDiff) != 0)
                        {
                            isNegative = tsDiff < TimeSpan.Zero;
                            diff = string.Format("{0}{1}", (isNegative ? "-" : "+"), string.Format("{0:hh\\:mm}", tsDiff));
                            g.DrawString(diff, baseFont, (isNegative ? Brushes.Red : Brushes.Black), cell5, centerAlignedFormat);
                        }
                    }   
                }
                leftMargin += cell5.Width;

                // Controllo che esistano tutti i dati, in caso contrario segnalo
                if (ore_lavorate == TimeSpan.Zero && ore_assenza == TimeSpan.Zero && giorno_assenza == 0)
                {
                    if(!mancanoOrePattuite)
                    {
                        var cell6 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                        g.DrawString("←", bigFont, Brushes.Red, cell6, leftAlignedFormat);
                    }
                }


                topMargin += cellHeight;
                dx = dx.AddDays(1);
            }

            // RIGA TOTALI DIPENDENTE
            leftMargin = e.MarginBounds.Left;
            leftMargin += firstCellWidth;

            // hh lavorate
            var cellTot1 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
            string sTotOreLavorate = FormatTimeSpan(tsTotaleOreLavorate);
            g.DrawRectangle(Pens.DarkGray, cellTot1);
            g.DrawString(sTotOreLavorate, boldFont, Brushes.Black, cellTot1, centerAlignedFormat);
            leftMargin += cellTot1.Width;

            // hh assenza
            var cellTot2 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
            string sTotOreAssenza = FormatTimeSpan(tsTotaleOreAssenza);
            g.DrawRectangle(Pens.DarkGray, cellTot2);
            g.DrawString(sTotOreAssenza, boldFont, Brushes.Black, cellTot2, centerAlignedFormat);
            leftMargin += cellTot2.Width;

            // gg assenza
            // 10/04/2017: riporto le assenze a giornata intera in ore
            var cellTot3 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);

            //var assenzeGiornataIntera = assenzeMensili.FindAll(delegate (NoWork nw)
            //{
            //    return nw.FullDay == true; // && nw.Reason.Code.Equals("RIP");
            //});

            var assenzeGiornataInteraSenzaRiposi = assenzeMensili.FindAll(delegate (NoWork nw)
            {
                return nw.FullDay == true && !nw.Reason.Code.Equals("RIP");
            });

            double ggToHh = orePattuite.TotalHours * assenzeGiornataInteraSenzaRiposi.Count;
            TimeSpan tsGGAssenzaInOre = new TimeSpan((int)ggToHh, 0, 0);
            string sTotGGAssenzaInOre = FormatTimeSpan(tsGGAssenzaInOre);
            g.DrawRectangle(Pens.DarkGray, cellTot3);
            g.DrawString(sTotGGAssenzaInOre, boldFont, Brushes.Black, cellTot3, centerAlignedFormat);
            leftMargin += (cellTot3.Width) * 2;     // salto la casella della causale

            // diff.
            // 10/04/2017: la differenza è calcolata a partire da un valore medio di 208 hh lavorative (26 gg)
            //             non considero i riposi

            //var assenzeGiornataInteraSenzaRiposi = assenzeMensili.FindAll(delegate (NoWork nw)
            //{
            //    return nw.FullDay == true && !nw.Reason.Code.Equals("RIP");
            //});

            int ggMediLavorabili = 26;
            int hhTotaliLavorabili = (int)(ggMediLavorabili * orePattuite.TotalHours);
            TimeSpan tsTotaleMensileLavorabile = new TimeSpan(hhTotaliLavorabili, 0, 0);
            TimeSpan tsDifferenzaMensile = tsTotaleMensileLavorabile.Subtract(tsTotaleOreLavorate);
            TimeSpan tsAssenzeGiornataInteraSenzaRiposi = new TimeSpan((int)(assenzeGiornataInteraSenzaRiposi.Count * orePattuite.TotalHours), 0, 0);
            tsDifferenzaMensile = tsDifferenzaMensile.Subtract(tsTotaleOreAssenza);
            tsDifferenzaMensile = tsDifferenzaMensile.Subtract(tsAssenzeGiornataInteraSenzaRiposi);

            var cellTot4 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
            g.DrawRectangle(Pens.DarkGray, cellTot4);
            string sTotDifferenza = FormatTimeSpan(tsDifferenzaMensile);
            if (tsDifferenzaMensile == TimeSpan.Zero)
            {
                sTotDifferenza = "";
            }
            else
            {
                sTotDifferenza = string.Format("{0}{1}", (tsDifferenzaMensile > TimeSpan.Zero ? "-" : "+"), sTotDifferenza);
            }

            g.DrawString(sTotDifferenza, boldFont, (tsDifferenzaMensile > TimeSpan.Zero ? Brushes.Red : Brushes.Black), cellTot4, centerAlignedFormat);
            leftMargin += cellTot4.Width;

            var cellTot5 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
            g.DrawString(string.Format(" su {0} ore medie mensili", (int)tsTotaleMensileLavorabile.TotalHours), baseFont, Brushes.Black, cellTot5, centerAlignedFormat);

            // Riga Avvisi
            topMargin += cellHeight;
            leftMargin = e.MarginBounds.Left;
            var cellAvviso = new Rectangle(leftMargin, topMargin, e.MarginBounds.Width, cellHeight);

            if (mancanoOrePattuite)
            {
                g.DrawString("*: mancano le ore giornaliere del dipendente", baseFont, Brushes.Black, cellAvviso);
            }

            // Piè di pagina
            var cellFooter = new Rectangle(e.MarginBounds.Left, e.MarginBounds.Bottom - 50, e.MarginBounds.Width, cellHeight);
            g.DrawString(string.Format("Data stampa: {0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToShortTimeString()), 
                miniFont, Brushes.Black, cellFooter, rightAlignedFormat);

            // Pagina successiva
            if (PageNumber < employees.Count)
            {
                PageNumber++;
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
        }

        protected void PrintHeaders(Graphics g, int leftMargin, int topMargin, int firstCellWidth, int cellHeight)
        {
            // stampa testata della tabella
            var head0 = new Rectangle(leftMargin, topMargin, firstCellWidth, cellHeight);
            g.DrawRectangle(Pens.DarkGray, head0);
            g.FillRectangle(Brushes.LightGray, head0);
            g.DrawString("DATA", baseFont, Brushes.Black, head0);
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

            var head4 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
            g.DrawRectangle(Pens.DarkGray, head4);
            g.FillRectangle(Brushes.LightGray, head4);
            g.DrawString("CAUSALE", baseFont, Brushes.Black, head4, centerAlignedFormat);
            leftMargin += head4.Width;

            var head5 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
            g.DrawRectangle(Pens.DarkGray, head5);
            g.FillRectangle(Brushes.LightGray, head5);
            g.DrawString("DIFF.", baseFont, Brushes.Black, head5, centerAlignedFormat);
        }

        //protected void OLD_PrintPage(object sender, PrintPageEventArgs e)
        //{
        //    int pn = PageNumber - 1;
        //    int leftMargin = e.MarginBounds.Left;
        //    int topMargin = e.MarginBounds.Top;
        //    Graphics g = e.Graphics;

        //    if (PageNumber == 1)
        //    {
        //        PrintDocTitle(ref e);
        //        topMargin += 50;
        //    }

        //    // totali
        //    TimeSpan tsTotaleOreLavorate = new TimeSpan();
        //    TimeSpan tsTotaleOreAssenza = new TimeSpan();
        //    TimeSpan tsTotaleDifferenza = new TimeSpan();
        //    int iTotaleGgAssenza = 0;
        //    var rowNome = table.Select("dipendente_id=" + dipendenti[pn]).First();

        //    string nominativo = String.Format("{0} {1}", rowNome["cognome"], rowNome["nome"]);
        //    var rcNome = new Rectangle(leftMargin, topMargin, 700, cellHeight);
        //    g.DrawString(nominativo, boldFont, Brushes.Black, rcNome);
        //    topMargin += (cellHeight * 2);

        //    // stampa testata della tabella
        //    var head0 = new Rectangle(leftMargin, topMargin, firstCellWidth, cellHeight);
        //    g.DrawRectangle(Pens.DarkGray, head0);
        //    g.FillRectangle(Brushes.LightGray, head0);
        //    g.DrawString("DATA", baseFont, Brushes.Black, head0);
        //    leftMargin += head0.Width;

        //    var head1 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //    g.DrawRectangle(Pens.DarkGray, head1);
        //    g.FillRectangle(Brushes.LightGray, head1);
        //    g.DrawString("HH LAV.", baseFont, Brushes.Black, head1, centerAlignedFormat);
        //    leftMargin += head1.Width;

        //    var head2 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //    g.DrawRectangle(Pens.DarkGray, head2);
        //    g.FillRectangle(Brushes.LightGray, head2);
        //    g.DrawString("HH ASS.", baseFont, Brushes.Black, head2, centerAlignedFormat);
        //    leftMargin += head2.Width;

        //    var head3 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //    g.DrawRectangle(Pens.DarkGray, head3);
        //    g.FillRectangle(Brushes.LightGray, head3);
        //    g.DrawString("GG ASS.", baseFont, Brushes.Black, head3, centerAlignedFormat);
        //    leftMargin += head3.Width;

        //    var head4 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //    g.DrawRectangle(Pens.DarkGray, head4);
        //    g.FillRectangle(Brushes.LightGray, head4);
        //    g.DrawString("CAUSALE", baseFont, Brushes.Black, head4, centerAlignedFormat);
        //    leftMargin += head4.Width;

        //    var head5 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //    g.DrawRectangle(Pens.DarkGray, head5);
        //    g.FillRectangle(Brushes.LightGray, head5);
        //    g.DrawString("DIFF", baseFont, Brushes.Black, head5, centerAlignedFormat);

        //    topMargin += cellHeight;

        //    foreach (var row in table.Select("dipendente_id=" + dipendenti[pn]))
        //    {
        //        leftMargin = e.MarginBounds.Left;

        //        // GIORNO
        //        DateTime giorno = (DateTime)row["giorno"];
        //        var rect = new Rectangle(leftMargin, topMargin, firstCellWidth, cellHeight);
        //        g.DrawRectangle(Pens.DarkGray, rect);
        //        g.DrawString(giorno.ToShortDateString(), baseFont, Brushes.Black, rect);
        //        leftMargin += rect.Width;
        //        //topMargin += cellHeight;

        //        // ORE LAVORATE
        //        string ol = "";
        //        var cell1 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //        TimeSpan ore_lavorate = new TimeSpan();
        //        if (!row.IsNull("tot_ore_lavorate"))
        //        {
        //            ore_lavorate = (TimeSpan)row["tot_ore_lavorate"];
        //            ol = string.Format("{0:00}:{1:00}", Math.Floor(ore_lavorate.TotalHours), ore_lavorate.Minutes);
        //        }

        //        tsTotaleOreLavorate = tsTotaleOreLavorate.Add(ore_lavorate);
        //        g.DrawRectangle(Pens.DarkGray, cell1);
        //        g.DrawString(ol, baseFont, Brushes.Black, cell1, centerAlignedFormat);
        //        leftMargin += cell1.Width;

        //        // ORE ASSENZA
        //        string oa = "";
        //        var cell2 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //        TimeSpan tsOreAssenza = new TimeSpan();
        //        if (!row.IsNull("tot_ore_assenza"))
        //        {
        //            tsOreAssenza = (TimeSpan)row["tot_ore_assenza"];
        //            oa = string.Format("{0:00}:{1:00}", Math.Floor(tsOreAssenza.TotalHours), tsOreAssenza.Minutes);
        //            tsTotaleOreAssenza = tsTotaleOreAssenza.Add(tsOreAssenza);
        //        }
        //        g.DrawRectangle(Pens.DarkGray, cell2);
        //        g.DrawString(oa, baseFont, Brushes.Black, cell2, centerAlignedFormat);
        //        leftMargin += cell2.Width;

        //        // GIORNO ASSENZA
        //        string ga = "";
        //        var cell3 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //        int giorno_assenza = 0;
        //        if ((int)row["giorno_assenza"] > 0)
        //        {
        //            giorno_assenza = (int)row["giorno_assenza"];
        //            ga = giorno_assenza.ToString();
        //            iTotaleGgAssenza++;
        //        }
        //        g.DrawRectangle(Pens.DarkGray, cell3);
        //        g.DrawString(ga, baseFont, Brushes.Black, cell3, centerAlignedFormat);
        //        leftMargin += cell3.Width;

        //        // CAUSALE ASSENZA
        //        string causale = "";
        //        var cell4 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //        if (!row.IsNull("causale_assenza"))
        //        {
        //            causale = row["causale_assenza"].ToString();
        //        }
        //        g.DrawRectangle(Pens.DarkGray, cell4);
        //        g.DrawString(causale, baseFont, Brushes.Black, cell4, centerAlignedFormat);
        //        leftMargin += cell4.Width;

        //        // DIFFERENZA
        //        string diff = "";
        //        bool isNegative = false;
        //        var cell5 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //        TimeSpan tsDiff = new TimeSpan();
        //        if (!row.IsNull("diff_oraria"))
        //        {
        //            tsDiff = (TimeSpan)row["diff_oraria"];
        //        }

        //        // tsDiff = tsDiff.Add(tsOreAssenza);   // aggiungo le ore di assenza
        //        tsTotaleDifferenza = tsTotaleDifferenza.Add(tsDiff);
        //        g.DrawRectangle(Pens.DarkGray, cell5);
        //        if (TimeSpan.Zero.CompareTo(tsDiff) != 0)
        //        {
        //            isNegative = tsDiff < TimeSpan.Zero;
        //            diff = string.Format("{0}{1}", (isNegative ? "-" : "+"), string.Format("{0:hh\\:mm}", tsDiff));
        //            g.DrawString(diff, baseFont, (isNegative ? Brushes.Red : Brushes.Black), cell5, centerAlignedFormat);
        //        }
                
        //        // Controllo che esistano tutti i dati, in caso contrario segnalo
        //        if(ore_lavorate == TimeSpan.Zero &&
        //            tsOreAssenza == TimeSpan.Zero &&
        //            giorno_assenza == 0)
        //        {
        //            g.DrawString("???", baseFont, Brushes.Red, cell5, centerAlignedFormat);
        //        }


        //        topMargin += cellHeight;
        //    }

        //    // RIGA TOTALI
        //    leftMargin = e.MarginBounds.Left;
        //    leftMargin += firstCellWidth;

        //    var cellTot1 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //    string sTotOreLavorate = FormatTimeSpan(tsTotaleOreLavorate);
        //    g.DrawRectangle(Pens.DarkGray, cellTot1);
        //    g.DrawString(sTotOreLavorate, boldFont, Brushes.Black, cellTot1, centerAlignedFormat);
        //    leftMargin += cellTot1.Width;

        //    var cellTot2 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //    string sTotOreAssenza = FormatTimeSpan(tsTotaleOreAssenza);
        //    g.DrawRectangle(Pens.DarkGray, cellTot2);
        //    g.DrawString(sTotOreAssenza, boldFont, Brushes.Black, cellTot2, centerAlignedFormat);
        //    leftMargin += cellTot2.Width;

        //    var cellTot3 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //    g.DrawRectangle(Pens.DarkGray, cellTot3);
        //    g.DrawString(iTotaleGgAssenza.ToString(), boldFont, Brushes.Black, cellTot3, centerAlignedFormat);
        //    leftMargin += (cellTot3.Width) * 2;

        //    var cellTot4 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
        //    g.DrawRectangle(Pens.DarkGray, cellTot4);
        //    string sTotDifferenza = FormatTimeSpan(tsTotaleDifferenza);
        //    if (tsTotaleDifferenza == TimeSpan.Zero)
        //    {
        //        sTotDifferenza = "";
        //    }
        //    else
        //    {
        //        sTotDifferenza = string.Format("{0}{1}", (tsTotaleDifferenza < TimeSpan.Zero ? "-" : "+"), sTotDifferenza);
        //    }
            
        //    g.DrawString(sTotDifferenza, boldFont, (tsTotaleDifferenza < TimeSpan.Zero ? Brushes.Red : Brushes.Black), cellTot4, centerAlignedFormat);
        //    leftMargin += cellTot4.Width;

        //    //g.DrawString(PageNumber.ToString(), baseFont, Brushes.Black, topMargin, leftMargin);

        //    if (PageNumber < dipendenti.Count)
        //    {
        //        PageNumber++;
        //        e.HasMorePages = true;
        //    }
        //    else
        //    {
        //        e.HasMorePages = false;
        //    }
        //}

        private string FormatTimeSpan(TimeSpan ts)
        {
            string dd = ts.ToString("%d");
            string hh = ts.ToString("%h");
            int h = (int.Parse(dd) * 24) + int.Parse(hh);
            string mm = ts.ToString("mm");
            return string.Format("{0}:{1}", h, mm);
        }
    }
}
