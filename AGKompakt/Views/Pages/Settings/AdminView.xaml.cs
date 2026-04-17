using BGH_Kompakt.ViewModel.SystemSettings;
using System.Windows.Controls;

namespace BGH_Kompakt.Views.SystemSettingsView
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : UserControl
    {
        public AdminViewModel AdminViewModel { get; }
        public AdminView(AdminViewModel adminViewModel)
        {
            InitializeComponent();
            AdminViewModel = adminViewModel;
            DataContext = AdminViewModel;
        }

    }
}
