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
        public static void CreateException (string errorLocation, string errorMessage, Exception errorInnerException)
        {
            Logger.WriteLog($"Es ist folgender Fehler aufgetreten: Funktion {errorLocation}; {errorMessage}; {errorInnerException}");
            ViewManager.ShowMainInfoFlyout($"Es ist folgender Fehler aufgetreten: Funktion {errorLocation}; {errorMessage}", false);
        }

        public static void CreateSimpleMessage(string message)
        {
            Logger.WriteLog(message);
            ViewManager.ShowMainInfoFlyout(message, false);
        }
    }
}
