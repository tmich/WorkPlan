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
        public static DialogResult Error(string message, IWin32Window parent, string title=null)
        {
            return MessageBox.Show(parent, message, (title == null ? AppDomain.CurrentDomain.FriendlyName : title), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult Warning(string message, IWin32Window parent, string title = null)
        {
            return MessageBox.Show(parent, message, (title == null ? AppDomain.CurrentDomain.FriendlyName : title), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static DialogResult Confirm(string message, IWin32Window parent, string title = null)
        {
            return MessageBox.Show(parent, message, (title == null ? AppDomain.CurrentDomain.FriendlyName : title), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult Notify(string message, IWin32Window parent, string title = null)
        {
            return MessageBox.Show(parent, message, (title == null ? AppDomain.CurrentDomain.FriendlyName : title), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
