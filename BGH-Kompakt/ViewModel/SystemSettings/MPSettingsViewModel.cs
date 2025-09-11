using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.SystemSettings
{
    public partial class MPSettingsViewModel : ViewModelBase
    {
        public ICommand SaveEMailCommand { get; set; }

        public MPSetting MPSetting { get; set; }
        public ObservableCollection<MPEMail> EMailList { get; set; } = new ObservableCollection<MPEMail>();
        private MPEMail _SelectedEMail;

        public MPEMail SelectedEMail
        {
            get { return _SelectedEMail; }
            set { SetProperty(ref _SelectedEMail, value); }
        }


        MPDBContext mPDBContext = new MPDBContext();
        public MPSettingsViewModel()
        {
            SaveEMailCommand = new RelayCommand(SaveEMailExecute);

            MPSetting = mPDBContext.MPSettings.FirstOrDefault();
            var query = mPDBContext.MPEMails;
            foreach (var mail in query) EMailList.Add(mail);
    }

        private void SaveEMailExecute(object obj)
        {
            MPSetting newMPSetting = mPDBContext.MPSettings.FirstOrDefault();
            newMPSetting.MPSettingEMailAnrede = MPSetting.MPSettingEMailAnrede;
            newMPSetting.MPSettingEMailSchluss = MPSetting.MPSettingEMailSchluss;
            newMPSetting.MPSettingDatenschutzhinweis= MPSetting.MPSettingDatenschutzhinweis;
            mPDBContext.MPSettings.AddOrUpdate(newMPSetting);
            mPDBContext.SaveChanges();
            ViewManager.ShowMainInfoFlyout("Die Änderungen wurden gespeichert", false);
        }
    }
}
