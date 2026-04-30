using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Commands;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BGH_Kompakt.Classes.Helper;

namespace BGH_Kompakt.ViewModel.Abteilungen.Familienabteilung
{
    public class VerfahrensbeistaendeViewModel : ViewModelBase
    {
        public UserDBContext UserDBContext { get; set; } = new UserDBContext();

        private bool _ShowVBPhoto = false;
        public bool ShowVBPhoto
        {
            get { return _ShowVBPhoto; }
            set
            {
                SetProperty(ref _ShowVBPhoto, value);
                //ViewManager.ShowMainInfoFlyout(_SelectedVB.NachName, false);
            }
        }


        public ObservableCollection<Verfahrensbeistand> VBList { get; set; } = new ObservableCollection<Verfahrensbeistand>();

        private Verfahrensbeistand _SelectedVB;
        public Verfahrensbeistand SelectedVB
        {
            get { return _SelectedVB; }
            set
            {
                SetProperty(ref _SelectedVB, value);
                ShowVBPhoto = _SelectedVB.Photo != null;
                //ViewManager.ShowMainInfoFlyout(_SelectedVB.NachName, false);
            }
        }

        public ICommand UploadPhotoCommand { get; }

        public VerfahrensbeistaendeViewModel()
        {
            var query = UserDBContext.Verfahrensbeistaende.Include(x => x.Geschlecht).Include(x => x.Sprachen).OrderBy(x => x.NachName).ThenBy(x => x.VorName).ToArray();
            VBList.Clear();
            foreach (var item in query) VBList.Add(item);

            UploadPhotoCommand = new RelayCommand(async _ => await ExecuteUploadPhotoAsync());
        }

        private async Task ExecuteUploadPhotoAsync()
        {
            if (SelectedVB == null)
            {
                ViewManager.ShowMainInfoFlyout("Bitte zuerst eine Verfahrensbeistand auswählen.", true);
                return;
            }

            var dlg = new OpenFileDialog
            {
                Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff",
                Multiselect = false
            };

            bool? result = dlg.ShowDialog();
            if (result != true) return;

            try
            {
                // Read bytes into the entity
                await SelectedVB.SetPhotoFromFileAsync(dlg.FileName);

                // Persist the change (SelectedVB is tracked because it was loaded from UserDBContext)
                await UserDBContext.SaveChangesAsync();

                ViewManager.ShowMainInfoFlyout("Foto wurde gespeichert.", false);
            }
            catch (Exception ex)
            {
                // Reuse existing error helper
                ErrorMessage.CreateExceptionWithFlyOutMessage("Fehler beim Speichern des Fotos", ex);
            }
        }
    }
}
