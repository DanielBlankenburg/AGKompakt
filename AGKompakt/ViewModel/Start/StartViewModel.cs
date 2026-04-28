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

        private bool _ShowBereitschaftsdienste = false;
        public bool ShowBereitschaftsdienste
        {
            get { return _ShowBereitschaftsdienste; }
            set { SetProperty(ref _ShowBereitschaftsdienste, value); }
        }
        private bool _ShowFamilie = true;
        public bool ShowFamilie
        {
            get { return _ShowFamilie; }
            set { SetProperty(ref _ShowFamilie, value); }
        }
        private bool _ShowZivil = true;
        public bool ShowZivil
        {
            get { return _ShowZivil; }
            set { SetProperty(ref _ShowZivil, value); }
        }
        private bool _ShowInsO = true;
        public bool ShowInsO
        {
            get { return _ShowInsO; }
            set { SetProperty(ref _ShowInsO, value); }
        }

        #endregion

        public StartViewModel()
        {
            if (UserManager.RegistratedUser != null)
            {

            }
        }
    }
}
