using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class PositionDao
    {
        protected string connstr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();

        public IEnumerable<Position> GetAll()
        {
            List<Position> positions = new List<Position>();

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GetReparti";

                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var pos = new Position(rdr.GetInt32(0), rdr.GetString(1));
                        positions.Add(pos);
                    }

                    conn.Close();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }

            return positions;
        }
    }
}
