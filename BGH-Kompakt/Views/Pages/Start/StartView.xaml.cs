using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.ViewModel.Start;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace BGH_Kompakt.Views.Start
{
    /// <summary>
    /// Interaction logic for StartView.xaml
    /// </summary>
    public partial class StartView : UserControl
    {
        public StartViewModel StartViewModel { get; }
        public StartView(StartViewModel startViewModel)
        {
            InitializeComponent();
            StartViewModel = startViewModel;
            DataContext = StartViewModel;

        }


        public void OpenPageOnMain(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (sender is Button button)
                {
                    Logger.WriteLog($"Es wurde der Button {button.Name} angeklickt.");
                    ViewManager.OpenMainMenu(button.Name);
                }
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Bei der Anzeige des ausgewählten Bereichs ist folgender Fehler aufgetreten: {ex.Message}.", false);
            }

        }

        private void Btn_Main_Instruction_Click(object sender, RoutedEventArgs e)
        {

            //string pathParent = Assembly.GetExecutingAssembly().Location.Split('\\')[0] + "\\";
            Logger.WriteLog("Es wurde die Anleitung geöffnet");
            string fileName = BGHKompaktSystemInfo.PathLaufwerksbuchstabe + BGHKompaktSystemInfo.PathSystemdateien + "Anleitung BGH-Kompakt.pdf";

            try
            {
                Process.Start(new ProcessStartInfo(fileName)
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Die Anleitung konnte nicht geöffnet werden. Es ist folgender Fehler aufgetreten:" + Environment.NewLine + Environment.NewLine + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void InfoClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    if (button == BtnInfoSitzungsunterlagen)
                    {
                        ViewManager.ShowDescriptionWindow("Der Bereich der Sitzungsunterlagen dient zur Verwaltung der Senatslaufwerke.\n" +
                            "In diesem Bereich können Sie einzelnen Sitzungstage verwalten.\n\n Es stehen folgende Möglichkeiten zur Auswahl:\n\n"+
                            "- Anlegen/ Editieren von Sitzungstagen\n" +
                            "- Anlegen/ Editieren von Verfahren\n" +
                            "- Automatisches Umbenennen von Dokumenten\n" +
                            "- Umwandlung von Word- in pdf-Dateien\n" +
                            "- Übertragügung der Vollständigkeit des BSCW-Servers\n" +
                            "- Übertragung von Dokumenten auf den BSCW-Server\n" +
                            "- Führung einer Votenmappe\n" +
                            "- Zentrale Sammlung der Sitzungs- und Beratungslisten\n",
                            button.Name);
                        e.Handled = true;
                    }
                    if (button == BtnInfoSpruchgruppen)
                    {
                        ViewManager.ShowDescriptionWindow("In diesem Bereich kann durch die Eingabe des Aktenzeichens die Spruchgruppe für das Verfahren bestimmt werden.\n\n" +
                            "ACHTUNG: Es ist erforderlich, dass für den Senat die entsprechenden Einstellungen vorgenommen wurden. Sollte dies nicht der Fall sein" +
                            "wenden Sie sich bitte an den Administrator.",
                            button.Name);
                        e.Handled = true;
                    }
                    if (button == BtnInfoNebentaetigkeiten)
                    {
                        ViewManager.ShowDescriptionWindow("Der Bereich der Nebentätigkeiten dient zur Anzeige einer Nebentätigkeit bzw. der Beantrag der Genehmigung einer solchen Tätigkeit.\n" +
                            "Es stehen folgende Möglichkeiten zur Auswahl:\n\n" +
                            "- Anzeige von Nebentätigkeiten\n" +
                            "- Beantragung einer Genehmigung für eine Nebentätigkeit\n" +
                            "- Übersicht über die angezeigten/ beantragten Tätigkeiten\n",
                            button.Name);
                        e.Handled = true;
                    }
                    if (button == BtnInfoSitzungsplaene)
                    {
                        ViewManager.ShowDescriptionWindow("Der Bereich dient zur Verwaltung der Sitzungsdienste für die wissenschaftlichen Mitarbeiter.\n", button.Name);
                        e.Handled = true;
                    }
                    if (button == BtnInfoMontagspost)
                    {
                        ViewManager.ShowDescriptionWindow("Über diesen Bereich kann die digitale Montagspost eingesehen werden.\n" +
                            "Es stehen folgende Möglichkeiten zur Auswahl:\n\n" +
                            "- Auswahl der Montagspost nach den Kalenderwochen\n" +
                            "- Filtereinstellungen\n" +
                            "- Eigene Mappe für Auswahl an Entscheidungen\n" +
                            "- Übertragung auf den BSCW-Server\n",
                            button.Name);
                        e.Handled = true;
                    }
                    if (button == BtnInfoMontagspostAdmin)
                    {
                        ViewManager.ShowDescriptionWindow("Der Bereich dient zur Verwaltung der Montagspost.\n" +
                            "Es stehen folgende Möglichkeiten zur Auswahl:\n\n" +
                            "- Montagsapost einlesen\n" +
                            "- Entscheidungssuche\n" +
                            "- Erstellung von Vermerken\n",
                            button.Name);
                        e.Handled = true;
                    }

                }
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Bei der Anzeige des ausgewählten Bereichs ist folgender Fehler aufgetreten: {ex.Message}.", false);
            }
        }
    }
}
