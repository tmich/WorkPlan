using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Drawing;

namespace WorkPlan
{
    class MonthlyReport
    {
        protected string connstr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
        protected int m_month, m_year;
        protected DataTable m_sintesi = null, m_dettaglio = null;

        public MonthlyReport(int month, int year)
        {
            m_month = month;
            m_year = year;
        }

        protected void CreateReport()
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ReportMensile";
                    cmd.Parameters.Add("data_rif", MySqlDbType.DateTime).Value = new DateTime(m_year, m_month, 1);

                    var res = cmd.ExecuteNonQuery();

                    if (res != 0)
                    {
                        throw new Exception("Errore nella generazione del report");
                    }

                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }

        protected void ExecuteDettaglio()
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetReportMensileDettaglio";
                    cmd.Parameters.Add("mese", MySqlDbType.Int32).Value = m_month;
                    cmd.Parameters.Add("anno", MySqlDbType.Int32).Value = m_year;

                    m_dettaglio = new DataTable();
                    m_dettaglio.Load(cmd.ExecuteReader());

                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }

        protected void ExecuteSintesi()
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetReportMensileSintesi";
                    cmd.Parameters.Add("mese", MySqlDbType.Int32).Value = m_month;
                    cmd.Parameters.Add("anno", MySqlDbType.Int32).Value = m_year;

                    m_sintesi = new DataTable();
                    m_sintesi.Load(cmd.ExecuteReader());

                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }

        public DataTable Dettaglio
        {
            get
            {
                if (m_dettaglio == null)
                {
                    Execute();
                }
                
                return m_dettaglio;
            }
        }

        public DataTable Sintesi
        {
            get
            {
                if(m_sintesi == null)
                {
                    Execute();
                }
                
                return m_sintesi;
            }
        }

        protected void Execute()
        {
            CreateReport();

            ExecuteDettaglio();

            ExecuteSintesi();
        }
    }
}
