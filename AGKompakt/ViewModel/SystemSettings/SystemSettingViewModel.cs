using BGH_Kompakt.Classes.Helper;
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

        //Aktenzeichen

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




        //private ListCollectionView _AktenzeichenListView;
        //public ListCollectionView AktenzeichenListView
        //{
        //    get { return _AktenzeichenListView; }
        //}


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
            try
            {

            }
            catch (Exception ex) { ErrorMessage.CreateExceptionWithoutMessage("SystemSettingViewModel", ex); }
        }


        //executes
        #region Executes

        #endregion

    }
}
