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
            MontagspostAdmin, NebentätigkeitenAdmin, Programm, Präsidentin, Präsidialrichter, Vorzimmer, Senat, MontagspostShow, NebentätigkeitenShow
        }

        public enum EnumDienstbezeichnungen
        {
            RiBGH,
            RinBGH,
            VRiBGH,
            VRinBGH,
            PräsBGH,
            PräsinBGH,
            VPräsBGH,
            VPräsinBGH
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

    }
}
