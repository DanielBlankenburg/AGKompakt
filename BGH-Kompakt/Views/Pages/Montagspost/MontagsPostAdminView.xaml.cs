using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Views.Montagspost;
using System.Windows.Controls;

namespace BGH_Kompakt.Views.Pages.Montagspost
{
    /// <summary>
    /// Interaction logic for MontagsPostAdminView.xaml
    /// </summary>
    public partial class MontagsPostAdminView : UserControl
    {
        public MontagsPostAdminView()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tabItem = ((sender as TabControl).SelectedItem as TabItem).Name as string;
            switch (tabItem)
            {
                case "AddKW":
                    ViewManager.ShowUnderPageOn<MontagspostImportView>(AnimatedContentControl);
                    break;
                case "Search":
                    ViewManager.ShowMainInfoFlyout("Dieser Bereich befindet sich noch in Entwicklung", false);
                    //ViewManager.ShowUnderPageOn<MontagsPostFilterView>(AnimatedContentControl);
                    break;
                case "BE":
                    ViewManager.ShowUnderPageOn<MontagsPostBE>(AnimatedContentControl);
                    //ViewManager.ShowUnderPageOn<MontagsPostFilterView>(AnimatedContentControl);
                    break;
            }

        }
    }
}
