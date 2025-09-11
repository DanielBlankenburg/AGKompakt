using BGH_Kompakt.Classes.UserClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.Senate
{
    public class SenatSpruchgruppe
    {
        public int SenatSpruchgruppeID { get; set; }
        public string SenatSpruchgruppeName { get; set; }
        public int SenatSpruchgruppeOrderNumber {  get; set; }
        public virtual SenatSetting SenatSetting { get; set; }
        public IList<User> Members { get; set; }
        [NotMapped]
        public string SenatSpruchgruppeAnzeige { get {return $"SG {SenatSpruchgruppeName}";}}
    }
}
