using BGH_Kompakt.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class Person : ViewModelBase
    {
        //public Verfahren() { }
        //public Verfahren(string verfahrensnummer, string Berechnungsgrundlage)
        //{
        //    _SenatArt = verfahrensnummer;
        //    _SenatNummer = Berechnungsgrundlage;
        //}

        private string _ID = string.Empty;
        private string _Nachname = string.Empty;
        private string _Vorname = string.Empty;
        private string _Titel = string.Empty;
        private string _EMail = string.Empty;
        private string _Geschlecht = string.Empty;
        private string _Position = string.Empty;
        private string _Status = string.Empty;
        private string _Dienstbezeichnung = string.Empty;

        public string ID
        {
            get { return _ID; }
            set { SetProperty<string>(ref _ID, value); }
        }

        public string Nachname
        {
            get { return _Nachname; }
            set { SetProperty<string>(ref _Nachname, value); }
        }

        public string Vorname
        {
            get { return _Vorname; }
            set { SetProperty<string>(ref _Vorname, value); }
        }

        public string Titel
        {
            get { return _Titel; }
            set { SetProperty<string>(ref _Titel, value); }
        }

        public string EMail
        {
            get { return _EMail; }
            set { SetProperty<string>(ref _EMail, value); }
        }

        public string Geschlecht
        {
            get { return _Geschlecht; }
            set { SetProperty<string>(ref _Geschlecht, value); }
        }

        public string Geschlecht_Anzeige
        {
            get
            {
                if (_Geschlecht == "1")
                {
                    return "männlich";
                }
                else
                {
                    return "weiblich";
                }
            }
        }

        public string Position
        {
            get { return _Position; }
            set { SetProperty<string>(ref _Position, value); }
        }

        public string Status
        {
            get { return _Status; }
            set { SetProperty<string>(ref _Status, value); }
        }

        public string Dienstbezeichnung
        {
            get { return _Dienstbezeichnung; }
            set { SetProperty<string>(ref _Dienstbezeichnung, value); }
        }


    }
}
