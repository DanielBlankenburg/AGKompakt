using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.Views;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Montagspost
{
    public class MontagsPostFilterViewModel : ViewModelBase
    {
        private UserDBContext userDBContext;
        private UserFilterMP _Filter;
        public UserFilterMP Filter
        {
            get { return _Filter; }
            set
            {
                SetProperty<UserFilterMP>(ref _Filter, value);
                //if (_Filter.StrafSenat1 || _Filter.StrafSenat2 || _Filter.StrafSenat3 || _Filter.StrafSenat4 || _Filter.StrafSenat5 || _Filter.StrafSenat6)
                //{
                //    _Filter.StrafGesamt = true;
                //}
            }
        }

        private bool _ChangeAll = true;

        private bool _ZivilsenateGesamt;
        public bool ZivilsenateGesamt
        {
            get { return _ZivilsenateGesamt; }
            set 
            { 
                SetProperty<bool>(ref _ZivilsenateGesamt, value);
                if (_ChangeAll )
                {
                    Zivilsenat1 = ZivilsenateGesamt;
                    Zivilsenat2 = ZivilsenateGesamt;
                    Zivilsenat3 = ZivilsenateGesamt;
                    Zivilsenat4 = ZivilsenateGesamt;
                    Zivilsenat5 = ZivilsenateGesamt;
                    Zivilsenat6 = ZivilsenateGesamt;
                    Zivilsenat6a = ZivilsenateGesamt;
                    Zivilsenat7 = ZivilsenateGesamt;
                    Zivilsenat8 = ZivilsenateGesamt;
                    Zivilsenat9 = ZivilsenateGesamt;
                    Zivilsenat10 = ZivilsenateGesamt;
                    Zivilsenat11 = ZivilsenateGesamt;
                    Zivilsenat12 = ZivilsenateGesamt;
                    Zivilsenat13 = ZivilsenateGesamt;
                }
            }
        }
        private bool _Zivilsenat1;
        public bool Zivilsenat1
        {
            get { return _Zivilsenat1; }
            set
            {
                SetProperty<bool>(ref _Zivilsenat1, value);
                ZivilsenateGesamtSet(_Zivilsenat1);
            }
        }
        private bool _Zivilsenat2;
        public bool Zivilsenat2
        {
            get { return _Zivilsenat2; }
            set
            {
                SetProperty<bool>(ref _Zivilsenat2, value);
                ZivilsenateGesamtSet(_Zivilsenat2);
            }
        }
        private bool _Zivilsenat3;
        public bool Zivilsenat3
        {
            get { return _Zivilsenat3; }
            set
            {
                SetProperty<bool>(ref _Zivilsenat3, value);
                ZivilsenateGesamtSet(_Zivilsenat3);
            }
        }
        private bool _Zivilsenat4;
        public bool Zivilsenat4
        {
            get { return _Zivilsenat4; }
            set
            {
                SetProperty<bool>(ref _Zivilsenat4, value);
                ZivilsenateGesamtSet(_Zivilsenat4);
            }
        }
        private bool _Zivilsenat5;
        public bool Zivilsenat5
        {
            get { return _Zivilsenat5; }
            set
            {
                SetProperty<bool>(ref _Zivilsenat5, value);
                ZivilsenateGesamtSet(_Zivilsenat5);
            }
        }
        private bool _Zivilsenat6;
        public bool Zivilsenat6
        {
            get { return _Zivilsenat6; }
            set
            {
                SetProperty<bool>(ref _Zivilsenat6, value);
                ZivilsenateGesamtSet(_Zivilsenat6);
            }
        }
        private bool _Zivilsenat6a;
        public bool Zivilsenat6a
        {
            get { return _Zivilsenat6a; }
            set
            {
                SetProperty<bool>(ref _Zivilsenat6a, value);
                ZivilsenateGesamtSet(_Zivilsenat6a);
            }
        }
        private bool _Zivilsenat7;
        public bool Zivilsenat7
        {
            get { return _Zivilsenat7; }
            set
            {
                SetProperty<bool>(ref _Zivilsenat7, value);
                ZivilsenateGesamtSet(_Zivilsenat7);
            }
        }
        private bool _Zivilsenat8;
        public bool Zivilsenat8
        {
            get { return _Zivilsenat8; }
            set
            {
                SetProperty<bool>(ref _Zivilsenat8, value);
                ZivilsenateGesamtSet(_Zivilsenat8);
            }
        }
        private bool _Zivilsenat9;
        public bool Zivilsenat9
        {
            get { return _Zivilsenat9; }
            set
            {
                SetProperty<bool>(ref _Zivilsenat9, value);
                ZivilsenateGesamtSet(_Zivilsenat9);
            }
        }
        private bool _Zivilsenat10;
        public bool Zivilsenat10
        {
            get { return _Zivilsenat10; }
            set
            {
                SetProperty<bool>(ref _Zivilsenat10, value);
                ZivilsenateGesamtSet(_Zivilsenat10);
            }
        }
        private bool _Zivilsenat11;
        public bool Zivilsenat11
        {
            get { return _Zivilsenat11; }
            set
            {
                SetProperty<bool>(ref _Zivilsenat11, value);
                ZivilsenateGesamtSet(_Zivilsenat11);
            }
        }
        private bool _Zivilsenat12;
        public bool Zivilsenat12
        {
            get { return _Zivilsenat12; }
            set
            {
                SetProperty<bool>(ref _Zivilsenat12, value);
                ZivilsenateGesamtSet(_Zivilsenat12);
            }
        }
        private bool _Zivilsenat13;
        public bool Zivilsenat13
        {
            get { return _Zivilsenat13; }
            set
            {
                SetProperty<bool>(ref _Zivilsenat13, value);
                ZivilsenateGesamtSet(_Zivilsenat13);
            }
        }
        private bool _ZivilsenateGesamtLS;
        public bool ZivilsenateGesamtLS
        {
            get { return _ZivilsenateGesamtLS; }
            set 
            { 
                SetProperty<bool>(ref _ZivilsenateGesamtLS, value);
                if (_ChangeAll )
                {
                    ZivilsenatLS1 = ZivilsenateGesamtLS;
                    ZivilsenatLS2 = ZivilsenateGesamtLS;
                    ZivilsenatLS3 = ZivilsenateGesamtLS;
                    ZivilsenatLS4 = ZivilsenateGesamtLS;
                    ZivilsenatLS5 = ZivilsenateGesamtLS;
                    ZivilsenatLS6 = ZivilsenateGesamtLS;
                    ZivilsenatLS6a = ZivilsenateGesamtLS;
                    ZivilsenatLS7 = ZivilsenateGesamtLS;
                    ZivilsenatLS8 = ZivilsenateGesamtLS;
                    ZivilsenatLS9 = ZivilsenateGesamtLS;
                    ZivilsenatLS10 = ZivilsenateGesamtLS;
                    ZivilsenatLS11 = ZivilsenateGesamtLS;
                    ZivilsenatLS12 = ZivilsenateGesamtLS;
                    ZivilsenatLS13 = ZivilsenateGesamtLS;
                }
            }
        }
        private bool _ZivilsenatLS1;
        public bool ZivilsenatLS1
        {
            get { return _ZivilsenatLS1; }
            set
            {
                SetProperty<bool>(ref _ZivilsenatLS1, value);
                ZivilsenateGesamtLSSet(_ZivilsenatLS1);
            }
        }
        private bool _ZivilsenatLS2;
        public bool ZivilsenatLS2
        {
            get { return _ZivilsenatLS2; }
            set
            {
                SetProperty<bool>(ref _ZivilsenatLS2, value);
                ZivilsenateGesamtLSSet(_ZivilsenatLS2);
            }
        }
        private bool _ZivilsenatLS3;
        public bool ZivilsenatLS3
        {
            get { return _ZivilsenatLS3; }
            set
            {
                SetProperty<bool>(ref _ZivilsenatLS3, value);
                ZivilsenateGesamtLSSet(_ZivilsenatLS3);
            }
        }
        private bool _ZivilsenatLS4;
        public bool ZivilsenatLS4
        {
            get { return _ZivilsenatLS4; }
            set
            {
                SetProperty<bool>(ref _ZivilsenatLS4, value);
                ZivilsenateGesamtLSSet(_ZivilsenatLS4);
            }
        }
        private bool _ZivilsenatLS5;
        public bool ZivilsenatLS5
        {
            get { return _ZivilsenatLS5; }
            set
            {
                SetProperty<bool>(ref _ZivilsenatLS5, value);
                ZivilsenateGesamtLSSet(_ZivilsenatLS5);
            }
        }
        private bool _ZivilsenatLS6;
        public bool ZivilsenatLS6
        {
            get { return _ZivilsenatLS6; }
            set
            {
                SetProperty<bool>(ref _ZivilsenatLS6, value);
                ZivilsenateGesamtLSSet(_ZivilsenatLS6);
            }
        }
        private bool _ZivilsenatLS6a;
        public bool ZivilsenatLS6a
        {
            get { return _ZivilsenatLS6a; }
            set
            {
                SetProperty<bool>(ref _ZivilsenatLS6a, value);
                ZivilsenateGesamtLSSet(_ZivilsenatLS6a);
            }
        }
        private bool _ZivilsenatLS7;
        public bool ZivilsenatLS7
        {
            get { return _ZivilsenatLS7; }
            set
            {
                SetProperty<bool>(ref _ZivilsenatLS7, value);
                ZivilsenateGesamtLSSet(_ZivilsenatLS7);
            }
        }
        private bool _ZivilsenatLS8;
        public bool ZivilsenatLS8
        {
            get { return _ZivilsenatLS8; }
            set
            {
                SetProperty<bool>(ref _ZivilsenatLS8, value);
                ZivilsenateGesamtLSSet(_ZivilsenatLS8);
            }
        }
        private bool _ZivilsenatLS9;
        public bool ZivilsenatLS9
        {
            get { return _ZivilsenatLS9; }
            set
            {
                SetProperty<bool>(ref _ZivilsenatLS9, value);
                ZivilsenateGesamtLSSet(_ZivilsenatLS9);
            }
        }
        private bool _ZivilsenatLS10;
        public bool ZivilsenatLS10
        {
            get { return _ZivilsenatLS10; }
            set
            {
                SetProperty<bool>(ref _ZivilsenatLS10, value);
                ZivilsenateGesamtLSSet(_ZivilsenatLS10);
            }
        }
        private bool _ZivilsenatLS11;
        public bool ZivilsenatLS11
        {
            get { return _ZivilsenatLS11; }
            set
            {
                SetProperty<bool>(ref _ZivilsenatLS11, value);
                ZivilsenateGesamtLSSet(_ZivilsenatLS11);
            }
        }
        private bool _ZivilsenatLS12;
        public bool ZivilsenatLS12
        {
            get { return _ZivilsenatLS12; }
            set
            {
                SetProperty<bool>(ref _ZivilsenatLS12, value);
                ZivilsenateGesamtLSSet(_ZivilsenatLS12);
            }
        }
        private bool _ZivilsenatLS13;
        public bool ZivilsenatLS13
        {
            get { return _ZivilsenatLS13; }
            set
            {
                SetProperty<bool>(ref _ZivilsenatLS13, value);
                ZivilsenateGesamtLSSet(_ZivilsenatLS13);
            }
        }

        private bool _StrafsenateGesamt;
        public bool StrafsenateGesamt
        {
            get { return _StrafsenateGesamt; }
            set 
            { 
                SetProperty<bool>(ref _StrafsenateGesamt, value);
                if (_ChangeAll)
                {
                    Strafsenat1 = StrafsenateGesamt;
                    Strafsenat2 = StrafsenateGesamt;
                    Strafsenat3 = StrafsenateGesamt;
                    Strafsenat4 = StrafsenateGesamt;
                    Strafsenat5 = StrafsenateGesamt;
                    Strafsenat6 = StrafsenateGesamt;

                }
            }
        }
        private bool _Strafsenat1;
        public bool Strafsenat1
        {
            get { return _Strafsenat1; }
            set 
            { 
                SetProperty<bool>(ref _Strafsenat1, value);
                StrafsenateGesamtSet(_Strafsenat1);
            }
        }
        private bool _Strafsenat2;
        public bool Strafsenat2
        {
            get { return _Strafsenat2; }
            set
            {
                SetProperty<bool>(ref _Strafsenat2, value);
                StrafsenateGesamtSet(_Strafsenat2);
            }
        }
        private bool _Strafsenat3;
        public bool Strafsenat3
        {
            get { return _Strafsenat3; }
            set
            {
                SetProperty<bool>(ref _Strafsenat3, value);
                StrafsenateGesamtSet(_Strafsenat3);
            }
        }
        private bool _Strafsenat4;
        public bool Strafsenat4
        {
            get { return _Strafsenat4; }
            set
            {
                SetProperty<bool>(ref _Strafsenat4, value);
                StrafsenateGesamtSet(_Strafsenat4);
            }
        }
        private bool _Strafsenat5;
        public bool Strafsenat5
        {
            get { return _Strafsenat5; }
            set
            {
                SetProperty<bool>(ref _Strafsenat5, value);
                StrafsenateGesamtSet(_Strafsenat5);
            }
        }
        private bool _Strafsenat6;
        public bool Strafsenat6
        {
            get { return _Strafsenat6; }
            set
            {
                SetProperty<bool>(ref _Strafsenat6, value);
                StrafsenateGesamtSet(_Strafsenat6);
            }
        }
        private bool _StrafsenateGesamtLS;
        public bool StrafsenateGesamtLS
        {
            get { return _StrafsenateGesamtLS; }
            set 
            { 
                SetProperty<bool>(ref _StrafsenateGesamtLS, value);
                if (_ChangeAll)
                {
                    StrafsenatLS1 = StrafsenateGesamtLS;
                    StrafsenatLS2 = StrafsenateGesamtLS;
                    StrafsenatLS3 = StrafsenateGesamtLS;
                    StrafsenatLS4 = StrafsenateGesamtLS;
                    StrafsenatLS5 = StrafsenateGesamtLS;
                    StrafsenatLS6 = StrafsenateGesamtLS;

                }
            }
        }
        private bool _StrafsenatLS1;
        public bool StrafsenatLS1
        {
            get { return _StrafsenatLS1; }
            set 
            { 
                SetProperty<bool>(ref _StrafsenatLS1, value);
                StrafsenateGesamtLSSet(_StrafsenatLS1);
            }
        }
        private bool _StrafsenatLS2;
        public bool StrafsenatLS2
        {
            get { return _StrafsenatLS2; }
            set
            {
                SetProperty<bool>(ref _StrafsenatLS2, value);
                StrafsenateGesamtLSSet(_StrafsenatLS2);
            }
        }
        private bool _StrafsenatLS3;
        public bool StrafsenatLS3
        {
            get { return _StrafsenatLS3; }
            set
            {
                SetProperty<bool>(ref _StrafsenatLS3, value);
                StrafsenateGesamtLSSet(_StrafsenatLS3);
            }
        }
        private bool _StrafsenatLS4;
        public bool StrafsenatLS4
        {
            get { return _StrafsenatLS4; }
            set
            {
                SetProperty<bool>(ref _StrafsenatLS4, value);
                StrafsenateGesamtLSSet(_StrafsenatLS4);
            }
        }
        private bool _StrafsenatLS5;
        public bool StrafsenatLS5
        {
            get { return _StrafsenatLS5; }
            set
            {
                SetProperty<bool>(ref _StrafsenatLS5, value);
                StrafsenateGesamtLSSet(_StrafsenatLS5);
            }
        }
        private bool _StrafsenatLS6;
        public bool StrafsenatLS6
        {
            get { return _StrafsenatLS6; }
            set
            {
                SetProperty<bool>(ref _StrafsenatLS6, value);
                StrafsenateGesamtLSSet(_StrafsenatLS6);
            }
        }

        private bool _SondersenateGesamt;
        public bool SondersenateGesamt
        {
            get { return _SondersenateGesamt; }
            set
            { 
                SetProperty<bool>(ref _SondersenateGesamt, value);
                if (_ChangeAll)
                {
                    SondersenatAnwalt = SondersenateGesamt;
                    SondersenatNotar = SondersenateGesamt;
                    SondersenatGOBG = SondersenateGesamt;
                    SondersenatVGS = SondersenateGesamt;
                    SondersenatGZS = SondersenateGesamt;
                    SondersenatGSS= SondersenateGesamt;
                    SondersenatPatentanwalt = SondersenateGesamt;
                    SondersenatSteuerberater = SondersenateGesamt;
                    SondersenatDienstgericht = SondersenateGesamt;
                    SondersenatLandwirtschaft = SondersenateGesamt;
                    SondersenatKartell = SondersenateGesamt;
                    SondersenatWirtschaftspruefer = SondersenateGesamt;
                }

            }
        }
        private bool _SondersenatGOBG;
        public bool SondersenatGOBG
        {
            get { return _SondersenatGOBG; }
            set
            {
                SetProperty<bool>(ref _SondersenatGOBG, value);
                SondersenateGesamtSet(_SondersenatGOBG);
            }
        }
        private bool _SondersenatVGS;
        public bool SondersenatVGS
        {
            get { return _SondersenatVGS; }
            set
            {
                SetProperty<bool>(ref _SondersenatVGS, value);
                SondersenateGesamtSet(_SondersenatVGS);
            }
        }
        private bool _SondersenatGZS;
        public bool SondersenatGZS
        {
            get { return _SondersenatGZS; }
            set
            {
                SetProperty<bool>(ref _SondersenatGZS, value);
                SondersenateGesamtSet(_SondersenatGZS);
            }
        }
        private bool _SondersenatGSS;
        public bool SondersenatGSS
        {
            get { return _SondersenatGSS; }
            set
            {
                SetProperty<bool>(ref _SondersenatGSS, value);
                SondersenateGesamtSet(_SondersenatGSS);
            }
        }
        private bool _SondersenatAnwalt;
        public bool SondersenatAnwalt
        {
            get { return _SondersenatAnwalt; }
            set
            {
                SetProperty<bool>(ref _SondersenatAnwalt, value);
                SondersenateGesamtSet(_SondersenatAnwalt);
            }
        }
        private bool _SondersenatNotar;
        public bool SondersenatNotar
        {
            get { return _SondersenatNotar; }
            set
            {
                SetProperty<bool>(ref _SondersenatNotar, value);
                SondersenateGesamtSet(_SondersenatNotar);
            }
        }
        private bool _SondersenatSteuerberater;
        public bool SondersenatSteuerberater
        {
            get { return _SondersenatSteuerberater; }
            set
            {
                SetProperty<bool>(ref _SondersenatSteuerberater, value);
                SondersenateGesamtSet(_SondersenatSteuerberater);
            }
        }
        private bool _SondersenatLandwirtschaft;
        public bool SondersenatLandwirtschaft
        {
            get { return _SondersenatLandwirtschaft; }
            set
            {
                SetProperty<bool>(ref _SondersenatLandwirtschaft, value);
                SondersenateGesamtSet(_SondersenatLandwirtschaft);
            }
        }
        private bool _SondersenatDienstgericht;
        public bool SondersenatDienstgericht
        {
            get { return _SondersenatDienstgericht; }
            set
            {
                SetProperty<bool>(ref _SondersenatDienstgericht, value);
                SondersenateGesamtSet(_SondersenatDienstgericht);
            }
        }
        private bool _SondersenatPatentanwalt;
        public bool SondersenatPatentanwalt
        {
            get { return _SondersenatPatentanwalt; }
            set
            {
                SetProperty<bool>(ref _SondersenatPatentanwalt, value);
                SondersenateGesamtSet(_SondersenatPatentanwalt);
            }
        }
        private bool _SondersenatKartell;
        public bool SondersenatKartell
        {
            get { return _SondersenatKartell; }
            set
            {
                SetProperty<bool>(ref _SondersenatKartell, value);
                SondersenateGesamtSet(_SondersenatKartell);
            }
        }
        private bool _SondersenatWirtschaftspruefer;
        public bool SondersenatWirtschaftspruefer
        {
            get { return _SondersenatWirtschaftspruefer; }
            set
            {
                SetProperty<bool>(ref _SondersenatWirtschaftspruefer, value);
                SondersenateGesamtSet(_SondersenatWirtschaftspruefer);
            }
        }
        private bool _SondersenateGesamtLS;
        public bool SondersenateGesamtLS
        {
            get { return _SondersenateGesamtLS; }
            set
            { 
                SetProperty<bool>(ref _SondersenateGesamtLS, value);
                if (_ChangeAll)
                {
                    SondersenatAnwaltLS = SondersenateGesamtLS;
                    SondersenatNotarLS = SondersenateGesamtLS;
                    SondersenatGOBGLS = SondersenateGesamtLS;
                    SondersenatVGSLS = SondersenateGesamtLS;
                    SondersenatGZSLS = SondersenateGesamtLS;
                    SondersenatGSSLS = SondersenateGesamtLS;
                    SondersenatPatentanwaltLS = SondersenateGesamtLS;
                    SondersenatSteuerberaterLS = SondersenateGesamtLS;
                    SondersenatDienstgerichtLS = SondersenateGesamtLS;
                    SondersenatLandwirtschaftLS = SondersenateGesamtLS;
                    SondersenatKartellLS = SondersenateGesamtLS;
                    SondersenatWirtschaftsprueferLS = SondersenateGesamtLS;
                }

            }
        }
        private bool _SondersenatGOBGLS;
        public bool SondersenatGOBGLS
        {
            get { return _SondersenatGOBGLS; }
            set
            {
                SetProperty<bool>(ref _SondersenatGOBGLS, value);
                SondersenateGesamtSet(_SondersenatGOBGLS);
            }
        }
        private bool _SondersenatVGSLS;
        public bool SondersenatVGSLS
        {
            get { return _SondersenatVGSLS; }
            set
            {
                SetProperty<bool>(ref _SondersenatVGSLS, value);
                SondersenateGesamtSet(_SondersenatVGSLS);
            }
        }
        private bool _SondersenatGZSLS;
        public bool SondersenatGZSLS
        {
            get { return _SondersenatGZSLS; }
            set
            {
                SetProperty<bool>(ref _SondersenatGZSLS, value);
                SondersenateGesamtSet(_SondersenatGZSLS);
            }
        }
        private bool _SondersenatGSSLS;
        public bool SondersenatGSSLS
        {
            get { return _SondersenatGSSLS; }
            set
            {
                SetProperty<bool>(ref _SondersenatGSSLS, value);
                SondersenateGesamtSet(_SondersenatGSSLS);
            }
        }
        private bool _SondersenatAnwaltLS;
        public bool SondersenatAnwaltLS
        {
            get { return _SondersenatAnwaltLS; }
            set
            {
                SetProperty<bool>(ref _SondersenatAnwaltLS, value);
                SondersenateGesamtSet(_SondersenatAnwaltLS);
            }
        }
        private bool _SondersenatNotarLS;
        public bool SondersenatNotarLS
        {
            get { return _SondersenatNotarLS; }
            set
            {
                SetProperty<bool>(ref _SondersenatNotarLS, value);
                SondersenateGesamtSet(_SondersenatNotarLS);
            }
        }
        private bool _SondersenatSteuerberaterLS;
        public bool SondersenatSteuerberaterLS
        {
            get { return _SondersenatSteuerberaterLS; }
            set
            {
                SetProperty<bool>(ref _SondersenatSteuerberaterLS, value);
                SondersenateGesamtSet(_SondersenatSteuerberaterLS);
            }
        }
        private bool _SondersenatLandwirtschaftLS;
        public bool SondersenatLandwirtschaftLS
        {
            get { return _SondersenatLandwirtschaftLS; }
            set
            {
                SetProperty<bool>(ref _SondersenatLandwirtschaftLS, value);
                SondersenateGesamtSet(_SondersenatLandwirtschaftLS);
            }
        }
        private bool _SondersenatDienstgerichtLS;
        public bool SondersenatDienstgerichtLS
        {
            get { return _SondersenatDienstgerichtLS; }
            set
            {
                SetProperty<bool>(ref _SondersenatDienstgerichtLS, value);
                SondersenateGesamtSet(_SondersenatDienstgerichtLS);
            }
        }
        private bool _SondersenatPatentanwaltLS;
        public bool SondersenatPatentanwaltLS
        {
            get { return _SondersenatPatentanwaltLS; }
            set
            {
                SetProperty<bool>(ref _SondersenatPatentanwaltLS, value);
                SondersenateGesamtSet(_SondersenatPatentanwaltLS);
            }
        }
        private bool _SondersenatKartellLS;
        public bool SondersenatKartellLS
        {
            get { return _SondersenatKartellLS; }
            set
            {
                SetProperty<bool>(ref _SondersenatKartellLS, value);
                SondersenateGesamtSet(_SondersenatKartellLS);
            }
        }
        private bool _SondersenatWirtschaftsprueferLS;
        public bool SondersenatWirtschaftsprueferLS
        {
            get { return _SondersenatWirtschaftsprueferLS; }
            set
            {
                SetProperty<bool>(ref _SondersenatWirtschaftsprueferLS, value);
                SondersenateGesamtSet(_SondersenatWirtschaftsprueferLS);
            }
        }


        private bool _Urteile;
        public bool Urteile
        {
            get { return _Urteile; }
            set { SetProperty<bool>(ref _Urteile, value); }
        }
        private bool _Beschlüsse;
        public bool Beschlüsse
        {
            get { return _Beschlüsse; }
            set { SetProperty<bool>(ref _Beschlüsse, value); }
        }
        private bool _Leitsatzentscheidungen;
        public bool Leitsatzentscheidungen
        {
            get { return _Leitsatzentscheidungen; }
            set { SetProperty<bool>(ref _Leitsatzentscheidungen, value); }
        }

        public void StrafsenateGesamtSet(bool Anzeige)
        {
            _ChangeAll = false;
            if (Anzeige)
            {
                StrafsenateGesamt = true;
            }
            else
            {
                if (!_Strafsenat1 && 
                    !_Strafsenat2 && 
                    !_Strafsenat3 && 
                    !_Strafsenat4 && 
                    !_Strafsenat5 && 
                    !_Strafsenat6)
                {
                    StrafsenateGesamt = false;
                }
            }
            _ChangeAll = true;
        }
        public void StrafsenateGesamtLSSet(bool Anzeige)
        {
            _ChangeAll = false;
            if (Anzeige)
            {
                StrafsenateGesamtLS = true;
            }
            else
            {
                if (!_StrafsenatLS1 && 
                    !_StrafsenatLS2 && 
                    !_StrafsenatLS3 && 
                    !_StrafsenatLS4 && 
                    !_StrafsenatLS5 && 
                    !_StrafsenatLS6)
                {
                    StrafsenateGesamtLS = false;
                }
            }
            _ChangeAll = true;
        }
        public void ZivilsenateGesamtSet(bool Anzeige)
        {
            _ChangeAll = false;
            if (Anzeige)
            {
                ZivilsenateGesamt = true;
            }
            else
            {
                if (!_Zivilsenat1 && 
                    !_Zivilsenat2 && 
                    !_Zivilsenat3 && 
                    !_Zivilsenat4 && 
                    !_Zivilsenat5 && 
                    !_Zivilsenat6 && 
                    !_Zivilsenat6a && 
                    !_Zivilsenat7 && 
                    !_Zivilsenat8 && 
                    !_Zivilsenat9 && 
                    !_Zivilsenat10 && 
                    !_Zivilsenat11 && 
                    !_Zivilsenat12 && 
                    !_Zivilsenat13)
                {
                    ZivilsenateGesamt = false;
                }
            }
            _ChangeAll = true;
        }
        public void ZivilsenateGesamtLSSet(bool Anzeige)
        {
            _ChangeAll = false;
            if (Anzeige)
            {
                ZivilsenateGesamtLS = true;
            }
            else
            {
                if (!_ZivilsenatLS1 && 
                    !_ZivilsenatLS2 && 
                    !_ZivilsenatLS3 && 
                    !_ZivilsenatLS4 && 
                    !_ZivilsenatLS5 && 
                    !_ZivilsenatLS6 && 
                    !_ZivilsenatLS6a && 
                    !_ZivilsenatLS7 && 
                    !_ZivilsenatLS8 && 
                    !_ZivilsenatLS9 && 
                    !_ZivilsenatLS10 && 
                    !_ZivilsenatLS11 && 
                    !_ZivilsenatLS12 && 
                    !_ZivilsenatLS13)
                {
                    ZivilsenateGesamtLS = false;
                }
            }
            _ChangeAll = true;
        }
        public void SondersenateGesamtSet(bool Anzeige)
        {
            _ChangeAll = false;
            if (Anzeige)
            {
                SondersenateGesamt = true;
            }
            else
            {
                if (!_SondersenatGOBG && 
                    !_SondersenatVGS && 
                    !_SondersenatGSS && 
                    !_SondersenatGZS && 
                    !_SondersenatAnwalt && 
                    !_SondersenatNotar && 
                    !_SondersenatPatentanwalt && 
                    !_SondersenatSteuerberater && 
                    !_SondersenatLandwirtschaft && 
                    !_SondersenatDienstgerichtLS && 
                    !_SondersenatKartell && 
                    !_SondersenatWirtschaftspruefer)
                {
                    SondersenateGesamt = false;
                }
            }
            _ChangeAll = true;
        }
        public void SondersenateGesamtLSSet(bool Anzeige)
        {
            _ChangeAll = false;
            if (Anzeige)
            {
                SondersenateGesamtLS = true;
            }
            else
            {
                if (!_SondersenatGOBGLS && 
                    !_SondersenatVGSLS &&
                    !_SondersenatGSSLS && 
                    !_SondersenatGZSLS && 
                    !_SondersenatAnwaltLS && 
                    !_SondersenatNotarLS && 
                    !_SondersenatPatentanwaltLS && 
                    !_SondersenatSteuerberaterLS && 
                    !_SondersenatLandwirtschaftLS && 
                    !_SondersenatDienstgerichtLS && 
                    !_SondersenatKartellLS &&
                    !_SondersenatWirtschaftsprueferLS)
                {
                    SondersenateGesamtLS = false;
                }
            }
            _ChangeAll = true;
        }

        public ICommand SaveCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand TestCommand { get; set; }


        public MontagsPostFilterViewModel()
        {
            SaveCommand = new RelayCommand(SaveExecute);
            BackCommand = new RelayCommand(BackExecute);
            TestCommand = new RelayCommand(TestExecute);

            userDBContext = new UserDBContext();
            UserFilterMP userFilterMP = userDBContext.FilterMP.Where(x => x.User.UserId == UserManager.RegistratedUser.UserId).FirstOrDefault();
            if (userFilterMP != null) _Filter = userFilterMP;
            ZivilsenateGesamt = Filter.ZivilGesamt;
            Zivilsenat1 = Filter.ZivilSenat1;
            Zivilsenat2 = Filter.ZivilSenat2;
            Zivilsenat3 = Filter.ZivilSenat3;
            Zivilsenat4 = Filter.ZivilSenat4;
            Zivilsenat5 = Filter.ZivilSenat5;
            Zivilsenat6 = Filter.ZivilSenat6;
            Zivilsenat6a = Filter.ZivilSenat6a;
            Zivilsenat7 = Filter.ZivilSenat7;
            Zivilsenat8 = Filter.ZivilSenat8;
            Zivilsenat9 = Filter.ZivilSenat9;
            Zivilsenat10 = Filter.ZivilSenat10;
            Zivilsenat11 = Filter.ZivilSenat11;
            Zivilsenat12 = Filter.ZivilSenat12;
            Zivilsenat13 = Filter.ZivilSenat13;

            ZivilsenateGesamtLS = Filter.ZivilGesamtLS;
            ZivilsenatLS1 = Filter.ZivilSenat1LS;
            ZivilsenatLS2 = Filter.ZivilSenat2LS;
            ZivilsenatLS3 = Filter.ZivilSenat3LS;
            ZivilsenatLS4 = Filter.ZivilSenat4LS;
            ZivilsenatLS5 = Filter.ZivilSenat5LS;
            ZivilsenatLS6 = Filter.ZivilSenat6LS;
            ZivilsenatLS6a = Filter.ZivilSenat6aLS;
            ZivilsenatLS7 = Filter.ZivilSenat7LS;
            ZivilsenatLS8 = Filter.ZivilSenat8LS;
            ZivilsenatLS9 = Filter.ZivilSenat9LS;
            ZivilsenatLS10 = Filter.ZivilSenat10LS;
            ZivilsenatLS11 = Filter.ZivilSenat11LS;
            ZivilsenatLS12 = Filter.ZivilSenat12LS;
            ZivilsenatLS13 = Filter.ZivilSenat13LS;

            StrafsenateGesamt = Filter.StrafGesamt;
            Strafsenat1 = Filter.StrafSenat1;
            Strafsenat2 = Filter.StrafSenat2;
            Strafsenat3 = Filter.StrafSenat3;
            Strafsenat4 = Filter.StrafSenat4;
            Strafsenat5 = Filter.StrafSenat5;
            Strafsenat6 = Filter.StrafSenat6;

            StrafsenateGesamtLS = Filter.StrafGesamtLS;
            StrafsenatLS1 = Filter.StrafSenat1LS;
            StrafsenatLS2 = Filter.StrafSenat2LS;
            StrafsenatLS3 = Filter.StrafSenat3LS;
            StrafsenatLS4 = Filter.StrafSenat4LS;
            StrafsenatLS5 = Filter.StrafSenat5LS;
            StrafsenatLS6 = Filter.StrafSenat6LS;

            SondersenateGesamt = Filter.SonderGesamt;
            SondersenatGOBG = Filter.GmSOG;
            SondersenatVGS = Filter.VGS;
            SondersenatGZS = Filter.GZS;
            SondersenatGSS = Filter.GStS;
            SondersenatAnwalt = Filter.Anwaltssenat;
            SondersenatNotar = Filter.Notarsenat;
            SondersenatPatentanwalt = Filter.Patentanwaltssenat;
            SondersenatSteuerberater = Filter.Steuerberater;
            SondersenatLandwirtschaft = Filter.Landwirtschaftssenat;
            SondersenatDienstgerichtLS = Filter.Dienstgericht;
            SondersenatKartell = Filter.Kartellsenat;
            SondersenatWirtschaftspruefer = Filter.Wirtschaftspruefersenat;

            SondersenateGesamtLS = Filter.SonderGesamtLS;
            SondersenatGOBGLS = Filter.GmSOGLS;
            SondersenatVGSLS = Filter.VGSLS;
            SondersenatGZSLS = Filter.GZSLS;
            SondersenatGSSLS = Filter.GStSLS;
            SondersenatAnwaltLS = Filter.AnwaltssenatLS;
            SondersenatNotarLS = Filter.NotarsenatLS;
            SondersenatPatentanwaltLS = Filter.PatentanwaltssenatLS;
            SondersenatSteuerberaterLS = Filter.SteuerberaterLS;
            SondersenatLandwirtschaftLS = Filter.LandwirtschaftssenatLS;
            SondersenatDienstgerichtLS = Filter.DienstgerichtLS;
            SondersenatKartellLS = Filter.KartellsenatLS;
            SondersenatWirtschaftsprueferLS = Filter.WirtschaftspruefersenatLS;

            Urteile = Filter.Urteile;
            Beschlüsse= Filter.Beschluesse;
            Leitsatzentscheidungen = Filter.Leitsatzentscheidung;
        }

        private void TestExecute(object obj)
        {
            //Filter.Zivilsenat1 = Filter.ZivilGesamt ? true : false;
        }

        private void BackExecute(object obj)
        {
            ViewManager.ShowPageOnMainView<MontagsPostView>();
        }

        private void SaveExecute(object obj)
        {
            ViewManager.ShowMainInfoFlyout("Die Filter wurden gespeichert.", false);

            Filter.ZivilGesamt = ZivilsenateGesamt;
            Filter.ZivilSenat1 = Zivilsenat1;
            Filter.ZivilSenat2 = Zivilsenat2;
            Filter.ZivilSenat3 = Zivilsenat3;
            Filter.ZivilSenat4 = Zivilsenat4;
            Filter.ZivilSenat5 = Zivilsenat5;
            Filter.ZivilSenat6 = Zivilsenat6;
            Filter.ZivilSenat6a = Zivilsenat6a;
            Filter.ZivilSenat7 = Zivilsenat7;
            Filter.ZivilSenat8 = Zivilsenat8;
            Filter.ZivilSenat9 = Zivilsenat9;
            Filter.ZivilSenat10 = Zivilsenat10;
            Filter.ZivilSenat11 = Zivilsenat11;
            Filter.ZivilSenat12 = Zivilsenat12;
            Filter.ZivilSenat13 = Zivilsenat13;

            Filter.ZivilGesamtLS = ZivilsenateGesamtLS;
            Filter.ZivilSenat1LS = ZivilsenatLS1;
            Filter.ZivilSenat2LS = ZivilsenatLS2;
            Filter.ZivilSenat3LS = ZivilsenatLS3;
            Filter.ZivilSenat4LS = ZivilsenatLS4;
            Filter.ZivilSenat5LS = ZivilsenatLS5;
            Filter.ZivilSenat6LS = ZivilsenatLS6;
            Filter.ZivilSenat6aLS = ZivilsenatLS6a;
            Filter.ZivilSenat7LS = ZivilsenatLS7;
            Filter.ZivilSenat8LS = ZivilsenatLS8;
            Filter.ZivilSenat9LS = ZivilsenatLS9;
            Filter.ZivilSenat10LS = ZivilsenatLS10;
            Filter.ZivilSenat11LS = ZivilsenatLS11;
            Filter.ZivilSenat12LS = ZivilsenatLS12;
            Filter.ZivilSenat13LS = ZivilsenatLS13;

            Filter.StrafGesamt = StrafsenateGesamt;
            Filter.StrafSenat1 = Strafsenat1;
            Filter.StrafSenat2 = Strafsenat2;
            Filter.StrafSenat3 = Strafsenat3;
            Filter.StrafSenat4 = Strafsenat4;
            Filter.StrafSenat5 = Strafsenat5;
            Filter.StrafSenat6 = Strafsenat6;

            Filter.StrafGesamtLS = StrafsenateGesamtLS;
            Filter.StrafSenat1LS = StrafsenatLS1;
            Filter.StrafSenat2LS = StrafsenatLS2;
            Filter.StrafSenat3LS = StrafsenatLS3;
            Filter.StrafSenat4LS = StrafsenatLS4;
            Filter.StrafSenat5LS = StrafsenatLS5;
            Filter.StrafSenat6LS = StrafsenatLS6;

            Filter.SonderGesamt = SondersenateGesamt;
            Filter.VGS = SondersenatVGS;
            Filter.GmSOG = SondersenatGOBG;
            Filter.GZS = SondersenatGZS;
            Filter.GStS= SondersenatGSS;
            Filter.Anwaltssenat = SondersenatAnwalt;
            Filter.Notarsenat = SondersenatNotar;
            Filter.Patentanwaltssenat = SondersenatPatentanwalt;
            Filter.Steuerberater= SondersenatSteuerberater;
            Filter.Landwirtschaftssenat = SondersenatLandwirtschaft;
            Filter.Dienstgericht = SondersenatDienstgericht;
            Filter.Kartellsenat = SondersenatKartell;
            Filter.Wirtschaftspruefersenat = SondersenatWirtschaftspruefer;

            Filter.SonderGesamtLS = SondersenateGesamtLS;
            Filter.GmSOGLS = SondersenatGOBGLS;
            Filter.VGSLS = SondersenatVGSLS;
            Filter.GZSLS = SondersenatGZSLS;
            Filter.GStSLS = SondersenatGSSLS;
            Filter.AnwaltssenatLS = SondersenatAnwaltLS;
            Filter.NotarsenatLS = SondersenatNotarLS;
            Filter.PatentanwaltssenatLS = SondersenatPatentanwaltLS;
            Filter.SteuerberaterLS = SondersenatSteuerberaterLS;
            Filter.LandwirtschaftssenatLS = SondersenatLandwirtschaftLS;
            Filter.DienstgerichtLS = SondersenatDienstgerichtLS;
            Filter.KartellsenatLS = SondersenatKartellLS;
            Filter.WirtschaftspruefersenatLS = SondersenatWirtschaftsprueferLS;


            Filter.Urteile = Urteile;
            Filter.Beschluesse = Beschlüsse;
            Filter.Leitsatzentscheidung = Leitsatzentscheidungen;

            userDBContext.FilterMP.AddOrUpdate(Filter);
            userDBContext.SaveChanges();

            ViewManager.ShowPageOnMainView<MontagsPostView>();
        }
    }
}
