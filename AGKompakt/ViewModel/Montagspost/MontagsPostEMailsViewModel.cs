using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Classes.UserClasses;
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
                SetProperty(ref _SelectedInteriorEMailRecipient, value); 
                if (value != null) EMailRecipientInterior = value.MPEMailRecipientAdress;
            }
        }

        private MPEMailRecipient _SelectedExteriorEMailRecipient;
        public MPEMailRecipient SelectedExteriorEMailRecipient
        {
            get { return _SelectedExteriorEMailRecipient; }
            set {
                SetProperty(ref _SelectedExteriorEMailRecipient, value); 
                if (value != null) EMailRecipientExterior = value.MPEMailRecipientAdress;
            }
        }

        private readonly UserDBContext userDBContext = new UserDBContext();

        public ICommand ListExteriorAddCommand { get; set; }
        public ICommand ListExteriorDeleteCommand { get; set; }
        public ICommand ListInteriorEditCommand { get; set; }

        public MontagsPostEMailsViewModel()
        {

            ListExteriorAddCommand = new RelayCommand(ListExteriorAddExecute, ListExteriorAddCanExecute);
            ListExteriorDeleteCommand = new RelayCommand(ListExteriorDeleteExecute, ListExteriorDeleteCanExecute);
            ListInteriorEditCommand = new RelayCommand(ListInteriorEditExecute, ListInteriorEditCanExecute);

            //ListExterior 
            var templist = mpDBContext.MPEMailRecipients.Where(x => x.MPEMailRecipientTyp == 2).ToArray();
            foreach (MPEMailRecipient recipient in templist) ListExterior.Add(recipient);

            //ListInterior
            var query = userDBContext.Users.Where(n => n.MPEMailNotification == true && n.StatusId == 1).ToArray();
            foreach (var item in query) ListInterior.Add(new MPEMailRecipient { MPEMailRecipientAdress = item.EMail, MPEMailRecipientTyp = 2, MPEMailUserID = item.UserId, MPEMailUserFullName = item.Fullname});

        }

        private bool ListExteriorDeleteCanExecute(object obj)
        {
            return SelectedExteriorEMailRecipient != null;
        }

        private void ListExteriorDeleteExecute(object obj)
        {
            if (SelectedExteriorEMailRecipient == null)
                ViewManager.ShowMainInfoFlyout("Bitte wählen Sie eine E-Mail-Adresse aus.", false); 
            MPEMailRecipient recipient = mpDBContext.MPEMailRecipients.FirstOrDefault(x => x.MPEMailRecipientID == SelectedExteriorEMailRecipient.MPEMailRecipientID);
            if (recipient != null)
            {
                mpDBContext.MPEMailRecipients.Remove(recipient);
                mpDBContext.SaveChanges();
                ListExterior.Remove(SelectedExteriorEMailRecipient);
            }
        }

        private bool ListInteriorEditCanExecute(object obj)
        {
            return SelectedInteriorEMailRecipient != null;
        }

        private void ListInteriorEditExecute(object obj)
        {
            if (SelectedInteriorEMailRecipient == null)
            ViewManager.ShowMainInfoFlyout("Bitte wählen Sie einen E-Mail-Empfänger aus.", false);

            User editUser = userDBContext.Users.FirstOrDefault(x => x.UserId == SelectedInteriorEMailRecipient.MPEMailUserID);
            if (editUser != null)
            {
                editUser.EMail = EMailRecipientInterior;
                userDBContext.SaveChanges();
                ListInterior.Remove(SelectedInteriorEMailRecipient);
                ListInterior.Add(new MPEMailRecipient { MPEMailRecipientAdress = EMailRecipientInterior, MPEMailRecipientTyp = 2, MPEMailUserID = editUser.UserId, MPEMailUserFullName = editUser.Fullname});

            }
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
