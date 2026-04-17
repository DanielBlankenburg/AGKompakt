using BGH_Kompakt.Classes;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.ViewModel;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Serialization;

namespace BGH_Kompakt.Views
{
    /// <summary>
    /// Interaktionslogik für SitzungsplaeneView.xaml
    /// </summary>
    public partial class SitzungsplaeneView : UserControl
    {
        public string strErgebnis = "";

        //public List<Richter> Richterliste = new List<Richter>();
        public static ObservableCollection<PersonenViewModel> _richterList = new ObservableCollection<PersonenViewModel>();
        public static ObservableCollection<Sitzungsdienste> _sitzungList = new ObservableCollection<Sitzungsdienste>();
        public string pathParent = "";
        public string dirParent = "Sitzungsunterlagen\\";
        public string dirApplication = "Unterlagenverwaltung\\";
        public string dirSitzungsplaene = "Sitzungsplaene\\";
        public string dirExe = "";
        public string pathTop = "";
        public int intIndexAktuellesJahr = 0;
        public static ObservableCollection<Sitzungsdienste> Sitzungsplaene = new ObservableCollection<Sitzungsdienste>();
        public static ObservableCollection<Vintages> sitzungsjahre = new ObservableCollection<Vintages>();

        public ListCollectionView Sitzungstageliste_View;

        public SitzungsplaeneView()
        {
            pathParent = BGHKompaktSystemInfo.PathLaufwerksbuchstabe;
            InitializeComponent();
            Cbo_Jahr.ItemsSource = Cbo_Jahr_Fill(pathParent, dirParent, dirApplication, dirSitzungsplaene);
            cbo_Jahr_aktuellesJahr_setzen();
            Cbo_Jahr.SelectedIndex = intIndexAktuellesJahr;
            Richterliste_Fill();
            //Sitzungsdienste_Fill.Fill(_richterList, _sitzungList, pathParent, dirParent, dirApplication, dirSitzungsplaene, Cbo_Jahr.Text, ref Sitzungsplaene);
            Sitzungstageliste_View = new ListCollectionView(Sitzungsplaene);
            UserDataGrid.ItemsSource = Sitzungstageliste_View;
        }

        public static ObservableCollection<Vintages> Cbo_Jahr_Fill(string pathparent, string dirParent, string dirApplication, string dirSitzungsplaene)
        {
            try
            {
                foreach (DirectoryInfo directory in new DirectoryInfo(pathparent + dirParent + dirApplication + dirSitzungsplaene).GetDirectories())
                {
                    if (int.TryParse(directory.Name, out int _))
                        sitzungsjahre.Add(new Vintages(directory.Name));
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("Es ist folgender Fehler beim Auslesen der Jahre aufgetreten: " + ex.Message);
            }
            return sitzungsjahre;
        }

        public void Richterliste_Fill()
        {

            string path = BGHKompaktSystemInfo.PathLaufwerksbuchstabe + BGHKompaktSystemInfo.PathSystemdateien + "Richterliste.xml";
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Person>));
                ObservableCollection<Person> templist = (ObservableCollection<Person>)serializer.Deserialize(fs);
                foreach (var item in templist) _richterList.Add(new PersonenViewModel(item));
                fs.Close();

            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("Es ist beim Auswerten der Richterliste aufgetreten: " + ex.Message);
            }
        
        //using (StreamReader streamReader = new StreamReader(new FileStream(pathparent + dirParent + dirApplication + "Richterliste.txt", FileMode.Open)))
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

        private void cbo_Jahr_aktuellesJahr_setzen()
        {
            int year = DateTime.Now.Year;
            IEnumerator enumerator = ((IEnumerable)Cbo_Jahr.Items).GetEnumerator();
            try
            {
                while (enumerator.MoveNext() && ((Vintages)enumerator.Current).Jahr != year.ToString())
                    ++intIndexAktuellesJahr;
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        private void Cbo_Jahr_DropDownClosed(object sender, EventArgs e)
        {
            //Sitzungsdienste_Fill.Fill(_richterList, _sitzungList, pathParent, dirParent, dirApplication, dirSitzungsplaene, Cbo_Jahr.Text, ref Sitzungsplaene);
            Sitzungstageliste_View = new ListCollectionView(Sitzungsplaene);
            UserDataGrid.ItemsSource = Sitzungstageliste_View;
        }
    }
}
