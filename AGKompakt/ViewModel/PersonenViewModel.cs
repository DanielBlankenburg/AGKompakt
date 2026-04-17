using BGH_Kompakt.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace BGH_Kompakt.ViewModel
{
    public class PersonenViewModel : ViewModelBase
    {
        private Person _Person;
        private PString _ID;
        private PString _Nachname;
        private PString _Vorname;
        private PString _Titel;
        private PString _EMail;
        private PString _Geschlecht;
        private PString _Geschlecht_Anzeige;
        private PString _Position;
        private PString _Status;
        private PString _Dienstbezeichnung;
        private string _changed;

        public PersonenViewModel(Person person)
        {
            if (person != null)
            {
                _Person = person;
                initializeFields();

            }
            else
            {
                _Person = new Person();
                initializeFields();
                _ID.HasChanged = true;
                _Nachname.HasChanged = true;
                _Vorname.HasChanged = true;
                _Titel.HasChanged = true;
                _EMail.HasChanged = true;
                _Geschlecht.HasChanged = true;
                _Geschlecht_Anzeige.HasChanged = true; 
                _Position.HasChanged = true;
                _Status.HasChanged = true;
                _Dienstbezeichnung.HasChanged = true;


            }
            _ID.PropertyChanged += person_PropertyChanged;
            _Nachname.PropertyChanged += person_PropertyChanged;
            _Vorname.PropertyChanged += person_PropertyChanged;
            _Titel.PropertyChanged += person_PropertyChanged;
            _EMail.PropertyChanged += person_PropertyChanged;
            _Geschlecht.PropertyChanged += person_PropertyChanged;
            _Position.PropertyChanged += person_PropertyChanged;
            _Status.PropertyChanged += person_PropertyChanged;
            _Geschlecht_Anzeige.PropertyChanged += person_PropertyChanged;
            _Dienstbezeichnung.PropertyChanged += person_PropertyChanged;

        }

        private void person_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ID.HasChanged || _Nachname.HasChanged || _Vorname.HasChanged || _Titel.HasChanged || _EMail.HasChanged || _Geschlecht.HasChanged || _Status.HasChanged || _Position.HasChanged || _Geschlecht_Anzeige.HasChanged) Changed = "*";
            else Changed = string.Empty;
        }

        private void initializeFields()
        {
            _ID = new PString(_Person.ID);
            _Nachname = new PString(_Person.Nachname);
            _Vorname = new PString(_Person.Vorname);
            _Titel = new PString(_Person.Titel);
            _EMail = new PString(_Person.EMail);
            _Geschlecht = new PString(_Person.Geschlecht);
            _Position = new PString(_Person.Position);
            _Status = new PString(_Person.Status);
            _Geschlecht_Anzeige = new PString(_Person.Geschlecht_Anzeige);
            _Dienstbezeichnung = new PString(_Person.Dienstbezeichnung);
        }

        public PString ID
        {
            get { return _ID; }
        }

        public PString Nachname
        {
            get { return _Nachname; }
        }

        public PString Vorname
        {
            get { return _Vorname; }
        }

        public PString Titel
        {
            get { return _Titel; }
        }

        public PString EMail
        {
            get { return _EMail; }
        }

        public PString Geschlecht
        {
            get { return _Geschlecht; }
        }

        public PString Geschlecht_Anzeige
        {
            get { return _Geschlecht_Anzeige; }
        }

        public PString Position
        {
            get { return _Position; }
        }

        public string Position_Anzeige
        {
            get
            {
                if (_Person.Position == "1")
                {
                    return "HiWi";
                }
                else
                {
                    return "Richter";
                }
            }
        }

        public PString Dienstbezeichnung
        {
            get { return _Dienstbezeichnung; }
        }
        
        public PString Status
        {
            get { return _Status; }

        }

        public string Status_Anzeige
        {
            get
            {
                if (_Person.Status == "1")
                {
                    return "Aktiv";
                }
                else if(_Person.Status == "2")
                {
                    return "Inaktiv";
                }
                else
                {
                    return "Kein Status";
                }
            }
        }


        public string Changed
        {
            get { return _changed; }
            set
            {
                SetProperty<string>(ref _changed, value);
            }
        }

 

        public void UndoChanges()
        {
            _ID.UndoChanges();
            _Nachname.UndoChanges();
            _Vorname.UndoChanges();
            _Titel.UndoChanges();
            _EMail.UndoChanges();
            _Geschlecht.UndoChanges();
            _Geschlecht_Anzeige.UndoChanges();
            _Position.UndoChanges();
            _Status.UndoChanges();
            _Dienstbezeichnung.UndoChanges();
        }

        public void AcceptChanges()
        {
            _ID.AccptChanges();
            _Nachname.AccptChanges();
            _Vorname.AccptChanges();
            _Titel.AccptChanges();
            _EMail.AccptChanges();
            _Geschlecht.AccptChanges();
            _Geschlecht_Anzeige.AccptChanges();
            _Position.AccptChanges();
            _Status.AccptChanges();
            _Dienstbezeichnung.AccptChanges();
        }
    }
}
