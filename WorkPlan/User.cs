using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    
    class User
    {
        protected static string connstr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();

        protected User(string username, Profile profile)
        {
            Username = username;
            Profile = profile;
        }

        public bool IsAdmin()
        {
            return Profile.IsAdmin();
        }

        public bool IsAuthorized(Function function)
        {
            return Profile.IsAuthorized(function);
        }
        
        public bool CanDelete(NoworkVM noworkVM)
        {
            return Profile.CanDelete(noworkVM);
        }

        public bool CanEdit(NoworkVM noworkVM)
        {
            return Profile.CanEdit(noworkVM);
        }

        public static bool Login(string username, string password)
        {
            bool ret = false;
            
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = string.Format("select username, profilo from utenti where username='{0}' and password = '{1}';", username, password);

                var rdr = cmd.ExecuteReader();
                
                if(rdr.Read())
                {
                    Profile p = null;
                    ret = true;
                    char prof = rdr.GetChar(1);

                    switch(prof)
                    {
                        case 'A':
                            p = new AdminProfile();
                            break;
                        default:
                            p = new UserProfile();
                            break;
                    }

                    CurrentUser = new User(username, p);
                }

                conn.Close();
            }
            
            return ret;
        }

        public static User CurrentUser
        {
            get;
            set;
        }

        public Profile Profile
        {
            get;
            protected set;
        }

        public string Username
        {
            get;
            protected set;
        }

        public bool CanDelete(DutyVM dutyVm)
        {
            return Profile.CanDelete(dutyVm);
        }

        public bool CanEdit(DutyVM dutyVM)
        {
            return Profile.CanEdit(dutyVM);
        }
    }
}
