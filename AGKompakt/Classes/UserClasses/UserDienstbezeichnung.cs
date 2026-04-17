using BGH_Kompakt.Classes._LookUp.UserLookUps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.UserClasses
{
    public class UserDienstbezeichnung
    {
        public int UserDienstbezeichnungId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int DienstbezeichnungId { get; set; }
        public virtual Dienstbezeichnung Dienstbezeichnung { get; set; }
        public DateTime GültigAb { get; set; }
    }
}
