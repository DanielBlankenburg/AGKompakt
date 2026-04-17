using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.Helper
{
    public static class ErrorMessage
    {
        public static void CreateExceptionWithoutMessage(string errorLocation, Exception ex)
        {
            Logger.WriteLog($"Es ist folgender Fehler aufgetreten: Funktion {errorLocation}; {ex.Message}; {ex.InnerException}; {ex.StackTrace}");
        }


        public static void CreateExceptionWithFlyOutMessage (string errorLocation, Exception ex)
        {
            Logger.WriteLog($"Es ist folgender Fehler aufgetreten: Funktion {errorLocation}; {ex.Message}; {ex.InnerException}; {ex.StackTrace}");
            ViewManager.ShowMainInfoFlyout($"Es ist folgender Fehler aufgetreten: Funktion {errorLocation}; {ex.Message}", false);
        }

        public static void CreateSimpleMessage(string message)
        {
            Logger.WriteLog(message);
            ViewManager.ShowMainInfoFlyout(message, false);
        }
    }
}
