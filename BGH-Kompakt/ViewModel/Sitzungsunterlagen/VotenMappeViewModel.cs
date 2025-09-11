using BGH_Kompakt.Classes.Sitzungsunterlagen;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.Views.Sitzungsunterlagen;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Sitzungsunterlagen
{
    public class VotenMappeViewModel : ViewModelBase
    {
        public ObservableCollection<VerfahrenVotenmappe> Votenlist { get; set; } = new ObservableCollection<VerfahrenVotenmappe>();
        public UserDBContext userDBContext = new UserDBContext();
        public ICommand EMailCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand BackCommand { get; set; }

        private VerfahrenVotenmappe _selectedVerfahren;
        public VerfahrenVotenmappe SelectedVerfahren
        {
            get { return _selectedVerfahren; }
            set { SetProperty(ref _selectedVerfahren, value); }
        }



        public VotenMappeViewModel()
        {
            EMailCommand = new RelayCommand(EMailExecute);
            DeleteCommand = new RelayCommand(DeleteExecute, DeleteCanExecute);
            ClearCommand = new RelayCommand(ClearExecute);
            BackCommand = new RelayCommand(BackExecute);

            var Query = userDBContext.Votenmappe.Where(x => x.Senat.SenatID == UserManager.SenatSettings.SenatID).ToArray();
            foreach (var item in Query) Votenlist.Add(item);
        }

        private bool DeleteCanExecute(object obj)
        {
            return (SelectedVerfahren != null);

        }
        private void DeleteExecute(object obj)
        {
            if (MessageBox.Show($"Soll das Verfahren {SelectedVerfahren.Verfahren_Anzeigedaten} aus der Liste entfernt werden?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    userDBContext.Votenmappe.Remove(SelectedVerfahren);
                    userDBContext.SaveChanges();
                    Votenlist.Remove(SelectedVerfahren);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Das Verfahren konnte nicht gelöscht werden. Es ist folgender Fehler aufgetreten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void BackExecute(object obj)
        {
            ViewManager.ShowPageOnMainView<SitzungsunterlagenView>();
        }

        private void ClearExecute(object obj)
        {
            if (MessageBox.Show("Soll die Votenmappe geleert werden?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    VotenlistClear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Das Verfahren konnte nicht gelöscht werden. Es ist folgender Fehler aufgetreten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void VotenlistClear()
        {
            List<VerfahrenVotenmappe> RemoveList = userDBContext.Votenmappe.Where(x => x.Senat.SenatID == UserManager.SenatSettings.SenatID).ToList();
            userDBContext.Votenmappe.RemoveRange(RemoveList);
            userDBContext.SaveChanges();
            Votenlist.Clear();
        }

        private void EMailExecute(object obj)
        {
            string Text = string.Empty;
            string anredeText = "Sehr gehrte Damen und Herren, <br> <br> für folgende Verfahren wurden Voten eingestellt: <br> <br> ";

            foreach (VerfahrenVotenmappe item in Votenlist)
            {
                Text += item.Verfahren_Anzeigedaten + "<br>";
            }
            EMailVersand eMailVersand = new EMailVersand();
            eMailVersand.EMailAlleAktivenRichterWiMa(Text, "Neue Voten", anredeText);
            if (MessageBox.Show("Möchten Sie die Votenliste leeren?", "Votenliste leeren", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                VotenlistClear();
            }

        }


    }
}
