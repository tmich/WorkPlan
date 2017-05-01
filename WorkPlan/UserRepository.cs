using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class UserRepository
    {
        protected string connstr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();

        public User GetUserById(int UserId)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    int userid = 0;
                    string username ="", profilo="";
                    Profile up = new UserProfile();

                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = string.Format("select id, username, profilo from utenti where id = {0};", UserId);

                    var recordset = cmd.ExecuteReader();

                    if (!recordset.HasRows)
                    {
                        return null;
                    }

                    if(recordset.Read())
                    {
                        userid = recordset.GetInt32(0);
                        username = recordset.GetString(1);
                        profilo = recordset.GetString(2);
                        if (profilo.Equals("A"))
                            up = new AdminProfile();
                    }

                    conn.Close();

                    return new User(userid, username, up);
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }
    }
}
