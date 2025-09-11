using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BGH_Kompakt.Services.SystemComponents
{
    public static class Logger
    {
        public static void WriteLog(string Message)
        {
            try
            {
                using StreamWriter writer = new StreamWriter($"{BGHKompaktSystemInfo.PathLoggingData}log_{Environment.UserName}.txt", true);
                writer.WriteLine(Message);
            }
            catch (Exception)
            {
                MessageBox.Show("Das Logging ist fehlgeschlagen. Die Log-Datei konnte nicht geschrieben werden.", "Logging-Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
