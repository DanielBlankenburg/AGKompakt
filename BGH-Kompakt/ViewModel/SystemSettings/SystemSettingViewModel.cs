using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using static BGH_Kompakt.Enums.SettingEnums;

namespace BGH_Kompakt.ViewModel.SystemSettings
{
    public partial class SystemSettingViewModel : ViewModelBase
    {
        private readonly UserDBContext userDBContext = new UserDBContext();

        //ICommands
        #region Commands
        public ICommand AzNewCommand { get; set; }
        public ICommand AzSaveCommand { get; set; }
        public ICommand AzDeleteCommand { get; set; }
        public ICommand SGNewCommand { get; set; }
        public ICommand SGSaveCommand { get; set; }
        public ICommand SGDeleteCommand { get; set; }
        public ICommand RichterSaveCommand { get; set; }
        public ICommand RichterDeleteCommand { get; set; }
        public ICommand ExpanderCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteMemeberCommand { get; set; }

        #endregion
        private ObservableCollection<Senat> _senatList = new ObservableCollection<Senat>();
        public ObservableCollection<Senat> SenatList { get { return _senatList; } }
        private Senat _selectedSenat;
        public Senat SelectedSenat
        {
            get { return _selectedSenat; }
            set 
            { 
                _selectedSenat = value;
                switch (SelectedSenat.SenatArt)
                {
                    case 1:
                        ShowZivilsenate = true;
                        ShowStrafsenate = false;
                        break;
                    case 2:
                        ShowZivilsenate = false;
                        ShowStrafsenate = true;
                        break;
                    case 3:
                        ShowZivilsenate = false;
                        ShowStrafsenate = false;
                        break;
                }
                SenatSetting newSenatSetting = userDBContext.SenatSettings.Include(x => x.Aktenzeichen).Include(x => x.Spruchgruppen).FirstOrDefault(x => x.SenatID == SelectedSenat.SenatID);
                if (newSenatSetting != null) SenatSetting = newSenatSetting;
                if (SenatSetting.Aktenzeichen != null)
                {
                    AktenzeichenList.Clear();
                    foreach (var item in SenatSetting.Aktenzeichen) AktenzeichenList.Add(item);
                }
                if (SenatSetting.Spruchgruppen!= null)
                {
                    SpruchgruppenList.Clear();
                    foreach (var item in SenatSetting.Spruchgruppen) SpruchgruppenList.Add(item);
                    SenatMemberSet();
                }
                _UserList.Clear();
                _AdminUserList.Clear();
                var Query = userDBContext.Senate.Include(x => x.Users).Include(x => x.AdminUsers).Where(x => x.SenatID == SelectedSenat.SenatID).FirstOrDefault();
                foreach (var item in Query.Users.OrderBy(o => o.NachName)) _UserList.Add(item);
                foreach (var item in Query.AdminUsers.OrderBy(o => o.NachName)) _AdminUserList.Add(item);

                ShowUser = true;
                if (value != null) ShowSettings = true;
                ShowAdmin = UserManager.RegistratedUser.IsSenatAdmin(SelectedSenat);

            }
        }

        private ObservableCollection<User> _UserList = new ObservableCollection<User>();
        public ObservableCollection<User> UserList { get { return _UserList; } }

        private User _UserSelected;
        public User UserSelected
        {
            get { return _UserSelected; }
            set { SetProperty(ref _UserSelected, value); }
        }

        private ObservableCollection<User> _AdminUserList = new ObservableCollection<User>();
        public ObservableCollection<User> AdminUserList { get { return _AdminUserList; } }

        private SenatSetting _SenatSetting;
        public SenatSetting SenatSetting
        {
            get { return _SenatSetting; }
            set { SetProperty(ref _SenatSetting, value); }
        }

        //Aktenzeichen
        public ObservableCollection<SenatAktenzeichen> AktenzeichenList { get; set; } = new ObservableCollection<SenatAktenzeichen>();
        private SenatAktenzeichen _SelectedAktenzeichen;

        public SenatAktenzeichen SelectedAktenzeichen
        {
            get { return _SelectedAktenzeichen; }
            set
            {
                _SelectedAktenzeichen = value;
                EditAktenzeichen = SelectedAktenzeichen;
                NewAZ = false;
            }
        }
        private SenatAktenzeichen _EditAktenzeichen;
        public SenatAktenzeichen EditAktenzeichen
        {
            get { return _EditAktenzeichen; }
            set 
            { 
                SetProperty(ref _EditAktenzeichen, value);
                EnableAZ = (value != null);
            }
        }

        //Spruchgruppen
        public ObservableCollection<SenatSpruchgruppe> SpruchgruppenList { get; set; } = new ObservableCollection<SenatSpruchgruppe>();
        private SenatSpruchgruppe _SelectedSpruchgruppe;

        public SenatSpruchgruppe SelectedSpruchgruppe
        {
            get { return _SelectedSpruchgruppe; }
            set
            {
                _SelectedSpruchgruppe = value;
                EditSpruchgruppe = SelectedSpruchgruppe;
                NewSG = false;
                SGMemberSet();
            }
        }

        public ObservableCollection<User> SpruchgruppenMembers { get; set; } = new ObservableCollection<User>();
        private User _SelectedSpruchgruppenMember;
        public User SelectedSpruchgruppenMember
        {
            get { return _SelectedSpruchgruppenMember; }
            set
            {
                SetProperty(ref _SelectedSpruchgruppenMember, value);
            }
        }

        public ObservableCollection<User> SenatMembers { get; set; } = new ObservableCollection<User>();
        private User _SelectedSenatMember;
        public User SelectedSenatMember
        {
            get { return _SelectedSenatMember; }
            set
            {
                SetProperty(ref _SelectedSenatMember, value);
                //SelectedSpruchgruppenMember = null;
            }
        }

        private void SGMemberSet()
        {
            SpruchgruppenMembers.Clear();
            var tempList = userDBContext.SenatSpruchgruppen.Include(x => x.Members).FirstOrDefault(sg => sg.SenatSpruchgruppeID == SelectedSpruchgruppe.SenatSpruchgruppeID);
            foreach (var item in tempList.Members) SpruchgruppenMembers.Add(item);
        }


        private void SenatMemberSet()
        {
            SenatMembers.Clear();
            var tempList = userDBContext.Senate.Include(x => x.Users).FirstOrDefault(s => s.SenatID == SelectedSenat.SenatID);
            foreach (var item in tempList.Users) if (item.PositionId == 1) SenatMembers.Add(item); //nur Richter (PositionID = 1) hinzufügen
        }



        private SenatSpruchgruppe _EditSpruchgruppe;
        public SenatSpruchgruppe EditSpruchgruppe
        {
            get { return _EditSpruchgruppe; }
            set
            {
                SetProperty(ref _EditSpruchgruppe, value);
                EnableSG = (value != null);
            }
        }


        //private ListCollectionView _AktenzeichenListView;
        //public ListCollectionView AktenzeichenListView
        //{
        //    get { return _AktenzeichenListView; }
        //}

        private SenatAktenzeichen _SenatAktenzeichen;
        public SenatAktenzeichen SenatAktenzeichen
        {
            get { return _SenatAktenzeichen; }
            set { SetProperty(ref _SenatAktenzeichen, value); }
        }


        public IEnumerable<Drives> DriveList
        {
            get { return Enum.GetValues(typeof(Drives)).Cast<Drives>(); }
        }

        //Show
        #region Show
        private bool _ShowSettings = false;
        public bool ShowSettings
        {
            get { return _ShowSettings; }
            set { SetProperty(ref _ShowSettings, value); }
        }

        private bool _ShowUser = true;
        public bool ShowUser
        {
            get { return _ShowUser; }
            set
            {
                SetProperty(ref _ShowUser, value);
                if (value)
                {
                    ShowAllgemeineEinstellungen = !value;
                    ShowAktenzeichen = !value;
                    ShowSpruchgruppen = !value;
                    ShowImport = !value;
                }
            }
        }

        private bool _ShowAdmin = false;
        public bool ShowAdmin
        {
            get { return _ShowAdmin; }
            set { SetProperty(ref _ShowAdmin, value); }
        }



        private bool _ShowAllgemeineEinstellungen = false;
        public bool ShowAllgemeineEinstellungen
        {
            get { return _ShowAllgemeineEinstellungen; }
            set 
            { 
                SetProperty(ref _ShowAllgemeineEinstellungen, value); 
                if (value)
                {
                    ShowUser = !value;
                    ShowAktenzeichen = !value;
                    ShowSpruchgruppen= !value;
                    ShowImport = !value;
                }
            }
        }

        private bool _ShowAktenzeichen = false;
        public bool ShowAktenzeichen
        {
            get { return _ShowAktenzeichen; }
            set
            {
                SetProperty(ref _ShowAktenzeichen, value);
                if (value)
                {
                    ShowUser = !value;
                    ShowAllgemeineEinstellungen = !value;
                    ShowSpruchgruppen = !value;
                    ShowImport = !value;
                }
            }
        }

        private bool _ShowSpruchgruppen = false;
        public bool ShowSpruchgruppen
        {
            get { return _ShowSpruchgruppen; }
            set
            {
                SetProperty(ref _ShowSpruchgruppen, value);
                if (value)
                {
                    ShowUser = !value;
                    ShowAktenzeichen = !value;
                    ShowAllgemeineEinstellungen = !value;
                    ShowImport = !value;
                }
            }
        }

        private bool _ShowImport = false;
        public bool ShowImport
        {
            get { return _ShowImport; }
            set
            {
                SetProperty(ref _ShowImport, value);
                if (value)
                {
                    ShowUser = !value;
                    ShowAktenzeichen = !value;
                    ShowSpruchgruppen = !value;
                    ShowAllgemeineEinstellungen = !value;
                }
            }
        }

        private bool _ShowAktenzeichenList = false;
        public bool ShowAktenzeichenList
        {
            get { return _ShowAktenzeichenList; }
            set { SetProperty(ref _ShowAktenzeichenList, value); }
        }
        private bool _ShowZivilsenate = false;
        public bool ShowZivilsenate
        {
            get { return _ShowZivilsenate; }
            set { SetProperty(ref _ShowZivilsenate, value); }
        }
        private bool _ShowStrafsenate = false;
        public bool ShowStrafsenate
        {
            get { return _ShowStrafsenate; }
            set { SetProperty(ref _ShowStrafsenate, value); }
        }

        #endregion

        //Variablen für AZ
        #region AZ
        private bool _NewAZ = false;
        public bool NewAZ
        {
            get { return _NewAZ; }
            set 
            { 
                SetProperty(ref _NewAZ, value);
                AZChangeText = (value) ? "AZ eintragen" : "Änderung speichern";
            }
        }

        private bool _EnableAZ = false;
        public bool EnableAZ
        {
            get { return _EnableAZ; }
            set { SetProperty(ref _EnableAZ, value); }
        }

        private string _AZChangeText = "Änderung speichern";
        public string AZChangeText
        {
            get { return _AZChangeText; }
            set { SetProperty(ref _AZChangeText, value); }
        }

        #endregion

        //Variablen für SG
        #region SG
        private bool _NewSG = false;
        public bool NewSG
        {
            get { return _NewSG; }
            set
            {
                SetProperty(ref _NewSG, value);
                SGChangeText = (value) ? "SG eintragen" : "Änderung speichern";
            }
        }

        private bool _EnableSG = false;
        public bool EnableSG
        {
            get { return _EnableSG; }
            set { SetProperty(ref _EnableSG, value); }
        }

        private string _SGChangeText = "Änderung speichern";
        public string SGChangeText
        {
            get { return _SGChangeText; }
            set { SetProperty(ref _SGChangeText, value); }
        }

        #endregion


        public SystemSettingViewModel()
        {
            AzNewCommand = new RelayCommand(AzNewExecute);
            AzSaveCommand = new RelayCommand(AzSaveExecute, AzSaveCanExecute);
            AzDeleteCommand = new RelayCommand(AzDeleteExecute, AzDeleteCanExecute);
            SGNewCommand = new RelayCommand(SGNewExecute);
            SGSaveCommand = new RelayCommand(SGSaveExecute, SGSaveCanExecute);
            SGDeleteCommand = new RelayCommand(SGDeleteExecute, SGDeleteCanExecute);
            ExpanderCommand = new RelayCommand(ExpanderExecute);
            RichterSaveCommand = new RelayCommand(RichterSaveExecute, RichterSaveCanExecute);
            RichterDeleteCommand = new RelayCommand(RichterDeleteExecute, RichterDeleteCanExecute);
            SaveCommand = new RelayCommand(SaveExecuted);
            DeleteMemeberCommand = new RelayCommand(DeleteMemeberExecuted);

            var QuerySenate = UserManager.RegistratedUser.Senate.ToArray();
            if (QuerySenate != null) foreach (var Senat in QuerySenate) SenatList.Add(Senat);

        }


        //executes
        #region Executes
        private void AzNewExecute(object obj)
        {
            SenatAktenzeichen NewAktenzeichen = new SenatAktenzeichen();
            //NewAktenzeichen.SenatAktenzeichenName = string.Empty;
            NewAktenzeichen.SenatAktenzeichenOrderNumber = AktenzeichenList.Count + 1;
            EditAktenzeichen = NewAktenzeichen;
            NewAZ = true;

        }
        //Execute für AZ
        private void AzSaveExecute(object obj)
        {

            if (NewAZ)
            {
                SenatAktenzeichen NewAktenzeichen = new SenatAktenzeichen
                {
                    SenatAktenzeichenName = EditAktenzeichen.SenatAktenzeichenName,
                    SenatAktenzeichenNameRaw = EditAktenzeichen.SenatAktenzeichenName.Replace(" ", "_"),
                    SenatAktenzeichenOrderNumber = EditAktenzeichen.SenatAktenzeichenOrderNumber,
                    SenatSetting = SenatSetting
                };
                try
                {
                    userDBContext.SenatAktenzeichen.Add(NewAktenzeichen);
                    userDBContext.SaveChanges();
                    AktenzeichenList.Add(NewAktenzeichen);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Der neue Eintrag konnte nicht gespeichert werden. Es ist folgender Fehler aufgetreten: " + ex.Message + "; " + ex.InnerException, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                    try
                    {
                        userDBContext.SenatAktenzeichen.AddOrUpdate(SelectedAktenzeichen);
                        userDBContext.SaveChanges();
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Der neue Eintrag konnte nicht gespeichert werden. Es ist folgender Fehler aufgetreten: " + ex.Message + "; " + ex.InnerException, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
            }
            EditAktenzeichen = null;
        }
        private void AzDeleteExecute(object obj)
        {
            try
            {
                userDBContext.SenatAktenzeichen.Remove(userDBContext.SenatAktenzeichen.FirstOrDefault(x => x.SenatAktenzeichenID == SelectedAktenzeichen.SenatAktenzeichenID));
                userDBContext.SaveChanges();
                AktenzeichenList.Remove(SelectedAktenzeichen);
                EditAktenzeichen = null;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Der Eintrag konnte nicht gelöscht werden. Es ist folgender Fehler aufgetreten: " + ex.Message + "; " + ex.InnerException, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool AzDeleteCanExecute(object obj)
        {
            return (SelectedAktenzeichen != null && NewAZ == false);
        }
        private bool AzSaveCanExecute(object obj)
        {
            return (EditAktenzeichen != null);
        }

        //Execute für SG
        private void SGNewExecute(object obj)
        {
            SenatSpruchgruppe NewSpruchgruppe = new SenatSpruchgruppe();
            //NewAktenzeichen.SenatAktenzeichenName = string.Empty;
            NewSpruchgruppe.SenatSpruchgruppeOrderNumber = SpruchgruppenList.Count + 1;
            EditSpruchgruppe = NewSpruchgruppe;
            NewSG = true;

        }
        private void SGSaveExecute(object obj)
        {

            if (NewSG)
            {
                SenatSpruchgruppe NewSpruchgruppe = new SenatSpruchgruppe();
                NewSpruchgruppe.SenatSpruchgruppeName = EditSpruchgruppe.SenatSpruchgruppeName;
                NewSpruchgruppe.SenatSpruchgruppeOrderNumber= EditSpruchgruppe.SenatSpruchgruppeOrderNumber;
                NewSpruchgruppe.SenatSetting = SenatSetting;
                try
                {
                    userDBContext.SenatSpruchgruppen.Add(NewSpruchgruppe);
                    userDBContext.SaveChanges();
                    SpruchgruppenList.Add(NewSpruchgruppe);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Der neue Eintrag konnte nicht gespeichert werden. Es ist folgender Fehler aufgetreten: " + ex.Message + "; " + ex.InnerException, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                    try
                    {
                        userDBContext.SenatSpruchgruppen.AddOrUpdate(SelectedSpruchgruppe);
                        userDBContext.SaveChanges();
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Der neue Eintrag konnte nicht gespeichert werden. Es ist folgender Fehler aufgetreten: " + ex.Message + "; " + ex.InnerException, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
            }
            EditSpruchgruppe = null;
        }
        private void SGDeleteExecute(object obj)
        {
            try
            {
                userDBContext.SenatSpruchgruppen.Remove(userDBContext.SenatSpruchgruppen.FirstOrDefault(x => x.SenatSpruchgruppeID== SelectedSpruchgruppe.SenatSpruchgruppeID));
                userDBContext.SaveChanges();
                SpruchgruppenList.Remove(SelectedSpruchgruppe);
                EditAktenzeichen = null;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Der Eintrag konnte nicht gelöscht werden. Es ist folgender Fehler aufgetreten: " + ex.Message + "; " + ex.InnerException, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool SGDeleteCanExecute(object obj)
        {
            return (SelectedSpruchgruppe != null && NewAZ == false);
        }
        private bool SGSaveCanExecute(object obj)
        {
            return (EditSpruchgruppe != null);
        }


        private void SaveExecuted(object obj)
        {
            try
            {
                userDBContext.SenatSettings.AddOrUpdate(SenatSetting);
                userDBContext.SaveChanges();
                ViewManager.ShowMainInfoFlyout("Die Änderungen wurden gespeichert.", false);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Der Einstellungen konnten nicht gespeichert werden. Es ist folgender Fehler aufgetreten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void ExpanderExecute(object obj)
        {
            if (obj != null)
            {
                switch ((string)obj)
                {
                    case "VorAZ":
                        ShowAllgemeineEinstellungen = false;
                        ShowAktenzeichen = true;
                        break;
                    case "BackSettings":
                        ShowAllgemeineEinstellungen = true;
                        ShowAktenzeichen = false;
                        break;
                    case "VorSpruchgruppe":
                        ShowAktenzeichen = false;
                        ShowSpruchgruppen = true;
                        break;
                    case "BackAZ":
                        ShowAktenzeichen = true;
                        ShowSpruchgruppen= false;
                        break;
                    case "VorImport":
                        ShowImport = true;
                        ShowSpruchgruppen = false;
                        break;
                    case "BackSpruchgruppen":
                        ShowSpruchgruppen = true;
                        ShowImport= false;
                        break;
                }
            }
        }

        private void DeleteMemeberExecuted(object obj)
        {
            bool confirm = ViewManager.ShowQuestionWindow("Soll der User gelöscht werden?\n\n Dieser wird dann per E-Mail über die Löschung informiert.", "Ja");
            if (confirm)
            {
                Senat senat = userDBContext.Senate.FirstOrDefault(x => x.SenatID == SelectedSenat.SenatID);
                User user = userDBContext.Users.FirstOrDefault(x => x.UserId == UserSelected.UserId);
                senat.Users.Remove(user);
                userDBContext.SaveChanges();
                UserList.Remove(UserSelected);
                try
                {
                    EMailVersand eMailVersand = new EMailVersand();
                    eMailVersand.Send_Email(user.EMail, $"Nutzungslöschung {senat.SenatName}", $"Sehr {(user.GeschlechtID == 1 ? "geehrter Herr" : "geehrte Frau")} {user.NachName},<br> <br> sie wurden als Nutzer aus dem {senat.SenatName} gelöscht.<br> <br>Mit freundlichen Grüßen<br>{UserManager.RegistratedUser.Fullname}", immediatSend: true);
                }
                catch (Exception) { }
            }

        }

        private bool RichterDeleteCanExecute(object obj)
        {
            return SelectedSpruchgruppenMember != null;
        }

        private void RichterDeleteExecute(object obj)
        {
            try
            {
                SelectedSpruchgruppe.Members.Remove(SelectedSpruchgruppenMember);
                userDBContext.SenatSpruchgruppen.AddOrUpdate(SelectedSpruchgruppe);
                userDBContext.SaveChanges();
                SpruchgruppenMembers.Remove(SelectedSpruchgruppenMember);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Der Richter konnte nicht hinzugefügt werden. Es ist folgender Fehler aufgetreten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool RichterSaveCanExecute(object obj)
        {
            return SelectedSenatMember != null;
        }

        private void RichterSaveExecute(object obj)
        {
            try
            {
                //SelectedSpruchgruppe.Members.Add(SelectedSenatMember);
                SenatSpruchgruppe selelctedSG = userDBContext.SenatSpruchgruppen.FirstOrDefault(x => x.SenatSpruchgruppeID == SelectedSpruchgruppe.SenatSpruchgruppeID);
                User selectedMember = userDBContext.Users.FirstOrDefault(u => u.UserId == SelectedSenatMember.UserId);
                selelctedSG.Members.Add(selectedMember);
                userDBContext.SenatSpruchgruppen.AddOrUpdate(SelectedSpruchgruppe);
                userDBContext.SaveChanges();
                SpruchgruppenMembers.Add(SelectedSenatMember);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Der Richter konnte nicht hinzugefügt werden. Es ist folgender Fehler aufgetreten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        #endregion

    }
}
