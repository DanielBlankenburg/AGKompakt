using BGH_Kompakt.Classes.UserClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes._LookUp.UserLookUps
{
    public class Status
    {
        public int StatusId { get; set; }
        public string StatusText { get; set; }
        public IList<User> Users { get; set; }
    }
}
