using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.Senate
{
    public class SenatAktenzeichen
    {
        public int SenatAktenzeichenID { get; set; }
        public string SenatAktenzeichenName { get; set; }
        public string SenatAktenzeichenNameRaw { get; set; }
        public int SenatAktenzeichenOrderNumber {  get; set; }
        public virtual SenatSetting SenatSetting { get; set; }
    }
}
