using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class User
    {
        protected static string connstr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();

        protected User(string username, Profile profile)
        {
            Username = username;
            Profile = profile;
        }

        public User(int id, string username, Profile profile)
            : this(username, profile)
        {
            Id = id;
        }

        public bool IsAdmin()
        {
            return Profile.IsAdmin();
        }

        public bool IsAuthorized(Function function)
        {
            return Profile.IsAuthorized(function);
        }

        public bool CanDelete(IShiftVM shift)
        {
            if (shift.User.Equals(this))
                return true;

            return Profile.IsAdmin();
        }

        internal bool CanEdit(IShiftVM shift)
        {
            if (shift.User.Equals(this))
                return true;

            return Profile.IsAdmin();
        }

        //public bool CanDelete(NoworkVM noworkVM)
        //{
        //    if (noworkVM.User.Equals(this))
        //        return true;

        //    return Profile.CanDelete(noworkVM);
        //}

        //public bool CanEdit(NoworkVM noworkVM)
        //{
        //    if (noworkVM.User.Equals(this))
        //        return true;

        //    return Profile.CanEdit(noworkVM);
        //}

        public static bool Login(string username, string password)
        {
            bool ret = false;
            
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = string.Format("select id, username, profilo from utenti where username='{0}' and password = '{1}';", username, password);

                var rdr = cmd.ExecuteReader();
                
                if(rdr.Read())
                {
                    Profile p = null;
                    ret = true;
                    int id = rdr.GetInt32(0);
                    char prof = rdr.GetChar(2);

                    switch(prof)
                    {
                        case 'A':
                            p = new AdminProfile();
                            break;
                        default:
                            p = new UserProfile();
                            break;
                    }

                    CurrentUser = new User(id, username, p);
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

        public int Id { get; protected set; }

        //public bool CanDelete(DutyVM dutyVm)
        //{
        //    if (dutyVm.User.Equals(this))
        //        return true;

        //    return Profile.CanDelete(dutyVm);
        //}

        //public bool CanEdit(DutyVM dutyVm)
        //{
        //    if (dutyVm.User.Equals(this))
        //        return true;

        //    return Profile.CanEdit(dutyVm);
        //}

        public override bool Equals(object obj)
        {
            User rhs = obj as User;
            if (rhs == null)
                return false;

            return rhs.Id == this.Id;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + Username.GetHashCode();
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + this.Profile.GetHashCode();
                return hash;
            }
        }
    }
}
