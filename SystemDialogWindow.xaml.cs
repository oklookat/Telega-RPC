using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telega_RPC.Core;

namespace Telega_RPC
{

    public partial class SystemDialogWindow : Window
    {

        public SystemDialogWindow()
        {
            InitializeComponent();
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            BotLogic.systemDialogClosedWindowMessage();
        }

        private void message_text_box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
            {
                message_text_box.Text += Environment.NewLine;
            }
        }
        private void message_text_box_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter &&
                !(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) &&
                !(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
            {
                message_text_send_Click(this, new RoutedEventArgs());
                e.Handled = true;
            }
        }

        private void message_text_send_Click(object sender, RoutedEventArgs e)
        {
            var message = message_text_box.Text;
            BotLogic.systemDialogReplyMessage(message);
            this.Close();
        }
    }
}
