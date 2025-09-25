using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Classes.Test;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.Interfaces;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.Views.Sitzungsunterlagen;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Sitzungsunterlagen
{
    public class SitzungsunterlagenStrafViewModel : ViewModelBase, IFilesDropped
    {
        private readonly UserDBContext userDBContext = new UserDBContext();
        private DBResponse resp = new DBResponse();

        public ICommand SenatsheftCommand { get; set; }
        public ICommand AZCommand { get; set; }
        public ICommand RenameCommand { get; set; }
        public ICommand DeleteAttachmentCommand { get; set; }
        public ICommand OpenAttachmentCommand { get; set; }
        public ICommand ExportCommand { get; set; }

        #region Test-Grouping
        public ObservableCollection<TestItem> Items { get; set; }
        public CollectionViewSource CollectionView { get; set; }

        #endregion


        public SitzungsunterlagenStrafViewModel()
        {
            try
            {
                SenatsheftCommand = new RelayCommand(SenatsheftExecute);
                AZCommand = new RelayCommand(AZExecute);
                RenameCommand = new RelayCommand(RenameExecute);
                DeleteAttachmentCommand = new RelayCommand(DeleteAttachmentExecute);
                OpenAttachmentCommand = new RelayCommand(OpenAttachmentExecute);
                ExportCommand = new RelayCommand(ExportExecute);

                string year = DateTime.Now.Year.ToString();
                TextJahr = year.Substring(2);
                FillComoboboxes();

                //Testgrouping
                List<TestItem> clients = new List<TestItem>();
                ActivityRequestDBContext activityRequestDBContext = new ActivityRequestDBContext();
                var Client_Query = activityRequestDBContext.ActivityClients.Include(t => t.ActivityClientTyp).OrderBy(ac => ac.ActivityClientTypID).ThenBy(ac => ac.ACName);
                foreach (var item in Client_Query) clients.Add(new TestItem() { Name = item.ACName, Category = item.ActivityClientTyp.ActivityClientTypText });
                Items = new ObservableCollection<TestItem>(clients);
                var view = new CollectionViewSource();
                view.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                view.Source = Items;
                CollectionView = view;
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Es ist folgender Fehler aufgetreten: {ex.Message}", false);
                Logger.WriteLog($"Es ist folgender Fehler aufgetreten: {ex.Message}");
            }
        }


        private void FillComoboboxes()
        {
            //Senate füllen
            resp = Cbo_Senat_Fill(SenatList);
            if (!resp.Success)
            {
                ViewManager.ShowMainInfoFlyout(resp.Message, false);
                return;
            }
            //Aktenzeichen füllen
            resp = Cbo_Aktenzeichen_Fill(AZList);
            if (!resp.Success)
            {
                ViewManager.ShowMainInfoFlyout(resp.Message, false);
                return;
            }

            //Beisitzer füllen
            resp = Cbo_Beisitzer_Fill(ListBeisitzer);
            if (!resp.Success)
            {
                ViewManager.ShowMainInfoFlyout(resp.Message, false);
                return;
            }
        }
        private ObservableCollection<MPImportFile> _ImportFileList = new ObservableCollection<MPImportFile>();
        public ObservableCollection<MPImportFile> ImportFileList { get { return _ImportFileList; } }
        public ObservableCollection<User> ListBeisitzer { get; set; } = new ObservableCollection<User>();
        private User _SelectedBeisitzer;
        public User SelectedBeisitzer
        {
            get { return _SelectedBeisitzer; }
            set
            {
                SetProperty(ref _SelectedBeisitzer, value);
            }
        }
        public ObservableCollection<Senat> SenatList { get; set; } = new ObservableCollection<Senat>();
        private Senat _SelectedSenat;
        public Senat SelectedSenat
        {
            get { return _SelectedSenat; }
            set
            {
                SetProperty(ref _SelectedSenat, value);
                //AZIsEnable = true;
                //AZList.Clear();
                //if (SelectedSenat != null)
                //{
                //    var Query = userDBContext.SenatAktenzeichen.Where(x => x.SenatSetting.SenatID == SelectedSenat.SenatID).ToList();
                //    if (Query.Count > 0) foreach (var item in Query) AZList.Add(item);
                //    var Query2 = userDBContext.SenatSpruchgruppen.Where(x => x.SenatSetting.SenatID == SelectedSenat.SenatID).ToList();
                //    if (Query2.Count > 0) foreach (var item in Query2) SGList.Add(item);
                //}
            }
        }
        public ObservableCollection<SenatAktenzeichen> AZList { get; set; } = new ObservableCollection<SenatAktenzeichen>();
        private SenatAktenzeichen _SelectedAZ;
        public SenatAktenzeichen SelectedAZ
        {
            get { return _SelectedAZ; }
            set { SetProperty(ref _SelectedAZ, value); }
        }
        private bool _ShowAttachmentList = false;
        public bool ShowAttachmentList
        {
            get { return _ShowAttachmentList; }
            set { SetProperty(ref _ShowAttachmentList, value); }
        }

        private MPImportFile _selectedAttachment;
        public MPImportFile SelectedAttachment
        {
            get { return _selectedAttachment; }
            set { _selectedAttachment = value; }
        }

        private string _TextJahr = string.Empty;
        public string TextJahr
        {
            get { return _TextJahr; }
            set
            {
                string pattern = "^[0-9]{2}$";
                Regex rg = new Regex(pattern);
                if (rg.IsMatch(value))
                {
                    SetProperty(ref _TextJahr, value);
                }
                else
                {
                    ViewManager.ShowMainInfoFlyout("Bitte geben Sie nur eine zweistellige Zahl ein.", false);
                }
            }
        }
        private string _TextLaufendeNummer = string.Empty;
        public string TextLaufendeNummer
        {
            get { return _TextLaufendeNummer; }
            set { SetProperty(ref _TextLaufendeNummer, value); }
        }
        public void OnFilesDropped(string[] files)
        {
            foreach (string file in files) ImportFileList.Add(new MPImportFile(file));
            ShowAttachmentList = true;
        }
        #region Executes
        private void AZExecute(object obj)
        {
            if (ValidateInput())
            {
                string newFileName = string.Empty;

                if (UserManager.SenatSettings.StrafFileAzPrefix)
                {
                    if (UserManager.SenatSettings.StrafFileSenat) newFileName += $"{SelectedSenat.SenatShort} {SelectedAZ.SenatAktenzeichenName} ";
                    newFileName += $"{TextLaufendeNummer} {TextJahr}";
                }
                else
                {
                    if (UserManager.SenatSettings.StrafFileSenat) newFileName += $"{SelectedSenat.SenatShort} {SelectedAZ.SenatAktenzeichenName} ";
                    newFileName += $"{TextLaufendeNummer} {TextJahr}";
                }

                newFileName = newFileName.Replace(" ", UserManager.SenatSettings.StrafFileWhiteSpaceFill);
                newFileName += $"{UserManager.SenatSettings.StrafFileWhiteSpaceFill}{SelectedAttachment.FileName}{SelectedAttachment.FileExtention}";
                MPImportFile selectedFile = SelectedAttachment;
                ObservableCollection<MPImportFile> templist = new ObservableCollection<MPImportFile>();
                foreach (MPImportFile item in ImportFileList) templist.Add(item);
                ImportFileList.Clear();
                foreach (MPImportFile item in templist)
                {
                    if (item.FileName == selectedFile.FileName) item.FileName = newFileName;
                    ImportFileList.Add(item);
                }
            }
        }

        private void SenatsheftExecute(object obj)
        {
            if (ValidateInput())
            {

                string newFileName = string.Empty;

                if (UserManager.SenatSettings.StrafFileAzPrefix)
                {
                    if (UserManager.SenatSettings.StrafFileSenat) newFileName += $"{SelectedSenat.SenatShort} {SelectedAZ.SenatAktenzeichenName} ";
                    newFileName += $"{TextLaufendeNummer} {TextJahr}";
                    newFileName += $"{UserManager.SenatSettings.StrafFileSenatsheftText}";
                }
                else
                {
                    newFileName += $"{UserManager.SenatSettings.StrafFileSenatsheftText} ";
                    if (UserManager.SenatSettings.StrafFileSenat) newFileName += $"{SelectedSenat.SenatShort} {SelectedAZ.SenatAktenzeichenName} ";
                    newFileName += $"{TextLaufendeNummer} {TextJahr}";
                }

                newFileName += $"{SelectedAttachment.FileExtention}";
                newFileName = newFileName.Replace(" ", UserManager.SenatSettings.StrafFileWhiteSpaceFill);
                MPImportFile selectedFile = SelectedAttachment;
                ObservableCollection<MPImportFile> templist = new ObservableCollection<MPImportFile>();
                foreach (MPImportFile item in ImportFileList) templist.Add(item);
                ImportFileList.Clear();
                foreach (MPImportFile item in templist)
                {
                    if (item.FileName == selectedFile.FileName) item.FileName = newFileName;
                    ImportFileList.Add(item);
                }
            }
        }

        private void RenameExecute(object obj)
        {

        }

        private void OpenAttachmentExecute(object obj)
        {
            try
            {
                Process.Start(new ProcessStartInfo(SelectedAttachment.FileFullPath)
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Das Dokument konnte nicht geöffnet werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
            }
        }
        private bool ValidateInput()
        {
            string Message = string.Empty;
            if (string.IsNullOrEmpty(TextLaufendeNummer)) Message = "Bitte tragen Sie eine laufende Nummer ein.";
            else if (string.IsNullOrEmpty(TextJahr)) Message = "Bitte tragen Sie ein Jahr ein.";
            if (Message != string.Empty)
            {
                ViewManager.ShowMainInfoFlyout(Message, false);
                return false;
            }
            else return true;

        }
        private bool ValidateBE()
        {
            if (!UserManager.SenatSettings.StrafFolderBerichterstatter) return true;
            if (SelectedBeisitzer != null) return true;
            ViewManager.ShowMainInfoFlyout("Bitte wählen Sie einen Beisitzer aus.", false);
            return false;
        }
        private void DeleteAttachmentExecute(object obj)
        {
            ImportFileList.Remove(SelectedAttachment);
        }
        private void ExportExecute(object obj)
        {
            ExportToBSCWServer();
        }

        private async void ExportToBSCWServer()
        {
            if (ValidateInput())
            {
                if (ValidateBE())
                {
                    string actionName = "Übertragung BSCW-Server";
                    if (ImportFileList.Count > 0)
                    {
                        if (Directory.Exists($"{UserManager.SenatSettings.BSCW_Server_Drive}:\\"))
                        {
                            Task<DBResponse> task = BSCWTransferFiles(ImportFileList);
                            ViewManager.ShowMainInfoFlyout($"Die Daten werden eingelesen und übertragen. Bitte warten Sie.", false);
                            ViewManager.ActionlistAdd(actionName);
                            await task;
                            ViewManager.ActionlistRemove(actionName);
                            if (task.Result.Success)
                            {
                                ImportFileList.Clear();
                                ViewManager.ShowMainInfoFlyout("Die Dateien wurden exportiert", false);
                            }
                            else ViewManager.ShowMainInfoFlyout(task.Result.Message, false);
                        }
                        else ViewManager.ShowMainInfoFlyout($"Der BSCW-Server konnte unter dem Laufwerk {UserManager.SenatSettings.BSCW_Server_Drive}:\\ nicht gefunden werden. Binden Sie bitte den BSCW-Server als Laufwerk ein.", false);
                    }
                    else ViewManager.ShowMainInfoFlyout("Bitte fügen Sie mindestens eine Datei an.", false);

                }

            }

        }

        private Task<DBResponse> BSCWTransferFiles(ObservableCollection<MPImportFile> importFileList)
        {
            Task<DBResponse> task = Task.Run<DBResponse>(() =>
            {
                DBResponse resp = new DBResponse();
                string BSCW_Server_Path = $"{UserManager.SenatSettings.BSCW_Server_Drive}:\\";
                string BSCW_Server_SubFolder = BSCW_Server_Path;
                #region Unterordner angelegen
                if (UserManager.SenatSettings.StrafFolderSubFolder)
                {
                    if (!string.IsNullOrWhiteSpace(UserManager.SenatSettings.StrafFolderSubFolderText))
                    {
                        BSCW_Server_SubFolder = $"{BSCW_Server_Path}{UserManager.SenatSettings.StrafFolderSubFolderText}\\";
                        if (!Directory.Exists(BSCW_Server_SubFolder))
                        {
                            try
                            {
                                Directory.CreateDirectory(BSCW_Server_SubFolder);
                            }
                            catch (Exception)
                            {
                                resp.Success = false;
                                resp.Message = $"Der Jahrgang konnte auf dem Laufwerk {UserManager.SenatSettings.BSCW_Server_Drive}:\\ nicht erstellt werden. Bitte prüfen Sie, ob Sie die hinreichenden Rechte haben.";
                                return resp;
                            }
                        }
                    }
                    else
                    {
                        resp.Success = false;
                        resp.Message = $"Bitte tragen Sie eine Bezeichnung für den Unterordner ein, der nach den Einstellungen für den Senat angelegt werden soll oder ändern Sie die Einstellungen ab.";
                        return resp;
                    }
                }

                #endregion
                #region Jahrgang anlegen
                string exportYearDir = $"{BSCW_Server_SubFolder}20{TextJahr}\\";
                if (!Directory.Exists(exportYearDir))
                {
                    try
                    {
                        Directory.CreateDirectory(exportYearDir);
                    }
                    catch (Exception)
                    {
                        resp.Success = false;
                        resp.Message = $"Der Jahrgang konnte auf dem Laufwerk {UserManager.SenatSettings.BSCW_Server_Drive}:\\ nicht erstellt werden. Bitte prüfen Sie, ob Sie die hinreichenden Rechte haben.";
                        return resp;
                    }
                }

                #endregion
                #region Verfahrensordner angelegen
                string verfahren = string.Empty;
                if (UserManager.SenatSettings.StrafFolderSenat) verfahren = $"{SelectedSenat.SenatShort} {SelectedAZ.SenatAktenzeichenName} ";
                verfahren += (UserManager.SenatSettings.StrafFolderYearFirst) ? $"{TextJahr}-{TextLaufendeNummer}" : $"{TextLaufendeNummer}-{TextJahr}";
                if (UserManager.SenatSettings.StrafFolderBerichterstatter)
                {
                    if (!string.IsNullOrWhiteSpace(UserManager.RegistratedUser.Initials)) verfahren += $"-{UserManager.RegistratedUser.Initials}";
                    else verfahren += $"-{UserManager.RegistratedUser.NachName.Substring(0, 3)}";
                }
                string ExportFolder = $"{exportYearDir}{verfahren}\\";
                if (!Directory.Exists(ExportFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(ExportFolder);
                    }
                    catch (Exception)
                    {
                        resp.Success = false;
                        resp.Message = $"Der Verfahrensordner konnte auf dem Laufwerk {UserManager.SenatSettings.BSCW_Server_Drive}:\\ nicht erstellt werden. Bitte prüfen Sie, ob Sie die hinreichenden Rechte haben.";
                        return resp;
                    }
                }


                #endregion
                #region Dateien übertragen
                foreach (MPImportFile file in importFileList)
                {
                    File.Copy(file.FileFullPath, $"{ExportFolder}{file.FileName}");
                }
                resp.Success = true;
                return resp;

                #endregion
            });
            return task;
        }
        #endregion

        #region Fill-Functions
        private DBResponse Cbo_Senat_Fill(ObservableCollection<Senat> iList)
        {
            //Alle Zivilsenat auswählen
            DBResponse resp = new DBResponse();
            try
            {
                iList.Clear();
                var Query = userDBContext.Senate.Include(u => u.Users).Where(x => x.SenatArt == 2).ToList();
                if (Query.Count > 0)
                {
                    foreach (var item in Query)
                    {
                        iList.Add(item);
                        if (item.SenatID == UserManager.SenatSettings.SenatID) SelectedSenat = item;
                    }

                }
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = $"Die Senate konnten nicht eingelesen werden. Es ist folgender Fehler aufgetreten:\n\n {ex.Message}.";
            }
            return resp;
        }

        private DBResponse Cbo_Aktenzeichen_Fill(ObservableCollection<SenatAktenzeichen> iList)
        {
            try
            {
                DBResponse resp = new DBResponse();
                SenatAktenzeichen senatAktenzeichen = new SenatAktenzeichen { SenatAktenzeichenName = "StR" };
                iList.Add(senatAktenzeichen);
                SelectedAZ = senatAktenzeichen;
                iList.Add(new SenatAktenzeichen { SenatAktenzeichenName = "AK" });
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Message = $"Die Aktenzeichen konnten nicht eingelesen werden. Es ist folgender Fehler aufgetreten:\n\n {ex.Message}";
                throw;
            }
            return resp;
        }

        private DBResponse Cbo_Beisitzer_Fill(ObservableCollection<User> iList)
        {
            try
            {
                DBResponse resp = new DBResponse();
                foreach (User user in SelectedSenat.Users) iList.Add(user);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Message = $"Die Aktenzeichen konnten nicht eingelesen werden. Es ist folgender Fehler aufgetreten:\n\n {ex.Message}";
                throw;
            }
            return resp;
        }


        #endregion
    }
}
