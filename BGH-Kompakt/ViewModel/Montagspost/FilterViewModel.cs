using BGH_Kompakt.Classes;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.SystemComponents;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel
{
    public class FilterViewModel : ViewModelBase
    {
        private string pathFilterliste { get; set; }
        public static MontagspostFilter _filterSetting = new MontagspostFilter();
        //private ObservableCollection<MontagspostFilter> _filterlist;
        //private ListCollectionView _filterView;
        private CommandBinding _saveCommandBinding;

        private bool _zivilsenate;
        private bool _zivilsenat1;
        private bool _zivilsenat2;
        private bool _zivilsenat3;
        private bool _zivilsenat4;
        private bool _zivilsenat5;
        private bool _zivilsenat6;
        private bool _zivilsenat6a;
        private bool _zivilsenat7;
        private bool _zivilsenat8;
        private bool _zivilsenat9;
        private bool _zivilsenat10;
        private bool _zivilsenat11;
        private bool _zivilsenat12;
        private bool _zivilsenat13;

        private bool _strafsenate;
        private bool _strafsenat1;
        private bool _strafsenat2;
        private bool _strafsenat3;
        private bool _strafsenat4;
        private bool _strafsenat5;
        private bool _strafsenat6;


        private bool _sondersenate;
        private bool _sondersenatGOBG;
        private bool _sondersenatGZS;
        private bool _sondersenatGSS;
        private bool _sondersenatAnwalt;
        private bool _sondersenatNotar;
        private bool _sondersenatLandwirtschaft;
        private bool _sondersenatSteuerberater;
        private bool _sondersenatDienstgericht;
        private bool _sondersenatPatentanwalt;
        private bool _sondersenatKartell;

        private bool _urteile;
        private bool _beschlüsse;
        private bool _leitsatzentscheidungen;


        //public ListCollectionView FilterView
        //{
        //    get { return _filterView; }
        //}

        public CommandBinding SaveCommandBinding
        {
            get { return _saveCommandBinding; }

        }

        public bool Zivilsenate
        {
            get { return _zivilsenate; }
            set { SetProperty<bool>(ref _zivilsenate, value); }
        }
        
        public bool Zivilsenat1
        {
            get { return _zivilsenat1; }
            set { SetProperty<bool>(ref _zivilsenat1, value); }
        }
        public bool Zivilsenat2
        {
            get { return _zivilsenat2; }
            set { SetProperty<bool>(ref _zivilsenat2, value); }
        }
        public bool Zivilsenat3
        {
            get { return _zivilsenat3; }
            set { SetProperty<bool>(ref _zivilsenat3, value); }
        }
        public bool Zivilsenat4
        {
            get { return _zivilsenat4; }
            set { SetProperty<bool>(ref _zivilsenat4, value); }
        }
        public bool Zivilsenat5
        {
            get { return _zivilsenat5; }
            set { SetProperty<bool>(ref _zivilsenat5, value); }
        }
        public bool Zivilsenat6
        {
            get { return _zivilsenat6; }
            set { SetProperty<bool>(ref _zivilsenat6, value); }
        }
        public bool Zivilsenat6a
        {
            get { return _zivilsenat6a; }
            set { SetProperty<bool>(ref _zivilsenat6a, value); }
        }
        public bool Zivilsenat7
        {
            get { return _zivilsenat7; }
            set { SetProperty<bool>(ref _zivilsenat7, value); }
        }
        public bool Zivilsenat8
        {
            get { return _zivilsenat8; }
            set { SetProperty<bool>(ref _zivilsenat8, value); }
        }
        public bool Zivilsenat9
        {
            get { return _zivilsenat9; }
            set { SetProperty<bool>(ref _zivilsenat9, value); }
        }
        public bool Zivilsenat10
        {
            get { return _zivilsenat10; }
            set { SetProperty<bool>(ref _zivilsenat10, value); }
        }
        public bool Zivilsenat11
        {
            get { return _zivilsenat11; }
            set { SetProperty<bool>(ref _zivilsenat11, value); }
        }
        public bool Zivilsenat12
        {
            get { return _zivilsenat12; }
            set { SetProperty<bool>(ref _zivilsenat12, value); }
        }
        public bool Zivilsenat13
        {
            get { return _zivilsenat13; }
            set { SetProperty<bool>(ref _zivilsenat13, value); }
        }


        public bool Strafsenate
        {
            get { return _strafsenate; }
            set { SetProperty<bool>(ref _strafsenate, value); }
        }
         public bool Strafsenat1
        {
            get { return _strafsenat1; }
            set { SetProperty<bool>(ref _strafsenat1, value); }
        }
        public bool Strafsenat2
        {
            get { return _strafsenat2; }
            set { SetProperty<bool>(ref _strafsenat2, value); }
        }
        public bool Strafsenat3
        {
            get { return _strafsenat3; }
            set { SetProperty<bool>(ref _strafsenat3, value); }
        }
        public bool Strafsenat4
        {
            get { return _strafsenat4; }
            set { SetProperty<bool>(ref _strafsenat4, value); }
        }
        public bool Strafsenat5
        {
            get { return _strafsenat5; }
            set { SetProperty<bool>(ref _strafsenat5, value); }
        }
        public bool Strafsenat6
        {
            get { return _strafsenat6; }
            set { SetProperty<bool>(ref _strafsenat6, value); }
        }
        
        public bool Sondersenate
        {
            get { return _sondersenate; }
            set { SetProperty<bool>(ref _sondersenate, value); }
        }

        public bool SondersenatGOBG
        {
            get { return _sondersenatGOBG; }
            set { SetProperty<bool>(ref _sondersenatGOBG, value); }
        }
        public bool SondersenatGZS
        {
            get { return _sondersenatGZS; }
            set { SetProperty<bool>(ref _sondersenatGZS, value); }
        }
        public bool SondersenatGSS
        {
            get { return _sondersenatGSS; }
            set { SetProperty<bool>(ref _sondersenatGSS, value); }
        }
        public bool SondersenatAnwalt
        {
            get { return _sondersenatAnwalt; }
            set { SetProperty<bool>(ref _sondersenatAnwalt, value); }
        }
        public bool SondersenatNotar
        {
            get { return _sondersenatNotar; }
            set { SetProperty<bool>(ref _sondersenatNotar, value); }
        }
        public bool SondersenatSteuerberater
        {
            get { return _sondersenatSteuerberater; }
            set { SetProperty<bool>(ref _sondersenatSteuerberater, value); }
        }
        public bool SondersenatLandwirtschaft
        {
            get { return _sondersenatLandwirtschaft; }
            set { SetProperty<bool>(ref _sondersenatLandwirtschaft, value); }
        }
        public bool SondersenatDienstgericht
        {
            get { return _sondersenatDienstgericht; }
            set { SetProperty<bool>(ref _sondersenatDienstgericht, value); }
        }
        public bool SondersenatPatentanwalt
        {
            get { return _sondersenatPatentanwalt; }
            set { SetProperty<bool>(ref _sondersenatPatentanwalt, value); }
        }

        public bool SondersenatKartell
        {
            get { return _sondersenatKartell; }
            set { SetProperty<bool>(ref _sondersenatKartell, value); }
        }

        public bool Urteile
        {
            get { return _urteile; }
            set { SetProperty<bool>(ref _urteile, value); }
        }
        public bool Beschlüsse
        {
            get { return _beschlüsse; }
            set { SetProperty<bool>(ref _beschlüsse, value); }
        }
        public bool Leitsatzentscheidungen
        {
            get { return _leitsatzentscheidungen; }
            set { SetProperty<bool>(ref _leitsatzentscheidungen, value); }
        }


        public ICommand CheckBoxZivilCommand { get; set; }
        public ICommand CheckBoxZivilEinzelnCommand { get; set; }
        public ICommand CheckBoxStrafCommand { get; set; }
        public ICommand CheckBoxStrafEinzelnCommand { get; set; }
        public ICommand CheckBoxSondersenateCommand { get; set; }
        public ICommand CheckBoxSondersenateEinzelnCommand { get; set; }

        public FilterViewModel()
        {
            pathFilterliste = BGHKompaktSystemInfo.PathEigeneDateien + BGHKompaktSystemInfo.PathMontagspost + "MontagspostFilter.xml";
            //_filterlist = new ObservableCollection<MontagspostFilter>();
            //FillKlassen.Filterliste_Fill(pathFilterliste, ref _filterSetting);
            Zivilsenate = _filterSetting.ZivilSenate;
            Zivilsenat1 = _filterSetting.Zivilsenat1;
            Zivilsenat2 = _filterSetting.Zivilsenat2;
            Zivilsenat3 = _filterSetting.Zivilsenat3;
            Zivilsenat4 = _filterSetting.Zivilsenat4;
            Zivilsenat5 = _filterSetting.Zivilsenat5;
            Zivilsenat6 = _filterSetting.Zivilsenat6;
            Zivilsenat6a = _filterSetting.Zivilsenat6a;
            Zivilsenat7 = _filterSetting.Zivilsenat7;
            Zivilsenat8 = _filterSetting.Zivilsenat8;
            Zivilsenat9 = _filterSetting.Zivilsenat9;
            Zivilsenat10 = _filterSetting.Zivilsenat10;
            Zivilsenat11 = _filterSetting.Zivilsenat11;
            Zivilsenat12 = _filterSetting.Zivilsenat12;
            Zivilsenat13 = _filterSetting.Zivilsenat13;

            Strafsenate = _filterSetting.Strafsenate;
            Strafsenat1 = _filterSetting.Strafsenat1;
            Strafsenat2 = _filterSetting.Strafsenat2;
            Strafsenat3 = _filterSetting.Strafsenat3;
            Strafsenat4 = _filterSetting.Strafsenat4;
            Strafsenat5 = _filterSetting.Strafsenat5;
            Strafsenat6 = _filterSetting.Strafsenat6;

            Sondersenate = _filterSetting.Sondersenate;
            SondersenatGOBG= _filterSetting.SondersenatGOBG;
            SondersenatGZS = _filterSetting.SondersenatGZS;
            SondersenatGSS= _filterSetting.SondersenatGSS;
            SondersenatAnwalt= _filterSetting.SondersenatAnwalt;
            SondersenatNotar = _filterSetting.SondersenatNotar;
            SondersenatSteuerberater= _filterSetting.SondersenatSteuerberater;
            SondersenatLandwirtschaft= _filterSetting.SondersenatLandwirtschaft;
            SondersenatPatentanwalt= _filterSetting.SondersenatPatentanwalt;
            SondersenatDienstgericht= _filterSetting.SondersenatDienstgericht;
            SondersenatKartell = _filterSetting.SondersenatKartell;

            Urteile= _filterSetting.Urteile;
            Beschlüsse= _filterSetting.Beschlüsse;
            Leitsatzentscheidungen= _filterSetting.Leitsatzentscheidungen;





            //_filterlist.Add(new MontagspostFilter(_filterSetting.ZivilSenate, _filterSetting.Strafsenate, _filterSetting.Sondersenate, _filterSetting.Zivilsenat1, _filterSetting.Zivilsenat2,
            //    _filterSetting.Zivilsenat3, _filterSetting.Zivilsenat4, _filterSetting.Zivilsenat5, _filterSetting.Zivilsenat6, _filterSetting.Zivilsenat7, _filterSetting.Zivilsenat8, _filterSetting.Zivilsenat9,
            //    _filterSetting.Zivilsenat10, _filterSetting.Zivilsenat11, _filterSetting.Zivilsenat12, _filterSetting.Zivilsenat13, _filterSetting.Strafsenat1, _filterSetting.Strafsenat2, _filterSetting.Strafsenat3,
            //    _filterSetting.Strafsenat4, _filterSetting.Strafsenat5, _filterSetting.Strafsenat6, _filterSetting.SondersenatGOBG, _filterSetting.SondersenatGZS, _filterSetting.SondersenatGSS, _filterSetting.SondersenatAnwalt,
            //    _filterSetting.SondersenatNotar, _filterSetting.SondersenatSteuerberater, _filterSetting.SondersenatLandwirtschaft, _filterSetting.SondersenatDienstgericht, _filterSetting.SondersenatPatentanwalt, _filterSetting.Urteile, _filterSetting.Beschlüsse, _filterSetting.Leitsatzentscheidungen, "Neu"));
            //_filterView = new ListCollectionView(_filterlist);
            _saveCommandBinding = new CommandBinding(ApplicationCommands.Save, SaveExecuted, SaveCanExecute);

            CheckBoxZivilCommand = new RelayCommand(CheckBoxZivilExecute, CheckBoxCanExecute);
            CheckBoxZivilEinzelnCommand = new RelayCommand(CheckBoxZivielEinzelnExecute, CheckBoxCanExecute);
            CheckBoxStrafCommand = new RelayCommand(CheckBoxStrafExecute, CheckBoxCanExecute);
            CheckBoxStrafEinzelnCommand = new RelayCommand(CheckBoxStrafEinzelnExecute, CheckBoxCanExecute);
            CheckBoxSondersenateCommand = new RelayCommand(CheckBoxSondersenateExecute, CheckBoxCanExecute);
            CheckBoxSondersenateEinzelnCommand = new RelayCommand(CheckBoxSondersenateEinzelnExecute, CheckBoxCanExecute);
        }

        private void CheckBoxZivilExecute(object obj)
        {

            bool Schalter;

            if (obj.ToString() == "True")
            {
                Schalter = true;
            }
            else
            {
                Schalter = false;
            }

            Zivilsenat1 = Schalter;
            Zivilsenat2 = Schalter;
            Zivilsenat3 = Schalter;
            Zivilsenat4 = Schalter;
            Zivilsenat5 = Schalter;
            Zivilsenat6 = Schalter;
            Zivilsenat6a = Schalter;
            Zivilsenat7 = Schalter;
            Zivilsenat8 = Schalter;
            Zivilsenat9 = Schalter;
            Zivilsenat10 = Schalter;
            Zivilsenat11 = Schalter;
            Zivilsenat12 = Schalter;
            Zivilsenat13 = Schalter;
        }
        private void CheckBoxZivielEinzelnExecute(object obj)
        {

            bool Schalter;

            if (obj.ToString() == "True")
            {
                Schalter = true;
            }
            else
            {
                Schalter = false;
            }

            Zivilsenate = Schalter;

        }

        private void CheckBoxStrafExecute(object obj)
        {

            bool Schalter;

            if (obj.ToString() == "True")
            {
                Schalter = true;
            }
            else
            {
                Schalter = false;
            }

            Strafsenat1 = Schalter;
            Strafsenat2 = Schalter;
            Strafsenat3 = Schalter;
            Strafsenat4 = Schalter;
            Strafsenat5 = Schalter;
            Strafsenat6 = Schalter;

        }
        
        private void CheckBoxStrafEinzelnExecute(object obj)
        {

            bool Schalter;

            if (obj.ToString() == "True")
            {
                Schalter = true;
            }
            else
            {
                Schalter = false;
            }

            Strafsenate = Schalter;

        }
        private void CheckBoxSondersenateExecute(object obj)
        {

            bool Schalter;

            if (obj.ToString() == "True")
            {
                Schalter = true;
            }
            else
            {
                Schalter = false;
            }

            SondersenatGOBG = Schalter;
            SondersenatGZS= Schalter;
            SondersenatGSS= Schalter;
            SondersenatAnwalt= Schalter;
            SondersenatNotar= Schalter;
            SondersenatSteuerberater = Schalter;
            SondersenatLandwirtschaft = Schalter;
            SondersenatDienstgericht = Schalter;
            SondersenatPatentanwalt = Schalter;
        }

        private void CheckBoxSondersenateEinzelnExecute(object obj)
        {

            bool Schalter;

            if (obj.ToString() == "True")
            {
                Schalter = true;
            }
            else
            {
                Schalter = false;
            }

            Sondersenate = Schalter;    
        }

        private bool CheckBoxCanExecute(object obj)
        {
            return true;
        }


        private void SaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;

        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
        }


    }
}
