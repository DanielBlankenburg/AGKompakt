using BGH_Kompakt.Classes;
using BGH_Kompakt.Services.UserService;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BGH_Kompakt.Services.Extentions
{
    public static class FileInfoExtensions
    {
        public static Task<string> DeepCopyBSCWFile(this FileInfo file, Sitzungstage selectedSitzungstag)
        {
            Task<string> task = Task.Run<string>(() =>
            {
                try
                {
                    string copyPath = file.DirectoryName.Substring(file.DirectoryName.IndexOf(selectedSitzungstag.Jahr.ToString()));
                    string[] directories = copyPath.Split('\\');
                    string BSCWPath = $"{UserManager.SenatSettings.BSCW_Server_Drive}:\\";
                    //bool ParentDirExists = false;

                    foreach (string directory1 in directories)
                    {
                        BSCWPath += $"{directory1}\\";
                        if (!Directory.Exists(BSCWPath)) Directory.CreateDirectory(BSCWPath);
                    }

                    string targetPath = $"{UserManager.SenatSettings.BSCW_Server_Drive}:\\{copyPath}\\{file.Name}";
                    File.Copy(file.FullName, targetPath, true);
                    return "Die Datei wurde auf den BSCW-Server kopiert.";
                    
                }
                catch (Exception)
                {
                    return "Die Datei konnte nicht kopiert werden.";
                }
            });
            return task;
        }
    }
}
