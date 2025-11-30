using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Montagspost
{
    public class MontagsPostEMailsViewModel : ViewModelBase
    {
        public ObservableCollection<MPEMailRecipient> ListExterior { get; set; } = new ObservableCollection<MPEMailRecipient>();
        public ObservableCollection<MPEMailRecipient> ListInterior { get; set; } = new ObservableCollection<MPEMailRecipient>();

        private readonly MPDBContext mpDBContext = new MPDBContext();

        private string _EMailRecipientInterior = string.Empty;
        public string EMailRecipientInterior
        {
            get { return _EMailRecipientInterior; }
            set { SetProperty(ref _EMailRecipientInterior, value); }
        }

        private string _EMailRecipientExterior = string.Empty;
        public string EMailRecipientExterior
        {
            get { return _EMailRecipientExterior; }
            set { SetProperty(ref _EMailRecipientExterior, value); }
        }

        private MPEMailRecipient _SelectedInteriorEMailRecipient;
        public MPEMailRecipient SelectedInteriorEMailRecipient
        {
            get { return _SelectedInteriorEMailRecipient; }
            set { 
                EMailRecipientInterior = value.MPEMailRecipientAdress;
                SetProperty(ref _SelectedInteriorEMailRecipient, value); 
            }
        }

        private MPEMailRecipient _SelectedExteriorEMailRecipient;
        public MPEMailRecipient SelectedExteriorEMailRecipient
        {
            get { return _SelectedExteriorEMailRecipient; }
            set {
                EMailRecipientExterior = value.MPEMailRecipientAdress;
                SetProperty(ref _SelectedExteriorEMailRecipient, value); 
            }
        }

        public ICommand ListExteriorAddCommand { get; set; }
        public ICommand ListExteriorDeleteCommand { get; set; }
        public ICommand ListInteriorEditCommand { get; set; }

        public MontagsPostEMailsViewModel()
        {

            ListExteriorAddCommand = new RelayCommand(ListExteriorAddExecute, ListExteriorAddCanExecute);
            ListExteriorDeleteCommand = new RelayCommand(ListExteriorDeleteExecute, ListExteriorDeleteCanExecute);
            ListInteriorEditCommand = new RelayCommand(ListInteriorEditExecute, ListInteriorEditCanExecute);

            //ListExterior 
            var templist = mpDBContext.MPEMailRecipients.Where(x => x.MPEMailRecipientTyp == 1).ToArray();
            foreach (MPEMailRecipient recipient in templist) ListExterior.Add(recipient);

            //ListInterior
            UserDBContext context = new UserDBContext();
            var query = context.Users.Where(n => n.MPEMailNotification == true && n.StatusId == 1).ToArray();
            foreach (var item in query) ListInterior.Add(new MPEMailRecipient { MPEMailRecipientAdress = item.EMail, MPEMailRecipientTyp = 2, MPEMailUser = item});

        }

        private bool ListExteriorDeleteCanExecute(object obj)
        {
            return SelectedExteriorEMailRecipient != null;
        }

        private void ListExteriorDeleteExecute(object obj)
        {
            throw new NotImplementedException();
        }

        private bool ListInteriorEditCanExecute(object obj)
        {
            return SelectedInteriorEMailRecipient != null;
        }

        private void ListInteriorEditExecute(object obj)
        {
            if (SelectedInteriorEMailRecipient == null);
            ViewManager.ShowMainInfoFlyout("Bitte wählen Sie eine E-Mail-Adresse aus.", false);
        }

        private bool ListExteriorAddCanExecute(object obj)
        {
            return EMailRecipientExterior != string.Empty;
        }

        private void ListExteriorAddExecute(object obj)
        {
            if (EMailRecipientExterior == string.Empty)
                ViewManager.ShowMainInfoFlyout("Bitte tragen Sie eine E-Mail-Adresse ein.", false);
            MPEMailRecipient recipient = new MPEMailRecipient(EMailRecipientExterior, 2);
            mpDBContext.MPEMailRecipients.Add(recipient);
            mpDBContext.SaveChanges();
            ListExterior.Add(recipient);
            EMailRecipientExterior = string.Empty;

        }
    }
}
