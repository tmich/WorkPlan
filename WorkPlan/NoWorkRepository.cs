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

        public void AddReason(ref NoWorkReason nwr)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = string.Format("insert into motivo_assenza (codice, descrizione) values ('{0}', '{1}')",
                        nwr.Code, nwr.Value);

                    int rows = cmd.ExecuteNonQuery();
                    
                    if (rows == 0)
                    {
                        throw new InvalidOperationException();
                    }

                    nwr.Id = (int)cmd.LastInsertedId;

                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
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
                        reason.Code = rdr.GetString(1);
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

        public void UpdateReason(ref NoWorkReason nwr)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = string.Format("update motivo_assenza set codice = '{0}', descrizione = '{1}' where id = {2}",
                        nwr.Code, nwr.Value, nwr.Id);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows == 0)
                    {
                        throw new InvalidOperationException();
                    }

                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
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
                            Reason = new NoWorkReason() { Id = rdr.GetInt32(4), Value = rdr.GetString(5), Code = rdr.GetString(6) },
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
                            Reason = new NoWorkReason() { Id = rdr.GetInt32(4), Value = rdr.GetString(5), Code = rdr.GetString(6) },
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

        public void Delete(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "DeleteAssenza";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var pid = new MySqlParameter("pid", MySqlDbType.Int32);
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = id;
                    cmd.Parameters.Add(pid);

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }

        public IEnumerable<NoWork> GetAssenzeByDateRange(DateTime startDate, DateTime endDate)
        {
            //GetAssenzeByDateRange
            var results = new List<NoWork>();

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GetAssenzeByDateRange";
                    cmd.Parameters.Add("pData1", MySqlDbType.DateTime).Value = startDate.Date;
                    cmd.Parameters.Add("pData2", MySqlDbType.DateTime).Value = endDate.Date;

                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        results.Add(new NoWork()
                        {
                            Id = rdr.GetInt32(0),
                            StartDate = rdr.GetDateTime(2),
                            EndDate = rdr.GetDateTime(3),
                            Reason = new NoWorkReason() { Id = rdr.GetInt32(4), Value = rdr.GetString(5) },
                            Notes = rdr.IsDBNull(7) ? String.Empty : rdr.GetString(7),
                            FullDay = rdr.GetBoolean(8),
                            Employee = new Employee()
                            {
                                Id=rdr.GetInt32(9),
                                Name = rdr.GetString(10),
                                LastName = rdr.GetString(11),
                                CodFisc = rdr.GetString(12),
                                Address = rdr.IsDBNull(13) ? "" : rdr.GetString(13),
                                City = rdr.IsDBNull(14) ? "" : rdr.GetString(14),
                                Telephone = rdr.IsDBNull(15) ? "" : rdr.GetString(15),
                                MobileNo = rdr.IsDBNull(16) ? "" : rdr.GetString(16),
                                Matr = rdr.IsDBNull(17) ? "" : rdr.GetString(17),
                                Qual = rdr.IsDBNull(18) ? "" : rdr.GetString(18),
                                HireDate = rdr.IsDBNull(19) ? new DateTime(1900, 1, 1) : rdr.GetDateTime(19),
                                Email = rdr.IsDBNull(20) ? "" : rdr.GetString(20),
                                BirthDate = rdr.GetDateTime(21)
                            }
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
