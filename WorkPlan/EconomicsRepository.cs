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

        public void AddBusta(ref BustaPaga busta)
        {
            bool duplicate = false;

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "AddBustaByIdDipendente";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    var pMese = new MySqlParameter("Mese", MySqlDbType.Int32);
                    pMese.Direction = System.Data.ParameterDirection.Input;
                    pMese.Value = busta.Mese;
                    cmd.Parameters.Add(pMese);

                    var pAnno = new MySqlParameter("Anno", MySqlDbType.Int32);
                    pAnno.Direction = System.Data.ParameterDirection.Input;
                    pAnno.Value = busta.Anno;
                    cmd.Parameters.Add(pAnno);

                    var pImp = new MySqlParameter("Importo", MySqlDbType.Double);
                    pImp.Direction = System.Data.ParameterDirection.Input;
                    pImp.Value = busta.Importo;
                    cmd.Parameters.Add(pImp);

                    var pid = new MySqlParameter("pid", MySqlDbType.Int32);
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = busta.IdDip;
                    cmd.Parameters.Add(pid);

                    var newId = new MySqlParameter("LID", MySqlDbType.Int32);
                    newId.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(newId);

                    int res = cmd.ExecuteNonQuery();

                    if(res > 0)
                    {
                        busta = new BustaPaga(busta, int.Parse(newId.Value.ToString()));
                    }
                }
                catch(MySqlException msex)
                {
                    if(msex.Number == 1062)
                    {
                        duplicate = true;
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            if(duplicate)
                throw new InvalidOperationException("Periodo già inserito");
        }

        public List<BustaPaga> GetBuste(int idDipendente)
        {
            List<BustaPaga> buste = new List<BustaPaga>();

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "GetBusteByIdDipendente";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    var pid = new MySqlParameter("pid", MySqlDbType.Int32);
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = idDipendente;
                    cmd.Parameters.Add(pid);

                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        int id = rdr.GetInt32(0);
                        int dipid = rdr.GetInt32(1);
                        int mese = rdr.GetInt32(2);
                        int anno = rdr.GetInt32(3);
                        double importo = rdr.GetDouble(4);

                        BustaPaga bp = new BustaPaga(id, dipid, mese, anno, importo);
                        buste.Add(bp);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            
            return buste;
        }

        public void DeleteBusta(BustaPaga busta)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "DeleteBusta";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    var pid = new MySqlParameter("pid", MySqlDbType.Int32);
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = busta.Id;
                    cmd.Parameters.Add(pid);

                    int res = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void SetBusta(int idDipendente, bool hasBusta)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SetBustaByIdDipendente";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    var pBusta = new MySqlParameter("pbusta", MySqlDbType.Bit);
                    pBusta.Direction = System.Data.ParameterDirection.Input;
                    pBusta.Value = hasBusta;
                    cmd.Parameters.Add(pBusta);

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

        public bool HasBusta(int idDipendente)
        {
            bool hasBusta = false;

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "select ifnull(busta, 0) FROM turni.dati_economici where id_dipendente=@IdDipendente;";
                    cmd.CommandType = System.Data.CommandType.Text;
                    var pid = new MySqlParameter("IdDipendente", MySqlDbType.Int32);
                    pid.MySqlDbType = MySqlDbType.Int32;
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = idDipendente;
                    cmd.Parameters.Add(pid);
                    int i = 0;
                    var obj = cmd.ExecuteScalar();
                    if (obj != null)
                    {
                        i = int.Parse(obj.ToString());
                    }
                    hasBusta = i == 1;
                }
                catch (Exception)
                {

                    throw;
                }

                return hasBusta;
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

        public void UpdateBusta(BustaPaga busta)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UpdateBusta";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    var pMese = new MySqlParameter("Mese", MySqlDbType.Int32);
                    pMese.Direction = System.Data.ParameterDirection.Input;
                    pMese.Value = busta.Mese;
                    cmd.Parameters.Add(pMese);

                    var pAnno = new MySqlParameter("Anno", MySqlDbType.Int32);
                    pAnno.Direction = System.Data.ParameterDirection.Input;
                    pAnno.Value = busta.Anno;
                    cmd.Parameters.Add(pAnno);

                    var pImp = new MySqlParameter("Importo", MySqlDbType.Double);
                    pImp.Direction = System.Data.ParameterDirection.Input;
                    pImp.Value = busta.Importo;
                    cmd.Parameters.Add(pImp);

                    var pid = new MySqlParameter("pid", MySqlDbType.Int32);
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = busta.Id;
                    cmd.Parameters.Add(pid);
                    

                    int res = cmd.ExecuteNonQuery();

                    if (res == 0)
                    {
                        throw new InvalidOperationException("Aggiornamento non riuscito");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
