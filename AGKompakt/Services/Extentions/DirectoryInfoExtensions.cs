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

        public static Task<string> DeepCopyBSCWDirAsync(this DirectoryInfo directory, Sitzungstage selectedSitzungstag)
        {
            Task<string> task = Task.Run<string>(() =>
            {
                try
                {
                    //Check and create directories
                    string copyPath = directory.FullName.Substring(directory.FullName.IndexOf(selectedSitzungstag.Jahr.ToString()));
                    string[] directories = copyPath.Split('\\');
                    string BSCWPath = $"{UserManager.SenatSettings.BSCW_Server_Drive}:\\";

                    foreach (string directory1 in directories)
                    {
                        BSCWPath += $"{directory1}\\";
                        if (!Directory.Exists(BSCWPath)) Directory.CreateDirectory(BSCWPath);
                    }

                    if (directory.FullName == selectedSitzungstag.FullDirectory)
                    {
                        var subDirectories = directory.GetDirectories().ToArray();
                        foreach (DirectoryInfo dir in subDirectories)
                        {
                            string targetDir = $"{UserManager.SenatSettings.BSCW_Server_Drive}:\\{copyPath}\\{dir}";
                            if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);
                            CopyFiles(dir, copyPath);
                        }
                    }
                    else
                    {
                        CopyFiles(directory, copyPath);
                    }

                    return "Die Dateien wurden erfolgreich kopiert.";
                }
                catch (Exception)
                {
                    return "Die Dateien konnten nicht kopiert werden.";
                }
            });
            return task;
        }

        private static void CopyFiles(DirectoryInfo directory, string copyPath)
        {
            string[] Files = Directory.GetFiles(directory.FullName);

            foreach (string file in Files)
            {
                FileInfo copyFile = new FileInfo(file);
                FileInfo targetFile = new FileInfo($"{UserManager.SenatSettings.BSCW_Server_Drive}:\\{copyPath}\\{directory}\\{copyFile.Name}");
                if (File.Exists(targetFile.FullName))
                {
                    if (copyFile.LastWriteTime <= targetFile.LastWriteTime) continue;
                }
                File.Copy(copyFile.FullName, targetFile.FullName, true);
            }
        }
    }
}
