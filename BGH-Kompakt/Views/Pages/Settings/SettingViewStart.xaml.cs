using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Enums;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.Views.Pages.Settings;
using BGH_Kompakt.Views.SystemSettingsView;
using System.Windows;
using System.Windows.Controls;

namespace BGH_Kompakt.Views.Settings
{
    /// <summary>
    /// Interaction logic for SettingViewStart.xaml
    /// </summary>
    public partial class SettingViewStart : UserControl
    {
        public SettingViewStart()
        {
            InitializeComponent();
        }

        private void OpenPageOnMain(object sender, RoutedEventArgs e)
        {
            //var mainView = _ServiceProvider.GetService<MainWindow>();

            if (sender is Button button)
            {

                //if (button == Btn_Richterliste)
                //{
                //    ViewManager.ShowUnderPageOn<PersonChangeView>(ViewManager.SettingView.AnimatedContentControl);
                //    ViewManager.SettingView.Grid_Settings.Visibility = Visibility.Visible;
                //}
                if (button == Btn_Senatseinstellungen)
                {
                    ViewManager.ShowUnderPageOn<SystemSettingView>(ViewManager.SettingView.AnimatedContentControl);
                    ViewManager.SettingView.Grid_Settings.Visibility = Visibility.Visible;
                }
                else if (button == Btn_Montagspost)
                {
                    bool adminMP = false;
                    if (UserManager.RegistratedUser.AdminStatus != null)
                    {
                        foreach (AdminStatus Status in UserManager.RegistratedUser.AdminStatus) if (Status.AdminStatusText == UserEnums.EnumAdminStatus.MontagspostAdmin.ToString()) adminMP = true;
                    }
                    if (adminMP)
                    {
                        ViewManager.ShowUnderPageOn<MPSettingsView>(ViewManager.SettingView.AnimatedContentControl);
                        ViewManager.SettingView.Grid_Settings.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ViewManager.ShowMainInfoFlyout("Sie sind nicht berechtigt, diesen Bereich zu öffnen", false);
                    }
                }
                else if (button == Btn_Admineinstellung)
                {
                    CheckAdmin(1);
                }
                else if (button == Btn_ProgrammSettings)
                {
                    CheckAdmin(2);
                }
                else if (button == Btn_Besoldung)
                {
                    CheckAdmin(3);
                }
                else if (button == Btn_ErrorArea)
                {
                    ViewManager.ShowUnderPageOn<ErrorAreaView>(ViewManager.SettingView.AnimatedContentControl);
                }


            }
        }

        private void CheckAdmin(int Art)
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
                if (Password == "BGHKompaktAdmin")
                {
                    switch (Art)
                    {
                        case 1:
                            ViewManager.ShowUnderPageOn<AdminView>(ViewManager.SettingView.AnimatedContentControl);
                            break;
                        case 2:
                            ViewManager.ShowUnderPageOn<ProgrammSettingsView>(ViewManager.SettingView.AnimatedContentControl);
                            break;
                        case 3:
                            ViewManager.ShowUnderPageOn<BesoldungVIew>(ViewManager.SettingView.AnimatedContentControl);
                            break;
                    }
                    ViewManager.SettingView.Grid_Settings.Visibility = Visibility.Visible;
                }
                else
                {
                    ViewManager.ShowMainInfoFlyout("Das Passwort ist inkorrekt. Sie können diesen Bereich nicht öffnen", false);
                }
            }
        }

    }
}
