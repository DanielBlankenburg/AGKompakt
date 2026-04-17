using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.SystemSettings
{
    public class ProgrammSetting
    {
        [Key]
        public int Id { get; set; }
        public string PathSitzungsunterlagen { get; set; }
        public string Pathunterlagenverwaltung { get; set; }
        public string PathMontagspost { get; set; }
        public string PathDokstelle { get; set; }
        public string PathDokstelleDFS { get; set; }
        public string EMailDokstelle { get; set; }
        public bool MontagspostActivated { get; set; } = false;
        public bool ActivityRequestActivated { get; set; } = false;

        public ProgrammSetting()
        {
            
        }

        public ProgrammSetting(string pathSitzungsunterlagen, string pathunterlagenverwaltung, string pathMontagspost, string pathDokstelle, string pathDokstelleDFS, string eMailDokstelle, bool montagspostActivated, bool activityRequestActivated)
        {
            PathSitzungsunterlagen = pathSitzungsunterlagen;
            Pathunterlagenverwaltung = pathunterlagenverwaltung;
            PathMontagspost = pathMontagspost;
            PathDokstelle = pathDokstelle;
            PathDokstelleDFS = pathDokstelleDFS;
            EMailDokstelle = eMailDokstelle;
            MontagspostActivated = montagspostActivated;
            ActivityRequestActivated = activityRequestActivated;
        }
    }
}
