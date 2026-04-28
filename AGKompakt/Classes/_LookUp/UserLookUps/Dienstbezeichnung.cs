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
                "RiAG" => "Richter am Amtsgericht",
                "RinAG" => "Richterin am Amtsgericht",
                "PräsAG" => "Präsidentin des Amtsgerichts",
                "PräsinAG" => "Präsident des Amtsgerichts",
                "VPräsAG" => "Vizepräsidenten des Amtsgerichts",
                "VPräsinAG" => "Vizepräsidenten des Amtsgerichts",
                _ => DienstbezeichnungText,
            };
        }
    }
}
