using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkPlan
{
  
    public partial class DlgInfo : Form
    {
        string assemblyTitle;
        Version assemblyVersion;
        string assemblyCopyright;

        public DlgInfo()
        {
            InitializeComponent();

            Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
            var assemblyTitleAttribute = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false).FirstOrDefault() as AssemblyTitleAttribute;
            var assemblyCopyrightAttribute = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false).FirstOrDefault() as AssemblyCopyrightAttribute;

            assemblyTitle = assemblyTitleAttribute.Title;
            assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            assemblyCopyright = assemblyCopyrightAttribute.Copyright;
            DateTime linkTimeLocal;
            
            label1.Text = assemblyTitle.ToString();
            label2.Text = string.Format("{0} v.{1}", assemblyTitle, assemblyVersion.ToString());
            label3.Text = string.Format("{0} Tiziano Michelessi", assemblyCopyright);

            try
            {
                linkTimeLocal = AssemblyBuildTime.GetLinkerTime(assembly);
                label4.Text = string.Format("Data ultima compilazione: {0} {1}", linkTimeLocal.ToLongDateString(), linkTimeLocal.ToLongTimeString());
            }
            catch (Exception)
            {
                label4.Text = "N.D.";
            }
        }

        
    }

}
