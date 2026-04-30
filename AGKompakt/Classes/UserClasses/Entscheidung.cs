using System.ComponentModel.DataAnnotations.Schema;

namespace BGH_Kompakt.Classes.UserClasses
{
    public class Entscheidung
    {
        public long Id { get; set; }
        public string Abteilung { get; set; }
        public int RegisterzeichenID { get; set; }
        public string LaufendeNummer { get; set; }
        public string Jahr {  get; set; }
        public string ZusatzFamilie {  get; set; }
        public int RechtsgebietID {  get; set; }
        [NotMapped]
        public string Aktenzeichen
        {
            get {  return $"{Abteilung} {Registerzeichen} {LaufendeNummer}/{Jahr} {Zusatz}"; }
        }
    }
}
