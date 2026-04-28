using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.ViewModel.Abteilungen.Familienabteilung;
using BGH_Kompakt.ViewModel.MainWindow;
using BGH_Kompakt.ViewModel.Start;
using BGH_Kompakt.ViewModel.Userlogin;
using BGH_Kompakt.Views.Pages.Abteilungen.Familienabteilung;
using BGH_Kompakt.Views.Pages.Start;
using BGH_Kompakt.Views.Start;
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
                //var settingView = _ServiceProvider.GetService<SettingsView>();
                //var nebentaetigkeiten = _ServiceProvider.GetService<NebentaetigkeitenView>();

                //if (mainView is null || settingView is null || nebentaetigkeiten is null)
                //{
                //    MessageBox.Show("Problem bei Initialisierung des Serviceproviders", "Fehler", MessageBoxButton.OK,MessageBoxImage.Error);
                //    Current.Shutdown();
                //}

                ViewManager.InitViewManagerData(mainView, mainViewModel, _ServiceProvider);
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


            iServiceColletion.AddTransient<StartView>();
            iServiceColletion.AddTransient<StartViewModel>();

            iServiceColletion.AddSingleton<UserLoginView>();
            iServiceColletion.AddSingleton<UserLoginViewModel>();
            iServiceColletion.AddTransient<UserPropertyView>();
            iServiceColletion.AddTransient<UserPropertyViewModel>();

            //iServiceColletion.AddSingleton<TestDesignView>();
            iServiceColletion.AddSingleton<UserSwitchWindow>();
            iServiceColletion.AddSingleton<InstructionsView>();

            #region Familie
            iServiceColletion.AddSingleton<FamilienabteilungMainView>();
            iServiceColletion.AddSingleton<FamilienabteilungMainViewModel>();
            iServiceColletion.AddSingleton<VerfahrensbeistaendeView>();
            iServiceColletion.AddSingleton<VerfahrensbeistaendeViewModel>();

            #endregion




            //iServiceColletion.AddTransient<AnwaltswahlView>();
        }
    }


}
