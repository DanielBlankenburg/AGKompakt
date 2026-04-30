using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Enums
{
    public class UserEnums
    {
        public enum EnumAdminStatus
        {
            Bereitschaftsdienst, Zivilabteilung, Familienabteilung, Insolvenzabteilung
        }

        public enum EnumDienstbezeichnungen
        {
            RiAG,
            RinAG,
            PräsAG,
            PräsinAG,
            VPräsAG,
            VPräsinAG
        }

        public enum EnumSex
        {
            männlich, weiblich, divers
        }

        public enum EnumPositionTyps
        {
            //Richter am Bundesgerichtshof, Wissenschaftlicher Mitarbeiter, Geschäftsstellenmitarbeiter"
        }

        public enum EnumUserStatus
        {
            aktiv, inaktiv
        }

        public enum EnumSprachen
        {
            Englisch, Deutsch, Französisch, Spanisch, Italienisch, Russisch, Chinesisch, Japanisch, Arabisch, Portugiesisch, Niederländisch, Schwedisch, Norwegisch, Dänisch, Finnisch, Polnisch, Tschechisch, Ungarisch, Griechisch, Türkisch
        }

    }
}
