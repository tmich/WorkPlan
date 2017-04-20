using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkPlan
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                LoginForm lform = new LoginForm();
                lform.ShowDialog();
                if(lform.DialogResult == DialogResult.OK)
                {
                    Application.Run(new Form1());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            
        }
    }
}
