using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.MP
{
    public class MPWeek
    {
        public int MPWeekID { get; set; }
        public int MPWeekYear { get; set; }
        public int MPWeekNumber { get; set; }
        public string MPWeekName
        {
            get { return MPWeekNumber + ". KW"; }
        }
        public ICollection<MPDecision> MPDecisions { get; set; } = new List<MPDecision>();
        public async void ExportBSCWAdmin(MPDBContext mpDBContext)
        {
            try
            {
                MPSetting settings = mpDBContext.MPSettings.FirstOrDefault();
                if (settings != null)
                {
                    if (settings.UploadBSCWServer)
                    {
                        if (Directory.Exists($"{settings.BSCWServerDrive}:\\"))
                        {
                            {
                                string actionName = "BSCW-Server Upload";
                                Task<DBResponse> task = BSCWCopy(settings);
                                ViewManager.ShowMainInfoFlyout("Die Dateien werden übertragen. Bitte warten Sie.", false);
                                ViewManager.ActionlistAdd(actionName);
                                await task;
                                string message = task.Result.Success ? "Die Daten wurden erfolgreich auf den BSCW-Server übertragen." : task.Result.Message;
                                ViewManager.ShowMainInfoFlyout(message, false);
                                ViewManager.ActionlistRemove(actionName);
                            }
                        }
                        else
                        {
                            ErrorMessage.CreateSimpleMessage($"Das Laufwerk {settings.BSCWServerDrive}:\\ konnte nicht gefunden werden.");
                        }
                    }
                    
                }
                else
                {
                    ErrorMessage.CreateSimpleMessage("Es konnte keine Datensatz für die Einstellungen der Montagspost gefunden werden");
                }
                //string BSCW_Server_Path = $"{UserManager.SenatSettings.BSCW_Server_Drive}:\\{SelectedVintage}\\{SelectedMPWeek.MPWeekNumber}\\";
            }
            catch (Exception ex) { ErrorMessage.CreateExceptionWithFlyOutMessage("MontagspostViewModel_BSCW_Check", ex); }
        }

        private Task<DBResponse> BSCWCopy(MPSetting settings)
        {
            Task<DBResponse> task = Task.Run<DBResponse>(() =>
            {
                DBResponse response = new DBResponse();
                List<string> ErrorList = new List<string>();
                foreach (MPDecision Decision in MPDecisions.OrderBy(x => x.SenatID))
                {
                    bool error = false;
                    string exportpath = $"{settings.BSCWServerDrive}:\\{Decision.PathName.Substring(Decision.PathName.IndexOf(BGHKompaktSystemInfo.PathMontagspost))}";
                    error = CreateFolder(exportpath, settings);
                    if (!error)
                    {
                        string exportfile = $"{exportpath}{Decision.FileName}";
                        exportfile = exportfile.Replace("Montagspost\\", "");
                        //MessageBox.Show(exportpath);
                        try
                        {
                            string sourcFile = $"{Decision.PathName}{Decision.FileName}";
                            if (File.Exists(sourcFile))
                            {
                                File.Copy(sourcFile, exportfile, true);
                                error = false;
                            }
                            else
                            {
                                Logger.WriteLog($"Die Datei {Decision.PathName}{Decision.FileName} konnte nicht gefunden werden.");
                                error = true;
                                //"N:\2025\KW18\Strafsenate\2. Strafsenat\2_StR_506-23.pdf"
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLog($"Die Datei {Decision.PathName}{Decision.FileName} konnte nicht kopiert werden. Es ist folgender Fehler aufgetreten: {ex.Message}; {ex.InnerException}");
                            error = true;
                        }
                    }
                    if (error) ErrorList.Add(Decision.PathName);
                }

                string text = string.Empty;
                foreach (string item in ErrorList) text += $"{item}";
                response.Success = ErrorList.Count == 0;
                if (ErrorList.Count > 0) response.Message = $"Folgende Dokumente konnte nicht auf den Server geladen werden: {text}";
                return response;
            });
            return task;
        }

        private bool CreateFolder(string pathName, MPSetting settings)
        {
            try
            {
                string folder = $"{settings.BSCWServerDrive}:\\";
                string[] creationPath = pathName.Split(new[] { Path.DirectorySeparatorChar });
                for (int i = 2; i < creationPath.Length - 1; i++)
                {
                    folder += $"{creationPath[i]}\\";
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                }
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }



    }
}
