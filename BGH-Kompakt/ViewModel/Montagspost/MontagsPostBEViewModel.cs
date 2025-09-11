using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.MontagspostService;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Views;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Montagspost
{
    public class MontagsPostBEViewModel : ViewModelBase
    {
        public ObservableCollection<MPDecision> MPDecisionList { get; set; } = new ObservableCollection<MPDecision>();
        public ObservableCollection<MPBE> MPBEList { get; set; } = new ObservableCollection<MPBE>();

        public ICommand SaveCommand { get; set; }
        public ICommand BackCommand { get; set; }


        public MontagsPostBEViewModel()
        {
            SaveCommand = new RelayCommand(SaveExecute);
            BackCommand = new RelayCommand(BackExecute);


            MPDBContext mPDBContext = new MPDBContext();
            var Query = mPDBContext.MPDecisions.Include(x => x.BE).Where(x => x.MPWeekID == MontagsPostManager.SavedWeek.MPWeekID).OrderBy(x => x.Senat.MPSenatName).ThenBy(x => x.Aktenzeichen);
            foreach (var item in Query) MPDecisionList.Add(item);
            var Query2 = mPDBContext.MPBE.OrderBy(x => x.MPBEName);
            foreach (var item in Query2) MPBEList.Add(item);
        }

        private void BackExecute(object obj)
        {
            ViewManager.ShowPageOnMainView<MontagsPostView>();
        }

        private void SaveExecute(object obj)
        {
            MPDBContext mPDBContext = new MPDBContext();
            foreach(var item in MPDecisionList)
            {
                if(item.BE != null)
                {
                    MPDecision Decision = mPDBContext.MPDecisions.Where(x => x.MPDecisionID == item.MPDecisionID).FirstOrDefault();
                    if (Decision != null)
                    {
                        MPBE newBE = mPDBContext.MPBE.Where(x => x.MPBEID == item.BE.MPBEID).FirstOrDefault();
                        Decision.BE = newBE;
                        mPDBContext.MPDecisions.AddOrUpdate(Decision);
                    }
                    
                }
            }
            mPDBContext.SaveChanges();
            ViewManager.ShowPageOnMainView<MontagsPostView>();
        }
    }
}
