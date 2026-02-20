using BGH_Kompakt.Classes;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Classes.Sitzungsunterlagen;
using BGH_Kompakt.Classes.Sitzungsunterlagen.Helper;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Converter;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.Extentions;
using BGH_Kompakt.Services.Interfaces;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.Views.Sitzungsunterlagen;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Management;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using static BGH_Kompakt.Enums.SitzungsunterlagenEnums;
using static System.Net.Mime.MediaTypeNames;

namespace BGH_Kompakt.ViewModel.Sitzungsunterlagen
{
    public class SitzungsunterlagenViewModel : ViewModelBase, IFileDragDropTarget
    {
        //Allgemeines
        #region Allgemeines
        private readonly UserDBContext userDBContext = new UserDBContext();
        private DBResponse resp = new DBResponse();
        private readonly bool Intial = true;
        private readonly string pathParent = string.Empty;
        private readonly string pathMainDirectory = string.Empty;
        private readonly string pathImport = string.Empty;
        public string[] spruchgruppen;
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                SetProperty(ref _Title, value);
            }
        }

        #endregion

        //Commands
        #region Commands
        public ICommand ImportCommand { get; set; }
        public ICommand SubscribeCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand BinCommand { get; set; }
        public ICommand OpenCommand { get; set; }
        public ICommand PrintCommand { get; set; }
        public ICommand ShowCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand VotenmappeCommand { get; set; }
        public ICommand LeitsatzVerteilenCommand { get; set; }
        public ICommand OpenBeratungsListCommand { get; set; }
        public ICommand OpenSitzungsListCommand { get; set; }
        public ICommand BinBeratungsListCommand { get; set; }
        public ICommand BinSitzungsListCommand { get; set; }
        public ICommand TestCommand { get; set; }


        #endregion

        //Lists/SelectedItems
        #region Lists/SelectedItems
        public ObservableCollection<Vintages> VintageList { get; set; } = new ObservableCollection<Vintages>();
        private Vintages _SelectedVintage;
        public Vintages SelectedVintage
        {
            get { return _SelectedVintage; }
            set
            {
                SetProperty(ref _SelectedVintage, value);
                SitzungstageListFillAsync();
            }
        }

        public ObservableCollection<Sitzungstage> SitzungstageList { get; set; } = new ObservableCollection<Sitzungstage>();


        private Sitzungstage _SelectedSitzungstage;
        public Sitzungstage SelectedSitzungstage
        {
            get { return _SelectedSitzungstage; }
            set
            {
                SetProperty(ref _SelectedSitzungstage, value);
                if (SelectedSitzungstage != null)
                {
                    VerfahrenListFill(0);
                    TextConverter = "Docx - Dateien (Sitzungstag) in pdf - Dateien konvertieren";
                    ShowSitzungsliste = List_Check(_SelectedSitzungstage, "_Sitzungsliste");
                    ShowBeratungsliste = List_Check(_SelectedSitzungstage, "_Beratungsliste");
                }
                else
                {
                    ShowVerfahrenDetails = false;
                }
            }
        }

        public ObservableCollection<Verfahren> VerfahrenList { get; set; } = new ObservableCollection<Verfahren>();
        public ObservableCollection<Verfahren> VerfahrenView { get; set; } = new ObservableCollection<Verfahren>();
        private Verfahren _SelectedVerfahren;
        public Verfahren SelectedVerfahren
        {
            get { return _SelectedVerfahren; }
            set
            {
                SetProperty(ref _SelectedVerfahren, value);
                if (SelectedVerfahren != null)
                {
                    UnterlagenListeFill();
                    TextConverter = "Docx - Dateien (Verfahren) in pdf - Dateien konvertieren";
                }
                else
                {
                    ShowUnterlagen = false;
                }
            }
        }

        private bool List_Check(Sitzungstage selectedVerfahren, string listName)
        {
            string path = $"{selectedVerfahren.FullDirectory}\\{listName}\\";
            if (!Directory.Exists(path)) return false;
            DirectoryInfo dirCheck = new DirectoryInfo(path);
            return dirCheck.EnumerateFiles().Any();
        }


        public ObservableCollection<Unterlagen> UnterlagenList { get; set; } = new ObservableCollection<Unterlagen>();
        private Unterlagen _SelectedUnterlagen;
        public Unterlagen SelectedUnterlagen
        {
            get { return _SelectedUnterlagen; }
            set
            {
                SetProperty(ref _SelectedUnterlagen, value);
                PdfWebViewerSet();
            }
        }

        public ObservableCollection<User> BEList { get; set; } = new ObservableCollection<User>();
        private User _SelectedBE;
        public User SelectedBE
        {
            get { return _SelectedBE; }
            set
            {
                SetProperty(ref _SelectedBE, value);
            }
        }

        public ObservableCollection<User> HiWiList { get; set; } = new ObservableCollection<User>();
        private User _SelectedHiWi;
        public User SelectedHiWi
        {
            get { return _SelectedHiWi; }
            set
            {
                SetProperty(ref _SelectedHiWi, value);
            }
        }

        #endregion

        private bool _SitzungstagePast;
        public bool SitzungstagePast
        {
            get { return _SitzungstagePast; }
            set
            {
                SetProperty(ref _SitzungstagePast, value);
                if (!Intial) SitzungstageListFillAsync();
            }
        }

        //Sitzungsunterlagen create
        #region SitzungstageCreate
        //public List<StringContainer> Monthlist { get; set; } = new List<StringContainer>();
        //private StringContainer _SelectedMonth;
        //public StringContainer SelectedMonth
        //{
        //    get { return _SelectedMonth; }
        //    set { SetProperty(ref _SelectedMonth, value); }
        //}
        //public List<int> DayList { get; set; } = new List<int>();
        //private int _SelectedDay;
        //public int SelectedDay
        //{
        //    get { return _SelectedDay; }
        //    set { SetProperty(ref _SelectedDay, value); }
        //}
        //public List<StringContainer> YearList { get; set; } = new List<StringContainer>();
        //private StringContainer _SelectedYear;
        //public StringContainer SelectedYear
        //{
        //    get { return _SelectedYear; }
        //    set { SetProperty(ref _SelectedYear, value); }
        //}
        private DateTime _SelectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { SetProperty(ref _SelectedDate, value); }
        }
        private int SelectedArtSitzungsunterlagen;
        #endregion

        //Verfahren create
        #region Verfahren create
        public ObservableCollection<Senat> SenatList { get; set; } = new ObservableCollection<Senat>();
        private Senat _SelectedSenat;
        public Senat SelectedSenat
        {
            get { return _SelectedSenat; }
            set
            {
                SetProperty(ref _SelectedSenat, value);
                AZIsEnable = true;
                AZList.Clear();
                if (SelectedSenat != null)
                {
                    var Query = userDBContext.SenatAktenzeichen.Where(x => x.SenatSetting.SenatID == SelectedSenat.SenatID).ToList();
                    if (Query.Count > 0) foreach (var item in Query) AZList.Add(item);
                    var Query2 = userDBContext.SenatSpruchgruppen.Where(x => x.SenatSetting.SenatID == SelectedSenat.SenatID).ToList();
                    if (Query2.Count > 0) foreach (var item in Query2) SGList.Add(item);
                }
            }
        }
        public ObservableCollection<SenatAktenzeichen> AZList { get; set; } = new ObservableCollection<SenatAktenzeichen>();
        private SenatAktenzeichen _SelectedAZ;
        public SenatAktenzeichen SelectedAZ
        {
            get { return _SelectedAZ; }
            set
            {
                SetProperty(ref _SelectedAZ, value);

            }
        }

        public ObservableCollection<SenatSpruchgruppe> SGList { get; set; } = new ObservableCollection<SenatSpruchgruppe>();
        private SenatSpruchgruppe _SelectedSG;
        public SenatSpruchgruppe SelectedSG
        {
            get { return _SelectedSG; }
            set
            {
                SetProperty(ref _SelectedSG, value);

            }
        }

        private string _TextLaufendeNummer = string.Empty;
        public string TextLaufendeNummer
        {
            get { return _TextLaufendeNummer; }
            set
            {
                SetProperty(ref _TextLaufendeNummer, value);

            }
        }

        private string _TextFileName = string.Empty;
        public string TextFileName
        {
            get { return _TextFileName; }
            set { SetProperty(ref _TextFileName, value); }
        }

        private string _TextFileExtention = string.Empty;
        public string TextFileExtention
        {
            get { return _TextFileExtention; }
            set { SetProperty(ref _TextFileExtention, value); }
        }

        private string _TextUnterlagenAdd = string.Empty;
        public string TextUnterlagenAdd
        {
            get { return _TextUnterlagenAdd; }
            set { SetProperty(ref _TextUnterlagenAdd, value); }
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

        private string _TextConverter = "Sitzungstag/Verfahren auswählen";
        public string TextConverter
        {
            get { return _TextConverter; }
            set { SetProperty(ref _TextConverter, value); }
        }


        #endregion

        //Show
        #region Show

        private bool _ShowSitzungstage = true;
        public bool ShowSitzungstage
        {
            get { return _ShowSitzungstage; }
            set
            {
                SetProperty(ref _ShowSitzungstage, value);
            }
        }

        private bool _ShowVerfahren = false;
        public bool ShowVerfahren
        {
            get { return _ShowVerfahren; }
            set
            {
                SetProperty(ref _ShowVerfahren, value);
            }
        }

        private bool _ShowVerfahrenDataGrid = false;
        public bool ShowVerfahrenDetails
        {
            get { return _ShowVerfahrenDataGrid; }
            set
            {
                SetProperty(ref _ShowVerfahrenDataGrid, value);
            }
        }

        private bool _ShowUnterlagen = false;
        public bool ShowUnterlagen
        {
            get { return _ShowUnterlagen; }
            set
            {
                SetProperty(ref _ShowUnterlagen, value);
            }
        }

        private bool _ShowPreview = false;
        public bool ShowPreview
        {
            get { return _ShowPreview; }
            set
            {
                SetProperty(ref _ShowPreview, value);
                WebbrowserHeight = 0;
            }
        }

        private bool _ShowSubscribe = false;
        public bool ShowSubscribe
        {
            get { return _ShowSubscribe; }
            set
            {
                SetProperty(ref _ShowSubscribe, value);
            }
        }

        private bool _ShowSubscribeSitzungstage;
        public bool ShowSubscribeSitzungstage
        {
            get { return _ShowSubscribeSitzungstage; }
            set { SetProperty(ref _ShowSubscribeSitzungstage, value); }
        }
        private bool _ShowSubscribeVerfahren;
        public bool ShowSubscribeVerfahren
        {
            get { return _ShowSubscribeVerfahren; }
            set { SetProperty(ref _ShowSubscribeVerfahren, value); }
        }

        private bool _ShowSubscribeUnterlagenEdit;
        public bool ShowSubscribeUnterlagenEdit
        {
            get { return _ShowSubscribeUnterlagenEdit; }
            set { SetProperty(ref _ShowSubscribeUnterlagenEdit, value); }
        }

        private bool _ShowSubscribeUnterlagenAdd;
        public bool ShowSubscribeUnterlagenAdd
        {
            get { return _ShowSubscribeUnterlagenAdd; }
            set { SetProperty(ref _ShowSubscribeUnterlagenAdd, value); }
        }

        private bool _ShowEnableCards = true;
        public bool ShowEnableCards
        {
            get { return _ShowEnableCards; }
            set { SetProperty(ref _ShowEnableCards, value); }
        }

        private bool _ShowUnterlagenAZDate = false;
        public bool ShowUnterlagenAZDate
        {
            get { return _ShowUnterlagenAZDate; }
            set { SetProperty(ref _ShowUnterlagenAZDate, value); }
        }

        private bool _AZIsEnable = false;
        public bool AZIsEnable
        {
            get { return _AZIsEnable; }
            set { SetProperty(ref _AZIsEnable, value); }
        }

        private bool _ShowFilterOrderBy = false;
        public bool ShowFilterOrderBy
        {
            get { return _ShowFilterOrderBy; }
            set { SetProperty(ref _ShowFilterOrderBy, value); }
        }
        private bool _ShowSitzungsliste = false;
        public bool ShowSitzungsliste
        {
            get { return _ShowSitzungsliste; }
            set
            {
                SetProperty(ref _ShowSitzungsliste, value);
                ShowSitzungslisteAdd = !value;
            }
        }
        private bool _ShowSitzungslisteAdd = true;
        public bool ShowSitzungslisteAdd
        {
            get { return _ShowSitzungslisteAdd; }
            set { SetProperty(ref _ShowSitzungslisteAdd, value); }
        }
        private bool _ShowBeratungsliste = true;
        public bool ShowBeratungsliste
        {
            get { return _ShowBeratungsliste; }
            set
            {
                SetProperty(ref _ShowBeratungsliste, value);
                ShowBeratungslisteAdd = !value;
            }
        }
        private bool _ShowBeratungslisteAdd = true;
        public bool ShowBeratungslisteAdd
        {
            get { return _ShowBeratungslisteAdd; }
            set { SetProperty(ref _ShowBeratungslisteAdd, value); }
        }

        private bool StateVisibilitySitzungstage { get; set; }
        private bool StateVisibilityVerfahren { get; set; }
        private bool StateVisibilityUnterlagen { get; set; }

        private bool _BSCWServer_Sitzungstag = true;
        public bool BSCWServer_Sitzungstag
        {
            get { return _BSCWServer_Sitzungstag; }
            set { SetProperty(ref _BSCWServer_Sitzungstag, value); }
        }

        private bool _BSCWServer_Verfahren = true;
        public bool BSCWServer_Verfahren
        {
            get { return _BSCWServer_Verfahren; }
            set { SetProperty(ref _BSCWServer_Verfahren, value); }
        }

        private bool _BSCWServer_Dokument = true;
        public bool BSCWServer_Dokument
        {
            get { return _BSCWServer_Dokument; }
            set { SetProperty(ref _BSCWServer_Dokument, value); }
        }




        private bool _FilterAll = true;
        public bool FilterAll
        {
            get { return _FilterAll; }
            set
            {
                SetProperty(ref _FilterAll, value);
                VerfahrenView.Clear();
                foreach (Verfahren v in VerfahrenList) VerfahrenView.Add(v);
            }
        }

        private bool _SpruchgruppeChanged = false;
        private bool _FilterSpruchgruppe = false;
        public bool FilterSpruchgruppe
        {
            get { return _FilterSpruchgruppe; }
            set
            {
                SetProperty(ref _FilterSpruchgruppe, value);
                if (!_SpruchgruppeChanged && value && SelectedSpruchgruppe != null) FilterVerfahren();
            }
        }


        public ObservableCollection<SenatSpruchgruppe> SpruchgruppenList { get; set; } = new ObservableCollection<SenatSpruchgruppe>();
        private SenatSpruchgruppe _SelectedSpruchgruppe;
        public SenatSpruchgruppe SelectedSpruchgruppe
        {
            get { return _SelectedSpruchgruppe; }
            set
            {
                SetProperty(ref _SelectedSpruchgruppe, value);
                if (SelectedSpruchgruppe != null)
                {
                    _SpruchgruppeChanged = true;
                    if (!FilterSpruchgruppe) FilterSpruchgruppe = true;
                    FilterVerfahren();
                    _SpruchgruppeChanged = false;
                }
            }
        }

        public ObservableCollection<SortingEnum> SortingList { get; set; } = new ObservableCollection<SortingEnum> { SortingEnum.Aktenzeichen, SortingEnum.Jahr, SortingEnum.Registerzeichen, SortingEnum.Spruchgruppe };
        private SortingEnum _SelectedSorting;
        public SortingEnum SelectedSorting
        {
            get { return _SelectedSorting; }
            set
            {
                SetProperty(ref _SelectedSorting, value);
                SortVerfahren();
            }
        }

        private void FilterVerfahren()
        {
            VerfahrenView.Clear();
            foreach (Verfahren v in VerfahrenList)
            {
                if (v.Spruchgruppe.SenatSpruchgruppeID.Equals(SelectedSpruchgruppe.SenatSpruchgruppeID)) VerfahrenView.Add(v);
            }
        }

        #endregion

        //Webbrowser
        #region Webbrowser
        private Uri _SelectedURI;
        public Uri SelectedURI
        {
            get { return _SelectedURI; }
            set
            { SetProperty(ref _SelectedURI, value); }
        }

        private Double _WebBrowserHeight;
        public Double WebbrowserHeight
        {
            get { return _WebBrowserHeight; }
            set
            { SetProperty(ref _WebBrowserHeight, value); }
        }
        private void PdfWebViewerSet()
        {
            if (ShowPreview == true)
            {
                try
                {
                    string URL = string.Empty;
                    if (SelectedUnterlagen != null)
                    {
                        if (SelectedUnterlagen.FileName_Extention == ".pdf")
                        {
                            string URLText = $"{SelectedUnterlagen.FileName_Fullpath}";
                            SelectedURI = new Uri(URLText);
                        }
                        else
                        {
                            SelectedURI = new Uri("about:blank");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewManager.ShowMainInfoFlyout($"Die Entscheidung kann nicht angezeigt werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
                }
            }
        }

        #endregion

        //Constructor
        public SitzungsunterlagenViewModel()
        {
            try
            {
                ImportCommand = new RelayCommand(ImportExecute);
                SubscribeCommand = new RelayCommand(SubscribeExecute);
                SubmitCommand = new RelayCommand(SubmitExecute);
                CancelCommand = new RelayCommand(CancelExecute);
                BinCommand = new RelayCommand(BinExecute);
                OpenCommand = new RelayCommand(OpenExecute, OpenCanExecute);
                PrintCommand = new RelayCommand(PrintExecute);
                ShowCommand = new RelayCommand(ShowExecute);
                ExportCommand = new RelayCommand(ExportExecute);
                VotenmappeCommand = new RelayCommand(VotenmappeExecute);
                LeitsatzVerteilenCommand = new RelayCommand(LeitsatzverteilenExecute);
                OpenBeratungsListCommand = new RelayCommand(OpenListExecute, BeratungslistCanExecute);
                OpenSitzungsListCommand = new RelayCommand(OpenListExecute, SitzungslistCanExecute);
                BinBeratungsListCommand = new RelayCommand(BinListExecute, BeratungslistCanExecute);
                BinSitzungsListCommand = new RelayCommand(BinListExecute, SitzungslistCanExecute);
                //TestCommand = new RelayCommand(TestExecute);

                //Auslesen des Senatslaufwerks
                if (UserManager.SenatSettings.Senat != null)
                {
                    pathParent = (UserManager.SenatSettings.Senat.Path != null) ? UserManager.SenatSettings.Senat.Path.ToString() : "c:\\";
                }
                else { pathParent = "c:\\"; }

                pathMainDirectory = $"{pathParent}{BGHKompaktSystemInfo.PathSitzungsunterlagen}";
                if (Directory.Exists(pathMainDirectory))
                {
                    pathImport = $"{pathMainDirectory}Import\\";
                    SitzungstagePast = UserManager.SenatSettings.ShowFormerDays;
                    VintageListFill(pathMainDirectory);
                    SelectedVintage = CurrentVintage();
                    SelectedURI = new Uri("about:blank");
                    Intial = false;
                    ShowPreview = true;
                    WebbrowserHeight = 300;
                }
                else
                {
                    MessageBox.Show($"Das Senatslaufwerk konnte unter dem angegebenen Laufwerk - {pathMainDirectory} nicht aufgerufen werden. Bitte ändern Sie die Einstellung für den Senat oder kontaktieren Sie eine Administrator.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    //ViewManager.ShowPageOnMainView<StartView>();
                }

                Title = "Sitzungsunterlagen";
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Es ist folgender Fehler beim Öffnen des Bereichs aufgetreten: {ex.Message}", true);
                Logger.WriteLog($"Es ist folgender Fehler beim öffnen des Bereichs aufgetreten: {ex.Message}; {ex.InnerException}");
            }
        }

        //private async void TestExecute(object obj)
        //{
        //    Task<DBResponse> task = TestFunctionAsync();
        //    ViewManager.ShowMainInfoFlyout("Es wird gerechnet", false);
        //    await task;
        //    if (task.Result.Success)
        //    {
        //        ViewManager.ShowMainInfoFlyout(task.Result.Message, false);
        //    }
        //    else
        //    {
        //        ViewManager.ShowMainInfoFlyout("Die Operation ist fehlgeschlagen", false);
        //    }
        //}

        //private Task<DBResponse> TestFunctionAsync()
        //{
        //    Task<DBResponse> task = Task.Run<DBResponse>(() =>
        //    {
        //        Thread.Sleep(2500);
        //        DBResponse response = new DBResponse
        //        {
        //            Success = true,
        //            Message = "OK"
        //        };
        //        return response;
        //    });
        //    return task;
        //}



        //Executes
        #region Executes
        private bool BeratungslistCanExecute(object obj) => ShowBeratungsliste;
        private bool SitzungslistCanExecute(object obj) => ShowSitzungsliste;
        private void OpenListExecute(object obj)
        {
            if (obj != null)
            {
                switch ((string)obj)
                {
                    case "Sitzungsliste":
                        OpenOrDeleteListFile("Sitzungsliste", 1);
                        break;
                    case "Beratungsliste":
                        OpenOrDeleteListFile("Beratungsliste", 1);
                        break;
                }
            }
        }
        private void BinListExecute(object obj)
        {
            if (obj != null)
            {
                switch ((string)obj)
                {
                    case "Sitzungsliste":
                        OpenOrDeleteListFile("Sitzungsliste", 2);
                        break;
                    case "Beratungsliste":
                        OpenOrDeleteListFile("Beratungsliste", 2);
                        break;
                }
            }
        }
        private void OpenOrDeleteListFile(string List, int Art) //1 = Open; 2 = Delete
        {
            List<string> errorList = new List<string>();
            string errorText = $"Es konnte keine {List} gefunden werden.";
            string path = $"{SelectedSitzungstage.FullDirectory}\\_{List}\\";
            string message = (Art == 1) ? "Folgende Dateien konnten nicht eingelesen werden: " : "Folgende Dateien konnten nicht gelöscht werden: ";
            if (!Directory.Exists(path))
            {
                ViewManager.ShowMainInfoFlyout(errorText, false);
                return;
            }
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.GetFiles().Count() == 0)
            {
                ViewManager.ShowMainInfoFlyout(errorText, false);
                return;
            }
            if (Art == 2)
            {
                bool Antwort = ViewManager.ShowQuestionWindow("Sollen die Dateien gelöscht werden?", "Ja");
                if (Antwort == false) return;
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                try
                {
                    switch (Art)
                    {
                        case 1:
                            Process.Start(new ProcessStartInfo(file.FullName) { UseShellExecute = true });
                            break;
                        case 2:
                            File.Delete(file.FullName);
                            ViewManager.ShowMainInfoFlyout("Die Dateien wurden gelöscht", false);
                            switch (List)
                            {
                                case "Sitzungsliste":
                                    ShowSitzungsliste = false;
                                    break;
                                case "Beratungsliste":
                                    ShowBeratungsliste = false;
                                    break;
                            }
                            break;
                    }
                }
                catch (Exception)
                {
                    errorList.Add($"{file.Name}; ");
                }
            }
            if (errorList.Count > 0)
            {
                foreach (string fileName in errorList)
                {
                    message += fileName;
                }
                ViewManager.ShowMainInfoFlyout(message, false);
            }


        }
        private async void LeitsatzverteilenExecute(object obj)
        {
            string actionName = "Leitsatz verteilen";
            Task<DBResponse> task = Leitsatzverteilenasync();
            ViewManager.ActionlistAdd(actionName);
            await task;
            string message = task.Result.Success ? "Die E-Mail wurde erstellt. Wechseln Sie bitte für den Versand zu Outlook." : task.Result.Message;
            ViewManager.ShowMainInfoFlyout(message, false);
            ViewManager.ActionlistRemove(actionName);
        }
        private Task<DBResponse> Leitsatzverteilenasync()
        {
            Task<DBResponse> task = Task.Run<DBResponse>(() =>
            {
                DBResponse response = new DBResponse();
                if (SelectedUnterlagen != null)
                {
                    if (SelectedUnterlagen.FileName_WithoutPath.Contains(UserManager.SenatSettings.ImportLeitsatz))
                    {
                        response = CreaterEMail();
                        if (!response.Success) return response;
                        EMailResponse eMail = (EMailResponse)response.Data;
                        EMailVersand eMailVersand = new EMailVersand();
                        response = eMailVersand.Send_Email(eMail.EmailTo, eMail.Subject, eMail.Body, attachmentList: eMail.Attachments);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = $"Bitte wählen Sie eine Leitsatz-Datei (*{UserManager.SenatSettings.ImportLeitsatz}.pdf) aus.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Bitte wählen Sie eine Dokument aus.";
                }
                return response;
            });
            return task;
        }
        private DBResponse CreaterEMail()
        {
            DBResponse resp = new DBResponse();
            EMailResponse eMail = new EMailResponse();
            try
            {
                string strEMailAdresses = string.Empty;

                eMail.Subject = $"Sitzungstag: {SelectedSitzungstage.Anzeigedatum} - Leitsatz für {SelectedVerfahren.Verfahren_Anzeigedaten}";
                eMail.Body = $"Sehr geehrte Damen und Herren, <br> <br> anbei erhalten Sie den Leitsatz zu der Entscheidung in dem Verfahren {SelectedVerfahren.Verfahren_Anzeigedaten}. <br> <br> " +
                    $"Mit freundlichen Grüßen <br> GS {UserManager.SenatSettings.Senat.SenatShort}";
                //resp.Success = false;
                //resp.Message = "Senatmembers: ";
                List<User> MemberList = new List<User>();
                var tempList = userDBContext.Senate.Include(x => x.Users).FirstOrDefault(s => s.SenatID == UserManager.SenatSettings.SenatID);
                foreach (var item in tempList.Users) MemberList.Add(item);

                List<User> SGMemberList = new List<User>();
                if (SelectedVerfahren.Spruchgruppe != null)
                {
                    var tempSGList = userDBContext.SenatSpruchgruppen.Include(x => x.Members).FirstOrDefault(s => s.SenatSpruchgruppeID == SelectedVerfahren.Spruchgruppe.SenatSpruchgruppeID);
                    foreach (var item in tempSGList.Members) SGMemberList.Add(item);
                }
                foreach (User Member in MemberList)
                {
                    if (Member.PositionId == 2 || SGMemberList.FirstOrDefault(x => x.UserId == Member.UserId) == null) strEMailAdresses = !(strEMailAdresses == "") ? strEMailAdresses + ";" + Member.EMail.ToString() : Member.EMail.ToString(); //nur WiMa(PositionID = 2) hinzufügen
                }
                eMail.EmailTo = strEMailAdresses;
                //foreach (User Member in MemberList) resp.Message += strEMailAdresses;
                //resp.Message = SelectedVerfahren.Spruchgruppe.SenatSpruchgruppeID.ToString();

                List<CustomMailAttachment> attachmentList = new List<CustomMailAttachment>
                {
                new CustomMailAttachment { AttachmentPath = SelectedUnterlagen.FileName_Fullpath, AttachmentName = SelectedUnterlagen.FileName_WithoutPath }
                };
                eMail.Attachments = attachmentList;
                resp.Success = true;
                resp.Data = eMail;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = $"Die E-Mail konnte nicht erstellt werden. Es ist folgender Fehler aufgetreten: {ex.Message}";
            }
            return resp;
        }

        private bool OpenCanExecute(object obj)
        {
            if (SelectedUnterlagen == null) return false;
            return (SelectedUnterlagen.FileName_Fullpath != "Es sind keine Unterlagen vorhanden.");
        }
        private void ImportExecute(object obj)
        {
            //Import_Mainview

            if (obj != null)
            {
                switch ((string)obj)
                {
                    case "Import_Sidemenu":
                        int Counter = 0;
                        try
                        {
                            List<string> dirImport = new List<string>(Directory.EnumerateDirectories(pathImport));

                            foreach (var dirI in dirImport)
                            {
                                string Jahr = dirI.Substring(dirI.LastIndexOf("\\") + 1);
                                string pathJahr = $"{pathImport}{Jahr}";
                                List<string> dirSitzung = new List<string>(Directory.EnumerateDirectories(pathJahr));
                                foreach (var dirS in dirSitzung)
                                {
                                    string Sitzung = dirS.Substring(dirS.LastIndexOf("\\") + 1);
                                    string pathSitzung = pathJahr + "\\" + Sitzung;
                                    List<string> dirVerfahren = new List<string>(Directory.EnumerateDirectories(pathSitzung));
                                    foreach (var dirV in dirVerfahren)
                                    {
                                        string Verfahren = dirV.Substring(dirV.LastIndexOf("\\") + 1);
                                        string pathVerfahren = pathSitzung + "\\" + Verfahren;
                                        List<string> dirUnterlagen = new List<string>(Directory.EnumerateDirectories(pathVerfahren));
                                        foreach (var dirU in dirUnterlagen)
                                        {
                                            string DokArt = dirU.Substring(dirU.LastIndexOf("\\") + 1);
                                            string dirDokart = pathVerfahren + "\\" + DokArt;

                                            if (Directory.GetFiles(dirDokart).Length > 0)
                                            {
                                                foreach (string Dok in Directory.GetFiles(dirDokart))
                                                {
                                                    string FileName = string.Empty;
                                                    string Verfahrensnummer = Verfahren.Substring(Verfahren.LastIndexOf(" ") + 1);
                                                    Verfahrensnummer = Verfahrensnummer.Substring(0, Verfahrensnummer.LastIndexOf("-"));
                                                    FileName = Verfahrensnummer;

                                                    switch (DokArt)
                                                    {
                                                        case "Votum":
                                                            FileName += UserManager.SenatSettings.ImportVotum;
                                                            break;
                                                        case "Vorvotum":
                                                            FileName += UserManager.SenatSettings.ImportVorVotum;
                                                            break;
                                                        case "Entscheidungsentwurf":
                                                            FileName += UserManager.SenatSettings.ImportEntwurf;
                                                            break;
                                                        case "EuGH-Vorlage":
                                                            FileName += UserManager.SenatSettings.ImportEUGHVorlage;
                                                            break;
                                                        case "EuGH-Urteil":
                                                            FileName += UserManager.SenatSettings.ImportEUGHURteil;
                                                            break;
                                                        case "OLG Urteil":
                                                            FileName += UserManager.SenatSettings.ImportOLGUrteil;
                                                            break;
                                                        case "OLG Beschluss":
                                                            FileName += UserManager.SenatSettings.ImportOLGBeschluss;
                                                            break;
                                                        case "OLG ZB":
                                                            FileName += UserManager.SenatSettings.ImportOLGZB;
                                                            break;
                                                        case "OLG HB":
                                                            FileName += UserManager.SenatSettings.ImportOLGHB;
                                                            break;
                                                        case "LG Urteil":
                                                            FileName += UserManager.SenatSettings.ImportLGUrteil;
                                                            break;
                                                        case "LG Beschluss":
                                                            FileName += UserManager.SenatSettings.ImportLGBeschluss;
                                                            break;
                                                        case "LG ZB":
                                                            FileName += UserManager.SenatSettings.ImportLGZB;
                                                            break;
                                                        case "LG HB":
                                                            FileName += UserManager.SenatSettings.ImportLGHB;
                                                            break;
                                                        case "AG Urteil":
                                                            FileName += UserManager.SenatSettings.ImportAGUrteil;
                                                            break;
                                                        case "AG Beschluss":
                                                            FileName += UserManager.SenatSettings.ImportAGBeschluss;
                                                            break;
                                                        case "Anlage":
                                                            FileName += UserManager.SenatSettings.ImportAnlage;
                                                            break;
                                                        case "Leitsatz":
                                                            FileName += UserManager.SenatSettings.ImportLeitsatz;
                                                            break;
                                                        case "RMB":
                                                            FileName += UserManager.SenatSettings.ImportRMB;
                                                            break;
                                                        case "RME":
                                                            FileName += UserManager.SenatSettings.ImportRME;
                                                            break;
                                                        case "Sonstiges":
                                                            FileName += UserManager.SenatSettings.ImportSonstiges;
                                                            break;


                                                    }
                                                    FileName += "." + Dok.Substring(Dok.LastIndexOf(".") + 1);
                                                    string dirPath = pathMainDirectory + Jahr + "\\" + Sitzung + "\\" + Verfahren;
                                                    if (Directory.Exists(dirPath))
                                                    {
                                                        string targetDir = pathMainDirectory + Jahr + "\\" + Sitzung + "\\" + Verfahren + "\\" + FileName;
                                                        int DirCounter = 1;
                                                        while (File.Exists(targetDir))
                                                        {
                                                            int Endung = targetDir.LastIndexOf(".");
                                                            targetDir = targetDir.Insert(Endung, "(" + DirCounter + ")");
                                                            DirCounter++;
                                                            if (DirCounter > 10) break;
                                                        }
                                                        File.Move(Dok.ToString(), targetDir);
                                                        Counter++;
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Der Import für die Datei " + Dok.ToString() + " konnte nicht erfolgen, weil das Zielverzeichnis nicht besteht.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                                                    }
                                                }
                                            }
                                            //Dokumentenordner löschen
                                            DirectoryDelete(dirDokart);
                                        }
                                        //Verfahrensordner löschen
                                        DirectoryDelete(pathVerfahren);
                                    }
                                    //Sitzungstagordner löschen
                                    DirectoryDelete(pathSitzung);
                                }
                                //Jahr löschen
                                DirectoryDelete(pathJahr);
                            }

                            if (Counter > 0)
                            {
                                ViewManager.ShowMainInfoFlyout("Die Dateien wurden erfolgreich importiert.", true);
                            }
                            else
                            {
                                MessageBox.Show("Im Importordner wurden keinen Dateien gefunden.", "Import abgeschlossen", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Der Import ist fehlgeschlagen. Es ist folgender Fehler aufgetreten: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        finally
                        {
                            UnterlagenListeFill();
                        }
                        break;
                    case "Import_Mainview":
                        string path = $"{pathImport}{SelectedSitzungstage.Jahr}\\{SelectedSitzungstage.Rohdatum}\\{SelectedVerfahren.Verfahren_Rohinformation}";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                            Directory.CreateDirectory(path + "\\" + "Votum");
                            Directory.CreateDirectory(path + "\\" + "Vorvotum");
                            Directory.CreateDirectory(path + "\\" + "Entscheidungsentwurf");
                            Directory.CreateDirectory(path + "\\" + "EuGH-Vorlage");
                            Directory.CreateDirectory(path + "\\" + "EuGH-Urteil");
                            Directory.CreateDirectory(path + "\\" + "OLG Urteil");
                            Directory.CreateDirectory(path + "\\" + "OLG Beschluss");
                            Directory.CreateDirectory(path + "\\" + "OLG ZB");
                            Directory.CreateDirectory(path + "\\" + "OLG HB");
                            Directory.CreateDirectory(path + "\\" + "LG Urteil");
                            Directory.CreateDirectory(path + "\\" + "LG Beschluss");
                            Directory.CreateDirectory(path + "\\" + "LG HB");
                            Directory.CreateDirectory(path + "\\" + "LG ZB");
                            Directory.CreateDirectory(path + "\\" + "AG Urteil");
                            Directory.CreateDirectory(path + "\\" + "AG Beschluss");
                            Directory.CreateDirectory(path + "\\" + "Anlage");
                            Directory.CreateDirectory(path + "\\" + "Leitsatz");
                            Directory.CreateDirectory(path + "\\" + "RMB");
                            Directory.CreateDirectory(path + "\\" + "RME");
                            Directory.CreateDirectory(path + "\\" + "Sonstiges");

                            if (MessageBox.Show("Für dieses Verfahren wurde ein Unterordner im Importordner angelegt. Möchten Sie diesen öffnen?", "Anlage erfolgreich", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                Process.Start("explorer.exe", path);
                            }
                        }
                        else
                        {
                            Process.Start("explorer.exe", path);
                        }

                        break;
                    case "Votenmappe":
                        string Text = string.Empty;
                        if (userDBContext.Votenmappe.FirstOrDefault(v => v.Verfahren_FullPath == SelectedVerfahren.Verfahren_FullPath) == null)
                        {
                            try
                            {
                                VerfahrenVotenmappe newVerfahren = new VerfahrenVotenmappe();
                                newVerfahren.FillVerfahren(SelectedVerfahren, userDBContext.Senate.FirstOrDefault(x => x.SenatID == UserManager.SenatSettings.Senat.SenatID));
                                //Senat senat = new Senat();
                                //if (SelectedVerfahren.Senat != null) senat = userDBContext.Senate.FirstOrDefault(x => x.SenatID == SelectedVerfahren.Senat.SenatID);
                                //if (senat != null) senat.Senatsetting = userDBContext.SenatSettings.FirstOrDefault(x => x.SenatID == SelectedVerfahren.Senat.SenatID);
                                //SenatAktenzeichen registerzeichen = new SenatAktenzeichen();
                                //if (SelectedVerfahren.Registerzeichen != null) registerzeichen = userDBContext.SenatAktenzeichen.FirstOrDefault(x => x.SenatAktenzeichenID == SelectedVerfahren.Registerzeichen.SenatAktenzeichenID);
                                //SenatSpruchgruppe spruchgruppe = new SenatSpruchgruppe();
                                //if (spruchgruppe != null) spruchgruppe.SenatSetting = userDBContext.SenatSettings.FirstOrDefault(x => x.SenatID == SelectedVerfahren.Senat.SenatID);
                                //if (SelectedVerfahren.Spruchgruppe != null) spruchgruppe = userDBContext.SenatSpruchgruppen.FirstOrDefault(x => x.SenatSpruchgruppeID == SelectedVerfahren.Spruchgruppe.SenatSpruchgruppeID);
                                //Verfahren newVerfahren = new Verfahren (SelectedVerfahren, senat, registerzeichen, spruchgruppe);

                                userDBContext.Votenmappe.Add(newVerfahren);
                                userDBContext.SaveChanges();
                                Text = "Das Verfahren wurde in die Votenmappe gelegt.";
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Es ist folgender Fehler aufgetreten: {ex.Message}");
                            }
                        }
                        else
                        {
                            Text = "Das Verfahren befindet sich bereits in der Votenmappe.";
                        }
                        ViewManager.ShowMainInfoFlyout(Text, false);
                        break;
                }
            }

        }
        private void CancelExecute(object obj)
        {
            //Dient zur Einblendung der Bereiche Sitzungstage, Verfahren und Dokumente
            ShowEnableCards = true;
            ShowSubscribe = false;
            ShowSitzungstage = StateVisibilitySitzungstage;
            ShowVerfahren = StateVisibilityVerfahren;
            ShowUnterlagen = StateVisibilityUnterlagen;
        }
        private void SubmitExecute(object obj)
        {

            switch (SelectedArtSitzungsunterlagen)
            {
                case 1:
                case 2:
                    SitzungstageAddOREdit();
                    break;
                case 3:
                case 4:
                    VerfahrenAddOREdit();
                    break;
                case 6:
                    UnterlagenEdit();
                    break;
                case 7:
                    UnterlagenDrop();
                    break;
            }
        }
        private void UnterlagenDrop()
        {
            if (UserManager.SenatSettings.AZPrefixDate)
            {
                if (ImportAGUrteil
                    || ImportAGBeschluss
                    || ImportLGUrteil
                    || ImportLGBeschluss
                    || ImportLGHB
                    || ImportLGZB
                    || ImportOLGUrteil
                    || ImportOLGBeschluss
                    || ImportOLGHB
                    || ImportOLGZB)
                {
                    if (Gerichtsort == string.Empty || Entscheidungsdatum == string.Empty)
                    {
                        ViewManager.ShowMainInfoFlyout("Bitte geben Sie den Ort und das Datum für die Entscheidung an.", false);
                        return;
                    }
                }
            }
            FileInfo file = new FileInfo(ImportFilename);
            if (ImportName != string.Empty)
            {
                try
                {
                    string path = SelectedVerfahren.Verfahren_FullPath;
                    string targetDir = $"{path}\\{ImportName}";
                    File.Copy(file.FullName, targetDir);
                    UnterlagenList.Add(new Unterlagen(targetDir));
                    DeleteNoUnterlagen();
                    //ViewManager.ShowMainInfoFlyout("Die Datei wurde importiert.", false);
                    CancelExecute("");
                }
                catch (Exception)
                {
                    ViewManager.ShowMainInfoFlyout("Die Datei konnte nicht importiert werden.", false);
                }
            }
            else
            {
                ViewManager.ShowMainInfoFlyout("Die Dokumentenart konnte nicht gefüllt werden. Bitte prüfen Sie, ob eine Dokumentenart ausgewählt wurde oder ob für die Dokumentart ein Text in den Einstellungen eingetragen wurde.", false);
            }

        }
        private void UnterlagenEdit()
        {
            string captionOld = SelectedUnterlagen.FileName_Fullpath;
            string captionNew = $"{SelectedUnterlagen.FileName_PlainPath}\\{TextFileName}{SelectedUnterlagen.FileName_Extention}";
            //SelectedURI = new Uri("about:blank");
            try
            {
                File.Move(captionOld, captionNew);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Die Datei kann nicht unbenannt werden. Es ist folgender Fehler aufgetreten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Unterlagen NewUnterlagen = new Unterlagen(captionNew);
            UnterlagenList.Remove(SelectedUnterlagen);
            UnterlagenList.Add(NewUnterlagen);
        }
        private void VerfahrenAddOREdit()
        {
            string ErrorHeader = "Eingabefehler";
            if (SelectedSenat == null)
            {
                ViewManager.ShowMainInfoFlyout("Bitte wählen Sie einen Senat aus.", false);
                return;
            }
            if (SelectedAZ == null)
            {
                ViewManager.ShowMainInfoFlyout("Bitte wählen Sie ein Registerzeichen aus!", false);
                return;
            }
            if (SelectedSG == null)
            {
                MessageBoxResult answer = MessageBox.Show("Es ist keine Spruchgruppe ausgewählt. Möchten Sie trotzdem fortfahren?", "Spruchgruppe", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.No) return;
            }

            try
            {
                string newPath = $"{pathMainDirectory}{SelectedSitzungstage.Jahr}\\{SelectedSitzungstage.Rohdatum}\\{SelectedSenat.SenatShort} {SelectedAZ.SenatAktenzeichenName} {TextLaufendeNummer}-{TextJahr}";
                if (SelectedSG != null) newPath += $"-SG{SelectedSG.SenatSpruchgruppeName}";
                Verfahren NewVerfahren = new Verfahren(newPath);

                if (!Directory.Exists(NewVerfahren.Verfahren_FullPath))
                {
                    if (SelectedArtSitzungsunterlagen == 3)
                    {
                        try
                        {
                            Directory.CreateDirectory(NewVerfahren.Verfahren_FullPath);
                            VerfahrenList.Add(NewVerfahren);
                            SortVerfahren();
                            ViewManager.ShowMainInfoFlyout($"Das Verfahren wurde angelegt.", false);
                        }
                        catch (Exception ex)
                        {
                            ViewManager.ShowMainInfoFlyout($"Das Verfahren konnte nicht angelegt werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
                        }
                    }
                    else
                    {
                        try
                        {
                            Directory.Move(SelectedVerfahren.Verfahren_FullPath, NewVerfahren.Verfahren_FullPath);
                            VerfahrenList.Remove(SelectedVerfahren);
                            VerfahrenList.Add(NewVerfahren);
                            SortVerfahren();
                            ViewManager.ShowMainInfoFlyout($"Das Verfahren wurde umbenannt.", false);
                        }
                        catch (Exception ex)
                        {
                            ViewManager.ShowMainInfoFlyout($"Das Verfahren konnte nicht angelegt werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
                        }
                    }
                }
                else
                {
                    ViewManager.ShowMainInfoFlyout("Dieses Verfahren besteht bereits.", false);
                }
            }
            catch (Exception)
            {
                string messagetext = string.Empty;
                MessageBox.Show(messagetext, ErrorHeader, MessageBoxButton.OK, MessageBoxImage.Stop);
            }

            ShowEnableCards = true;
            ShowSubscribe = false;

            ShowSitzungstage = StateVisibilitySitzungstage;
            ShowVerfahren = StateVisibilityVerfahren;
            ShowUnterlagen = StateVisibilityUnterlagen;
        }
        private void SitzungstageAddOREdit()
        {
            if (SelectedDate == null) ViewManager.ShowMainInfoFlyout("Bitte geben Sie ein Datum ein!", false);
            else
            {
                int intDay = SelectedDate.Day;
                string strDay = intDay.ToString();
                if (intDay <= 9) strDay = "0" + strDay;
                int intMonth = SelectedDate.Month;
                string strMonth = intMonth.ToString();
                if (intMonth <= 9) strMonth = "0" + strMonth;
                string sitzungstag = $"{SelectedDate.Year}_{strMonth}_{strDay}";
                string path = $"{pathMainDirectory}\\{SelectedDate.Year}\\{sitzungstag}\\";

                switch (SelectedArtSitzungsunterlagen)
                {
                    case 1: //Add
                        try
                        {
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                                string pathSitzung = path + "\\_Sitzungsliste";
                                Directory.CreateDirectory(pathSitzung);
                                string pathBeratung = path + "\\_Beratungsliste";
                                Directory.CreateDirectory(pathBeratung);
                                SitzungstageAddAndSort($"{sitzungstag}");
                                ViewManager.ShowMainInfoFlyout($"Der Ordner für den Sitzungstag am {SelectedDate:dd.MM.yyyy} wurde angelegt.", false);
                            }
                            else
                            {
                                ViewManager.ShowMainInfoFlyout("Für dieses Datum exisitert bereits ein Sitzungstag.", false);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show("Es konnte kein Ordner erstellt werden. Es ist folgender Fehler aufgetreten: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        break;
                    case 2: //Edit
                        string oldfolder = $"{pathMainDirectory}\\{SelectedSitzungstage.Jahr}\\{SelectedSitzungstage.Rohdatum}\\";
                        try
                        {
                            SelectedSitzungstage.Tag = strDay;
                            SelectedSitzungstage.Monat = strMonth;
                            SelectedSitzungstage.Jahr = SelectedDate.Year.ToString();
                            Directory.Move(oldfolder, path);
                            SitzungstageList.Remove(SelectedSitzungstage);
                            SitzungstageAddAndSort($"{sitzungstag}");
                            ViewManager.ShowMainInfoFlyout($"Der Ordner wurde umbenannt.", false);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Der Sitzungstag konnte nicht umbenannt werden. Es ist folgender Fehler aufgetreten: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        break;

                }
                ShowEnableCards = true;
                ShowSubscribe = false;
            }
        }
        private void DeleteNoUnterlagen()
        {
            Unterlagen DeleteItem = UnterlagenList.FirstOrDefault(x => x.FileName_Fullpath == "Es sind keine Unterlagen vorhanden");
            if (DeleteItem != null) UnterlagenList.Remove(DeleteItem);
        }
        private void SitzungstageAddAndSort(string Sitzungstag)
        {
            DBResponse respAdd = SitzungstagAddToList(Sitzungstag);
            if (respAdd.Success && respAdd.Data != null)
            {
                SitzungstageList.Add((Sitzungstage)respAdd.Data);
                SelectedSitzungstage = (Sitzungstage)respAdd.Data;
            }
            ObservableCollection<Sitzungstage> Sortlist = new ObservableCollection<Sitzungstage>();
            foreach (Sitzungstage v in SitzungstageList.OrderBy(x => x.Jahr).ThenBy(x => x.Monat).ThenBy(x => x.Tag).ToList()) Sortlist.Add(v);
            SitzungstageList.Clear();
            foreach (Sitzungstage v in Sortlist) SitzungstageList.Add(v);
        }
        private void SubscribeExecute(object obj)
        {
            if (obj != null)
            {
                //1 = Add (Sitzungsunterlagen); 2 = Edit (Sitzungsunterlagen; 3 = Add (Verfahren)
                switch ((string)obj)
                {
                    case "Add_Sitzung":
                        ShowSubscribeSet(stateVerfahren: false, stateUnterlagen: false, showSitzungstage: true, showSubscribeSitzungstage: true);
                        SelectedArtSitzungsunterlagen = 1;
                        SitzungstageTitle = "Sitzungstag anlegen";
                        SitzungstageSubTitle = "Bitte geben Sie das Datum für den Sitzungstag ein.";
                        ButtonChangeTitle = "Anlegen";
                        break;
                    case "Edit_Sitzung":
                        ShowSubscribeSet(stateVerfahren: false, stateUnterlagen: false, showSitzungstage: true, showSubscribeSitzungstage: true);
                        SelectedArtSitzungsunterlagen = 2;
                        SitzungstageTitle = "Sitzungstag umbenennen";
                        SitzungstageSubTitle = $"Bitte geben Sie ein neues Datum für den Sitzungstag an.\n Das bisherige Datum war der {SelectedSitzungstage.Anzeigedatum}.";
                        ButtonChangeTitle = "Umbenennen";
                        SelectedDate = new DateTime(Int32.Parse(SelectedSitzungstage.Jahr), Int32.Parse(SelectedSitzungstage.Monat), Int32.Parse(SelectedSitzungstage.Tag));
                        break;
                    case "Add_Verfahren":
                        if (SelectedSitzungstage == null)
                        {
                            MessageBox.Show("Bitte wählen Sie einen Sitzungstag aus, für den das Verfahren angelegt werden soll.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }
                        resp = Cbo_Senat_Fill(SenatList);
                        if (resp.Success)
                        {
                            ShowSubscribeSet(stateUnterlagen: false, showVerfahren: true, showSubscribeVerfahren: true);
                            SelectedArtSitzungsunterlagen = 3;
                            SelectedSenat = SenatList.Count > 0 ? SenatList.FirstOrDefault(x => x.SenatID == UserManager.SenatSettings.SenatID) : null;
                            SelectedAZ = AZList.Count > 0 ? AZList.ElementAt(0) : null;
                            SitzungstageTitle = "Verfahren anlegen";
                            SitzungstageSubTitle = "Bitte geben Sie ein Aktenzeichen für das neuanzulegende Verfahren ein.";
                            ButtonChangeTitle = "Anlegen";
                        }
                        else ViewManager.ShowMainInfoFlyout(resp.Message, false);
                        break;
                    case "Edit_Verfahren":
                        resp = Cbo_Senat_Fill(SenatList);
                        if (resp.Success)
                        {
                            ShowSubscribeSet(stateUnterlagen: false, showVerfahren: true, showSubscribeVerfahren: true);
                            SelectedArtSitzungsunterlagen = 4;
                            SitzungstageTitle = "Verfahren umbenennen";
                            SitzungstageSubTitle = "Bitte geben Sie ein neues Aktenzeichen für dasVerfahren ein.";
                            ButtonChangeTitle = "Umbenennen";
                            SelectedSenat = SenatList.FirstOrDefault(x => x.SenatID == SelectedVerfahren.Senat.SenatID);
                            SelectedAZ = AZList.Count > 0 ? AZList.FirstOrDefault(x => x.SenatAktenzeichenID == SelectedVerfahren.Registerzeichen.SenatAktenzeichenID) : null;
                            SelectedSG = SGList.Count > 0 ? SGList.FirstOrDefault(x => x.SenatSpruchgruppeID == SelectedVerfahren.Spruchgruppe.SenatSpruchgruppeID) : null;
                            TextLaufendeNummer = SelectedVerfahren != null ? SelectedVerfahren.LaufendeNummer : string.Empty;
                            TextJahr = SelectedVerfahren.Jahr;
                        }
                        else
                        {
                            ViewManager.ShowMainInfoFlyout(resp.Message, false);
                        }
                        break;
                    case "Add_Unterlagen":
                        SelectedArtSitzungsunterlagen = 5;
                        string zielVerzeichnis = $"{pathMainDirectory}{SelectedSitzungstage.Jahr}\\{SelectedSitzungstage.Rohdatum}\\{SelectedVerfahren.Verfahren_Rohinformation}";
                        OpenFileDialog openFileDialog = new OpenFileDialog
                        {
                            Filter = "Text-Dateien (*.docx; *.pdf)|*.docx;*.pdf|Word-Dateien (*.docx)|*.docx|pdf-Dateien (*.pdf)|*.pdf|Alle Dateien (*.*)|*.*",
                            Multiselect = true,
                            Title = "Dokumente hinzufügen",
                            InitialDirectory = pathMainDirectory
                        };
                        if (openFileDialog.ShowDialog() == true)
                        {
                            try
                            {
                                foreach (string filename in openFileDialog.FileNames)
                                {
                                    Unterlagen CopyUnterlagen = new Unterlagen(filename);
                                    Unterlagen CopiedFile = CopyUnterlagen.Copy(zielVerzeichnis);
                                    UnterlagenList.Add(CopiedFile);
                                }
                                DeleteNoUnterlagen();
                                //UnterlagenListeFill(); //Neues Füllen vermeiden
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Die Unterlagen konnten nicht eingelesen werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        break;
                    case "Edit_Unterlagen":
                        SelectedURI = new Uri("about:blank");
                        ShowSubscribeSet(showUnterlagen: true, showSubscribeUnterlagenEdit: true);
                        SelectedArtSitzungsunterlagen = 6;
                        SitzungstageTitle = "Dokument umbenennen";
                        SitzungstageSubTitle = "Bitte geben Sie eine neue Bezeichnung für das Dokument ein. Die Endung ist nicht miteinzutragen.";
                        ButtonChangeTitle = "Umbenennen";
                        TextFileName = SelectedUnterlagen.FileName_WithoutPath;
                        TextFileExtention = SelectedUnterlagen.FileName_Extention;
                        break;
                }
            }
        }

        private void ShowSubscribeSet(bool stateSitzungstage = true,
            bool stateVerfahren = true,
            bool stateUnterlagen = true,
            bool showSitzungstage = false,
            bool showVerfahren = false,
            bool showUnterlagen = false,
            bool showSubscribeSitzungstage = false,
            bool showSubscribeVerfahren = false,
            bool showSubscribeUnterlagenEdit = false,
            bool showSubscribeUnterlagenAdd = false)
        {
            //Status setzen
            StateVisibilitySitzungstage = stateSitzungstage;
            StateVisibilityVerfahren = stateVerfahren;
            StateVisibilityUnterlagen = stateUnterlagen;

            //Eingabebereich anzeigen
            ShowEnableCards = false;
            ShowSubscribe = true;
            ShowSubscribeSitzungstage = showSubscribeSitzungstage;
            ShowSubscribeVerfahren = showSubscribeVerfahren;
            ShowSubscribeUnterlagenEdit = showSubscribeUnterlagenEdit;
            ShowSubscribeUnterlagenAdd = showSubscribeUnterlagenAdd;

            //Übrige Bereiche ausblenden
            ShowSitzungstage = showSitzungstage;
            ShowVerfahren = showVerfahren;
            ShowUnterlagen = showUnterlagen;


        }

        private void BinExecute(object obj)
        {
            if (obj != null)
            {
                switch ((string)obj)
                {
                    case "Bin_Sitzung":
                        if (MessageBox.Show("Soll der Ordner für den Sitzungstag am " + SelectedSitzungstage.Anzeigedatum + " gelöscht werden?", "Sitzungstag löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                            return;
                        try
                        {
                            Directory.Delete($"{pathMainDirectory}\\{SelectedSitzungstage.Jahr}\\{SelectedSitzungstage.Rohdatum}", true);
                            SitzungstageList.Remove(SelectedSitzungstage);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Der Sitzungstag konnte nicht gelöscht werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    case "Bin_Verfahren":
                        if (MessageBox.Show("Soll der Ordner für das Verfahren " + SelectedVerfahren.Verfahren_Anzeigedaten + " gelöscht werden?", "Verfahren löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                            return;
                        try
                        {
                            Directory.Delete($"{pathMainDirectory}\\{SelectedSitzungstage.Jahr}\\{SelectedSitzungstage.Rohdatum}\\{SelectedVerfahren.Verfahren_Rohinformation}\\", true);
                            VerfahrenList.Remove(SelectedVerfahren);
                            VerfahrenView.Remove(SelectedVerfahren);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Das Verfahren konnte nicht gelöscht werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    case "Bin_Unterlagen":
                        if (SelectedUnterlagen.FileName_Fullpath == "Es sind keine Unterlagen vorhanden")
                        {
                            ViewManager.ShowMainInfoFlyout("Es sind keine Unterlagen in dem Verfahren vorhanden. Bitte fügen Sie zunächst eine Datei dem Verfahren an.", false);
                            //MessageBox.Show($"Bitte fügen Sie zunächst eine Datei dem Verfahren an.", "Keine Datei ausgewählt", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (MessageBox.Show($"Soll die Datei mit der Bezeichnung {SelectedUnterlagen.FileName_WithoutPath} gelöscht werden?", "Datei löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                            return;
                        try
                        {
                            File.Delete(SelectedUnterlagen.FileName_Fullpath);
                            UnterlagenList.Remove(SelectedUnterlagen);
                            SelectedURI = new Uri("about:blank");

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Die Datei konnte nicht gelöscht werden. Es ist folgender Fehler aufgetreten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    default:
                        MessageBox.Show("Fehler");
                        break;

                }
            }
        }
        private void OpenExecute(object obj)
        {
            if (obj != null)
            {
                switch ((string)obj)
                {
                    case "Open_Unterlagen":
                        if (SelectedUnterlagen.FileName_Fullpath == "Es sind keine Unterlagen vorhanden")
                        {
                            ViewManager.ShowMainInfoFlyout("Es sind keine Unterlagen in dem Verfahren vorhanden. Bitte fügen Sie zunächst eine Datei dem Verfahren an.", false);
                            return;
                        }
                        Process.Start(new ProcessStartInfo(SelectedUnterlagen.FileName_Fullpath) { UseShellExecute = true });
                        break;
                }
            }
        }
        private void PrintExecute(object obj)
        {

            if (SelectedUnterlagen.FileName_Fullpath == "Es sind keine Unterlagen vorhanden")
            {
                switch (SelectedUnterlagen.FileName_Extention)
                {
                    case ".pdf":
                        MessageBox.Show("Derzeit können keine pdf-Dokumente ausgedruckt werden. Öffnen Sie stattdessen das Dokument und drucken es bitte über den pdf-Reader aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case ".docx":
                        object instance = Activator.CreateInstance(Type.GetTypeFromProgID("Word.Application"));
                        object target1 = instance.GetType().InvokeMember("Documents", System.Reflection.BindingFlags.GetProperty, (System.Reflection.Binder)null, instance, (object[])null);
                        object[] args = new object[1]
                        {
                          (object) SelectedUnterlagen.FileName_PlainPath
                        };
                        object target2 = target1.GetType().InvokeMember("Open", System.Reflection.BindingFlags.InvokeMethod, (System.Reflection.Binder)null, target1, args);
                        target2.GetType().InvokeMember("PrintOut", System.Reflection.BindingFlags.InvokeMethod, (System.Reflection.Binder)null, target2, (object[])null);
                        target2.GetType().InvokeMember("Close", System.Reflection.BindingFlags.InvokeMethod, (System.Reflection.Binder)null, target2, (object[])null);
                        instance.GetType().InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, (System.Reflection.Binder)null, instance, (object[])null);
                        break;
                    default:
                        MessageBox.Show("Eine Datei mit der Dateiendung: " + SelectedUnterlagen.FileName_Extention + " kann nicht ausgedruckt werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
            else
            {
                //MessageBox.Show("Bitte wählen Sie ein Dokument aus.", "Auswahl", MessageBoxButton.OK, MessageBoxImage.Information);
                ViewManager.ShowMainInfoFlyout("Es sind keine Unterlagen in dem Verfahren vorhanden. Bitte fügen Sie zunächst eine Datei dem Verfahren an.", false);
            }

        }
        private void ExportExecute(object obj)
        {
            if (obj != null)
            {
                switch ((string)obj)
                {
                    case "Desktop":

                        try
                        {
                            Export_Desktop();
                        }
                        catch (Exception ex)
                        {
                            ViewManager.ShowMainInfoFlyout($"Der Sitzungstag konnte nicht exportiert werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
                        }
                        break;
                    case "Convert":
                        ViewManager.ShowMainInfoFlyout("Die Docx- und Docm-Dateien werden in pdf-Dateien konvertiert. Sie können währenddessen mit dem Programm weiterarbeiten. ", false);
                        ConvertDocxToPDFAsync();
                        ViewManager.ShowMainInfoFlyout("Die Aktion wurden abgeschlossen.", false);
                        break;
                    case "BSCW_Check":
                        BSCW_Check_Drive(1);
                        break;
                    case "BSCW_Sitzung":
                        if (SelectedSitzungstage == null)
                        {
                            ViewManager.ShowMainInfoFlyout("Bitte wählen Sie einen Sitzungstag aus.", false);
                            return;
                        }
                        BSCW_Check_Drive(2);
                        break;
                    case "BSCW_Verfahren":
                        if (SelectedVerfahren == null)
                        {
                            ViewManager.ShowMainInfoFlyout("Bitte wählen Sie ein Verfahren aus.", false);
                            return;
                        }
                        BSCW_Check_Drive(3);
                        break;
                    case "BSCW_Dokument":
                        if (SelectedUnterlagen == null)
                        {
                            ViewManager.ShowMainInfoFlyout("Bitte wählen Sie ein Dokument aus.", false);
                            return;
                        }
                        BSCW_Check_Drive(4);
                        break;
                    default:
                        MessageBox.Show($"Keine Funktion hinterlegt für {obj}");
                        break;
                }
            }
        }
        private void ShowExecute(object obj)
        {
            if (SelectedSitzungstage != null)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(SelectedSitzungstage.FullDirectory) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    ViewManager.ShowMainInfoFlyout($"Der Sitzungstag konnte nicht geöffnet werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
                }
            }
            else
            {
                ViewManager.ShowMainInfoFlyout("Bitte wählen Sie einen Sitzungstag aus.", false);
            }

        }
        private bool DesktopExportSuccess = false;
        private async void Export_Desktop()
        {
            Task task = ExportToDesktop();
            await task;
            if (DesktopExportSuccess)
            {
                ViewManager.ShowMainInfoFlyout("Der Ordner wurde auf den Desktop kopiert.", false);
            }
        }
        private Task ExportToDesktop()
        {
            Task task = Task.Run(() =>
            {
                string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                int intErgebnis = new DirectoryInfo(SelectedSitzungstage.FullDirectory).DeepCopy(pathDesktop + "\\Sitzung " + SelectedSitzungstage.Anzeigedatum, 1);
                switch (intErgebnis)
                {
                    case 0:
                        DesktopExportSuccess = true;
                        break;
                    case 1:
                        DesktopExportSuccess = false;
                        MessageBox.Show("Der Ordner existiert bereits. Bitte löschen Sie diesen und stoßen den Kopiervorgang dann erneut an.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                        //case 2:
                        //    MessageBox.Show("Die Dateien für den Sitzungstag am " + SelectedSitzungstage.Anzeigedatum + " wurden auf den Desktop kopiert.", "Kopierungvorgang", MessageBoxButton.OK, MessageBoxImage.Information);
                        //    break;
                }
            });
            return task;
        }
        private async void ConvertDocxToPDFAsync()
        {
            if (SelectedVerfahren != null)
            {
                try
                {
                    await DocxToPDFConverter.ConvertFileSingleFolderAsync(SelectedVerfahren.Verfahren_FullPath);
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Es konnten nicht alle Dateien konvertiert werden. Es ist folgender Fehler aufgetreten: {ex.Message}", "Umwandlung", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (SelectedSitzungstage != null)
            {

                try
                {
                    await DocxToPDFConverter.ConvertFileMultiFolderAsync(SelectedSitzungstage.FullDirectory);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Es konnten nicht alle Dateien konvertiert werden. Es ist folgender Fehler aufgetreten: {ex.Message}.", "Umwandlung", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }
        private void VotenmappeExecute(object obj)
        {
            ViewManager.ShowPageOnMainView<VotenMappeView>();
        }

        #endregion
        //BSCW-Server Functions
        #region BSCW-Server-Functions
        private async void BSCW_Check_Drive(int Art)
        {
            string actionName = "Übertragung BSCW-Server";
            if (SelectedSitzungstage != null)
            {
                if (Directory.Exists($"{UserManager.SenatSettings.BSCW_Server_Drive}:\\"))
                {
                    switch (Art)
                    {
                        case 1: //Check_Completed
                            Task<DBResponse> task = BSCWCheckFilesCompletedAsync();
                            ViewManager.ShowMainInfoFlyout($"Die Daten werden eingelesen. Bitte warten Sie.", false);
                            ViewManager.ActionlistAdd(actionName);
                            await task;
                            ViewManager.ActionlistRemove(actionName);
                            if (task.Result.Success) ViewManager.ShowPageOnMainView<BSCWServerView>();
                                else ViewManager.ShowMainInfoFlyout(task.Result.Message, false);
                            break;
                        case 2:
                            ViewManager.ShowMainInfoFlyout("Der ausgewählte Ordner wird auf den BSCW-Server kopiert", false);
                            ViewManager.ActionlistAdd(actionName);
                            string taskCopy = await new DirectoryInfo(SelectedSitzungstage.FullDirectory).DeepCopyBSCWDirAsync(SelectedSitzungstage);
                            ViewManager.ShowMainInfoFlyout(taskCopy, false);
                            ViewManager.ActionlistRemove(actionName);
                            break;
                        case 3: //Kopie Directory
                            ViewManager.ShowMainInfoFlyout("Der ausgewählte Ordner wird auf den BSCW-Server kopiert", false);
                            ViewManager.ActionlistAdd(actionName);
                            string taskCopyVerfahren = await new DirectoryInfo(SelectedVerfahren.Verfahren_FullPath).DeepCopyBSCWDirAsync(SelectedSitzungstage);
                            ViewManager.ShowMainInfoFlyout(taskCopyVerfahren, false);
                            ViewManager.ActionlistRemove(actionName);
                            break;

                        case 4: //Kopie file
                            ViewManager.ShowMainInfoFlyout("Die Datei wird auf den BSCW-Server kopiert", false);
                            ViewManager.ActionlistAdd(actionName);
                            string taskCopyFile = await new FileInfo(SelectedUnterlagen.FileName_Fullpath).DeepCopyBSCWFile(SelectedSitzungstage);
                            ViewManager.ShowMainInfoFlyout(taskCopyFile, false);
                            ViewManager.ActionlistRemove(actionName);
                            break;
                    }
                }
                else ViewManager.ShowMainInfoFlyout($"Der BSCW-Server konnte unter dem Laufwerk {UserManager.SenatSettings.BSCW_Server_Drive}:\\ nicht gefunden werden. Binden Sie bitte den BSCW-Server als Laufwerk ein.", false);
            }
            else ViewManager.ShowMainInfoFlyout("Bitte wählen Sie einen Sitzungstag aus.", false);
        }
        private Task<DBResponse> BSCWCheckFilesCompletedAsync()
        {
            Task<DBResponse> task = Task.Run<DBResponse>(() =>
            {
                DBResponse resp = new DBResponse();
                string BSCW_Server_Path = $"{UserManager.SenatSettings.BSCW_Server_Drive}:\\{SelectedSitzungstage.Jahr}\\{SelectedSitzungstage.Rohdatum}\\";
                if (Directory.Exists(BSCW_Server_Path))
                {
                    ViewManager.Filelist.Clear();
                    List<CompareBSCW> Filelist = new List<CompareBSCW>();
                    foreach (FileInfo file in new DirectoryInfo(SelectedSitzungstage.FullDirectory).GetFiles(searchPattern: "*.*", searchOption: SearchOption.AllDirectories))
                        Filelist.Add(new CompareBSCW { FilePath = file.FullName, FileName = file.Name });
                    List<CompareFile> FilelistBSCW = new List<CompareFile>();
                    foreach (FileInfo file in new DirectoryInfo(BSCW_Server_Path).GetFiles(searchPattern: "*.*", searchOption: SearchOption.AllDirectories))
                        FilelistBSCW.Add(new CompareFile { FilePath = file.FullName, FileName = file.Name });
                    foreach (CompareBSCW file in Filelist)
                        file.FileExists = (FilelistBSCW.FirstOrDefault(x => x.FileName == file.FileName) != null);
                    ViewManager.Filelist = Filelist;
                    resp.Success = true;
                }
                else
                {
                    resp.Success = false;
                    resp.Message = $"Der Sitzungstag konnte auf dem Laufwerk {UserManager.SenatSettings.BSCW_Server_Drive}:\\ nicht gefunden werden. Bitte legen Sie den Sitzungstag auf dem BSCW-Server ab.";
                    //ViewManager.ShowMainInfoFlyout($"Der Sitzungstag konnte auf dem Laufwerk {UserManager.SenatSettings.BSCW_Server_Drive}:\\ nicht gefunden werden.", false);
                }
                return resp;

            });
            return task;
        }

        #endregion


        //Unterlagen import
        #region UnterlagenImport
        public void OnFileDrop(string[] files, string senderName)
        {
            switch (senderName.ToLower())
            {
                case "card_unterlagen":
                    ImportUnterlagen(files);
                    break;
                case "sitzungsliste":
                case "beratungsliste":
                    ImportList(files, senderName);
                    break;
            }
        }

        private void ImportList(string[] files, string listName)
        {
            try
            {
                foreach (string filePath in files)
                {
                    FileInfo importFile = new FileInfo(filePath);
                    string targetDir = $"{SelectedSitzungstage.FullDirectory}\\_{listName}\\{importFile.Name}";
                    File.Copy(importFile.FullName, targetDir);
                    ViewManager.ShowMainInfoFlyout($"Die {listName} wurde abgelegt.", false);
                    switch (listName)
                    {
                        case "Sitzungsliste":
                            ShowSitzungsliste = true;
                            break;
                        case "Beratungsliste":
                            ShowBeratungsliste = true;
                            break;
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteLog($"Die Datei konnte nicht abgelegt werden. Es ist folgender Fehler aufgetreten: {ex.Message}; {ex.InnerException}");
                ViewManager.ShowMainInfoFlyout($"Die Datei konnte nicht abgelegt werden. Es ist folgender Fehler aufgetreten: {ex.Message}", true);
            }

        }

        private void ImportUnterlagen(string[] files)
        {
            SelectedArtSitzungsunterlagen = 7;
            ImportFilename = files[0];
            ShowSubscribeSet(showUnterlagen: true, showSubscribeUnterlagenAdd: true);
            ShowUnterlagenAZDate = UserManager.SenatSettings.AZPrefixDate;
            SitzungstageTitle = "Dokument importieren";
            SitzungstageSubTitle = "Bitte wählen Sie eine Kategorie aus.";
            ButtonChangeTitle = "Importieren";
            //Daten der ursprünglichen Datei 
            FileInfo file = new FileInfo(ImportFilename);
            TextUnterlagenAdd = file.Name;
            TextFileExtention = file.Extension;
            ImportName = string.Empty;
        }
        private string ImportNameSet(string name)
        {
            string importName = string.Empty;
            if (UserManager.SenatSettings.AZPrefix) importName = $"{SelectedVerfahren.LaufendeNummer}-{SelectedVerfahren.Jahr}";
            if (UserManager.SenatSettings.AZPrefixChar != null) importName += UserManager.SenatSettings.AZPrefixDate;
            importName += name;
            if (UserManager.SenatSettings.AZPrefixDate) ImportName += $"{Gerichtsort} - {Entscheidungsdatum}";
            importName += TextFileExtention;
            return importName;
        }
        private string _ImportFileName;
        public string ImportFilename
        {
            get { return _ImportFileName; }
            set { SetProperty(ref _ImportFileName, value); }
        }
        private string _ImportName;
        public string ImportName
        {
            get { return _ImportName; }
            set { SetProperty(ref _ImportName, value); }
        }
        private bool _ImportAGUrteil;
        public bool ImportAGUrteil
        {
            get { return _ImportAGUrteil; }
            set
            {
                SetProperty(ref _ImportAGUrteil, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportAGUrteil);
            }
        }

        private bool _ImportAGBeschluss;
        public bool ImportAGBeschluss
        {
            get { return _ImportAGBeschluss; }
            set
            {
                SetProperty(ref _ImportAGBeschluss, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportAGBeschluss);
            }
        }
        private bool _ImportLGUrteil;
        public bool ImportLGUrteil
        {
            get { return _ImportLGUrteil; }
            set
            {
                SetProperty(ref _ImportLGUrteil, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportLGUrteil);
            }
        }
        private bool _ImportLGBeschluss;
        public bool ImportLGBeschluss
        {
            get { return _ImportLGBeschluss; }
            set
            {
                SetProperty(ref _ImportLGBeschluss, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportLGBeschluss);
            }
        }
        private bool _ImportLGHB;
        public bool ImportLGHB
        {
            get { return _ImportLGHB; }
            set
            {
                SetProperty(ref _ImportLGHB, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportLGHB);
            }
        }
        private bool _ImportLGZB;
        public bool ImportLGZB
        {
            get { return _ImportLGZB; }
            set
            {
                SetProperty(ref _ImportLGZB, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportLGZB);
            }
        }
        private bool _ImportOLGUrteil;
        public bool ImportOLGUrteil
        {
            get { return _ImportOLGUrteil; }
            set
            {
                SetProperty(ref _ImportOLGUrteil, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportOLGUrteil);
            }
        }
        private bool _ImportOLGBeschluss;
        public bool ImportOLGBeschluss
        {
            get { return _ImportOLGBeschluss; }
            set
            {
                SetProperty(ref _ImportOLGBeschluss, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportOLGBeschluss);
            }
        }
        private bool _ImportOLGHB;
        public bool ImportOLGHB
        {
            get { return _ImportOLGHB; }
            set
            {
                SetProperty(ref _ImportOLGHB, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportOLGHB);
            }
        }
        private bool _ImportOLGZB;
        public bool ImportOLGZB
        {
            get { return _ImportOLGZB; }
            set
            {
                SetProperty(ref _ImportOLGZB, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportOLGZB);
            }
        }
        private bool _ImportVorlageEuGH;
        public bool ImportVorlageEuGH
        {
            get { return _ImportVorlageEuGH; }
            set
            {
                SetProperty(ref _ImportVorlageEuGH, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportEUGHVorlage);
            }
        }
        private bool _ImportUrteilEuGH;
        public bool ImportUrteilEuGH
        {
            get { return _ImportUrteilEuGH; }
            set
            {
                SetProperty(ref _ImportUrteilEuGH, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportEUGHURteil);
            }
        }
        private bool _ImportEntwurf;
        public bool ImportEntwurf
        {
            get { return _ImportEntwurf; }
            set
            {
                SetProperty(ref _ImportEntwurf, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportEntwurf);
            }
        }
        private bool _ImportVotum;
        public bool ImportVotum
        {
            get { return _ImportVotum; }
            set
            {
                SetProperty(ref _ImportVotum, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportVotum);
            }
        }
        private bool _ImportVorvotum;
        public bool ImportVorvotum
        {
            get { return _ImportVorvotum; }
            set
            {
                SetProperty(ref _ImportVorvotum, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportVorVotum);
            }
        }
        private bool _ImportLeitsatz;
        public bool ImportLeitsatz
        {
            get { return _ImportLeitsatz; }
            set
            {
                SetProperty(ref _ImportLeitsatz, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportLeitsatz);
            }
        }
        private bool _ImportRMB;
        public bool ImportRMB
        {
            get { return _ImportRMB; }
            set
            {
                SetProperty(ref _ImportRMB, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportRMB);
            }
        }
        private bool _ImportRME;
        public bool ImportRME
        {
            get { return _ImportRME; }
            set
            {
                SetProperty(ref _ImportRME, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportRME);
            }
        }
        private bool _ImportSonstiges;
        public bool ImportSonstiges
        {
            get { return _ImportSonstiges; }
            set
            {
                SetProperty(ref _ImportSonstiges, value);
                if (value) ImportName = ImportNameSet(UserManager.SenatSettings.ImportSonstiges);
            }
        }
        private bool _ImportFreitext;
        public bool ImportFreitext
        {
            get { return _ImportFreitext; }
            set
            {
                SetProperty(ref _ImportFreitext, value);
                if (value) ImportName = ImportNameSet(Freitext);
            }
        }
        private string _Freitext;
        public string Freitext
        {
            get { return _Freitext; }
            set
            {
                SetProperty(ref _Freitext, value);
                ImportName = ImportNameSet(Freitext);
            }
        }
        private string _Gerichtsort;
        public string Gerichtsort
        {
            get { return _Gerichtsort; }
            set { SetProperty(ref _Gerichtsort, value); }
        }
        private string _Entscheidungsdatum;
        public string Entscheidungsdatum
        {
            get { return _Entscheidungsdatum; }
            set { SetProperty(ref _Entscheidungsdatum, value); }
        }


        #endregion

        //FillFunctions
        #region FillFunctions
        private void VintageListFill(string path)
        {
            List<Vintages> Filllist = new List<Vintages>();
            try
            {
                foreach (DirectoryInfo directory in new DirectoryInfo(path).GetDirectories()) if (int.TryParse(directory.Name, out int _)) Filllist.Add(new Vintages(directory.Name));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Es ist folgender Fehler beim Auslesen der Jahrgänge aufgetreten: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            var OrderedList = Filllist.OrderBy(x => x.Jahr);
            foreach (var item in OrderedList) VintageList.Add(item);
        }
        private Vintages CurrentVintage()
        {
            foreach (Vintages item in VintageList) if (item.Jahr.ToString() == DateTime.Now.Year.ToString()) return item;
            return null;
        }
        private async void SitzungstageListFillAsync()
        {
            Task<DBResponse> task = SitzungstageListFill();
            await task;
            if (task.Result.Success)
            {
                SitzungstageList.Clear();
                if (task.Result.Data != null) foreach (var item in (List<Sitzungstage>)task.Result.Data) SitzungstageList.Add(item);
                if (task.Result.Message != string.Empty) ErrorMessage.CreateSimpleMessage(task.Result.Message);
            }
            else ViewManager.ShowMainInfoFlyout(task.Result.Message, false);
        }
        private Task<DBResponse> SitzungstageListFill()
        {
            Task<DBResponse> task = Task.Run<DBResponse>(() =>
            {
                DBResponse response = new DBResponse();
                List<Sitzungstage> sitzungstageList = new List<Sitzungstage>();
                try
                {
                    List<string> FailedDirectoryList = new List<string>();
                    foreach (FileSystemInfo fileSystemInfo in (IEnumerable<DirectoryInfo>)new DirectoryInfo(pathMainDirectory + SelectedVintage.Jahr + "\\").GetDirectories("*.*", SearchOption.TopDirectoryOnly).OrderBy<DirectoryInfo, string>(f => f.Name))
                    {
                        DBResponse respAdd = SitzungstagAddToList(fileSystemInfo.FullName);
                        if (respAdd.Success)
                        {
                            if (respAdd.Data != null) sitzungstageList.Add((Sitzungstage)respAdd.Data);
                        }
                        else FailedDirectoryList.Add(fileSystemInfo.FullName);
                    }
                    if(FailedDirectoryList.Count > 0)
                    {
                        string message = (FailedDirectoryList.Count == 1) ? 
                            "Folgender Ordner entspricht nicht den Konventionen und wird nicht angezeigt: " :
                            "Folgende Ordner entsprechen nicht den Konventionen und werden nicht angezeigt: ";
                        int counter = 1;
                        foreach (string directory in FailedDirectoryList)
                        {
                            message += directory;
                            if (counter < FailedDirectoryList.Count) message += ", ";
                            counter++;
                        }
                        response.Success = true;
                        response.Message = message;
                    } else response.Success = true;
                }
                catch (Exception ex)
                {
                    response.Message = $"Die Ordner konnten nicht importiert werden. Es ist folgender Fehler aufgetreten: {ex.Message}";
                }
                if (response.Success) response.Data = sitzungstageList;
                return response;
            });
            return task;
        }

        private DBResponse SitzungstagAddToList(string path)
        {
            DBResponse response = new DBResponse();
            try
            {
                Sitzungstage NewSitzungstag = new Sitzungstage(path);
                //var cultureInfo = new CultureInfo("en-US");
                DateTime datSitzung = DateTime.ParseExact(NewSitzungstag.Rohdatum, "yyyy_MM_dd", CultureInfo.InvariantCulture);
                bool Anzeige = !SitzungstagePast || (datSitzung >= DateTime.Today);
                response.Data = (Anzeige) ? NewSitzungstag : null;
                response.Success = true;
            }
            catch (Exception)
            {
                //response-object bleibt bei den Default-Werten
            }
            return response;
        }
        private void VerfahrenListFill(int Filter)
        {
            try
            {
                //Verfahrensliste löschen
                if (VerfahrenList.Count > 0) VerfahrenList.Clear();
                DirectoryInfo directoryInfo = new DirectoryInfo($"{pathMainDirectory}\\{SelectedSitzungstage.Jahr}\\{SelectedSitzungstage.Rohdatum}\\");
                int index = 0;
                //sämtliche Verfahren aus dem Ordner einlesen
                foreach (FileSystemInfo directory in directoryInfo.GetDirectories())
                {
                    if (directory.Name != "_Sitzungsliste" && directory.Name != "_Beratungsliste")
                    {
                        Verfahren NewVerfahren = new Verfahren(directory.FullName);
                        if (Filter == 0)
                            VerfahrenList.Add(NewVerfahren);
                        else
                        {
                            if (NewVerfahren.Spruchgruppe != null)
                            {
                                VerfahrenList.Add(NewVerfahren);
                            }
                        }
                        ++index;
                    }
                }

                //Spruchgruppen für den Filter ermitteln
                bool UnknownSG = false;
                SpruchgruppenList.Clear();
                if (UserManager.SenatSettings.Spruchgruppen != null)
                {

                    List<SenatSpruchgruppe> List = new List<SenatSpruchgruppe>();
                    foreach (var item in VerfahrenList)
                    {

                        string SpruchgruppePlain = (item.Spruchgruppe != null) ? item.Spruchgruppe.SenatSpruchgruppeName : string.Empty;
                        SenatSpruchgruppe SG = UserManager.SenatSettings.Spruchgruppen.FirstOrDefault(x => x.SenatSpruchgruppeName == SpruchgruppePlain);
                        if (SG != null)
                        {

                            if (List.FirstOrDefault(x => x.SenatSpruchgruppeName == SpruchgruppePlain) == null) List.Add(SG);
                        }
                        else
                        {
                            if (!UnknownSG)
                            {
                                List.Add(new SenatSpruchgruppe { SenatSpruchgruppeID = 0, SenatSpruchgruppeName = "Unbekannte Spruchgruppe", SenatSpruchgruppeOrderNumber = 100 });
                                UnknownSG = true;
                            }
                        }
                    }
                    List = List.OrderBy(sg => sg.SenatSpruchgruppeOrderNumber).ToList();
                    foreach (SenatSpruchgruppe SG in List) SpruchgruppenList.Add(SG);
                }

                SortVerfahren();
                ShowVerfahren = (SelectedSitzungstage != null);
                ShowVerfahrenDetails = (VerfahrenList.Count > 0);
            }
            catch (Exception)
            {
                //int num = (int)MessageBox.Show("Die Verfahren konnten nicht ausgelesen werden. Es ist folgender Fehler aufgetreten: " + ex.Message);
            }

        }
        private void SortVerfahren()
        {
            VerfahrenView.Clear();
            switch (SelectedSorting)
            {
                case SortingEnum.Aktenzeichen:
                    foreach (Verfahren v in VerfahrenList.OrderBy(x => x.LaufendeNummerInt).ToList()) VerfahrenView.Add(v);
                    break;
                case SortingEnum.Jahr:
                    foreach (Verfahren v in VerfahrenList.OrderBy(x => x.Jahr).ThenBy(x => x.LaufendeNummerInt).ToList()) VerfahrenView.Add(v);
                    break;
                case SortingEnum.Registerzeichen:
                    foreach (Verfahren v in VerfahrenList.OrderBy(x => x.Registerzeichen.SenatAktenzeichenOrderNumber).ThenBy(x => x.LaufendeNummerInt).ToList()) VerfahrenView.Add(v);
                    break;
                case SortingEnum.Spruchgruppe:
                    foreach (Verfahren v in VerfahrenList.OrderBy(x => x.Spruchgruppe.SenatSpruchgruppeOrderNumber).ToList()) VerfahrenView.Add(v);
                    break;
            }

            ////Verfahren zur Verfahrensview-Liste hinzufügen
            //foreach (Verfahren v in List2) VerfahrenView.Add(v);

        }
        private void UnterlagenListeFill()
        {
            UnterlagenList.Clear();
            try
            {
                if (UnterlagenList.Count > 0) UnterlagenList.Clear();
                DirectoryInfo directoryInfo = new DirectoryInfo($"{pathMainDirectory}{SelectedSitzungstage.Jahr}\\{SelectedSitzungstage.Rohdatum}\\{SelectedVerfahren.Verfahren_Rohinformation}\\ ");

                int num = 0;
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    UnterlagenList.Add(new Unterlagen(file.FullName));
                    ++num;
                }
                if (num == 0) UnterlagenList.Add(new Unterlagen("Es sind keine Unterlagen vorhanden"));
                ShowUnterlagen = UnterlagenList.Count > 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Die Unterlagenliste konnte nicht erstellt werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        #endregion

        private void DirectoryDelete(string path)
        {
            if (Directory.GetDirectories(path).Length < 1)
            {
                Directory.Delete(path, false);
            }
        }

        //SitzungstageManager
        #region Sitzungstagemanager
        private string _SitzungstageTitle;
        public string SitzungstageTitle
        {
            get { return _SitzungstageTitle; }
            set { SetProperty(ref _SitzungstageTitle, value); }
        }
        private string _SitzungstageSubTitle;
        public string SitzungstageSubTitle
        {
            get { return _SitzungstageSubTitle; }
            set { SetProperty(ref _SitzungstageSubTitle, value); }
        }
        private string _ButtonChangeTitle;
        public string ButtonChangeTitle
        {
            get { return _ButtonChangeTitle; }
            set { SetProperty(ref _ButtonChangeTitle, value); }
        }
        private DBResponse Cbo_Senat_Fill(ObservableCollection<Senat> iList)
        {
            //Alle Zivilsenat auswählen
            DBResponse resp = new DBResponse();
            try
            {
                iList.Clear();
                var Query = userDBContext.Senate.Where(x => x.SenatArt == 1).ToList();
                if (Query.Count > 0) foreach (var item in Query) iList.Add(item);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = $"Es ist folgender Fehler beim Füllen der Senate aufgetreten:\n\n {ex.Message}.";
            }
            return resp;
        }

        #endregion




    }
}
