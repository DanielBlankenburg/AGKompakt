using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Enums;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.Views.SystemSettingsView;
using System.Windows;
using System.Windows.Controls;

namespace BGH_Kompakt.Views.Settings
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewManager.ShowUnderPageOn<SettingViewStart>(AnimatedContentControl);
            Grid_Settings.Visibility = Visibility.Collapsed;
        }

        //private void Btn_Open_Richterliste_Click(object sender, RoutedEventArgs e)
        //{
        //    ViewManager.ShowUnderPageOn<PersonChangeView>(AnimatedContentControl);
        //    Grid_Settings.Visibility = Visibility.Visible;

        //}

        private void Btn_Open_Senatseinstellungen_Click(object sender, RoutedEventArgs e)
        {

            ViewManager.ShowUnderPageOn<SystemSettingView>(AnimatedContentControl);
            Grid_Settings.Visibility = Visibility.Visible;
        }

        private void Btn_Open_UserVerwaltung_Click(object sender, RoutedEventArgs e)
        {
            bool admin = false;
            if (UserManager.RegistratedUser.AdminStatus != null)
            {
                foreach (AdminStatus Status in UserManager.RegistratedUser.AdminStatus) if (Status.AdminStatusText == UserEnums.EnumAdminStatus.Programm.ToString()) admin = true;
            }

            if (admin)
            {
                ViewManager.ShowUnderPageOn<AdminView>(ViewManager.SettingView.AnimatedContentControl);
                ViewManager.SettingView.Grid_Settings.Visibility = Visibility.Visible;
            }
            else
            {
                string Password = ViewManager.ShowPasswordWindow("Bitte geben Sie ein Passwort ein:");
                //InputBoxBGH Inputbox = new InputBoxBGH("Bitte geben Sie ein Passwort ein:", SettingEnums.BGHKompaktDialogType.Input);
                //if (Inputbox.ShowDialog() == true)
                //{
                if (Password == "BGHKompaktAdmin")
                {
                    ViewManager.ShowUnderPageOn<AdminView>(ViewManager.SettingView.AnimatedContentControl);
                    ViewManager.SettingView.Grid_Settings.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Sie sind nicht als Admin zugelassen und das Passwort ist inkorrekt. Sie können diesen Bereich nicht öffnen", "Unberechtigter Zugriff", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                //}
            }
        }
    }
}
