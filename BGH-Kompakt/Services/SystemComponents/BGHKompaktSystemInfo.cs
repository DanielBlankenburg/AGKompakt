using BGH_Kompakt.Classes;
using BGH_Kompakt.Classes.SystemSettings;
using BGH_Kompakt.Services.DBContexts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Services.SystemComponents
{
    public static class BGHKompaktSystemInfo
    {
        public static SystemSetting _settingList = new SystemSetting();

        public static string PathLaufwerksbuchstabe { get; }
        public static string PathSitzungsunterlagen { get; }
        public static string Pathunterlagenverwaltung { get; }
        public static string PathSystemdateien { get; }
        public static string PathSitzungsplaene { get; }
        public static string PathMontagspost { get; }
        public static string PathDokstelle { get; }
        public static string PathEigeneDateien { get; }
        public static string PathDokstelleDFS { get; }
        public static string PathLoggingData { get; }
        public static string PathTemp { get; }
        public static string PathTempARDOC { get; }


        public static string EMailDokstelle { get; }
        //public static string EMailDokstelleSignatur { get; } 

        static BGHKompaktSystemInfo()
        {
            UserDBContext userDBContext = new UserDBContext();
            ProgrammSetting programmSettings = userDBContext.ProgrammSettings.FirstOrDefault();
            PathLaufwerksbuchstabe = Assembly.GetExecutingAssembly().Location.Split('\\')[0] + "\\";


            PathSitzungsunterlagen = (programmSettings != null) ? programmSettings.PathSitzungsunterlagen : string.Empty;
            Pathunterlagenverwaltung = PathSitzungsunterlagen;
            Pathunterlagenverwaltung += (programmSettings != null) ? programmSettings.Pathunterlagenverwaltung : string.Empty;
            PathSystemdateien = Pathunterlagenverwaltung + "Systemdateien\\";
            PathSitzungsplaene = Pathunterlagenverwaltung + "Systemdateien\\";
            PathDokstelle = (programmSettings != null) ? programmSettings.PathDokstelle : string.Empty;
            PathMontagspost = (programmSettings != null) ? programmSettings.PathMontagspost : string.Empty;
            PathEigeneDateien = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";
            PathDokstelleDFS = (programmSettings != null) ? programmSettings.PathDokstelleDFS : string.Empty;
            EMailDokstelle = (programmSettings != null) ? programmSettings.EMailDokstelle : string.Empty;
            
            
            PathTemp = System.Environment.GetEnvironmentVariable("TEMP") + Path.DirectorySeparatorChar;
            PathTempARDOC = $"{PathTemp}ARDOC\\";
            PathLoggingData = $"{PathTemp}LoggingData\\";
            try
            {
                if (!Directory.Exists(PathLoggingData)) Directory.CreateDirectory(PathLoggingData);
            }
            catch (Exception)
            {
                ViewManager.ShowMainInfoFlyout("Der Tempordner konnte nicht erstellt werden.", false);
            }
        }
    }
}
