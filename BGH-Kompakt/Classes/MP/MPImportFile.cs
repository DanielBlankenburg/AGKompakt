using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes.Senate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.MP
{
    public class MPImportFile
    {
        public string FileFullPath { get; set; }
        public string FileName { get; set; }
        public string FileExtention { get; set; }
        public string FileRohChar { get; set; } //Dateiname ohne Leerzeichen und Unterstrichen
        public bool WordFileExist { get; set; } = false;
        public string FileWordFullPath { get; set; }
        public bool ImportSuccessfull { get; set; } = false;
        public string ImportPathMP { get; set; }
        public string ImportPathDok { get; set; }
        public MPSenat Senat { get; set; }
        public string SenatRohstring { get; set; }
        public int Bereich { get; set; }

        public MPImportFile(string file)
        {

            FileInfo fileInfo = new FileInfo(file);
            FileFullPath = fileInfo.FullName;
            FileName = fileInfo.Name;
            FileExtention = fileInfo.Extension;
            FileRohChar = FileName.Replace(" ", "").Replace("_", "").Replace(FileExtention, "");


        }
    }
}
