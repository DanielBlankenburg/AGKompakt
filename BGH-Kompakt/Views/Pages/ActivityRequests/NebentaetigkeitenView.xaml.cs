using BGH_Kompakt.Services.ActivityRequestService;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.ViewModel.Nebentaetigkeiten;
using BGH_Kompakt.Views.Pages.ActivityRequests;
using BGH_Kompakt.Views.Settings;
using System.Windows;
using System.Windows.Controls;

namespace BGH_Kompakt.Views
{
    /// <summary>
    /// Interaction logic for NebentaetigkeitenView.xaml
    /// </summary>
    public partial class NebentaetigkeitenView : UserControl
    {
        public NebentaetgkeitenViewModel NebentaetgkeitenViewModel { get; }

        public NebentaetigkeitenView(NebentaetgkeitenViewModel nebentaetgkeitenViewModel)
        {
            InitializeComponent();
            NebentaetgkeitenViewModel = nebentaetgkeitenViewModel;
            DataContext = NebentaetgkeitenViewModel;
        }




        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewManager.ShowUnderPageOn<NebentaetigkeitenListView>(AnimatedContentControl);
            //Grid_Options.Visibility = Visibility.Collapsed;
        }

        private void Btn_Open_Richterliste_Click(object sender, RoutedEventArgs e)
        {
            //ViewManager.ShowUnderPageOn<PersonChangeView>(AnimatedContentControl);
            //Grid_Options.Visibility = Visibility.Visible;

        }

        private void Btn_Open_Senatseinstellungen_Click(object sender, RoutedEventArgs e)
        {
            ViewManager.ShowUnderPageOn<SystemSettingView>(AnimatedContentControl);
            //Grid_Options.Visibility = Visibility.Visible;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tabItem = ((sender as TabControl).SelectedItem as TabItem).Name as string;
            //Debug.WriteLine((sender as TabControl).Items.Count);

            //ActivityRequestManager.ActionType: 1 = Create; 2 = Edit
            //ActivityRequestManager.ListArt: 1 = Eigener Bereich; 2 = Übersicht offene Einträge; 3 = Archiv
            //ActivityRequestManager.LoginType: 1 = Own; 2 = Admin
            switch (tabItem)
            {
                case "Overview":
                    if (ActivityRequestManager.DirectJump == false)
                    {
                        ActivityRequestManager.LoginType = 1;
                        ActivityRequestManager.ListArt = 1;
                        ActivityRequestManager.ActionType = 2;
                    }
                    ActivityRequestManager.DirectJump = false;
                    ViewManager.ShowUnderPageOn<NebentaetigkeitenListView>(AnimatedContentControl);
                    break;
                case "OverviewOpenRequests":
                    if (ActivityRequestManager.DirectJump == false)
                    {
                        ActivityRequestManager.LoginType = 2;
                        ActivityRequestManager.ListArt = 2;
                        ActivityRequestManager.ActionType = 2;
                    }
                    ActivityRequestManager.DirectJump = false;
                    ViewManager.ShowUnderPageOn<NebentaetigkeitenListView>(AnimatedContentControl);
                    break;
                case "ArchivRequests":
                    if (ActivityRequestManager.DirectJump == false)
                    {
                        ActivityRequestManager.LoginType = 2;
                        ActivityRequestManager.ListArt = 3;
                        ActivityRequestManager.ActionType = 2;
                    }
                    ActivityRequestManager.DirectJump = false;
                    ViewManager.ShowUnderPageOn<NebentaetigkeitenListView>(AnimatedContentControl);
                    break;
                case "RequestOwn":
                    if (ActivityRequestManager.DirectJump == false)
                    {
                        ActivityRequestManager.LoginType = 1;
                        ActivityRequestManager.ActionType = 1;
                        ActivityRequestManager.SelectedActivityRequest = null;
                    }
                    ActivityRequestManager.DirectJump = false;
                    ViewManager.ShowUnderPageOn<NebentaetigkeitenAnzeigeView>(AnimatedContentControl);
                    break;
                 case "Request":
                    if (ActivityRequestManager.DirectJump == false)
                    {
                        ActivityRequestManager.LoginType = 2;
                        ActivityRequestManager.ActionType = 1;
                        ActivityRequestManager.SelectedActivityRequest = null;
                    }
                    ActivityRequestManager.DirectJump = false;
                    ViewManager.ShowUnderPageOn<NebentaetigkeitenAnzeigeView>(AnimatedContentControl);
                    break;
                 case "ClientEditor":
                    ViewManager.ShowUnderPageOn<NebentaetigkeitenClientEditorView>(AnimatedContentControl);
                    break;
                 case "RBesoldung":
                    ViewManager.ShowUnderPageOn<ActivityRequestsRBesoldungView>(AnimatedContentControl);
                    break;
                 case "ReportMain":
                    ViewManager.ShowUnderPageOn<ActivityRequestsReportView>(AnimatedContentControl);
                    break;
                 case "Search":
                    ViewManager.ShowUnderPageOn<ActivityRequestsReportView>(AnimatedContentControl);
                    break;
            }
            //Debug.WriteLine(tabItem);
            //ViewManager.ShowUnderPageOn<NebentaetigkeitenAnzeigeView>(AnimatedContentControl);
        }


    }
}
