using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace WorkPlan
{
    public class EmployeeRepository
    {
        protected string connstr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();

        public EmployeeRepository()
        {
        
        }

        public Employee GetById(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    Employee employee = null;
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "GetDipendenteById";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var pid = new MySqlParameter("pid", MySqlDbType.Int32);
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = id;
                    cmd.Parameters.Add(pid);
                    var rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        //id, nome, cognome, codice_fiscale, indirizzo, citta, telefono, 
                        //cellulare, matricola, qualifica, data_assunzione, email, data_nascita

                        employee = new Employee()
                        {
                            Id = rdr.GetInt32(0),
                            Name = rdr.GetString(1),
                            LastName = rdr.GetString(2),
                            CodFisc = rdr.GetString(3),
                            Address = rdr.IsDBNull(4) ? "" : rdr.GetString(4),
                            City = rdr.IsDBNull(5) ? "" : rdr.GetString(5),
                            Telephone = rdr.IsDBNull(6) ? "" : rdr.GetString(6),
                            MobileNo = rdr.IsDBNull(7) ? "" : rdr.GetString(7),
                            Matr = rdr.IsDBNull(8) ? "" : rdr.GetString(8),
                            Qual = rdr.IsDBNull(9) ? "" : rdr.GetString(9),
                            HireDate = rdr.IsDBNull(10) ? new DateTime(1900, 1, 1) : rdr.GetDateTime(10),
                            Email = rdr.IsDBNull(11) ? "" : rdr.GetString(11),
                            BirthDate = rdr.GetDateTime(12),
                            DefaultPosition = new Position(rdr.GetInt32(13), rdr.GetString(14)),
                            AddressDom = rdr.IsDBNull(15) ? "" : rdr.GetString(15),
                            CityDom = rdr.IsDBNull(16) ? "" : rdr.GetString(16),
                            Nationality = rdr.IsDBNull(17) ? "" : rdr.GetString(17),
                            BirthCity = rdr.IsDBNull(18) ? "" : rdr.GetString(18),
                            MobileNo2 = rdr.IsDBNull(19) ? "" : rdr.GetString(19)
                        };
                    }

                    return employee;
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }

        public List<Employee> All()
        {

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    List<Employee> employees = new List<Employee>();
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "GetDipendenti";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        //id, nome, cognome, codice_fiscale, indirizzo, citta, telefono, 
                        //cellulare, matricola, qualifica, data_assunzione, email, data_nascita
                        
                        employees.Add(new Employee()
                        {
                            Id = rdr.GetInt32(0),
                            Name = rdr.GetString(1),
                            LastName = rdr.GetString(2),
                            CodFisc = rdr.GetString(3),
                            Address = rdr.IsDBNull(4) ? "" : rdr.GetString(4),
                            City = rdr.IsDBNull(5) ? "" : rdr.GetString(5),
                            Telephone = rdr.IsDBNull(6) ? "" : rdr.GetString(6),
                            MobileNo = rdr.IsDBNull(7) ? "" : rdr.GetString(7),
                            Matr = rdr.IsDBNull(8) ? "" : rdr.GetString(8),
                            Qual = rdr.IsDBNull(9) ? "" : rdr.GetString(9),
                            HireDate = rdr.IsDBNull(10) ? new DateTime(1900,1,1) : rdr.GetDateTime(10),
                            Email = rdr.IsDBNull(11) ? "" : rdr.GetString(11),
                            BirthDate = rdr.GetDateTime(12),
                            DefaultPosition = new Position(rdr.GetInt32(13), rdr.GetString(14)),
                            AddressDom = rdr.IsDBNull(15) ? "" : rdr.GetString(15),
                            CityDom = rdr.IsDBNull(16) ? "" : rdr.GetString(16),
                            Nationality = rdr.IsDBNull(17) ? "" : rdr.GetString(17),
                            BirthCity = rdr.IsDBNull(18) ? "" : rdr.GetString(18),
                            MobileNo2 = rdr.IsDBNull(19) ? "" : rdr.GetString(19)
                        });
                    }

                    employees.Sort((x, y) => x.LastName.CompareTo(y.LastName));
                    return employees;
                }
                catch (SqlException)
                {
                    throw;
                }
            }
        }

        public void Delete(int EmployeeId)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "DeleteDipendente";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var pid = new MySqlParameter("pid", MySqlDbType.Int32);
                    pid.Direction = System.Data.ParameterDirection.Input;
                    pid.Value = EmployeeId;
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

        public void Add(ref Employee employee)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "InsertDipendente";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("pnome", MySqlDbType.VarChar, 50).Value = employee.Name;
                    cmd.Parameters.Add("pcognome", MySqlDbType.VarChar, 50).Value = employee.LastName;
                    cmd.Parameters.Add("pcodfisc", MySqlDbType.VarChar, 16).Value = employee.CodFisc;
                    cmd.Parameters.Add("pindirizzo", MySqlDbType.VarChar, 100).Value = employee.Address;
                    cmd.Parameters.Add("pcitta", MySqlDbType.VarChar, 50).Value = employee.City;
                    cmd.Parameters.Add("ptelefono", MySqlDbType.VarChar, 20).Value = employee.Telephone;
                    cmd.Parameters.Add("pcellulare", MySqlDbType.VarChar, 15).Value = employee.MobileNo;
                    cmd.Parameters.Add("pmatricola", MySqlDbType.VarChar, 10).Value = employee.Matr;
                    cmd.Parameters.Add("pqualifica", MySqlDbType.VarChar, 20).Value = employee.Qual;
                    cmd.Parameters.Add("pdataass", MySqlDbType.Date).Value = employee.HireDate;
                    cmd.Parameters.Add("pemail", MySqlDbType.VarChar, 50).Value = employee.Email;
                    cmd.Parameters.Add("pdatanascita", MySqlDbType.Date).Value = employee.BirthDate;
                    cmd.Parameters.Add("preparto", MySqlDbType.Int32).Value = employee.DefaultPosition.Id;
                    cmd.Parameters.Add("pindirizzodom", MySqlDbType.VarChar, 100).Value = employee.AddressDom;
                    cmd.Parameters.Add("pcittadom", MySqlDbType.VarChar, 50).Value = employee.CityDom;
                    cmd.Parameters.Add("pnazionalita", MySqlDbType.VarChar, 50).Value = employee.Nationality;
                    cmd.Parameters.Add("pcittanasc", MySqlDbType.VarChar, 50).Value = employee.BirthCity;
                    cmd.Parameters.Add("pcellulare2", MySqlDbType.VarChar, 15).Value = employee.MobileNo2;

                    var lid = new MySqlParameter("LID", MySqlDbType.Int32);
                    lid.Direction = System.Data.ParameterDirection.InputOutput;
                    cmd.Parameters.Add(lid);

                    cmd.ExecuteNonQuery();

                    employee.Id = (int)lid.Value;

                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }

        public void Update(Employee employee)
        {
            if (employee.Id == 0)
            {
                throw new InvalidOperationException("Inserire il dipendente nel database");
            }

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UpdateDipendente";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("pid", MySqlDbType.Int32).Value = employee.Id;
                    cmd.Parameters.Add("pnome", MySqlDbType.VarChar, 50).Value = employee.Name;
                    cmd.Parameters.Add("pcognome", MySqlDbType.VarChar, 50).Value = employee.LastName;
                    cmd.Parameters.Add("pcodfisc", MySqlDbType.VarChar, 16).Value = employee.CodFisc;
                    cmd.Parameters.Add("pindirizzo", MySqlDbType.VarChar, 100).Value = employee.Address;
                    cmd.Parameters.Add("pcitta", MySqlDbType.VarChar, 50).Value = employee.City;
                    cmd.Parameters.Add("ptelefono", MySqlDbType.VarChar, 20).Value = employee.Telephone;
                    cmd.Parameters.Add("pcellulare", MySqlDbType.VarChar, 15).Value = employee.MobileNo;
                    cmd.Parameters.Add("pmatricola", MySqlDbType.VarChar, 10).Value = employee.Matr;
                    cmd.Parameters.Add("pqualifica", MySqlDbType.VarChar, 20).Value = employee.Qual;
                    cmd.Parameters.Add("pdataass", MySqlDbType.Date).Value = employee.HireDate;
                    cmd.Parameters.Add("pemail", MySqlDbType.VarChar, 50).Value = employee.Email;
                    cmd.Parameters.Add("pdatanascita", MySqlDbType.Date).Value = employee.BirthDate;
                    cmd.Parameters.Add("preparto", MySqlDbType.Int32).Value = employee.DefaultPosition.Id;
                    cmd.Parameters.Add("pindirizzodom", MySqlDbType.VarChar, 100).Value = employee.AddressDom;
                    cmd.Parameters.Add("pcittadom", MySqlDbType.VarChar, 50).Value = employee.CityDom;
                    cmd.Parameters.Add("pnazionalita", MySqlDbType.VarChar, 50).Value = employee.Nationality;
                    cmd.Parameters.Add("pcittanasc", MySqlDbType.VarChar, 50).Value = employee.BirthCity;
                    cmd.Parameters.Add("pcellulare2", MySqlDbType.VarChar, 15).Value = employee.MobileNo2;

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
