using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Services.DBContexts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{

    public class MPDecisionImportWord
    {
        public string Rohdaten { get; set; } = "";
        public string Entscheidungsdatum { get; set; } = "";
        public int Entscheidungsart { get; set; } = 0; //1=Urteil oder 2=Beschluss
        public string Rechtsgebiet { get; set; } = ""; //z.B. Zwangsvollstreckung, Familiensache, Insolvenzverfahren
        public string Normenkette { get; set; } = "";
        public string Leitsatz { get; set; } = "";
        public string Aktenzeichen { get; set; } = "";
        public string RegZeichen { get; set; } = "";
        public string LaufendeNummer { get; set; } = "";
        public string Jahr { get; set; } = "";
        public string InstanzErste { get; set; } = "";
        public string InstanzZweite { get; set; } = "";
        public int Bereich { get; set; } = 0;
        public bool ImportpdfSuccessfull { get; set; } = false;
        public bool ImportWordSuccessfull { get; set; } = false;

        public MPSenat Senat { get; set; }

        public string PathName { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        //public string FileName_Extention { get; set; } = "";

        public MPDecisionImportWord(MPImportFile File, bool imprtWordSuccessfull, string aktenzeichen = "", string entscheidungart = "", string rechtsgebiet = "", string entscheidungsdatum = "", string leitsatz = "", string normenkette = "", string vorinstanz1 = "", string vorinstanz2 = "")
        {
            Bereich = File.Bereich;
            ImportWordSuccessfull = imprtWordSuccessfull;
            if (aktenzeichen != string.Empty)
            {
                //Aktenzeichen extrahieren
                try
                {
                    string text = aktenzeichen;
                    //for (int i = 0; i < 2; i++)
                    //{
                    //    text = text.Substring(text.IndexOf("--") + 2);
                    //}
                    //text = text.Substring(0, text.IndexOf("/") + 3);
                    //text = text.Trim();
                    Aktenzeichen = text;
                    //Bereich = Bereich_ermitteln(text);
                    //Senat = senat_ermitteln(text);
                    RegZeichen = Reg_ermitteln(text, Bereich);
                    LaufendeNummer = LaufendeNummer_ermitteln(text);
                    Jahr = Jahr_Ermitteln(text);
                }
                catch (Exception)
                {
                    Aktenzeichen = "";
                }

                //Entscheidungsart extrahieren
                try
                {
                    //string text = rohdaten.Substring(rohdaten.IndexOf(",") + 2);
                    string Typ = entscheidungart;
                    Entscheidungsart = Typ switch
                    {
                        "Urteil" => 1,
                        "Verzichtsurteil" => 1,
                        "Versäumnisurteil" => 1,
                        "Anerkenntnisurteil" => 1,
                        "Beschluss" => 2,
                        _ => 0,
                    };
                }
                catch (Exception)
                {
                    Entscheidungsart = 0;
                }
                //Entscheidungsdatum extrahieren
                try
                {
                    Entscheidungsdatum = entscheidungsdatum;
                }
                catch (Exception)
                {

                    Entscheidungsdatum = "";
                }
                //Rechtsgebiet extrahieren
                try
                {
                    Rechtsgebiet = rechtsgebiet;
                }
                catch (Exception)
                {

                    Rechtsgebiet = "";
                }
                //Normenkette extrahieren
                try
                {
                    Normenkette = "";
                }
                catch (Exception)
                {
                    Normenkette = "";
                }


                ////Leitsatz extrahieren
                try
                {
                    Leitsatz = "";
                }
                catch (Exception)
                {

                    Leitsatz = "";
                }
                InstanzErste = vorinstanz1;
                InstanzZweite = vorinstanz2;
                Senat = File.Senat;
                Leitsatz = leitsatz;
                Normenkette = normenkette;

                PathName = File.ImportPathMP;
                FileName = File.FileName;
                ImportpdfSuccessfull = File.ImportSuccessfull;
            }
        }

        public MPDecisionImportWord() { }

        //private string CharToUnicodeFormat(char c)
        //{
        //    return string.Format(@"U+{0:x4}", (int)c);
        //}

        private string Reg_ermitteln(string Aktenzeichen, int Bereich)
        {
            if (Bereich < 3)
            {
                Aktenzeichen = Aktenzeichen.Substring(Aktenzeichen.IndexOf(' ') + 1);
                Aktenzeichen = Aktenzeichen.Substring(0, Aktenzeichen.IndexOf(' '));
            }
            else
            {
                if (Aktenzeichen.IndexOf("AnwZ") < 0)
                {
                    return Aktenzeichen.Substring(0, Aktenzeichen.IndexOf(" "));
                }
                else if (Aktenzeichen.Contains("EnVR") || Aktenzeichen.Contains("StB"))
                {
                    return string.Empty;
                }
                else
                {
                    try
                    {
                        return Aktenzeichen.Substring(0, Aktenzeichen.IndexOf(")") + 1);
                    }
                    catch (Exception)
                    {
                        return string.Empty;
                    }
                }
            }
            return Aktenzeichen;
        }


        private string LaufendeNummer_ermitteln(string Aktenzeichen)
        {
            Aktenzeichen = Aktenzeichen.Substring(0, Aktenzeichen.IndexOf('/'));
            Aktenzeichen = Aktenzeichen.Substring(Aktenzeichen.IndexOf(" "));
            Aktenzeichen = Aktenzeichen.Trim();
            if (Aktenzeichen.IndexOf(" ") > 0)
            {
                Aktenzeichen = Aktenzeichen.Substring(Aktenzeichen.IndexOf(" "));
                Aktenzeichen = Aktenzeichen.Trim();
            }
            return Aktenzeichen;
        }

        private string Jahr_Ermitteln(string Aktenzeichen)
        {
            return Aktenzeichen.Substring(Aktenzeichen.IndexOf('/') + 1);
        }

        public void ExportToMPDecesion(ref MPDecision mPDecisionTarget, ref MPDBContext context)
        {
            mPDecisionTarget.Rohdaten = Rohdaten;
            mPDecisionTarget.Date = DateTime.Parse(Entscheidungsdatum);
            mPDecisionTarget.Typ = Entscheidungsart;
            mPDecisionTarget.Rechtsgebiet = Rechtsgebiet;
            mPDecisionTarget.Normenkette = Normenkette;
            mPDecisionTarget.Leitsatz = Leitsatz;
            mPDecisionTarget.Aktenzeichen = Aktenzeichen;

            //string senat = Aktenzeichen.Substring(0, Aktenzeichen.IndexOf(" "));
            var SenatID = context.MPSenate.Where(x => x.MPSenatID == Senat.MPSenatID).FirstOrDefault();
            if (SenatID != null)
            {
                mPDecisionTarget.SenatID = SenatID.MPSenatID;
            }
            else
            {
                mPDecisionTarget.SenatID = 1;
            }
            mPDecisionTarget.RegZeichen = RegZeichen;
            mPDecisionTarget.LaufendeNummer = LaufendeNummer;
            mPDecisionTarget.Jahr = Jahr;
            mPDecisionTarget.InstanzErste = InstanzErste;
            mPDecisionTarget.InstanzZweite = InstanzZweite;
            mPDecisionTarget.PathName = PathName;
            mPDecisionTarget.FileName = FileName;
        }
    }


}
