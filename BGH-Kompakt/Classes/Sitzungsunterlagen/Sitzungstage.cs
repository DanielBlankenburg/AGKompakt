using BGH_Kompakt.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class Sitzungstage : ViewModelBase
    {

        private string _FullDirectory;
        public string FullDirectory
        {
            get { return _FullDirectory; }
            set { SetProperty(ref _FullDirectory, value); }
        }


        private string _Rohdatum;
        public string Rohdatum
        {
            get { return _Rohdatum; }
            set { SetProperty(ref _Rohdatum, value); }
        }



        private string _Anzeigedatum;
        public string Anzeigedatum
        {
            get { return _Anzeigedatum; }
            set { SetProperty(ref _Anzeigedatum, value); }
        }

        private string _Jahr;
        public string Jahr
        {
            get { return _Jahr; }
            set { SetProperty(ref _Jahr, value); }
        }

        private string _Monat;
        public string Monat
        {
            get { return _Monat; }
            set { SetProperty(ref _Monat, value); }
        }


        private string _Tag;
        public string Tag
        {
            get { return _Tag; }
            set { SetProperty(ref _Tag, value); }
        }

        public Sitzungstage()
        {
            
        }



        public Sitzungstage(
          string rohdatum)
        {
            FullDirectory = rohdatum;
            DirectoryInfo importDirectory = new DirectoryInfo(FullDirectory);
            Rohdatum = importDirectory.Name;
            DateExtract();
        }

        private void DateExtract()
        {
            Jahr = Rohdatum.Substring(0, 4);
            string str = Rohdatum.Substring(5, Rohdatum.Length - 5);
            Monat = str.Substring(0, 2);
            Tag = str.Substring(str.Length - 2, 2);
            Anzeigedatum = Tag + "." + Monat + "." + Jahr;

        }
    }
}
