using BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps;
using BGH_Kompakt.Classes.ActivityRequestClasses;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.ActivityRequestService;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.Interfaces;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.Views;
using BGH_Kompakt.Views.UserLogin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel
{
    public partial class NebentaetigkeitenAnzeigeViewModel : ViewModelBase, IFilesDropped
    {
        //#region Title
        //public ObservableCollection<ItemPresenter> Items { get; } = new ObservableCollection<ItemPresenter>();
        //public ItemPresenter SelectedItem { get; set; }
        //public int SelectedIndex { get; set; } = 0;
        //#endregion
        const string Assurance = "Ich versichere, dass die Angaben vollständig sind und ich nachträglich eintretende Änderungen hinsichtlich meiner Angaben unverzüglich anzeigen werde. \n";
        const string ChangePermissionApproval = "Sollte die angezeigte Tätigkeit gemäß §§ 99, 100 BBG, § 46 DRiG genehmigungspflichtig sein, bitte ich die Anzeige als Genehmigungsantrag zu behandeln.";
        const string ChangePermissionNotice = "Sollte die zur Genehmigung beantragte Tätigkeit nicht gemäß §§ 99, 100 BBG, § 46 DRiG genehmigungspflichtig sein, bitte ich den Genehmigungsantrag als Anzeige zu behandeln.";
        readonly ActivityRequestDBContext activityRequestDBcontext = new ActivityRequestDBContext();
        #region ObvervableCollections/SelectedItems
        private readonly ObservableCollection<ActivityRequestDataFile> _ImportFileList = new ObservableCollection<ActivityRequestDataFile>();
        public ObservableCollection<ActivityRequestDataFile> ImportFileList { get { return _ImportFileList; } }
        public ObservableCollection<ActivityClient> Items { get; set; }
        public CollectionViewSource CollectionView { get; set; }
        public ObservableCollection<ActivityClient> Clientlist { get; set; }

        private ActivityClient selectedClient;
        public ActivityClient SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                ErrorClient = false;
            }
        }
        public int SelectedIndexClient { get; set; } = 0;
        private readonly ObservableCollection<ActivityRequestTyp> _RequestTypList = new ObservableCollection<ActivityRequestTyp>();
        public ObservableCollection<ActivityRequestTyp> RequestTyplist { get { return _RequestTypList; } }
        private ActivityRequestTyp _selectedRequestTyp;
        public ActivityRequestTyp SelectedRequestTyp
        {
            get { return _selectedRequestTyp; }
            set
            {
                SetProperty<ActivityRequestTyp>(ref _selectedRequestTyp, value);
                ErrorRequestTyp = false;
                if (_selectedRequestTyp != null) ShowContentRequestTyp();
            }
        }
        public int SelectedIndextRequestTyp { get; set; } = 0;
        private readonly ObservableCollection<ActivityClientTyp> _ClientTypList = new ObservableCollection<ActivityClientTyp>();
        public ObservableCollection<ActivityClientTyp> ClientTyplist { get { return _ClientTypList; } }
        private ActivityClientTyp selectedClientTyp;
        public ActivityClientTyp SelectedClientTyp
        {
            get { return selectedClientTyp; }
            set
            {
                selectedClientTyp = value;
                ErrorClient = false;
            }
        }
        private readonly ObservableCollection<ARVerguetungAdventage> _AdventageList = new ObservableCollection<ARVerguetungAdventage>();
        public ObservableCollection<ARVerguetungAdventage> AdventageList { get { return _AdventageList; } }
        private ARVerguetungAdventage selectedAdventage;
        public ARVerguetungAdventage SelectedAdventage
        {
            get { return selectedAdventage; }
            set { selectedAdventage = value; }
        }
        private ARVerguetungAdventage _selectedAdventageListView;
        public ARVerguetungAdventage SelectedAdventageListView
        {
            get
            {
                return _selectedAdventageListView;
            }
            set { _selectedAdventageListView = value; }
        }
        private readonly ObservableCollection<ActivityRequestArbitrationClient> _ArbitrationClientList = new ObservableCollection<ActivityRequestArbitrationClient>();
        public ObservableCollection<ActivityRequestArbitrationClient> ArbitrationClientList { get { return _ArbitrationClientList; } }
        private ActivityRequestArbitrationClient _selectedArbitrationClient;
        public ActivityRequestArbitrationClient SelectedArbitrationClient
        {
            get { return _selectedArbitrationClient; }
            set { _selectedArbitrationClient = value; }
        }
        private readonly ObservableCollection<ARVerguetungAdventageTyp> _AdventageTypList = new ObservableCollection<ARVerguetungAdventageTyp>();
        public ObservableCollection<ARVerguetungAdventageTyp> AdventageTypList { get { return _AdventageTypList; } }
        private ARVerguetungAdventageTyp selectedAdventageTyp;
        public ARVerguetungAdventageTyp SelectedAdventageTyp
        {
            get { return selectedAdventageTyp; }
            set { 
                selectedAdventageTyp = value;
                if (value != null) ErrorAdventageTyp = false;
            }
        }
        private readonly ObservableCollection<ScienceAuthor> _ScienceAuthorList = new ObservableCollection<ScienceAuthor>();
        public ObservableCollection<ScienceAuthor> ScienceAuthorList { get { return _ScienceAuthorList; } }
        private readonly ObservableCollection<ActivityRequestScienceTyp> _ScienceTypList = new ObservableCollection<ActivityRequestScienceTyp>();
        public ObservableCollection<ActivityRequestScienceTyp> ScienceTypList { get { return _ScienceTypList; } }
        private ActivityRequestScienceTyp _selectedScienceTyp;
        public ActivityRequestScienceTyp SelectedScienceTyp
        {
            get { return _selectedScienceTyp; }
            set
            {
                _selectedScienceTyp = value;
                ErrorScienceTyp = false;
            }
        }
        public ObservableCollection<User> ApplicantList { get; set; } = new ObservableCollection<User>();
        private User _ApplicantSelected;
        public User ApplicantSelected
        {
            get { return _ApplicantSelected; }
            set { _ApplicantSelected = value; }
        }
        private ActivityRequestDataFile _selectedAttechment;
        public ActivityRequestDataFile SelectedAttechment
        {
            get { return _selectedAttechment; }
            set { _selectedAttechment = value; }
        }
        #endregion
        #region Eventhandler
        public event EventHandler OnClientClick;
        #endregion
        #region System
        private readonly string _Introduction;
        //private readonly string _titel;
        //public string Titel { get { return _titel; } }
        public string Introduction { get { return _Introduction; } }
        public bool Startprozedur { get; set; }
        public bool ErrorCRUD { get; set; } = false;
        public string SaveLabel { get; set; } = string.Empty;
        private bool setRequestDate = false;
        #endregion
        #region Show/Anzeige
        private bool _ShowClientTyp = false;
        public bool ShowClientTyp
        {
            get => _ShowClientTyp;
            set { SetProperty<bool>(ref _ShowClientTyp, value); }
        }
        private bool _ShowClient = false;
        public bool ShowClient
        {
            get => _ShowClient;
            set { SetProperty<bool>(ref _ShowClient, value); }
        }
        private bool _ShowAdventage = false;
        public bool ShowAdventage
        {
            get => _ShowAdventage;
            set { SetProperty<bool>(ref _ShowAdventage, value); }
        }
        private bool _ShowArbitrationClientAdd = false;
        public bool ShowArbitrationClientAdd
        {
            get => _ShowArbitrationClientAdd;
            set { SetProperty<bool>(ref _ShowArbitrationClientAdd, value); }
        }
        private bool _ShowActivityRequest = true;
        public bool ShowActivityRequest
        {
            get => _ShowActivityRequest;
            set { SetProperty<bool>(ref _ShowActivityRequest, value); }
        }
        private bool _ShowClear = true;
        public bool ShowClear
        {
            get => _ShowClear;
            set { SetProperty<bool>(ref _ShowClear, value); }
        }
        private bool _ShowVermerk = true;
        public bool ShowVermerk
        {
            get => _ShowVermerk;
            set { SetProperty<bool>(ref _ShowVermerk, value); }
        }
        private bool _ShowHinweis = true;
        public bool ShowHinweis
        {
            get => _ShowHinweis;
            set { SetProperty<bool>(ref _ShowHinweis, value); }
        }
        private bool _anzeigeGesamt = false;
        public bool AnzeigeGesamt
        {
            get { return _anzeigeGesamt; }
            set { SetProperty<bool>(ref _anzeigeGesamt, value); }
        }
        private bool _anzeigeVortragstaetigkeit = false;
        public bool AnzeigeVortragstaetigkeit
        {
            get { return _anzeigeVortragstaetigkeit; }
            set { SetProperty<bool>(ref _anzeigeVortragstaetigkeit, value); }
        }
        private bool _anzeigeDate = false;
        public bool AnzeigeDate
        {
            get { return _anzeigeDate; }
            set { SetProperty<bool>(ref _anzeigeDate, value); }
        }
        private bool _anzeigeScience = false;
        public bool AnzeigeScience
        {
            get { return _anzeigeScience; }
            set { SetProperty<bool>(ref _anzeigeScience, value); }
        }
        private bool _anzeigeScienceTyp = false;
        public bool AnzeigeScienceTyp
        {
            get { return _anzeigeScienceTyp; }
            set { SetProperty<bool>(ref _anzeigeScienceTyp, value); }
        }
        private bool _anzeigeScienceAuthor = false;
        public bool AnzeigeScienceAuthor
        {
            get { return _anzeigeScienceAuthor; }
            set { SetProperty<bool>(ref _anzeigeScienceAuthor, value); }
        }
        private bool _anzeigeExam = false;
        public bool AnzeigeExam
        {
            get { return _anzeigeExam; }
            set { SetProperty<bool>(ref _anzeigeExam, value); }
        }
        private bool _anzeigeDetails = false;
        public bool AnzeigeDetails
        {
            get { return _anzeigeDetails; }
            set { SetProperty<bool>(ref _anzeigeDetails, value); }
        }
        private bool _anzeigeScienceArt = false;
        public bool AnzeigeScienceArt
        {
            get { return _anzeigeScienceArt; }
            set { SetProperty<bool>(ref _anzeigeScienceArt, value); }
        }
        private bool _anzeigeDetailUnter = false;
        public bool AnzeigeDetailUnter
        {
            get { return _anzeigeDetailUnter; }
            set { SetProperty<bool>(ref _anzeigeDetailUnter, value); }
        }
        private bool _anzeigeSonstiges = false;
        public bool AnzeigeSonstiges
        {
            get { return _anzeigeSonstiges; }
            set { SetProperty<bool>(ref _anzeigeSonstiges, value); }
        }
        private bool _anzeigeDateRadioButton = false;
        public bool AnzeigeDateRadioButton
        {
            get { return _anzeigeDateRadioButton; }
            set { SetProperty<bool>(ref _anzeigeDateRadioButton, value); }
        }
        private bool _anzeigeDateOnce = false;
        public bool AnzeigeDateOnce
        {
            get { return _anzeigeDateOnce; }
            set { SetProperty<bool>(ref _anzeigeDateOnce, value); }
        }
        private bool _anzeigeDatePermanent = false;
        public bool AnzeigeDatePermanent
        {
            get { return _anzeigeDatePermanent; }
            set { SetProperty<bool>(ref _anzeigeDatePermanent, value); }
        }
        private bool _anzeigeArbitration = false;
        public bool AnzeigeArbitration
        {
            get { return _anzeigeArbitration; }
            set { SetProperty<bool>(ref _anzeigeArbitration, value); }
        }
        private bool _anzeigeArbitrationParties = false;
        public bool AnzeigeArbitrationParties
        {
            get { return _anzeigeArbitrationParties; }
            set { SetProperty<bool>(ref _anzeigeArbitrationParties, value); }
        }
        private bool _anzeigeAdventageList = false;
        public bool AnzeigeAdventageList
        {
            get { return _anzeigeAdventageList; }
            set { SetProperty<bool>(ref _anzeigeAdventageList, value); }
        }
        private bool _anzeigeArbitrationClientList = false;
        public bool AnzeigeArbitrationClientList
        {
            get { return _anzeigeArbitrationClientList; }
            set { SetProperty<bool>(ref _anzeigeArbitrationClientList, value); }
        }
        //RequestTyp
        private bool _anzeigepflichtig;
        public bool Anzeigepflichtig
        {
            get { return _anzeigepflichtig; }
            set
            {
                SetProperty<bool>(ref _anzeigepflichtig, value);
                ErrorRequestMeldeArt = false;
                AnzeigeGesamt = true;
                AnzeigeDetails = false;
                AnzeigeSonstiges = false;

                if (value) Instruction = $"{Assurance}{ChangePermissionApproval}";
                if (_anzeigepflichtig && !Startprozedur)
                {
                    ActivityRequestDBContext activityRequestDBcontext = new ActivityRequestDBContext();
                    var RequestTyp_Query = activityRequestDBcontext.ActivityRequestTyps.Where(t => t.ActivityRequestTypMeldeArt == 1);
                    _RequestTypList.Clear();
                    foreach (var item in RequestTyp_Query) _RequestTypList.Add(item);
                }
            }
        }
        private bool _genehmigungspflichtig;
        public bool Genehmigungspflichtig
        {
            get { return _genehmigungspflichtig; }
            set
            {
                SetProperty<bool>(ref _genehmigungspflichtig, value);
                ErrorRequestMeldeArt = false;
                AnzeigeGesamt = true;
                AnzeigeDetails = false;
                AnzeigeSonstiges = false;

                if (value) Instruction = $"{Assurance}{ChangePermissionNotice}";
                if (_genehmigungspflichtig && !Startprozedur)
                {
                    ActivityRequestDBContext activityRequestDBcontext = new ActivityRequestDBContext();
                    var RequestTyp_Query = activityRequestDBcontext.ActivityRequestTyps.Where(t => t.ActivityRequestTypMeldeArt == 2);
                    _RequestTypList.Clear();
                    foreach (var item in RequestTyp_Query) _RequestTypList.Add(item);
                }

            }
        }
        private string _Instruction = "Bitte wählen Sie eine Antragsart aus.";
        public string Instruction
        {
            get => _Instruction;
            set { SetProperty(ref _Instruction, value); }
        }
        private bool _AnzeigeOrt = false;
        public bool AnzeigeOrt
        {
            get { return _AnzeigeOrt; }
            set { SetProperty<bool>(ref _AnzeigeOrt, value); }
        }
        private bool _Online = true;
        public bool Online
        {
            get { return _Online; }
            set
            {
                SetProperty<bool>(ref _Online, value);
                if (_Online)
                {
                    AnzeigeOrt = false;
                    ErrorOrtArt = false;
                }
            }
        }
        private bool _Presence = true;
        public bool Presence
        {
            get { return _Presence; }
            set
            {
                SetProperty<bool>(ref _Presence, value);
                if (_Presence)
                {
                    ErrorOrtArt = false;
                    AnzeigeOrt = true;
                }
            }
        }
        //ScienceCategories
        private bool _AnzeigePublication = false;
        public bool AnzeigePublication
        {
            get { return _AnzeigePublication; }
            set { SetProperty<bool>(ref _AnzeigePublication, value); }
        }
        private bool _Publication = false;
        public bool Publication
        {
            get { return _Publication; }
            set
            {
                SetProperty<bool>(ref _Publication, value);
                if (_Publication)
                {
                    ErrorScienceCategory = false;
                    AnzeigePublication = true;
                    AnzeigeEducation = false;
                    ErrorScienceCategory = false;
                    AnzeigeDetailUnter = true;
                    Hint_Client = "Verlag/Zeitschrift";
                    AnzeigeScienceTyp = true;
                    Hint_RequestTitel = "Titel der Veröffentlichung";
                    AnzeigeVortragstaetigkeit = false;
                    AnzeigeDate = false;
                    AnzeigeScienceAuthor = true;
                    AnzeigeDateRadioButton = false;
                }

            }
        }
        private bool _AnzeigeEducation = false;
        public bool AnzeigeEducation
        {
            get { return _AnzeigeEducation; }
            set { SetProperty<bool>(ref _AnzeigeEducation, value); }
        }
        private bool _Education = false;
        public bool Education
        {
            get { return _Education; }
            set
            {
                SetProperty<bool>(ref _Education, value);
                if (_Education)
                {
                    ErrorScienceCategory = false;
                    AnzeigeEducation = true;
                    AnzeigeDateRadioButton = true;
                    AnzeigePublication = false;
                    ErrorScienceCategory = false;
                    AnzeigeDetailUnter = true;
                    Hint_Client = "Hochschule";
                    AnzeigeScienceTyp = false;
                    Hint_RequestTitel = "Titel der Veranstaltung";
                    AnzeigeVortragstaetigkeit = false;
                    AnzeigeDate = true;
                    AnzeigeDateOnce = false;
                    AnzeigeDatePermanent = false;
                    AnzeigeScienceAuthor = false;
                }
            }
        }
        private bool _Permanent = false;
        public bool Permanent
        {
            get { return _Permanent; }
            set
            {
                SetProperty<bool>(ref _Permanent, value);
                if (_Permanent)
                {
                    AnzeigeDatePermanent = true;
                    AnzeigeDateOnce = false;
                    ErrorDuration = false;
                    AnzeigeHinweisEinsatz = true;
                }
            }
        }
        private bool _Once = false;
        public bool Once
        {
            get { return _Once; }
            set
            {
                SetProperty<bool>(ref _Once, value);
                if (_Once)
                {
                    AnzeigeDatePermanent = false;
                    AnzeigeDateOnce = true;
                    ErrorDuration = false;
                    AnzeigeHinweisEinsatz = false;
                }
            }
        }
        private bool _Anzeige_Art = true;
        public bool Anzeige_Art
        {
            get { return _Anzeige_Art; }
            set
            {
                SetProperty<bool>(ref _Anzeige_Art, value);
                if (_Anzeige_Art)
                {
                    SetAnzeigeExpander(art: true);
                    //HinweisText = SetHinweistext(1);
                }
            }
        }
        private bool _Anzeige_Verguetung = false;
        public bool Anzeige_Verguetung
        {
            get { return _Anzeige_Verguetung; }
            set
            {
                SetProperty<bool>(ref _Anzeige_Verguetung, value);
                if (_Anzeige_Verguetung)
                {
                    SetAnzeigeExpander(verguetung: true);
                    //HinweisText = SetHinweistext(2);
                }
            }
        }
        private bool _Anzeige_Zeitaufwand = false;
        public bool Anzeige_Zeitaufwand
        {
            get { return _Anzeige_Zeitaufwand; }
            set
            {
                SetProperty<bool>(ref _Anzeige_Zeitaufwand, value);
                if (_Anzeige_Zeitaufwand)
                {
                    SetAnzeigeExpander(zeitaufwand: true);
                    //HinweisText = SetHinweistext(3);
                }
            }
        }
        private bool _Anzeige_Anlagen = false;
        public bool Anzeige_Anlagen
        {
            get { return _Anzeige_Anlagen; }
            set
            {
                SetProperty<bool>(ref _Anzeige_Anlagen, value);
                if (_Anzeige_Anlagen)
                {
                    SetAnzeigeExpander(anlagen: true);
                    //HinweisText = SetHinweistext(4);
                }
            }
        }
        private void SetAnzeigeExpander(bool art = false, bool verguetung = false, bool zeitaufwand = false, bool anlagen = false)
        {
            if (!art) Anzeige_Art = art;
            if (!verguetung) Anzeige_Verguetung = verguetung;
            if (!zeitaufwand) Anzeige_Zeitaufwand = zeitaufwand;
            if (!anlagen) Anzeige_Anlagen = anlagen;
        }
        private bool _Party = false;
        public bool Party
        {
            get { return _Party; }
            set
            {
                SetProperty<bool>(ref _Party, value);
                if (_Party)
                {
                    AnzeigeArbitrationParties = false;
                }
            }
        }
        private bool _Third = false;
        public bool Third
        {
            get { return _Third; }
            set
            {
                SetProperty<bool>(ref _Third, value);
                if (_Third)
                {
                    AnzeigeArbitrationParties = true;
                }
            }
        }
        private bool _ShowApplicant = false;
        public bool ShowApplicant
        {
            get { return _ShowApplicant; }
            set { SetProperty(ref _ShowApplicant, value); }
        }
        private bool _ShowAttechmentList = false;
        public bool ShowAttechmentList
        {
            get { return _ShowAttechmentList; }
            set { SetProperty(ref _ShowAttechmentList, value); }
        }
        private bool _PaymentPredicted = true;
        public bool PaymentPredicted
        {
            get { return _PaymentPredicted; }
            set 
            {
                SetProperty<bool>(ref _PaymentPredicted, value);
                if (_PaymentPredicted)
                {
                    PaymentNothing = false;
                    PaymentUnknown = false;
                    ErrorPaymentTyp = false;
                }
            }
        }
        private bool _PaymentNothing = false;
        public bool PaymentNothing
        {
            get { return _PaymentNothing; }
            set
            {
                SetProperty<bool>(ref _PaymentNothing, value);
                if (_PaymentNothing)
                {
                    PaymentPredicted = false;
                    PaymentUnknown = false;
                    ErrorPaymentTyp = false;
                }
            }
        }
        private bool _PaymentUnknown = false;
        public bool PaymentUnknown
        {
            get { return _PaymentUnknown; }
            set
            {
                SetProperty<bool>(ref _PaymentUnknown, value);
                if (_PaymentUnknown)
                {
                    PaymentPredicted = false;
                    PaymentNothing = false;
                    ErrorPaymentTyp = false;
                }
            }
        }
        private bool _HoursPredicted = true;
        public bool HoursPredicted
        {
            get { return _HoursPredicted; }
            set
            {
                SetProperty<bool>(ref _HoursPredicted, value);
                if (_HoursPredicted)
                {
                    HoursUnknown = false;
                    ErrorHourTyp = false;
                }
            }
        }
        private bool _HoursUnknown = false;
        public bool HoursUnknown
        {
            get { return _HoursUnknown; }
            set
            {
                SetProperty<bool>(ref _HoursUnknown, value);
                if (_HoursUnknown)
                {
                    HoursPredicted = false;
                    ErrorHourTyp = false;
                }
            }
        }
        private bool _AnzeigeHinweisEinsatz = false;
        public bool AnzeigeHinweisEinsatz
        {
            get { return _AnzeigeHinweisEinsatz; }
            set { SetProperty<bool>(ref _AnzeigeHinweisEinsatz, value);}
        }
        private bool _ShowChanges = false;
        public bool ShowChanges
        {
            get { return _ShowChanges; }
            set { SetProperty<bool>(ref _ShowChanges, value);}
        }


        #endregion
        #region ErrorMessages
        private bool _ErrorRequestTyp = false;
        public bool ErrorRequestTyp
        {
            get { return _ErrorRequestTyp; }
            set { SetProperty<bool>(ref _ErrorRequestTyp, value); }
        }
        private bool _ErrorRequestMeldeArt = false;
        public bool ErrorRequestMeldeArt
        {
            get { return _ErrorRequestMeldeArt; }
            set { SetProperty<bool>(ref _ErrorRequestMeldeArt, value); }
        }
        private bool _ErrorScienceCategory = false;
        public bool ErrorScienceCategory
        {
            get { return _ErrorScienceCategory; }
            set { SetProperty<bool>(ref _ErrorScienceCategory, value); }
        }
        private bool _ErrorTitel = false;
        public bool ErrorTitel
        {
            get { return _ErrorTitel; }
            set { SetProperty<bool>(ref _ErrorTitel, value); }
        }
        private bool _ErrorClient = false;
        public bool ErrorClient
        {
            get { return _ErrorClient; }
            set { SetProperty<bool>(ref _ErrorClient, value); }
        }
        private bool _ErrorOrt = false;
        public bool ErrorOrt
        {
            get { return _ErrorOrt; }
            set { SetProperty<bool>(ref _ErrorOrt, value); }
        }
        private bool _ErrorOrtArt = false;
        public bool ErrorOrtArt
        {
            get { return _ErrorOrtArt; }
            set { SetProperty<bool>(ref _ErrorOrtArt, value); }
        }
        private bool _ErrorDate = false;
        public bool ErrorDate
        {
            get { return _ErrorDate; }
            set { SetProperty<bool>(ref _ErrorDate, value); }
        }
        private bool _ErrorScienceTyp = false;
        public bool ErrorScienceTyp
        {
            get { return _ErrorScienceTyp; }
            set { SetProperty<bool>(ref _ErrorScienceTyp, value); }
        }
        private bool _ErrorScienceAuthor = false;
        public bool ErrorScienceAuthor
        {
            get { return _ErrorScienceAuthor; }
            set { SetProperty<bool>(ref _ErrorScienceAuthor, value); }
        }
        private bool _ErrorDatePermanentFrom = false;
        public bool ErrorDatePermanentFrom
        {
            get { return _ErrorDatePermanentFrom; }
            set { SetProperty<bool>(ref _ErrorDatePermanentFrom, value); }
        }
        private bool _ErrorDatePermanentUntil = false;
        public bool ErrorDatePermanentUntil
        {
            get { return _ErrorDatePermanentUntil; }
            set { SetProperty<bool>(ref _ErrorDatePermanentUntil, value); }
        }
        private bool _ErrorDatePermanentDuration = false;
        public bool ErrorDatePermanentDuration
        {
            get { return _ErrorDatePermanentDuration; }
            set { SetProperty<bool>(ref _ErrorDatePermanentDuration, value); }
        }
        private bool _ErrorDuration = false;
        public bool ErrorDuration
        {
            get { return _ErrorDuration; }
            set { SetProperty<bool>(ref _ErrorDuration, value); }
        }
        private bool _ErrorArbitrationCLient = false;
        public bool ErrorArbitrationCLient
        {
            get { return _ErrorArbitrationCLient; }
            set { SetProperty<bool>(ref _ErrorArbitrationCLient, value); }
        }
        private bool _ErrorArbitrationTyp = false;
        public bool ErrorArbitrationTyp
        {
            get { return _ErrorArbitrationTyp; }
            set { SetProperty<bool>(ref _ErrorArbitrationTyp, value); }
        }
        private bool _ErrorArbitrationCaller = false;
        public bool ErrorArbitrationCaller
        {
            get { return _ErrorArbitrationCaller; }
            set { SetProperty<bool>(ref _ErrorArbitrationCaller, value); }
        }
        private bool _ErroVerguetung = false;
        public bool ErrorVerguetung
        {
            get { return _ErroVerguetung; }
            set { SetProperty<bool>(ref _ErroVerguetung, value); }
        }
        private bool _ErrorPaymentTyp = false;
        public bool ErrorPaymentTyp
        {
            get { return _ErrorPaymentTyp; }
            set { SetProperty<bool>(ref _ErrorPaymentTyp, value); }
        }
        private bool _ErrorHoursMain = false;
        public bool ErrorHoursMain
        {
            get { return _ErrorHoursMain; }
            set { SetProperty<bool>(ref _ErrorHoursMain, value); }
        }
        private string _ErrorHoursMainText = "Bitte tragen Sie eine ganze Stundenzahl ein.";
        public string ErrorHoursMainText
        {
            get { return _ErrorHoursMainText; }
            set { SetProperty(ref _ErrorHoursMainText, value); }
        }
        private bool _ErrorHoursPrep = false;
        public bool ErrorHoursPrep
        {
            get { return _ErrorHoursPrep; }
            set { SetProperty<bool>(ref _ErrorHoursPrep, value); }
        }
        private bool _ErrorInstruction = false;
        public bool ErrorInstruction
        {
            get { return _ErrorInstruction; }
            set { SetProperty<bool>(ref _ErrorInstruction, value); }
        }
        private bool _ErrorAdventageTyp = false;
        public bool ErrorAdventageTyp
        {
            get { return _ErrorAdventageTyp; }
            set { SetProperty<bool>(ref _ErrorAdventageTyp, value); }
        }
        private bool _ErrorAdentageAmount = false;
        public bool ErrorAdventageAmount
        {
            get { return _ErrorAdentageAmount; }
            set { SetProperty<bool>(ref _ErrorAdentageAmount, value); }
        }
        private bool _ErrorHourTyp = false;
        public bool ErrorHourTyp
        {
            get { return _ErrorHourTyp; }
            set { SetProperty<bool>(ref _ErrorHourTyp, value); }
        }
        #endregion
        #region Values
        private int RequestUserID;
        private string _requestTitel;
        public string RequestTitel
        {
            get { return _requestTitel; }
            set
            {
                ErrorTitel = false;
                SetProperty<string>(ref _requestTitel, value);
            }
        }
        private string _requestClientName;
        public string RequestClientName
        {
            get { return _requestClientName; }
            set
            {
                ErrorTitel = false;
                SetProperty<string>(ref _requestClientName, value);
            }
        }
        private string _responseOrt;
        public string RequestOrt
        {
            get { return _responseOrt; }
            set
            {
                ErrorOrt = false;
                SetProperty<string>(ref _responseOrt, value);
            }
        }
        private DateTime? _RequestDate;
        public DateTime? RequestDate
        {
            get { return _RequestDate; }
            set
            {
                ErrorDate = false;
                if (setRequestDate)
                {
                    SetProperty<DateTime?>(ref _RequestDate, value);
                    setRequestDate = false;
                }

            }
        }
        private DateTime? _RequestDatePermanentFrom;
        public DateTime? RequestDatePermanentFrom
        {
            get { return _RequestDatePermanentFrom; }
            set
            {
                SetProperty<DateTime?>(ref _RequestDatePermanentFrom, value);
                ErrorDatePermanentFrom = false;
            }
        }
        private DateTime? _RequestDatePermanentUntil;
        public DateTime? RequestDatePermanentUntil
        {
            get { return _RequestDatePermanentUntil; }
            set
            {
                SetProperty<DateTime?>(ref _RequestDatePermanentUntil, value);
                ErrorDatePermanentUntil = false;
            }
        }
        private int _RequestDatePermanentDuration;
        public int RequestDatePermanentDuration
        {
            get { return _RequestDatePermanentDuration; }
            set
            {
                SetProperty<int>(ref _RequestDatePermanentDuration, value);
                ErrorDatePermanentDuration = false;
            }
        }
        private string _AdventageAmount;
        public string AdventageAmount
        {
            get { return _AdventageAmount; }
            set {  
                SetProperty<string>(ref _AdventageAmount, value);
                if (value != null) ErrorAdventageAmount = false;
            }
        }
        private decimal? _Verguetung;
        public decimal? Verguetung
        {
            get { return _Verguetung; }
            set
            {
                SetProperty<decimal?>(ref _Verguetung, value);
                ErrorVerguetung = false;
            }
        }
        private Single? _HoursMain;
        public Single? HoursMain
        {
            get { return _HoursMain; }
            set
            {
                SetProperty<Single?>(ref _HoursMain, value);
                ErrorHoursMain = false;
            }
        }
        private Single? _HoursPrep;
        public Single? HoursPrep
        {
            get { return _HoursPrep; }
            set
            {
                SetProperty<Single?>(ref _HoursPrep, value);
                ErrorHoursPrep = false;
            }
        }
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetProperty<string>(ref _Description, value); }
        }
        private string _ActivityRequestArbitrationClientText;
        public string ActivityRequestArbitrationClientText
        {
            get { return _ActivityRequestArbitrationClientText; }
            set { SetProperty<string>(ref _ActivityRequestArbitrationClientText, value); }
        }
        private string _ActivityRequestArbitrationCaller;
        public string ActivityRequestArbitrationCaller
        {
            get { return _ActivityRequestArbitrationCaller; }
            set
            {
                SetProperty<string>(ref _ActivityRequestArbitrationCaller, value);
                ErrorArbitrationCaller = false;
            }
        }
        private string _ARNoteAdmin;
        public string ARNoteAdmin
        {
            get { return _ARNoteAdmin; }
            set { SetProperty(ref _ARNoteAdmin, value); }
        }
        private bool _ARAssurance;
        public bool ARAssurance
        {
            get { return _ARAssurance; }
            set { 
                SetProperty(ref _ARAssurance, value);
                ErrorInstruction = false;
            }
        }
        private string _HinweisText;
        public string HinweisText
        {
            get { return _HinweisText; }
            set { SetProperty(ref _HinweisText, value); }
        }
        #endregion
        #region Hinttexte
        private string _hint_Client = "Auftraggeber";
        public string Hint_Client
        {
            get { return _hint_Client; }
            set { SetProperty<string>(ref _hint_Client, value); }
        }

        private string _hint_RequestTitel = "Tätigkeitsbeschreibung";
        public string Hint_RequestTitel
        {
            get { return _hint_RequestTitel; }
            set { SetProperty<string>(ref _hint_RequestTitel, value); }
        }

        private string _hint_Description = "Tätigkeitsbeschreibung";
        public string Hint_Description
        {
            get { return _hint_Description; }
            set { SetProperty<string>(ref _hint_Description, value); }
        }
        #endregion
        #region ICommands
        public ICommand QuitCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand ClientCommand { get; set; }
        public ICommand ClientApplyCommand { get; set; }
        public ICommand ClientBackCommand { get; set; }
        public ICommand ExpanderCommand { get; set; }
        public ICommand AdventageCommand { get; set; }
        public ICommand AdventageApplyCommand { get; set; }
        public ICommand AdventageClearCommand { get; set; }
        public ICommand ArbitrationClientAddApplyCommand { get; set; }
        public ICommand ArbitrationClientAddCommand { get; set; }
        public ICommand ArbitrationClientAddClearCommand { get; set; }
        public ICommand NewCommand { get; set; }
        public ICommand AddPersonCommand { get; set; }
        public ICommand ImportListClearCommand { get; set; }
        public ICommand OpenAttechmentCommand { get; set; }
        public ICommand DeleteAttechmentCommand { get; set; }
        public ICommand AddRequestsCommand { get; set; }
        public ICommand ScienceAuthorListSelectedCommand { get; set; }
        public ICommand HyperLinkCommand { get; set; }
        #endregion
        public NebentaetigkeitenAnzeigeViewModel()
        {
            //Testrichter_Fill();
            //Geschlechter_Fill();
            SaveLabel = ActivityRequestManager.ActionType == 1 ? "Eintragen" : "Änderung speichern";
            ShowClear = ActivityRequestManager.ActionType == 1;
            ShowVermerk = ActivityRequestManager.ActionType == 2 && ActivityRequestManager.LoginType == 2;
            ShowHinweis = !ShowVermerk;
            _Introduction = "Bitte füllen Sie das Formular vollständig aus.";
            SetExecutes();
            Startprozedur = true;
            //RadioButtonCommand = new RelayCommand(RadioButtonExecute);
            Fill_ComboBoxes();

            if (ActivityRequestManager.SelectedActivityRequest != null)
            {
                SetActivityRequest(ActivityRequestManager.SelectedActivityRequest);
            }
            else
            {
                SetActivityRequest();
            }
            Startprozedur = false;
            ShowApplicant = ActivityRequestManager.LoginType == 2 && ActivityRequestManager.ActionType == 1;
            Anzeige_Art = true;
            ShowChanges = FuncShowChanges();

        }

        private bool FuncShowChanges()
        {
            if (ActivityRequestManager.SelectedActivityRequest == null) return true;
            if (ActivityRequestManager.LoginType == 1 && ActivityRequestManager.SelectedActivityRequest.ARZustaendigkeitsbereich > 1) return false;
            return true;
        }

        private void SetExecutes()
        {
            QuitCommand = new RelayCommand(QuitExecute);
            ApplyCommand = new RelayCommand(ApplyExecute, ApplyCanExecute);
            ClientCommand = new RelayCommand(ClientExecute);
            ClientApplyCommand = new RelayCommand(ClientApplyExecute);
            ClientBackCommand = new RelayCommand(ClientBackExecute);
            ExpanderCommand = new RelayCommand(ExpanderExecute);
            AdventageCommand = new RelayCommand(AdventageExecute);
            AdventageApplyCommand = new RelayCommand(AdventageApplyExecute);
            AdventageClearCommand = new RelayCommand(AdventageClearExecute);
            ArbitrationClientAddApplyCommand = new RelayCommand(ArbitrationClientAddApplyExecute);
            ArbitrationClientAddCommand = new RelayCommand(ArbitrationClientAddExecute);
            ImportListClearCommand = new RelayCommand(ImportListClearExecute, ImportListClearCanExecute);
            OpenAttechmentCommand = new RelayCommand(OpenAttechmentExecute);
            DeleteAttechmentCommand = new RelayCommand(DeleteAttechmentExecute);
            NewCommand = new RelayCommand(NewExecute);
            AddPersonCommand = new RelayCommand(AddPersonExecute);
            AddRequestsCommand = new RelayCommand(AddRequestExecute);
            ScienceAuthorListSelectedCommand = new RelayCommand(ScienceAuthorSelctionExecute);
            HyperLinkCommand = new RelayCommand(HyperLinkExecute);
        }

        private void Fill_ComboBoxes()
        {
            ActivityRequestDBContext activityRequestDBcontext = new ActivityRequestDBContext();
            //Client füllen
            try
            {
                //Client füllen
                //var Client_Query = activityRequestDBcontext.ActivityClients.OrderBy(ac => ac.ACName);
                //foreach (var item in Client_Query) _ClientList.Add(item);
                List<ActivityClient> clients = new List<ActivityClient>();
                ActivityRequestDBContext activityRequestDBContext = new ActivityRequestDBContext();
                var Client_Query = activityRequestDBContext.ActivityClients.Include(t => t.ActivityClientTyp).OrderBy(ac => ac.ActivityClientTypID).ThenBy(ac => ac.ACName);
                foreach (var item in Client_Query) clients.Add(item);
                Clientlist = new ObservableCollection<ActivityClient>(clients);
                var view = new CollectionViewSource();
                view.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                view.Source = Clientlist;
                CollectionView = view;
                //ClientTyps füllen
                var ClientTyps_Query = activityRequestDBcontext.ActivityClientTyps;
                foreach (var item in ClientTyps_Query) _ClientTypList.Add(item);
                //ReqestTyps füllen
                var RequestTyp_Query = activityRequestDBcontext.ActivityRequestTyps;
                foreach (var item in RequestTyp_Query) _RequestTypList.Add(item);
                //Adventage füllen
                //var Adventage_Query = activityRequestDBcontext.ActivityRequests.Include(x => x.ARVerguetungAdventages).to;
                //foreach (var item in Adventage_Query) _AdventageList.Add(item);
                //AdventageTyp füllen
                var AdventageTyp_Query = activityRequestDBcontext.ARVerguetungAdventageTyps;
                foreach (var item in AdventageTyp_Query) _AdventageTypList.Add(item);
                //ScienceTyps füllen
                var ScienceTyp_Query = activityRequestDBcontext.ActivityRequestScienceTyps;
                foreach (var item in ScienceTyp_Query) _ScienceTypList.Add(item);
                //user füllen
                UserDBContext userDBContext = new UserDBContext();
                var ApplicantQuery = userDBContext.Users
                                        .Where(u => u.PositionId == 1 || u.PositionId == 2)
                                        .Include(x => x.Position)
                                        .OrderBy(x => x.PositionId)
                                        .ThenBy(x => x.NachName)
                                        .ThenBy(x => x.VorName)
                                        .ToList();
                foreach (var item in ApplicantQuery) ApplicantList.Add(item);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Es ist beim Laden folgender Fehler aufgetreten: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #region Executes
        private void AdventageClearExecute(object obj)
        {
            bool confirm = ViewManager.ShowQuestionWindow("Möchten Sie den Eintrag endgültig löschen?", "Ja");
            if (confirm)
            {
                if (ActivityRequestManager.ActionType == 2)
                {
                    ARVerguetungAdventage deleteAdventage = activityRequestDBcontext.ARVerguetungAdventages.FirstOrDefault(a => a.ARVerguetungAdventageTypId == SelectedAdventageListView.ARVerguetungAdventageId);
                    if (deleteAdventage != null) activityRequestDBcontext.ARVerguetungAdventages.Remove(deleteAdventage);
                }
                AdventageList.Remove(SelectedAdventageListView);
                if (AdventageList.Count == 0) AnzeigeAdventageList = false;
            }
        }
        private void AdventageApplyExecute(object obj)
        {
            if (SelectedAdventageTyp == null)
            {
                ErrorAdventageTyp = true;
                return;
            }
            if (AdventageAmount == null || decimal.Parse(AdventageAmount) < 0)
            {
                ErrorAdventageAmount = true;
                return;
            }

            ActivityRequestDBContext activityRequestDBcontext = new ActivityRequestDBContext();
            ARVerguetungAdventageTyp addAdventage = activityRequestDBcontext.ARVerguetungAdventageTyps.FirstOrDefault(a => a.ARVerguetungAdventageTypId == selectedAdventageTyp.ARVerguetungAdventageTypId);
            ARVerguetungAdventage newAdventage = new ARVerguetungAdventage
            {               
                ARVerguetungAdventageTyp = addAdventage,
                ARVerguetungAdventageTypId = SelectedAdventageTyp.ARVerguetungAdventageTypId,
                ARVerguetungAdventageAmount = decimal.Parse(AdventageAmount)
            };
            AdventageList.Add(newAdventage);
            AdventageAmount = null;
            ShowAdventageConvert(false, true);
            AnzeigeAdventageList = true;
        }
        private void AdventageExecute(object obj) => ShowAdventageConvert(true, false);
        private void ArbitrationClientAddApplyExecute(object obj)
        {
            ActivityRequestArbitrationClient newClient = new ActivityRequestArbitrationClient
            {
                ActivityRequestArbitrationClientText = ActivityRequestArbitrationClientText,
                //ActivityRequest = activityRequest 
            };
            ArbitrationClientList.Add(newClient);
            ShowArbitrationClientConvert(false, true);
            AnzeigeArbitrationClientList = true;
        }
        private void ArbitrationClientAddExecute(object obj)
        {
            ErrorArbitrationCLient = false;
            ShowArbitrationClientConvert(true, false);
        }
        private void ExpanderExecute(object obj)
        {
            if (obj != null)
            {
                switch ((string)obj)
                {
                    case "VorArt":
                        Anzeige_Art = false;
                        Anzeige_Verguetung = true;
                        break;
                    case "ZurückVergütung":
                        Anzeige_Art = true;
                        Anzeige_Verguetung = false;
                        break;
                    case "VorVergütung":
                        Anzeige_Verguetung = false;
                        Anzeige_Zeitaufwand = true;
                        break;
                    case "ZurückZeitaufwand":
                        Anzeige_Verguetung = true;
                        Anzeige_Zeitaufwand = false;
                        break;
                    case "VorZeitaufwand":
                        Anzeige_Anlagen = true;
                        Anzeige_Zeitaufwand = false;
                        break;
                    case "ZurückAnlagen":
                        Anzeige_Zeitaufwand = true;
                        Anzeige_Anlagen = false;
                        break;
                }
            }
        }
        private void ClientApplyExecute(object obj)
        {
            if (selectedClientTyp == null)
            {
                ViewManager.ShowMainInfoFlyout("Bitte wählen Sie einen Typ aus.", false);
                return;
            }
            try
            {
                ActivityRequestDBContext activityRequestDBcontext = new ActivityRequestDBContext();
                ActivityClientTyp activityClient = activityRequestDBcontext.ActivityClientTyps.FirstOrDefault(c => c.ActivityClientTypId == SelectedClientTyp.ActivityClientTypId);
                ActivityClient newClient = new ActivityClient
                {
                    ACName = RequestClientName,
                    ActivityClientTypID = activityClient.ActivityClientTypId,
                    ActivityClientTyp = activityClient
                };
                activityRequestDBcontext.ActivityClients.Add(newClient);
                activityRequestDBcontext.SaveChanges();
                Clientlist.Add(newClient);
                RequestClientName = string.Empty;
                SelectedClientTyp = null;
                ShowClientConvert(false, true);
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Der Auftraggeber konnte nicht eingetragen werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
            }

        }
        private void ClientExecute(object obj) => ShowClientConvert(true, false);
        private void ClientBackExecute(object obj) => ShowClientConvert(false, true);
        private void QuitExecute(object obj)
        {
            ActivityRequestManager.DirectJump = true;
            if (ActivityRequestManager.LoginType == 2)
            {
                if (ActivityRequestManager.ListArt == 3) ViewManager.NebentaetigkeitenView.ArchivRequests.IsSelected = true;
                else ViewManager.NebentaetigkeitenView.OverviewOpenRequests.IsSelected = true;
            }
            else ViewManager.NebentaetigkeitenView.Overview.IsSelected = true;


        }
        private async void ApplyExecute(object obj)
        {
            string actionName = "Änderungen speichern";
            Task<DBResponse> task = SaveActivityRequest();
            ViewManager.ActionlistAdd(actionName);
            await task;
            ViewManager.ActionlistRemove(actionName);
            if (!task.Result.Success) { string message = task.Result.Message; ViewManager.ShowMainInfoFlyout(message, false); }
            else ViewManager.NebentaetigkeitenView.Overview.IsSelected = true;
        }
        private bool ApplyCanExecute(object obj) => FuncShowChanges();


        private Task<DBResponse> SaveActivityRequest()
        {
            Task<DBResponse> task = Task.Run<DBResponse>(() =>
            {
                DBResponse resp = new DBResponse();
                ErrorCRUD = false;
                Anzeige_Art = true;
                ActivityRequest newActivityRequest = new ActivityRequest();
                if (ActivityRequestManager.ActionType == 2)
                    newActivityRequest = activityRequestDBcontext.ActivityRequests.FirstOrDefault(x => x.ActivityRequestId == ActivityRequestManager.SelectedActivityRequest.ActivityRequestId);
                if (!Genehmigungspflichtig && !Anzeigepflichtig)
                {
                    ErrorRequestMeldeArt = true;
                    ErrorCRUD = true;
                    resp.Success = true;
                    return resp;
                }
                newActivityRequest.ActivityRequestMeldeArtID = Genehmigungspflichtig == true ? 2 : 1;
                newActivityRequest.ARDatum = DateTime.Now;
                if (SelectedRequestTyp != null)
                {
                    newActivityRequest.ActivityRequestTypID = SelectedRequestTyp.ActivityRequestTypId;
                }
                else
                {
                    ErrorRequestTyp = true;
                    ErrorCRUD = true;
                }
                if (SelectedRequestTyp != null)
                {
                    switch (SelectedRequestTyp.ActivityRequestTypId)
                    {
                        //Vortragstätigkeit/Referententätigkeit
                        case 1:
                        case 5:
                            ValidationTitel(ref newActivityRequest);
                            ValidationClient(ref newActivityRequest);
                            ValidationOrtArt(ref newActivityRequest);
                            if (Presence)
                            {
                                ValidationOrt(ref newActivityRequest);
                            }
                            ValidationDate(ref newActivityRequest);
                            break;
                        case 2: //Wissenschaftliche Tätigkeit
                            ValidationTitel(ref newActivityRequest);
                            ValidationClient(ref newActivityRequest);
                            ValidationScienceCategory(ref newActivityRequest);
                            if (Education)
                            {
                                ValidationDuration(ref newActivityRequest);
                                if (!ErrorCRUD)
                                {
                                    if (Once) //Wenn einmalig
                                    {
                                        ValidationDate(ref newActivityRequest);
                                    }
                                    else
                                    {
                                        ValidationDatePermanentFrom(ref newActivityRequest);
                                        ValidationDatePermanentUntil(ref newActivityRequest);
                                        ValidationDatePermanentDuration(ref newActivityRequest);
                                    }
                                }
                            }
                            else
                            {
                                ValidationScienceTyp(ref newActivityRequest);
                                //Art der Mitwirkung wird derzeit noch nicht geprüft
                                ValidationScienceAuthor(ref newActivityRequest);
                            }
                            break;
                        case 3: //schriftstellerische Tätigkeit
                            ValidationTitel(ref newActivityRequest);
                            break;
                        case 4: //künstlerische Tätigkeit
                            ValidationTitel(ref newActivityRequest);
                            break;
                        case 6: //Prüfertätigkeit
                            ValidationTitel(ref newActivityRequest);
                            ValidationClient(ref newActivityRequest);

                            ValidationDuration(ref newActivityRequest);
                            if (!ErrorCRUD)
                            {
                                if (Once) //Wenn einmalig
                                {
                                    ValidationDate(ref newActivityRequest);
                                }
                                else
                                {
                                    ValidationDatePermanentFrom(ref newActivityRequest);
                                    ValidationDatePermanentUntil(ref newActivityRequest);
                                    ValidationDatePermanentDuration(ref newActivityRequest);
                                }
                            }
                            break;
                        case 7: //Tätigkeit als Schiedsrichter
                            ValidationTitel(ref newActivityRequest);
                            ValidationArbitrationTyp(ref newActivityRequest);
                            if (Third)
                            {
                                ValidationArbitrationCaller(ref newActivityRequest);
                            }
                            ValidationArbitrationClient(ref newActivityRequest);
                            break;
                        case 8: //Sonstige Nebentätigkeit
                            ValidationTitel(ref newActivityRequest);
                            break;
                    }
                }
                //Vergütung prüfen
                if (!ErrorCRUD)
                {
                    Anzeige_Verguetung = true;
                    ValidationVerguetung(ref newActivityRequest);
                    AdventagesAdd(ref newActivityRequest);
                }
                //Stunden prüfen
                if (!ErrorCRUD)
                {
                    Anzeige_Zeitaufwand = true;
                    ValidationHoursSelect(ref newActivityRequest);
                }
                if (!ErrorCRUD)
                {
                    Anzeige_Anlagen = true;
                    ValidationInstruction(ref newActivityRequest);
                }
                //Speichern
                if (!ErrorCRUD)
                {
                    try
                    {
                        newActivityRequest.ARNoteAdmin = ARNoteAdmin;
                        newActivityRequest.Assurance = ARAssurance;
                        if (ActivityRequestManager.ActionType == 1) //Create
                        {
                            newActivityRequest.ARUserID = ActivityRequestManager.LoginType == 2 ? ApplicantSelected.UserId : UserManager.RegistratedUser.UserId;
                            if (ActivityRequestManager.LoginType == 2) newActivityRequest.ARZustaendigkeitsbereich = 5;
                        }
                        else //Edit
                        {
                            newActivityRequest.ARUserID = RequestUserID;
                        }
                        activityRequestDBcontext.ActivityRequests.AddOrUpdate(newActivityRequest);
                        activityRequestDBcontext.SaveChanges();
                        //Anlagen prüfen
                        if (ImportFileList.Count > 0)
                        {
                            foreach (ActivityRequestDataFile importFile in ImportFileList)
                            {
                                importFile.ActivityRequestId = newActivityRequest.ActivityRequestId;
                                activityRequestDBcontext.ActivityRequestDataFiles.Add(importFile);
                            }
                            activityRequestDBcontext.SaveChanges();

                        }
                        resp.Success = true;
                    }
                    catch (Exception ex)
                    {
                        resp.Success = false;
                        resp.Message = "Die Meldung konnte nicht eintragen werden. Es ist folgender Fehler aufgetreten: " + ex.InnerException;
                    }
                }
                else
                {
                    resp.Success = false;
                    resp.Message = "Bitte füllen Sie Formular vollständig aus.";
                }

                return resp;
            });
            return task;
        }


        private void NewExecute(object obj)
        {
            bool result = ViewManager.ShowQuestionWindow("Sollen die Eingaben zurückgesetzt werden?", "Ja");
            //MessageBoxResult messageBoxResult = MessageBox.Show("Sollen die Eingaben zurückgesetzt werden?", "Zurücksetzen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result)
            {
                ActivityRequestManager.SelectedActivityRequest = null;
                SetActivityRequest();
            }
        }
        private void AddPersonExecute(object obj)
        {
            ActivityRequestManager.LoginType = 2;
            ActivityRequestManager.ActionType = 1;
            ActivityRequestManager.SelectedActivityRequest = null;
            ViewManager.ShowPageOnMainView<UserLoginView>();
        }
        private bool ImportListClearCanExecute(object obj) => ImportFileList.Count > 0;
        private void ImportListClearExecute(object obj)
        { 
            ImportFileList.Clear();
            ShowAttechmentList = false;
        }
        private void DeleteAttechmentExecute(object obj)
        {
            ImportFileList.Remove(SelectedAttechment);
            ShowAttechmentList = ImportFileList.Count > 0;
        }
        private void OpenAttechmentExecute(object obj)
        {
            try
            {
                System.IO.File.WriteAllBytes($"{BGHKompaktSystemInfo.PathTemp}{SelectedAttechment.FileName}", SelectedAttechment.Data);

                Process.Start(new ProcessStartInfo($"{tempDir}{SelectedAttechment.FileName}") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Das Dokument konnte nicht geöffnet werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
            }
        }
        private void ScienceAuthorSelctionExecute(object obj) => ErrorScienceAuthor = false;
        private void HyperLinkExecute(object obj)
        {
            if (obj != null)
            {
                string URL = string.Empty;
                switch ((string)obj)
                {
                    case "§ 99 BBG":
                        URL = "https://www.gesetze-im-internet.de/bbg_2009/__99.html";
                        break;
                    case "§ 46 DRiG":
                        URL = "https://www.gesetze-im-internet.de/drig/__46.html";
                        break;
                }
                Process.Start(URL);
            }
        }
        private void AddRequestExecute(object obj)
        {
            ActivityRequestDBContext activityRequestDBcontext = new ActivityRequestDBContext();

            for (int i = 4; i < 30; i++)
            {
                string titel = $"Test {i}";
                int verguetung = 1000 + i * 100;
                int zeitaufwand = 10 + i;
                ActivityRequest newActivityRequest = new ActivityRequest
                {
                    ARTitel = titel,
                    ActivityRequestOrtArtId = 1,
                    ActivityRequestTypID = 1,
                    ARDatum = DateTime.Now,
                    ARVerguetung = verguetung,
                    ARZeitaufwandMain = zeitaufwand,
                    ActivityClientID = 1,
                    ARUserID = 7,
                    ActivityRequestMeldeArtID = 1,
                    ARZustaendigkeitsbereich = 4
                };
                activityRequestDBcontext.ActivityRequests.Add(newActivityRequest);

            }
            activityRequestDBcontext.SaveChanges();
        }
        #endregion
        #region Validations
        private void ValidationDuration(ref ActivityRequest newActivityRequest)
        {
            if (Once || Permanent) newActivityRequest.ActivityRequestFrequencyId = Once == true ? 2 : 1;
            else { ErrorDuration = true; ErrorCRUD = true; }
        }
        private void ValidationOrtArt(ref ActivityRequest newActivityRequest)
        {
            if (Presence || Online) newActivityRequest.ActivityRequestOrtArtId = Online == true ? 2 : 1;
            else
            { ErrorOrtArt = true; ErrorCRUD = true; }
        }
        private void ValidationOrt(ref ActivityRequest newActivityRequest)
        {
            if (RequestOrt != string.Empty) newActivityRequest.AROrt = RequestOrt;
            else
            { ErrorOrt = true; ErrorCRUD = true; }
        }
        private void ValidationClient(ref ActivityRequest newActivityRequest)
        {
            if (SelectedClient != null) { newActivityRequest.ActivityClientID = SelectedClient.ActivityClientId; }
            else
            { ErrorClient = true; ErrorCRUD = true; }
        }
        private void ValidationTitel(ref ActivityRequest newActivityRequest)
        {
            if (RequestTitel != string.Empty) newActivityRequest.ARTitel = RequestTitel;
            else
            { ErrorTitel = true; ErrorCRUD = true; }
        }
        private void ValidationDate(ref ActivityRequest newActivityRequest)
        {
            if (RequestDate != null) { newActivityRequest.ARActivityDate = (DateTime)RequestDate; }
            else
            { ErrorDate = true; ErrorCRUD = true; }
        }
        private void ValidationScienceTyp(ref ActivityRequest newActivityRequest)
        {
            if (SelectedScienceTyp != null) newActivityRequest.ActivityRequestScienceTypId = SelectedScienceTyp.ActivityRequestScienceTypId;
            else
            { ErrorScienceTyp = true; ErrorCRUD = true; }
        }
        private void ValidationScienceCategory(ref ActivityRequest newActivityRequest)
        {
            if (Publication || Education) newActivityRequest.ActivityRequestScienceCategorieId = Education == true ? 2 : 1;
            else
            { ErrorScienceCategory = true; ErrorCRUD = true; }
        }
        private void ValidationScienceAuthor(ref ActivityRequest newActivityRequest)
        {
            bool selection = false;
            foreach (ScienceAuthor item in ScienceAuthorList) if (item.IsSelected) { selection = true; break; }
            if (selection)
            {
                List<ActivityRequestScienceAuthorName> listNames = activityRequestDBcontext.ActivityRequestScienceAuthorNames.ToList();
                foreach (ScienceAuthor item in ScienceAuthorList)
                {
                    for (int i = 0; i < listNames.Count - 1;i++)
                    {
                        switch (i)
                        {
                            case 0:
                                if (listNames[i].ActivityRequestScienceAuthorText == item.ScienceAuthorText && item.IsSelected) newActivityRequest.SciencenAuthorAuthor = true;
                                break;
                            case 1:
                                if (listNames[i].ActivityRequestScienceAuthorText == item.ScienceAuthorText && item.IsSelected) newActivityRequest.SciencenAuthorHerausgeber = true;
                                break;
                            case 2:
                                if (listNames[i].ActivityRequestScienceAuthorText == item.ScienceAuthorText && item.IsSelected) newActivityRequest.SciencenAuthorSchriftleitung = true;
                                break;
                            case 3:
                                if (listNames[i].ActivityRequestScienceAuthorText == item.ScienceAuthorText && item.IsSelected) newActivityRequest.SciencenAuthorWissenschaftlicherBeirat = true;
                                break;
                            case 4:
                                if (listNames[i].ActivityRequestScienceAuthorText == item.ScienceAuthorText && item.IsSelected) newActivityRequest.SciencenAuthorSonstiges = true;
                                break;
                        }
                    }
                }
            }
            else { ErrorScienceAuthor = true; ErrorCRUD = true; }
        }
        private void ValidationDatePermanentFrom(ref ActivityRequest newActivityRequest)
        {
            if (RequestDatePermanentFrom != null) newActivityRequest.ActivityRequestDatePermanentFrom = (DateTime)RequestDatePermanentFrom;
            else
            { ErrorDatePermanentFrom = true; ErrorCRUD = true; }
        }
        private void ValidationDatePermanentUntil(ref ActivityRequest newActivityRequest)
        {
            if (RequestDatePermanentUntil != null) newActivityRequest.ActivityRequestDatePermanentUntil = (DateTime)RequestDatePermanentUntil;
            else
            { ErrorDatePermanentUntil = true; ErrorCRUD = true; }
        }
        private void ValidationDatePermanentDuration(ref ActivityRequest newActivityRequest)
        {
            if (RequestDatePermanentDuration != 0) newActivityRequest.ActivityRequestDatePermantenDuration = RequestDatePermanentDuration;
            else
            { ErrorDatePermanentDuration = true; ErrorCRUD = true; }
        }
        private void ValidationArbitrationTyp(ref ActivityRequest newActivityRequest)
        {
            if (Party || Third) newActivityRequest.ActivityRequestArbitrationTypId = Third == true ? 2 : 1;
            else
            { ErrorArbitrationTyp = true; ErrorCRUD = true; }
        }
        private void ValidationArbitrationCaller(ref ActivityRequest newActivityRequest)
        {
            if (ActivityRequestArbitrationCaller != string.Empty) newActivityRequest.ActivityRequestArbitrationCaller = ActivityRequestArbitrationCaller;
            else
            { ErrorArbitrationCaller = true; ErrorCRUD = true; }
        }
        private void ValidationArbitrationClient(ref ActivityRequest newActivityRequest)
        {
            if (ArbitrationClientList.Count > 0)
            {
                newActivityRequest.ActivityRequestArbitrationClients = new Collection<ActivityRequestArbitrationClient>();
                foreach (var item in ArbitrationClientList) 
                {
                    ActivityRequestArbitrationClient dbItem = activityRequestDBcontext.ActivityRequestArbitrationClients.FirstOrDefault(ac => ac.ActivityRequestArbitrationClientId == item.ActivityRequestArbitrationClientId);
                    newActivityRequest.ActivityRequestArbitrationClients.Add(dbItem);
                }
            }
            else { ErrorArbitrationCLient = true; ErrorCRUD = true; }
        }
        private void ValidationVerguetung(ref ActivityRequest newActivityRequest)
        {
            if (!PaymentPredicted && !PaymentNothing && !PaymentUnknown) { ErrorPaymentTyp = true; ErrorCRUD = true; return;}
            else
            {
                int paymentTyp = 0;
                if (PaymentPredicted == true) paymentTyp = 1;
                else if (PaymentNothing == true) paymentTyp = 2;
                else if (PaymentUnknown == true) paymentTyp = 3;
                newActivityRequest.ActivityRequestVerguetungTypId = paymentTyp;

            }
            if (PaymentPredicted)
            {
                if (Verguetung != null) newActivityRequest.ARVerguetung = (decimal)Verguetung;
                else { ErrorVerguetung = true; ErrorCRUD = true; }
            }
        }
        private void ValidationHoursSelect(ref ActivityRequest newActivityRequest)
        {
            if (!HoursPredicted && !HoursUnknown ) { ErrorHourTyp = true; ErrorCRUD = true; return; }
            else
            {
                int hourTyp;
                if (HoursPredicted == true) hourTyp = 1;
                else hourTyp = 2;
                newActivityRequest.ActivityRequestHourTypId = hourTyp;
                ValidationHoursMain(ref newActivityRequest);
                ValidationHoursPrep(ref newActivityRequest);

            }
        }
        private void ValidationHoursMain(ref ActivityRequest newActivityRequest)
        {
            Regex numericRegex = new Regex(@"^[0-9]*$");
            if (numericRegex.IsMatch(HoursMain.ToString()) && HoursMain > 0) newActivityRequest.ARZeitaufwandMain = (float)HoursMain;
            else { ErrorHoursMainText = "Bitte tragen Sie eine ganze Stundenzahl ein."; ErrorHoursMain = true; ErrorCRUD = true; }
            //Prüfen, ob Referententätigkeit über 2 Stunden liegt
            if (SelectedRequestTyp.ActivityRequestTypId == 5) 
                if(HoursMain < 3) { ErrorHoursMainText = "Bei der Referententätigkeit darf die Stundenzahl nicht unter 3 Stunden liegen.";  ErrorHoursMain = true; ErrorCRUD = true; }
            else if (SelectedRequestTyp.ActivityRequestTypId == 1)
                if (HoursMain > 2) { ErrorHoursMainText = "Bei der Vortragstätigkeit darf die Stundenzahl nicht über 2 Stunden liegen."; ErrorHoursMain = true; ErrorCRUD = true; }
        }
        private void ValidationHoursPrep(ref ActivityRequest newActivityRequest)
        {
            if (HoursPrep > 0) { newActivityRequest.ARZeitaufwandPrep = (float)HoursPrep; }
            else { ErrorHoursMainText = "Bitte tragen Sie eine ganze Stundenzahl ein."; ErrorHoursPrep = true; ErrorCRUD = true; }
        }
        private void ValidationInstruction(ref ActivityRequest newActivityRequest)
        {
            if (ARAssurance == true) newActivityRequest.ARZeitaufwandPrep = (float)HoursPrep;
            else { ErrorInstruction = true; ErrorCRUD = true; }
        }
        private void AdventagesAdd(ref ActivityRequest newActivityRequest)
        {
            if (AdventageList.Count > 0)
            {
                //Gelöschte Adventages aus der Datenbank entfernen
                //if (ActivityRequestManager.ActionType == 2)


                newActivityRequest.ARVerguetungAdventages = new Collection<ARVerguetungAdventage>();
                foreach (ARVerguetungAdventage item in AdventageList)
                {                   
                    ARVerguetungAdventage dbItem = activityRequestDBcontext.ARVerguetungAdventages.FirstOrDefault(a => a.ARVerguetungAdventageId == item.ARVerguetungAdventageId);
                    if (dbItem == null)
                    {
                        //Der Typ muss mit der aktuellen Verbindung synchronisiert werden
                        ARVerguetungAdventageTyp adTtyp = activityRequestDBcontext.ARVerguetungAdventageTyps.FirstOrDefault(a => a.ARVerguetungAdventageTypId == item.ARVerguetungAdventageTyp.ARVerguetungAdventageTypId);
                        item.ARVerguetungAdventageTyp = adTtyp;
                        activityRequestDBcontext.ARVerguetungAdventages.Add(item);
                        dbItem = item;
                    }
                    newActivityRequest.ARVerguetungAdventages.Add(dbItem);
                }
            }
        }
        #endregion
        #region ShowFunctions
        private void ShowClientConvert(bool iShowClient, bool iShowRequest)
        {
            ShowClient = iShowClient;
            ShowActivityRequest = iShowRequest;
            RequestClientName = string.Empty;
            SelectedClientTyp = null;
        }
        private void ShowAdventageConvert(bool iShowClient, bool iShowRequest)
        {
            ShowAdventage = iShowClient;
            ShowActivityRequest = iShowRequest;
        }
        private void ShowArbitrationClientConvert(bool iShowClient, bool iShowRequest)
        {
            ShowArbitrationClientAdd = iShowClient;
            ShowActivityRequest = iShowRequest;
        }
        private void ShowContentRequestTyp()
        {
            AnzeigeDetails = true;
            switch (SelectedRequestTyp != null ? SelectedRequestTyp.ActivityRequestTypId : 0)
            {
                //Vortragstätigkeit/Referententätigkeit
                case 1:
                case 5:
                    ShowSettings
                        (
                            ShowDetails: true,
                            ShowDetailUnter: true,
                            ShowVortragstaetigkeit: true,
                            HintClient: "Veranstalter",
                            HintRequestTitel: "Thema"
                        );
                    break;
                case 2: //Wissenschaftliche Tätigkeit
                    ShowSettings
                        (
                            //if (Publication || Education)
                            ShowDetails: true,
                            ShowScience: true,
                            ShowScienceArt: true,
                            HintClient: "Veranstalter",
                            HintRequestTitel: "Thema"
                        );
                    break;
                case 3: //schriftstellerische Tätigkeit
                    ShowSettings
                        (
                            ShowDetails: true,
                            ShowSonstiges: true,
                            HintDescription: "Künstlerische Tätigkeit bitte näher erläutern:"
                        );
                    break;
                case 4: //künstlerische Tätigkeit
                    ShowSettings
                        (
                            ShowDetails: true,
                            ShowSonstiges: true,
                            HintDescription: "Schriftstellerische Tätigkeit bitte näher erläutern:"
                        );
                    break;
                //case 5: //Referententätigkeit
                //    ShowSettings
                //        (
                //            ShowDetails: true, 
                //            ShowDetailUnter: true, 
                //            ShowVortragstaetigkeit: true, 
                //            ShowOrt: true, 
                //            ShowDate: true,
                //            ShowDateOnce: true,
                //            HintClient: "Veranstalter", 
                //            HintRequestTitel: "Thema"
                //        );
                //    break;
                case 6: //Prüfertätigkeit
                    ShowSettings
                        (
                            ShowExam: true,
                            ShowDetails: true,
                            ShowDetailUnter: true,
                            ShowDate: true,
                            ShowDateRadioButton: true,
                            HintClient: "Prüfungsamt",
                            HintRequestTitel: "Art der Prüfung"
                        );
                    break;
                case 7: //Tätigkeit als Schiedsrichter
                    ShowSettings
                        (
                            ShowArbitration: true,
                            ShowDetails: true
                        );
                    break;
                case 8: //Sonstige Nebentätigkeit
                    ShowSettings
                        (
                            ShowDetails: true,
                            ShowSonstiges: true,
                            HintDescription: "Sonstige Tätigkeit bitte näher erläutern:"
                        );
                    break;
                default:
                    ShowSettings();
                    break;
            }
            ;
        }
        private void ShowSettings(
            bool ShowDetails = false,
            bool ShowDetailUnter = false,
            bool ShowScience = false,
            bool ShowScienceArt = false,
            bool ShowScienceTyp = false,
            bool ShowScienceAuthor = false,
            bool ShowVortragstaetigkeit = false,
            bool ShowExam = false,
            bool ShowOrt = false,
            bool ShowDate = false,
            bool ShowDateOnce = false,
            bool ShowDatePermanent = false,
            bool ShowDateRadioButton = false,
            bool ShowSonstiges = false,
            bool ShowPublication = false,
            bool ShowEducation = false,
            bool ShowArbitration = false,
            string HintClient = "Veranstalter",
            string HintRequestTitel = "Thema",
            string HintDescription = "Tätigkeit bitte näher erläutern"
            )
        {
            AnzeigeDetails = ShowDetails;
            AnzeigeScience = ShowScience;
            AnzeigeScienceArt = ShowScienceArt;
            AnzeigeDetailUnter = ShowDetailUnter;
            AnzeigeExam = ShowExam;
            Hint_Client = HintClient;
            Hint_RequestTitel = HintRequestTitel;
            AnzeigeVortragstaetigkeit = ShowVortragstaetigkeit;
            AnzeigeOrt = ShowOrt;
            AnzeigeDate = ShowDate;
            AnzeigeScienceTyp = ShowScienceTyp;
            AnzeigeScienceAuthor = ShowScienceAuthor;
            AnzeigeSonstiges = ShowSonstiges;
            AnzeigeDateRadioButton = ShowDateRadioButton;
            AnzeigeArbitration = ShowArbitration;
            AnzeigePublication = ShowPublication;
            AnzeigeEducation = ShowEducation;
            Description = HintDescription;
            AnzeigeDateOnce = ShowDateOnce;
            AnzeigeDatePermanent = ShowDatePermanent;

        }
        #endregion
        private void SetActivityRequest(ActivityRequest iActivityRequest = null)
        {
            if (iActivityRequest != null)
            {
                RequestUserID = iActivityRequest.ARUserID;
                Anzeigepflichtig = iActivityRequest.ActivityRequestMeldeArtID == 1;
                Genehmigungspflichtig = iActivityRequest.ActivityRequestMeldeArtID == 2;
                RequestTitel = iActivityRequest.ARTitel;
                RequestOrt = iActivityRequest.AROrt;
                setRequestDate = true;
                RequestDate = iActivityRequest.ARDatum;
                Verguetung = iActivityRequest.ARVerguetung;
                HoursMain = iActivityRequest.ARZeitaufwandMain;
                HoursPrep = iActivityRequest.ARZeitaufwandPrep;
                HoursPredicted = iActivityRequest.ActivityRequestHourTypId == 1;
                HoursUnknown = iActivityRequest.ActivityRequestHourTypId == 2;

                Description = iActivityRequest.ARNote;
                ARNoteAdmin = iActivityRequest.ARNoteAdmin;
                SelectedClient = Clientlist.FirstOrDefault(x => x.ActivityClientId == iActivityRequest.ActivityClientID);
                SelectedRequestTyp = RequestTyplist.FirstOrDefault(x => x.ActivityRequestTypId == iActivityRequest.ActivityRequestTypID);
                //ShowContentRequestTyp();
                ARAssurance = iActivityRequest.Assurance;
                //AdventageList füllen
                SelectedScienceTyp = iActivityRequest.ActivityRequestScienceTyp;
                Online = iActivityRequest.ActivityRequestOrtArtId == 1;
                Presence = iActivityRequest.ActivityRequestOrtArtId == 2;
                Publication = iActivityRequest.ActivityRequestScienceCategorieId == 1;
                Education = iActivityRequest.ActivityRequestScienceCategorieId == 2;
                Permanent = iActivityRequest.ActivityRequestFrequencyId == 1;
                Once = iActivityRequest.ActivityRequestFrequencyId == 2;
                Party = iActivityRequest.ActivityRequestArbitrationTypId == 1;
                Third = iActivityRequest.ActivityRequestArbitrationTypId == 2;
                PaymentPredicted = iActivityRequest.ActivityRequestVerguetungTypId == 1;
                PaymentNothing = iActivityRequest.ActivityRequestVerguetungTypId == 2;
                PaymentUnknown = iActivityRequest.ActivityRequestVerguetungTypId == 3;
                SetScienceAuthor(iActivityRequest);
                //VerguetungsAdventages füllen
                if (iActivityRequest.ARVerguetungAdventages != null) 
                    foreach (ARVerguetungAdventage adventage in iActivityRequest.ARVerguetungAdventages) AdventageList.Add(adventage);
                AnzeigeAdventageList = AdventageList.Count > 0;
                //Anlagen füllen
                if (iActivityRequest.ActivityRequestDataFiles != null) 
                    foreach (ActivityRequestDataFile file in iActivityRequest.ActivityRequestDataFiles) ImportFileList.Add(file);
                ShowAttechmentList = ImportFileList.Count > 0;

            }
            else
            {
                RequestUserID = 0;
                RequestTitel = string.Empty;
                RequestOrt = string.Empty;
                setRequestDate = true;
                RequestDate = DateTime.Now;
                Verguetung = null;
                HoursMain = null;
                HoursPrep = null;
                HoursPredicted = true;
                HoursUnknown = false;
                Description = string.Empty;
                ARNoteAdmin = string.Empty;
                SelectedClient = null;
                SelectedRequestTyp = null;
                SelectedIndextRequestTyp = 0;
                //AdventageList füllen
                SelectedScienceTyp = null;
                Anzeigepflichtig = false;
                Genehmigungspflichtig = false;
                Online = false;
                Presence = false;
                Publication = false;
                Education = false;
                Permanent = false;
                Once = false;
                Party = false;
                Third = false;
                AnzeigeGesamt = false;
                ArbitrationClientList.Clear();
                AnzeigeArbitrationClientList = false;
                AdventageList.Clear();
                AnzeigeAdventageList = false;
                ARAssurance = false;
                PaymentPredicted = true;
                PaymentNothing = false;
                PaymentUnknown = false;
                SetScienceAuthor(null);
            }
        }
        private void SetScienceAuthor(ActivityRequest iActivityRequest)
        {
            List<ActivityRequestScienceAuthorName> authorNames = activityRequestDBcontext.ActivityRequestScienceAuthorNames.ToList();
            for (int i = 0; i < authorNames.Count; i++)
            {
                ScienceAuthor author = new ScienceAuthor { ScienceAuthorText = authorNames[i].ActivityRequestScienceAuthorText };
                switch (i)
                {
                    case 0:
                        author.IsSelected = iActivityRequest != null && iActivityRequest.SciencenAuthorAuthor;
                        break;
                    case 1:
                        author.IsSelected = iActivityRequest != null && iActivityRequest.SciencenAuthorSchriftleitung;
                        break;
                    case 2:
                        author.IsSelected = iActivityRequest != null && iActivityRequest.SciencenAuthorHerausgeber;
                        break;
                    case 3:
                        author.IsSelected = iActivityRequest != null && iActivityRequest.SciencenAuthorWissenschaftlicherBeirat;
                        break;
                    case 4:
                        author.IsSelected = iActivityRequest != null && iActivityRequest.SciencenAuthorSonstiges;
                        break;
                }
                ScienceAuthorList.Add(author);
            }
        }
        public void OnFilesDropped(string[] files)
        {
            //MessageBox.Show("Test");
            ImportFileList.Clear();
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);

                string pdfFilePath = fileInfo.FullName;
                byte[] bytes = System.IO.File.ReadAllBytes(pdfFilePath);
                ActivityRequestDataFile importFile = new ActivityRequestDataFile { FileName = fileInfo.Name, Data = bytes };
                ImportFileList.Add(importFile);
            }
            ShowAttechmentList = true;
        }
    }
}
