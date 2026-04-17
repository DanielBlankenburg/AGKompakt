using BGH_Kompakt.Enums;
using BGH_Kompakt.Services.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.ViewModel.Start
{
    public class StartViewModel : ViewModelBase
    {
        public string Title { get; set; } = string.Empty;
        public string SenatsText { get; set; } = string.Empty;

        #region Show

        private bool _ShowSitzungsunterlagen = false;
        public bool ShowSitzungsunterlagen
        {
            get { return _ShowSitzungsunterlagen; }
            set { SetProperty(ref _ShowSitzungsunterlagen, value); }
        }
        private bool _ShowSpruchgruppen = false;
        public bool ShowSpruchgruppen
        {
            get { return _ShowSpruchgruppen; }
            set { SetProperty(ref _ShowSpruchgruppen, value); }
        }
        private bool _ShowNebentaetigkeiten = false;
        public bool ShowNebentaetigkeiten
        {
            get { return _ShowNebentaetigkeiten; }
            set { SetProperty(ref _ShowNebentaetigkeiten, value); }
        }
        private bool _ShowSitzungsplaene = false;
        public bool ShowSitzungsplaene
        {
            get { return _ShowSitzungsplaene; }
            set { SetProperty(ref _ShowSitzungsplaene, value); }
        }
        private bool _ShowKanzlei = false;
        public bool ShowKanzlei
        {
            get { return _ShowKanzlei; }
            set { SetProperty(ref _ShowKanzlei, value); }
        }
        private bool _ShowMontagspost = false;
        public bool ShowMontagspost
        {
            get { return _ShowMontagspost; }
            set { SetProperty(ref _ShowMontagspost, value); }
        }
        private bool _ShowMontagspostAdmin = false;
        public bool ShowMontagspostAdmin
        {
            get { return _ShowMontagspostAdmin; }
            set { SetProperty(ref _ShowMontagspostAdmin, value); }
        }

        private bool _ShowAnwaltsauswahl = false;
        public bool ShowAnwaltsauswahl
        {
            get { return _ShowAnwaltsauswahl; }
            set { SetProperty(ref _ShowAnwaltsauswahl, value); }
        }
        #endregion

        public StartViewModel()
        {
            Title = (UserManager.SenatSettings.Senat != null) ? UserManager.SenatSettings.Senat.SenatName : "kein Senat ausgewählt";
            if (UserManager.RegistratedUser != null)
            {
                ShowSitzungsunterlagen = UserManager.RegistratedUser.ShowSitzungsunterlagen;
                if (UserManager.SenatSettings.Senat != null)
                    SenatsText = UserManager.SenatSettings.Senat.SenatArt == 2 ? "Senatshefte" : "Sitzungsunterlagen";
                ShowNebentaetigkeiten = UserManager.RegistratedUser.ShowActivityRequests;
                ShowMontagspost = UserManager.RegistratedUser.ShowMontagspost;
                ShowMontagspostAdmin = UserManager.RegistratedUser.ShowMontagspostAdmin;
                ShowSitzungsplaene = UserManager.SenatSettings.ShowSitzungsplaene;
                ShowSpruchgruppen = UserManager.SenatSettings.ShowSpruchgruppen;
                ShowKanzlei = false;
                ShowAnwaltsauswahl = false;
            }
        }
    }
}
