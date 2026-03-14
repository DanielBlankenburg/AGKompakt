using BGH_Kompakt.Classes.ActivityRequestClasses;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Nebentaetigkeiten
{
    public partial class ActivityRequestsReportViewModel : ViewModelBase
    {
        private ActivityRequestDBContext activityRequestDBContext = new ActivityRequestDBContext();
        private DateTime? _StartDate;
        public DateTime? StartDate
        {
            get { return _StartDate; }
            set {SetProperty(ref _StartDate, value);}
        }
        private DateTime? _EndDate;
        public DateTime? EndDate
        {
            get { return _EndDate; }
            set {SetProperty(ref _EndDate, value);}
        }
        public ICommand StartRequestCommand { get; set; }
        private int _JudgeCount;
        public int JudgeCount
        {
            get { return _JudgeCount; }
            set { SetProperty(ref _JudgeCount, value); }
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

        private bool _ShowReport = false;
        public bool ShowReport
        {
            get { return _ShowReport; }
            set { SetProperty(ref _ShowReport, value); }
        }

        public ActivityRequestsReportViewModel()
        {
            StartRequestCommand = new RelayCommand(StartRequestExecute);
            StartDate = new DateTime(2025, 1, 1);
            EndDate = DateTime.Now;
        }

        private void StartRequestExecute(object obj)
        {
            if (StartDate != null && EndDate != null)
            {
                DateTime Start = (DateTime)StartDate;
                DateTime End = (DateTime)EndDate;
                List<ActivityRequest> RequestList = new List<ActivityRequest>();
                RequestList = activityRequestDBContext.ActivityRequests
                    .Where(x => x.ARDatum >= Start && x.ARDatum <= End)
                    .Include(x => x.ActivityClient)
                    .Include(x => x.ActivityClient.ActivityClientTyp)
                    .ToList();

                List<ActivityRequest> RequestListAnzeige = new List<ActivityRequest>();
                RequestListAnzeige = RequestList.Where(x => x.ActivityRequestMeldeArtID == 1).ToList();
                List<ActivityRequest> RequestListGenehmigung = new List<ActivityRequest>();
                RequestListGenehmigung = RequestList.Where(x => x.ActivityRequestMeldeArtID == 2).ToList();
                var query = RequestList.Select(x => x.ActivityClient?.ACName).Distinct();

                //Cout
                List<User> UserList = new List<User>();
                AnzeigeCount = RequestList.Where(x => x.ActivityRequestMeldeArtID == 1).Count();
                GenehmigungCount = RequestList.Where(x => x.ActivityRequestMeldeArtID == 2).Count();

                foreach (ActivityRequest request in RequestList)
                {
                    User user = request.ARUser;
                    if (user != null) UserList.Add(user);
                }
                JudgeCount = UserList.Select(x => x.UserId).Distinct().Count();

                //Value
                AnzeigeHoursMax = RequestListAnzeige.Count > 0 ? (int)RequestListAnzeige.Max(x => x.Gesamtzeitaufwand) : 0;
                GenehmigungHoursMax = RequestListGenehmigung.Count > 0 ? (int)RequestListGenehmigung.Max(x => x.Gesamtzeitaufwand) : 0;
                AnzeigeHoursAverage = RequestListAnzeige.Count > 0 ? (int)RequestListAnzeige.Average(x => x.Gesamtzeitaufwand) : 0;
                GenehmigungHoursAverage = RequestListGenehmigung.Count > 0 ? (int)RequestListGenehmigung.Average(x => x.Gesamtzeitaufwand) : 0;

                AnzeigeAmountMax = RequestListAnzeige.Count > 0 ? (int)RequestListAnzeige.Max(x => x.Gesamtverguetung) : 0;
                GenehmigungAmountMax = RequestListGenehmigung.Count > 0 ? (int)RequestListGenehmigung.Max(x => x.Gesamtverguetung) : 0;
                AnzeigeAmountAverage = RequestListAnzeige.Count > 0 ? (int)RequestListAnzeige.Average(x => x.Gesamtverguetung) : 0;
                GenehmigungAmountAverage = RequestListGenehmigung.Count > 0 ? (int)RequestListGenehmigung.Average(x => x.Gesamtverguetung) : 0;
                AnzeigeSingleAmountMax = RequestListAnzeige.Count > 0 ? (int)RequestListAnzeige.Max(x => x.Gesamtverguetung) : 0;
                GenehmigungSingleAmountMax = RequestListGenehmigung.Count > 0 ? (int)RequestListGenehmigung.Max(x => x.Gesamtverguetung) : 0;

                var groupedByClientType = RequestList
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

                RequestsByClient = groupedByClientType;

                // New: group by user and keep the full list of ActivityRequest items per user
                var groupedUsers = RequestList
                    .Where(r => r.ARUser != null)
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

                //// include requests without a user under "Unbekannt"
                //var unknownUserRequests = RequestList.Where(r => r.ARUser == null).OrderByDescending(r => r.ARDatum).ToList();
                //if (unknownUserRequests.Any())
                //{
                //    groupedUsers.Add(new UserRequestGroup
                //    {
                //        UserId = 0,
                //        UserName = "Unbekannt",
                //        RequestCount = unknownUserRequests.Count,
                //        Requests = unknownUserRequests
                //    });
                //}

                RequestsByUser = groupedUsers;

                ShowReport = true;
                ViewManager.ShowMainInfoFlyout($"Anzahl Requests: {RequestList.Count}; Anzahl Clients: {query.Count()}", false);
            }
        }

}

    // Small DTO for grouping result per client
    public class ClientRequestCount
    {
        public string ClientName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    // DTO for grouping by client type and containing clients with counts
    public class ClientTypeRequestCount
    {
        public string TypeName { get; set; } = string.Empty;
        public List<ClientRequestCount> Clients { get; set; } = new List<ClientRequestCount>();
        public int ClientCount => Clients?.Count ?? 0;
        public int TotalRequests => Clients?.Sum(c => c.Count) ?? 0;
    }

    // New DTO: groups activity requests per user and contains the full ActivityRequest list for details
    public class UserRequestGroup
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int RequestCount { get; set; }
        public List<ActivityRequest> Requests { get; set; } = new List<ActivityRequest>();

        // convenience properties
        public int TotalHours => Requests?.Where(r => r.Gesamtzeitaufwand != null).Sum(r => (int)r.Gesamtzeitaufwand) ?? 0;
        public int TotalAmount => Requests?.Where(r => r.Gesamtverguetung != null).Sum(r => (int)r.Gesamtverguetung) ?? 0;
    }
}
