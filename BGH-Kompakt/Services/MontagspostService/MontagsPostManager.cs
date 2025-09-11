using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes.MP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Services.MontagspostService
{
    public static class MontagsPostManager
    {
        private static MPWeek savedWeek;
        public static MPWeek SavedWeek
        {
            get { return savedWeek; }
        }
        public static bool SavedWeekStatus = false;

        private static int savedVintage;
        public static int SavedVintage
        {
            get { return savedVintage; }
        }

        private static MPSenat savedSenat;
        public static MPSenat SavedSenat
        {
            get { return savedSenat; }
        }
        public static bool SavedSenatStatus = false;

        private static MPDecision savedDecision;
        public static MPDecision SavedDecision
        {
            get { return savedDecision; }
        }
        public static bool SavedDecisionStatus = false;
        public static bool RecoverStatus = false;   



        //public static MPWeek SelectedMPWeek { get; }
        //public static MPDecision SelectedDecision { get; }

        public static void SaveWeek(int SelectedVintage, MPWeek SelectedMPWeek)
        {
            savedVintage = SelectedVintage;
            savedWeek = SelectedMPWeek;
            SavedWeekStatus = true;
            SavedSenatStatus = false;
            SavedDecisionStatus = false;
            RecoverStatus = true;

        }

        public static void SaveSenat(int SelectedVintage, MPWeek SelectedMPWeek, MPSenat SelectedSenat)
        {
            savedVintage = SelectedVintage;
            savedWeek = SelectedMPWeek;
            savedSenat = SelectedSenat;
            SavedWeekStatus = false;
            SavedSenatStatus = true;
            SavedDecisionStatus = false;
            RecoverStatus = true;

        }

        public static void SaveDecision (int SelectedVintage, MPWeek SelectedMPWeek, MPSenat SelectedSenat, MPDecision SelectedDecision)
        {
            savedVintage = SelectedVintage;
            savedWeek = SelectedMPWeek;
            savedSenat = SelectedSenat;
            savedDecision = SelectedDecision;
            SavedWeekStatus = false;
            SavedSenatStatus = false;
            SavedDecisionStatus = true;
            RecoverStatus = true;

        }

    }
}
