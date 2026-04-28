using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.ViewModel.Abteilungen.Familienabteilung
{
    public class VerfahrensbeistaendeViewModel : ViewModelBase
    {
        public UserDBContext UserDBContext { get; set; } = new UserDBContext();
        public ObservableCollection<Verfahrensbeistand> VBList { get; set; } = new ObservableCollection<Verfahrensbeistand>();

        private Verfahrensbeistand _SelectedVB;
        public Verfahrensbeistand SelectedVB
        {
            get { return _SelectedVB; }
            set
            {
                SetProperty(ref _SelectedVB, value);
                ViewManager.ShowMainInfoFlyout(_SelectedVB.NachName, false);
            }
        }

        public VerfahrensbeistaendeViewModel()
        {
            var query = UserDBContext.Verfahrensbeistaende.ToArray();
            VBList.Clear();
            foreach (var item in query) VBList.Add(item);
        }

    }
}
