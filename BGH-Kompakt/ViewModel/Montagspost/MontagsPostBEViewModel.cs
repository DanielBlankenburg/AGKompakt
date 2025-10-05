using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.MontagspostService;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Montagspost
{
    public class MontagsPostBEViewModel : ViewModelBase
    {
        public ObservableCollection<MPDecisionBE> MPDecisionBEList { get; set; } = new ObservableCollection<MPDecisionBE>();
        public ObservableCollection<User> MPBEList { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<MPWeek> MPWeekList { get; set; } = new ObservableCollection<MPWeek>();
        private ObservableCollection<int> _VintageList = new ObservableCollection<int>();
        public ObservableCollection<int> VintageList { get { return _VintageList; } }

        public ICommand SaveCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand BECommand { get; set; }

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
                DecsionBEListFill();
            }
        }
        private MPDecisionBE _SelectedBE;
        public MPDecisionBE SelectedBE
        {
            get { return _SelectedBE; }
            set { SetProperty(ref _SelectedBE, value); }
        }


        public MontagsPostBEViewModel()
        {
            SaveCommand = new RelayCommand(SaveExecute);
            BackCommand = new RelayCommand(BackExecute);
            BECommand = new RelayCommand(BESelectionExecute);
            MPDBContext mPDBContext = new MPDBContext();

            var MPVintages_Query = mPDBContext.MPWeeks.Select(x => x.MPWeekYear).Distinct();
            foreach (var Vintage in MPVintages_Query) VintageList.Add(Vintage);
            if (VintageList.Count > 0) SelectedVintage = VintageList.LastOrDefault();

            UserDBContext db = new UserDBContext();
            var query = db.Users.Where(x => x.PositionId == 1).OrderBy(x => x.NachName).ThenBy(x => x.VorName);
            foreach (User Richter in query) MPBEList.Add(Richter);

            //var Query2 = mPDBContext.MPBE.OrderBy(x => x.MPBEName);
            //foreach (var item in Query2) MPBEList.Add(item);
        }

        private void BESelectionExecute(object obj)
        {
            User user = (User)obj;
            MessageBox.Show(user.VorName);
        }

        private void BackExecute(object obj)
        {
            ViewManager.ShowPageOnMainView<MontagsPostView>();
        }

        private void WeekFill()
        {
            MPWeekList.Clear();
            if (SelectedVintage > 0)
            {
                MPDBContext mPDBContext = new MPDBContext();
                var MPWeek_Query = mPDBContext.MPWeeks.Include(x => x.MPDecisions).Where(x => x.MPWeekYear == SelectedVintage).OrderByDescending(x => x.MPWeekNumber);
                foreach (var item in MPWeek_Query) MPWeekList.Add(item);
            }
            return;
        }
        private void DecsionBEListFill()
        {
            MPDecisionBEList.Clear();
            if (SelectedWeek != null)
            {
                MPDBContext mPDBContext = new MPDBContext();
                UserDBContext userDBContext = new UserDBContext();
                var Query = mPDBContext.MPDecisions.Where(x => x.MPWeekID == SelectedWeek.MPWeekID).OrderBy(x => x.Senat.MPSenatName).ThenBy(x => x.Aktenzeichen);
                foreach (MPDecision item in Query)
                {
                    User BE = userDBContext.Users.FirstOrDefault(x => x.UserId == item.BE);
                    MPDecisionBEList.Add(new MPDecisionBE { Decision = item, BE = BE });
                }
            }
        }


        private void SaveExecute(object obj)
        {
            try
            {
                MPDBContext mPDBContext = new MPDBContext();
                foreach (var item in MPDecisionBEList)
                {
                    if (item.BE != null)
                    {
                        MPDecision Decision = mPDBContext.MPDecisions.Where(x => x.MPDecisionID == item.Decision.MPDecisionID).FirstOrDefault();
                        if (Decision != null)
                        {
                            Decision.BE = item.BE.UserId;
                            mPDBContext.MPDecisions.AddOrUpdate(Decision);
                        }

                    }
                }
                mPDBContext.SaveChanges();
                ViewManager.ShowMainInfoFlyout("Die Änderungen wurden gespeichert", false);
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"Beim Bespeichern der BE ist folgender Fehler aufgetreten: {ex.Message}, {ex.InnerException}");
                ViewManager.ShowMainInfoFlyout($"Beim Bespeichern der BE ist folgender Fehler aufgetreten: {ex.Message}", false);
                throw;
            }
        }
    }
}
