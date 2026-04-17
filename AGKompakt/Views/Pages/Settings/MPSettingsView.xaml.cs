using BGH_Kompakt.ViewModel.SystemSettings;
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

namespace BGH_Kompakt.Views.Settings
{
    /// <summary>
    /// Interaction logic for MPSettingsView.xaml
    /// </summary>
    public partial class MPSettingsView : UserControl
    {
        public MPSettingsViewModel MPSettingsViewModel { get; }
        public MPSettingsView(MPSettingsViewModel mPSettingsViewModel)
        {
            InitializeComponent();
            MPSettingsViewModel = mPSettingsViewModel;
            DataContext = MPSettingsViewModel;
        }

    }
}
