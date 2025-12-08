using BGH_Kompakt.Classes;
using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Converter;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Migrartions.Users;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.Interfaces;
using BGH_Kompakt.Services.SystemComponents;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls.Expressions;
using System.Windows;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Montagspost
{
    public class MontagspostImportViewModel : ViewModelBase, IFilesDropped
    {
        private readonly MPDBContext mpDBContext = new MPDBContext();
        public List<int> KalenderwochenList { get; set; } = new List<int>();
        public List<int> VintageList { get; set; } = new List<int>();
        public ObservableCollection<MPImportFile> ImportFileList { get; set; } = new ObservableCollection<MPImportFile>();
        public ObservableCollection<MPImportFile> ImportWordFileList { get; set; } = new ObservableCollection<MPImportFile>();
        public ObservableCollection<MPEMailRecipient> EMailWordList { get; set; } = new ObservableCollection<MPEMailRecipient>();
        public ObservableCollection<MPEMailRecipient> EMailPDFList { get; set; } = new ObservableCollection<MPEMailRecipient>();
        public ObservableCollection<MPImportResult> ImportResultList { get; set; } = new ObservableCollection<MPImportResult>();
        private List<MPImportResult> ImportResultTemp { get; set; } = new List<MPImportResult>();

        private readonly string MPStateText = "Import abgeschlossen";
        private string _readMPState = string.Empty;

        public string ReadMPState
        {
            get { return _readMPState; }
            set { SetProperty<string>(ref _readMPState, value); }
        }

        public int SelectedKWIndex { get; set; }

        private int _SelectedKW;
        public int SelectedKW
        {
            get { return _SelectedKW; }
            set { SetProperty(ref _SelectedKW, value); }
        }

        private int _SelectedVintage;
        public int SelectedVintage
        {
            get { return _SelectedVintage; }
            set { SetProperty(ref _SelectedVintage, value); }
        }

        private string _EMailWordRecipient = string.Empty;
        public string EMailWordRecipient
        {
            get { return _EMailWordRecipient; }
            set { SetProperty(ref _EMailWordRecipient, value); }
        }

        private string _EMailpdfRecipient = string.Empty;
        public string EMailpdfRecipient
        {
            get { return _EMailpdfRecipient; }
            set { SetProperty(ref _EMailpdfRecipient, value); }
        }

        private MPEMailRecipient _SelectedEMailWordRecipient;
        public MPEMailRecipient SelectedEMailWordRecipient
        {
            get { return _SelectedEMailWordRecipient; }
            set { SetProperty(ref _SelectedEMailWordRecipient, value); }
        }

        private MPEMailRecipient _SelectedEMailpdfRecipient;
        public MPEMailRecipient SelectedEMailpdfRecipient
        {
            get { return _SelectedEMailpdfRecipient; }
            set { SetProperty(ref _SelectedEMailpdfRecipient, value); }
        }

        private string _AnzahlImportDateien;
        public string AnzahlImportDateien
        {
            get { return _AnzahlImportDateien; }
            set { SetProperty(ref _AnzahlImportDateien, value); }
        }

        #region ICommands
        public ICommand ClearCommand { get; set; }
        public ICommand NewImportCommand { get; set; }
        public ICommand ImportCommand { get; set; }
        public ICommand EMailWordAddCommand { get; set; }
        public ICommand EMailpdfAddCommand { get; set; }
        public ICommand EMailWordDeleteCommand { get; set; }
        public ICommand EMailpdfDeleteCommand { get; set; }
        public ICommand MPWeekDeleteCommand { get; set; }
        #endregion

        #region Show
        private bool _ShowSettings = false;
        public bool ShowSettings
        {
            get { return _ShowSettings; }
            set { SetProperty(ref _ShowSettings, value); }
        }

        private bool _ShowImportResult = false;
        public bool ShowImportResult
        {
            get { return _ShowImportResult; }
            set { SetProperty(ref _ShowImportResult, value); }
        }

        private bool ShowNewImport { get; set; } = false;
        #endregion

        public MontagspostImportViewModel()
        {
            ClearCommand = new RelayCommand(ClearExecute, ClearCanExecute);
            ImportCommand = new RelayCommand(ImportExecute, ImportCanExecute);
            NewImportCommand = new RelayCommand(NewImportExecute, NewImportCanExecute);
            EMailWordAddCommand = new RelayCommand(EMailAddExecute, EMailWordAddCanExecute);
            EMailpdfAddCommand = new RelayCommand(EMailAddExecute, EMailpdfAddCanExecute);
            EMailWordDeleteCommand = new RelayCommand(EMailDeleteExecute, EMailWordDeleteCanExecute);
            EMailpdfDeleteCommand = new RelayCommand(EMailDeleteExecute, EMailpdfDeleteCanExecute);
            MPWeekDeleteCommand = new RelayCommand(MPWeekDeleteExecute);

            ReadMPState = MPStateText;

            for (int i = 1; i < 52; i++) KalenderwochenList.Add(i + 1);
            VintageList.Add(DateTime.Now.Year);
            VintageList.Add(DateTime.Now.Year + 1);
            SelectedVintage = VintageList[0];
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            Calendar calendar = currentCulture.Calendar;

            int kw = calendar.GetWeekOfYear(DateTime.Now, currentCulture.DateTimeFormat.CalendarWeekRule, currentCulture.DateTimeFormat.FirstDayOfWeek) - 1;
            SelectedKWIndex = (kw <= 52) ? kw : 1; //Kalenderwoche wird auf 1 gesetzt, wenn 53 KW berechnet werden soll
            SelectedKW = SelectedKWIndex;
            var templist = mpDBContext.MPEMailRecipients.Where(x => x.MPEMailRecipientTyp == 1).ToArray();
            foreach (MPEMailRecipient recipient in templist) EMailWordList.Add(recipient);
            var templist2 = mpDBContext.MPEMailRecipients.Where(x => x.MPEMailRecipientTyp == 2).ToArray();
            foreach (MPEMailRecipient recipient in templist2) EMailPDFList.Add(recipient);

        }


        //Executes
        #region Executes
        private bool NewImportCanExecute(object obj)
        {
            return ShowNewImport;
        }
        private void NewImportExecute(object obj)
        {
            ImportFileList.Clear();
            ShowImportResult = false;
            ShowSettings = false;
            ShowNewImport = false;
        }
        private void MPWeekDeleteExecute(object obj)
        {
            throw new NotImplementedException();
        }
        private bool ImportCanExecute(object obj)
        {
            return ImportFileList.Count > 0;
        }
        private void ImportExecute(object obj)
        {
            ImportResultList.Clear();
            ShowImportResult = true;
            ShowSettings = false;
            Import();
        }
        private bool EMailpdfAddCanExecute(object obj)
        {
            return EMailpdfRecipient != string.Empty;
        }
        private bool EMailWordAddCanExecute(object obj)
        {
            return EMailWordRecipient != string.Empty;
        }
        private bool EMailpdfDeleteCanExecute(object obj)
        {
            return SelectedEMailpdfRecipient != null;
        }
        private bool EMailWordDeleteCanExecute(object obj)
        {
            return SelectedEMailWordRecipient != null;
        }
        private void EMailDeleteExecute(object obj)
        {
            if (obj == null) return;
            switch ((string)obj)
            {
                case "word":
                    if (SelectedEMailWordRecipient == null) return;
                    mpDBContext.MPEMailRecipients.Remove(SelectedEMailWordRecipient);
                    mpDBContext.SaveChanges();
                    EMailWordList.Remove(SelectedEMailWordRecipient);
                    return;
                case "pdf":
                    if (SelectedEMailpdfRecipient == null) return;
                    mpDBContext.MPEMailRecipients.Remove(SelectedEMailpdfRecipient);
                    mpDBContext.SaveChanges();
                    EMailWordList.Remove(SelectedEMailpdfRecipient);
                    return;
            }
        }
        private void EMailAddExecute(object obj)
        {
            if (obj == null) return;
            int typ = ((string)obj == "word") ? 1 : 2;
            string recipientAdress = ((string)obj == "word") ? EMailWordRecipient : EMailpdfRecipient;
            MPEMailRecipient recipient = new MPEMailRecipient(recipientAdress, typ);
            mpDBContext.MPEMailRecipients.Add(recipient);
            mpDBContext.SaveChanges();
            if ((string)obj == "word")
            {
                EMailWordList.Add(recipient);
                EMailWordRecipient = string.Empty;
            }
            else
            {
                EMailPDFList.Add(recipient);
                EMailpdfRecipient = string.Empty;

            }

        }
        private bool ClearCanExecute(object obj)
        {
            return ImportFileList.Count > 0;
        }
        private void ClearExecute(object obj)
        {
            ImportFileList.Clear();
            AnzahlImportDateien = "Anzahl der Importdateien: 0";
        }


        public void OnFilesDropped(string[] files)
        {
            ImportFileList.Clear();
            ImportWordFileList.Clear();

            foreach (string file in files) ImportFileList.Add(new MPImportFile(file));
            FileInfo fileImport = new FileInfo(ImportFileList[0].FileFullPath);
            foreach (FileInfo file in new DirectoryInfo(fileImport.Directory.ToString()).GetFiles())
            {
                if (file.Name.Contains("docx")) ImportWordFileList.Add(new MPImportFile(file.FullName));
            }
            var templist = ImportFileList.ToArray();
            foreach (MPImportFile file in templist)
            {
                MPImportFile WordFile = ImportWordFileList.FirstOrDefault(x => x.FileRohChar == file.FileRohChar);
                if (WordFile != null)
                {
                    file.WordFileExist = true;
                    file.FileWordFullPath = WordFile.FileFullPath;
                }
                ImportFileList.Remove(file);
                ImportFileList.Add(file);
                AnzahlImportDateien = $"Anzahl der Importdateien: {ImportFileList.Count()}";
            }
            ShowSettings = true;
        }
        #endregion
        private async void Import()
        {
            string actionName = "Import Kalenderwoche";
            Task<DBResponse> task = ImportAsync();
            ViewManager.ShowMainInfoFlyout("Die Dateien werden importiert", false);
            ViewManager.ActionlistAdd(actionName);
            await task;
            ViewManager.ActionlistRemove(actionName);
            if (task.Result.Success)
            {
                foreach (MPImportResult result in ImportResultTemp) ImportResultList.Add(result);
                ShowNewImport = true;
                ViewManager.ShowMainInfoFlyout("Die Kalenderwoche wurde importiert.", false);
                MPWeek importweek = task.Result.Data as MPWeek;
                importweek.ExportBSCWAdmin(mpDBContext);
            }
            else
            {
                ErrorMessage.CreateSimpleMessage(task.Result.Message);
            }
            

        }

        //Parallel
        private Task<DBResponse> ImportAsync()
        {

            Task<DBResponse> task = Task.Run<DBResponse>(() =>
            {
                DBResponse response = new DBResponse();
                Stopwatch sw = Stopwatch.StartNew();
                sw.Start();
                MPWeek ImportMPWeek = new MPWeek();

                var select = SelectedVintage;

                #region Dateien einlesen
                DBResponse importResponse = new DBResponse();
                importResponse = ImportFiles();
                if (!importResponse.Success) ViewManager.ShowMainInfoFlyout(importResponse.Message, false);

                ImportMPWeek.MPWeekNumber = SelectedKW;
                ImportMPWeek.MPWeekYear = SelectedVintage;


                #endregion                
                #region Word-Dateien auslesen

                //Entscheidungen aus dem Text auslesen
                List<MPDecisionImportWord> entscheidungsListe = new List<MPDecisionImportWord>();

                ReadMPState = "Das Einlesen der Verfahrensdaten wird vorbereitet";
                int AnzahlEntscheidungen = ImportFileList.Count();

                entscheidungsListe = Word_Datei_Auslesen(ImportFileList);
                #endregion
                #region Dateien speichern
                bool DataRead = false;
                try
                {
                    MPDBContext context = new MPDBContext();
                    string MessageboxText = string.Empty;
                    foreach (MPDecisionImportWord mpImport in entscheidungsListe)
                    {
                        MessageboxText += $"{mpImport.FileName}; ";
                        MPImportResult importResult = new MPImportResult
                        {
                            Importpdf = mpImport.ImportpdfSuccessfull,
                            ImportWord = mpImport.ImportWordSuccessfull,
                            Name = mpImport.FileName
                        };
                        ImportResultTemp.Add(importResult);

                        if (mpImport.ImportpdfSuccessfull && mpImport.ImportWordSuccessfull)
                        {
                            MPDecision newMPDecision = new MPDecision();
                            mpImport.ExportToMPDecesion(ref newMPDecision, ref context);
                            ImportMPWeek.MPDecisions.Add(newMPDecision);
                        }
                    }
                    //MessageBox.Show(MessageboxText);
                    ////Änderungen in der Datenbank speichern
                    context.MPWeeks.Add(ImportMPWeek);
                    context.SaveChanges();
                    ReadMPState = MPStateText;
                    DataRead = true;
                }
                catch (System.Exception ex)
                {
                    Logger.WriteLog($"Die Datensätze für die Montagspost konnten nicht in die Datenbank geschrieben werden. Es ist folgender Fehler aufgetreten: {ex.Message}; {ex.InnerException}");
                    ReadMPState = $"Die Datensätze konnten nicht in die Datenbank geschrieben werden. Es ist folgender Fehler aufgetreten: {ex.Message}";
                }

                #endregion                //MPWeekList.Add(ImportMPWeek);
                #region E-Mails erstellen
                if (DataRead)
                {
                    EMails_Erstellen();
                    EMailNotification_Erstellen();
                }
                #endregion
                #region Gesamt-PDF erstellen
                string NumKW = (SelectedKW <= 9) ? $"0{SelectedKW}" : SelectedKW.ToString();
                GesamtListeErstellen($"{BGHKompaktSystemInfo.PathDokstelleDFS}{BGHKompaktSystemInfo.PathMontagspost}{SelectedVintage}\\KW{NumKW}\\");
                #endregion
                response.Success  = true;
                response.Data = ImportMPWeek;
                sw.Stop();
                return response;

            });
            return task;

        }
        private DBResponse ImportFiles()
        {
            DBResponse response = new DBResponse();
            string errorList = string.Empty;
            
            foreach (MPImportFile file in ImportFileList)
            {
                //int counter = (int)i;
                if (file.WordFileExist)
                {

                    string fileName = file.FileName;

                    ReadMPState = $"Es wird folgende Anlage eingelesen: {fileName}";
                    //MessageBox.Show($"Es wird folgende Anlage eingelesen {fileName}");
                    string senat = fileName.Substring(0, fileName.IndexOf("_"));
                    MPSenat mPSenat = new MPSenat();
                    MPDBContext context = new MPDBContext();
                    List<MPSenat> SenateList = context.MPSenate.ToList();
                    MPImportHelper importhelper = new MPImportHelper();
                    try
                    {
                        switch (senat) //1 = Zivilbereich, 2 = Strafbereich, 3 = Sondersenate
                        {
                            case "1":
                                importhelper = FolderCreation("1. Strafsenat", SenateList);
                                break;
                            case "2":
                                importhelper = FolderCreation("2. Strafsenat", SenateList);
                                break;
                            case "3":
                            case "StB":
                            case "AK":
                            case "BGs":
                                importhelper = FolderCreation("3. Strafsenat", SenateList);
                                break;
                            case "4":
                                importhelper = FolderCreation("4. Strafsenat", SenateList);
                                break;
                            case "5":
                                importhelper = FolderCreation("5. Strafsenat", SenateList);
                                break;
                            case "6":
                                importhelper = FolderCreation("6. Strafsenat", SenateList);
                                break;
                            //case "AK":
                            //case "BGs":
                            //    importhelper = FolderCreation("Ermittlungsrichter", SenateList);
                            //    break;
                            case "I":
                                importhelper = FolderCreation("I. Zivilsenat", SenateList);
                                break;
                            case "II":
                                importhelper = FolderCreation("II. Zivilsenat", SenateList);
                                break;
                            case "III":
                                importhelper = FolderCreation("III. Zivilsenat", SenateList);
                                break;
                            case "IV":
                                importhelper = FolderCreation("IV. Zivilsenat", SenateList);
                                break;
                            case "V":
                                importhelper = FolderCreation("V. Zivilsenat", SenateList);
                                break;
                            case "VI":
                                importhelper = FolderCreation("VI. Zivilsenat", SenateList);
                                break;
                            case "VIa":
                                importhelper = FolderCreation("VIa. Zivilsenat", SenateList);
                                break;
                            case "VII":
                                importhelper = FolderCreation("VII. Zivilsenat", SenateList);
                                break;
                            case "VIII":
                                importhelper = FolderCreation("VIII. Zivilsenat", SenateList);
                                break;
                            case "IX":
                                importhelper = FolderCreation("IX. Zivilsenat", SenateList);
                                break;
                            case "X":
                                importhelper = FolderCreation("X. Zivilsenat", SenateList);
                                break;
                            case "XI":
                                importhelper = FolderCreation("XI. Zivilsenat", SenateList);
                                break;
                            case "XII":
                                importhelper = FolderCreation("XII. Zivilsenat", SenateList);
                                break;
                            case "XIII":
                                importhelper = FolderCreation("XIII. Zivilsenat", SenateList);
                                break;
                            case "GmS-OGB":
                                importhelper = FolderCreation("Gemeinsamer Senat der obersten Gerichtshöfe", SenateList);
                                break;
                            case "GSSt":
                                importhelper = FolderCreation("Gemeinsamer Strafsenat", SenateList);
                                break;
                            case "GSZ":
                                importhelper = FolderCreation("Gemeinsamer Zivilsenat", SenateList);
                                break;
                            case "KZR":
                            case "KZA":
                            case "KZB":
                            case "KVB":
                            case "KVR":
                            case "AR(Kart)":
                            case "EnZR":
                            case "EnZA":
                            case "EnZB":
                            case "EnVR":
                            case "EnVZ":
                            case "EnRB":
                            case "AR(Enw)":
                                importhelper = FolderCreation("Kartellsenat", SenateList);
                                break;
                            case "StBSt(R)":
                            case "StBSt(B)":
                                importhelper = FolderCreation("Steuerberatersenat", SenateList);
                                break;
                            case "WpSt(R)":
                            case "WpSt(B)":
                                importhelper = FolderCreation("Wirtschaftsprüfersenat", SenateList);
                                break;
                            case "AnwZ":
                            case "AnwZ(P)":
                            case "AnwZ(B)":
                            case "AnwZ(Brfg)":
                            case "AnwSt":
                            case "AnwSt(B)":
                            case "AnwSt(R)":
                                importhelper = FolderCreation("Anwaltssenat", SenateList);
                                break;
                            case "NotZ":
                            case "NotZ(Brfg)":
                            case "NotSt(B)":
                            case "NotSt(Brfg)":
                                importhelper = FolderCreation("Notarsenat", SenateList);
                                break;
                            case "PatAnwZ":
                            case "PatAnwZ(R)":
                            case "PatAnwZ(B)":
                                importhelper = FolderCreation("Patentanwaltsenat", SenateList);
                                break;
                            case "LwZR":
                            case "LwZA":
                            case "LwZB":
                            case "BLw":
                            case "ARLw":
                                importhelper = FolderCreation("Landwirtschaftssenat", SenateList);
                                break;
                            case "VGS":
                                importhelper = FolderCreation("Vereinigte Große Senate", SenateList);
                                break;
                            case "RiZ":
                            case "RiZ(R)":
                            case "RiZ(B)":
                            case "RiSt":
                            case "RiSt(B)":
                            case "RiSt(R)":
                            case "AR(Ri)":
                                importhelper = FolderCreation("Dienstgericht", SenateList);
                                break;
                            default:
                                importhelper = FolderCreation("unbekannter Senat", SenateList);
                                break;
                        }
                        if (importhelper != null)
                        {
                            Debug.WriteLine($"{fileName}; {importhelper.PathentscheidungDok}");
                            file.ImportPathDok = importhelper.PathentscheidungDok;
                            file.ImportPathMP = importhelper.PathentscheidungMP;
                            file.Senat = importhelper.MPSenat;
                            file.Bereich = importhelper.Bereich;
                            //MessageBox.Show($"CopyPath: {importhelper.PathentscheidungMP}");
                            if (!File.Exists($"{importhelper.PathentscheidungDok}{file.FileName}")) File.Copy(file.FileFullPath, $"{importhelper.PathentscheidungDok}{file.FileName}");
                            //In den Montagspostordner kopieren
                            if (!File.Exists($"{importhelper.PathentscheidungMP}{file.FileName}"))
                            {
                                //MessageBox.Show($"CopyFile: {importhelper.PathentscheidungMP}{file.FileName}");
                                File.Copy(file.FileFullPath, $"{importhelper.PathentscheidungMP}{file.FileName}");
                            }
                            file.ImportSuccessfull = true;
                        }
                        else
                        {
                            Logger.WriteLog($"Import-Montagspost: Für folgende Datei konnte kein Importhelper erstellt werden: {file.FileName}");
                            errorList += $"{file.FileName}; ";
                            file.ImportSuccessfull = false;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Logger.WriteLog($"Import-Montagspost: Für die Datei {file.FileName} ist folgender Fehler aufgetreten: {ex.Message}; {ex.InnerException}");
                        file.ImportSuccessfull = false;
                    }
                }
            }

            if (errorList != string.Empty)
            {
                response.Message = $"Der Import folgender Dateien konnte nicht erfolgen: {errorList}";
            }
            else
            {
                response.Success = true;
            }
            return response;
        }
        #region EMail
        private void EMails_Erstellen()
        {
            if (EMailPDFList.Count > 0)
            {
                MPSetting mpSetting = mpDBContext.MPSettings.FirstOrDefault();
                MPEMail mpEMail = mpDBContext.MPEMails.FirstOrDefault(e => e.MPEMailDescription == "Anschreiben Externe Empfänger");
                if (mpSetting != null && mpEMail != null)
                {
                    string jahr = SelectedVintage.ToString().Substring(2);
                    string strSubject = Regex.Replace(mpEMail.MPEMailSubject, "'KW'", $"{SelectedKW}/{jahr}");

                    string Text = $"{mpSetting.MPSettingEMailAnrede}, <br> <br>{mpEMail.MPEMailBody}<br> <br> {mpSetting.MPSettingEMailSchluss}<br> <br>{mpSetting.MPSettingDatenschutzhinweis}";
                    Text = Regex.Replace(Text, "'KW'", $"{SelectedKW}/{jahr}");

                    List<CustomMailAttachment> attachmentListpfd = new List<CustomMailAttachment>();

                    foreach (MPImportFile file in ImportFileList)
                    {
                        if (file.ImportSuccessfull)
                        {
                            attachmentListpfd.Add(new CustomMailAttachment { AttachmentPath = file.FileFullPath, AttachmentName = file.FileName });
                        }
                    }

                    EMails_Versenden(EMailPDFList, strSubject, Text, attachmentListpfd);
                }
            }
            //EMails_Versenden(EMailWordList, strSubject, Text, attachmentListWord);
        }
        private void EMailNotification_Erstellen()
        {
            MPSetting mpSetting = mpDBContext.MPSettings.FirstOrDefault();
            MPEMail mpEMail = mpDBContext.MPEMails.FirstOrDefault(e => e.MPEMailDescription == "Anschreiben Interne Empfänger");
            if (mpSetting != null && mpEMail != null)
            {
                string jahr = SelectedVintage.ToString().Substring(2);
                string strSubject = Regex.Replace(mpEMail.MPEMailSubject, "'KW'", $"{SelectedKW}/{jahr}");

                string Text = $"{mpSetting.MPSettingEMailAnrede}, <br> <br>{mpEMail.MPEMailBody}<br> <br> {mpSetting.MPSettingEMailSchluss}<br> <br>{mpSetting.MPSettingDatenschutzhinweis}";
                Text = Regex.Replace(Text, "'KW'", $"{SelectedKW}/{jahr}");
                //Adressenliste erstellen
                ObservableCollection<MPEMailRecipient> AddressList = new ObservableCollection<MPEMailRecipient>();
                UserDBContext context = new UserDBContext();
                var query = context.Users.Where(n => n.MPEMailNotification == true && n.StatusId == 1).ToArray();
                foreach (var item in query) AddressList.Add(new MPEMailRecipient { MPEMailRecipientAdress = item.EMail, MPEMailRecipientTyp = 2 });

                EMails_Versenden(AddressList, strSubject, Text, null);
            }
        }
        private void EMails_Versenden(ObservableCollection<MPEMailRecipient> List, string strSubject, string text, List<CustomMailAttachment> attachmentListpfd)
        {
            string strEMailAdresses = string.Empty;
            foreach (MPEMailRecipient Member in List)
            {
                strEMailAdresses = !(strEMailAdresses == "") ? strEMailAdresses + ";" + Member.MPEMailRecipientAdress.ToString() : Member.MPEMailRecipientAdress.ToString();
            }
            if (strEMailAdresses.Length > 0)
            {
                EMailVersand eMailVersand = new EMailVersand();
                DBResponse dBResponse = eMailVersand.Send_Email(
                    emailTo: BGHKompaktSystemInfo.EMailDokstelle,
                    BCC: strEMailAdresses,
                    subject: strSubject,
                    mailBody: text,
                    attachmentList: attachmentListpfd);
                if (!dBResponse.Success)
                {
                    Logger.WriteLog(dBResponse.Message);
                    ViewManager.ShowMainInfoFlyout(dBResponse.Message, false);
                }
            }
        }
        #endregion
        private MPImportHelper FolderCreation(string senatBezeichnung, List<MPSenat> SenateList)
        {
            try
            {
                //int bereich, string senatBezeichnung, int senatNummer, List< MPSenat > SenateList

                string pathDok = BGHKompaktSystemInfo.PathDokstelleDFS + BGHKompaktSystemInfo.PathDokstelle;
                string pathMP = BGHKompaktSystemInfo.PathDokstelleDFS + BGHKompaktSystemInfo.PathMontagspost;
                //MessageBox.Show($"ImportPath: {pathDok}; {pathMP}");
                if (!Directory.Exists(pathDok))
                {
                    try
                    {
                        Directory.CreateDirectory(pathDok);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog($"Das Verzeichnis {pathDok} konnte nicht erstellt werden; es ist folgender Fehler aufgetreten: {ex.Message}; {ex.InnerException}");
                    }
                }
                if (!Directory.Exists(pathMP))
                {
                    try
                    {
                        Directory.CreateDirectory(pathMP);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog($"Das Verzeichnis {pathMP} konnte nicht erstellt werden; es ist folgender Fehler aufgetreten: {ex.Message}; {ex.InnerException}");
                    }
                }

                string importPathDok = $"{pathDok}{SelectedVintage}\\kw{SelectedVintage.ToString().Substring(2)}{SelectedKW}\\";
                string NumKW = (SelectedKW <= 9) ? $"0{SelectedKW}" : SelectedKW.ToString();
                string importPathMP = $"{pathMP}{SelectedVintage}\\KW{NumKW}\\";
                //MessageBox.Show($"CreationPath: {importPathMP}");

                int bereich = 0;
                int senatNummer = 0;
                MPSenat searchSenat = SenateList.FirstOrDefault(x => x.MPSenatName == senatBezeichnung);
                if (searchSenat != null)
                {
                    senatNummer = searchSenat.MPSenatID;
                    bereich = searchSenat.MPCategorieID;
                }
                else //unbekannter Senat
                {
                    senatNummer = 1;
                    bereich = 1;
                    searchSenat = SenateList.FirstOrDefault(x => x.MPSenatName == "unbekannter Senat");
                }
                MPImportHelper mPImportHelper = new MPImportHelper
                {
                    PathentscheidungDok = CreateFolder(senatBezeichnung, importPathDok, bereich),
                    PathentscheidungMP = CreateFolder(senatBezeichnung, importPathMP, bereich),
                    MPSenat = searchSenat,
                    Bereich = bereich
                };
                return mPImportHelper;
            }
            catch (System.Exception ex)
            {
                Logger.WriteLog($"Beim Import der Montagspost ist beim Senat {senatBezeichnung} folgender Fehler aufgetreten: {ex.Message}; {ex.InnerException}");
                return null;
            }
        }
        public async Task ConvertFileMultiFolderAsync(string path)
        {
            //Stopwatch watch = new Stopwatch();
            //watch.Start();

            List<String> Seachfiles = new List<String>();
            ReadMPState = "pdf-Dateien werden umgewandelt";
            string VerfahrenName = string.Empty;

            //Bereiche
            List<string> dirBereiche = new List<string>(Directory.EnumerateDirectories(path));
            foreach (var dirV in dirBereiche)
            {
                //Senate
                List<string> dirSenate = new List<string>(Directory.EnumerateDirectories(dirV));

                foreach (string dirV2 in dirSenate)
                {
                    //await ConvertFile (path);
                    //Senate
                    List<string> fileSenate = new List<string>(Directory.EnumerateFiles(dirV2));

                    foreach (string file in fileSenate)
                    {
                        Seachfiles.Add(file);
                    }
                }
            }
            //ReadMPState = "pdf-Dateien werden umgewandelt";
            List<Task> allTasks = new List<Task>();
            for (int i = 0; i < Seachfiles.Count; i++)
            {
                FileInfo file = new FileInfo(Seachfiles[i]);

                if (file.Extension == ".docx" || file.Extension == ".docm")
                {
                    if (file.Name.Substring(0, 7) != "Verweis")
                    {
                        //ReadMPState = $"Datei {file.Name} wird in eine pdf-Dateien umgewandelt.";
                        allTasks.Add(System.Threading.Tasks.Task.Run(() =>
                            DocxToPDFConverter.ConvertDOCtoPDFasync(file.DirectoryName, file.Name, file.Extension)));
                    }
                }
            }

            await System.Threading.Tasks.Task.WhenAll(allTasks);

        }
        private string CreateFolder(string senat, string path, int bereichsArt)
        {
            string bereich = string.Empty;
            switch (bereichsArt)
            {
                case 2:
                    bereich = "Zivilsenate\\";
                    break;
                case 3:
                    bereich = "Strafsenate\\";
                    break;
                case 4:
                    bereich = "Sondersenate\\";
                    break;

            }
            string folder = path + bereich + senat;
            try
            {
                if (!Directory.Exists(folder))
                {
                    try
                    {
                        Directory.CreateDirectory(folder);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog($"Das Verzeichnis {folder} konnte nicht erstellt werden; es ist folgender Fehler aufgetreten: {ex.Message}; {ex.InnerException}");
                    }
                }
                return path + bereich + senat + "\\";
            }
            catch (System.Exception ex)
            {
                Logger.WriteLog($"Das Verzeichnis {folder} konnte nicht erstellt werden; es ist folgender Fehler aufgetreten: {ex.Message}; {ex.InnerException}");
                return string.Empty;
            }
        }
        private void GesamtListeErstellen(string path)
        {

            XFont font = new XFont("Verdana", 16);

            if (!Directory.Exists(path)) return;
            string[] directories = Directory.GetDirectories(path);

            foreach (string directory in directories)
            {

                string bereich = directory.Substring(directory.LastIndexOf("\\") + 1);

                string[] directories2 = Directory.GetDirectories(directory);

                PdfDocument outputDocument = new PdfDocument();
                int counter = 0;
                PdfOutline outline = null;


                foreach (string dir in directories2)
                {
                    string[] files = Directory.GetFiles(dir);

                    foreach (string file in files)
                    {
                        // Open the document to import pages from it.
                        //Prüfen, ob eine pdf-Datei vorliegt
                        FileInfo ConvertFile = new FileInfo(file);
                        if (ConvertFile.Extension == ".pdf")
                        {
                            PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);

                            // Iterate pages

                            if (counter == 0)
                            {
                                PdfPage page = outputDocument.AddPage();
                                XGraphics gfx = XGraphics.FromPdfPage(page);

                                string Filename = inputDocument.FullPath;
                                Filename = Filename.Substring(Filename.LastIndexOf("\\") + 1);
                                Filename = Filename.Substring(0, (Filename.Length - 4));
                                gfx.DrawString(Filename, font, XBrushes.Black, 90, 200, XStringFormats.Default);

                                // Create the root bookmark. You can set the style and the color.
                                outline = outputDocument.Outlines.Add(bereich, page, true, PdfOutlineStyle.Bold, XColors.Red);
                            }
                            else
                            {
                                PdfPage page = outputDocument.AddPage();
                                XGraphics gfx = XGraphics.FromPdfPage(page);
                                string Filename = inputDocument.FullPath;
                                Filename = Filename.Substring(Filename.LastIndexOf("\\") + 1);
                                Filename = Filename.Substring(0, (Filename.Length - 4));
                                string text = Filename;
                                gfx.DrawString(text, font, XBrushes.Black, 90, 200, XStringFormats.Default);

                                // Create a sub bookmark
                                outline.Outlines.Add(text, page, true);
                            }

                            int count = inputDocument.PageCount;
                            for (int idx = 0; idx < count; idx++)
                            {
                                // Get the page from the external document...
                                PdfPage page = inputDocument.Pages[idx];
                                // ...and add it to the output document.
                                outputDocument.AddPage(page);
                            }
                            counter++;
                        }
                    }
                }
                // Dokument speichern, falls mindestens eine Datei vorhanden ist
                if (counter > 0)
                {
                    string filename = directory + "\\" + directory.Substring(directory.LastIndexOf("\\") + 1) + ".pdf";
                    outputDocument.Save(filename);
                }

            }
        }
        private List<MPDecisionImportWord> Word_Datei_Auslesen(ObservableCollection<MPImportFile> ImportList)
        {
            string Entscheidungsart = string.Empty;
            string Aktenzeichen = string.Empty;
            string Verfahrensart = string.Empty;
            string Entscheidungsdatum = string.Empty;
            string Leitsatz = string.Empty;
            string Normenkette = string.Empty;
            string Vorinstanz1 = string.Empty;
            string Vorinstanz2 = string.Empty;
            int EntscheidungsartSchlüssel = 0; //0 = keine Festlegung; 1 = Urteil; 2 = Beschluss
            bool VerfahrensartErfasst = false;
            bool AktenzeichenErfasst = false;
            bool EntscheidungsdatumErfasst = false;
            bool LeitsatzErfasst = false;
            bool StraftatbestandErfasst = false;
            bool NormenketteErfasst = false;
            bool NormenketteStart = false;
            bool RubrumEnde = false;
            bool EntscheidungsartErfasst = false;
            bool StraftatbestandStart = false;
            int ZeilenAnzahlEntscheidungsdatum = 0;
            int ZaehlerVorinstanz = 0;
            int AnzahlEntscheidungen = 0;
            int Zaehler = 1;
            string path = string.Empty;

            List<MPImportFile> ListFiles = new List<MPImportFile>();
            foreach (MPImportFile file in ImportList) ListFiles.Add(file);

            List<MPDecisionImportWord> entscheidungsListe = new List<MPDecisionImportWord>();

            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            object miss = System.Reflection.Missing.Value;
            AnzahlEntscheidungen = ListFiles.Count();

            foreach (MPImportFile entscheidung in ListFiles)
            {
                if (entscheidung.ImportSuccessfull) //Auslesen nur dann, wenn die pdf-Datei erfolgreich kopiert werden konnte
                {

                    try
                    {
                        ReadMPState = $"Auswertung der Word-Dateien\n {Zaehler} von {AnzahlEntscheidungen}; {entscheidung.FileName}";
                        Zaehler++;
                        //path = entscheidung.Path + entscheidung.FileName;
                        object readOnly = true;
                        Microsoft.Office.Interop.Word.Document document = word.Documents.Open(entscheidung.FileWordFullPath, miss, readOnly);
                        string totaltext = string.Empty;
                        AktenzeichenErfasst = false;
                        EntscheidungsdatumErfasst = false;
                        VerfahrensartErfasst = false;
                        RubrumEnde = false;
                        Entscheidungsart = string.Empty;
                        Aktenzeichen = string.Empty;
                        Verfahrensart = string.Empty;
                        Entscheidungsdatum = string.Empty;
                        Vorinstanz1 = string.Empty;
                        Vorinstanz2 = string.Empty;
                        Leitsatz = string.Empty;
                        Normenkette = string.Empty;
                        EntscheidungsartSchlüssel = 0;
                        EntscheidungsartErfasst = false;
                        LeitsatzErfasst = false;
                        StraftatbestandErfasst = false;
                        NormenketteErfasst = false;
                        NormenketteStart = false;
                        StraftatbestandStart = false;

                        //Properties auslesen
                        dynamic properties = document.CustomDocumentProperties;
                        foreach (var item in properties)
                        {
                            if (item.Name == "Aktenzeichen")
                            {
                                Aktenzeichen = item.Value;
                                AktenzeichenErfasst = true;
                                //continue;
                            }
                            if (item.Name == "Dokumenttyp")
                            {
                                Verfahrensart = item.Value;
                                EntscheidungsartSchlüssel = (Verfahrensart == "Urteil") ? 1 : 2;
                                VerfahrensartErfasst = true;
                                //continue;
                            }
                            if (item.Name == "Entscheidungsdatum")
                            {
                                Entscheidungsdatum = item.Value;
                                EntscheidungsdatumErfasst = true;
                                //continue;
                            }
                        }


                        foreach (Microsoft.Office.Interop.Word.Paragraph p in document.Paragraphs)
                        {
                            Debug.WriteLine(p.Range.Text);
                            //if (p.Range.Text.Contains("Erinnerung gegen den Ansatz der Gerichtskosten in der Kostenrechnung des Bundesgerichtshofs vom 23. August 2023"))
                            //{
                            //    MessageBox.Show("Fehlerstelle");
                            //}
                            //MessageBox.Show(p.Range.Text);
                            if (!RubrumEnde && p.Range.Text.Length > 2)
                            {
                                //int range = p.Range.Text.IndexOf("\r");
                                //Debug.WriteLine(p.Range.Text);
                                if (p.Range.Text.IndexOf("\r") >= 0)
                                {
                                    string Text = p.Range.Text.Substring(0, p.Range.Text.IndexOf("\r"));
                                    string TextLower = Text.ToLower();
                                    //Aktenzeichen auslesen
                                    if (!AktenzeichenErfasst)
                                    {
                                        if (TextLower.IndexOf("/") > 0)
                                        {
                                            if (p.Range.Text.Length > 3)
                                            {
                                                Aktenzeichen = p.Range.Text.Substring(0, TextLower.IndexOf("/") + 3);
                                                AktenzeichenErfasst = true;
                                            }
                                        }
                                    }
                                    if (!EntscheidungsartErfasst)
                                    {
                                        if (TextLower == "beschluss")
                                        {
                                            Entscheidungsart = "Beschluss";
                                            EntscheidungsartSchlüssel = 2;
                                            EntscheidungsartErfasst = true;
                                        }
                                        if (TextLower == "urteil" || TextLower == "verzichtsurteil" || TextLower == "anerkenntnisurteil" || TextLower == "versäumnisurteil")
                                        {
                                            Entscheidungsart = "Urteil";
                                            EntscheidungsartSchlüssel = 1;
                                            EntscheidungsartErfasst = true;
                                        }
                                    }
                                    //Entscheidungsart auslesen
                                    //Entscheidugsdatum auslesen
                                    if (!EntscheidungsdatumErfasst)
                                    {
                                        if (EntscheidungsartSchlüssel == 1) // Urteil
                                        {
                                            if (TextLower.IndexOf("verkündet") > 0) ZeilenAnzahlEntscheidungsdatum++;
                                            if (ZeilenAnzahlEntscheidungsdatum > 0)
                                            {
                                                if (ZeilenAnzahlEntscheidungsdatum > 1)
                                                {
                                                    Entscheidungsdatum = TextClean(p.Range.Text);
                                                    EntscheidungsdatumErfasst = true;
                                                }
                                                else
                                                {
                                                    ZeilenAnzahlEntscheidungsdatum++;
                                                }
                                            }
                                        }
                                        else //Beschluss
                                        {
                                            if (TextLower.IndexOf("vom") > 0 || TextLower == "vom") ZeilenAnzahlEntscheidungsdatum = 1;
                                            if (ZeilenAnzahlEntscheidungsdatum > 0)
                                            {
                                                if (ZeilenAnzahlEntscheidungsdatum == 2)
                                                {
                                                    Entscheidungsdatum = TextClean(p.Range.Text);
                                                    ZeilenAnzahlEntscheidungsdatum = 0;
                                                    EntscheidungsdatumErfasst = true;
                                                }
                                                else
                                                {
                                                    ZeilenAnzahlEntscheidungsdatum++;
                                                }
                                            }
                                        }
                                    }
                                    if (!VerfahrensartErfasst)
                                    {
                                        if (TextLower.IndexOf("in ") >= 0)
                                        {
                                            Verfahrensart = FirstLetterToUpper(TextClean(p.Range.Text));
                                            VerfahrensartErfasst = true;
                                        }
                                    }
                                    //In den ersten drei Wörten muss "§", "Art." oder "Verordnung" vorkommen
                                    if (!NormenketteErfasst)
                                    {
                                        string[] paragraphText = TextLower.Split(' ');
                                        if (paragraphText.Length > 0)
                                        {
                                            bool normGefunden = false;
                                            int End = paragraphText.Length > 3 ? 3 : paragraphText.Length;
                                            for (int i = 0; i < End; i++)
                                            {
                                                if (paragraphText[i].IndexOf("§") >= 0
                                                    || paragraphText[i].IndexOf("art.") >= 0
                                                    || paragraphText[i].IndexOf("verordnung") >= 0)
                                                {
                                                    //Es wird nur dann von einem Normensatz ausgegangen, wenn weniger als 20 Wörter genannt werden
                                                    if (paragraphText.Length < 20)
                                                    {
                                                        normGefunden = true;
                                                    }
                                                    else
                                                    {
                                                        normGefunden = LeitsatzCheck(p.Range.Text, paragraphText);
                                                        //int paragraphCount = Regex.Matches(p.Range.Text, "§").Count;
                                                        //if (paragraphCount > 5) normGefunden = true;

                                                        //Paragrahen zählen
                                                    }
                                                    break;
                                                }
                                            }
                                            if (normGefunden)
                                            {
                                                Normenkette += p.Range.Text;
                                                NormenketteStart = true;
                                            }
                                            else
                                            {
                                                if (NormenketteStart) NormenketteErfasst = true;
                                            }

                                        }
                                        //if (TextLower.IndexOf("§") >= 0 || TextLower.IndexOf("art.") >= 0)
                                        //{
                                        //    Normenkette += p.Range.Text;
                                        //    if (!NormenketteStart) NormenketteStart = true;
                                        //}
                                        //else
                                        //{
                                        //    if (NormenketteStart) NormenketteErfasst = true;
                                        //}
                                    }
                                    if (!LeitsatzErfasst || !StraftatbestandErfasst)
                                    {
                                        if (NormenketteErfasst)
                                        {
                                            if (!LeitsatzErfasst)
                                            {
                                                if (TextLower.Substring(0, 4) == "bgh,")
                                                {
                                                    LeitsatzErfasst = true;
                                                    if (entscheidung.Senat.MPCategorieID == 2) StraftatbestandErfasst = true; //Zivilbereich
                                                }
                                                else
                                                {
                                                    Leitsatz += Environment.NewLine + p.Range.Text;
                                                }
                                            }
                                        }
                                        if (entscheidung.Senat.MPCategorieID == 3) //Strafbereich
                                        {
                                            if (p.Range.Text.Contains("wegen"))
                                            {
                                                Leitsatz += (Leitsatz == string.Empty) ? p.Range.Text : Environment.NewLine + p.Range.Text;
                                                //LeitsatzErfasst = true;
                                                //StraftatbestandErfasst = true;
                                                //NormenketteErfasst = true;
                                                StraftatbestandStart = true;
                                            }
                                            else if(StraftatbestandStart)
                                            {
                                                Regex WordCharRegex = new Regex(@"^[\w]");
                                                if (WordCharRegex.IsMatch(p.Range.Text.TrimStart()))
                                                {
                                                    Leitsatz += (Leitsatz == string.Empty) ? p.Range.Text : Environment.NewLine + p.Range.Text;
                                                }
                                                else
                                                {
                                                    NormenketteErfasst = true;
                                                    LeitsatzErfasst = true;
                                                    StraftatbestandErfasst = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (StraftatbestandStart && !StraftatbestandErfasst)
                            {
                                NormenketteErfasst = true;
                                LeitsatzErfasst = true;
                                StraftatbestandErfasst = true;
                            }
                            if (!RubrumEnde && (p.Range.Text.Contains("\f") == true || p.PageBreakBefore == -1))
                            {
                                //if (EntscheidungsartSchlüssel == 2) break;
                                RubrumEnde = true;
                            }

                            // Bereich nach dem Rubrum auf das Verkündungsdatum durchsuchen
                            if (RubrumEnde && p.Range.Text.Length > 2)
                            {
                                if (!EntscheidungsdatumErfasst)
                                {
                                    string Text = p.Range.Text.Substring(0, p.Range.Text.IndexOf("\r"));
                                    string TextLower = Text.ToLower();

                                    //Verkündungsdatum auslesen
                                    if (TextLower.IndexOf("verkündet") > 0) ZeilenAnzahlEntscheidungsdatum++;
                                    if (ZeilenAnzahlEntscheidungsdatum > 0)
                                    {
                                        if (ZeilenAnzahlEntscheidungsdatum > 1)
                                        {
                                            Entscheidungsdatum = TextClean(p.Range.Text);
                                            EntscheidungsdatumErfasst = true;
                                        }
                                        else
                                        {
                                            ZeilenAnzahlEntscheidungsdatum++;
                                        }
                                    }
                                }

                                //Vorinstanzen auslesen
                                if (p.Range.Text.IndexOf("Vorinstanz") >= 0)
                                {
                                    ZaehlerVorinstanz++;
                                    continue;
                                }

                                if (ZaehlerVorinstanz > 0)
                                {
                                    if (ZaehlerVorinstanz == 1)
                                    {
                                        Vorinstanz1 = TextClean(p.Range.Text);
                                        ZaehlerVorinstanz++;
                                    }
                                    else if (ZaehlerVorinstanz == 2)
                                    {
                                        Vorinstanz2 = TextClean(p.Range.Text);
                                        ZaehlerVorinstanz = 0;
                                    }
                                }

                            }
                        }
                        ReleaseObject(document);

                        entscheidungsListe.Add(new MPDecisionImportWord(entscheidung, true, Aktenzeichen, Entscheidungsart, Verfahrensart, Entscheidungsdatum, Leitsatz, Normenkette, Vorinstanz1, Vorinstanz2));
                    }
                    catch (System.Exception)
                    {
                        entscheidungsListe.Add(new MPDecisionImportWord(entscheidung, false));
                    }
                }
            }
            word.Quit(false);
            ReleaseObject(word);
            return entscheidungsListe;
        }
        private bool LeitsatzCheck(string text, string[] paragraphText)
        {
            int paragraphCount = Regex.Matches(text, "§").Count;
            if (paragraphText.Length < 30 && paragraphCount > 5) return true;
            if (paragraphText.Length < 20 && paragraphCount > 8) return true;

            if (paragraphText == null || paragraphText.Length == 0) return false;
            int totalLength = 0;
            foreach (string str in paragraphText) totalLength += str.Length;
            if (totalLength / paragraphText.Length <= 3) return true;
            return false;
        }
        public string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
        public string TextClean(string str)
        {
            if (str == null)
                return null;

            string TextBereinigt = str.Replace("\t", "");
            TextBereinigt = TextBereinigt.Replace("\r", "");
            TextBereinigt = TextBereinigt.Replace("\v", "");
            return TextBereinigt;
        }
        public static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (System.Exception)
            {
                //TODO
            }
            finally
            {
                GC.Collect();
            }
        }

        //private async void BSCCopyMain(MPWeek ImportMPWeek)
        //{
        //    try
        //    {
        //        MPSetting settings = mpDBContext.MPSettings.FirstOrDefault();
        //        if (settings == null)
        //        {
        //            if (Directory.Exists($"{settings.BSCWServerDrive}:\\"))
        //            {
        //                {
        //                    Task task = BSCWCopy(ImportMPWeek, settings);
        //                    ViewManager.ShowMainInfoFlyout($"Die Daten werden eingelesen. Bitte warten Sie.", false);
        //                    ViewManager.MainWindowViewModel.ActionList.Clear();
        //                    ViewManager.MainWindowViewModel.ActionList.Add(new ActionStatus { ActionsStatusName = "Übertragung BSCW-Server" });
        //                    ViewManager.MainWindowViewModel.ShowStatusBar = true;
        //                    await task;
        //                    ViewManager.ShowMainInfoFlyout($"Die Daten wurden erfolgreich auf den BSCW-Server übertragen.", false);
        //                    ViewManager.MainWindowViewModel.ShowStatusBar = false;
        //                }
        //            }
        //            else
        //            {
        //                ErrorMessage.CreateSimpleMessage($"Das Laufwerk {settings.BSCWServerDrive}:\\ konnte nicht gefunden werden.");
        //            }
        //        }
        //        else
        //        {
        //            ErrorMessage.CreateSimpleMessage("Es konnte keine Datensatz für die Einstellungen der Montagspost gefunden werden"); 
        //        }
        //        //string BSCW_Server_Path = $"{UserManager.SenatSettings.BSCW_Server_Drive}:\\{SelectedVintage}\\{SelectedMPWeek.MPWeekNumber}\\";
        //    }
        //    catch (Exception ex) { ErrorMessage.CreateException("MontagspostViewModel_BSCW_Check", ex.Message, ex.InnerException); }
        //}
        //private Task BSCWCopy(MPWeek ImportMPWeek, MPSetting settings)
        //{
        //    Task task = Task.Run(() =>
        //    {
        //        List<string> ErrorList = new List<string>();
        //        foreach (MPDecision Decision in ImportMPWeek.MPDecisions.OrderBy(x => x.SenatID))
        //        {
        //            bool error = false;
        //            string exportpath = $"{settings.BSCWServerDrive}:\\{ImportMPWeek.MPWeekYear}\\KW{ImportMPWeek.MPWeekNumber}\\";
        //            error = CreateFolder(exportpath, settings);
        //            if (!error)
        //            {
        //                string exportfile = $"{exportpath}{Decision.FileName}";
        //                //MessageBox.Show(exportpath);
        //                try
        //                {
        //                    File.Copy($"{Decision.PathName}{Decision.FileName}", exportfile, true);
        //                    error = false;
        //                }
        //                catch (Exception)
        //                {
        //                    error = true;
        //                }
        //            }
        //            if (error) ErrorList.Add(Decision.PathName);
        //        }

        //        string text = string.Empty;
        //        foreach (string item in ErrorList) text += $"{item}";
        //        if (ErrorList.Count > 0) ErrorMessage.CreateSimpleMessage($"Folgende Dokumente konnte nicht auf den Server geladen werden: {text}");

        //    });
        //    return task;
        //}
        //private bool CreateFolder(string pathName, MPSetting settings)
        //{
        //    try
        //    {
        //        string folder = $"{settings.BSCWServerDrive}:\\";
        //        string[] creationPath = pathName.Split(new[] { Path.DirectorySeparatorChar });
        //        for (int i = 2; i < creationPath.Length - 1; i++)
        //        {
        //            folder += $"{creationPath[i]}\\";
        //            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        //        }
        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        return true;
        //    }
        //}


    }
}
