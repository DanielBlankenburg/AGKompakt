using BGH_Kompakt.Classes.ActivityRequestClasses;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.Fillclasses;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.ViewModel;
using BGH_Kompakt.ViewModel.MainWindow;
using BGH_Kompakt.Views;
using BGH_Kompakt.Views.Pages.Montagspost;
using BGH_Kompakt.Views.Pages.Sitzungsunterlagen;
using BGH_Kompakt.Views.Pages.Start;
using BGH_Kompakt.Views.Pages.TestDesign;
using BGH_Kompakt.Views.Settings;
using BGH_Kompakt.Views.Sitzungsunterlagen;
using BGH_Kompakt.Views.Start;
using BGH_Kompakt.Views.UserLogin;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace BGH_Kompakt
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //private MainWindow mainview;

        //public ICommand StartCommand { get; set; }

        public MainWindowViewModel MainWindowViewModel { get; }

        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            MainWindowViewModel = mainWindowViewModel;
            if (mainWindowViewModel is ViewModelBase @base)
            {
                this.SizeChanged += @base.OnWindowSizeChanged;
                DataContext = mainWindowViewModel;
             }
            //MessageBox.Show(Environment.UserName);

        }


        public ServiceProvider ServiceProvider { get; internal set; }

        //private void InitEvents() => OpenPageOnMain(mainview, new RoutedEventArgs());

        public void OpenPageOnMain(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {

                if (button == btn_Main)
                    ViewManager.ShowPageOnMainView<StartView>();
                else if (button == btn_Sitzungsunterlagen )
                    ViewManager.ShowPageOnMainView<SitzungsunterlagenView>();
                else if (button == btn_Sitzungplaene)
                    ViewManager.ShowPageOnMainView<SitzungsplaeneView>();
                else if (button == btn_Spruchgruppen)
                    ViewManager.ShowPageOnMainView<SpruchgruppenView>();
                else if (button == btn_Settings)
                    ViewManager.ShowPageOnMainView<SettingsView>();
                else if (button == btn_Montagspost)
                    ViewManager.ShowPageOnMainView<MontagsPostView>();
                else if (button == btn_MontagspostAdmin)
                    ViewManager.ShowPageOnMainView<MontagsPostAdminView>();
                else if (button == btn_Nebentätigkeiten)
                    ViewManager.ShowPageOnMainView<NebentaetigkeitenView>();
                else if (button == btn_Instruction)
                    ViewManager.ShowPageOnMainView<InstructionsView>();
                else if (button == btn_Test)
                {
                    //TestWriteFile();
                    //TestReadFile();

                    ViewManager.ShowPageOnMainView<SitzungsunterlagenStrafView>();
                }
            }
        }

        private void Btn_Main_Instruction_Click(object sender, RoutedEventArgs e)
        {
            string fileName = BGHKompaktSystemInfo.PathLaufwerksbuchstabe + BGHKompaktSystemInfo.PathSystemdateien + "Anleitung BGH-Kompakt.pdf";
            try
            {
                Process.Start(new ProcessStartInfo(fileName)
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Die Anleitung konnte nicht geöffnet werden. Es ist folgender Fehler aufgetreten:" + Environment.NewLine + Environment.NewLine + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //List<User> UserList = new List<User>();
            //string Username = Environment.UserName;
            //UserDBContext dBContext = new UserDBContext();
            //DbSet<User> _dBSet = dBContext.Set<User>();
            //var UserListing = _dBSet.ToList<User>();

            //foreach (User user in UserListing)
            //{
            //    UserList.Add(user);
            //}

            //Seed SenatsSettings for each Senat
            try
            {
                UserFill.SenatSettingFill();

                //if (!BGHKompaktSystemInfo.ShowSitzungsplaene) btn_Sitzungplaene.Visibility = Visibility.Collapsed;
                //if (!BGHKompaktSystemInfo.ShowKanzlei) btn_Kanzleiunterlagen.Visibility = Visibility.Collapsed;
                //if (!BGHKompaktSystemInfo.ShowSpruchgruppen) btn_Spruchgruppen.Visibility = Visibility.Collapsed;

                if (UserManager.RegistratedUser != null)
                {
                    MainMenu.Visibility = Visibility.Collapsed;
                    ViewManager.ShowPageOnMainView<StartView>();
                }
                else
                {
                    MainMenu.Visibility = Visibility.Collapsed;
                    ViewManager.ShowPageOnMainView<UserLoginView>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Es ist folgender Fehler beim Laden der Anwendung (Mainwindow) aufgetreten. Bitte wenden Sie sich an den Administrator. \n\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.WriteLog($"logTime: {DateTime.Now}; Fehler beim Laden der Anwendung (Mainwindow): {ex.Message}");
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void Btn_UserLogin_Click(object sender, RoutedEventArgs e)
        {
            ViewManager.PageInfo.UserSettingType = 0;
            ViewManager.ShowPageOnMainView<UserPropertyView>();
        }

        private void UserSwitch_Click(object sender, RoutedEventArgs e)
        {
            UserSwitchWindow userSwitchWindow = new UserSwitchWindow();
            userSwitchWindow.Show();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            ViewManager.ShowPageOnMainView<StartTest>();
        }

        private void TestWriteFile()
        {
            string pdfFilePath = "C:/Montagspost/Kanzleipool/wkw29/1_StR_145-24.pdf";
            byte[] bytes = System.IO.File.ReadAllBytes(pdfFilePath);
            ActivityRequestDataFile file = new ActivityRequestDataFile { FileName = "1_StR_145-24.pdf", Data = bytes, ActivityRequestId = 1 };
            ActivityRequestDBContext context = new ActivityRequestDBContext();
            context.ActivityRequestDataFiles.Add(file);
            context.SaveChanges();

        }

        private void TestReadFile()
        {
            ActivityRequestDBContext context = new ActivityRequestDBContext();
            ActivityRequestDataFile readFile = context.ActivityRequestDataFiles.FirstOrDefault(x => x.ActivityRequestId == 1);

            string[] directories = Assembly.GetExecutingAssembly().Location.Split('\\');
            string pathApp = string.Empty;
            for (int i = 0; i < directories.Length - 1; i++) pathApp += directories[i] + "\\";

            string tempDir = $"{pathApp}temp\\";
            if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);

            System.IO.File.WriteAllBytes($"{tempDir}{readFile.FileName}.pdf", readFile.Data);

            Process.Start(new ProcessStartInfo($"{tempDir}{readFile.FileName}.pdf")
            {
                UseShellExecute = true
            });

            MessageBox.Show("Fertig");
        }
    }
}
