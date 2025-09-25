using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes;
using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Converter;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using BGH_Kompakt.Services;
using BGH_Kompakt.Classes.Helper;
using System.Text.RegularExpressions;
using BGH_Kompakt.Services.SystemComponents;

namespace BGH_Kompakt.ViewModel.Montagspost
{
    public class MontagspostImportViewModel : ViewModelBase, IFilesDropped
    {
        private MPDBContext mpDBContext = new MPDBContext();
        public List<int> KalenderwochenList { get; set; } = new List<int>();
        public List<int> VintageList { get; set; } = new List<int>();
        public ObservableCollection<MPImportFile> ImportFileList { get; set; } = new ObservableCollection<MPImportFile>();
        public ObservableCollection<MPImportFile> ImportWordFileList { get; set; } = new ObservableCollection<MPImportFile>();
        public ObservableCollection<MPEMailRecipient> EMailWordList { get; set; } = new ObservableCollection<MPEMailRecipient>();
        public ObservableCollection<MPEMailRecipient> EMailPDFList { get; set; } = new ObservableCollection<MPEMailRecipient>();
        public ObservableCollection<MPImportResult> ImportResultList { get; set; } = new ObservableCollection<MPImportResult>();
        private List<MPImportResult> ImportResultTemp { get; set; } = new List<MPImportResult>();

        private string MPStateText = "Import abgeschlossen";
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
            Task task = ImportAsync();
            //ViewManager.ShowMainInfoFlyout("Die E-Mail wird importiert.", false);
            await task;
            foreach (MPImportResult result in ImportResultTemp) ImportResultList.Add(result);
            ShowNewImport = true;
            ViewManager.ShowMainInfoFlyout("Der Import ist abgeschlossen.", false);

        }

        #region Import Synchron

        //private Task ImportMessageAsync(string file)
        //{
        //    //ViewManager.ShowMainInfoFlyout("Die E-Mail wird importiert.", false);
        //    //         //Thread.Sleep(1000);
        //    string path = BGHKompaktSystemInfo.PathLaufwerksbuchstabe + BGHKompaktSystemInfo.PathMontagspost;
        //    string KWPath = string.Empty;

        //    Mouse.OverrideCursor = Cursors.Wait;
        //    //         //Unterlagen imoportieren

        //    Task task = Task.Run(async () =>
        //    {

        //        try
        //        {
        //            Stopwatch sw = Stopwatch.StartNew();
        //            sw.Start();
        //            MPWeek ImportMPWeek = new MPWeek();
        //            string pathEntscheidung = string.Empty;

        //            //Email einlesen
        //            Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();

        //            var item = app.Session.OpenSharedItem(file) as Microsoft.Office.Interop.Outlook.MailItem;
        //            string body = item.HTMLBody;
        //            string subject = item.Subject;
        //            int att = item.Attachments.Count;

        //            //int Jahr = DateTime.Parse(DateTime.Now.ToString()).Year;

        //            int Ende = subject.LastIndexOf("Kw");
        //            string kw = subject.Substring(Ende + 3, 2);
        //            string JahrString = "20" + subject.Substring(Ende + 6, 2);
        //            int Jahr = int.Parse(JahrString);

        //            //kw = "31";
        //            //Jahr = 2024;

        //            string[] entscheidungsSammlung = new string[att - 1];

        //            KWPath = path + Jahr + "\\KW" + kw + "\\";

        //            if (!Directory.Exists(KWPath)) Directory.CreateDirectory(KWPath);
        //            List<MPDecisionFile> ListFiles = new List<MPDecisionFile>();
        //            try
        //            {
        //                //Attachments auslesen
        //                for (int counter = 1; counter <= att; counter++)
        //                {
        //                    //jedes Attachment ablegen
        //                    Microsoft.Office.Interop.Outlook.Attachment attachment = item.Attachments[counter];
        //                    string fileName = item.Attachments[counter].FileName;
        //                    string senat = fileName.Substring(0, fileName.IndexOf(" "));
        //                    MPSenat mPSenat = new MPSenat();
        //                    MPDBContext userContext = new MPDBContext();
        //                    List<MPSenat> SenateList = userContext.MPSenate.ToList();
        //                    int Bereich = 0;
        //                    switch (senat) //1 = Zivilbereich, 2 = Strafbereich, 3 = Sondersenate
        //                    {
        //                        case "1":
        //                            pathEntscheidung = CreateFolder("1. Strafsenat", KWPath, 2);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 16).FirstOrDefault();
        //                            Bereich = 2;
        //                            break;
        //                        case "2":
        //                            pathEntscheidung = CreateFolder("2. Strafsenat", KWPath, 2);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 17).FirstOrDefault();

        //                            Bereich = 2;
        //                            break;
        //                        case "3":
        //                            pathEntscheidung = CreateFolder("3. Strafsenat", KWPath, 2);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 18).FirstOrDefault();
        //                            Bereich = 2;
        //                            break;
        //                        case "4":
        //                            pathEntscheidung = CreateFolder("4. Strafsenat", KWPath, 2);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 19).FirstOrDefault();
        //                            Bereich = 2;
        //                            break;
        //                        case "5":
        //                            pathEntscheidung = CreateFolder("5. Strafsenat", KWPath, 2);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 20).FirstOrDefault();
        //                            Bereich = 2;
        //                            break;
        //                        case "6":
        //                            pathEntscheidung = CreateFolder("6. Strafsenat", KWPath, 2);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 21).FirstOrDefault();
        //                            Bereich = 2;
        //                            break;
        //                        case "AK":
        //                            pathEntscheidung = CreateFolder("Ermittlungsverfahren", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 32).FirstOrDefault();
        //                            Bereich = 2;
        //                            break;
        //                        case "I":
        //                            pathEntscheidung = CreateFolder("I. Zivilsenat", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 2).FirstOrDefault();
        //                            Bereich = 1;
        //                            break;
        //                        case "II":
        //                            pathEntscheidung = CreateFolder("II. Zivilsenat", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 3).FirstOrDefault();
        //                            Bereich = 1;
        //                            break;
        //                        case "III":
        //                            pathEntscheidung = CreateFolder("III. Zivilsenat", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 4).FirstOrDefault();
        //                            Bereich = 1;
        //                            break;
        //                        case "IV":
        //                            pathEntscheidung = CreateFolder("IV. Zivilsenat", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 5).FirstOrDefault();
        //                            Bereich = 1;
        //                            break;
        //                        case "V":
        //                            pathEntscheidung = CreateFolder("V. Zivilsenat", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 6).FirstOrDefault();
        //                            Bereich = 1;
        //                            break;
        //                        case "VI":
        //                            pathEntscheidung = CreateFolder("VI. Zivilsenat", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 7).FirstOrDefault();
        //                            Bereich = 1;
        //                            break;
        //                        case "VIa":
        //                            pathEntscheidung = CreateFolder("VIa. Zivilsenat", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 8).FirstOrDefault();
        //                            Bereich = 1;
        //                            break;
        //                        case "VII":
        //                            pathEntscheidung = CreateFolder("VII. Zivilsenat", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 9).FirstOrDefault();
        //                            Bereich = 1;
        //                            break;
        //                        case "VIII":
        //                            pathEntscheidung = CreateFolder("VIII. Zivilsenat", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 10).FirstOrDefault();
        //                            Bereich = 1;
        //                            break;
        //                        case "IX":
        //                            pathEntscheidung = CreateFolder("IX. Zivilsenat", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 11).FirstOrDefault();
        //                            Bereich = 1;
        //                            break;
        //                        case "X":
        //                            pathEntscheidung = CreateFolder("X. Zivilsenat", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 12).FirstOrDefault();
        //                            Bereich = 1;
        //                            break;
        //                        case "XI":
        //                            pathEntscheidung = CreateFolder("XI. Zivilsenat", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 13).FirstOrDefault();
        //                            Bereich = 1;
        //                            break;
        //                        case "XII":
        //                            pathEntscheidung = CreateFolder("XII. Strafsenat", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 14).FirstOrDefault();
        //                            Bereich = 1;
        //                            break;
        //                        case "XIII":
        //                            pathEntscheidung = CreateFolder("XIII. Zivilsenat", KWPath, 1);
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 15).FirstOrDefault();
        //                            Bereich = 1;
        //                            break;
        //                        case "KVB":
        //                        case "EnVR":
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 31).FirstOrDefault();
        //                            pathEntscheidung = CreateFolder("Kartelsenat", KWPath, 3);
        //                            Bereich = 1;
        //                            break;
        //                        case "StB":
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 26).FirstOrDefault();
        //                            pathEntscheidung = CreateFolder("Steuerberatersenat", KWPath, 3);
        //                            Bereich = 1;
        //                            break;
        //                        case "AnwZ(Brfg)":
        //                        case "AnwStr(B)":
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 22).FirstOrDefault();
        //                            pathEntscheidung = CreateFolder("Anwaltssenat", KWPath, 3);
        //                            Bereich = 3;
        //                            break;
        //                        case "LwZR":
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 24).FirstOrDefault();
        //                            pathEntscheidung = CreateFolder("Landwirtschaftssenat", KWPath, 3);
        //                            Bereich = 3;
        //                            break;
        //                        default:
        //                            mPSenat = SenateList.Where(x => x.MPSenatID == 1).FirstOrDefault();
        //                            pathEntscheidung = CreateFolder("Sonstiges", KWPath, 3);
        //                            Bereich = 3;
        //                            break;
        //                    }
        //                    ListFiles.Add(new MPDecisionFile { FileName = fileName, Path = KWPath + pathEntscheidung, SenatRohstring = senat, Senat = mPSenat, Bereich = Bereich });
        //                    attachment.SaveAsFile(KWPath + pathEntscheidung + fileName);
        //                    //entscheidungsSammlung[counter - 1] = KWPath + pathEntscheidung + fileName;
        //                }

        //                if (att > 0)
        //                {
        //                    ImportMPWeek.MPWeekNumber = int.Parse(kw);
        //                    ImportMPWeek.MPWeekYear = Jahr;

        //                }

        //            }
        //            catch (System.Exception ex)
        //            {

        //                MessageBox.Show("Die Nachricht konnte nicht vollständig importiert werden. Es ist folgender Fehler aufgetreten: " + ex.Message, "Importfehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //            finally
        //            {
        //                //Word schließen
        //                item.Close(OlInspectorClose.olDiscard);
        //                app.Quit();
        //                Marshal.ReleaseComObject(item);
        //                //Docx-Dateien in pdf-Dateien umwandeln
        //                //await DocxToPDFConverter.ConvertFileMultiFolderAsync(KWPath);
        //            }

        //            try
        //            {
        //                #region E-Mail mit pdf-Dateien auslesen

        //                ////Entscheidungen aus dem Text auslesen
        //                //body = ScrubHtml(body);
        //                //body = body.Substring(body.IndexOf('\u2116'));

        //                ////Liste mit den Entscheidungen aus der E-Mail erstellen
        //                //ObservableCollection<MPDecisionImport> entscheidungsListe = new ObservableCollection<MPDecisionImport>();

        //                //string[] entscheidungsSammlung = body.Split(new char[] { '\u2116' });
        //                //foreach (string entscheidung in entscheidungsSammlung)
        //                //{
        //                //    if (entscheidung != string.Empty)
        //                //        entscheidungsListe.Add(new MPDecisionImport(entscheidung));
        //                //}

        //                //MPDBContext userContext = new MPDBContext();
        //                ////Liste mit den Attachments durchgehen und mit Daten aus E-Mail vergleichen
        //                //foreach (MPDecisionFile mPDecisionFile in ListFiles)
        //                //{
        //                //    string Vergleichsaktenzeichen = AZVergleich(mPDecisionFile);
        //                //    var Query_Info = entscheidungsListe.Where(x => x.Aktenzeichen == Vergleichsaktenzeichen).FirstOrDefault();
        //                //    if (Query_Info != null)
        //                //    {
        //                //        MPDecision newMPDecision = new MPDecision();
        //                //        Query_Info.ExportToMPDecesion(ref newMPDecision);
        //                //        newMPDecision.Senat = userContext.MPSenate.Where(x => x.MPSenatID == mPDecisionFile.Senat.MPSenatID).FirstOrDefault();
        //                //        newMPDecision.PathName = mPDecisionFile.Path;
        //                //        newMPDecision.FileName_Fullpath = mPDecisionFile.FileName_Fullpath;
        //                //        ImportMPWeek.MPDecisions.Add(newMPDecision);
        //                //    }
        //                //    else
        //                //    {
        //                //        MPDecision newMPDecision = new MPDecision();
        //                //        newMPDecision.SenatID = 1;
        //                //        newMPDecision.Typ = 0;
        //                //        newMPDecision.Rohdaten = string.Empty;
        //                //        newMPDecision.Date = DateTime.Now;
        //                //        newMPDecision.Aktenzeichen = string.Empty;
        //                //        newMPDecision.RegZeichen = string.Empty;
        //                //        newMPDecision.LaufendeNummer= string.Empty;
        //                //        newMPDecision.Jahr = string.Empty;
        //                //        newMPDecision.PathName = mPDecisionFile.Path;
        //                //        newMPDecision.FileName_Fullpath = mPDecisionFile.FileName_Fullpath;
        //                //    }
        //                //}

        //                //userContext.MPWeeks.Add(ImportMPWeek);
        //                //userContext.SaveChanges();
        //                //if (SelectedVintage == ImportMPWeek.MPWeekYear) MPWeekList.Add(ImportMPWeek);
        //                //ViewManager.ShowMainInfoFlyout("Die E-Mail wurde erfolgreich eingelesen.", false);

        //                #endregion

        //                #region E-Mail mit Word-Dateien auslesen

        //                //Entscheidungen aus dem Text auslesen
        //                List<MPDecisionImportWord> entscheidungsListe = new List<MPDecisionImportWord>();


        //                //int AnzahlEntscheidungen = ListFiles.Count();

        //                //Word_Datei_Auslesen(ListFiles, ref entscheidungsListe);

        //                //MPDBContext userContext = new MPDBContext();
        //                //foreach (MPDecisionImportWord mpImport in entscheidungsListe) 
        //                //{
        //                //    MPDecision newMPDecision = new MPDecision();
        //                //    mpImport.ExportToMPDecesion(ref newMPDecision, ref userContext);
        //                //    ImportMPWeek.MPDecisions.Add(newMPDecision);
        //                //}


        //                //userContext.MPWeeks.Add(ImportMPWeek);
        //                //userContext.SaveChanges();

        //                #endregion
        //                sw.Stop();
        //                MessageBox.Show($"Dauer des Einlesens: {sw.ElapsedMilliseconds} ms");

        //            }
        //            catch (System.Exception ex)
        //            {

        //                MessageBox.Show("Die Nachricht konnte nicht vollständig importiert werden. Es ist folgender Fehler aufgetreten: " + ex.Message, "Importfehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }

        //            //GesamtListeErstellen(KWPath);
        //            //             this.Cursor = Cursors.Arrow;

        //        }
        //        catch (System.Exception ex)
        //        {
        //            MessageBox.Show("Die Nachricht konnte nicht vollständig importiert werden. Es ist folgender Fehler aufgetreten: " + ex.Message, "Importfehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    });
        //    Mouse.OverrideCursor = Cursors.Arrow;
        //    return task;

        //}
        #endregion

        //Parallel
        private Task ImportAsync()
        {

            Task task = Task.Run(() =>
            {

                Stopwatch sw = Stopwatch.StartNew();
                sw.Start();
                MPWeek ImportMPWeek = new MPWeek();

                var select = SelectedVintage;
                //const bool forceNonParallel = false;
                //var options = new ParallelOptions { MaxDegreeOfParallelism = forceNonParallel ? 1 : -1 };

                //Parallel.For(0, ImportFileList.Count, options, i =>
                //{
                //    int counter = (int)i;
                //    if (ImportFileList[i].WordFileExist)
                //    {

                //        string fileName = ImportFileList[counter].FileName;

                //        ReadMPState = $"Es wird folgende Anlage eingelesen: {fileName}";
                //        string senat = fileName.Substring(0, fileName.IndexOf("_"));
                //        MPSenat mPSenat = new MPSenat();
                //        MPDBContext context = new MPDBContext();
                //        List<MPSenat> SenateList = context.MPSenate.ToList();
                //        MPImportHelper importhelper = new MPImportHelper();
                //        try
                //        {
                //            switch (senat) //1 = Zivilbereich, 2 = Strafbereich, 3 = Sondersenate
                //            {
                //                case "1":
                //                    importhelper = FolderCreation(2, "1. Strafsenat", 16, SenateList);
                //                    break;
                //                case "2":
                //                    importhelper = FolderCreation(2, "2. Strafsenat", 17, SenateList);
                //                    break;
                //                case "3":
                //                    importhelper = FolderCreation(2, "3. Strafsenat", 18, SenateList);
                //                    break;
                //                case "4":
                //                    importhelper = FolderCreation(2, "4. Strafsenat", 19, SenateList);
                //                    break;
                //                case "5":
                //                    importhelper = FolderCreation(2, "5. Strafsenat", 20, SenateList);
                //                    break;
                //                case "6":
                //                    importhelper = FolderCreation(2, "6. Strafsenat", 21, SenateList);
                //                    break;
                //                case "AK":
                //                    importhelper = FolderCreation(2, "Ermittlungsverfahren", 32, SenateList);
                //                    break;
                //                case "I":
                //                    importhelper = FolderCreation(1, "I. Zivilsenat", 2, SenateList);
                //                    break;
                //                case "II":
                //                    importhelper = FolderCreation(1, "II. Zivilsenat", 3, SenateList);
                //                    break;
                //                case "III":
                //                    importhelper = FolderCreation(1, "III. Zivilsenat", 4, SenateList);
                //                    break;
                //                case "IV":
                //                    importhelper = FolderCreation(1, "IV. Zivilsenat", 5, SenateList);
                //                    break;
                //                case "V":
                //                    importhelper = FolderCreation(1, "V. Zivilsenat", 6, SenateList);
                //                    break;
                //                case "VI":
                //                    importhelper = FolderCreation(1, "VI. Zivilsenat", 7, SenateList);
                //                    break;
                //                case "VIa":
                //                    importhelper = FolderCreation(1, "VIa. Zivilsenat", 8, SenateList);
                //                    break;
                //                case "VII":
                //                    importhelper = FolderCreation(1, "VII. Zivilsenat", 9, SenateList);
                //                    break;
                //                case "VIII":
                //                    importhelper = FolderCreation(1, "VIII. Zivilsenat", 10, SenateList);
                //                    break;
                //                case "IX":
                //                    importhelper = FolderCreation(1, "IX. Zivilsenat", 11, SenateList);
                //                    break;
                //                case "X":
                //                    importhelper = FolderCreation(1, "X. Zivilsenat", 12, SenateList);
                //                    break;
                //                case "XI":
                //                    importhelper = FolderCreation(1, "XI. Zivilsenat", 13, SenateList);
                //                    break;
                //                case "XII":
                //                    importhelper = FolderCreation(1, "XII. Zivilsenat", 14, SenateList);
                //                    break;
                //                case "XIII":
                //                    importhelper = FolderCreation(1, "XIII. Zivilsenat", 15, SenateList);
                //                    break;
                //                case "KVB":
                //                case "EnVR":
                //                    importhelper = FolderCreation(3, "Kartelsenat", 31, SenateList);
                //                    break;
                //                case "StB":
                //                    importhelper = FolderCreation(3, "Steuerberatersenat", 26, SenateList);
                //                    break;
                //                case "AnwZ(Brfg)":
                //                case "AnwSt(B)":
                //                    importhelper = FolderCreation(3, "Anwaltssenat", 22, SenateList);
                //                    break;
                //                case "NotSt(Brfg)":
                //                case "NotZ(Brfg)":
                //                    importhelper = FolderCreation(3, "Notarsenat", 23, SenateList);
                //                    break;
                //                case "LwZR":
                //                    importhelper = FolderCreation(3, "Landwirtschaftssenat", 24, SenateList);
                //                    break;
                //                default:
                //                    importhelper = FolderCreation(3, "Sonstiges", 1, SenateList);
                //                    break;
                //            }
                //            Debug.WriteLine($"{fileName}; {importhelper.PathentscheidungDok}");
                //            ImportFileList[counter].ImportPathDok = importhelper.PathentscheidungDok;
                //            ImportFileList[counter].ImportPathMP = importhelper.PathentscheidungMP;
                //            ImportFileList[counter].Senat = importhelper.MPSenat;
                //            ImportFileList[counter].Bereich = importhelper.Bereich;
                //            if (!File.Exists($"{importhelper.PathentscheidungDok}{ImportFileList[counter].FileName}")) File.Copy(ImportFileList[counter].FileFullPath, $"{importhelper.PathentscheidungDok}{ImportFileList[counter].FileName}");
                //            //In den Montagspostordner kopieren
                //            if (!File.Exists($"{importhelper.PathentscheidungMP}{ImportFileList[counter].FileName}")) File.Copy(ImportFileList[counter].FileFullPath, $"{importhelper.PathentscheidungMP}{ImportFileList[counter].FileName}");
                //            ImportFileList[counter].ImportSuccessfull = true;
                //        }
                //        catch (System.Exception)
                //        {
                //            ImportFileList[counter].ImportSuccessfull = false;
                //        }
                //    }
                //});

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
                                case "AK":
                                case "BGs":
                                    importhelper = FolderCreation("Ermittlungsrichter", SenateList);
                                    break;
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
                                MessageBox.Show($"Import der Datei {file.FileName} ist gescheitert.");
                                file.ImportSuccessfull = false;
                            }
                        }
                        catch (System.Exception)
                        {
                            file.ImportSuccessfull = false;
                        }
                    }
                }
                ;


                //if (att > 0)
                //{
                ImportMPWeek.MPWeekNumber = SelectedKW;
                ImportMPWeek.MPWeekYear = SelectedVintage;


                #region E-Mail mit Word-Dateien auslesen

                //Entscheidungen aus dem Text auslesen
                List<MPDecisionImportWord> entscheidungsListe = new List<MPDecisionImportWord>();

                ReadMPState = "Das Einlesen der Verfahrensdaten wird vorbereitet";
                int AnzahlEntscheidungen = ImportFileList.Count();

                entscheidungsListe = Word_Datei_Auslesen(ImportFileList);

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
                    ReadMPState = $"Die Datensätze konnten nicht in die Datenbank geschrieben werden. Es ist folgender Fehler aufgetreten: {ex.Message}";
                }
                //MPWeekList.Add(ImportMPWeek);

                //E-Mail-Erstellen
                if (DataRead)
                {
                    EMails_Erstellen();
                    EMailNotification_Erstellen();
                }

                //Gesamt - pdf - Erstellen

                string NumKW = (SelectedKW <= 9) ? $"0{SelectedKW}" : SelectedKW.ToString();
                GesamtListeErstellen($"{BGHKompaktSystemInfo.PathDokstelleDFS}{BGHKompaktSystemInfo.PathMontagspost}{SelectedVintage}\\KW{NumKW}\\");


                #endregion
                sw.Stop();

            });
            return task;

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
                var query = context.Users.Where(n => n.MPEMailNotification == true).ToArray();
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
                try
                {
                    EMailVersand eMailVersand = new EMailVersand();
                    eMailVersand.Send_Email(
                        emailTo: BGHKompaktSystemInfo.EMailDokstelle,
                        BCC: strEMailAdresses,
                        subject: strSubject,
                        mailBody: text,
                        attachmentList: attachmentListpfd);
                }
                catch (System.Exception) { }
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
                    Directory.CreateDirectory(pathDok);
                }
                if (!Directory.Exists(pathMP))
                {
                    Directory.CreateDirectory(pathMP);
                }

                string importPathDok = $"{pathDok}{SelectedVintage}\\kw{SelectedVintage.ToString().Substring(2)}{SelectedKW.ToString()}\\";
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
                MessageBox.Show($"Es ist folgender Fehler aufgetreten: {ex.Message}");
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
                    Directory.CreateDirectory(folder);
                }
                return path + bereich + senat + "\\";
            }
            catch (System.Exception)
            {
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
                                                LeitsatzErfasst = true;
                                                StraftatbestandErfasst = true;
                                                NormenketteErfasst = true;
                                            }
                                        }
                                    }
                                }
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

    }
}
