using BGH_Kompakt.ViewModel.Montagspost;
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

namespace BGH_Kompakt.Views.Montagspost
{
    /// <summary>
    /// Interaction logic for MontagsPostVermerkView.xaml
    /// </summary>
    public partial class MontagsPostVermerkView : UserControl
    {
        public MontagsPostVermerkViewModel MontagsPostVermerkViewModel { get; }
        public MontagsPostVermerkView(MontagsPostVermerkViewModel montagsPostVermerkViewModel)
        {
            InitializeComponent();
            MontagsPostVermerkViewModel = montagsPostVermerkViewModel;
            DataContext = MontagsPostVermerkViewModel;
        }

    }
}
