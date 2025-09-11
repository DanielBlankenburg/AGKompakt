using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.ViewModel;
using BGH_Kompakt.Views.Montagspost;
using System.Windows;

namespace BGH_Kompakt.Views
{
    /// <summary>
    /// Interaktionslogik für MontagsPostView.xaml
    /// </summary>
    public partial class MontagsPostView : System.Windows.Controls.UserControl
    {

        public MontagsPostViewModel MontagsPostViewModel { get; }
        public MontagsPostView(MontagsPostViewModel montagsPostViewModel)
        {

            InitializeComponent();
            MontagsPostViewModel = montagsPostViewModel;
            DataContext = MontagsPostViewModel;
        }
        
        private void btn_Filter_Click(object sender, RoutedEventArgs e)
        {
            ViewManager.ShowPageOnMainView<MontagsPostFilterView>();

        }

        //private void DataGridDetail_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    MessageBox.Show("Test");
        //}
    }
}
