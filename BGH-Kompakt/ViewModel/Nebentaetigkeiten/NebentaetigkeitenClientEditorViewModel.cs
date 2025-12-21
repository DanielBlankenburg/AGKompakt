using BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps;
using BGH_Kompakt.Classes.ActivityRequestClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Nebentaetigkeiten
{
    public class NebentaetigkeitenClientEditorViewModel : ViewModelBase
    {
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ListBoxUsedARDblClickCommand { get; set; }

        public List<ActivityClient> ClientList { get; set; }
        public List<ActivityClientTyp> ClientTypList { get; set; }
        private ActivityRequestDBContext activityRequestDBContext = new ActivityRequestDBContext();
        private ActivityClient _SelectedClient;
        public ActivityClient SelectedClient
        {
            get { return _SelectedClient; }
            set
            {
                SetProperty(ref _SelectedClient, value);
                SelectedClientTyp = SelectedClient.ActivityClientTyp;
            }
        }
        private ActivityClientTyp _SelectedClientTyp;
        public ActivityClientTyp SelectedClientTyp
        {
            get { return _SelectedClientTyp; }
            set
            {
                SetProperty(ref _SelectedClientTyp, value);
                SelectedClient.ActivityClientTyp = value;
            }
        }
        private ActivityRequest _SelectedUsedAR;
        public ActivityRequest SelectedUsedAR
        {
            get { return _SelectedUsedAR; }
            set { SetProperty(ref _SelectedUsedAR, value);}
        }

        public NebentaetigkeitenClientEditorViewModel()
        {
            ClientList = activityRequestDBContext.ActivityClients.Include(x => x.ActivityClientTyp).Include(x => x.ActivityRequests).OrderBy(x => x.ACName).ToList();
            ClientTypList = activityRequestDBContext.ActivityClientTyps.ToList();

            SaveCommand = new RelayCommand(SaveExecute, SaveCanExecute);
            DeleteCommand = new RelayCommand(DeleteExecute, DeleteCanExecute);
            ListBoxUsedARDblClickCommand = new RelayCommand(UsedARDblClickExecute);

        }

        private void UsedARDblClickExecute(object obj)
        {
            ViewManager.ShowMainInfoFlyout(SelectedUsedAR.ARTitel, false);
        }

        private bool SaveCanExecute(object obj)
        {
            return SelectedClient != null;
        }

        private bool DeleteCanExecute(object obj)
        {

            return SelectedClient != null && SelectedClient.ActivityRequests.Count == 0;
        }

        private void DeleteExecute(object obj)
        {
            bool answer = ViewManager.ShowQuestionWindow("Soll der Auftraggeber aus der Datenbank gelöscht werden?", "ja");
            if (answer)
            {
                ActivityClient savedClient = activityRequestDBContext.ActivityClients.FirstOrDefault(x => x.ActivityClientId == SelectedClient.ActivityClientId);
                if (savedClient == null)
                {
                    ViewManager.ShowMainInfoFlyout("Der Auftraggeber konnte nicht gelöscht werden, da der Auftraggeber nicht in der Datenbank aufgerufen werden konnte.", false);
                    return;
                }
                activityRequestDBContext.ActivityClients.Remove(savedClient);
                activityRequestDBContext.SaveChanges();
                ClientList.Remove(SelectedClient);
            }
        }

        private void SaveExecute(object obj)
        {
            ActivityClient savedClient = activityRequestDBContext.ActivityClients.FirstOrDefault(x => x.ActivityClientId == SelectedClient.ActivityClientId);
            if (savedClient == null)
            {
                ViewManager.ShowMainInfoFlyout("Die Änderungen konnten nicht gespeichert werden, da der Auftraggeber nicht in der Datenbank aufgerufen werden konnte.", false);
                return;
            }
            savedClient.ACName = SelectedClient.ACName;
            if (savedClient.ActivityClientTyp != SelectedClientTyp)
            {
                ActivityClientTyp add = activityRequestDBContext.ActivityClientTyps.FirstOrDefault(x => x.ActivityClientTypId == SelectedClientTyp.ActivityClientTypId);
                if (add == null)
                {
                    ViewManager.ShowMainInfoFlyout("Die Änderungen konnten nicht gespeichert werden, da der AuftragsgeberTyp nicht in der Datenbank aufgerufen werden konnte.", false);
                    return;
                }
                savedClient.ActivityClientTyp = add;
            }
            activityRequestDBContext.ActivityClients.AddOrUpdate(savedClient);
            activityRequestDBContext.SaveChanges();
            ClientList.Clear();
            ClientList = activityRequestDBContext.ActivityClients.Include(x => x.ActivityClientTyp).Include(x => x.ActivityRequests).OrderBy(x => x.ACName).ToList();
            ViewManager.ShowMainInfoFlyout("Die Änderungen wurden gespeichert.", false);

        }
    }
}
