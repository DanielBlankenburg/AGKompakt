using BGH_Kompakt.Classes.UserClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.Senate
{
    public class Senat
    {
        public int SenatID { get; set; }
        public string SenatName { get; set; } //z.B. IX. Zivilsenat
        public string SenatShort { get; set; } //Kurzbezeichnung des Senats, z.B. IX
        public int SenatArt { get; set; } //1 = Zivilsenat, 2 = Strafsenat, 3 = Sondersenat 
        public IList<User> Users { get; set; }
        public IList<User> AdminUsers { get; set; }
        public SenatSetting Senatsetting { get; set; }
        public string? Path {  get; set; } = string.Empty;

        public Senat()
        {
            
        }

        public Senat(string senatName, string senatShort, int senatArt, string path = "")
        {
            SenatName = senatName;
            SenatShort = senatShort;
            SenatArt = senatArt;
            Path = path;
        }
    }
}
