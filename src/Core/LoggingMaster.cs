using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Telega_RPC.Core
{
    static class LoggingMaster
    {
        static string exe_path = AppDomain.CurrentDomain.BaseDirectory;

        static string logs_temp_path = exe_path + @"userfiles\temp\logs\";
        static string log_name = "log.txt";
        static string logs_file_path = logs_temp_path + log_name;


        public static void createLogs()
        {
            try
            {
                if (!Directory.Exists(logs_temp_path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(logs_temp_path);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("The process failed: {0}", e.ToString());
            }

            try
            {
                File.Create(logs_file_path).Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }


        public static async void addToLog(string from_where, string text)
        {
            string hour_minutes_seconds = DateTime.Now.ToString("HH:mm:ss");
            string pre_text = "[" + hour_minutes_seconds + "]" + " " + "[" + from_where + "]:" + " ";
            var formatted_text = pre_text + text;
            using (StreamWriter file = new StreamWriter(logs_file_path, append: true))
            {
                await file.WriteLineAsync(formatted_text);
                MainWindow.appendToLogTextbox(formatted_text);
            }
        }

    }
}
