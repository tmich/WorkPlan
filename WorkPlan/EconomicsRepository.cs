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
    }
}
