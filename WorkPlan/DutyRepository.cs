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
                            Notes = rdr.IsDBNull(5) ? String.Empty : rdr.GetString(5)
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
