using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.SystemSettings
{
    public class ErrorAreaViewModel
    {
        public ICommand OpenLogFileCommand { get; set; }
        public ICommand SendLogFileCommand { get; set; }
        public ICommand OpenLogDirectoryCommand { get; set; }

        public ErrorAreaViewModel() 
        {
            OpenLogFileCommand = new RelayCommand(OpenLogFileExecute);
            SendLogFileCommand = new RelayCommand(SendLogFileExecute);
            OpenLogDirectoryCommand = new RelayCommand(OpenLogDirectoryExecute);

        }

        private void OpenLogDirectoryExecute(object obj)
        {
            try
            {
                Process.Start(new ProcessStartInfo(BGHKompaktSystemInfo.PathLoggingData) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Der Ordner für die Log-Files konnte nicht geöffnet werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
            }
        }

        private void SendLogFileExecute(object obj)
        {
            ViewManager.ShowMainInfoFlyout("Diese Funktion ist noch nicht implementiert.", false);
            //try
            //{
            //    Process.Start(new ProcessStartInfo($"BGHKompaktSystemInfo.PathLoggingData\\log_{UserManager.RegistratedUser.NachName}.txt") { UseShellExecute = true });
            //}
            //catch (Exception ex)
            //{
            //    ViewManager.ShowMainInfoFlyout($"Der Ordner für die Log-Files konnte nicht geöffnet werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
            //}
        }

        private void OpenLogFileExecute(object obj)
        {
            try { Process.Start(new ProcessStartInfo($"{BGHKompaktSystemInfo.PathLoggingData}log_{UserManager.RegistratedUser.NachName}.txt") { UseShellExecute = true });}
            catch (Exception ex) {ViewManager.ShowMainInfoFlyout($"Die Log-File konnte nicht geöffnet werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);}
        }
    }
}
