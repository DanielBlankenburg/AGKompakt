using BGH_Kompakt.ViewModel.Sitzungsunterlagen;
using System.Windows.Controls;
using System.Windows.Input;

namespace BGH_Kompakt.Views.Sitzungsunterlagen
{
    /// <summary>
    /// Interaction logic for BSCWServerView.xaml
    /// </summary>
    public partial class BSCWServerView : UserControl
    {
        public BSCWServerViewModel BSCWServerViewModel { get; }
        public BSCWServerView(BSCWServerViewModel bSCWServerViewModel)
        {
            InitializeComponent();
            BSCWServerViewModel = bSCWServerViewModel;
            DataContext = BSCWServerViewModel;
        }

        //Auswahl der Zeilen verhindern
        private void DataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
