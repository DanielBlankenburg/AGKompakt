using BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps;
using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Classes.SystemSettings;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Enums;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.SystemSettings
{
    public class ProgrammSettingViewModel : ViewModelBase
    {
        public ICommand SeedMontagspostCommand { get; set; }
        public ICommand SeedAZCommand { get; set; }
        public ICommand SeedARCommand { get; set; }
        public ICommand TestPath { get; set; }
        public ProgrammSetting ProgrammSetting { get; set; }
        private readonly MPDBContext mpContext = new MPDBContext();
        private readonly UserDBContext userDBContext = new UserDBContext();
        private readonly ActivityRequestDBContext arDBContext = new ActivityRequestDBContext();

        private bool _AnzeigeMontagspost;
        public bool AnzeigeMontagspost
        {
            get => _AnzeigeMontagspost;
            set 
            {
                _AnzeigeMontagspost = value;
                ProgrammSetting.MontagspostActivated = value;
                userDBContext.ProgrammSettings.AddOrUpdate(ProgrammSetting);
                userDBContext.SaveChanges();
            }
        }

        private bool _AnzeigeActivitRequests;
        public bool AnzeigeActivitRequests
        {
            get => _AnzeigeActivitRequests;
            set
            {
                _AnzeigeActivitRequests = value;
                ProgrammSetting.ActivityRequestActivated = value;
                userDBContext.ProgrammSettings.AddOrUpdate(ProgrammSetting);
                userDBContext.SaveChanges();
            }
        }



        public ProgrammSettingViewModel()
        {
            SeedMontagspostCommand = new RelayCommand(SeedMontagspostExecute);
            SeedAZCommand = new RelayCommand(SeedAZExecute);
            TestPath = new RelayCommand(TestPathExecute);
            SeedARCommand = new RelayCommand(SeedARExecute);

            ProgrammSetting = userDBContext.ProgrammSettings.FirstOrDefault() ?? new ProgrammSetting();
            AnzeigeMontagspost = ProgrammSetting.MontagspostActivated;
            AnzeigeActivitRequests = ProgrammSetting.ActivityRequestActivated;
        }

        private void SeedARExecute(object obj)
        {
            try
            {
                #region SeedAR

                List<String> List = new List<String> { "Öffentlicher Auftraggeber", "Verlage", "Berufsverbände", "Private Seminarveranstalter", "Sonstige" };
                foreach (string suchText in List)
                {
                    if (arDBContext.ActivityClientTyps.FirstOrDefault(x => x.ActivityClientTypText == suchText) == null)
                        arDBContext.ActivityClientTyps.AddOrUpdate(a => a.ActivityClientTypText, new ActivityClientTyp { ActivityClientTypText = suchText });
                };

                List.Clear();
                List = new List<String> { "Anzeigepflichtig", "Genehmigungspflichtig" };
                foreach (string suchText in List)
                {
                    if (arDBContext.ActivityRequestMeldeArten.FirstOrDefault(x => x.ActivityRequestMeldeArtText == suchText) == null)
                        arDBContext.ActivityRequestMeldeArten.AddOrUpdate(a => a.ActivityRequestMeldeArtText, new ActivityRequestMeldeArt { ActivityRequestMeldeArtText = suchText });
                };

                List.Clear();
                List = new List<String> { "Wissenschaftliche Veröffentlichung", "Lehr- oder sonstige wissenschaftliche Tätigkeit" };
                foreach (string suchText in List)
                {
                    //ActivityRequestScienceCategorie test = arDBContext.ActivityRequestScienceCategories.FirstOrDefault(x => x.ActivityRequestScienceCategorieText == suchText);
                    //Debug.WriteLine(test.ActivityRequestScienceCategorieText);
                    if (arDBContext.ActivityRequestScienceCategories.FirstOrDefault(x => x.ActivityRequestScienceCategorieText == suchText) == null)
                        arDBContext.ActivityRequestScienceCategories.AddOrUpdate(a => a.ActivityRequestScienceCategorieText, new ActivityRequestScienceCategorie { ActivityRequestScienceCategorieText = suchText });
                };

                List.Clear();
                List = new List<String> { "Aufsatz", "Monographie", "Kommentar", "Zeitschrift", "Handbuch", "Sonstiges" };
                foreach (string suchText in List)
                {
                    if (arDBContext.ActivityRequestScienceTyps.FirstOrDefault(x => x.ActivityRequestScienceTypText == suchText) == null)
                        arDBContext.ActivityRequestScienceTyps.AddOrUpdate(a => a.ActivityRequestScienceTypText, new ActivityRequestScienceTyp { ActivityRequestScienceTypText = suchText });
                };


                List.Clear();
                List = new List<String> { "Autor/Mitautor", "Schriftleitung", "Herausgaber/in", "wissenschaftlicher Beirat", "sonstige Mitwirkung" };
                foreach (string suchText in List)
                {
                    if (arDBContext.ActivityRequestScienceAuthorNames.FirstOrDefault(x => x.ActivityRequestScienceAuthorText == suchText) == null)
                        arDBContext.ActivityRequestScienceAuthorNames.AddOrUpdate(a => a.ActivityRequestScienceAuthorText, new ActivityRequestScienceAuthorName { ActivityRequestScienceAuthorText = suchText });
                };

                List.Clear();
                List = new List<String> { "Präsenz", "Online" };
                foreach (string suchText in List)
                {
                    if (arDBContext.ActivityRequestOrtArten.FirstOrDefault(x => x.ActivityRequestOrtArtText == suchText) == null)
                        arDBContext.ActivityRequestOrtArten.AddOrUpdate(a => a.ActivityRequestOrtArtText, new ActivityRequestOrtArt { ActivityRequestOrtArtText = suchText });
                };


                List.Clear();
                List = new List<String> { "Parteien gemeinsam", "Unbeteiligte Stelle" };
                foreach (string suchText in List)
                {
                    if (arDBContext.ActivityRequestArbitrationTyps.FirstOrDefault(x => x.ActivityRequestArbitrationTypText == suchText) == null)
                        arDBContext.ActivityRequestArbitrationTyps.AddOrUpdate(a => a.ActivityRequestArbitrationTypText, new ActivityRequestArbitrationTyp { ActivityRequestArbitrationTypText = suchText });
                };


                List.Clear();
                List = new List<String> { "Einmalig", "Pro Jahr" };
                foreach (string suchText in List)
                {
                    if (arDBContext.ActivityRequestFrequencies.FirstOrDefault(x => x.ActivityRequestFrequencyText == suchText) == null)
                        arDBContext.ActivityRequestFrequencies.AddOrUpdate(a => a.ActivityRequestFrequencyText, new ActivityRequestFrequency { ActivityRequestFrequencyText = suchText });
                };

                List.Clear();
                List = new List<String> { "Essen/Getränke", "Hotelkosten", "Überschießende Reisekosten", "Sonstiges" };
                foreach (string suchText in List)
                {
                    if (arDBContext.ARVerguetungAdventageTyps.FirstOrDefault(x => x.ARVerguetungAdventageTypText == suchText) == null)
                        arDBContext.ARVerguetungAdventageTyps.AddOrUpdate(a => a.ARVerguetungAdventageTypText, new ARVerguetungAdventageTyp { ARVerguetungAdventageTypText = suchText });
                };

                List<ActivityRequestTyp> TypList = new List<ActivityRequestTyp>();
                TypList.AddRange(new List<ActivityRequestTyp> {

                        new ActivityRequestTyp { ActivityRequestTypText = "Vortragstätigkeit", ActivityRequestTypMeldeArt = 1 },
                        new ActivityRequestTyp { ActivityRequestTypText = "Wissenschaftliche Tätigkeit", ActivityRequestTypMeldeArt = 1 },
                        new ActivityRequestTyp { ActivityRequestTypText = "Schriftstellerische Tätigkeit", ActivityRequestTypMeldeArt = 1 },
                        new ActivityRequestTyp { ActivityRequestTypText = "Künstlerische Tätigkeit", ActivityRequestTypMeldeArt = 1 },
                        new ActivityRequestTyp { ActivityRequestTypText = "Referententätigkeit", ActivityRequestTypMeldeArt = 2 },
                        new ActivityRequestTyp { ActivityRequestTypText = "Prüfertätigkeit", ActivityRequestTypMeldeArt = 2 },
                        new ActivityRequestTyp { ActivityRequestTypText = "Tätigkeit als Schiedsrichter/ Schiedsgutachter", ActivityRequestTypMeldeArt = 2 },
                        new ActivityRequestTyp { ActivityRequestTypText = "Sonstige Nebentätigkeit", ActivityRequestTypMeldeArt = 2 }
                    });

                foreach (ActivityRequestTyp suchText in TypList)
                {
                    if (arDBContext.ActivityRequestTyps.FirstOrDefault(x => x.ActivityRequestTypText == suchText.ActivityRequestTypText) == null)
                        arDBContext.ActivityRequestTyps.AddOrUpdate(a => a.ActivityRequestTypText, new ActivityRequestTyp { ActivityRequestTypText = suchText.ActivityRequestTypText, ActivityRequestTypMeldeArt = suchText.ActivityRequestTypMeldeArt });
                };

                List.Clear();
                List = new List<String> { "durch Antragsteller/in eingereicht", "durch Präsidialrichter/in als genehmigungsfähig weiterleitet", "durch Präsidialrichter/in als ablehnungsreif", "durch Präsident/in genehmigung", "durch Präsident/in abgelehnt", "durch Vorsitzende/n als genehmigungsfähig weiterleitet", "durch Vorsitzende/n als ablehnungsreife weiterleitet" };
                foreach (string suchText in List)
                {
                    if (arDBContext.ActivityRequestStatuses.FirstOrDefault(x => x.ActivityRequestStatusText == suchText) == null)
                        arDBContext.ActivityRequestStatuses.AddOrUpdate(a => a.ActivityRequestStatusText, new ActivityRequestStatus { ActivityRequestStatusText = suchText });
                }
                ;


                //List.Clear();
                //List = new List<String> { "Erhaltene Vergütung", "Honorarfrei", "derzeit unbekannte Vergütung" };
                //foreach (string suchText in List)
                //{
                //    if (arDBContext.ActivityRequestVerguetungTypes.FirstOrDefault(x => x.ARVerguetungTypText == suchText) == null)
                //        arDBContext.ActivityRequestVerguetungTypes.AddOrUpdate(a => a.ARVerguetungTypText, new ARVerguetungTyp { ARVerguetungTypText = suchText });
                //}


                arDBContext.SaveChanges();
                ViewManager.ShowMainInfoFlyout($"Die Werte für die Nebentätigkeiten wurden gesetzt.", false);

                #endregion
            }
            catch (Exception)
            {
                ViewManager.ShowMainInfoFlyout("Die Werte für die Nebentätigkeiten konnten nicht gesetzt werden.", false);
            }
        }

        private void TestPathExecute(object obj)
        {
            string path = $"{ProgrammSetting.PathDokstelleDFS}{ProgrammSetting.PathDokstelle}";
            if (Directory.Exists(path))
            {
                ViewManager.ShowMainInfoFlyout($"Der Pfad {path} wurde gefunden.", false);
            }
            else
            {
                ViewManager.ShowMainInfoFlyout($"Der Pfad {path} konnte nicht gefunden werden.", false);
            }

        }

        private void SeedMontagspostExecute(object obj)
        {
            try
            {
                #region SeedMontagspost

                foreach (MPEnums.EnumMPCategories val in Enum.GetValues(typeof(MPEnums.EnumMPCategories)))
                {
                    if (mpContext.MPCategories.FirstOrDefault(x => x.MPCategoryText == val.ToString()) == null)
                        mpContext.MPCategories.AddOrUpdate(a => a.MPCategoryText, new MPCategory { MPCategoryText = val.ToString() });
                }



                List<String> MPList = new List<String> { "I", "II", "III", "IV", "V", "VI", "VIa", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "1", "2", "3", "4", "5", "6", "KVB", "EnVR", "StB", "AnwZ(Brfg)", "AnwStr(B)", "LwZR", "NotZ", "NotStr", "AK", "Unbekannt", "KVR", "GSSt", "GSZ" };
                foreach (string suchText in MPList)
                {
                    if (mpContext.MPSenateAbbreviation.FirstOrDefault(x => x.MPSenatAbbreviationText == suchText) == null)
                        mpContext.MPSenateAbbreviation.AddOrUpdate(a => a.MPSenatAbbreviationText, new MPSenatAbbreviation { MPSenatAbbreviationText = suchText });
                }


                //Seed MPSenate
                List<MPSenat> MPSenatList = new List<MPSenat>();
                MPSenatList.AddRange(new List<MPSenat> {

                        new MPSenat("unbekannter Senat", 1, 1),
                        new MPSenat("I. Zivilsenat", 2, 1),
                        new MPSenat("II. Zivilsenat", 2, 2),
                        new MPSenat("III. Zivilsenat", 2, 3),
                        new MPSenat("IV. Zivilsenat", 2, 4),
                        new MPSenat("V. Zivilsenat", 2, 5),
                        new MPSenat("VI. Zivilsenat", 2, 6),
                        new MPSenat("VIa. Zivilsenat", 2, 7),
                        new MPSenat("VII. Zivilsenat", 2, 8),
                        new MPSenat("VIII. Zivilsenat", 2, 9),
                        new MPSenat("IX. Zivilsenat", 2, 10),
                        new MPSenat("X. Zivilsenat", 2, 11),
                        new MPSenat("XI. Zivilsenat", 2, 12),
                        new MPSenat("XII. Zivilsenat", 2, 13),
                        new MPSenat("XIII. Zivilsenat", 2, 14),
                        new MPSenat("1. Strafsenat", 3, 1),
                        new MPSenat("2. Strafsenat", 3, 2),
                        new MPSenat("3. Strafsenat", 3, 3),
                        new MPSenat("4. Strafsenat", 3, 4),
                        new MPSenat("5. Strafsenat", 3, 5),
                        new MPSenat("6. Strafsenat", 3, 6),
                        new MPSenat("Anwaltssenat", 4, 4),
                        new MPSenat("Notarsenat", 4, 5),
                        new MPSenat("Anwaltssenat", 4, 6),
                        new MPSenat("Landwirtschaftssenat", 4, 10),
                        new MPSenat("Patentanwaltsenat", 4, 6),
                        new MPSenat("Steuerberatersenat", 4, 7),
                        new MPSenat("Gemeinsamer Senat der obersten Gerichtshöfe", 4, 1),
                        new MPSenat("Großer Zivilsenat", 4, 2),
                        new MPSenat("Großer Strafsenat", 4, 3),
                        new MPSenat("Dienstgericht", 4, 8),
                        new MPSenat("Kartellsenat", 4, 9),
                        new MPSenat("Vereinigte Große Senate", 4, 11),
                        new MPSenat("Wirtschaftsprüfersenat", 4, 12),
                        new MPSenat("Ermittlungsrichter", 3, 7)



                    });

                foreach (MPSenat suchText in MPSenatList)
                {
                    if (mpContext.MPSenate.FirstOrDefault(x => x.MPSenatName == suchText.MPSenatName) == null)
                        mpContext.MPSenate.AddOrUpdate(a => a.MPSenatName, new MPSenat { MPSenatName = suchText.MPSenatName, MPSenatSorting = suchText.MPSenatSorting, MPCategorieID = suchText.MPCategorieID });
                }

                //Seed MPSettings
                MPSetting newSetting = new MPSetting
                {
                    MPSettingEMailAnrede = "Sehr geehrte Damen und Herren",
                    MPSettingEMailSchluss = "Mit freundlichen Grüßen <br> <br> Bundesgerichtshof <br> - Montagspost - <br> Mail: montagspost@bgh.bund.de",
                    MPSettingDatenschutzhinweis = "Informationen nach Artikel 12 bis 14 Datenschutz-Grundverordnung (DSGVO):<br> " +
                                                   "Der Bundesgerichtshof verarbeitet zur Erfüllung seiner Aufgaben Ihre personenbezogenen Daten in gesetzlich geregelten Verfahren. " +
                                                   "Weitere Ausführungen zum Datenschutz des Bundesgerichtshofs können Sie dem Internetangebot des Bundesgerichtshofs im Bereich Datenschutz (http://www.bundesgerichtshof.de/DE/Oben/Datenschutz/datenschutz_node.html) entnehmen."
                };
                mpContext.MPSettings.AddOrUpdate(a => a.MPSettingEMailAnrede, new MPSetting
                {
                    MPSettingEMailAnrede = newSetting.MPSettingEMailAnrede,
                    MPSettingEMailSchluss = newSetting.MPSettingEMailSchluss,
                    MPSettingDatenschutzhinweis = newSetting.MPSettingDatenschutzhinweis
                });

                //Seed MPEMails
                List<MPEMail> MPEMailList = new List<MPEMail>();
                MPEMailList.AddRange(new List<MPEMail> {

                        new MPEMail
                        {
                            MPEMailDescription = "Anschreiben Externe Empfänger",
                            MPEMailBody = "im Anhang erhalten Sie die Entscheidungen des Bundesgerichtshofs aus der Kalenderwoche 'KW'.",
                            MPEMailSubject = "Montagspost Kalenderwoche 'KW'"
                        },
                        new MPEMail
                        {
                            MPEMailDescription = "Anschreiben Interne Empfänger",
                            MPEMailBody = "die Entscheidungen aus der Kalenderwoche 'KW' wurden bereitgestellt.",
                            MPEMailSubject = "Montagspost Kalenderwoche 'KW'"
                        },
                        new MPEMail
                        {
                            MPEMailDescription = "Anschreiben Berichtigung",
                            MPEMailBody = "hinsichtlich der Entscheidung in dem Verfahren 'VerfahrenAZ' (versendet in Kw 'KW') wird auf folgendes hinzuweisen: <br><br>'Vermerk'<br><br>" +
                            "Die korrigierte Fassung wird in den nächsten Tagen ins Internet eingespielt. Sie erhalten die Entscheidung dann zu gegebenem Zeitpunkt mit der Montagspost.",
                            MPEMailSubject = "Berichtigung der Entscheidung 'VerfahrenAZ'"
                        }

                    });

                foreach (MPEMail suchText in MPEMailList)
                {
                    if (mpContext.MPEMails.FirstOrDefault(x => x.MPEMailDescription == suchText.MPEMailDescription) == null)
                        mpContext.MPEMails.AddOrUpdate(a => a.MPEMailDescription, new MPEMail
                        {
                            MPEMailDescription = suchText.MPEMailDescription,
                            MPEMailBody = suchText.MPEMailBody,
                            MPEMailSubject = suchText.MPEMailSubject

                        });
                }

                List<MPVermerk> MPVermerkList = new List<MPVermerk>();
                MPVermerkList.AddRange(new List<MPVermerk> {

                        new MPVermerk { MPVermerkText = "Es ergeht ein Berichtigungsbeschluss."},
                        new MPVermerk { MPVermerkText = "Es wurde eine Schreibfehlerberichtigung vorgenommen."},
                        new MPVermerk { MPVermerkText = "Die Entscheidung ist nicht vollständig anonymisiert."},
                        new MPVermerk { MPVermerkText = "Es erging nachträglich ein Leitsatz."}
                    });

                foreach (MPVermerk suchText in MPVermerkList)
                {
                    if (mpContext.MPVermerke.FirstOrDefault(x => x.MPVermerkText == suchText.MPVermerkText) == null)
                        mpContext.MPVermerke.AddOrUpdate(a => a.MPVermerkText, new MPVermerk {MPVermerkText= suchText.MPVermerkText});
                }


                mpContext.SaveChanges();
                MessageBox.Show("Die Werte für die Montagspost wurden gesetzt.", "Seed Montagspost", MessageBoxButton.OK, MessageBoxImage.Information);

                #endregion
            }
            catch (Exception)
            {
                ViewManager.ShowMainInfoFlyout("Die Werte für die Montagspost konnten nicht gesetzt.", false);
            }
        }

        private void SeedAZExecute(object obj)
        {
            var query = userDBContext.SenatSettings.ToArray();
            foreach (SenatSetting s in query)
            {
                if(userDBContext.SenatAktenzeichen.FirstOrDefault(f => f.SenatSetting.SenatSettingID  == s.SenatSettingID) == null)
                {
                    if(s.Senat.SenatArt == 1)
                    {
                        for(int i = 0; i < 3; i++)
                        {
                            string azName = string.Empty;
                            switch(i)
                            {
                                case 0:
                                    azName = "ZR";
                                    break;
                                case 1:
                                    azName = "ZB";
                                    break;
                                case 2:
                                    azName = "ZA";
                                    break;
                            }
                            SenatAktenzeichen NewAktenzeichen = new SenatAktenzeichen
                            {
                                SenatAktenzeichenName = azName,
                                SenatAktenzeichenNameRaw = azName,
                                SenatAktenzeichenOrderNumber = i+1,
                                SenatSetting = s
                            };
                            try
                            {
                                userDBContext.SenatAktenzeichen.Add(NewAktenzeichen);
                                userDBContext.SaveChanges();
                            }
                            catch (System.Exception)
                            {
                            }
                        }
                    }
                }
            }
        }
    }
}
