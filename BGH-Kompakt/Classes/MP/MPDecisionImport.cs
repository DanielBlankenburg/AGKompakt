using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Services.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes
{
    public class MPDecisionImport
    {
        public string Rohdaten { get; set; } = "";
        public string Entscheidungsdatum { get; set; } = "";
        public int Entscheidungsart { get; set; } = 0; //1=Urteil oder 2=Beschluss
        public string Rechtsgebiet { get; set; } = ""; //z.B. Zwangsvollstreckung, Familiensache, Insolvenzverfahren
        public string Normenkette { get; set; } = "";
        public string Leitsatz { get; set; } = "";
        public string Aktenzeichen { get; set; } = "";
        public string Senat { get; set; } = "";
        public string RegZeichen { get; set; } = "";
        public string LaufendeNummer { get; set; } = "";
        public string Jahr { get; set; } = "";
        public string InstanzErste { get; set; } = "";
        public string InstanzZweite { get; set; } = "";
        public int Bereich { get; set; } = 0;

        //public string FileName_Extention { get; set; } = "";

        public MPDecisionImport(string rohdaten)
        {

            Rohdaten = rohdaten;
            if (Rohdaten != string.Empty)
            {
                //Aktenzeichen extrahieren
                try
                {
                    string text = rohdaten;
                    for (int i = 0; i < 2; i++)
                    {
                        text = text.Substring(text.IndexOf("--") + 2);
                    }
                    text = text.Substring(0, text.IndexOf("/") + 3);
                    text = text.Trim();
                    Aktenzeichen = text;
                    Bereich = Bereich_ermitteln(text);
                    Senat = senat_ermitteln(text);
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
                    string text = rohdaten.Substring(rohdaten.IndexOf(",") + 2);
                    string Typ = text.Substring(0, text.IndexOf(" "));
                    switch (Typ)
                    {
                        case "Urteil":
                            Entscheidungsart = 1;
                            break;
                        case "Beschluss":
                            Entscheidungsart = 2;
                            break;
                        default:
                            Entscheidungsart = 0;
                            break;
                    }
                }
                catch (Exception)
                {
                    Entscheidungsart = 0;
                }
                //Entscheidungsdatum extrahieren
                try
                {
                    string text = rohdaten.Substring(rohdaten.IndexOf("vom") + 4);
                    Entscheidungsdatum = text.Substring(0, text.IndexOf(" "));
                }
                catch (Exception)
                {

                    Entscheidungsdatum = "";
                }
                //Rechtsgebiet extrahieren
                try
                {
                    string text = rohdaten.Substring(rohdaten.IndexOf("Verfahrensart:") + 15);
                    int index = text.IndexOf('\u2022');
                    if (index > 0)
                    {
                        Rechtsgebiet = text.Substring(0, index);
                    }
                    else
                    {
                        Rechtsgebiet = text;
                    }

                    //string undertext = text.Substring(0, 1);
                    //char character = char.Parse(undertext);
                    //Punkt = CharToUnicodeFormat(character);
                }
                catch (Exception)
                {

                    Rechtsgebiet = "";
                }
                //Normenkette extrahieren
                try
                {
                    int index = rohdaten.IndexOf("Normenkette:");
                    if (index > 0)
                    {
                        string text = rohdaten.Substring(index + 13);
                        Normenkette = text.Substring(0, text.IndexOf('\u2022'));

                    }
                    else
                    {
                        Normenkette = "";
                    }
                }
                catch (Exception)
                {

                    Normenkette = "";
                }


                ////Leitsatz extrahieren
                try
                {
                    int index = rohdaten.IndexOf("Leitsatz:");
                    if (index > 0)
                    {
                        string text = rohdaten.Substring(index + 10);
                        int index2 = text.IndexOf('\u2022');
                        if (index2 > 0)
                        {
                            Leitsatz = text.Substring(0, index2);

                        }
                        else
                        {
                            Leitsatz = text.Substring(0);
                        }

                    }
                    else
                    {
                        Leitsatz = "";
                    }
                }
                catch (Exception)
                {

                    Leitsatz = "";
                }
            }
        }

        public MPDecisionImport() { }

        //private string CharToUnicodeFormat(char c)
        //{
        //    return string.Format(@"U+{0:x4}", (int)c);
        //}

        private int Bereich_ermitteln(string Aktenzeichen)
        {
            string senat = Aktenzeichen.Substring(0, Aktenzeichen.IndexOf(" "));

            switch (senat) //1 = Zivilbereich, 2 = Strafbereich, 3 = Sondersenate
            {
                case "1":
                    return 2;
                case "2":
                    return 2;
                case "3":
                    return 2;
                case "4":
                    return 2;
                case "5":
                    return 2;
                case "6":
                    return 2;
                case "I":
                    return 1;
                case "II":
                    return 1;
                case "III":
                    return 1;
                case "IV":
                    return 1;
                case "V":
                    return 1;
                case "VI":
                    return 1;
                case "VIa":
                    return 1;
                case "VII":
                    return 1;
                case "VIII":
                    return 1;
                case "IX":
                    return 1;
                case "X":
                    return 1;
                case "XI":
                    return 1;
                case "XII":
                    return 1;
                case "XIII":
                    return 1;
                case "KVB":
                case "EnVR":
                    return 3;
                case "StB":
                    return 3;
                case "AnwZ":
                    return 3;
                case "LwZR":
                    return 3;
                default:
                    return 1;
            }
        }

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
                else
                {
                    return Aktenzeichen.Substring(0, Aktenzeichen.IndexOf(")") + 1);
                }
            }
            return Aktenzeichen;
        }



        private string senat_ermitteln(string Aktenzeichen)
        {
            if (Aktenzeichen.IndexOf("AnwZ") < 0)
            {
                return Aktenzeichen.Substring(0, Aktenzeichen.IndexOf(" "));
            }
            else
            {
                return Aktenzeichen.Substring(0, Aktenzeichen.IndexOf(")") + 1);
            }
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

        public void ExportToMPDecesion(ref MPDecision mPDecisionTarget)
        {
            mPDecisionTarget.Rohdaten = Rohdaten;
            mPDecisionTarget.Date = DateTime.Parse(Entscheidungsdatum);
            mPDecisionTarget.Typ = Entscheidungsart;
            mPDecisionTarget.Rechtsgebiet = Rechtsgebiet;
            mPDecisionTarget.Normenkette = Normenkette;
            mPDecisionTarget.Leitsatz = Leitsatz;
            mPDecisionTarget.Aktenzeichen = Aktenzeichen;
            string senat = Aktenzeichen.Substring(0, Aktenzeichen.IndexOf(" "));
            //MPDBContext userContext = new MPDBContext();
            //var SenatID = userContext.MPSenate.Where(x => x.MPSenatNameShort == senat).FirstOrDefault();
            //if (SenatID != null)
            //{
            //    mPDecisionTarget.SenatID = SenatID.MPSenatID;
            //}
            //else
            //{
            //    mPDecisionTarget.SenatID = 1;
            //}
            mPDecisionTarget.RegZeichen = RegZeichen;
            mPDecisionTarget.LaufendeNummer = LaufendeNummer;
            mPDecisionTarget.Jahr = Jahr;
            mPDecisionTarget.InstanzErste = InstanzErste;
            mPDecisionTarget.InstanzZweite = InstanzZweite;
            mPDecisionTarget.PathName = string.Empty;
            mPDecisionTarget.FileName = string.Empty;
        }
    }

}
