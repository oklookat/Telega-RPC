using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Telega_RPC.Core
{
    class BotFuncs
    {
        [DllImport("/Lib/OhMyKey.dll", EntryPoint = "EMULATE")]
        public static extern int emulate_key(int key_hex);

        public bool securityChecker(long chat_id, string chat_username)
        {
            LoggingMaster.addToLog("BotFuncs/SECURITY", "SECURITY CHECK STARTED FOR USER @" + chat_username);
            var allowed_users = SettingsWizard.getAllowedUsers();
            foreach(string allowed_user in allowed_users)
            {
                var result = long.TryParse(allowed_user, out long allowed_user_long);
                if (result == true)
                {
                    if (allowed_user_long == chat_id)
                    {
                        LoggingMaster.addToLog("BotFuncs/SECURITY", "ACCESS ALLOWED FOR @" + chat_username + " (by chat_id)");
                        return true;
                    }
                }
                else if (result != true)
                {
                    if (chat_username.Equals(allowed_user))
                    {
                        LoggingMaster.addToLog("BotFuncs/SECURITY", "ACCESS ALLOWED FOR @" + chat_username + " (by chat_username)");
                        return true;
                    }
                }
                else
                {
                    LoggingMaster.addToLog("BotFuncs/SECURITY", "SECURITY WARNING! ACCESS DENIED FOR @" + chat_username);
                    return false;
                }
            }
            LoggingMaster.addToLog("BotFuncs/SECURITY", "SECURITY ERROR! WHATS WRONG WITH SETTINGS FILE?");
            return false;
        }

        private string directoriesManager(string what_need, long chat_id)
        {
            string exe_path = AppDomain.CurrentDomain.BaseDirectory;
            string user_files_path = @"userfiles";
            string temp_path = @"\temp";
            string screenshots_path = @"\screenshots";
            string chat_id_path = @"\" + chat_id.ToString();
            if (what_need.Equals("screen"))
            {
                string temp_screens_path = exe_path + user_files_path + temp_path + chat_id_path + screenshots_path;
                try
                {
                    if (Directory.Exists(temp_screens_path))
                    {
                        return temp_screens_path;
                    }
                    DirectoryInfo di = Directory.CreateDirectory(temp_screens_path);
                }
                catch (Exception e)
                {
                    LoggingMaster.addToLog("BotFuncs/EXCEPTON", "EXCEPTION FROM directoriesManager (" + e.ToString() + ")");
                }
                return temp_screens_path;
            }
            return "null";
        }

        public string screenShot(long chat_id)
        {
            var path = directoriesManager("screen", chat_id);
            double width_double = SystemParameters.VirtualScreenWidth;
            double height_double = SystemParameters.VirtualScreenHeight;
            int width_int = Convert.ToInt32(width_double);
            int height_int = Convert.ToInt32(height_double);
            var bitmap = new Bitmap(width_int, height_int);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
            }
            var screen_path = path + @"\screenshot.png";
            bitmap.Save(screen_path, ImageFormat.Png);
            return screen_path;
        }

        public string systemTime()
        {
            return DateTime.Now.ToString();
        }

        public void mediaPlayPause()
        {
            emulate_key(0xB3);
        }
        public void mediaNext()
        {
            emulate_key(0xB0);
        }
        public void mediaPrev()
        {
            emulate_key(0xB1);
        }


    }
}
