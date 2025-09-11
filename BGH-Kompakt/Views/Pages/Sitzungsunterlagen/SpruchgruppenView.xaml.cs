using BGH_Kompakt.Classes;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace BGH_Kompakt.Views
{
    /// <summary>
    /// Interaktionslogik für SpruchgruppenView.xaml
    /// </summary>
    public partial class SpruchgruppenView : UserControl
    {
        private string strDateiname = "";
        public string pathParent = "";
        public string dirParent = "Sitzungsunterlagen\\";
        public string dirApplication = "Unterlagenverwaltung\\";
        public static List<Spruchgruppe> Spruchgruppe1 = new List<Spruchgruppe>();
        public static List<Spruchgruppe> Spruchgruppe2 = new List<Spruchgruppe>();
        public static List<Spruchgruppe> Spruchgruppe3 = new List<Spruchgruppe>();
        public static List<Spruchgruppe> Spruchgruppe4 = new List<Spruchgruppe>();
        public static ObservableCollection<PersonenViewModel> _richterList = new ObservableCollection<PersonenViewModel>();
        public static List<Richter> Richterliste = new List<Richter>();

        public SpruchgruppenView()
        {
            InitializeComponent();

            Grid_Aktuelle_Spruchgruppe.Visibility = Visibility.Collapsed;
            Btn_NeueSpruchgruppe.Visibility = Visibility.Collapsed;
            Grid_Spruchgruppe_Neu.Visibility = Visibility.Collapsed;
            pathParent = Assembly.GetExecutingAssembly().Location.Split('\\')[0] + "\\";
            Cbo_Zeitraum.Items.Add("vor 2022");
            Cbo_Zeitraum.Items.Add("ab 2022");
            Cbo_Zeitraum.SelectedIndex = Cbo_Zeitraum.Items.Count - 1;
            Spruchgruppen_Fill(pathParent, 2);
            Richterliste_Fill(pathParent);
        }

        private void Txt_LaufendeNummer_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(Txt_LaufendeNummer.Text != string.Empty))
                return;
            int spruchgruppe = spruchgruppeBerechnen(0, int.Parse(Txt_LaufendeNummer.Text), false);
            txt_Spruchgruppe.Text = spruchgruppe.ToString();
            Data_Spruchgruppe.ItemsSource = Liste_Spruchgruppe(spruchgruppe);
            Grid_Aktuelle_Spruchgruppe.Visibility = Visibility.Visible;
        }

        public void Spruchgruppen_Fill(string pathparent, int intVersion)
        {
            switch (intVersion)
            {
                case 1:
                    strDateiname = "Spruchgruppen.txt";
                    break;
                case 2:
                    strDateiname = "Spruchgruppen_2022.txt";
                    break;
            }
            string path = pathparent + dirParent + dirApplication + strDateiname;
            try
            {
                using (StreamReader streamReader = new StreamReader(new FileStream(path, FileMode.Open)))
                {
                    int id = 1;
                    string str;
                    while ((str = streamReader.ReadLine()) != null)
                    {
                        string[] strArray = str.Split(';');
                        switch (id)
                        {
                            case 1:
                                foreach (string s in strArray)
                                    Spruchgruppe1.Add(new Spruchgruppe(id, int.Parse(s)));
                                break;
                            case 2:
                                foreach (string s in strArray)
                                    Spruchgruppe2.Add(new Spruchgruppe(id, int.Parse(s)));
                                break;
                            case 3:
                                foreach (string s in strArray)
                                    Spruchgruppe3.Add(new Spruchgruppe(id, int.Parse(s)));
                                break;
                            case 4:
                                foreach (string s in strArray)
                                    Spruchgruppe4.Add(new Spruchgruppe(id, int.Parse(s)));
                                break;
                        }
                        ++id;
                    }
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("Es ist beim Auswerten der Spruchgruppenliste aufgetreten: " + ex.Message);
            }
        }

        public void Richterliste_Fill(string pathparent)
        {
            string path = BGHKompaktSystemInfo.PathLaufwerksbuchstabe + BGHKompaktSystemInfo.PathSystemdateien + "Richterliste.xml";
            try
            {
                //_settingList = new ObservableCollection<PersonenViewModel>();
                FileStream fs = new FileStream(path, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Person>));
                ObservableCollection<Person> templist = (ObservableCollection<Person>)serializer.Deserialize(fs);
                foreach (var item in templist) _richterList.Add(new PersonenViewModel(item));
                fs.Close();

                //using (StreamReader streamReader = new StreamReader(new FileStream(path, FileMode.Open)))
                //{
                //    int id = 0;
                //    string str;
                //    while ((str = streamReader.ReadLine()) != null)
                //    {
                //        string[] strArray = str.Split(';');
                //        Richterliste.Add(new Richter(id, strArray[2], strArray[1], strArray[3], strArray[4], strArray[5], int.Parse(strArray[6])));
                //        ++id;
                //    }
                //}
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("Es ist beim Auswerten der Richterliste aufgetreten: " + ex.Message);
            }
        }

        private int spruchgruppeBerechnen(int intZeitraum, int intlaufendeNummer, bool bolVergütung)
        {
            int num1;
            if (intZeitraum == 1)
            {
                if (intlaufendeNummer > 1)
                {
                    if (bolVergütung)
                    {
                        num1 = 6;
                    }
                    else
                    {
                        int num2 = intlaufendeNummer / 7;
                        num1 = intlaufendeNummer - num2 * 7;
                        if (num1 == 0)
                            num1 = 7;
                    }
                }
                else
                    num1 = 0;
            }
            else
                num1 = !bolVergütung ? (intlaufendeNummer % 6 != 0 ? ((intlaufendeNummer + 3) % 6 != 0 ? ((intlaufendeNummer + 1) % 3 != 0 ? 1 : 2) : 3) : 4) : 4;
            return num1;
        }

        public static ObservableCollection<SpruchgruppeAnzeige> Liste_Spruchgruppe(int spruchgruppe)
        {
            ObservableCollection<SpruchgruppeAnzeige> observableCollection = new ObservableCollection<SpruchgruppeAnzeige>();
            List<Spruchgruppe> spruchgruppeList = new List<Spruchgruppe>();
            switch (spruchgruppe)
            {
                case 1:
                    spruchgruppeList = Spruchgruppe1;
                    break;
                case 2:
                    spruchgruppeList = Spruchgruppe2;
                    break;
                case 3:
                    spruchgruppeList = Spruchgruppe3;
                    break;
                case 4:
                    spruchgruppeList = Spruchgruppe4;
                    break;
            }
            try
            {
                foreach (Spruchgruppe spruchgruppe1 in spruchgruppeList)
                {
                    //foreach (Richter richter in Richterliste)
                    //{
                    //    if (richter.ID == spruchgruppe1.RichterID - 1)
                    //        observableCollection.Add(new SpruchgruppeAnzeige(richter.ID, richter.Nachname, richter.Titel, richter.Dienstbezeichnung));
                    //}

                    foreach (PersonenViewModel richter in _richterList)
                    {
                        //int VergleichsID = spruchgruppe1.RichterID - 1;
                        if (richter.ID.Value == spruchgruppe1.RichterID.ToString())
                            observableCollection.Add(new SpruchgruppeAnzeige(richter.ID.Value, richter.Nachname.Value, richter.Titel.Value, richter.Dienstbezeichnung.Value));
                    }
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("Die Spruchgrupppe konnten nicht ausgelesen werden. Es ist folgender Fehler aufgetreten: " + ex.Message);
            }
            return observableCollection;
        }
    }
}
