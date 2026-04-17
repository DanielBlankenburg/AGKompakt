using BGH_Kompakt.Classes;

namespace BGH_Kompakt.Services
{
    public class FillKlassen
    {
        public static MontagspostFilter _filterSetting = new MontagspostFilter();

        //public static void Cbo_Jahr_Fill(string path, ref ObservableCollection<Vintages>  jahresliste)
        //{
        //    jahresliste.Clear();
        //    try
        //    {
        //        foreach (DirectoryInfo directory in new DirectoryInfo(path).GetDirectories())
        //        {
        //            if (int.TryParse(directory.Name, out int _))
        //                jahresliste.Add(new Vintages(directory.Name));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Es ist folgender Fehler beim Auslesen der Jahre aufgetreten: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //public static void Sitzungsliste_Fill(string path, ref ObservableCollection<Sitzungstage> Sitzungstageliste, bool AnzeigeAktuelleVerfahren)
        //{
        //    bool Anzeige;
        //    Sitzungstageliste.Clear();
        //    try
        //    {
        //        foreach (FileSystemInfo fileSystemInfo in (IEnumerable<DirectoryInfo>)new DirectoryInfo(path).GetDirectories("*.*", SearchOption.TopDirectoryOnly).OrderBy<DirectoryInfo, string>(f => f.Name))
        //        {

        //            Sitzungsdatum sitzungsdatum = new Sitzungsdatum(fileSystemInfo.Name);
        //            var cultureInfo = new CultureInfo("en-US");
        //            //DateTime datSitzung = DateTime.Parse(sitzungsdatum.Rohdatum, cultureInfo);
        //            DateTime datSitzung = DateTime.ParseExact(sitzungsdatum.Rohdatum, "yyyy_MM_dd", CultureInfo.InvariantCulture);
        //            if (AnzeigeAktuelleVerfahren)
        //            {
        //                if (datSitzung >= DateTime.Today)
        //                {
        //                    Anzeige = true;
        //                }
        //                else
        //                {
        //                    Anzeige = false;
        //                }
        //            }
        //            else
        //            {
        //                Anzeige = true;
        //            }
        //            {

        //            }
        //            if (Anzeige)
        //            {
        //                Sitzungstageliste.Add(new Sitzungstage(sitzungsdatum.Rohdatum, sitzungsdatum.AnzeigeDatum, sitzungsdatum.Jahr, sitzungsdatum.Monat, sitzungsdatum.Tag));
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        int num = (int)MessageBox.Show("Die Sitzungstage konnten nicht ausgelesen werden. Es ist folgender Fehler aufgetreten: " + ex.Message);
        //    }
        //}

        //public static void Sitzungsdienste_Fill(ObservableCollection<PersonenViewModel> _richterList, ObservableCollection<Sitzungsdienste> _sitzungList, string pathparent, string dirParent, string dirApplication, string dirSitzungsplaene, string jahr, ref ObservableCollection<Sitzungsdienste> Sitzungsdienste)
        //{
        //    Sitzungsdienste.Clear();
        //    try
        //    {
        //        string pathxml = pathparent + dirParent + dirApplication + dirSitzungsplaene + jahr + "\\Sitzungsplaene.xml";

        //        using (StreamReader streamReader = new StreamReader(new FileStream(pathparent + dirParent + dirApplication + dirSitzungsplaene + jahr + "\\Sitzungsplaene.txt", FileMode.Open)))
        //        {
        //            int id = 1;
        //            string str1;
        //            while ((str1 = streamReader.ReadLine()) != null)
        //            {
        //                string[] strArray1 = str1.Split(';');
        //                string[] strArray2 = strArray1[0].Split('-');
        //                int num1 = int.Parse(strArray1[1]) - 1;
        //                string hiWiNameZitat;
        //                if (num1 >= 0)
        //                {
        //                    if (_richterList[num1].Titel.Value != null)
        //                        hiWiNameZitat = _richterList[num1].Titel.Value + " " + _richterList[num1].Nachname.Value;
        //                    else hiWiNameZitat = _richterList[num1].Nachname.Value;
        //                }
        //                else
        //                {
        //                    num1 = 0;
        //                    hiWiNameZitat = "Ausfall";
        //                }
        //                int num2;
        //                string hiwiNameGuide;
        //                if (strArray1[3] != string.Empty)
        //                {
        //                    num2 = int.Parse(strArray1[3]) - 1;
        //                    if (_richterList[num2].Titel.Value != null)
        //                        hiwiNameGuide = _richterList[num2].Titel.Value + " " + _richterList[num2].Nachname.Value;
        //                    else hiwiNameGuide = _richterList[num2].Nachname.Value;
        //                }
        //                else
        //                {
        //                    num2 = 0;
        //                    hiwiNameGuide = string.Empty;
        //                }
        //                Sitzungsdienste.Add(new Sitzungsdienste(id, strArray1[0], strArray2[2] + "." + strArray2[1] + "." + strArray2[0], num1, hiWiNameZitat, strArray2[0], strArray2[1], strArray2[2], strArray1[2], num2, hiwiNameGuide));
        //                ++id;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        int num = (int)MessageBox.Show("Die Sitzungspläne konnten nicht ausgelesen werden. Es ist folgender Fehler aufgetreten: " + ex.Message);
        //    }
        //}

        //public static void KW_Fill(string path, string jahr, ref ObservableCollection<Kalenderwoche> KWListe)
        //{
        //    try
        //    {
        //        KWListe.Clear();
        //        foreach (DirectoryInfo directory in new DirectoryInfo(path).GetDirectories())
        //        {
        //             KWListe.Add(new Kalenderwoche(directory.FullName, path, jahr));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Es ist folgender Fehler beim Auslesen der Jahre aufgetreten: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
    //    public static void SenateMP_Fill(string KW, BereichMP Bereich, ref ObservableCollection<SenatKW> Senatsliste)
    //    {


    //        try
    //        {
    //            List<SenatKW> Rohliste = new List<SenatKW>();

				//string pathFilterliste = BGHKompaktSystemInfo.PathEigeneDateien + BGHKompaktSystemInfo.PathMontagspost + "MontagspostFilter.xml";
    //            //_filterlist = new ObservableCollection<MontagspostFilter>();
    //            //FillKlassen.Filterliste_Fill(pathFilterliste, ref _filterSetting);

    //            Senatsliste.Clear();

    //            string pathBereich = KW + "\\" + Bereich.Bereich_Path;

    //            if (Directory.Exists(pathBereich))
    //            {
    //                DirectoryInfo directoryInfo = new DirectoryInfo(pathBereich);

    //                bool Anzeige = false;

    //                foreach (FileSystemInfo directory in directoryInfo.GetDirectories())
    //                {
    //                    switch (Bereich.Bereich)
    //                    {
    //                        case 1:
    //                            if (_filterSetting.ZivilSenate) Anzeige = true; break;
    //                        case 2:
    //                            if (_filterSetting.Strafsenate) Anzeige = true; break;
    //                        case 3:
    //                            if (_filterSetting.Sondersenate) Anzeige = true; break;
    //                    }
                        
    //                    if (Anzeige) if (Filtervergleich(SenatsConverter.Convert(directory.Name, 1))) Rohliste.Add(new SenatKW(SenatsConverter.Convert(directory.Name, 2), directory.FullName, directory.Name));
    //                }
    //                Rohliste = Rohliste.OrderBy(o => o.Sortierung).ToList();

    //                foreach (SenatKW senat in  Rohliste)
    //                {
    //                    Senatsliste.Add(new SenatKW(senat.Sortierung, senat.Rohdaten, senat.SenatBezeichnung));
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show("Es ist folgender Fehler beim Auslesen der Senate aufgetreten: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
    //        }
    //    }

        //public static bool Filtervergleich(int senatID)
        //{
        //    switch (senatID)
        //    {
        //        case 1:
        //            {
        //                return _filterSetting.Zivilsenat1;
        //            }
        //        case 2:
        //            {
        //                return _filterSetting.Zivilsenat2;
        //            }
        //        case 3:
        //            {
        //                return _filterSetting.Zivilsenat3;
        //            }
        //        case 4:
        //            {
        //                return _filterSetting.Zivilsenat4;
        //            }
        //        case 5:
        //            {
        //                return _filterSetting.Zivilsenat5;
        //            }
        //        case 6:
        //            {
        //                return _filterSetting.Zivilsenat6;
        //            }
        //        case 7:
        //            {
        //                return _filterSetting.Zivilsenat7;
        //            }
        //        case 8:
        //            {
        //                return _filterSetting.Zivilsenat8;
        //            }
        //        case 9:
        //            {
        //                return _filterSetting.Zivilsenat9;
        //            }
        //        case 10:
        //            {
        //                return _filterSetting.Zivilsenat10;
        //            }
        //        case 11:
        //            {
        //                return _filterSetting.Zivilsenat11;
        //            }
        //        case 12:
        //            {
        //                return _filterSetting.Zivilsenat12;
        //            }
        //        case 13:
        //            {
        //                return _filterSetting.Zivilsenat13;
        //            }
        //        case 14:
        //            {
        //                return _filterSetting.Zivilsenat6a;
        //            }
        //        case 15:
        //            {
        //                return _filterSetting.Strafsenat1;
        //            }
        //        case 16:
        //            {
        //                return _filterSetting.Strafsenat2;
        //            }
        //        case 17:
        //            {
        //                return _filterSetting.Strafsenat3;
        //            }
        //        case 18:
        //            {
        //                return _filterSetting.Strafsenat4;
        //            }
        //        case 19:
        //            {
        //                return _filterSetting.Strafsenat5;
        //            }
        //        case 20:
        //            {
        //                return _filterSetting.Strafsenat6;
        //            }
        //        case 21:
        //            {
        //                return _filterSetting.SondersenatAnwalt;
        //            }
        //        case 22:
        //            {
        //                return _filterSetting.SondersenatNotar;
        //            }
        //        case 23:
        //            {
        //                return _filterSetting.SondersenatLandwirtschaft;
        //            }
        //        case 24:
        //            {
        //                return _filterSetting.SondersenatSteuerberater;
        //            }
        //        default: return false;

        //    }


            

        //}



        //public static void UnterlagenMP_Fill(string path, BereichMP Bereich, bool bolAnzeige, ref ObservableCollection<UnterlagenMP> Unterlagenliste, ObservableCollection<MPDecisionImport> Entscheidungsliste)
        //{
        //    try
        //    {
        //        if (!bolAnzeige)
        //            return;
        //        Unterlagenliste.Clear();
        //        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        //        int num = 0;
        //        foreach (FileInfo file in directoryInfo.GetFiles())
        //        {
        //            string Vergleichsaktenzeichen = AZVergleich(file.Name.ToString(), file.Extension.ToString(), Bereich.Bereich);
        //            //string AktenzeichenDaten = string.Empty;

        //            MPDecisionImport Übergabeentscheidung = null;
        //            foreach(MPDecisionImport item in Entscheidungsliste)
        //            {
        //                if (item.Aktenzeichen == Vergleichsaktenzeichen)
        //                {
        //                    Übergabeentscheidung = item;
        //                }
        //            }

        //            Unterlagenliste.Add(new UnterlagenMP(file.Name.ToString(), file.DirectoryName?.ToString(), file.Extension.ToString(), Bereich.Bereich, Übergabeentscheidung));
        //            ++num;
        //        }
        //        if (num == 0)
        //            Unterlagenliste.Add(new UnterlagenMP("Es sind keine Unterlagen vorhanden"));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Es ist folgender Fehler beim Auslesen der Unterlagen aufgetreten: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}





        //public  static string AZVergleich(string name_voll, string DateiEndung, int Bereich)
        //{
        //    string NameRest = string.Empty;
        //    string Senat;
        //    string RegZeichen;
        //    string laufendeNummer;
        //    string Jahr;
        //    string Anzeige;

        //    int DateiEndungAnzahl = DateiEndung.Length;
        //    //Senat und RegZeichen bestimmen
        //    name_voll = name_voll.Substring(0, name_voll.Length - DateiEndungAnzahl);
        //    Senat = name_voll.Substring(0, name_voll.IndexOf("_"));
        //    if (Senat == "AnwZ(Brfg)") Senat = "AnzW (Brfg)";
        //    switch (Bereich)
        //    {
        //        case 1:
        //            NameRest = name_voll.Substring(name_voll.IndexOf("_") + 1);
        //            RegZeichen = NameRest.Substring(0, NameRest.IndexOf("_"));
        //            NameRest = NameRest.Substring(NameRest.IndexOf("_") + 1);
        //            NameRest = NameRest.Replace("_", "");
        //            laufendeNummer = NameRest.Substring(0, NameRest.IndexOf("-"));
        //            Jahr = NameRest.Substring(NameRest.IndexOf("-") + 1, 2);
        //            Anzeige = Senat + " " + RegZeichen + " " + laufendeNummer + "/" + Jahr;
        //            break;
        //        case 2:
        //            NameRest = name_voll.Substring(name_voll.IndexOf("_") + 1);
        //            RegZeichen = NameRest.Substring(0, NameRest.IndexOf("_"));
        //            NameRest = NameRest.Substring(NameRest.IndexOf("_") + 1);
        //            NameRest = NameRest.Replace("_", "");
        //            laufendeNummer = NameRest.Substring(0, NameRest.IndexOf("-"));
        //            Jahr = NameRest.Substring(NameRest.IndexOf("-") + 1, 2);
        //            Anzeige = Senat + " " + RegZeichen + " " + laufendeNummer + "/" + Jahr;
        //            break;
        //        case 3:
        //            NameRest = name_voll.Substring(name_voll.IndexOf("_") + 1);
        //            NameRest = NameRest.Substring(NameRest.IndexOf("_") + 1);
        //            NameRest = NameRest.Replace("_", "");
        //            laufendeNummer = NameRest.Substring(0, NameRest.IndexOf("-"));
        //            Jahr = NameRest.Substring(NameRest.IndexOf("-") + 1, 2);
        //            Anzeige = Senat + " " + laufendeNummer + "/" + Jahr;
        //            break;
        //        default:
        //            Anzeige = "";
        //            break;

        //    }
        //    return Anzeige;
        //}

        //public static void Richterliste_Fill(ref ObservableCollection<PersonenViewModel> _richterliste)
        //{
        //    string pathrichterliste = BGHKompaktSystemInfo.PathLaufwerksbuchstabe + BGHKompaktSystemInfo.PathSystemdateien + "Richterliste.xml";
        //    try
        //    {
        //        FileStream fs = new FileStream(pathrichterliste, FileMode.Open);
        //        XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Person>));
        //        ObservableCollection<Person> templist = (ObservableCollection<Person>)serializer.Deserialize(fs);
        //        foreach (var item in templist) _richterliste.Add(new PersonenViewModel(item));
        //        fs.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Es ist beim Auswerten der Richterliste aufgetreten: " + ex.Message);
        //    }
        //}







    }
}
