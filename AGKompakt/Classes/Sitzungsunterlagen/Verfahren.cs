using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Services.DBContexts;
using System.IO;
using System.Linq;
using System.Windows;

namespace BGH_Kompakt.Classes
{
    public class Verfahren
    {
        public int VerfahrenID { get; set; }
        public string Verfahren_Rohinformation { get; set; }
        public string Verfahren_Anzeigedaten { get; set; }
        public string Verfahren_FullPath { get; set; }
        public Senat Senat { get; set; } = new Senat();
        public SenatAktenzeichen Registerzeichen { get; set; }
        public string Jahr { get; set; }
        public string LaufendeNummer { get; set; }
        public SenatSpruchgruppe Spruchgruppe { get; set; }
        public bool Verteilt { get; set; }
        public bool Anzeige_Verteilung { get; set; }
        public int LaufendeNummerInt { get; set; }

        public Verfahren()
        {
            
        }

        //public Verfahren(Verfahren verfahren)
        //{
        //    Verfahren_Rohinformation = verfahren.Verfahren_Rohinformation;
        //    Verfahren_Anzeigedaten = verfahren.Verfahren_Anzeigedaten;
        //    Verfahren_FullPath = verfahren.Verfahren_FullPath;
        //    if (verfahren.Senat != null) Senat = verfahren.Senat;
        //    if (verfahren.Registerzeichen != null) Registerzeichen = verfahren.Registerzeichen;
        //    Jahr = verfahren.Jahr;
        //    LaufendeNummer = verfahren.LaufendeNummer;
        //    if (verfahren.Spruchgruppe != null) Spruchgruppe = verfahren.Spruchgruppe;
        //    Verteilt = verfahren.Verteilt;
        //    Anzeige_Verteilung = verfahren.Anzeige_Verteilung;
        //    LaufendeNummerInt = verfahren.LaufendeNummerInt;

        //}

        public Verfahren(Verfahren verfahren, Senat senat, SenatAktenzeichen registerzeichen, SenatSpruchgruppe spruchgruppe)
        {
            Verfahren_Rohinformation = verfahren.Verfahren_Rohinformation;
            Verfahren_Anzeigedaten = verfahren.Verfahren_Anzeigedaten;
            Verfahren_FullPath = verfahren.Verfahren_FullPath;
            if (senat != null) Senat = senat;
            if (registerzeichen != null) Registerzeichen = registerzeichen;
            Jahr = verfahren.Jahr;
            LaufendeNummer = verfahren.LaufendeNummer;
            if (spruchgruppe != null) Spruchgruppe = spruchgruppe;
            Verteilt = verfahren.Verteilt;
            Anzeige_Verteilung = verfahren.Anzeige_Verteilung;
            LaufendeNummerInt = verfahren.LaufendeNummerInt;

        }


        public Verfahren(string rohdaten)
        {
            Rohdaten_Converter(rohdaten);
        }

        //public Verfahren(
        //  string rohdaten,
        //  string anzeigedaten,
        //  Senat senat,
        //  SenatAktenzeichen registerzeichen,
        //  string jahr,
        //  string laufendeNummer,
        //  SenatSpruchgruppe spruchgruppe,
        //  bool verteilt = false,
        //  bool anzeige_Verteilung = false)
        //{
        //    Verfahren_Rohinformation = rohdaten;
        //    Verfahren_Anzeigedaten = anzeigedaten;
        //    Senat = senat;
        //    Registerzeichen = registerzeichen;
        //    Jahr = jahr;
        //    LaufendeNummer = laufendeNummer;
        //    Spruchgruppe = spruchgruppe;
        //    Verteilt = verteilt;
        //    Anzeige_Verteilung = anzeige_Verteilung;
        //    LaufendeNummerInt = int.TryParse(LaufendeNummer, out nNumber) ? nNumber : 0;
        //}

        private void Rohdaten_Converter(string rohdaten)
        {
            DirectoryInfo ImportDir = new DirectoryInfo(rohdaten);
            string VerfahrenName = ImportDir.Name;
            Verfahren_FullPath = ImportDir.FullName;
            if (VerfahrenName.IndexOf("-") > -1)
            {
                Verfahren_Rohinformation = VerfahrenName;
                int length1 = VerfahrenName.IndexOf(" ");
                UserDBContext userDBContext = new UserDBContext();
                try
                {
                    string SenatRoh = VerfahrenName.Substring(0, length1);
                    var s = userDBContext.Senate.Where(x => x.SenatShort == VerfahrenName.Substring(0, length1)).FirstOrDefault();
                    Senat = s ?? null;
                }
                catch (System.Exception)
                {
                    MessageBox.Show("Es konnte kein Senat eingelesen werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                string str1 = VerfahrenName.Substring(length1 + 1, VerfahrenName.Length - (length1 + 1));
                int length2 = str1.IndexOf(" ");
                string AZRoh = str1.Substring(0, length2);
                var az = userDBContext.SenatAktenzeichen.FirstOrDefault(x => x.SenatAktenzeichenNameRaw == AZRoh);
                Registerzeichen = az ?? null;
                string str2 = str1.Substring(length2 + 1, str1.Length - (length2 + 1));
                int length3 = str2.IndexOf("-");
                bool Jahrzuerst;
                Jahrzuerst = (Senat.SenatShort == "VIa");
                if (Jahrzuerst)
                {
                    Jahr = str2.Substring(0, length3);
                }
                else
                {
                    LaufendeNummer = str2.Substring(0, length3);
                }
                string str3 = str2.Substring(length3 + 1, str2.Length - (length3 + 1));
                if (str3.IndexOf("-SG") >= 0) //Spruchgruppe vorhanden
                {
                    int length4 = str3.IndexOf("-");
                    if (Jahrzuerst)
                    {
                        LaufendeNummer = str3.Substring(0, length4);
                    }
                    else
                    {
                        Jahr = str3.Substring(0, length4);
                    }

                    string str4 = str3.Substring(length4 + 1, str3.Length - (length4 + 1));
                    if (str4.IndexOf("_") >= 0)
                    {
                        int length5 = str4.IndexOf("_");
                        string spruchgruppe = str4.Substring(0, length5);
                        spruchgruppe = spruchgruppe.Replace("SG", "");
                        var sg = userDBContext.SenatSpruchgruppen.FirstOrDefault(x => x.SenatSpruchgruppeName == spruchgruppe);
                        Spruchgruppe = sg ?? null;
                        Verteilt = true;
                    }
                    else
                    {
                        string spruchgruppe = str4.Substring(0, str4.Length);
                        spruchgruppe = spruchgruppe.Replace("SG", "");
                        var sg = userDBContext.SenatSpruchgruppen.FirstOrDefault(x => x.SenatSpruchgruppeName == spruchgruppe);
                        Spruchgruppe = sg ?? null;
                        Verteilt = false;
                    }

                }
                else if (str3.IndexOf("_") >= 0)
                {
                    int length4 = str3.IndexOf("_");
                    if (Jahrzuerst)
                    {
                        LaufendeNummer = str3.Substring(0, length4);
                        Verteilt = true;
                    }
                    else
                    {
                        Jahr = str3.Substring(0, length4);
                    }
                }
                else if (str3.IndexOf("- verteilt") >= 0)
                {
                    LaufendeNummer = str3.Substring(0, str3.IndexOf("-") - 1);
                    Verteilt = true;
                }
                else
                {
                    if (Jahrzuerst)
                    {
                        LaufendeNummer = str3.Substring(0, str3.Length);
                    }
                    else
                    {
                        Jahr = str3.Substring(0, str3.Length);
                    }
                }

                Verfahren_Anzeigedaten = (Senat != null && Registerzeichen != null && LaufendeNummer != null) ? Senat.SenatShort + " " + Registerzeichen.SenatAktenzeichenName + " " + LaufendeNummer + "/" + Jahr : "Fehler in der Anzeige; Senatseinstellung (RegZeichen, Spruchgruppen) prüfen";
            }
            else
            {
                Verfahren_Rohinformation = VerfahrenName;
                Verfahren_Anzeigedaten = VerfahrenName;
            }
        }
    }
}
