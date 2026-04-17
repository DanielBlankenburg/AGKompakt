using BGH_Kompakt.ViewModel.Montagspost;
using System.Windows.Controls;

namespace BGH_Kompakt.Views.Montagspost
{
    /// <summary>
    /// Interaction logic for MontagsPostBE.xaml
    /// </summary>
    public partial class MontagsPostBE : UserControl
    {
        public MontagsPostBEViewModel MontagsPostBEViewModel { get; }
        public MontagsPostBE(MontagsPostBEViewModel montagsPostBEViewModel)
        {
            InitializeComponent();
            MontagsPostBEViewModel = montagsPostBEViewModel;
            DataContext = MontagsPostBEViewModel;
        }

    }
}
