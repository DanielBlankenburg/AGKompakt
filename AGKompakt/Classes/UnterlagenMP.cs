using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class UnterlagenMP : Unterlagen
    {
        public string Entscheidungsdatum { get; set; } = "";
        public string Normenkette { get; set; } = "";
        public string Leitsatz{ get; set; } = "";
        public string Entscheidungsart { get; set; } = "";


        //public UnterlagenMP(string filenmae, int Bereich = 0, MPDecisionImport Entscheidung = null)
        //{
        //    FileName_Fullpath = name_voll;
        //    if (Entscheidung != null)
        //    {
        //        FileName_WithoutPath = Entscheidung.Aktenzeichen;
        //        Entscheidungsdatum = Entscheidung.Entscheidungsdatum;
        //        Normenkette = Entscheidung.Normenkette;
        //        //Entscheidungsart = Entscheidung.Entscheidungsart;
        //        Leitsatz = Entscheidung.Leitsatz;
        //    }
        //    else
        //    {
        //        FileName_WithoutPath = AnzeigeName_Erstellen(name_voll, dateiendung.Length, Bereich);
        //    }

        //    FileName_PlainPath = verzeichnis;
        //    FileName_Extention = dateiendung;
        //}

        public UnterlagenMP(string filename, int Bereich = 0, MPDecisionImport Entscheidung = null) : base(filename)
        {
            FileName_Fullpath = filename;
            string dateiName = System.IO.Path.GetFileNameWithoutExtension(filename);
            string dateiEndung = System.IO.Path.GetExtension(filename);
            string path = System.IO.Path.GetFullPath(filename);


            if (Entscheidung != null)
            {
                FileName_WithoutPath = Entscheidung.Aktenzeichen;
                Entscheidungsdatum = Entscheidung.Entscheidungsdatum;
                Normenkette = Entscheidung.Normenkette;
                //Entscheidungsart = Entscheidung.Entscheidungsart;
                Leitsatz = Entscheidung.Leitsatz;
            }
            else
            {
                FileName_WithoutPath = AnzeigeName_Erstellen(filename, dateiEndung.Length, Bereich);
            }

            FileName_PlainPath = path;
            FileName_Extention = dateiEndung;

        }


        private string AnzeigeName_Erstellen(string name_voll, int DateiEndungAnzahl, int Bereich)
        {
            string NameRest = string.Empty;
            string Senat;
            string RegZeichen;
            string laufendeNummer;
            string Jahr;
            string Anzeige;

            //Senat und RegZeichen bestimmen
            name_voll = name_voll.Substring(0, name_voll.Length - DateiEndungAnzahl);
            Senat = name_voll.Substring(0, name_voll.IndexOf("_"));
            switch (Bereich)
            {
                case 1:
                    NameRest = name_voll.Substring(name_voll.IndexOf("_") + 1);
                    RegZeichen = NameRest.Substring(0, NameRest.IndexOf("_"));
                    NameRest = NameRest.Substring(NameRest.IndexOf("_") + 1);
                    NameRest = NameRest.Replace("_", "");
                    laufendeNummer = NameRest.Substring(0, NameRest.IndexOf("-"));
                    Jahr = NameRest.Substring(NameRest.IndexOf("-") + 1, 2);
                    Anzeige = Senat + " " + RegZeichen + " " + laufendeNummer + "/" + Jahr;
                    break;
                case 2:
                    NameRest = name_voll.Substring(name_voll.IndexOf("_") + 1);
                    RegZeichen = NameRest.Substring(0, NameRest.IndexOf("_"));
                    NameRest = NameRest.Substring(NameRest.IndexOf("_") + 1);
                    NameRest = NameRest.Replace("_", "");
                    laufendeNummer = NameRest.Substring(0, NameRest.IndexOf("-"));
                    Jahr = NameRest.Substring(NameRest.IndexOf("-") + 1, 2);
                    Anzeige = Senat + " " + RegZeichen + " " + laufendeNummer + "/" + Jahr;
                    break;
                case 3:
                    NameRest = name_voll.Substring(name_voll.IndexOf("_") + 1);
                    NameRest = NameRest.Substring(NameRest.IndexOf("_") + 1);
                    NameRest = NameRest.Replace("_", "");
                    laufendeNummer = NameRest.Substring(0, NameRest.IndexOf("-"));
                    Jahr = NameRest.Substring(NameRest.IndexOf("-") + 1, 2);
                    Anzeige = Senat + " " + laufendeNummer + "/" + Jahr;
                    break;
                default:
                    Anzeige = "";
                    break;

            }
            return Anzeige;
        }
    }
}
