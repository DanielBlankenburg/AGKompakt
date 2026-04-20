using BGH_Kompakt.Classes.UserClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.UserLookUps
{
    public class Dienstbezeichnung
    {
        public int DienstbezeichnungId { get; set; }
        public string DienstbezeichnungText { get; set; }
        public IList<UserDienstbezeichnung> UserDienstbezeichnungen { get; set; }
        public IList<User> Users { get; set; }

        public string DienstbezeichnungLong()
        {
            return DienstbezeichnungText switch
            {
                "RiBGH" => "Richter am Bundesgerichtshof",
                "RinBGH" => "Richterin am Bundesgerichtshof",
                "VRiBGH" => "Vorsitzender Richter am Bundesgerichtshof",
                "VRinBGH" => "Vorsitzende Richterin am Bundesgerichtshof",
                "PräsBGH" => "Präsidenten des Bundesgerichtshof",
                "PräsinBGH" => "Präsidentin des Bundesgerichtshof",
                "VPräsBGH" => "Vizepräsidenten des Bundesgerichtshof",
                "VPräsinBGH" => "Vizepräsidenten des Bundesgerichtshof",
                _ => DienstbezeichnungText,
            };
        }
    }
}
