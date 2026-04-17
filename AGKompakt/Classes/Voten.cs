using BGH_Kompakt.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class Voten : ViewModelBase
    {
        private string _Sitzung = string.Empty;
        private string _Verfahren = string.Empty;
        private string _Verfahren_Kurz = string.Empty;
        private string _Sitzung_Anzeige = string.Empty;
        private string _Spruchgruppe = string.Empty;

        public string Sitzung
        {
            get { return _Sitzung; }
            set { SetProperty<string>(ref _Sitzung, value); }
        }

        public string Verfahren
        {
            get { return _Verfahren; }
            set { SetProperty<string>(ref _Verfahren, value); }
        }

        public string Verfahren_Kurz
        {
            get { return _Verfahren_Kurz; }
            set { SetProperty<string>(ref _Verfahren_Kurz, value); }
        }

        public string Sitzung_Anzeige
        {
            get { return _Sitzung_Anzeige; }
            set { SetProperty<string>(ref _Sitzung_Anzeige, value); }
        }

        public string Spruchgruppe
        {
            get { return _Spruchgruppe; }
            set { SetProperty<string>(ref _Spruchgruppe, value); }
        }

        public Voten() { }
        public Voten(string Sitzung, string Verfahren, string sitzung_Anzeige, string verfahren_Kurz, string spruchgruppe)
        {
            _Sitzung = Sitzung;
            _Verfahren = Verfahren;
            _Sitzung_Anzeige = sitzung_Anzeige;
            _Verfahren_Kurz = verfahren_Kurz;
            _Spruchgruppe = spruchgruppe;
        }

    }
}
