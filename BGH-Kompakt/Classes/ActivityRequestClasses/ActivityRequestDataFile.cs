using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.ActivityRequestClasses
{
    public class ActivityRequestDataFile
    {
        public int ActivityRequestDataFileID { get; set; }
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public int FileTyp { get; set; } //1 = Anlage; 2 = Schreiben Präsidialgeschäftsstelle
        public int ActivityRequestId { get; set; }
        public ActivityRequest ActivityRequest { get; set; }

    }
}
