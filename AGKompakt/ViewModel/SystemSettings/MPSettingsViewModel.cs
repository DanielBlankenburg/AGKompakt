using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows.Input;
using static BGH_Kompakt.Enums.SettingEnums;

namespace BGH_Kompakt.ViewModel.SystemSettings
{
    public partial class MPSettingsViewModel : ViewModelBase
    {
        public ICommand SaveEMailCommand { get; set; }
        public ICommand SaveAutotextCommand { get; set; }
        public ICommand SaveBSCWServerCommand { get; set; }

        public MPSetting MPSetting { get; set; }
        public ObservableCollection<MPEMail> EMailList { get; set; } = new ObservableCollection<MPEMail>();
        private MPEMail _SelectedEMail;

        public MPEMail SelectedEMail
        {
            get { return _SelectedEMail; }
            set { SetProperty(ref _SelectedEMail, value); }
        }

        public IEnumerable<Drives> DriveList
        {
            get { return Enum.GetValues(typeof(Drives)).Cast<Drives>(); }
        }


        readonly MPDBContext mPDBContext = new MPDBContext();
        public MPSettingsViewModel()
        {
            SaveEMailCommand = new RelayCommand(SaveEMailExecute);
            SaveAutotextCommand = new RelayCommand(SaveAutoTextExecute);
            SaveBSCWServerCommand = new RelayCommand(SaveBSCWServerExecute);

            try
            {
                MPSetting = mPDBContext.MPSettings.FirstOrDefault();
                var query = mPDBContext.MPEMails;
                foreach (var mail in query) EMailList.Add(mail);
            }
            catch (Exception ex) { ErrorMessage.CreateExceptionWithFlyOutMessage("Open MPSettingsViewModel", ex); }
    }

        private void SaveBSCWServerExecute(object obj)
        {
            try
            {
                MPSetting newMPSetting = mPDBContext.MPSettings.FirstOrDefault();
                newMPSetting.UploadBSCWServer = MPSetting.UploadBSCWServer;
                newMPSetting.BSCWServerDrive = MPSetting.BSCWServerDrive;
                mPDBContext.MPSettings.AddOrUpdate(newMPSetting);
                mPDBContext.SaveChanges();
                ViewManager.ShowMainInfoFlyout("Die Änderungen wurden gespeichert", false);
            }
            catch (Exception ex) 
            { 
                Logger.WriteLog( ex.Message + "; " + ex.InnerException);
                ViewManager.ShowMainInfoFlyout("Die Änderungen konnten nicht gespeichert werden. Die Fehlerbeschreibung befindet sich in der Log-Datei.", false);
            }
        }

        private void SaveAutoTextExecute(object obj)
        {
            try
            {
                foreach(MPEMail Autotext in EMailList)
                {
                    MPEMail changeText = mPDBContext.MPEMails.FirstOrDefault(x => x.MPEMailID == Autotext.MPEMailID);
                    if (changeText != null)
                    {
                        changeText.MPEMailBody = Autotext.MPEMailBody;
                        mPDBContext.MPEMails.AddOrUpdate(changeText);
                        mPDBContext.SaveChanges();
                    }
                }
                ViewManager.ShowMainInfoFlyout("Die Änderungen wurden gespeichert", false);
            }
            catch (Exception ex) 
            { 
                Logger.WriteLog(ex.Message + "; " + ex.InnerException);
                ViewManager.ShowMainInfoFlyout("Die Änderungen konnten nicht gespeichert werden. Die Fehlerbeschreibung befindet sich in der Log-Datei.", false);
            }
        }

        private void SaveEMailExecute(object obj)
        {
            try
            {
                MPSetting newMPSetting = mPDBContext.MPSettings.FirstOrDefault();
                newMPSetting.MPSettingEMailAnrede = MPSetting.MPSettingEMailAnrede;
                newMPSetting.MPSettingEMailSchluss = MPSetting.MPSettingEMailSchluss;
                newMPSetting.MPSettingDatenschutzhinweis= MPSetting.MPSettingDatenschutzhinweis;
                mPDBContext.MPSettings.AddOrUpdate(newMPSetting);
                mPDBContext.SaveChanges();
                ViewManager.ShowMainInfoFlyout("Die Änderungen wurden gespeichert", false);
            }
            catch (Exception ex) 
            { 
                ErrorMessage.CreateExceptionWithFlyOutMessage("MPSettings-SaveEMailExecute", ex); 
            }

        }
    }
}
