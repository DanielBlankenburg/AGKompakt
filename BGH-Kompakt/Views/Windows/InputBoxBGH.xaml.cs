using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Windows;
using System.Windows.Input;
using static BGH_Kompakt.Enums.SettingEnums;

namespace BGH_Kompakt.Views.HelperWindows
{
    public partial class InputBoxBGH : Window
    {
        private BGHKompaktDialogType DialogType { get; set; } = new BGHKompaktDialogType();
        private string SelectedMainMenuButton { get; set; } = string.Empty;
        public string Inputtext { get; set; } = string.Empty;
        public InputBoxBGH(string question, BGHKompaktDialogType iDialogType, string title = "", string defaultAnswer = "", string mainMenuButton = "", string buttonContent = "")
        {
            InitializeComponent();
            lblQuestion.Content = question;
            TxtAnswer.Text = defaultAnswer;
            DialogType = iDialogType;
            SelectedMainMenuButton = mainMenuButton;

            switch (DialogType)
            {
                case BGHKompaktDialogType.Confirmation:
                case BGHKompaktDialogType.Question:
                    Title.Content = title != string.Empty ? title : "Bestätigung";
                    TxtAnswer.Visibility = Visibility.Collapsed;
                    BtnDialogOk.Content = (DialogType == BGHKompaktDialogType.Information) ? "_OK" : buttonContent;
                    BtnCancel.Content = "_Nein";
                    BGHPasswordBox.Visibility = Visibility.Collapsed;
                    BtnDialogOpen.Visibility = Visibility.Collapsed;
                    BtnDialogOk.Focus();
                    break;
                case BGHKompaktDialogType.Information:
                    Title.Content = title != string.Empty ? title : "Information";
                    BtnCancel.Content = "_Abbrechen";
                    TxtAnswer.Visibility = Visibility.Collapsed;
                    BGHPasswordBox.Visibility = Visibility.Collapsed;
                    BtnDialogOpen.Visibility = Visibility.Collapsed;
                    BtnDialogOk.Focus();
                    break;
                case BGHKompaktDialogType.Description:
                    Title.Content = title != string.Empty ? title : "Information";
                    BtnCancel.Content = "_Abbrechen";
                    TxtAnswer.Visibility = Visibility.Collapsed;
                    BGHPasswordBox.Visibility = Visibility.Collapsed;
                    BtnDialogOpen.Visibility = Visibility.Visible;
                    BtnDialogOk.Focus();
                    break;
                case BGHKompaktDialogType.Input:
                    Title.Content = title != string.Empty ? title : "Eingabe";
                    BtnCancel.Content = "_Abbrechen";
                    TxtAnswer.Visibility = Visibility.Visible;
                    BGHPasswordBox.Visibility = Visibility.Collapsed;
                    BtnDialogOpen.Visibility = Visibility.Collapsed;
                    TxtAnswer.Focus();
                    break;
                case BGHKompaktDialogType.Password:
                    Title.Content = title != string.Empty ? title : "Passworteingabe";
                    BtnCancel.Content = "_Abbrechen";
                    TxtAnswer.Visibility = Visibility.Collapsed;
                    BGHPasswordBox.Visibility = Visibility.Visible;
                    BtnDialogOpen.Visibility = Visibility.Collapsed;
                    BGHPasswordBox.Focus();
                    break;
            }
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            if (DialogType == BGHKompaktDialogType.Input) Inputtext = TxtAnswer.Text;
            if (DialogType == BGHKompaktDialogType.Password) Inputtext = BGHPasswordBox.Password;
            Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //TxtAnswer.SelectAll();
            //TxtAnswer.Focus();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnDialogOpen_Click(object sender, RoutedEventArgs e)
        {
            ViewManager.OpenMainMenu(SelectedMainMenuButton);
            Close();
        }
    }
}
