using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class EconomicsRepository
    {
        protected string connstr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();

        public void SetAgreedSalary(int idDipendente, decimal salary)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SetSalarioPattuito";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var pid = new MySqlParameter("pid", MySqlDbType.Int32);
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = idDipendente;
                    cmd.Parameters.Add(pid);

                    var sal = new MySqlParameter("salario", MySqlDbType.Decimal);
                    sal.Direction = System.Data.ParameterDirection.Input;
                    sal.Value = salary;
                    cmd.Parameters.Add(sal);
                    int res = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public void SetDailyHours(int idDipendente, TimeSpan hours)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SetOreGiornaliereByIdDipendente";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    var pore = new MySqlParameter("ore", MySqlDbType.Time);
                    pore.Direction= System.Data.ParameterDirection.Input;
                    pore.Value = hours;
                    cmd.Parameters.Add(pore);

                    var pid = new MySqlParameter("pid", MySqlDbType.Int32);
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = idDipendente;
                    cmd.Parameters.Add(pid);
                    
                    int res = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void SetMonthlyHours(int idDipendente, TimeSpan hours)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SetOreMensiliByIdDipendente";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    int ore = (hours.Days * 24) + hours.Hours;

                    var pore = new MySqlParameter("ore", MySqlDbType.Time);
                    pore.Direction = System.Data.ParameterDirection.Input;
                    //pore.Value = string.Format("{0}:{1}", ore, hours.ToString("%mm"));
                    pore.Value = hours;
                    cmd.Parameters.Add(pore);

                    var pid = new MySqlParameter("pid", MySqlDbType.Int32);
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = idDipendente;
                    cmd.Parameters.Add(pid);

                    int res = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public decimal GetAgreedSalary(int idDipendente)
        {
            decimal salary = 0;
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "GetSalarioPattuitoByIdDipendente";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var pid = new MySqlParameter("pid", MySqlDbType.Int32);
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = idDipendente;
                    cmd.Parameters.Add(pid);
                    var rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        if(!rdr.IsDBNull(1))
                            salary = rdr.GetDecimal(1);
                    }

                }
                catch (Exception)
                {

                    throw;
                }
                return salary;
            }

        }

        public TimeSpan GetDailyHours(int idDipendente)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0);
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "select ore_giornaliere FROM turni.dati_economici where id_dipendente=@IdDipendente;";
                    cmd.CommandType = System.Data.CommandType.Text;
                    var pid = new MySqlParameter("IdDipendente", MySqlDbType.Int32);
                    pid.MySqlDbType = MySqlDbType.Int32;
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = idDipendente;
                    cmd.Parameters.Add(pid);
                    var rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        if(!rdr.IsDBNull(0))
                            ts = rdr.GetTimeSpan(0);
                    }

                }
                catch (Exception)
                {
                    throw;
                }
                return ts;
            }
        }

        public TimeSpan GetMonthlyHours(int idDipendente)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0);
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "select ore_mensili FROM turni.dati_economici where id_dipendente=@IdDipendente;";
                    cmd.CommandType = System.Data.CommandType.Text;
                    var pid = new MySqlParameter("IdDipendente", MySqlDbType.Int32);
                    pid.MySqlDbType = MySqlDbType.Int32;
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = idDipendente;
                    cmd.Parameters.Add(pid);
                    var rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                            ts = rdr.GetTimeSpan(0);
                    }

                }
                catch (Exception)
                {
                    throw;
                }
                return ts;
            }
        }
    }
}
