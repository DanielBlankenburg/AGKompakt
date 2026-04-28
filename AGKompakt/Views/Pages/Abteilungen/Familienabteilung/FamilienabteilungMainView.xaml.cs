using BGH_Kompakt.ViewModel.Abteilungen.Familienabteilung;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BGH_Kompakt.Views.Pages.Abteilungen.Familienabteilung
{
    /// <summary>
    /// Interaction logic for FamilienabteilungMainView.xaml
    /// </summary>
    public partial class FamilienabteilungMainView : UserControl
    {
        public FamilienabteilungMainViewModel FamilienabteilungMainViewModel { get; }
        public FamilienabteilungMainView(FamilienabteilungMainViewModel familienabteilungMainViewModel)
        {
            InitializeComponent();
            FamilienabteilungMainViewModel = familienabteilungMainViewModel;
            DataContext = FamilienabteilungMainViewModel;
        }

    }
}
