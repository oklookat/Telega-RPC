using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Telega_RPC
{
    /// <summary>
    /// Логика взаимодействия для AllowedUsersAddWindow.xaml
    /// </summary>
    public partial class AllowedUsersAddWindow : Window
    {
        MainWindow main_window = (MainWindow)Application.Current.MainWindow;

        public AllowedUsersAddWindow()
        {
            InitializeComponent();
        }

        private void user_id_ok_button_Click(object sender, RoutedEventArgs e)
        {
            var user_id = user_id_checkbox.Text;
            if (!String.IsNullOrEmpty(user_id))
            {
                main_window.bot_add_allowed_user_from_window(user_id);
            }
            this.Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {

        }
    }
}
