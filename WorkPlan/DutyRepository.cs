using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace WorkPlan
{
    public class DutyRepository
    {
        protected string connstr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();

        public DutyRepository()
        {
        }

        public List<Duty> GetCassaBy(Employee employee, DateTime startDate, DateTime endDate)
        {
            var results = new List<Duty>();

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GetTurniCassaByDipDateRange";
                    cmd.Parameters.Add("pDipendenteId", MySqlDbType.Int32).Value = employee.Id;
                    cmd.Parameters.Add("pData1", MySqlDbType.DateTime).Value = startDate.Date;
                    cmd.Parameters.Add("pData2", MySqlDbType.DateTime).Value = endDate.Date;

                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        results.Add(new Duty()
                        {
                            Id = rdr.GetInt32(0),
                            Employee = employee,
                            StartDate = rdr.GetDateTime(2),
                            EndDate = rdr.GetDateTime(3),
                            Position = rdr.GetString(4),
                            Notes = rdr.IsDBNull(5) ? String.Empty : rdr.GetString(5),
                            User = new User(rdr.GetInt32(6), rdr.GetString(7), Profile.Create(rdr.GetString(8)))
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

        public List<Duty> GetBy(Employee employee, DateTime startDate, DateTime endDate)
        {
            var results = new List<Duty>();

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GetTurniByDipDateRange";
                    cmd.Parameters.Add("pDipendenteId", MySqlDbType.Int32).Value = employee.Id;
                    cmd.Parameters.Add("pData1", MySqlDbType.DateTime).Value = startDate.Date;
                    cmd.Parameters.Add("pData2", MySqlDbType.DateTime).Value = endDate.Date;

                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        results.Add(new Duty()
                        {
                            Id = rdr.GetInt32(0),
                            Employee = employee,
                            StartDate = rdr.GetDateTime(2),
                            EndDate = rdr.GetDateTime(3),
                            Position = rdr.GetString(4),
                            Notes = rdr.IsDBNull(5) ? String.Empty : rdr.GetString(5),
                            User = new User(rdr.GetInt32(6), rdr.GetString(7), Profile.Create(rdr.GetString(8)))
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

        public List<Duty> GetBy(DateTime startDate, DateTime endDate)
        {
            var results = new List<Duty>();

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GetTurniByDateRange";
                    cmd.Parameters.Add("pData1", MySqlDbType.DateTime).Value = startDate.Date;
                    cmd.Parameters.Add("pData2", MySqlDbType.DateTime).Value = endDate.Date;

                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        results.Add(new Duty()
                        {
                            Id = rdr.GetInt32(0),
                            StartDate = rdr.GetDateTime(1),
                            EndDate = rdr.GetDateTime(2),
                            Position = rdr.GetString(3),
                            Notes = rdr.IsDBNull(4) ? String.Empty : rdr.GetString(4),
                            Employee = new Employee() {
                                Id = rdr.GetInt32(5),
                                Name = rdr.GetString(6),
                                LastName = rdr.GetString(7),
                                CodFisc = rdr.GetString(8),
                                Address = rdr.IsDBNull(9) ? "" : rdr.GetString(9),
                                City = rdr.IsDBNull(10) ? "" : rdr.GetString(10),
                                Telephone = rdr.IsDBNull(11) ? "" : rdr.GetString(11),
                                MobileNo = rdr.IsDBNull(12) ? "" : rdr.GetString(12),
                                Matr = rdr.IsDBNull(13) ? "" : rdr.GetString(13),
                                Qual = rdr.IsDBNull(14) ? "" : rdr.GetString(14),
                                HireDate = rdr.IsDBNull(15) ? new DateTime(1900, 1, 1) : rdr.GetDateTime(15),
                                Email = rdr.IsDBNull(16) ? "" : rdr.GetString(16),
                                BirthDate = rdr.GetDateTime(17),
                                DefaultPosition = new Position(rdr.IsDBNull(18) ? 0 : rdr.GetInt32(18), rdr.IsDBNull(19) ? "" : rdr.GetString(19))
                            },
                            User = new User(rdr.GetInt32(20), rdr.GetString(21), Profile.Create(rdr.GetString(22)))
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
            //results.Sort((x, y) => x.Employee.FullName.CompareTo(y.Employee.FullName));
            return results;
        }

        public List<Duty> GetBy(Employee employee, DateTime startDate)
        {
            var results = new List<Duty>();

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GetTurniByDipDate";
                    cmd.Parameters.Add("pDipendenteId", MySqlDbType.Int32).Value = employee.Id;
                    cmd.Parameters.Add("pDataInizio", MySqlDbType.DateTime).Value = startDate.Date;

                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        results.Add(new Duty()
                        {
                            Id = rdr.GetInt32(0),
                            Employee = employee,
                            StartDate = rdr.GetDateTime(2),
                            EndDate = rdr.GetDateTime(3),
                            Position = rdr.GetString(4),
                            Notes = rdr.IsDBNull(5) ? String.Empty : rdr.GetString(5),
                            User = new User(rdr.GetInt32(6), rdr.GetString(7), Profile.Create(rdr.GetString(8)))
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

        public DutyList All()
        {
            return new DutyList();
        }

        public void Add(ref Duty duty)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "InsertTurno";
                    cmd.Parameters.Add("pDipendenteId", MySqlDbType.Int32).Value=duty.Employee.Id;
                    cmd.Parameters.Add("pDataInizio", MySqlDbType.DateTime).Value = duty.StartDate;
                    cmd.Parameters.Add("pDataFine", MySqlDbType.DateTime).Value = duty.EndDate;
                    cmd.Parameters.Add("pPosizione", MySqlDbType.VarChar, 50).Value = duty.Position;
                    cmd.Parameters.Add("pNote", MySqlDbType.VarChar, 50).Value = duty.Notes;
                    cmd.Parameters.Add("pUtenteId", MySqlDbType.Int32, 50).Value = duty.User.Id;

                    var lid = new MySqlParameter("LID", MySqlDbType.Int32);
                    lid.Direction = System.Data.ParameterDirection.InputOutput;
                    cmd.Parameters.Add(lid);

                    cmd.ExecuteNonQuery();

                    duty.Id = (int)lid.Value;

                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }

        public void Update(ref Duty duty)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "UpdateTurno";
                    cmd.Parameters.Add("pId", MySqlDbType.Int32).Value = duty.Id;
                    cmd.Parameters.Add("pDataInizio", MySqlDbType.DateTime).Value = duty.StartDate;
                    cmd.Parameters.Add("pDataFine", MySqlDbType.DateTime).Value = duty.EndDate;
                    cmd.Parameters.Add("pPosizione", MySqlDbType.VarChar, 50).Value = duty.Position;
                    cmd.Parameters.Add("pNote", MySqlDbType.VarChar, 50).Value = duty.Notes;
                    
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }

        public void Delete(int DutyId)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "DeleteTurno";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var pid = new MySqlParameter("pid", MySqlDbType.Int32);
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = DutyId;
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
    }
}
