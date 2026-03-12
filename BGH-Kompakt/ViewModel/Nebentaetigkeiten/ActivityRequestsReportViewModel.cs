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
        public ActivityRequestsReportViewModel()
        {
            StartRequestCommand = new RelayCommand(StartRequestExecute);
        }
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
        private int _AnzeigeHoursCount;
        public int AnzeigeHoursCount
        {
            get { return _AnzeigeHoursCount; }
            set { SetProperty(ref _AnzeigeHoursCount, value); }
        }
        private int _GenehmigungHoursCount;
        public int GenehmigungHoursCount
        {
            get { return _GenehmigungHoursCount; }
            set { SetProperty(ref _GenehmigungHoursCount, value); }
        }

        private void StartRequestExecute(object obj)
        {
            if (StartDate != null && EndDate != null)
            {
                DateTime Start = (DateTime)StartDate;
                DateTime End = (DateTime)EndDate;
                List<ActivityRequest> RequestList = new List<ActivityRequest>();
                RequestList = activityRequestDBContext.ActivityRequests.Where(x => x.ARDatum >= Start && x.ARDatum <= End).Include(x => x.ActivityClient).ToList();
                //List<ActivityClient> ClientList = new List<ActivityClient>();
                var query = RequestList.Select(x => x.ActivityClient.ACName).Distinct();
                List<User> UserList = new List<User>();
                AnzeigeCount = RequestList.Where(x => x.ActivityRequestMeldeArtID == 1).Count();
                GenehmigungCount = RequestList.Where(x => x.ActivityRequestMeldeArtID == 2).Count();

                foreach (ActivityRequest request in RequestList) {
                    User user = request.ARUser;
                    if (user != null) UserList.Add(user);
                }
                JudgeCount = UserList.Select(x => x.UserId).Distinct().Count();
                ViewManager.ShowMainInfoFlyout($"Anzahl Requests: {RequestList.Count}; Anzahl Clients: {query.Count()}", false);

            }
        }
    }
}
