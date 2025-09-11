using BGH_Kompakt.ViewModel;
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

namespace BGH_Kompakt.Views
{
    /// <summary>
    /// Interaction logic for NebentaetigkeitenListView.xaml
    /// </summary>
    public partial class NebentaetigkeitenListView : UserControl
    {
        public NebentaetigkeitenListViewModel NebentaetigkeitenListViewModel { get; set; }

        public NebentaetigkeitenListView(NebentaetigkeitenListViewModel nebentaetigkeitenListViewModel)
        {
            InitializeComponent();
            NebentaetigkeitenListViewModel = nebentaetigkeitenListViewModel;
            DataContext = NebentaetigkeitenListViewModel;
        }
    }
}
