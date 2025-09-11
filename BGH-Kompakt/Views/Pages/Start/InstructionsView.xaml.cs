using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace BGH_Kompakt.Views.Pages.Start
{
    /// <summary>
    /// Interaction logic for InstructionsView.xaml
    /// </summary>
    public partial class InstructionsView : UserControl
    {
        public InstructionsView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string fileName = string.Empty;
                switch (button.Name)
                {
                    case "Allgemein":
                        fileName = "Anleitung BGHKompaktSitzungsunterlagen.pdf";
                        break;
                    case "Montagspost":
                        fileName = "Anleitung BGHKompaktMontagspost.pdf";
                        break;
                    case "Nebentätigkeiten":
                        fileName = "Anleitung BGHKompaktNebentaetigkeiten.pdf";
                        break;
                    default:
                        break;
                }
                
                string[] directories = Assembly.GetExecutingAssembly().Location.Split('\\');
                string pathApp = string.Empty;
                for (int i = 0; i < directories.Length - 1; i++) pathApp += directories[i] + "\\";
                string PathInstruction = $"{pathApp}Documents\\";

                string PathName = PathInstruction + fileName;
                try
                {
                    Process.Start(new ProcessStartInfo(PathName) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    ViewManager.ShowMainInfoFlyout($"Die Anleitung konnte nicht geöffnet werden. Es ist folgender Fehler aufgetreten:\n{ex.Message}", false);
                }
            }



        }
    }
}
