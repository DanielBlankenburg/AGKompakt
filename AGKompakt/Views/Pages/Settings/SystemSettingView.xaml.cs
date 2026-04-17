using BGH_Kompakt.ViewModel.SystemSettings;
using System.Windows.Controls;

namespace BGH_Kompakt.Views.Settings
{
    /// <summary>
    /// Interaction logic for SystemSettingView.xaml
    /// </summary>
    public partial class SystemSettingView : UserControl
    {
        public SystemSettingViewModel SystemSettingViewModel { get; }
        public SystemSettingView(SystemSettingViewModel systemSettingViewModel)
        {
            InitializeComponent();
            SystemSettingViewModel = systemSettingViewModel;
            DataContext = SystemSettingViewModel;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
