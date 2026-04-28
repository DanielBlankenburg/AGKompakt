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
        public string PathAG { get; set; }
        public string PathFam { get; set; }
        public string PathZiv { get; set; }
        public string PathInsO { get; set; }

        public ProgrammSetting()
        {
            
        }

        public ProgrammSetting(string pathAG, string pathFam, string pathZiv, string pathInsO)
        {
            PathAG = pathAG;
            PathFam = pathFam;
            PathZiv = pathZiv;
            PathInsO = pathInsO;
        }
    }
}
