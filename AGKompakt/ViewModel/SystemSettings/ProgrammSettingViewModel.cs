using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Classes.SystemSettings;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Exception = System.Exception;

namespace BGH_Kompakt.ViewModel.SystemSettings
{
    public class ProgrammSettingViewModel : ViewModelBase
    {
        public ICommand SeedMontagspostCommand { get; set; }
        public ICommand SeedAZCommand { get; set; }
        public ICommand SeedARCommand { get; set; }
        public ICommand TestPath { get; set; }
        public ICommand SaveSettings { get; set; }
        public ICommand SeedDienstbezeichnungen { get; set; }
        public ProgrammSetting ProgrammSetting { get; set; }
        private readonly UserDBContext userDBContext = new UserDBContext();

        private bool _AnzeigeMontagspost;
        public bool AnzeigeMontagspost
        {
            get => _AnzeigeMontagspost;
            set 
            {
                _AnzeigeMontagspost = value;
                ProgrammSetting.MontagspostActivated = value;
                SaveProgrammsettings(ProgrammSetting, false);
            }
        }

        private bool _AnzeigeActivitRequests;
        public bool AnzeigeActivitRequests
        {
            get => _AnzeigeActivitRequests;
            set
            {
                _AnzeigeActivitRequests = value;
                ProgrammSetting.ActivityRequestActivated = value;
                SaveProgrammsettings(ProgrammSetting, false);
            }
        }

        public ProgrammSettingViewModel()
        {
            TestPath = new RelayCommand(TestPathExecute);
            SaveSettings = new RelayCommand(SaveSettingsExecute);
            SeedDienstbezeichnungen = new RelayCommand(SeedDienstbezeichnungenExecute);

            ProgrammSetting = userDBContext.ProgrammSettings.FirstOrDefault() ?? new ProgrammSetting();
            AnzeigeMontagspost = ProgrammSetting.MontagspostActivated;
            AnzeigeActivitRequests = ProgrammSetting.ActivityRequestActivated;
        }

        private void SeedDienstbezeichnungenExecute(object obj)
        {
            List<User> users = userDBContext.Users.ToList();
            foreach (User user in users)
            {
                if (user.PositionId == 1)
                {
                    UserDienstbezeichnung userDienstbezeichnung = userDBContext.UserDienstbezeichnungen.FirstOrDefault(x => x.UserId == user.UserId);
                    if (userDienstbezeichnung == null)
                    {
                        int dienstbezeichnung = (int)(user.Dienstbezeichnung != null ? user.DienstbezeichnungId : 1);
                        userDienstbezeichnung = new UserDienstbezeichnung { User = user, 
                                                                            DienstbezeichnungId = dienstbezeichnung,
                                                                            GültigAb = new DateTime(2025,1,1)};
                        userDBContext.UserDienstbezeichnungen.AddOrUpdate(userDienstbezeichnung);
                    }
                }
            }

            userDBContext.SaveChanges();
        }

        private void SaveSettingsExecute(object obj)
        {
            if (Directory.Exists(ProgrammSetting.PathDokstelleDFS))
            {
                SaveProgrammsettings(ProgrammSetting, true); 
            }
            else
            {
                ViewManager.ShowMainInfoFlyout("Der Pfad DokstelleDFS exisitiert nicht. Bitte tragen Sie einen gültigen Pfad ein.",false);
                return;
            }
            
        }


        private void TestPathExecute(object obj)
        {
            string path = $"{ProgrammSetting.PathDokstelleDFS}{ProgrammSetting.PathDokstelle}";
            if (Directory.Exists(path))
            {
                ViewManager.ShowMainInfoFlyout($"Der Pfad {path} wurde gefunden.", false);
            }
            else
            {
                ViewManager.ShowMainInfoFlyout($"Der Pfad {path} konnte nicht gefunden werden.", false);
            }

        }

        private void SaveProgrammsettings(ProgrammSetting programmSetting, bool successMessage)
        {
            try
            {
                userDBContext.ProgrammSettings.AddOrUpdate(programmSetting);
                userDBContext.SaveChanges();
                if (successMessage) ViewManager.ShowMainInfoFlyout("Die Änderungen wurden gespeichert", false);
            }
            catch (Exception ex) { ErrorMessage.CreateExceptionWithFlyOutMessage("Speichern der Einstellung", ex);}
        }
    }
}
