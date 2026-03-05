using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.ViewModel;
using BGH_Kompakt.ViewModel.MainWindow;
using BGH_Kompakt.ViewModel.Montagspost;
using BGH_Kompakt.ViewModel.Nebentaetigkeiten;
using BGH_Kompakt.ViewModel.Sitzungsunterlagen;
using BGH_Kompakt.ViewModel.Start;
using BGH_Kompakt.ViewModel.SystemSettings;
using BGH_Kompakt.ViewModel.Userlogin;
using BGH_Kompakt.Views;
using BGH_Kompakt.Views.Montagspost;
using BGH_Kompakt.Views.Pages.ActivityRequests;
using BGH_Kompakt.Views.Pages.Montagspost;
using BGH_Kompakt.Views.Pages.Settings;
using BGH_Kompakt.Views.Pages.Sitzungsunterlagen;
using BGH_Kompakt.Views.Pages.Start;
using BGH_Kompakt.Views.Pages.TestDesign;
using BGH_Kompakt.Views.Settings;
using BGH_Kompakt.Views.Sitzungsunterlagen;
using BGH_Kompakt.Views.Start;
using BGH_Kompakt.Views.SystemSettingsView;
using BGH_Kompakt.Views.UserLogin;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace BGH_Kompakt
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _ServiceProvider;

        public App()
        {
            Logger.WriteLog($"logTime: {DateTime.Now}; Starting App");
            try
            {
                ServiceCollection _serviceCollection = new ServiceCollection();
                ConfigureSerive(_serviceCollection);
                _ServiceProvider = _serviceCollection.BuildServiceProvider();
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"Die App konnte nicht gestartet werden. Es ist folgender Fehler aufgetreten: {ex.Message}");
                throw;
            }
            //Aktivierung bei jedem erneuten Aufspielen der Datenbank MP erforderlich
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            //MainWindow service = _ServiceProvider.GetService<MainWindow>();
            //if (service == null)
            //{
            //    int num = (int)MessageBox.Show("Problem bei der Initialisierung SeriveProvider");
            //    Current.Shutdown();
            //}
            //service.ServiceProvider = _ServiceProvider;
            //service.Show();
            Logger.WriteLog($"logTime: {DateTime.Now}; Loading Service-Provider");
            try
            {
                var mainView = _ServiceProvider.GetService<MainWindow>();
                var mainViewModel = _ServiceProvider.GetService<MainWindowViewModel>();
                var settingView = _ServiceProvider.GetService<SettingsView>();
                var nebentaetigkeiten = _ServiceProvider.GetService<NebentaetigkeitenView>();

                if (mainView is null || settingView is null || nebentaetigkeiten is null)
                {
                    MessageBox.Show("Problem bei Initialisierung des Serviceproviders", "Fehler", MessageBoxButton.OK,MessageBoxImage.Error);
                    Current.Shutdown();
                }

                ViewManager.InitViewManagerData(mainView, mainViewModel, settingView, nebentaetigkeiten, _ServiceProvider);
                mainView.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Die Verbindung zur Datenbank konnte nicht hergestellt werden. Das Programm wird geschlossen. Bitte wenden Sie sich an den Administrator.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Threading.Thread.Sleep(1000);
                Logger.WriteLog($"logTime: {DateTime.Now}; Die Verbindung zur Datenbank konnte nicht hergestellt werden. Es ist folgender Fehler aufgetreten: {ex.Message}");
            }
            base.OnStartup(e);
        }

        private void ConfigureSerive(ServiceCollection iServiceColletion)
        {

            //Register Views and ViewModels
            iServiceColletion.AddSingleton<MainWindow>();
            iServiceColletion.AddSingleton<MainWindowViewModel>();

            iServiceColletion.AddTransient<SitzungsunterlagenView>();
            iServiceColletion.AddTransient<SitzungsunterlagenViewModel>();
            iServiceColletion.AddTransient<SitzungsunterlagenStrafView>();
            iServiceColletion.AddTransient<SitzungsunterlagenStrafViewModel>();


            iServiceColletion.AddSingleton<SitzungsplaeneView>();
            iServiceColletion.AddSingleton<SpruchgruppenView>();
            iServiceColletion.AddSingleton<Kanzleiunterlagen>();
            //iServiceColletion.AddTransient<PersonChangeView>();
            iServiceColletion.AddSingleton<SettingsView>();

            iServiceColletion.AddTransient<StartView>();
            iServiceColletion.AddTransient<StartViewModel>();

            iServiceColletion.AddTransient<VotenMappeView>();
            iServiceColletion.AddTransient<VotenMappeViewModel>();

            iServiceColletion.AddSingleton<SettingViewStart>();

            iServiceColletion.AddSingleton<SystemSettingView >();
            iServiceColletion.AddSingleton<SystemSettingViewModel>();

            iServiceColletion.AddTransient<MontagsPostView>();
            iServiceColletion.AddTransient<MontagsPostViewModel>();

            iServiceColletion.AddTransient<MontagsPostBE>();
            iServiceColletion.AddTransient<MontagsPostBEViewModel>();

            iServiceColletion.AddTransient<MontagsPostVermerkView>();
            iServiceColletion.AddTransient<MontagsPostVermerkViewModel>();

            iServiceColletion.AddSingleton<MontagsPostFilterView>();
            iServiceColletion.AddSingleton<MontagsPostFilterViewModel>();

            iServiceColletion.AddSingleton<AnwaltswahlView>();

            iServiceColletion.AddSingleton<NebentaetigkeitenView>();
            iServiceColletion.AddSingleton<NebentaetgkeitenViewModel>();
            iServiceColletion.AddTransient<NebentaetigkeitenAnzeigeView>();
            iServiceColletion.AddTransient<NebentaetigkeitenAnzeigeViewModel>();

            //iServiceColletion.AddSingleton<ActivityRequestInputView>();
            //iServiceColletion.AddSingleton<ActivityRequestInputViewModel>();
            iServiceColletion.AddTransient<NebentaetigkeitenListView>();
            iServiceColletion.AddTransient<NebentaetigkeitenListViewModel>();

            iServiceColletion.AddTransient<NebentaetigkeitenListViewModel>();

            iServiceColletion.AddSingleton<UserLoginView>();
            iServiceColletion.AddSingleton<UserLoginViewModel>();
            iServiceColletion.AddTransient<UserPropertyView>();
            iServiceColletion.AddTransient<UserPropertyViewModel>();

            iServiceColletion.AddTransient<AdminView>();
            iServiceColletion.AddTransient<AdminViewModel>();

            //iServiceColletion.AddSingleton<TestDesignView>();
            iServiceColletion.AddSingleton<UserSwitchWindow>();

            iServiceColletion.AddSingleton<MontagspostImportView>();
            iServiceColletion.AddSingleton<MontagspostImportViewModel>();

            iServiceColletion.AddSingleton<ProgrammSettingsView>();
            iServiceColletion.AddSingleton<ProgrammSettingViewModel>();

            iServiceColletion.AddSingleton<BSCWServerView>();
            iServiceColletion.AddSingleton<BSCWServerViewModel>();

            iServiceColletion.AddSingleton<MPSettingsView>();
            iServiceColletion.AddSingleton<MPSettingsViewModel>();

            iServiceColletion.AddSingleton<MontagsPostAdminView>();


            iServiceColletion.AddSingleton<StartTest>();

            iServiceColletion.AddSingleton<InstructionsView>();
            iServiceColletion.AddSingleton<ErrorAreaView>();
            iServiceColletion.AddSingleton<ErrorAreaViewModel>();

            iServiceColletion.AddTransient<NebentaetigkeitenClientEditorView>();
            iServiceColletion.AddTransient<NebentaetigkeitenClientEditorViewModel>();

            iServiceColletion.AddTransient<MontagsPostBGHRView>();
            iServiceColletion.AddTransient<MontagsPostBGHRViewModel>();
            iServiceColletion.AddTransient<MontagsPostEMailsView>();
            iServiceColletion.AddTransient<MontagsPostEMailsViewModel>();
            iServiceColletion.AddTransient<MontagsPostMetadataEditView>();
            iServiceColletion.AddTransient<MontagsPostMetadataEditViewModel>();
            iServiceColletion.AddTransient<MontagsPostSettingsView>();
            iServiceColletion.AddTransient<MontagsPostSettingsViewModel>();
            iServiceColletion.AddTransient<MontagsPostEditorView>();
            iServiceColletion.AddTransient<MontagsPostEditorViewModel>();
            iServiceColletion.AddTransient<BesoldungVIew>();
            iServiceColletion.AddTransient<BesoldungVIewModel>();




            //iServiceColletion.AddTransient<AnwaltswahlView>();
        }
    }


}
