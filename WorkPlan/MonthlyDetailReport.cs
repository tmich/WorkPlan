using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
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

        int firstCellWidth = 100;
        int leftMargin = 50;
        int topMargin = 50;
        int cellWidth = 100;
        int cellHeight = 30;
        //int cellHeadHeight = 20;
        Font baseFont = new Font("Arial", 12.0f);
        Font boldFont = new Font("Arial", 12.0f, FontStyle.Bold);
        List<String> dipendenti = new List<string>();

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

        protected DataTable table;

        public MonthlyDetailReport(int mese, int anno)
        {
            m_mese = mese;
            m_anno = anno;

            MonthlyReportCmd mrep = new MonthlyReportCmd(m_mese, m_anno);

            DataView dv = mrep.Dettaglio.DefaultView;
            dv.Sort = "cognome";
            table = dv.ToTable();

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

        public void Print()   // questa funziona!!!
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                var dv = table.DefaultView;
                DataTable ids = dv.ToTable(true, "dipendente_id", "cognome", "nome");
                foreach (var id in ids.AsEnumerable())
                {
                    dipendenti.Add(id["dipendente_id"].ToString());
                }

                PageNumber = 1;
                pd.Print();
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

        private void PrintTestata(ref PrintPageEventArgs e)
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
            
            if(PageNumber == 1)
            {
                PrintTestata(ref e);
                topMargin += 50;
            }

            var rowNome = table.Select("dipendente_id=" + dipendenti[pn]).First();

            string nominativo = String.Format("{0} {1}", rowNome["cognome"], rowNome["nome"]);
            var rcNome = new Rectangle(leftMargin, topMargin, 700, cellHeight);
            g.DrawString(nominativo, boldFont, Brushes.Black, rcNome);
            topMargin += (cellHeight * 2);

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
            g.DrawString("DIFF", baseFont, Brushes.Black, head5, centerAlignedFormat);

            topMargin += cellHeight;

            foreach (var row in table.Select("dipendente_id=" + dipendenti[pn]))
            {
                leftMargin = e.MarginBounds.Left;

                // GIORNO
                DateTime giorno = (DateTime)row["giorno"];
                var rect = new Rectangle(leftMargin, topMargin, firstCellWidth, cellHeight);
                g.DrawRectangle(Pens.DarkGray, rect);
                g.DrawString(giorno.ToShortDateString(), baseFont, Brushes.Black, rect);
                leftMargin += rect.Width;
                //topMargin += cellHeight;

                // ORE LAVORATE
                string ol = "";
                var cell1 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                if (!row.IsNull("tot_ore_lavorate"))
                {
                    TimeSpan ore_lavorate = (TimeSpan)row["tot_ore_lavorate"];
                    ol = string.Format("{0:00}:{1:00}", Math.Floor(ore_lavorate.TotalHours), ore_lavorate.Minutes);
                }
                g.DrawRectangle(Pens.DarkGray, cell1);
                g.DrawString(ol, baseFont, Brushes.Black, cell1, centerAlignedFormat);
                leftMargin += cell1.Width;

                // ORE ASSENZA
                string oa = "";
                var cell2 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                if (!row.IsNull("tot_ore_assenza"))
                {
                    TimeSpan ore_assenza = (TimeSpan)row["tot_ore_assenza"];
                    oa = string.Format("{0:00}:{1:00}", Math.Floor(ore_assenza.TotalHours), ore_assenza.Minutes);
                }
                g.DrawRectangle(Pens.DarkGray, cell2);
                g.DrawString(oa, baseFont, Brushes.Black, cell2, centerAlignedFormat);
                leftMargin += cell2.Width;

                // GIORNO ASSENZA
                string ga = "";
                var cell3 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                if ((int)row["giorno_assenza"] > 0)
                {
                    int giorno_assenza = (int)row["giorno_assenza"];
                    ga = giorno_assenza.ToString();
                }
                g.DrawRectangle(Pens.DarkGray, cell3);
                g.DrawString(ga, baseFont, Brushes.Black, cell3, centerAlignedFormat);
                leftMargin += cell3.Width;

                // CAUSALE ASSENZA
                string causale = "";
                var cell4 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                if(!row.IsNull("causale_assenza"))
                {
                    causale = row["causale_assenza"].ToString();
                }
                g.DrawRectangle(Pens.DarkGray, cell4);
                g.DrawString(causale, baseFont, Brushes.Black, cell4, centerAlignedFormat);
                leftMargin += cell4.Width;

                // DIFFERENZA
                string diff = "";
                bool isNegative = false;
                var cell5 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                if(!row.IsNull("diff_oraria"))
                {
                    TimeSpan ts = (TimeSpan)row["diff_oraria"];

                    if (TimeSpan.Zero.CompareTo(ts) != 0)
                    {
                        isNegative = ts < TimeSpan.Zero;
                        diff = string.Format("{0}{1}", (isNegative ? "-" : ""), string.Format("{0:hh\\:mm}", ts));
                    }
                    
                }
                g.DrawRectangle(Pens.DarkGray, cell5);
                g.DrawString(diff, baseFont, (isNegative ? Brushes.Red : Brushes.Black), cell5, centerAlignedFormat);

                topMargin += cellHeight;
            }
            
            //g.DrawString(PageNumber.ToString(), baseFont, Brushes.Black, topMargin, leftMargin);

            if (PageNumber < dipendenti.Count)
            {
                PageNumber++;
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
        }
    }
}
