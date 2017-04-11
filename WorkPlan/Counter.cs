using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public abstract class Counter
    {
        protected string connstr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();

        public virtual TimeSpan Duration
        { get; }
    }

    public class AbsenceCounter : Counter
    {
        protected NoWorkReason m_Reason;
        protected Employee m_Employee;
        protected int m_Year;
        protected List<NoWork> m_Results;

        public AbsenceCounter(NoWorkReason reason, Employee employee, int year)
        {
            m_Reason = reason;
            m_Employee = employee;
            m_Year = year;
        }

        public override TimeSpan Duration
        {
            get
            {
                TimeSpan duration = new TimeSpan();
                m_Results = new List<NoWork>();

                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    try
                    {
                        string sql= "SELECT `assenze`.`id`, `assenze`.`dipendente_id`, `assenze`.`data_inizio`,	`assenze`.`data_fine`, ";
                        sql += "`assenze`.`motivo_id`, `motivo_assenza`.`descrizione`, `motivo_assenza`.`codice`, ";
                        sql += "`assenze`.`note`, `assenze`.`giornata_intera` ";
                        sql += "FROM `turni`.`assenze` INNER JOIN `turni`.`motivo_assenza` ON `motivo_assenza`.`id` = `assenze`.`motivo_id` ";
                        sql += "WHERE cancellato = false AND year(data_inizio) = {0} AND dipendente_id = {1} AND motivo_id = {2}";

                        sql = string.Format(sql, m_Year, m_Employee.Id, m_Reason.Id);

                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = sql;
                        var rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {
                            m_Results.Add(new NoWork()
                            {
                                Id = rdr.GetInt32(0),
                                Employee = m_Employee,
                                StartDate = rdr.GetDateTime(2),
                                EndDate = rdr.GetDateTime(3),
                                Reason = new NoWorkReason() { Id = rdr.GetInt32(4), Value = rdr.GetString(5), Code = rdr.GetString(6) },
                                Notes = rdr.IsDBNull(7) ? String.Empty : rdr.GetString(7),
                                FullDay = rdr.GetBoolean(8)
                            });
                        }

                        conn.Close();

                        EconomicsRepository ecoRepo = new EconomicsRepository();
                        var orePattuite = ecoRepo.GetDailyHours(m_Employee.Id);

                        // calcolo
                        foreach (var nw in m_Results)
                        {
                            if(nw.FullDay)
                            {
                                duration = duration.Add(orePattuite);
                            }
                            else
                            {
                                duration = duration.Add(nw.GetDuration());
                            }
                            
                        }

                        return duration;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

            }
        }
    }
}
