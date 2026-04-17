using BGH_Kompakt.Classes;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BGH_Kompakt.ViewModel
{
    public class PersonChangeViewModel : ViewModelBase
    {
        private string pathrichterliste {  get; set; }
        private ObservableCollection<PersonenViewModel> _personenList;
        private ListCollectionView _personView;
        private string _actualPosition;
        private CommandBinding _newCommandBinding;
        private CommandBinding _deleteCommandBinding;
        private CommandBinding _saveCommandBinding;
        private CommandBinding _undoCommandBinding;
        private bool _ListChanged;
        private bool _setFocus;
        public bool SetFocus
        {
            get { return _setFocus; }
            set { SetProperty<bool>(ref _setFocus, value); }
        }


        public ListCollectionView PersonView
        {
            get { return _personView; }
        }

        public string ActualPosition
        {
            get => _actualPosition;
            private set {
                SetProperty<string>(ref _actualPosition, value); 
            }
        }


        public ICommand FirstCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand PreviousCommand { get; set; }
        public ICommand LastCommand { get; set; }


        public PersonChangeViewModel()
        {
            //string pathParent = Assembly.GetExecutingAssembly().Location.Split('\\')[0] + "\\";
            pathrichterliste = BGHKompaktSystemInfo.PathLaufwerksbuchstabe + BGHKompaktSystemInfo.PathSystemdateien + "Richterliste.xml";
            _personenList = new ObservableCollection<PersonenViewModel>();
            LoadRichter();
            _personView = new ListCollectionView(_personenList);
            _personView.CurrentChanged += _person_CurrentChanged;

            FirstCommand = new RelayCommand(FirstExecute, BackCanExecute);
            PreviousCommand = new RelayCommand(PreviousExecute, BackCanExecute);
            NextCommand = new RelayCommand(NextExecute, ForwardCanExecute);
            LastCommand = new RelayCommand(LastExecute, ForwardCanExecute);

            _newCommandBinding = new CommandBinding(ApplicationCommands.New, NewExecuted, NewCanExecute);
            _saveCommandBinding = new CommandBinding(ApplicationCommands.Save, SaveExecuted, SaveCanExecute);
            _deleteCommandBinding = new CommandBinding(ApplicationCommands.Delete, DeleteExecuted, DeleteCanExecute);
            _undoCommandBinding = new CommandBinding(ApplicationCommands.Undo, UndoExecuted, UndoCanExecute);
        }

        private void UndoCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((_personView).Count == 0)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute =
                ((PersonenViewModel)_personView.CurrentItem).Changed == "*";
        }

        private void UndoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ((PersonenViewModel)_personView.CurrentItem).UndoChanges();
        }

        private void DeleteCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = PersonView.Count > 0;
        }

        private void DeleteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            PersonenViewModel personDelete = PersonView.CurrentItem as PersonenViewModel;
            if (personDelete != null) _personenList.Remove(personDelete);
            _personView.MoveCurrentToFirst();
            ListChanged = true;
        }

        private void SaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ListChanged == true)
            {
                e.CanExecute = true;
                return;
            }
            foreach (PersonenViewModel item in _personenList)
            {
                if (item.Changed == "*")
                {
                    e.CanExecute = true;
                    return;
                }
                else
                {
                    e.CanExecute = false;
                }
            }
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ObservableCollection<Person> person = new ObservableCollection<Person>();
            foreach (PersonenViewModel item in _personenList)
            {
                Person p = new Person();
                p.ID = item.ID.Value;
                p.Nachname = item.Nachname.Value;
                p.Vorname = item.Vorname.Value;
                p.Titel = item.Titel.Value;
                p.EMail = item.EMail.Value;
                p.Geschlecht = item.Geschlecht.Value;
                p.Position = item.Position.Value;
                p.Status = item.Status.Value;
                p.Dienstbezeichnung = item.Dienstbezeichnung.Value;
                person.Add(p);
            }
            FileStream fs = new FileStream(pathrichterliste, FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Person>));
            serializer.Serialize(fs, person);
            fs.Close();
            AcceptChanges();
            ListChanged = false;
        }

        private void NewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            PersonenViewModel person = new PersonenViewModel(null);
            _personenList.Add(person);
            _personView.MoveCurrentTo(person);
            SetFocus = true;
        }

        public CommandBinding NewCommandBinding
        {
            get { return _newCommandBinding; }

        }

        public CommandBinding SaveCommandBinding
        {
            get { return _saveCommandBinding; }

        }

        public CommandBinding DeleteCommandBinding
        {
            get { return _deleteCommandBinding; }

        }

        public CommandBinding UndoCommandBinding
        {
            get { return _undoCommandBinding; }

        }


        private void _person_CurrentChanged(object sender, EventArgs e)
        {
            ActualPosition = "Datensatz " + (_personView.CurrentPosition + 1) + " von " + _personView.Count;
        }

        private bool ForwardCanExecute(object obj)
        {
            return _personView.CurrentPosition < _personView.Count - 1;
        }

        private void LastExecute(object obj)
        {
            _personView.MoveCurrentToLast();
        }

        private void NextExecute(object obj)
        {
            _personView.MoveCurrentToNext();
        }

        private bool BackCanExecute(object obj)
        {
            return _personView.CurrentPosition > 0;
        }

        private void PreviousExecute(object obj)
        {
            _personView.MoveCurrentToPrevious();
        }

        private void FirstExecute(object obj)
        {
            _personView.MoveCurrentToFirst();
        }

        private void LoadRichter()
        {
            _personenList = new ObservableCollection<PersonenViewModel>();
            FileStream fs = new FileStream(pathrichterliste, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Person>));
            ObservableCollection<Person> templist = (ObservableCollection<Person>)serializer.Deserialize(fs);
            foreach (var item in templist) _personenList.Add(new PersonenViewModel(item));
            fs.Close();
        }

        public bool ListChanged
        {
            get { return _ListChanged; }
            set
            {
                if (_ListChanged != value) SetProperty(ref _ListChanged, value);
            }
        }

        private void AcceptChanges()
        {
            foreach (PersonenViewModel item in _personenList) item.AcceptChanges();
        }
    }

}
