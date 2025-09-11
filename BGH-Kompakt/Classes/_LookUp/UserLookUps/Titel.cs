using BGH_Kompakt.Classes.UserClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.UserLookUps
{
    public class Titel
    {
        public int TitelId { get; set; }
        public string TitelText { get; set; }
        public IList<User> Users { get; set; }
    }
}
