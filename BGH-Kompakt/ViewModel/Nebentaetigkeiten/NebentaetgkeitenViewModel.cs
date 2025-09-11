using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Enums;
using BGH_Kompakt.Services.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.ViewModel.Nebentaetigkeiten
{
    public class NebentaetgkeitenViewModel : ViewModelBase
    {
        private bool _ShowAdmin = false;
        public bool ShowAdmin
        {
            get => _ShowAdmin;
            set { SetProperty<bool>(ref _ShowAdmin, value); }
        }

        private bool _ShowPraesidalbereich = false;
        public bool ShowPraesidalbereich
        {
            get => _ShowPraesidalbereich;
            set { SetProperty<bool>(ref _ShowPraesidalbereich, value); }
        }
        private bool _ShowNewRequest = false;
        public bool ShowNewRequest
        {
            get => _ShowNewRequest;
            set { SetProperty<bool>(ref _ShowNewRequest, value); }
        }

        //private bool _ShowPraesident = false;
        //public bool ShowPraesident
        //{
        //    get => _ShowPraesident;
        //    set { SetProperty<bool>(ref _ShowPraesident, value); }
        //}
        public NebentaetgkeitenViewModel()
        {
            ShowAdmin = UserManager.RegistratedUser != null && (UserManager.RegistratedUser.IsARVorzimmer ||UserManager.RegistratedUser.IsARPraesdialrichter || UserManager.RegistratedUser.IsARPraesident);
            ShowPraesidalbereich = UserManager.RegistratedUser != null && (UserManager.RegistratedUser.IsARVorzimmer || UserManager.RegistratedUser.IsARPraesdialrichter || UserManager.RegistratedUser.IsARPraesident || UserManager.RegistratedUser.IsVorsitzenderRichter);
            ShowNewRequest = UserManager.RegistratedUser != null && UserManager.RegistratedUser.IsARVorzimmer;
            //ShowPraesident = UserManager.RegistratedUser.IsARPraesident;


        }
    }
}
