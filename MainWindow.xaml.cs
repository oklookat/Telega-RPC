using System;
using System.Windows;
using Telega_RPC.Core;
using System.Diagnostics;
using System.Windows.Navigation;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Telega_RPC
{

    public partial class MainWindow : Window
    {
        string bot_token_settings;
        string enable_on_startup_settings;
        string enable_on_system_startup_settings;
        bool bot_active = false;
        public static string allowed_user_id;


        private void mainSettingsRefresh()
        {
            SettingsWizard.getMainSettings();
            bot_token_settings = SettingsWizard.bot_token_settings;
            enable_on_startup_settings = SettingsWizard.enable_on_startup_settings;
            enable_on_system_startup_settings = SettingsWizard.enable_on_system_startup_settings;
        }

        public MainWindow()
        {
            InitializeComponent();
            mainSettingsRefresh();

            if (!String.IsNullOrEmpty(bot_token_settings))
            {
                bot_token_passwordbox.Password = bot_token_settings;
            }
            if (enable_on_startup_settings.Equals("true"))
            {
                bot_activate();
                bot_activate_checkbox.IsChecked = true;
                bot_activate_on_startup_checkbox.IsChecked = true;
            }
            if (enable_on_system_startup_settings.Equals("true"))
            {
                bot_activate_on_system_startup_checkbox.IsChecked = true;
            }
        }


        private void bot_activate_on_startup_checkbox_Click(object sender, RoutedEventArgs e)
        {
            if (enable_on_startup_settings.Equals("true"))
            {
                SettingsWizard.setMainSettings("enable-on-startup", "false");
            }
            else
            {
                SettingsWizard.setMainSettings("enable-on-startup", "true");
            }
            mainSettingsRefresh();
        }
        private void bot_activate_on_system_startup_checkbox_Click(object sender, RoutedEventArgs e)
        {
            if (enable_on_system_startup_settings.Equals("true"))
            {
                SettingsWizard.setMainSettings("in-system-startup", "false");
                bot_activate_on_system_startup_checkbox.IsChecked = false;
            }
            else
            {
                SettingsWizard.setMainSettings("in-system-startup", "true");
                bot_activate_on_system_startup_checkbox.IsChecked = true;
            }
            mainSettingsRefresh();
        }

        private void bot_token_set_button_Click(object sender, RoutedEventArgs e)
        {
            var bot_token_from_textbox = bot_token_passwordbox.Password;
            SettingsWizard.setMainSettings("bot-token", bot_token_from_textbox);
            mainSettingsRefresh();
        }

        private void bot_activate_checkbox_Click(object sender, RoutedEventArgs e)
        {
            bot_activate();    
        }

        private void bot_activate()
        {
            if (bot_active == true)
            {
                BotLogic.bot_deactivate();
                bot_active = false;
            }
            else
            {
                BotLogic.bot_init(bot_token_settings);
                bot_active = true;
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }





        private void bot_add_allowed_user_button_Click(object sender, RoutedEventArgs e)
        {
            AllowedUsersAddWindow allowed_users_add_window = new AllowedUsersAddWindow();
            allowed_users_add_window.Show();
        }
        public void bot_add_allowed_user_from_window(string user_id)
        {
            SettingsWizard.addAllowedUser(user_id);
            allowedUsersListboxRefresh();
        }

        private void allowedUsersListboxRefresh()
        {
            bot_allowed_users_listbox.Items.Clear();
            var allowed_users = SettingsWizard.getAllowedUsers();
            foreach(string allowed_user in allowed_users)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = allowed_user;
                bot_allowed_users_listbox.Items.Add(item);
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                allowedUsersListboxRefresh();
            }
        }

        private void bot_remove_allowed_user_button_Click(object sender, RoutedEventArgs e)
        {
            var selected_items = bot_allowed_users_listbox.SelectedItems;
            List<string> user_ids = new List<string> ();
            foreach (ListBoxItem selected_item in selected_items)
            {
                user_ids.Add(selected_item.Content.ToString());
            }
            SettingsWizard.deleteAllowedUsers(user_ids);
            allowedUsersListboxRefresh();
        }
    }
}
