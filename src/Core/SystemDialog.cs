using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Telega_RPC.Core
{
    class SystemDialog
    {
        public static bool dialog_active = false;
        public static string dialog_text = "";

        public void activate()
        {
            dialog_active = true;
        }
        public void deactivate()
        {
            dialog_active = false;
        }

        public void showDialog()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var system_dialog_window = new SystemDialogWindow();
                system_dialog_window.Show();
                system_dialog_window.message_text_box.Text = dialog_text;
            });
        }
    }
}
