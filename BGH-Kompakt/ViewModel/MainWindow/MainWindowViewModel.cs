using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Classes.SystemSettings;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Enums;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.Views.Start;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;

namespace BGH_Kompakt.ViewModel.MainWindow
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        //private UserDBContext userDBContext = new UserDBContext();
        private string _SenatsText;
        public string SenatsText
        {
            get { return _SenatsText; }
            set { SetProperty<string>(ref _SenatsText, value); }
        }

        private string _loginUser;
        private readonly UserDBContext userContext = new UserDBContext();
        //String suchText = string.Empty;

        public string LoginUser 
        {
            get { return _loginUser; }
            set { SetProperty<string>(ref _loginUser, value); }
        }

        public ObservableCollection<Senat> SenatList { get; set; } = new ObservableCollection<Senat>();
        private Senat selectedSenat;
        public Senat SelectedSenat
        {
            get { return selectedSenat; }
            set 
            { 
                selectedSenat = value; 
                UserManager.Set_SenatSettings(UserManager.RegistratedUser.Senate, SelectedSenat);
                ViewManager.ShowPageOnMainView<StartView>();
                SenatsText = UserManager.SenatSettings.Senat.SenatArt == 2 ? "Senatshefte" : "Sitzungsunterlagen";
            }
        }

        public ObservableCollection<ActionStatus> ActionList { get; set; } = new ObservableCollection<ActionStatus>();
        private bool _ShowStatusBar = false;
        public bool ShowStatusBar
        {
            get { return _ShowStatusBar; }
            set { SetProperty(ref _ShowStatusBar, value); }
        }

        #region Show

        private bool _ShowSitzungsunterlagen = false;
        public bool ShowSitzungsunterlagen
        {
            get { return _ShowSitzungsunterlagen; }
            set { SetProperty(ref _ShowSitzungsunterlagen, value); }
        }
        private bool _ShowSpruchgruppen = false;
        public bool ShowSpruchgruppen
        {
            get { return _ShowSpruchgruppen; }
            set { SetProperty(ref _ShowSpruchgruppen, value); }
        }
        private bool _ShowNebentaetigkeiten = false;
        public bool ShowNebentaetigkeiten
        {
            get { return _ShowNebentaetigkeiten; }
            set { SetProperty(ref _ShowNebentaetigkeiten, value); }
        }
        private bool _ShowSitzungsplaene = false;
        public bool ShowSitzungsplaene
        {
            get { return _ShowSitzungsplaene; }
            set { SetProperty(ref _ShowSitzungsplaene, value); }
        }
        private bool _ShowKanzlei = false;
        public bool ShowKanzlei
        {
            get { return _ShowKanzlei; }
            set { SetProperty(ref _ShowKanzlei, value); }
        }
        private bool _ShowMontagspost = false;
        public bool ShowMontagspost
        {
            get { return _ShowMontagspost; }
            set { SetProperty(ref _ShowMontagspost, value); }
        }

        private bool _ShowMontagspostAdmin = false;
        public bool ShowMontagspostAdmin
        {
            get { return _ShowMontagspostAdmin; }
            set { SetProperty(ref _ShowMontagspostAdmin, value); }
        }

        private bool _ShowAnwaltsauswahl = false;
        public bool ShowAnwaltsauswahl
        {
            get { return _ShowAnwaltsauswahl; }
            set { SetProperty(ref _ShowAnwaltsauswahl, value); }
        }
        #endregion

        public string Version { get; set; }
        
        public MainWindowViewModel() 
        {

            try
            {
                Logger.WriteLog($"logTime: {DateTime.Now}; Loading MainWindow");
                if (UserManager.LoginUser(Environment.UserName))
                {
                    LoginUser = UserManager.RegistratedUser.Fullname;
                    SenatListFill(UserManager.RegistratedUser);
                    ShowSitzungsunterlagen = UserManager.RegistratedUser.ShowSitzungsunterlagen;
                    if (UserManager.SenatSettings.Senat != null)
                        SenatsText = UserManager.SenatSettings.Senat.SenatArt == 2 ? "Senatshefte" : "Sitzungsunterlagen";
                    ShowMontagspost = UserManager.RegistratedUser.ShowMontagspost;
                    ShowMontagspostAdmin = UserManager.RegistratedUser.IsMPAdmin;
                    ShowNebentaetigkeiten = UserManager.RegistratedUser.ShowActivityRequests;
                }
                if (UserManager.RegistratedUser != null)
                {
                    Logger.WriteLog($"logTime: {DateTime.Now}; {UserManager.RegistratedUser.Fullname} hat sich eingeloggt.");
                }
                else
                {
                    Logger.WriteLog($"logTime: {DateTime.Now}; Es konnte kein User eingeloggt werden.");
                }
                Version = $"Version: {ConfigurationManager.AppSettings["Version"]}";
                SeedUserDB();
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"logTime: {DateTime.Now}; Es ist folgender Fehler beim Erstellen des MainWindowsViewModels aufgetreten: {ex.Message}.");
            }
        }

        private void SeedUserDB()
        {
            try
            {
                #region SeedUser
                //Seed Dienstbezeichnung
                foreach (UserEnums.EnumDienstbezeichnungen val in Enum.GetValues(typeof(UserEnums.EnumDienstbezeichnungen))) 
                {
                    if (userContext.Dienstbezeichnungen.FirstOrDefault(x => x.DienstbezeichnungText == val.ToString()) == null)
                        userContext.Dienstbezeichnungen.AddOrUpdate(a => a.DienstbezeichnungText, new Dienstbezeichnung { DienstbezeichnungText = val.ToString()});
                }

                //Seed Geschlecht
                foreach (UserEnums.EnumSex val in Enum.GetValues(typeof(UserEnums.EnumSex)))
                {
                    if (userContext.Geschlechter.FirstOrDefault(x => x.GeschlechtText == val.ToString()) == null)
                        userContext.Geschlechter.AddOrUpdate(a => a.GeschlechtText, new Geschlecht { GeschlechtText = val.ToString()});
                }


                //Seed Position
                List<String> List = new List<String> { "Richter am Bundesgerichtshof", "Wissenschaftlicher Mitarbeiter", "Geschäftsstellenmitarbeiter", "Verwaltungsmitarbeiter" };
                foreach (string suchText in List)
                {
                    if (userContext.Positions.FirstOrDefault(x => x.PositionText == suchText) == null)
                        userContext.Positions.AddOrUpdate(a => a.PositionText, new Position { PositionText = suchText });
                }

                foreach (UserEnums.EnumUserStatus val in Enum.GetValues(typeof(UserEnums.EnumUserStatus)))
                {
                    if (userContext.Status.FirstOrDefault(x => x.StatusText == val.ToString()) == null)
                        userContext.Status.AddOrUpdate(a => a.StatusText, new Status { StatusText = val.ToString()});
                }


                //Seed Titel
                List.Clear();
                List = new List<String> { "kein Titel", "Dr.", "Prof. Dr." };
                foreach (string suchText in List)
                {
                    if (userContext.Titel.FirstOrDefault(x => x.TitelText == suchText) == null)
                        userContext.Titel.AddOrUpdate(a => a.TitelText, new Titel { TitelText = suchText });
                }

                //Seed Senate
                List<Senat> senatsListe = new List<Senat>();
                senatsListe.AddRange(new List<Senat> { 
                
                    new Senat("unbekannter Senat", "u.S.", 1),
                    new Senat("I. Zivilsenat", "I", 1, "\\bgh.bund.de\\dfs\\1-Zivilsenat"),
                    new Senat("II. Zivilsenat", "II", 1, "\\bgh.bund.de\\dfs\\2-Zivilsenat"),
                    new Senat("III. Zivilsenat", "III", 1, "\\bgh.bund.de\\dfs\\3-Zivilsenat"),
                    new Senat("IV. Zivilsenat", "IV", 1, "\\bgh.bund.de\\dfs\\4-Zivilsenat"),
                    new Senat("V. Zivilsenat", "V", 1, "\\bgh.bund.de\\dfs\\5-Zivilsenat"),
                    new Senat("VI. Zivilsenat", "VI", 1, "\\bgh.bund.de\\dfs\\6-Zivilsenat"),
                    new Senat("VIa. Zivilsenat", "VIa", 1, "\\bgh.bund.de\\dfs\\6a-Zivilsenat"),
                    new Senat("VII. Zivilsenat", "VII", 1, "\\bgh.bund.de\\dfs\\7-Zivilsenat"),
                    new Senat("VIII. Zivilsenat", "VIII", 1, "\\bgh.bund.de\\dfs\\8-Zivilsenat"),
                    new Senat("IX. Zivilsenat", "IX", 1, "\\bgh.bund.de\\dfs\\9-Zivilsenat"),
                    new Senat("X. Zivilsenat", "X", 1, "\\bgh.bund.de\\dfs\\10-Zivilsenat"),
                    new Senat("XI. Zivilsenat", "XI", 1, "\\bgh.bund.de\\dfs\\11-Zivilsenat"),
                    new Senat("XII. Zivilsenat", "XII", 1, "\\bgh.bund.de\\dfs\\12-Zivilsenat"),
                    new Senat("XIII. Zivilsenat", "XIII", 1, "\\bgh.bund.de\\dfs\\13-Zivilsenat"),
                    new Senat("1. Strafsenat", "1", 2, "\\bgh.bund.de\\dfs\\1-Strafsenat"),
                    new Senat("2. Strafsenat", "2", 2, "\\bgh.bund.de\\dfs\\2-Strafsenat"),
                    new Senat("3. Strafsenat", "3", 2, "\\bgh.bund.de\\dfs\\3-Strafsenat"),
                    new Senat("4. Strafsenat", "4", 2, "\\bgh.bund.de\\dfs\\4-Strafsenat"),
                    new Senat("5. Strafsenat", "5", 2, "\\bgh.bund.de\\dfs\\5-Strafsenat"),
                    new Senat("6. Strafsenat", "6", 2, "\\bgh.bund.de\\dfs\\6-Strafsenat"),
                    new Senat("Anwaltssenat", "AnwS", 3),
                    new Senat("Notarsenat", "NotS", 3),
                    new Senat("Landwirtschaftssenat", "LwS", 3),
                    new Senat("Patentanwaltsenat", "PAnwS", 3),
                    new Senat("Steuerberatersenat", "StBS", 3),
                    new Senat("Gemeinsamer Senat der obersten Gerichtshöfe", "GSOGH", 3),
                    new Senat("Großer Zivilsenat", "GZS", 3),
                    new Senat("Großer Strafsenat", "GStZ", 3),
                    new Senat("Dienstgericht", "DG", 3),
                    new Senat("Vereinigte Große Senate", "VGZ", 3)
                
                });

                foreach (Senat suchText in senatsListe)
                {
                    if (userContext.Senate.FirstOrDefault(x => x.SenatName == suchText.SenatName) == null)
                        userContext.Senate.AddOrUpdate(a => a.SenatName, new Senat { SenatName = suchText.SenatName, SenatShort = suchText.SenatShort, SenatArt = suchText.SenatArt });
                }

                //Seed Adminstatus
                foreach (UserEnums.EnumAdminStatus val in Enum.GetValues(typeof(UserEnums.EnumAdminStatus)))
                {
                    if (userContext.AdminStatus.FirstOrDefault(x => x.AdminStatusText == val.ToString()) == null)
                        userContext.AdminStatus.AddOrUpdate(a => a.AdminStatusText, new AdminStatus { AdminStatusText = val.ToString() });
                }

                //List.Clear();

                //List = new List<String> { UserEnums.EnumAdminStatus.MontagspostAdmin.ToString(), UserEnums.EnumAdminStatus.NebentätigkeitenAdmin.ToString(), UserEnums.EnumAdminStatus.Programm.ToString(), UserEnums.EnumAdminStatus.Präsidentin.ToString(), UserEnums.EnumAdminStatus.Präsidialrichter.ToString(), UserEnums.EnumAdminStatus.Vorzimmer.ToString(), UserEnums.EnumAdminStatus.Senat.ToString() };
                //foreach (string suchText in List)
                //{
                //    if (userContext.AdminStatus.FirstOrDefault(x => x.AdminStatusText == suchText) == null)
                //        userContext.AdminStatus.AddOrUpdate(a => a.AdminStatusText, new AdminStatus { AdminStatusText = suchText });
                //}

                List<User> userListe = new List<User>();
                userListe.AddRange(new List<User> {

                    new User("Max", "Präsidialrichter", "Mustermann@gmx.de", "Test1", 1, 2, 1, 1, 1),
                    new User("Paula", "Präsidentin", "Mustermann@gmx.de", "Test1", 2, 1, 1, 1, 3),
                    new User("Uwe", "Vorzimmer", "Mustermann@gmx.de", "Test1", 2, 1, 3, 1, 1),
                    //new User("Heinrich", "Schoppmeyer", "Heinrich-Schoppmeyer@bgh.bund.de", "Test1", 1, 3, 1, 1, 3),
                    //new User("Volker", "Schultze", "Volker-Schultze@bgh.bund.de", "Test1", 1, 2, 1, 1, 1),
                    //new User("Christian", "Röhl", "Christian-Roehl@bgh.bund.de", "Test1", 1, 1, 1, 1, 1),
                    //new User("Sven", "Harms", "Sven-Harms@bgh.bund.de", "Test1", 1, 1, 1, 1, 1)
                    //new User("Max", "Präsidialrichter", "Mustermann@gmx.de", "Test1", userContext.Geschlechter.FirstOrDefault(x => x.GeschlechtText == "männlich").GeschlechtID, userContext.Positions.FirstOrDefault(x => x.PositionText == "Richter am Bundesgerichtshof").PositionId, userContext.Status.FirstOrDefault(x => x.StatusText == "aktiv").StatusId, userContext.enumDienstbezeichnungen.FirstOrDefault(x => x.DienstbezeichnungText == UserEnums.EnumAdminStatus.Präsidentin.ToString()).DienstbezeichnungId),
                    //new User("Max", "Präsidentin", "Mustermann@gmx.de", "Test1", userContext.Geschlechter.FirstOrDefault(x => x.GeschlechtText == "weiblich").GeschlechtID, userContext.Positions.FirstOrDefault(x => x.PositionText == "Richter am Bundesgerichtshof").PositionId, userContext.Status.FirstOrDefault(x => x.StatusText == "aktiv").StatusId, userContext.enumDienstbezeichnungen.FirstOrDefault(x => x.DienstbezeichnungText == UserEnums.EnumAdminStatus.Präsidialrichter.ToString()).DienstbezeichnungId),
                    //new User("Max", "Vorzimmer", "Mustermann@gmx.de", "Test1", userContext.Geschlechter.FirstOrDefault(x => x.GeschlechtText == "männlich").GeschlechtID, userContext.Positions.FirstOrDefault(x => x.PositionText == "Geschäftsstellenmitarbeiter").PositionId, userContext.Status.FirstOrDefault(x => x.StatusText == "aktiv").StatusId, userContext.enumDienstbezeichnungen.FirstOrDefault(x => x.DienstbezeichnungText == UserEnums.EnumAdminStatus.Vorzimmer.ToString()).DienstbezeichnungId),
                });

                foreach (User suchText in userListe)
                {
                    if (userContext.Users.FirstOrDefault(x => x.NachName == suchText.NachName) == null)
                        userContext.Users.AddOrUpdate(a => a.NachName, new User { VorName = suchText.VorName, NachName = suchText.NachName, EMail = suchText.EMail, ComputerName = suchText.ComputerName, GeschlechtID = suchText.GeschlechtID, PositionId = suchText.PositionId, StatusId = suchText.StatusId, DienstbezeichnungId = suchText.DienstbezeichnungId });
                }

                var programmSetting = userContext.ProgrammSettings.ToArray();
                if (programmSetting.Count() == 0)
                {
                    userContext.ProgrammSettings.AddOrUpdate(a => a.Id, new ProgrammSetting
                    {
                        PathSitzungsunterlagen = "Sitzungsunterlagen\\",
                        Pathunterlagenverwaltung = "Unterlagenverwaltung\\",
                        PathDokstelle = "Diskettenversand\\Entscheidungen\\",
                        PathMontagspost = "Montagspost\\",
                        PathDokstelleDFS = $"\\\\bgh.bund.de\\dfs\\Dokumentationsstelle\\",
                        EMailDokstelle = "Montagspost@bgh.bund.de"
                    });
                }



                userContext.SaveChanges();
                #endregion
            }
            catch (Exception ex) 
            {
                Logger.WriteLog($"logTime: {DateTime.Now}; Es konnte folgender Fehler beim Füllen der User-Werte aufgetreten: {ex.Message}.");
            }
        }
        public void SenatListFill(User RegistratedUser)
        {
            if (RegistratedUser.Senate.Count > 0)
            {
                SenatList.Clear();
                foreach (var item in RegistratedUser.Senate)
                {
                    SenatList.Add(item);
                }
                SelectedSenat = RegistratedUser.Senate.FirstOrDefault();
            }
        }
    }
}
