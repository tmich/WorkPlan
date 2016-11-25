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
    class MonthlySummaryReport : IMonthlyReport
    {
        protected PrintDocument pd;
        protected int PageNumber = 0;
        protected int PrintedRows = 0;
        protected int RowsPerPage = 16;

        int m_mese, m_anno;

        int firstCellWidth = 500;
        int leftMargin = 50;
        int topMargin = 50;
        int cellWidth = 80;
        int cellHeight = 20;
        //int cellHeadHeight = 20;
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

        protected DataTable table;

        public MonthlySummaryReport(int mese, int anno)
        {
            m_mese = mese;
            m_anno = anno;

            MonthlyReportCmd mrep = new MonthlyReportCmd(m_mese, m_anno);
            
            DataView dv = mrep.Sintesi.DefaultView;
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

        private void PrintTestata(ref PrintPageEventArgs e)
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

        protected void PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            if (PageNumber == 1)
            {
                PrintTestata(ref e);
            }
            
            foreach (DataRow row in table.Rows)
            {
                string nominativo = String.Format("{0} {1}", row["cognome"], row["nome"]);

                var rect = new Rectangle(leftMargin, topMargin, firstCellWidth, cellHeight);
                g.DrawRectangle(Pens.DarkGray, rect);
                g.DrawString(nominativo, baseFont, Brushes.Black, rect);
                leftMargin += rect.Width;

                // ore lavorate
                string ol = "";
                var cell1 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                if (!row.IsNull("tot_ore_lavorate"))
                {
                    TimeSpan ore_lavorate = (TimeSpan)row["tot_ore_lavorate"];
                    ol= string.Format("{0:00}:{1:00}", Math.Floor(ore_lavorate.TotalHours), ore_lavorate.Minutes);
                }
                g.DrawRectangle(Pens.DarkGray, cell1);
                g.DrawString(ol, baseFont, Brushes.Black, cell1, centerAlignedFormat);
                leftMargin += cell1.Width;

                // ore di assenza
                string oa = "";
                var cell2 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                if (!row.IsNull("ore_assenza"))
                {
                    TimeSpan ore_assenza = (TimeSpan)row["ore_assenza"];
                    oa = string.Format("{0:00}:{1:00}", Math.Floor(ore_assenza.TotalHours), ore_assenza.Minutes);
                }
                g.DrawRectangle(Pens.DarkGray, cell2);
                g.DrawString(oa, baseFont, Brushes.Black, cell2, centerAlignedFormat);
                leftMargin += cell2.Width;

                // giorni di assenza
                int gg = int.Parse(row["giorni_assenza"].ToString());
                string ga = "";
                var cell3 = new Rectangle(leftMargin, topMargin, cellWidth, cellHeight);
                if (gg != 0)
                {
                    ga = gg.ToString();
                }
                g.DrawRectangle(Pens.DarkGray, cell3);
                g.DrawString(ga, baseFont, Brushes.Black, cell3, centerAlignedFormat);
                leftMargin += cell3.Width;

                leftMargin = 50;
                topMargin += cellHeight;
            }

            PrintFooter(ref e);
        }
    }
}
