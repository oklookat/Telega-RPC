using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace Telega_RPC.Core
{
    static class SettingsWizard
    {
        static string exe_path = AppDomain.CurrentDomain.BaseDirectory;

        public static string bot_token_settings;
        public static string enable_on_startup_settings;
        public static string enable_on_system_startup_settings;
        private static string main_settings_path = exe_path + @"Config\MainSettings.xml";
        private static string allowed_users_path = exe_path + @"Config\AllowedUsers.xml";



        //////////// MAIN SETTINGS ////////////
        public static void getMainSettings()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(main_settings_path);
            XmlNodeList xnList = xDoc.SelectNodes("/Settings");
            foreach (XmlNode xnode in xnList)
            {
                bot_token_settings = xnode["bot-token"].InnerText;
                enable_on_startup_settings = xnode["enable-on-startup"].InnerText;
                enable_on_system_startup_settings = xnode["in-system-startup"].InnerText;
            }
        }

        public static void setMainSettings(string section, string value)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(main_settings_path);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Name.Equals(section))
                {
                    xnode.InnerText = value;
                }
            }
            xDoc.Save(main_settings_path);
        }
        //////////// MAIN SETTINGS ////////////





        //////////// ALLOWED USERS ////////////
        public static bool addAllowedUser(string user_id)
        {

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(allowed_users_path);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.InnerText == user_id)
                {
                    return false;
                }
            }
            XmlNode newUser = xDoc.CreateElement("user");
            newUser.InnerText = user_id;
            xRoot.AppendChild(newUser);
            xDoc.Save(allowed_users_path);
            return true;
        }

        public static List<string> getAllowedUsers()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(allowed_users_path);
            XmlElement xRoot = xDoc.DocumentElement;
            List<string> allowed_users = new List<string> ();
            foreach (XmlNode xnode in xRoot)
            {
                allowed_users.Add(xnode.InnerText);
            }
            return allowed_users;
        }

        public static bool deleteAllowedUsers(List<string> user_ids)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(allowed_users_path);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (string user_id in user_ids)
            {
                foreach (XmlNode xnode in xRoot)
                {
                    if (user_id == xnode.InnerText)
                    {
                        xRoot.RemoveChild(xnode);
                    }
                }
            }
            xDoc.Save(allowed_users_path);
            return true;
        }
        //////////// ALLOWED USERS ////////////
        

    }
}
