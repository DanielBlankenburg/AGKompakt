using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Montagspost
{
    public partial class MontagsPostEditorViewModel : ViewModelBase
    {
        private readonly MPDBContext mPDBContext = new MPDBContext();

        public ObservableCollection<MPWeek> MPWeekList { get; set; } = new ObservableCollection<MPWeek>();
        private readonly ObservableCollection<int> _VintageList = new ObservableCollection<int>();
        public ObservableCollection<int> VintageList { get { return _VintageList; } }
        public ObservableCollection<MPDecision> MPDecisionList { get; set; } = new ObservableCollection<MPDecision>();
        public ObservableCollection<MPSenat> SenatList { get; set; } = new ObservableCollection<MPSenat>();

        public ICommand SaveCommand { get; set; }


        private int _SelectedVintage;
        public int SelectedVintage
        {
            get { return _SelectedVintage; }
            set
            {
                SetProperty<int>(ref _SelectedVintage, value);
                WeekFill();
            }
        }
        private MPWeek _SelectedWeek;
        public MPWeek SelectedWeek
        {
            get { return _SelectedWeek; }
            set
            {
                SetProperty(ref _SelectedWeek, value);
                DecsionListFill();
            }
        }
        private MPDecision _SelectedDecision;
        public MPDecision SelecteDecision
        {
            get { return _SelectedDecision; }
            set
            {
                SetProperty(ref _SelectedDecision, value);
                EditDecision = _SelectedDecision;
            }
        }

        private MPDecision _EditDecision;
        public MPDecision EditDecision
        {
            get { return _EditDecision; }
            set { SetProperty(ref _EditDecision, value);}
        }
        private MPSenat _SelectSenat;
        public MPSenat SelectSenat
        {
            get { return _SelectSenat; }
            set { SetProperty(ref _SelectSenat, value);}
        }


        public MontagsPostEditorViewModel()
        {
            SaveCommand = new RelayCommand(SaveExecute);

            Logger.WriteLog("MontagsPostEditor opened");
            try
            {
                var MPVintages_Query = mPDBContext.MPWeeks.Select(x => x.MPWeekYear).Distinct();
                foreach (var Vintage in MPVintages_Query) VintageList.Add(Vintage);
                if (VintageList.Count > 0) SelectedVintage = VintageList.LastOrDefault();
                var MPSenat_Query = mPDBContext.MPSenate.OrderBy(x => x.MPCategorieID).ThenBy(x => x.MPSenatSorting);
                foreach (var senat in MPSenat_Query) SenatList.Add(senat);
            }
            catch (Exception ex) { ErrorMessage.CreateExceptionWithFlyOutMessage("MontagsPostEditorViewModel-Constructor", ex);}
        }

        private void SaveExecute(object obj)
        {
            try
            {
                MPDecision mPDecision = mPDBContext.MPDecisions.Find(EditDecision.MPDecisionID);
                if (mPDecision != null)
                {
                    mPDecision.UpdateDecision(EditDecision);
                    mPDBContext.SaveChanges();
                    ViewManager.ShowMainInfoFlyout("Die Änderungen wurden gespeichert.", false);
                    Logger.WriteLog($"MontagsPost Decision ID:{EditDecision.MPDecisionID} gespeichert.");
                }
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout("Fehler beim Speichern der Änderungen der Montagspost.", false);
                Logger.WriteLog($"Fehler beim Speichern der Aenderungen der Montagspost: {ex.Message}, {ex.InnerException}");
            }

            //MessageBox.Show($"Datum: {EditDecision.Date}; ID: {EditDecision.MPDecisionID}");
        }

        private void WeekFill()
        {
            MPWeekList.Clear();
            try
            {
                if (SelectedVintage > 0)
                {
                    var MPWeek_Query = mPDBContext.MPWeeks.Include(x => x.MPDecisions).Include("MPDecisions.Senat").Where(x => x.MPWeekYear == SelectedVintage).OrderByDescending(x => x.MPWeekNumber);
                    foreach (var item in MPWeek_Query) MPWeekList.Add(item);
                }
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout("Fehler beim Füllen der Kalenderwoche. Bitte überprüfen die Log-Files.", false);
                Logger.WriteLog($"Fehler beim Fuellen der Kalenderwoche: {ex.Message}, {ex.InnerException}");
            }
        }
        private void DecsionListFill()
        {
            try
            {
                MPDecisionList.Clear();
                var query = SelectedWeek.MPDecisions.OrderBy(x => x.Senat.MPCategorieID).ThenBy(x => x.Senat.MPSenatSorting);
                if (query != null)
                {
                    foreach (MPDecision decision in query) MPDecisionList.Add(decision);
                }
                else
                {
                    ErrorMessage.CreateSimpleMessage("Die Kalenderwochen konnten nicht eingelesen werden.");
                }
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout("Fehler beim Füllen der Entscheidungen. Bitte überprüfen die Log-Files.", false);
                Logger.WriteLog($"Fehler beim Fuellen der Entscheidungen: {ex.Message}, {ex.InnerException}");
            }
        }



    }
}
