using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    class MyAppDomain
    {
        public new static string ToString()
        {
            var attributes = typeof(Program).GetTypeInfo().Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute));
            var assemblyTitleAttribute = attributes.SingleOrDefault() as AssemblyTitleAttribute;

            string assemblyTitle = assemblyTitleAttribute.Title;
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string assemblyVersion = string.Format("{0}.{1}", version.Major, version.Minor);
            return string.Format("{0} v.{1}", assemblyTitle, assemblyVersion);
        }
    }
}
