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
    /// Interaction logic for VotenMappeView.xaml
    /// </summary>
    public partial class VotenMappeView : UserControl
    {
        public VotenMappeViewModel VotenMappeViewModel { get; }
        public VotenMappeView(VotenMappeViewModel votenMappeViewModel)
        {
            InitializeComponent();
            VotenMappeViewModel = votenMappeViewModel;
            DataContext = VotenMappeViewModel;
        }

    }
}
