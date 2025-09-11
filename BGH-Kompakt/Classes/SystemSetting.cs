using BGH_Kompakt.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace BGH_Kompakt.Classes
{
    public class SystemSetting : ViewModelBase
    {
        private string _SenatArt = string.Empty;
        private string _SenatNummer = string.Empty;
        private string _SenatZusatz = string.Empty; 
        private bool _ShowSitzungsplaene = true;
        private bool _ShowKanzlei = true;
        private bool _ShowSpruchgruppen = true;
        private bool _ShowCkbVerteil = false;
        private bool _ShowVotenmappe = false;
        private string _ImportAGUrteil = string.Empty;
        private string _ImportAGBeschluss = string.Empty;
        private string _ImportEntscheidungsentwurf = string.Empty;
        private string _ImportVotum = string.Empty;
        private string _ImportVorVotum = string.Empty;
        private string _ImportLGBeschluss = string.Empty;
        private string _ImportLGUrteil = string.Empty;
        private string _ImportOLGUrteil = string.Empty;
        private string _ImportOLGHB = string.Empty;
        private string _ImportOLGZB = string.Empty;
        private string _ImportLGHB = string.Empty;
        private string _ImportLGZB = string.Empty;
        private string _ImportOLGBeschluss = string.Empty;
        private bool _ImportShowAZ = true;
        private bool _ImportEntscheidungenDetails = true;
        private string _ImportAnlage = string.Empty;
        private string _ImportLeitsatz = string.Empty;
        private string _ImportRMB = string.Empty;
        private string _ImportRME = string.Empty;
        private string _ImportEuGHVorlage = string.Empty;
        private string _ImportEuGHUrteil = string.Empty;
        private string _ImportSonstiges = string.Empty;
        private string _ImportEntscheidungOrt = string.Empty;
        private string _ImportEntscheidungDatum = string.Empty;
        private string _ImportZwischenZeichen = string.Empty;
        private string _AZZR = string.Empty;
        private string _AZZB = string.Empty;
        private string _AZZA = string.Empty;
        private string _AZAR = string.Empty;
        private string _AZ1 = string.Empty;
        private string _AZ2 = string.Empty;
        private string _AZ3 = string.Empty;
        private string _AZ4 = string.Empty;
        private string _AZ5 = string.Empty;
        private string _AZ6 = string.Empty;
        private string _AZ7 = string.Empty;
        private string _AZ8 = string.Empty;
        private string _AZ9 = string.Empty;
        private string _AZ10 = string.Empty;
        private string _testAnzeige = string.Empty;
        private string _SG1 = string.Empty;
        private string _SG2 = string.Empty;
        private string _SG3 = string.Empty;
        private string _SG4 = string.Empty;
        private string _SG5 = string.Empty;
        private string _SG6 = string.Empty;
        private string _SG7 = string.Empty;
        private string _SG8 = string.Empty;
        private string _SG9 = string.Empty;
        private bool _AbschlosseneVerfahren = true;
        private string _Laufwerk_BSCW_Server = string.Empty;

        public string SenatArt
        {
            get { return _SenatArt; }
            set { SetProperty<string>(ref _SenatArt, value); }
        }
        public string SenatNummer
        {
            get { return _SenatNummer; }
            set { SetProperty<string>(ref _SenatNummer, value); }
        }
        public string SenatZusatz
        {
            get { return _SenatZusatz; }
            set { SetProperty<string>(ref _SenatZusatz, value); }
        }
        public bool ShowSitzungsplaene
        {
            get { return _ShowSitzungsplaene; }
            set { SetProperty<bool>(ref _ShowSitzungsplaene, value); }
        } 
        public bool ShowKanzlei
        {
            get { return _ShowKanzlei; }
            set { SetProperty<bool>(ref _ShowKanzlei, value); }
        } 
        public bool ShowSpruchgruppen
        {
            get { return _ShowSpruchgruppen; }
            set { SetProperty<bool>(ref _ShowSpruchgruppen, value); }
        }
        public bool ShowVerteilung
        {
            get { return _ShowCkbVerteil; }
            set { SetProperty<bool>(ref _ShowCkbVerteil, value); }
        }
        public bool ShowVotenmappe
        {
            get { return _ShowVotenmappe; }
            set { SetProperty<bool>(ref _ShowVotenmappe, value); }
        }
        public string ImportAGUrteil
        {
            get => _ImportAGUrteil;
            set => SetProperty<string>(ref _ImportAGUrteil, value, nameof(ImportAGUrteil));
        }

        public string ImportAGBeschluss
        {
            get => _ImportAGBeschluss;
            set
            {
                SetProperty<string>(ref _ImportAGBeschluss, value, nameof(ImportAGBeschluss));
            }
        }

        public string ImportLGBeschluss
        {
            get => _ImportLGBeschluss;
            set
            {
                SetProperty<string>(ref _ImportLGBeschluss, value, nameof(ImportLGBeschluss));
            }
        }

        public string ImportLGUrteil
        {
            get => _ImportLGUrteil;
            set => SetProperty<string>(ref _ImportLGUrteil, value, nameof(ImportLGUrteil));
        }

        public string ImportEntscheidungsentwurf
        {
            get => _ImportEntscheidungsentwurf;
            set
            {
                SetProperty<string>(ref _ImportEntscheidungsentwurf, value, nameof(ImportEntscheidungsentwurf));
            }
        }

        public string ImportVotum
        {
            get => _ImportVotum;
            set => SetProperty<string>(ref _ImportVotum, value, nameof(ImportVotum));
        }
        public string ImportVorVotum
        {
            get => _ImportVorVotum;
            set => SetProperty<string>(ref _ImportVorVotum, value, nameof(ImportVorVotum));
        }

        public string ImportOLGUrteil
        {
            get => _ImportOLGUrteil;
            set => SetProperty<string>(ref _ImportOLGUrteil, value, nameof(ImportOLGUrteil));
        }

        public string ImportOLGBeschluss
        {
            get => _ImportOLGBeschluss;
            set
            {
                SetProperty<string>(ref _ImportOLGBeschluss, value, nameof(ImportOLGBeschluss));
            }
        }
        public string ImportOLGHB
        {
            get => _ImportOLGHB;
            set => SetProperty<string>(ref _ImportOLGHB, value, nameof(ImportOLGHB));
        }

        public string ImportOLGZB
        {
            get => _ImportOLGZB;
            set => SetProperty<string>(ref _ImportOLGZB, value, nameof(ImportOLGZB));
        }
        public bool ImportShowAZ
        {
            get { return _ImportShowAZ; }
            set { SetProperty<bool>(ref _ImportShowAZ, value); }
        }
        public bool ImportEntscheidungenDetails
        {
            get { return _ImportEntscheidungenDetails; }
            set { SetProperty<bool>(ref _ImportEntscheidungenDetails, value); }
        }
        public string ImportLGHB
        {
            get => _ImportLGHB;
            set => SetProperty<string>(ref _ImportLGHB, value, nameof(_ImportLGHB));
        }
        public string ImportLGZB
        {
            get => _ImportLGZB;
            set => SetProperty<string>(ref _ImportLGZB, value, nameof(_ImportLGZB));
        } 
        public string ImportAnlage
        {
            get => _ImportAnlage;
            set => SetProperty<string>(ref _ImportAnlage, value, nameof(_ImportAnlage));
        } 
        public string ImportLeitsatz
        {
            get => _ImportLeitsatz;
            set => SetProperty<string>(ref _ImportLeitsatz, value, nameof(_ImportLeitsatz));
        }
        public string ImportRMB
        {
            get => _ImportRMB;
            set => SetProperty<string>(ref _ImportRMB, value, nameof(_ImportRMB));
        }
        public string ImportRME
        {
            get => _ImportRME;
            set => SetProperty<string>(ref _ImportRME, value, nameof(_ImportRME));
        }

        public string ImportEuGHVorlage
        {
            get => _ImportEuGHVorlage;
            set => SetProperty<string>(ref _ImportEuGHVorlage, value, nameof(_ImportEuGHVorlage));
        }
        public string ImportEuGHUrteil
        {
            get => _ImportEuGHUrteil;
            set => SetProperty<string>(ref _ImportEuGHUrteil, value, nameof(_ImportEuGHUrteil));
        }
        public string ImportSonstiges
        {
            get => _ImportSonstiges;
            set => SetProperty<string>(ref _ImportSonstiges, value, nameof(_ImportSonstiges));
        }

        public string ImportZwischenZeichen
        {
            get => _ImportZwischenZeichen;
            set => SetProperty<string>(ref _ImportZwischenZeichen, value, nameof(_ImportZwischenZeichen));
        }



        public string AZZR
        {
            get => _AZZR;
            set => SetProperty<string>(ref _AZZR, value, nameof(_AZZR));
        }
        public string AZZB
        {
            get => _AZZB;
            set => SetProperty<string>(ref _AZZB, value, nameof(_AZZB));
        }
        public string AZZA
        {
            get => _AZZA;
            set => SetProperty<string>(ref _AZZA, value, nameof(_AZZA));
        }
        public string AZAR
        {
            get => _AZAR;
            set => SetProperty<string>(ref _AZAR, value, nameof(_AZAR));
        }

        public string AZ1
        {
            get => _AZ1;
            set => SetProperty<string>(ref _AZ1, value, nameof(_AZ1));
        }
        public string AZ2
        {
            get => _AZ2;
            set => SetProperty<string>(ref _AZ2, value, nameof(_AZ2));
        }
        public string AZ3
        {
            get => _AZ3;
            set => SetProperty<string>(ref _AZ3, value, nameof(_AZ3));
        }
        public string AZ4
        {
            get => _AZ4;
            set => SetProperty<string>(ref _AZ4, value, nameof(_AZ4));
        }
        public string AZ5
        {
            get => _AZ5;
            set => SetProperty<string>(ref _AZ5, value, nameof(_AZ5));
        }
        public string AZ6
        {
            get => _AZ6;
            set => SetProperty<string>(ref _AZ6, value, nameof(_AZ6));
        }
        public string AZ7
        {
            get => _AZ7;
            set => SetProperty<string>(ref _AZ7, value, nameof(_AZ7));
        }
        public string AZ8
        {
            get => _AZ8;
            set => SetProperty<string>(ref _AZ8, value, nameof(_AZ8));
        }
        public string AZ9
        {
            get => _AZ9;
            set => SetProperty<string>(ref _AZ9, value, nameof(_AZ9));
        }
        public string AZ10
        {
            get => _AZ10;
            set => SetProperty<string>(ref _AZ10, value, nameof(_AZ10));
        }
  

        //public string TestAnzeige
        //{
        //    get => _testAnzeige;
        //    set
        //    {
        //        _testAnzeige = _ImportAGUrteil + "; " + DateTime.Now.ToString();
        //        SetProperty<string>(ref _testAnzeige, value, nameof(ImportOLGHB)); 
        //    }
        //}
        public string SG1
        {
            get => _SG1;
            set => SetProperty<string>(ref _SG1, value, nameof(_SG1));
        }
        public string SG2
        {
            get => _SG2;
            set => SetProperty<string>(ref _SG2, value, nameof(_SG2));
        }
        public string SG3
        {
            get => _SG3;
            set => SetProperty<string>(ref _SG3, value, nameof(_SG3));
        }
        public string SG4
        {
            get => _SG4;
            set => SetProperty<string>(ref _SG4, value, nameof(_SG4));
        }
        public string SG5
        {
            get => _SG5;
            set => SetProperty<string>(ref _SG5, value, nameof(_SG5));
        }
        public string SG6
        {
            get => _SG6;
            set => SetProperty<string>(ref _SG6, value, nameof(_SG6));
        }
        public string SG7
        {
            get => _SG7;
            set => SetProperty<string>(ref _SG7, value, nameof(_SG7));
        }
        public string SG8
        {
            get => _SG8;
            set => SetProperty<string>(ref _SG8, value, nameof(_SG8));
        }
        public string SG9
        {
            get => _SG9;
            set => SetProperty<string>(ref _SG9, value, nameof(_SG9));
        }

        public string Laufwerk_BSCW_Server
        {
            get => _Laufwerk_BSCW_Server.ToUpper();
            set => SetProperty<string>(ref _Laufwerk_BSCW_Server, value, nameof(_Laufwerk_BSCW_Server));
        }


        public bool AbschlosseneVerfahren
        {
            get => _AbschlosseneVerfahren;
            set => SetProperty<bool>(ref _AbschlosseneVerfahren, value, nameof(_AbschlosseneVerfahren));
        }
        public SystemSetting() { }
        public SystemSetting(string SenatArt, string SenatNummer, string SenatZusatz, bool Sitzungsplaene, bool Kanzlei, bool Spruchgruppen, bool Verteilung, bool Votenmappe, string ImportAGBeschluss,
      string ImportAGUrteil,
      string ImportLGBeschluss,
      string ImportLGUrteil,
      string ImportEntscheidungsentwurf,
      string ImportVotum,
      string ImportVorVotum,
      string ImportOLGUrteil,
      string ImportOLGBeschluss,
      string ImportOLGHB,
      string ImportOLGZB,
      string ImportLGHB,
      string ImportLGZB,
      bool ImportShowAZ,
      bool ImportEntscheidungenDetails,
      string ImportAnlage,
      string ImportLeitsatz,
      string ImportRMB,
      string ImportRME,
      string ImportEuGHVorlage,
      string ImportEuGHUrteil,
      string ImportSonstiges,
      string ImportZwischenZeichen,
      string AZZR,
      string AZZB,
      string AZZA,
      string AZAR, 
      string AZ1, 
      string AZ2, 
      string AZ3, 
      string AZ4, 
      string AZ5, 
      string AZ6, 
      string AZ7, 
      string AZ8, 
      string AZ9, 
      string AZ10,
      string SG1, string SG2, string SG3, string SG4, string SG5, string SG6, string SG7, string SG8, string SG9, bool AbschlosseneVerfahren, string Laufwerk_BSCW_Server)
        {
            _SenatArt = SenatArt;
            _SenatNummer = SenatNummer;
            _SenatZusatz = SenatZusatz;
            _ShowSitzungsplaene = Sitzungsplaene;
            _ShowKanzlei = Kanzlei;
            _ShowSpruchgruppen = Spruchgruppen;
            _ShowCkbVerteil = Verteilung;
            _ShowVotenmappe = Votenmappe;
            _ImportAGBeschluss = ImportAGBeschluss;
            _ImportAGUrteil = ImportAGUrteil;
            _ImportLGUrteil = ImportLGUrteil;
            _ImportLGBeschluss = ImportLGBeschluss;
            _ImportEntscheidungsentwurf = ImportEntscheidungsentwurf;
            _ImportVotum = ImportVotum;
            _ImportVorVotum = ImportVorVotum;
            _ImportOLGUrteil = ImportOLGUrteil;
            _ImportOLGBeschluss = ImportOLGBeschluss;
            _ImportOLGHB = ImportOLGHB;
            _ImportOLGZB = ImportOLGZB;
            _ImportLGHB = ImportLGHB;
            _ImportLGZB = ImportLGZB;
            _ImportShowAZ = ImportShowAZ;
            _ImportEntscheidungenDetails = ImportEntscheidungenDetails;
            _ImportAnlage = ImportAnlage;
            _ImportLeitsatz = ImportLeitsatz;
            _ImportRMB = ImportRMB;
            _ImportRME = ImportRME;
            _ImportEuGHVorlage = ImportEuGHVorlage;
            _ImportEuGHUrteil = ImportEuGHUrteil;
            _ImportSonstiges = ImportSonstiges;
            _ImportZwischenZeichen = ImportZwischenZeichen;
            _AZZR = "ZR";
            _AZZB = "ZB";
            _AZZA = "ZA";
            _AZAR = AZAR;
            _AZ1 = AZ1;
            _AZ2 = AZ2;
            _AZ3 = AZ3;
            _AZ4 = AZ4; 
            _AZ5 = AZ5;
            _AZ6 = AZ6;
            _AZ7 = AZ7;
            _AZ8 = AZ8;
            _AZ9 = AZ9;
            _AZ10 = AZ10;
            _testAnzeige = DateTime.Now.ToString();
            _SG1 = SG1;
            _SG2 = SG2;
            _SG3 = SG3;
            _SG4 = SG4;
            _SG5 = SG5;
            _SG6 = SG6;
            _SG7 = SG7;
            _SG8 = SG8;
            _SG9 = SG9;
            _AbschlosseneVerfahren = AbschlosseneVerfahren;
            _Laufwerk_BSCW_Server = Laufwerk_BSCW_Server;
        }

    }
}
