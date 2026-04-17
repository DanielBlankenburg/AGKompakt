using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class Unterlagen
    {
        public string FileName_Fullpath { get; set; } = "";

        public string FileName_WithoutPath { get; set; } = "";
        public string FileName_WithExtention { get; set; } = "";

        public string FileName_PlainPath { get; set; } = "";

        public string FileName_Extention { get; set; } = "";

        public Unterlagen(string filename)
        {
            string dateiName = System.IO.Path.GetFileNameWithoutExtension(filename);
            string fileNameWithExntention = System.IO.Path.GetFileName(filename);
            string dateiEndung = System.IO.Path.GetExtension(filename);
            string path = System.IO.Path.GetDirectoryName(filename);


            FileName_Fullpath = filename;
            FileName_WithoutPath = dateiName;
            FileName_PlainPath = path;
            FileName_Extention = dateiEndung;
            FileName_WithExtention = fileNameWithExntention;
        }

        public Unterlagen Copy(string TargetPath)
        {
            string dateiName = System.IO.Path.GetFileNameWithoutExtension(FileName_Fullpath);
            string dateiEndung = System.IO.Path.GetExtension(FileName_Fullpath);
            string destinationFile = TargetPath + "\\" + dateiName + dateiEndung;
            File.Copy(FileName_Fullpath, destinationFile, true);
            return new Unterlagen(destinationFile);

        }
    }
}
