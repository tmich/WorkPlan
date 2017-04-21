using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkPlan
{
    public partial class MonthSummaryView : Form
    {
        private List<MonthReport> m_Reports;
        protected PrintDocument pd;
        protected int PageNumber = 0;

        public MonthSummaryView(List<MonthReport> reports)
        {
            InitializeComponent();
            m_Reports = reports;

            pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintPage);
            PaperSize ps = new PaperSize();
            ps.RawKind = (int)PaperKind.A4;

            pd.DefaultPageSettings.PaperSize = ps;
            pd.DefaultPageSettings.Landscape = true;
            pd.DefaultPageSettings.Margins.Top = 30;
            pd.DefaultPageSettings.Margins.Bottom = 30;
            pd.DefaultPageSettings.Margins.Left = 30;
            pd.DefaultPageSettings.Margins.Right = 30;
        }

        private void PaintTotals(object sender, Graphics g)
        {
            int marginLeft = 50;
            int marginTop = 50;
            int y = marginTop;
            int x = marginLeft;

            Font smallBoldFont = new Font("Arial", 8, FontStyle.Bold);

            Rectangle rTitle = new Rectangle(x, y, 400, 20);
            g.DrawString(m_Reports[0].StartDate.ToString("MMMM yyyy"), smallBoldFont, Brushes.Black, rTitle);

            //testata
            y += 20;
            m_Reports[0].DrawTotalsHeaders(g, x, y);

            foreach (var report in m_Reports)
            {
                y += 20;
                report.DrawTotals(g, marginLeft, y);
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            PaintTotals(sender, e.Graphics);
            m_Reports[0].DrawFooter(e.Graphics, e.MarginBounds.Left, e.MarginBounds.Bottom - 50, e.MarginBounds.Width, 20);
            e.HasMorePages = false;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            PaintTotals(sender, e.Graphics);
        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
                Close();
            }
        }
    }
}
