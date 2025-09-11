using BGH_Kompakt.Classes.UserClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.UserClasses
{
    public class UserFilterMP
    {
        public int UserFilterMPID { get; set; }
        #region Zivilsenatefilter
        public bool ZivilGesamt { get; set; } = true;
        public bool ZivilGesamtLS { get; set; } = true;
        public bool ZivilSenat1 { get; set; } = true;
        public bool ZivilSenat1LS { get; set; } = true;
        public bool ZivilSenat2 { get; set; } = true;
        public bool ZivilSenat2LS { get; set; } = true;
        public bool ZivilSenat3 { get; set; } = true;
        public bool ZivilSenat3LS { get; set; } = true;
        public bool ZivilSenat4 { get; set; } = true;
        public bool ZivilSenat4LS { get; set; } = true;
        public bool ZivilSenat5 { get; set; } = true;
        public bool ZivilSenat5LS { get; set; } = true;
        public bool ZivilSenat6 { get; set; } = true;
        public bool ZivilSenat6LS { get; set; } = true;
        public bool ZivilSenat6a { get; set; } = true;
        public bool ZivilSenat6aLS { get; set; } = true;
        public bool ZivilSenat7 { get; set; } = true;
        public bool ZivilSenat7LS { get; set; } = true;
        public bool ZivilSenat8 { get; set; } = true;
        public bool ZivilSenat8LS { get; set; } = true;
        public bool ZivilSenat9 { get; set; } = true;
        public bool ZivilSenat9LS { get; set; } = true;
        public bool ZivilSenat10 { get; set; } = true;
        public bool ZivilSenat10LS { get; set; } = true;
        public bool ZivilSenat11 { get; set; } = true;
        public bool ZivilSenat11LS { get; set; } = true;
        public bool ZivilSenat12 { get; set; } = true;
        public bool ZivilSenat12LS { get; set; } = true;
        public bool ZivilSenat13 { get; set; } = true;
        public bool ZivilSenat13LS { get; set; } = true;

        #endregion
        #region Strafsentefilter
        public bool StrafGesamt { get; set; } = true;
        public bool StrafGesamtLS { get; set; } = true;
        public bool StrafSenat1 { get; set; } = true;
        public bool StrafSenat1LS { get; set; } = true;
        public bool StrafSenat2 { get; set; } = true;
        public bool StrafSenat2LS { get; set; } = true;
        public bool StrafSenat3 { get; set; } = true;
        public bool StrafSenat3LS { get; set; } = true;
        public bool StrafSenat4 { get; set; } = true;
        public bool StrafSenat4LS { get; set; } = true;
        public bool StrafSenat5 { get; set; } = true;
        public bool StrafSenat5LS { get; set; } = true;
        public bool StrafSenat6 { get; set; } = true;
        public bool StrafSenat6LS { get; set; } = true;

        #endregion
        #region Sondersenatefilter
        public bool SonderGesamt { get; set; } = true;
        public bool SonderGesamtLS { get; set; } = true;
        public bool GmSOG { get; set; } = true;
        public bool GmSOGLS { get; set; } = true;
        public bool VGS { get; set; } = true;
        public bool VGSLS { get; set; } = true;
        public bool GZS { get; set; } = true;
        public bool GZSLS { get; set; } = true;
        public bool GStS { get; set; } = true;
        public bool GStSLS { get; set; } = true;
        public bool Anwaltssenat { get; set; } = true;
        public bool AnwaltssenatLS { get; set; } = true;
        public bool Patentanwaltssenat { get; set; } = true;
        public bool PatentanwaltssenatLS { get; set; } = true;
        public bool Notarsenat { get; set; } = true;
        public bool NotarsenatLS { get; set; } = true;
        public bool Steuerberater { get; set; } = true;
        public bool SteuerberaterLS { get; set; } = true;
        public bool Dienstgericht { get; set; } = true;
        public bool DienstgerichtLS { get; set; } = true;
        public bool Kartellsenat { get; set; } = true;
        public bool KartellsenatLS { get; set; } = true;
        public bool Landwirtschaftssenat { get; set; } = true;
        public bool LandwirtschaftssenatLS { get; set; } = true;
        public bool Wirtschaftspruefersenat { get; set; } = true;
        public bool WirtschaftspruefersenatLS { get; set; } = true;

        #endregion
        #region Sonstige
        public bool Urteile { get; set; } = true; 
        public bool Beschluesse { get; set; } = true; 
        public bool Leitsatzentscheidung { get; set; } = false;
        public bool AscSorting { get; set; } = true;

        #endregion

        public User User { get; set; }

    }
}
