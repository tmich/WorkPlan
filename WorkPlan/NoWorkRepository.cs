using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace WorkPlan
{
    public class NoWorkRepository
    {
        protected string connstr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();

        public NoWorkRepository()
        {
        }

        public List<NoWorkReason> GetReasons()
        {
            List<NoWorkReason> reasons = new List<NoWorkReason>();

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GetMotiviAssenza";
                    
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var reason = new NoWorkReason();
                        reason.Id = rdr.GetInt32(0);
                        reason.Value = rdr.GetString(2);
                        reasons.Add(reason);
                    }
                    
                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }

            return reasons;
        }

        public List<NoWork> GetAssenzeByDipDateRange(Employee employee, DateTime startDate, DateTime endDate)
        {
            var results = new List<NoWork>();

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GetAssenzeByDipDateRange";
                    cmd.Parameters.Add("pDipendenteId", MySqlDbType.Int32).Value = employee.Id;
                    cmd.Parameters.Add("pData1", MySqlDbType.DateTime).Value = startDate.Date;
                    cmd.Parameters.Add("pData2", MySqlDbType.DateTime).Value = endDate.Date;

                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        results.Add(new NoWork()
                        {
                            Id = rdr.GetInt32(0),
                            Employee = employee,
                            StartDate = rdr.GetDateTime(2),
                            EndDate = rdr.GetDateTime(3),
                            Reason = new NoWorkReason() { Id = rdr.GetInt32(4), Value = rdr.GetString(5) },
                            Notes = rdr.IsDBNull(7) ? String.Empty : rdr.GetString(7),
                            FullDay = rdr.GetBoolean(8)
                        });
                    }

                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }

            results.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));
            return results;
        }

        public List<NoWork> GetAssenzeByEmployeeAndDate(Employee employee, DateTime datetime)
        {
            var results = new List<NoWork>();

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GetAssenzeByDipDate";
                    cmd.Parameters.Add("pDipendenteId", MySqlDbType.Int32).Value = employee.Id;
                    cmd.Parameters.Add("pDataInizio", MySqlDbType.DateTime).Value = datetime.Date;

                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        results.Add(new NoWork()
                        {
                            Id = rdr.GetInt32(0),
                            Employee = employee,
                            StartDate = rdr.GetDateTime(2),
                            EndDate = rdr.GetDateTime(3),
                            Reason = new NoWorkReason() { Id = rdr.GetInt32(4), Value = rdr.GetString(5) },
                            Notes = rdr.IsDBNull(7) ? String.Empty : rdr.GetString(7),
                            FullDay = rdr.GetBoolean(8)
                        });
                    }

                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }

            results.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));
            return results;
        }

        internal IEnumerable<object> GetAssenzeByDateRange(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public void Add(ref NoWork nowork)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "InsertAssenza";
                    cmd.Parameters.Add("pDipendenteId", MySqlDbType.Int32).Value = nowork.Employee.Id;
                    cmd.Parameters.Add("pDataInizio", MySqlDbType.DateTime).Value = nowork.StartDate;
                    cmd.Parameters.Add("pDataFine", MySqlDbType.DateTime).Value = nowork.EndDate;
                    cmd.Parameters.Add("pGiornataIntera", MySqlDbType.Int16).Value = nowork.FullDay;
                    cmd.Parameters.Add("pMotivo", MySqlDbType.VarChar, 50).Value = nowork.Reason.Id;
                    cmd.Parameters.Add("pNote", MySqlDbType.VarChar, 50).Value = nowork.Notes;

                    var lid = new MySqlParameter("LID", MySqlDbType.Int32);
                    lid.Direction = System.Data.ParameterDirection.InputOutput;
                    cmd.Parameters.Add(lid);

                    cmd.ExecuteNonQuery();

                    nowork.Id = (int)lid.Value;

                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }

        public void Update(ref NoWork nw)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "UpdateAssenza";
                    cmd.Parameters.Add("pId", MySqlDbType.Int32).Value = nw.Id;
                    cmd.Parameters.Add("pDataInizio", MySqlDbType.DateTime).Value = nw.StartDate;
                    cmd.Parameters.Add("pDataFine", MySqlDbType.DateTime).Value = nw.EndDate;
                    cmd.Parameters.Add("pGiornataIntera", MySqlDbType.Int16).Value = nw.FullDay;
                    cmd.Parameters.Add("pMotivo", MySqlDbType.VarChar, 50).Value = nw.Reason.Id;
                    cmd.Parameters.Add("pNote", MySqlDbType.VarChar, 50).Value = nw.Notes;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }
    }
}
