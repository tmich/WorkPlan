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
    public partial class MonthReportView : Form
    {
        private List<MonthReport> m_Reports;
        int idx;
        protected PrintDocument pd;
        protected int PageNumber = 0;

        // allineamento a destra
        StringFormat rightAlignedFormat = new StringFormat()
        {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Center
        };

        public MonthReportView(List<MonthReport> reports)
        {
            InitializeComponent();
            m_Reports = reports;
            nPagina.Maximum = m_Reports.Count;
            nPagina.Minimum = 1;

            idx = 0;
            nPagina.Value = 1;

            foreach(var rep in m_Reports)
                rep.Calculate();

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

        protected void PrintPage(object sender, PrintPageEventArgs e)
        {
            var rep = m_Reports[PageNumber];
            rep.Draw(e.Graphics);
            rep.DrawFooter(e.Graphics, e.MarginBounds.Left, e.MarginBounds.Bottom - 50, e.MarginBounds.Width, 20);

            e.HasMorePages = (++PageNumber < m_Reports.Count);
        }
        
        private void nPagina_ValueChanged(object sender, EventArgs e)
        {
            idx = (int)nPagina.Value - 1;
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            m_Reports[idx].Draw(e.Graphics);
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
