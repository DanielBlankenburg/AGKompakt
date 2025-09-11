using BGH_Kompakt.ViewModel.Sitzungsunterlagen;
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

namespace BGH_Kompakt.Views.Sitzungsunterlagen
{
    /// <summary>
    /// Interaction logic for SitzungsunterlagenView.xaml
    /// </summary>
    public partial class SitzungsunterlagenView : UserControl
    {
        public SitzungsunterlagenViewModel SitzungsunterlagenViewModel { get; }
        public SitzungsunterlagenView(SitzungsunterlagenViewModel sitzungsunterlagenViewModel)
        {
            InitializeComponent();
            SitzungsunterlagenViewModel = sitzungsunterlagenViewModel;
            DataContext = SitzungsunterlagenViewModel;
        }

    }
}
