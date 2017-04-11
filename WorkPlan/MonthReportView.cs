using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        
        public MonthReportView(List<MonthReport> reports)
        {
            InitializeComponent();
            m_Reports = reports;
            nPagina.Maximum = m_Reports.Count;
            nPagina.Minimum = 1;

            idx = 0;
            nPagina.Value = 1;
            m_Reports[idx].Calculate();
        }
        
        private void MonthReportView_Paint(object sender, PaintEventArgs e)
        {
            m_Reports[idx].Draw(e.Graphics);
        }

        private void nPagina_ValueChanged(object sender, EventArgs e)
        {
            idx = (int)nPagina.Value - 1;
            m_Reports[idx].Calculate();
            Invalidate();
        }
    }
}
