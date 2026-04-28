using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Services.DBContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.Base
{
    public class Person
    {
        public string VorName { get; set; } = string.Empty;
        public string NachName { get; set; } = string.Empty;
        public string EMail { get; set; } = string.Empty;
        public int GeschlechtID { get; set; }
        public virtual Geschlecht Geschlecht { get; set; }
        public int? TitelId { get; set; }
        public virtual Titel Titel { get; set; }

        [NotMapped]
        public string Fullname
        {
            get { return $"{SetTitel()}{VorName} {NachName}"; }
        }

        [NotMapped]
        public string FullSurname
        {
            get { return $"{SetTitel()}{NachName}"; }
        }

        private string SetTitel()
        {
            string titel = string.Empty;
            if (TitelId > 1)
            {
                if (Titel != null)
                {
                    titel = Titel.TitelText + " ";
                }
                else
                {
                    UserDBContext userDBContext = new UserDBContext();
                    Titel iTitel = userDBContext.Titel.Where(t => t.TitelId == TitelId).FirstOrDefault();
                    if (iTitel != null) titel = iTitel.TitelText + " ";
                }
            }
            return titel;
        }

    }
}
