using BGH_Kompakt.Classes.UserClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.UserLookUps
{
    public class AdminStatus
    {
        public int AdminStatusID { get; set; }
        public string AdminStatusText { get; set; }
        public IList<User> Users { get; set; }
    }
}
