using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Montagspost
{
    public class MontagsPostBGHRViewModel : ViewModelBase
    {
        public ObservableCollection<MPDecisionBE> MPDecisionBEList { get; set; } = new ObservableCollection<MPDecisionBE>();
        public ObservableCollection<User> MPBEList { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<MPWeek> MPWeekList { get; set; } = new ObservableCollection<MPWeek>();
        private ObservableCollection<int> _VintageList = new ObservableCollection<int>();
        public ObservableCollection<int> VintageList { get { return _VintageList; } }
        public ObservableCollection<Senat> SenateList { get; set; } = new ObservableCollection<Senat>();
        public ObservableCollection<User> SenatBEList { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<MPBGHRSenat> SenateRecipientList { get; set; } = new ObservableCollection<MPBGHRSenat>();


        private readonly string MPEMailDescription = "BGHR-EMail";
        private readonly string DefaultSubject = "BGHRZ - Entscheidungen zur Bearbeitun";
        private readonly string DefaultBody = "Liebe Kollegin, lieber Kollege, <Br> <BR> anbei erhalten Sie die Entscheidungen Ihres Senats zur Bearbeitung für BGHRZ." +
                                    "Bitte füllen Sie das elektronische Erfassungsformular aus und senden es dann über den Senatsredaktor bzw. die Senatsredaktorin an. <BR> Eine Übersendung der Datei mit der Entscheidung ist nicht erforderlich. " +
                                    "Sollten Sie versehentlich zu Unrecht als Berichterstatter bzw. Berichterstatterin angemailt worden sein, leiten Sie bitte die Mail mit der Entscheidungsdatei an den richtigen Berichterstatter bzw. die richtige Berichterstatterin in Ihrem Senat weiter. " + 
                                    "Sollte es sich um eine Entscheidung eines anderen Senats handeln, leiten Sie die Mail an mich (woestmann-heinz@bgh.bund.de) mit  einem entsprechenden Hinweis weiter. Leider kann systembedingt eine falsche Zuordnung nicht vollständig ausgeschlossen werden." +
                                    "<BR> <BR> Vielen Dank und viele Grüße <BR> Ihr Heinz Wöstmann <BR> Geschäftsführer BGHR";
        private readonly string emailBody = string.Empty;
        private readonly MPDBContext mPDBContext = new MPDBContext();
        private readonly UserDBContext UserDbContext = new UserDBContext();


        public ICommand SendCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand BECommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand ResetEMailCommand { get; set; }
        public ICommand SaveEMailCommand { get; set; }
        public ICommand AddSenateEmpfaengerCommand { get; set; }
        public ICommand RemoveSenateEmpfaengerCommand { get; set; }

        private int _SelectedVintage;
        public int SelectedVintage
        {
            get { return _SelectedVintage; }
            set
            {
                SetProperty<int>(ref _SelectedVintage, value);
                WeekFill();
            }
        }
        private MPWeek _SelectedWeek;
        public MPWeek SelectedWeek
        {
            get { return _SelectedWeek; }
            set
            {
                SetProperty(ref _SelectedWeek, value);
                DecsionBEListFill();
            }
        }
        private MPDecisionBE _SelectedBE;
        public MPDecisionBE SelectedBE
        {
            get { return _SelectedBE; }
            set { SetProperty(ref _SelectedBE, value); }
        }
        private MPEMail _BGHR_EMail;
        public MPEMail BGHR_EMail
        {
            get { return _BGHR_EMail; }
            set { SetProperty(ref _BGHR_EMail, value); }
        }

        private string _tagHint;
        public string TagHint
        {
            get { return _tagHint; }
            set { SetProperty(ref _tagHint, value); }
        }

        private bool _VersandArt;
        public bool VersandArt
        {
            get { return _VersandArt; }
            set { SetProperty(ref _VersandArt, value); }
        }

        private Senat _SelectedSenat;
        public Senat SelectedSenat
        {
            get { return _SelectedSenat; }
            set { 
                SetProperty(ref _SelectedSenat, value); 
                SenatBEList.Clear();
                Senat QuerySenat = UserDbContext.Senate.Where(x => x.SenatID == SelectedSenat.SenatID).Include(x => x.Users).FirstOrDefault();
                foreach (User user in QuerySenat.Users)
                {
                    SenatBEList.Add(user);
                }
            }
        }

        private User _SelectedSenatBE;
        public User SelectedSenatBE
        {
            get { return _SelectedSenatBE; }
            set { SetProperty(ref _SelectedSenatBE, value); }
        }

        private MPBGHRSenat _SelectedSenatRecipient;
        public MPBGHRSenat SelectedSenatRecipient
        {
            get { return _SelectedSenatRecipient; }
            set { 
                SetProperty(ref _SelectedSenatRecipient, value);
                try
                {
                       
                    SelectedSenat = SelectedSenatRecipient != null ? SenateList.FirstOrDefault(x => x.SenatID == SelectedSenatRecipient.Senat) ?? null : null;
                    SelectedSenatBE = SelectedSenatRecipient != null ? SenatBEList.FirstOrDefault(x => x.UserId == SelectedSenatRecipient.Recipient) ?? null : null;
                }
                catch (Exception ex) { ErrorMessage.CreateExceptionWithFlyOutMessage("SelectedSenatRecipient", ex);}
            }
        }


        public MontagsPostBGHRViewModel()
        {
            SendCommand = new RelayCommand(SendExecute);
            BackCommand = new RelayCommand(BackExecute);
            BECommand = new RelayCommand(BESelectionExecute);
            ResetCommand = new RelayCommand(ResetExecute);
            SaveEMailCommand = new RelayCommand(SaveEMailExecute);
            ResetEMailCommand = new RelayCommand(ResetEMailExecute);
            AddSenateEmpfaengerCommand = new RelayCommand(AddSenateEmpfaengerExecute);
            RemoveSenateEmpfaengerCommand = new RelayCommand(RemoveSenateEmpfaengerExecute);
            TagHint = "Achtung: Durch die Abkürzung (Tag) <BR> wird ein Absatz erzeugt. Die Tags sollten beibehalten bleiben.";
            VersandArt = true;

            Logger.WriteLog("MontagsPostBGHR opend");
            var MPVintages_Query = mPDBContext.MPWeeks.Select(x => x.MPWeekYear).Distinct();
            foreach (var Vintage in MPVintages_Query) VintageList.Add(Vintage);
            if (VintageList.Count > 0) SelectedVintage = VintageList.LastOrDefault();

            BGHR_EMail = mPDBContext.MPEMails.FirstOrDefault(x => x.MPEMailDescription == MPEMailDescription);
            if (BGHR_EMail == null)
            {
                BGHR_EMail = new MPEMail
                {
                    MPEMailDescription = MPEMailDescription,
                    MPEMailSubject = DefaultSubject,
                    MPEMailBody = DefaultBody
                };
                mPDBContext.MPEMails.Add(BGHR_EMail);
                mPDBContext.SaveChanges();
            };

            Fill_SenateRecipientList();

            var Seante_Query = UserDbContext.Senate.Where(x => x.SenatArt == 1).ToList();
            foreach (var Senat in Seante_Query) {
                if (Senat.SenatName != "unbekannter Senat") SenateList.Add(Senat);
            };

            var query = UserDbContext.Users.Where(x => x.PositionId == 1).OrderBy(x => x.NachName).ThenBy(x => x.VorName);
            foreach (User Richter in query) MPBEList.Add(Richter);

        }

        private void Fill_SenateRecipientList()
        {
            SenateRecipientList.Clear();
            var SenateRecipient_Query = mPDBContext.MPBGHRSenate.ToList();
            foreach (var item in SenateRecipient_Query) SenateRecipientList.Add(item);
        }

        private void RemoveSenateEmpfaengerExecute(object obj)
        {
            string messageBoxText = "Bitte fülllen Sie alle Felder aus.";
            bool ErrorSenat = false;
            bool ErrorBE = false;

            Logger.WriteLog("RemoveSenateEmpfaengerExecute fired");
            if (SelectedSenat == null) ErrorSenat = true;
            if (SelectedSenatBE == null) ErrorBE = true;
            if (ErrorSenat || ErrorBE)
            {
                ViewManager.ShowMainInfoFlyout(messageBoxText, false);
                return;
            }

            MPBGHRSenat deleteSenat = mPDBContext.MPBGHRSenate.FirstOrDefault(x => x.Senat == SelectedSenat.SenatID);
            if (deleteSenat != null)
            {
                mPDBContext.MPBGHRSenate.Remove(deleteSenat);
                mPDBContext.SaveChanges();
                Fill_SenateRecipientList();
            }
            else
            {
                ViewManager.ShowMainInfoFlyout("Der Senat konnte nicht gelöscht werden.", false);
            }

        }

        private void AddSenateEmpfaengerExecute(object obj)
        {
            string messageBoxText = "Bitte tragen Sie in folgende Felder einen Wert ein: ";
            bool ErrorSenat = false;
            bool ErrorBE = false;

            Logger.WriteLog("AddSenateEmpfaengerExecute fired");
            if (SelectedSenat == null)
            {
                messageBoxText += "Senat";
                ErrorSenat = true;
            }
            if (SelectedSenatBE == null)
            {
                messageBoxText += ErrorSenat ? "; Berichterstatter" : "Berichterstatter";
                ErrorBE = true;
            }
            if (ErrorSenat || ErrorBE)
            {
                ViewManager.ShowMainInfoFlyout(messageBoxText, false);
                return;
            }

            MPBGHRSenat AddSenat = new MPBGHRSenat
            {
                Senat = SelectedSenat.SenatID,
                Recipient = SelectedSenatBE.UserId
            };
            mPDBContext.MPBGHRSenate.AddOrUpdate(AddSenat);
            mPDBContext.SaveChanges();
            Fill_SenateRecipientList();

        }

        private void ResetEMailExecute(object obj)
        {
            bool antwort = ViewManager.ShowQuestionWindow("Soll der E-Mail-Text zurückgesetzt werden?", "Ja");
            if (antwort) { 
                BGHR_EMail = new MPEMail
                {
                    MPEMailDescription = MPEMailDescription,
                    MPEMailSubject = DefaultSubject,
                    MPEMailBody = DefaultBody
                };
                SaveEMail("Der E-Mail-Text wurde zurückgesetzt und gespeichert.");            
            }


        }

        private void SaveEMail(string Message)
        {
            MPEMail saveItem = mPDBContext.MPEMails.FirstOrDefault(x => x.MPEMailDescription == MPEMailDescription);
            if (saveItem != null)
            {
                saveItem.MPEMailBody = BGHR_EMail.MPEMailBody;
                try
                {
                    mPDBContext.MPEMails.Add(saveItem);
                    mPDBContext.SaveChanges();
                    ViewManager.ShowMainInfoFlyout(Message, false);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog($"Beim Speichern der BGHR E-Mail ist folgender Fehler aufgetreten: {ex.Message}; {ex.InnerException}");
                    ViewManager.ShowMainInfoFlyout("Die E-Mail konnte nicht gespeichert werden. Bitte prüfen Sie den Fehler in den Log-Files.", false);
                }
            };
        }
        private void SaveEMailExecute(object obj)
        {
            SaveEMail("Der E-Mail-Text wurde gespeichert.");
        }
        private void ResetExecute(object obj)
        {
            try
            {
                MPDBContext mPDBContext = new MPDBContext();
                MPDecision editDecision = mPDBContext.MPDecisions.FirstOrDefault(x => x.MPDecisionID == SelectedBE.Decision.MPDecisionID);
                if (editDecision != null)
                {
                    editDecision.BEEMail = false;
                    mPDBContext.MPDecisions.AddOrUpdate(editDecision);
                    mPDBContext.SaveChanges();
                    DecsionBEListFill();

                }
                else
                {
                    Logger.WriteLog($"Beim Reset der E-Mail für BGHR ist folgender Fehler aufgetreten: Die Entscheidung wurde nicht gefunden");
                    ViewManager.ShowMainInfoFlyout("Der E-Mail-Empfang konnte nicht zurück gesetzt werden", false);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog($"Beim Reset der E-Mail für BGHR ist folgender Fehler aufgetreten: {ex.Message}; {ex.InnerException}");
                ViewManager.ShowMainInfoFlyout("Der E-Mail-Empfang konnte nicht zurück gesetzt werden", false);
            }
            
        }
        private void BESelectionExecute(object obj)
        {
            User user = (User)obj;
            MessageBox.Show(user.VorName);
        }
        private void BackExecute(object obj)
        {
            ViewManager.ShowPageOnMainView<MontagsPostView>();
        }
        private void WeekFill()
        {
            MPWeekList.Clear();
            if (SelectedVintage > 0)
            {
                MPDBContext mPDBContext = new MPDBContext();
                var MPWeek_Query = mPDBContext.MPWeeks.Include(x => x.MPDecisions).Where(x => x.MPWeekYear == SelectedVintage).OrderByDescending(x => x.MPWeekNumber);
                foreach (var item in MPWeek_Query) MPWeekList.Add(item);
            }
            return;
        }
        private void DecsionBEListFill()
        {
            MPDecisionBEList.Clear();
            if (SelectedWeek != null)
            {
                MPDBContext mPDBContext = new MPDBContext();
                UserDBContext userDBContext = new UserDBContext();
                var Query = mPDBContext.MPDecisions.Where(x => x.MPWeekID == SelectedWeek.MPWeekID && (x.Senat.MPCategorieID == 2 || x.Senat.MPCategorieID == 4)).OrderBy(x => x.Senat.MPSenatSorting).ThenBy(x => x.Aktenzeichen);
                foreach (MPDecision item in Query)
                {
                    if (item.Leitsatz != string.Empty)
                    {
                        //Entweder Zivilsenat oder Anwalts-, Notarsenat oder Kartellsenat
                        if (item.Senat.MPCategorieID == 2 || item.Senat.MPSenatID == 22 || item.Senat.MPSenatID == 23 || item.Senat.MPSenatID == 24 || item.Senat.MPSenatID == 32)
                        {
                            User BEAlternative = userDBContext.Users.FirstOrDefault(x => x.UserId == item.BE);
                            MPDecisionBEList.Add(new MPDecisionBE { Decision = item, BEAlternative = BEAlternative });
                        }
                        User BE = userDBContext.Users.FirstOrDefault(x => x.UserId == item.BE);
                        MPDecisionBEList.Add(new MPDecisionBE { Decision = item, BE = BE });
                    }
                }
            }
        }
        private async void SendExecute(object obj)
        {
            if (SelectedWeek == null)
            {
                ViewManager.ShowMainInfoFlyout("Bitte wählen Sie eine Kalenderwoche aus.", false);
                return;
            }
            string actionName = "Import Kalenderwoche";
            Task<DBResponse> task = SendEMails();
            ViewManager.ShowMainInfoFlyout("Die EMails werden versendet", false);
            ViewManager.ActionlistAdd(actionName);
            await task;
            ViewManager.ActionlistRemove(actionName);
            if (task.Result.Success)
            {
                ViewManager.ShowMainInfoFlyout("Die EMails wurden erfolgreich versendet.", false);
            }
            else
            {
                ErrorMessage.CreateSimpleMessage(task.Result.Message);
            }
        }

        private Task<DBResponse> SendEMails()
        {
            Task<DBResponse> task = Task.Run<DBResponse>(() =>
            {
                DBResponse response = new DBResponse();

                try
                {
                    MPDBContext mPDBContext = new MPDBContext();
                    EMailVersand eMailVersand = new EMailVersand();

                    //var query = from m in MPDecisionBEList
                    //            group m.Decision by m.BE.UserId into g
                    //            select new { UserID = g.Key, DecisionsID = g.ToList() };

                //foreach (var item in query)
                //{
                //    Debug.WriteLine (item);
                //}
                    ObservableCollection<MPDecisionBE> SortlistMPBE = new ObservableCollection<MPDecisionBE>();

                    foreach (MPDecisionBE item in MPDecisionBEList)
                    {
                        if (item.BEAlternative != null) item.BE = item.BEAlternative;
                        if (item.BE != null || item.BEAlternative != null)
                        {
                            MPBGHRSenat commonBE = mPDBContext.MPBGHRSenate.FirstOrDefault(x => x.Senat == item.Decision.SenatID);
                            if (commonBE != null)
                            {
                                User recipientUser = UserDbContext.Users.FirstOrDefault(x => x.UserId == commonBE.Recipient);
                                if (recipientUser != null) item.BE = recipientUser;
                            }
                            SortlistMPBE.Add(item);
                        }
                    }

                    var groupedBEList = SortlistMPBE
                        .GroupBy(u => u.BE.UserId)
                        .Select(g => g.ToList())
                        .ToList();

                    foreach (var item in groupedBEList)
                    {
                        string eMailAdress = string.Empty;
                        List<MPDecision> decisionList = new List<MPDecision>();

                        foreach (MPDecisionBE mPDecision in item)
                        {
                            if (eMailAdress == string.Empty) eMailAdress = mPDecision.BE.EMail;
                            if (!mPDecision.Decision.BEEMail) decisionList.Add(mPDecision.Decision);
                        }

                        if (decisionList.Count > 0)
                        {
                            List<CustomMailAttachment> attachmentListpfd = new List<CustomMailAttachment>();
                            foreach (MPDecision mPDecision in decisionList) 
                                attachmentListpfd.Add(new CustomMailAttachment { AttachmentPath = mPDecision.PathName + mPDecision.FileName, AttachmentName = mPDecision.FileName });
                            DBResponse eMailResponse = eMailVersand.Send_Email(
                                emailTo: eMailAdress,
                                subject: $"Entscheidungen {SelectedWeek.MPWeekName}",
                                mailBody: BGHR_EMail.MPEMailBody,
                                attachmentList: attachmentListpfd,
                                immediatSend: VersandArt);
                            if (eMailResponse.Success)
                            {
                                foreach (MPDecision mPDecision in decisionList)
                                {
                                    MPDecision changeDecision = mPDBContext.MPDecisions.FirstOrDefault(x => x.MPDecisionID == mPDecision.MPDecisionID);
                                    if (changeDecision != null)
                                    {
                                        changeDecision.BEEMail = true;
                                        try
                                        {
                                            mPDBContext.MPDecisions.AddOrUpdate(changeDecision);
                                            mPDBContext.SaveChanges();
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.WriteLog($"Beim Speichern des Versandmerkmals ist bei der Entscheidung {mPDecision.FileName} folgender Fehler aufgetreten: {ex.Message}; {ex.InnerException}");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Logger.WriteLog(eMailResponse.Message);
                                response.Message = response.Message == string.Empty ? $"An folgende Empfänger konnte keine E-Mail versendet werden: {eMailAdress}" : response.Message + $"; {eMailAdress}";
                            }
                        }
                        else
                        {
                            response.Message = "Es wurden bereits für alle Entscheidungen die Benachrichtigungen verschickt.";
                        }
                    }
                    if (response.Message == string.Empty) response.Success = true;
                }
                catch (Exception ex)
                {
                    Logger.WriteLog($"Beim Senden ist folgender Fehler aufgetreten: {ex.Message}, {ex.InnerException}");
                    response.Message = $"Beim Senden ist folgender Fehler aufgetreten: {ex.Message}";
                    throw;
                }
                return response;
            });
            return task;
        }
    }
}
