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
                case "EMails":
                    ViewManager.ShowUnderPageOn<MontagsPostEMailsView>(AnimatedContentControl);
                    break;
                case "MPEditor":
                    ViewManager.ShowUnderPageOn<MontagsPostEditorView>(AnimatedContentControl);
                    break;
                case "AddKW":
                    ViewManager.ShowUnderPageOn<MontagspostImportView>(AnimatedContentControl);
                    break;
                case "MetadataEdit":
                    ViewManager.ShowUnderPageOn<MontagsPostMetadataEditView>(AnimatedContentControl);
                    break;
                case "Search":
                    ViewManager.ShowMainInfoFlyout("Dieser Bereich befindet sich noch in Entwicklung", false);
                    break;
                case "BE":
                    ViewManager.ShowUnderPageOn<MontagsPostBE>(AnimatedContentControl);
                    break;
                case "BGHR_Zivil":
                    ViewManager.ShowUnderPageOn<MontagsPostBGHRView>(AnimatedContentControl);
                    break;
                case "Settings":
                    ViewManager.ShowUnderPageOn<MontagsPostSettingsView>(AnimatedContentControl);
                    break;
            }

        }
    }
}
