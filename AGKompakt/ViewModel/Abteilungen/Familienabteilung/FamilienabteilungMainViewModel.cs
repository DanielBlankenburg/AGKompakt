using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Views.Pages.Abteilungen.Familienabteilung;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Abteilungen.Familienabteilung
{
    public class FamilienabteilungMainViewModel : ViewModelBase
    {
        public ICommand VerfahrensbeistandCommand { get; set; }

        public string Title { get; set; }

        public FamilienabteilungMainViewModel()
        {
            VerfahrensbeistandCommand = new RelayCommand(VerfahrensbeistandExecute);
            Title = "Familienabteilung";
        }

        private void VerfahrensbeistandExecute(object obj)
        {
            ViewManager.ShowPageOnMainView<VerfahrensbeistaendeView>();
        }
    }
}
