using BGH_Kompakt.Classes.ActivityRequestClasses;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Classes.SystemSettings;
using BGH_Kompakt.Components;
using BGH_Kompakt.Enums;
using BGH_Kompakt.Services.ActivityRequestService;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.ViewModel.MainWindow;
using BGH_Kompakt.Views;
using BGH_Kompakt.Views.HelperWindows;
using BGH_Kompakt.Views.Pages.Montagspost;
using BGH_Kompakt.Views.Pages.Sitzungsunterlagen;
using BGH_Kompakt.Views.Pages.Start;
using BGH_Kompakt.Views.Settings;
using BGH_Kompakt.Views.Sitzungsunterlagen;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;

namespace BGH_Kompakt.Services.SystemComponents
{
    public class ViewManager
    {
        public static MainWindow MainView { get; set; }
        public static MainWindowViewModel MainWindowViewModel { get; set; }
        public static SettingsView SettingView { get; set; }
        public static NebentaetigkeitenView NebentaetigkeitenView { get; set; }
        public static List<CompareBSCW> Filelist { get; set; } = new List<CompareBSCW>();
        public static ServiceProvider ServiceProvider { get; set; }
        //public static string PageName { get; set; }
        public static PageInfo PageInfo { get; set; } = new PageInfo();

        public static void InitViewManagerData(MainWindow iMainView, MainWindowViewModel iMainWindowViewModel, SettingsView iSettingView, NebentaetigkeitenView iNebentaetigkeitenView, ServiceProvider iSeriveProvider)
        {
            MainView = iMainView;
            MainWindowViewModel = iMainWindowViewModel;
            SettingView = iSettingView;
            NebentaetigkeitenView = iNebentaetigkeitenView;
            ServiceProvider = iSeriveProvider;
        }
        public static void ShowPageOnMainView<T>() where T : UserControl
        {
            try
            {
                var pageServices = ServiceProvider?.GetService<T>();
                if (pageServices is null) return;
                if (pageServices is UserControl page)
                {
                    Show(page);
                }
                PageInfo.TopPage = pageServices.Name;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public static void Show(UserControl iPage)
        {
            MainView.AnimatedContentControl.PagePlace.Content = null;
            MainView.AnimatedContentControl.PagePlace.Content = iPage;

            MainView.AnimatedContentControl.MetroTabItem.IsSelected = false;
            MainView.AnimatedContentControl.MetroTabItem.IsSelected = true;

        }
        public static void ShowUnderPageOn<T>(AnimatedContentControl iAnimatedContentControl) where T : UserControl
        {
            try
            {
                var pageServices = ServiceProvider?.GetService<T>();
                if (pageServices is null) return;
                if (pageServices is UserControl page)
                {
                    iAnimatedContentControl.PagePlace.Content = null;
                    iAnimatedContentControl.PagePlace.Content = page;

                    iAnimatedContentControl.MetroTabItem.IsSelected = false;
                    iAnimatedContentControl.MetroTabItem.IsSelected = true;
                }
                PageInfo.UnderPage = pageServices.Name;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public static void ShowMainInfoFlyout(string iMessage, bool iWarning)
        {
            MainView.LblFlyoutInfo.Text = iMessage;
            //MainView.LblFlyoutInfo.Foreground = (iWarning) ? Brushes.DarkRed : Brushes.White;
            MainView.InfoFlyout.IsOpen = true;
        }
        public static bool ShowInformationWindow(string iMessage)
        {
            return new InputBoxBGH(iMessage, SettingEnums.BGHKompaktDialogType.Information).ShowDialog() ?? false;
        }
        public static bool ShowQuestionWindow(string iMessage, string questionButton)
        {
            return new InputBoxBGH(iMessage, SettingEnums.BGHKompaktDialogType.Question, buttonContent: questionButton).ShowDialog() ?? false;
        }

        public static bool ShowDescriptionWindow(string iMessage, string button)
        {
            return new InputBoxBGH(iMessage, SettingEnums.BGHKompaktDialogType.Description, mainMenuButton: button).ShowDialog() ?? false;
        }
        public static string ShowInputWindow(string iMessage)
        {
            InputBoxBGH inputbox = new InputBoxBGH(iMessage, SettingEnums.BGHKompaktDialogType.Input);
            inputbox.ShowDialog();
            return inputbox.Inputtext;
        }
        public static string ShowPasswordWindow(string iMessage)
        {
            InputBoxBGH inputbox = new InputBoxBGH(iMessage, SettingEnums.BGHKompaktDialogType.Password);
            inputbox.ShowDialog();
            return inputbox.Inputtext;
        }
        public static bool ShowActivityRequestChangeHistory(ActivityRequest request)
        {
            //return new InputBoxBGH(iMessage, SettingEnums.BGHKompaktDialogType.Information).ShowDialog() ?? false;
            return false;
        }
        public static void OpenMainMenu(string button)
        {
            MainView.MainMenu.Visibility = Visibility.Visible;
            if (button == "BtnMainSitzungsunterlagen" || button == "BtnInfoSitzungsunterlagen")
            {
                if (UserManager.SenatSettings.Senat.SenatArt == 1) ViewManager.ShowPageOnMainView<SitzungsunterlagenView>();
                else if (UserManager.SenatSettings.Senat.SenatArt == 2) ViewManager.ShowPageOnMainView<SitzungsunterlagenStrafView>();
                else ViewManager.ShowMainInfoFlyout("Für diesen Senat steht dieser Bereich nicht zur Verfügung", false);


            }
            else if (button == "BtnMainSitzungsplaene")
            {
                List<SenatAktenzeichen> aktenzeichen = UserManager.SenatSettings.Aktenzeichen.ToList();
                List<SenatSpruchgruppe> spruchgruppen = UserManager.SenatSettings.Spruchgruppen.ToList();
                if (aktenzeichen.Count > 0 && spruchgruppen.Count > 0)
                {
                    ViewManager.ShowPageOnMainView<SitzungsplaeneView>();
                }
                else
                {
                    ViewManager.ShowPageOnMainView<SettingsView>();
                }
            }
            else if (button == "BtnMainSpruchgruppen")
            {
                ViewManager.ShowPageOnMainView<SpruchgruppenView>();
            }
            //else if (button == Btn_Main_Kanzleiunterlagen)
            //{
            //    MessageBox.Show("Dieser Bereich steht noch nicht zur Verfügung");
            //}
            else if (button == "Btn_Main_PersonChange")
            {
                ViewManager.ShowPageOnMainView<SettingsView>();
            }
            else if (button == "BtnMainMontagspost")
            {
                ViewManager.ShowPageOnMainView<MontagsPostView>();
            }
            else if (button == "BtnMontagspostAdmin")
            {
                ViewManager.ShowPageOnMainView<MontagsPostAdminView>();
            }
            //else if (button == Btn_Anwaltswahl)
            //{
            //    ViewManager.ShowPageOnMainView<AnwaltswahlView>();
            //}
            else if (button == "BtnNebentaetigkeiten")
            {
                ActivityRequestManager.LoginType = 1;
                ActivityRequestManager.ListArt = 1;
                ViewManager.ShowPageOnMainView<NebentaetigkeitenView>();
            }
            else if (button == "BtnNebentaetigkeitenAdmin")
            {
                ViewManager.ShowPageOnMainView<NebentaetigkeitenView>();
            }
            else if (button == "Btn_Main_Instruction")
            {
                ViewManager.ShowPageOnMainView<InstructionsView>();
            }
        }



        #region Actionlist
        public static void ActionlistAdd(string actionstatusname)
        {
            MainWindowViewModel.ActionList.Add(new ActionStatus { ActionsStatusName = actionstatusname });
            MainWindowViewModel.ShowStatusBar = true;
        }
        public static void ActionlistRemove(string actionstatusname)
        {
            ActionStatus removeActionsstatus = MainWindowViewModel.ActionList.Where(x => x.ActionsStatusName == actionstatusname).FirstOrDefault();
            if (removeActionsstatus != null) MainWindowViewModel.ActionList.Remove(removeActionsstatus);
            if (MainWindowViewModel.ActionList.Count == 0) MainWindowViewModel.ShowStatusBar = false;
        }

        #endregion
    }
}
