using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkPlan
{
    public class GuiUtils
    {
        public static DialogResult Error(string message, IWin32Window parent)
        {
            return MessageBox.Show(parent, message, AppDomain.CurrentDomain.FriendlyName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult Warning(string message, IWin32Window parent)
        {
            return MessageBox.Show(parent, message, AppDomain.CurrentDomain.FriendlyName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static DialogResult Confirm(string message, IWin32Window parent)
        {
            return MessageBox.Show(parent, message, AppDomain.CurrentDomain.FriendlyName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult Notify(string message, IWin32Window parent)
        {
            return MessageBox.Show(parent, message, AppDomain.CurrentDomain.FriendlyName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
