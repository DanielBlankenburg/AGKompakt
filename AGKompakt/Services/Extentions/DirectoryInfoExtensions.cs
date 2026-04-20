using BGH_Kompakt.Classes;
using BGH_Kompakt.Enums;
using BGH_Kompakt.Services.UserService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BGH_Kompakt.Services.Extentions
{
    public static class DirectoryInfoExtensions
    {
        public static int DeepCopy(this DirectoryInfo directory, string destinationDir, int Art)
        {
            try
            {
                if (Directory.Exists(destinationDir))
                {
                    return 1;
                }
                else
                {
                    foreach (string directory1 in Directory.GetDirectories(directory.FullName, "*", SearchOption.AllDirectories))
                        Directory.CreateDirectory(directory1.Replace(directory.FullName, destinationDir));
                    foreach (string file in Directory.GetFiles(directory.FullName, "*.*", SearchOption.AllDirectories))
                        File.Copy(file, file.Replace(directory.FullName, destinationDir), false);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                string Text = string.Empty;
                switch (Art)
                {
                    case 1:
                        Text = "Der Ordner konnte nicht auf dem Desktop abgelegt werden. ";
                        break;
                    case 2:
                        Text = "Die Unterlagen konnten nicht auf dem BSCW-Server abgelegt werden. ";
                        break;
                }
                MessageBox.Show(Text + "Es ist folgender Fehler aufgetreten: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }
    }
}
