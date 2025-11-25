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
        public IList<User> Users { get; set; }

        public string DienstbezeichnungLong()
        {
            return DienstbezeichnungText switch
            {
                "RiBGH" => "Richter am Bundesgerichtshof",
                "RinBGH" => "Richterin am Bundesgericht",
                "VRiBGH" => "Vorsitzender Richter am Bundesgericht",
                "VRinBGH" => "Vorsitzende Richterin am Bundesgericht",
                "PräsBGH" => "Präsidenten des Bundesgerichts",
                "PräsinBGH" => "Präsidentin des Bundesgerichts",
                "VPräsBGH" => "Vizepräsidenten des Bundesgerichts",
                "VPräsinBGH" => "Vizepräsidenten des Bundesgerichts",
                _ => DienstbezeichnungText,
            };
        }
    }
}
