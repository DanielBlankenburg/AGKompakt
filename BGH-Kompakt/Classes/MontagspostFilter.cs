using BGH_Kompakt.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class MontagspostFilter 
    {
        public bool ZivilSenate { get; set; }
        public bool Strafsenate { get; set; }
        public bool Sondersenate { get; set; }
        public bool Zivilsenat1 { get; set; }
        public bool Zivilsenat2 { get; set; }
        public bool Zivilsenat3 { get; set; }
        public bool Zivilsenat4 { get; set; }
        public bool Zivilsenat5 { get; set; }
        public bool Zivilsenat6 { get; set; }
        public bool Zivilsenat6a { get; set; }
        public bool Zivilsenat7 { get; set; }
        public bool Zivilsenat8 { get; set; }
        public bool Zivilsenat9 { get; set; }
        public bool Zivilsenat10 { get; set; }
        public bool Zivilsenat11 { get; set; }
        public bool Zivilsenat12 { get; set; }
        public bool Zivilsenat13 { get; set; }
        public bool Strafsenat1 { get; set; }
        public bool Strafsenat2 { get; set; }
        public bool Strafsenat3 { get; set; }
        public bool Strafsenat4 { get; set; }
        public bool Strafsenat5 { get; set; }
        public bool Strafsenat6 { get; set; }
        public bool SondersenatGOBG { get; set; }
        public bool SondersenatGZS { get; set; }
        public bool SondersenatGSS { get; set; }
        public bool SondersenatAnwalt { get; set; }
        public bool SondersenatNotar { get; set; }
        public bool SondersenatSteuerberater { get; set; }
        public bool SondersenatLandwirtschaft { get; set; }
        public bool SondersenatDienstgericht { get; set; }
        public bool SondersenatPatentanwalt { get; set; }
        public bool SondersenatKartell { get; set; }
        public bool Urteile { get; set; }
        public bool Beschlüsse { get; set; }
        public bool Leitsatzentscheidungen { get; set; }
        public string TestString{ get; set; }

        public MontagspostFilter() { }

        public MontagspostFilter(bool zivilsenate, bool strafsenate, bool sondersenate, bool zivilsenat1, bool zivilsenat2, bool zivilsenat3, bool zivilsenat4, bool zivilsenat5
            , bool zivilsenat6, bool zivilsenat6a, bool zivilsenat7, bool zivilsenat8, bool zivilsenat9, bool zivilsenat10, bool zivilsenat11, bool zivilsenat12, bool zivilsenat13, bool strafsenat1
            , bool strafsenat2, bool strafsenat3, bool strafsenat4, bool strafsenat5, bool strafsenat6, bool sondersenatGOBG, bool sondersenatGZS, bool sondersenatGSS, bool sondersenatAnwalt
            , bool sondersenatNotar, bool sondersenatSteuerberater, bool sondersenatLandwirtschaft, bool sondersenatDienstgericht, bool sondersenatPatentanwalt, bool sondersenatKartell, bool urteile, bool beschlüsse, bool leitsatzentscheidungen, string teststring)
        {
            ZivilSenate = zivilsenate;
            Zivilsenat1 = zivilsenat1;
            Zivilsenat2 = zivilsenat2;
            Zivilsenat3 = zivilsenat3;
            Zivilsenat4 = zivilsenat4;
            Zivilsenat5 = zivilsenat5;
            Zivilsenat6 = zivilsenat6;
            Zivilsenat6a = zivilsenat6a;
            Zivilsenat7 = zivilsenat7;
            Zivilsenat8 = zivilsenat8;
            Zivilsenat9 = zivilsenat9;
            Zivilsenat10 = zivilsenat10;
            Zivilsenat11 = zivilsenat11;
            Zivilsenat12 = zivilsenat12;
            Zivilsenat13 = zivilsenat13;
            Strafsenate = strafsenate;
            Strafsenat1 = strafsenat1;
            Strafsenat2 = strafsenat2;
            Strafsenat3 = strafsenat3;
            Strafsenat4 = strafsenat4;
            Strafsenat5 = strafsenat5;  
            Strafsenat6 = strafsenat6;
            Sondersenate = sondersenate;
            SondersenatGOBG = sondersenatGOBG;
            SondersenatGSS = sondersenatGSS;
            SondersenatGZS = sondersenatGZS;
            SondersenatLandwirtschaft = sondersenatLandwirtschaft;
            SondersenatNotar = sondersenatNotar;    
            SondersenatPatentanwalt = sondersenatPatentanwalt;
            SondersenatSteuerberater = sondersenatSteuerberater;
            SondersenatAnwalt = sondersenatAnwalt;
            SondersenatDienstgericht = sondersenatDienstgericht;
            SondersenatKartell = sondersenatKartell;
            Urteile = urteile;
            Beschlüsse = beschlüsse;
            Leitsatzentscheidungen = leitsatzentscheidungen;
            TestString = teststring;
        }
    }
}
