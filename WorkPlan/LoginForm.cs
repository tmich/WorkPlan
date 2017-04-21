using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkPlan
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            DialogResult = DialogResult.Cancel;
            Text = string.Format("Accesso {0}", MyAppDomain.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(txtUsername.Text.Trim().Equals(string.Empty) ||
               txtPassword.Text.Trim().Equals(string.Empty))
            {
                GuiUtils.Warning("Inserire nome utente e password", this, "Attenzione");
            }
            else
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;

                if (User.Login(username, password))
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    GuiUtils.Warning("Accesso non riuscito", this, "Attenzione");
                }
            }
        }
    }
}
