using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Services.DBContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.MP
{
    public  class MPBGHRSenat
    {
        public int MPBGHRSenatID { get; set; }
        public int Senat { get; set; }
        public int Recipient { get; set; }

        [NotMapped]
        public string SenatAnzeige {
            get {
                UserDBContext context = new UserDBContext();
                var senat = context.Senate.FirstOrDefault(s => s.SenatID == Senat);
                string senatText = senat != null ? senat.SenatName : "Kein Senat gefunden";
                return senatText;
            }
        }

        [NotMapped]
        public string RecipientAnzeige
        {
            get
            {
                UserDBContext context = new UserDBContext();
                var user = context.Users.FirstOrDefault(u => u.UserId == Recipient);
                string recipientText = user != null ? user.Fullname : "Kein Empfänger gefunden";
                return recipientText;
            }
        }

    }
}
