using BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps;
using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.ActivityRequestClasses;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.ActivityRequestService;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using MSWord = Microsoft.Office.Interop.Word;
using Task = System.Threading.Tasks.Task;


namespace BGH_Kompakt.ViewModel.Nebentaetigkeiten
{
    public partial class ActivityRequestsReportViewModel : ViewModelBase
    {
        private readonly ActivityRequestDBContext activityRequestDBContext = new ActivityRequestDBContext();
        private readonly  UserDBContext userDBContext = new UserDBContext();
        private const int MeldeArt_Anzeige = 1;
        private const int MeldeArt_Genehmigung = 2;

        private DateTime? _StartDate;
        public DateTime? StartDate
        {
            get { return _StartDate; }
            set { SetProperty(ref _StartDate, value); }
        }
        private DateTime? _EndDate;
        public DateTime? EndDate
        {
            get { return _EndDate; }
            set { SetProperty(ref _EndDate, value); }
        }
        #region ICommands
        public ICommand StartRequestCommand { get; set; }
        public ICommand CollapseAllCommand { get; set; }
        public ICommand CollapseAllARCommand { get; set; }
        public ICommand ExpandAllCommand { get; set; }
        public ICommand ExpandAllARCommand { get; set; }
        public ICommand CreateReportCommand { get; set; }
        public ICommand CreateTableCommand { get; set; }
        #endregion
        #region Values
        private int _JudgeCount;
        public int JudgeCount
        {
            get { return _JudgeCount; }
            set { SetProperty(ref _JudgeCount, value); }
        }
        private int _JudgeCountGenehmigung;
        public int JudgeCountGenehmigung
        {
            get { return _JudgeCountGenehmigung; }
            set { SetProperty(ref _JudgeCountGenehmigung, value); }
        }
        private int _JudgeCountAnzeige;
        public int JudgeCountAnzeige
        {
            get { return _JudgeCountAnzeige; }
            set { SetProperty(ref _JudgeCountAnzeige, value); }
        }
        private int _AnzeigeCount;
        public int AnzeigeCount
        {
            get { return _AnzeigeCount; }
            set { SetProperty(ref _AnzeigeCount, value); }
        }
        private int _GenehmigungCount;
        public int GenehmigungCount
        {
            get { return _GenehmigungCount; }
            set { SetProperty(ref _GenehmigungCount, value); }
        }

        private int _AnzeigeHoursAverage;
        public int AnzeigeHoursAverage
        {
            get { return _AnzeigeHoursAverage; }
            set { SetProperty(ref _AnzeigeHoursAverage, value); }
        }
        private int _GenehmigungHoursAverage;
        public int GenehmigungHoursAverage
        {
            get { return _GenehmigungHoursAverage; }
            set { SetProperty(ref _GenehmigungHoursAverage, value); }
        }
        private int _AnzeigeHoursMax;
        public int AnzeigeHoursMax
        {
            get { return _AnzeigeHoursMax; }
            set { SetProperty(ref _AnzeigeHoursMax, value); }
        }
        private int _GenehmigungHoursMax;
        public int GenehmigungHoursMax
        {
            get { return _GenehmigungHoursMax; }
            set { SetProperty(ref _GenehmigungHoursMax, value); }
        }

        private int _AnzeigeAmountAverage;
        public int AnzeigeAmountAverage
        {
            get { return _AnzeigeAmountAverage; }
            set { SetProperty(ref _AnzeigeAmountAverage, value); }
        }
        private int _GenehmigungAmountAverage;
        public int GenehmigungAmountAverage
        {
            get { return _GenehmigungAmountAverage; }
            set { SetProperty(ref _GenehmigungAmountAverage, value); }
        }
        private int _AnzeigeAmountMax;
        public int AnzeigeAmountMax
        {
            get { return _AnzeigeAmountMax; }
            set { SetProperty(ref _AnzeigeAmountMax, value); }
        }
        private int _GenehmigungAmountMax;
        public int GenehmigungAmountMax
        {
            get { return _GenehmigungAmountMax; }
            set { SetProperty(ref _GenehmigungAmountMax, value); }
        }

        private int _AnzeigeSingleAmountMax;
        public int AnzeigeSingleAmountMax
        {
            get { return _AnzeigeSingleAmountMax; }
            set { SetProperty(ref _AnzeigeSingleAmountMax, value); }
        }
        private int _GenehmigungSingleAmountMax;
        public int GenehmigungSingleAmountMax
        {
            get { return _GenehmigungSingleAmountMax; }
            set { SetProperty(ref _GenehmigungSingleAmountMax, value); }
        }
        private ARHourAmountProportion _AnzeigeProportion;
        public ARHourAmountProportion AnzeigeProportion
        {
            get { return _AnzeigeProportion; }
            set { SetProperty(ref _AnzeigeProportion, value); }
        }
        private ARHourAmountProportion _GenehmigungProportion;
        public ARHourAmountProportion GenehmigungProportion
        {
            get { return _GenehmigungProportion; }
            set { SetProperty(ref _GenehmigungProportion, value); }
        }
        private ExceedInfo _ExceedR6 = new ExceedInfo();
        public ExceedInfo ExceedR6
        {
            get { return _ExceedR6; }
            set { SetProperty(ref _ExceedR6, value); }
        }
        private ExceedInfo _ExceedR8 = new ExceedInfo();
        public ExceedInfo ExceedR8
        {
            get { return _ExceedR8; }
            set { SetProperty(ref _ExceedR8, value); }
        }
        private ExceedInfo _ExceedR9 = new ExceedInfo();
        public ExceedInfo ExceedR9
        {
            get { return _ExceedR9; }
            set { SetProperty(ref _ExceedR9, value); }
        }
        private ExceedInfo _ExceedR10 = new ExceedInfo();
        public ExceedInfo ExceedR10
        {
            get { return _ExceedR10; }
            set { SetProperty(ref _ExceedR10, value); }
        }
        private List<ClientTypeRequestCount> _RequestsByClient = new List<ClientTypeRequestCount>();
        public List<ClientTypeRequestCount> RequestsByClient
        {
            get { return _RequestsByClient; }
            set { SetProperty(ref _RequestsByClient, value); }
        }

        private List<UserRequestGroup> _RequestsByUser = new List<UserRequestGroup>();
        public List<UserRequestGroup> RequestsByUser
        {
            get { return _RequestsByUser; }
            set { SetProperty(ref _RequestsByUser, value); }
        }
        private List<ActivityRequest> _ReportRequest = new List<ActivityRequest>();
        public List<ActivityRequest> ReportRequest
        {
            get { return _ReportRequest; }
            set { SetProperty(ref _ReportRequest, value); }
        }

        #endregion
        #region Show variables
        private bool _ShowReport = false;
        public bool ShowReport
        {
            get { return _ShowReport; }
            set { SetProperty(ref _ShowReport, value); }
        }
        private bool _IsExpanded = false;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set { SetProperty(ref _IsExpanded, value); }
        }
        private bool _IsExpandedReport1 = true;
        public bool IsExpandedReport1
        {
            get { return _IsExpandedReport1; }
            set { SetProperty(ref _IsExpandedReport1, value); }
        }
        private bool _IsExpandedReport2 = true;
        public bool IsExpandedReport2
        {
            get { return _IsExpandedReport2; }
            set { SetProperty(ref _IsExpandedReport2, value); }
        }
        private bool _IsExpandedReport3 = true;
        public bool IsExpandedReport3
        {
            get { return _IsExpandedReport3; }
            set { SetProperty(ref _IsExpandedReport3, value); }
        }
        private bool _IsExpandedReport4 = true;
        public bool IsExpandedReport4
        {
            get { return _IsExpandedReport4; }
            set { SetProperty(ref _IsExpandedReport4, value); }
        }
        private bool _IsExpandedReport5 = true;
        public bool IsExpandedReport5
        {
            get { return _IsExpandedReport5; }
            set { SetProperty(ref _IsExpandedReport5, value); }
        }
        private bool _IsExpandedReport6 = true;
        public bool IsExpandedReport6
        {
            get { return _IsExpandedReport6; }
            set { SetProperty(ref _IsExpandedReport6, value); }
        }
        private bool _IsExpandedReport7 = true;
        public bool IsExpandedReport7
        {
            get { return _IsExpandedReport7; }
            set { SetProperty(ref _IsExpandedReport7, value); }
        }
        private bool _IsExpandedReport8 = true;
        public bool IsExpandedReport8
        {
            get { return _IsExpandedReport8; }
            set { SetProperty(ref _IsExpandedReport8, value); }
        }
        private bool _IsExpandedReport9 = true;
        public bool IsExpandedReport9
        {
            get { return _IsExpandedReport9; }
            set { SetProperty(ref _IsExpandedReport9, value); }
        }

        #endregion

        public ActivityRequestsReportViewModel()
        {
            StartRequestCommand = new RelayCommand(StartRequestExecute);
            ExpandAllCommand = new RelayCommand(ExpandAllExcute);
            ExpandAllARCommand = new RelayCommand(ExpandARExcute);
            CollapseAllCommand = new RelayCommand(CollapseAllRequestExecute);
            CollapseAllARCommand = new RelayCommand(CollapseARRequestExecute);
            CreateReportCommand = new RelayCommand(CreateReportExecute);
            CreateTableCommand = new RelayCommand(CreateTableExecute);

            StartDate = new DateTime(2025, 1, 1);
            EndDate = DateTime.Now;
        }
        private async void CreateTableExecute(object obj)
        {
            string actionName = "Tabellen erstellen";
            Task<DBResponse> task = CreateTables();
            //ErrorMessage.CreateSimpleMessage("Die Tabellen werden erstellt");
            ViewManager.ActionlistAdd(actionName);
            await task;
            string message = task.Result.Success ? "Die Tabellen wurden erstellt." : task.Result.Message;
            ErrorMessage.CreateSimpleMessage(message);
            ViewManager.ActionlistRemove(actionName);

        }
        private async void CreateReportExecute(object obj)
        {
            string actionName = "Bericht erstellen";
            Task<DBResponse> task = CreateWordDocumnent();
            //ErrorMessage.CreateSimpleMessage("Der Bericht wird erstellt");
            ViewManager.ActionlistAdd(actionName);
            await task;
            string message = task.Result.Success ? "Der Bericht wurde erstellt." : task.Result.Message;
            ErrorMessage.CreateSimpleMessage(message);
            ViewManager.ActionlistRemove(actionName);
        }
        private Task<DBResponse> CreateWordDocumnent()
        {
            Task<DBResponse> task = Task.Run<DBResponse>(() =>
            {
                DBResponse response = new DBResponse();
                MSWord.Application wordApp = new MSWord.Application();
                Document wordDoc = null;

                if (GetWordDocument(ref wordApp, ref wordDoc, "Bericht BMJ.dotx", ref response) == false) return response;

                try
                {
                    foreach (Bookmark bM in wordDoc.Bookmarks)
                    {
                        Range range = null;

                        switch (bM.Name)
                        {
                            case "Anzahl_Richter":
                                range = bM.Range;
                                range.Text = JudgeCount.ToString();
                                break;
                            case "Anzahl_Richter_A":
                                range = bM.Range;
                                range.Text = JudgeCountAnzeige.ToString();
                                break;
                            case "Anzahl_Richter_G":
                                range = bM.Range;
                                range.Text = JudgeCountGenehmigung.ToString();
                                break;
                            //case "Anzahl_Richter_Überschreitung":
                            //    range = bM.Range;
                            //    range.Text = $"{(ActivityRequestManager.SelectedActivityRequest.ActivityRequestMeldeArtID == 2 ? "Genehmigung" : "Anzeige")} einer Nebentätigkeit";
                            //    break;
                            //case "Anzahl_Richter_Überschreitung2":
                            //    range = bM.Range;
                            //    range.Text = $"{ActivityRequestManager.SelectedActivityRequest.ActivityRequestTyp.ActivityRequestTypText} - " +
                            //                    $"{ActivityRequestManager.SelectedActivityRequest.ARTitel} - " +
                            //                    $"{ActivityRequestManager.SelectedActivityRequest.ActivityClient.ACName} - " +
                            //                    $"{(ActivityRequestManager.SelectedActivityRequest.AROrt != null && ActivityRequestManager.SelectedActivityRequest.AROrt != string.Empty ? ActivityRequestManager.SelectedActivityRequest.AROrt + " - " : "")}" +
                            //                    $"{ActivityRequestManager.SelectedActivityRequest.ARActivityDate:d. MMMM yyyy}";
                            //    break;
                            case "Betrag_Durchschnitt_A":
                                range = bM.Range;
                                range.Text = AnzeigeAmountAverage.ToString();
                                break;
                            case "Betrag_Durchschnitt_G":
                                range = bM.Range;
                                range.Text = GenehmigungAmountAverage.ToString("F2");
                                break;
                            case "Betrag_Max_A":
                                range = bM.Range;
                                range.Text = AnzeigeSingleAmountMax.ToString("F2");
                                break;
                            case "Betrag_Max_G":
                                range = bM.Range;
                                range.Text = GenehmigungSingleAmountMax.ToString("F2");
                                break;
                            case "Betrag_Relation_A":
                                range = bM.Range;
                                range.Text = AnzeigeProportion != null ? AnzeigeProportion.Proportion.ToString("F2") : "0";
                                break;
                            case "Betrag_Relation_G":
                                range = bM.Range;
                                range.Text = GenehmigungProportion != null ? GenehmigungProportion.Proportion.ToString("F2") : "0";
                                //range.Text = GenehmigungProportion.Proportion.ToString("F2");
                                break;
                            case "Betrag_Richter_Max_A":
                                range = bM.Range;
                                range.Text = AnzeigeSingleAmountMax.ToString("F2");
                                break;
                            case "Betrag_Richter_Max_G":
                                range = bM.Range;
                                range.Text = GenehmigungSingleAmountMax.ToString("F2");
                                break;
                            case "Jahr":
                                range = bM.Range;
                                range.Text = DateTime.Parse(StartDate.ToString()).Year.ToString();
                                break;
                            case "Jahr2":
                                range = bM.Range;
                                range.Text = DateTime.Parse(StartDate.ToString()).Year.ToString();
                                break;
                            case "Stunden_Durchschnitt_A":
                                range = bM.Range;
                                range.Text = AnzeigeHoursAverage.ToString();
                                break;
                            case "Stunden_Durchschnitt_G":
                                range = bM.Range;
                                range.Text = GenehmigungHoursAverage.ToString();
                                break;
                            case "Stunden_Max_A":
                                range = bM.Range;
                                range.Text = AnzeigeHoursMax.ToString();
                                break;
                            case "Stunden_Max_G":
                                range = bM.Range;
                                range.Text = GenehmigungHoursMax.ToString();
                                break;
                        }
                    }

                    DateTime date = new DateTime();
                    date = DateTime.Now;
                    string docName = $"BerichtBMJV_{date:d}.docx";
                    string dirTemp = $"{BGHKompaktSystemInfo.PathTempARDOC}";
                    if (Directory.Exists(dirTemp)) Directory.Delete(dirTemp, true);
                    Directory.CreateDirectory(dirTemp);
                    wordDoc.SaveAs2($"{dirTemp}{docName}");
                    wordApp.Visible = true;
                    response.Success = true;
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = $"Der Bericht konnte nicht erstellt werden. Es ist folgender Fehler aufgetreten: {ex.Message}";
                }
                finally
                {
                    if (response.Success == false)
                    {
                        wordDoc?.Close(WdSaveOptions.wdDoNotSaveChanges);
                        wordApp.Quit();
                    }
                }
                return response;
            });
            return task;
        }
        private bool GetWordDocument(ref Application wordApp, ref Document wordDoc, string fileName, ref DBResponse response)
        {
            string[] directories = Assembly.GetExecutingAssembly().Location.Split('\\');
            string pathApp = string.Empty;
            for (int i = 0; i < directories.Length - 1; i++) pathApp += directories[i] + "\\";
            string DocDir = $"{pathApp}Documents\\";
            object oTemplate = $"{DocDir}\\{fileName}";

            try
            {
                if (!File.Exists(oTemplate.ToString()))
                {
                    //MessageBox.Show(oTemplate.ToString());
                    response.Success = false;
                    response.Message = $"Die Vorlage \"{fileName}\" wurde im Pfad {DocDir} nicht gefunden";
                    return false;
                }
                wordDoc = wordApp.Documents.Add(oTemplate);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Die Vorlage \"{fileName}\" im Pfad {DocDir} konnte nicht geöffnet werden. Es ist folgender Fehler aufgetreten: {ex.Message}";
                return false;
            }
            return true;
        }
        private void CollapseARRequestExecute(object obj) => IsExpanded = false;
        private void ExpandARExcute(object obj) => IsExpanded = true;
        private void CollapseAllRequestExecute(object obj)
        {
            IsExpandedReport1 = false;
            IsExpandedReport2 = false;
            IsExpandedReport3 = false;
            IsExpandedReport4 = false;
            IsExpandedReport5 = false;
            IsExpandedReport6 = false;
            IsExpandedReport7 = false;
            IsExpandedReport8 = false;
            IsExpandedReport9 = false;
        }
        private void ExpandAllExcute(object obj)
        {
            IsExpandedReport1 = true;
            IsExpandedReport2 = true;
            IsExpandedReport3 = true;
            IsExpandedReport4 = true;
            IsExpandedReport5 = true;
            IsExpandedReport6 = true;
            IsExpandedReport7 = true;
            IsExpandedReport8 = true;
            IsExpandedReport9 = true;
        }
        private async void StartRequestExecute(object obj)
        {
            string actionName = "Berichtsdaten ermitteln";
            Task<DBResponse> task = CreateReport();
            ErrorMessage.CreateSimpleMessage("Die Berichtsdaten werden ermittelt");
            ViewManager.ActionlistAdd(actionName);
            await task;
            if (!task.Result.Success) ErrorMessage.CreateSimpleMessage(task.Result.Message);
            ViewManager.ActionlistRemove(actionName);
        }
        private Task<DBResponse> CreateReport()
        {
            Task<DBResponse> task = Task.Run<DBResponse>(() => {

                DBResponse response = new DBResponse();
                try
                {
                    DBResponse dateValidationResponse = ValidateDates(out var start, out var end);
                    if (!dateValidationResponse.Success)
                    {
                        response.Message = dateValidationResponse.Message;
                        return response;
                    }

                    ReportRequest = LoadRequests(start, end);

                    if (ReportRequest == null || ReportRequest.Count == 0)
                    {
                        response.Message = "Für den Zeitraum konnten keine Nebentätigkeiten gefunden werden";
                        return response;
                    }

                    ComputeCounts(ReportRequest);
                    ComputeAggregates(ReportRequest);

                    RequestsByClient = BuildRequestsByClient(ReportRequest);
                    RequestsByUser = BuildRequestsByUser(ReportRequest, null);

                    AnzeigeAmountMax = BuildRequestsByUser(ReportRequest, MeldeArt_Anzeige).DefaultIfEmpty().Max(x => x?.TotalAmount ?? 0);
                    GenehmigungAmountMax = BuildRequestsByUser(ReportRequest, MeldeArt_Genehmigung).DefaultIfEmpty().Max(x => x?.TotalAmount ?? 0);
                    CountExeeds(ReportRequest);

                    ShowReport = true;
                    response.Success = true;
                }
                catch (Exception ex)
                {
                    response.Message = $"Es ist ein Fehler bei der Erstellung des Berichts aufgetreten. Bitte prüfe Sie die Log-Dateien.";
                    ErrorMessage.CreateExceptionWithoutMessage("StartRequestExecute", ex);
                }
                return response;
            }
            );
            return task;
        }
        private void CountExeeds(List<ActivityRequest> requests)
        {
            List<PaymentLimit> limits = SetPaymentLimits();
            List<UserRequestGroup> allGroups = BuildRequestsByUser(requests, null);

            ExceedR6.Count = 0;
            ExceedR8.Count = 0;
            ExceedR9.Count = 0;
            ExceedR10.Count = 0;

            foreach (UserRequestGroup User in allGroups)
            {
                UserDienstbezeichnung userDienstbezeichnung = userDBContext.UserDienstbezeichnungen
                    .Where(u => u.UserId == User.UserId && u.GültigAb <= StartDate)
                    .FirstOrDefault();
                if (userDienstbezeichnung != null)
                {
                    Dienstbezeichnung dienstbezeichnung = userDBContext.Dienstbezeichnungen.Where(d => d.DienstbezeichnungId == userDienstbezeichnung.DienstbezeichnungId).FirstOrDefault();
                    if (dienstbezeichnung != null)
                    {
                        PaymentLimit limit = limits.Where(l => l.Id == dienstbezeichnung.Besoldungsgruppe.id).FirstOrDefault();
                        if (limit != null)
                        {
                            if (User.TotalAmount > limit.Limit)
                            {
                                switch (limit.Besoldungsgruppe)
                                {
                                    case "R6":
                                        ExceedR6.Count++;
                                        ExceedR6.Limit = limit.Limit;
                                        break;
                                    case "R8":
                                        ExceedR8.Count++;
                                        ExceedR8.Limit = limit.Limit;
                                        break;
                                    case "R9":
                                        ExceedR9.Count++;
                                        ExceedR9.Limit = limit.Limit;
                                        break;
                                    case "R10":
                                        ExceedR10.Count++;
                                        ExceedR10.Limit = limit.Limit;
                                        break;
                                }
                            }
                        }
                    }

                }
            }
        }
        private List<PaymentLimit> SetPaymentLimits()
        {
            List<PaymentLimit> limits = new List<PaymentLimit>();
            for (int i = 6; i <= 10; i++)
            {
                if (i != 7)
                {
                    try
                    {
                        string R = $"R{i}";
                        var payment = userDBContext.RBesoldungPayments
                        .Where(p => p.RBesoldung.Name == R && p.Start <= StartDate)
                        .Include(x => x.RBesoldung)
                        .OrderByDescending(p => p.Start)
                        .FirstOrDefault();
                        if (payment != null)
                        {
                            decimal limit = ((payment.PaymentValue * 12)/ 100) * 40;
                            limits.Add(new PaymentLimit { Id = payment.RBesoldung.id, Besoldungsgruppe = $"R{i}", Limit = limit });
                        }
                    }
                    catch (Exception ex) { ErrorMessage.CreateExceptionWithoutMessage("SetPaymentLimits", ex); }
                }
            }
            return limits;
        }
        private DBResponse ValidateDates(out DateTime start, out DateTime end)
        {
            DBResponse response = new DBResponse();
            start = default;
            end = default;

            if (StartDate == null || EndDate == null)
            {
                response.Message = "Bitte wählen Sie ein Start und Enddatum aus.";
                return response;
            }

            start = StartDate.Value;
            end = EndDate.Value;

            if (start >= end)
            {
                response.Message = "Das Startdatum muss kleiner als das Enddatum sein.";
                return response;
            }
            response.Success = true;
            return response;
        }
        private List<ActivityRequest> LoadRequests(DateTime start, DateTime end)
        {
            var query = activityRequestDBContext.ActivityRequests
                .Where(x => x.ARDatum >= start && x.ARDatum <= end && x.ARZustaendigkeitsbereich > 1)
                .Include(x => x.ActivityClient)
                .Include(x => x.ActivityClient.ActivityClientTyp)
                .ToList();
            List<ActivityRequest> judgeList = new List<ActivityRequest>();
            foreach (ActivityRequest request in query)
            {
                if (request.ARUser.PositionId == 1) judgeList.Add(request);
            }
            return judgeList;

        }
        private void ComputeCounts(List<ActivityRequest> requests)
        {
            AnzeigeCount = requests.Count(x => x.ActivityRequestMeldeArtID == MeldeArt_Anzeige);
            GenehmigungCount = requests.Count(x => x.ActivityRequestMeldeArtID == MeldeArt_Genehmigung);

            JudgeCountAnzeige = requests
               .Where(r => r.ARUser != null && r.ActivityRequestMeldeArtID == MeldeArt_Anzeige)
               .Select(r => r.ARUser.UserId)
               .Distinct()
               .Count();

            JudgeCountGenehmigung = requests
                .Where(r => r.ARUser != null && r.ActivityRequestMeldeArtID == MeldeArt_Genehmigung)
                .Select(r => r.ARUser.UserId)
                .Distinct()
                .Count();

            var users = requests
                .Where(r => r.ARUser != null)
                .Select(r => r.ARUser.UserId)
                .Distinct()
                .Count();

            JudgeCount = users;
        }
        private void ComputeAggregates(List<ActivityRequest> requests)
        {
            var anzeigen = requests.Where(x => x.ActivityRequestMeldeArtID == MeldeArt_Anzeige).ToList();
            var genehmigungen = requests.Where(x => x.ActivityRequestMeldeArtID == MeldeArt_Genehmigung).ToList();

            AnzeigeHoursMax = anzeigen.Count > 0 ? (int)anzeigen.Max(x => x.Gesamtzeitaufwand) : 0;
            GenehmigungHoursMax = genehmigungen.Count > 0 ? (int)genehmigungen.Max(x => x.Gesamtzeitaufwand) : 0;
            AnzeigeHoursAverage = anzeigen.Count > 0 ? (int)anzeigen.Average(x => x.Gesamtzeitaufwand) : 0;
            GenehmigungHoursAverage = genehmigungen.Count > 0 ? (int)genehmigungen.Average(x => x.Gesamtzeitaufwand) : 0;

            AnzeigeAmountAverage = anzeigen.Count > 0 ? (int)anzeigen.Average(x => x.Gesamtverguetung) : 0;
            GenehmigungAmountAverage = genehmigungen.Count > 0 ? (int)genehmigungen.Average(x => x.Gesamtverguetung) : 0;
            AnzeigeSingleAmountMax = anzeigen.Count > 0 ? (int)anzeigen.Max(x => x.Gesamtverguetung) : 0;
            GenehmigungSingleAmountMax = genehmigungen.Count > 0 ? (int)genehmigungen.Max(x => x.Gesamtverguetung) : 0;

            AnzeigeProportion = anzeigen.Count > 0 ? FindMaxProportion(anzeigen) : null;
            GenehmigungProportion = genehmigungen.Count > 0 ? FindMaxProportion(genehmigungen): null;
        }
        private ARHourAmountProportion FindMaxProportion(List<ActivityRequest> list)
        {
            var query = list.OrderByDescending(x => x.HourAmountProportion.Proportion).First();
            return query.HourAmountProportion;
        }
        private List<ClientTypeRequestCount> BuildRequestsByClient(List<ActivityRequest> requests)
        {
            var groupedByClientType = requests
                .GroupBy(r => r.ActivityClient?.ActivityClientTyp?.ActivityClientTypText ?? "Unbekannt")
                .Select(g => new ClientTypeRequestCount
                {
                    TypeName = g.Key,
                    Clients = g
                        .GroupBy(r2 => r2.ActivityClient?.ACName ?? "Unbekannt")
                        .Select(gc => new ClientRequestCount { ClientName = gc.Key, Count = gc.Count() })
                        .OrderByDescending(c => c.Count)
                        .ToList()
                })
                .OrderByDescending(ct => ct.Clients.Sum(c => c.Count))
                .ToList();

            return groupedByClientType;
        }
        private List<UserRequestGroup> BuildRequestsByUser(List<ActivityRequest> requests, int? meldeArtId)
        {
            var filtered = meldeArtId.HasValue
                ? requests.Where(r => r.ARUser != null && r.ActivityRequestMeldeArtID == meldeArtId.Value)
                : requests.Where(r => r.ARUser != null);

            var groupedUsers = filtered
                .GroupBy(r => new { r.ARUser.UserId, Name = r.ARUser.Fullname })
                .Select(g => new UserRequestGroup
                {
                    UserId = g.Key.UserId,
                    UserName = g.Key.Name,
                    RequestCount = g.Count(),
                    Requests = g.OrderByDescending(r => r.ARDatum).ToList()
                })
                .OrderByDescending(ug => ug.RequestCount)
                .ToList();

            return groupedUsers;
        }
        public Task<DBResponse> CreateTables()
        {
            Task<DBResponse> task = Task.Run<DBResponse>(() =>
            {
                DBResponse response = new DBResponse();
                MSWord.Application wordApp = new MSWord.Application();
                Document wordDoc = null;

                string dirTemp = $"{BGHKompaktSystemInfo.PathTempARDOC}";
                if (Directory.Exists(dirTemp)) Directory.Delete(dirTemp, true);

                if (CreateTableByType(ref wordApp, ref wordDoc, 1, ref response, dirTemp) == false) return response;
                CreateTableByType(ref wordApp, ref wordDoc, 2, ref response, dirTemp);
                return response;
            });
            return task;
        }
        private bool CreateTableByType(ref Application wordApp, ref Document wordDoc, int art, ref DBResponse response, string dirTemp)
        {
            if (GetWordDocument(ref wordApp, ref wordDoc, "Bericht BMJ - Anlage Nebentätigkeiten.dotx", ref response) == false) return false;

            try
            {
                foreach (Bookmark bM in wordDoc.Bookmarks)
                {

                    switch (bM.Name)
                    {
                        case "Date":
                            Range rangeDate = bM.Range;
                            rangeDate.Text = DateTime.Now.ToString("d");
                            break;
                        case "RequestType":
                            Range rangeRequestType = bM.Range;
                            rangeRequestType.Text = art == 1 ? "Anzeigepflichtige" : "Genehmigungspflichtige";
                            break;
                        case "Year":
                            Range rangeYear = bM.Range;
                            rangeYear.Text = DateTime.Parse(StartDate.ToString()).Year.ToString();
                            break;
                        case "Table":
                            dynamic rangeTable = bM.Range;
                            CreateTabeleContent(ref rangeTable, wordDoc, art);
                            break;
                    }
                }
                DateTime date = new DateTime();
                date = DateTime.Now;
                string docName = $"Bericht BMJ - Anlage Nebentätigkeiten - {(art == 1 ? "Anzeigepflichtig" : "Genehmiungspflichtig")}_{date:d}.docx";
                //if (Directory.Exists(dirTemp)) Directory.Delete(dirTemp, true);
                Directory.CreateDirectory(dirTemp);
                wordDoc.SaveAs2($"{dirTemp}{docName}");
                wordApp.Visible = true;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Der Bericht konnte nicht erstellt werden. Es ist folgender Fehler aufgetreten: {ex.Message}";
            }
            finally
            {
                if (response.Success == false)
                {
                    wordDoc?.Close(WdSaveOptions.wdDoNotSaveChanges);
                    wordApp.Quit();
                }
            }
            return true;
        }
        private void CreateTabeleContent(ref dynamic rangeTable, Document wordDoc, int art)
        {
            int RowCount = 0;
            int ColumnCount = 2;

            List<int> headlines = new List<int>();
            string tableExportString = "";

            List<ActivityRequest> requests = ReportRequest.Where(r => r.ActivityRequestMeldeArtID == art).ToList();
            if (requests == null || requests.Count == 0)
            {
                rangeTable.Text = "Es sind keine Einträge für diesen Nebentätigkeitstyp vorhanden.";
                return;
            }
            List<ClientTypeRequestCount> clients = new List<ClientTypeRequestCount>();
            clients = BuildRequestsByClient(requests);

            foreach (ClientTypeRequestCount item in clients)
            {
                tableExportString += $"{item.TypeName}\t{item.ClientCount}\t";
                headlines.Add(RowCount);
                RowCount++;
                foreach (ClientRequestCount client in item.Clients)
                {
                    tableExportString += $"{client.ClientName}\t{client.Count}\t";
                    RowCount++;
                }
            }

            rangeTable.Text = tableExportString;

            object Separator = MSWord.WdTableFieldSeparator.wdSeparateByTabs;
            object Format = MSWord.WdTableFormat.wdTableFormatWeb1;
            object ApplyBorders = false;
            object AutoFit = true;
            object ApplyHeadingRows = true;

            object DefaultTableBehavior = MSWord.WdDefaultTableBehavior.wdWord9TableBehavior;
            object AutoFitBehavior = MSWord.WdAutoFitBehavior.wdAutoFitContent;
            rangeTable.ConvertToTable(ref Separator,
                ref RowCount, ref ColumnCount, Type.Missing, ref Format,
                ref ApplyBorders, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, ref AutoFit, ref AutoFitBehavior,
                    ref DefaultTableBehavior);

            rangeTable.Select();
            wordDoc.Application.Selection.Tables[1].Rows[1].Select();
            wordDoc.Application.Selection.Tables[1].Columns[1].Width = 450;
            wordDoc.Application.Selection.Tables[1].Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            wordDoc.Application.Selection.Tables[1].Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;

            foreach (int item in headlines)
            {
                for (int c = 1; c <= 2; c++)
                {
                    wordDoc.Application.Selection.Tables[1].Cell(item + 1, c).Range.Font.Color = WdColor.wdColorWhite;
                    wordDoc.Application.Selection.Tables[1].Cell(item + 1, c).Range.Shading.BackgroundPatternColor = WdColor.wdColorGray40;
                }
            }
        }
    }
    public class ClientRequestCount
    {
        public string ClientName { get; set; } = string.Empty;
        public int Count { get; set; }
    }
    public class ClientTypeRequestCount
    {
        public string TypeName { get; set; } = string.Empty;
        public List<ClientRequestCount> Clients { get; set; } = new List<ClientRequestCount>();
        public int ClientCount => Clients?.Count ?? 0;
        public int TotalRequests => Clients?.Sum(c => c.Count) ?? 0;
    }
    public class UserRequestGroup
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int RequestCount { get; set; }
        public List<ActivityRequest> Requests { get; set; } = new List<ActivityRequest>();

        public int TotalHours => Requests?.Where(r => r.Gesamtzeitaufwand != null).Sum(r => (int)r.Gesamtzeitaufwand) ?? 0;
        public int TotalAmount => Requests?.Where(r => r.Gesamtverguetung != null).Sum(r => (int)r.Gesamtverguetung) ?? 0;
    }
    public class PaymentLimit
    {
        public int Id { get; set; }    
        public string Besoldungsgruppe { get; set; }
        public decimal Limit { get; set; }
    }
    public class ExceedInfo : ViewModelBase
    {
        private int _Count;
        public int Count
        {
            get { return _Count; }
            set { SetProperty(ref _Count, value); }
        }

        private decimal _Limit;
        public decimal Limit
        {
            get { return _Limit; }
            set { SetProperty(ref _Limit, value); }
        }
    }

}
