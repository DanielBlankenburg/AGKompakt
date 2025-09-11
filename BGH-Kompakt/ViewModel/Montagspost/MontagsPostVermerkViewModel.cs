using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.MontagspostService;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Views;
using BGH_Kompakt.Views.Windows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Montagspost
{
    public class MontagsPostVermerkViewModel : ViewModelBase
    {
        public List<MPVermerk> VermerkList { get; set; } = new List<MPVermerk>();
        private MPVermerk _selectedVermerk;
        public MPVermerk SelectedVermerk
        {
            get { return _selectedVermerk; }
            set
            {
                Vermerktext = value.MPVermerkText;
                value = null;
                SetProperty<MPVermerk>(ref _selectedVermerk, value);
                
                //Vermerkindex = -1;
            }
        }

        private MPDecision _SelectedDecision;
        public MPDecision SelectedDecision
        {
            get { return _SelectedDecision; }
            set
            {
                SetProperty<MPDecision>(ref _SelectedDecision, value);
            }
        }

        private string _Vermerktext = string.Empty;
        public string Vermerktext
        {
            get { return _Vermerktext; }
            set { SetProperty(ref _Vermerktext, value); }
        }

        private int _Vermerkindex;
        public int Vermerkindex
        {
            get { return _Vermerkindex; }
            set { SetProperty(ref _Vermerkindex, value); }
        }



        public MPDBContext mPDBContext = new MPDBContext();

        public ICommand SaveCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand EMailPreviewCommand { get; set; }

        public MontagsPostVermerkViewModel()
        {
            SaveCommand = new RelayCommand(SaveExecute);
            BackCommand = new RelayCommand(BackExecute);
            EMailPreviewCommand = new RelayCommand(EmailPreviewExecute);

            SelectedDecision = mPDBContext.MPDecisions.Include(x => x.MPWeek).Where(x => x.MPDecisionID == MontagsPostManager.SavedDecision.MPDecisionID).FirstOrDefault();
            VermerkList = mPDBContext.MPVermerke.ToList();
            Vermerktext = SelectedDecision.Vermerk;
        }

        private void EmailPreviewExecute(object obj)
        {
            DBResponse resp = CreateEMail();
            EMailResponse email = (EMailResponse)resp.Data;
            PreviewEMail previewEMail = new PreviewEMail(email) { Owner = Application.Current.MainWindow };
            previewEMail.ShowDialog();
        }

        private void BackExecute(object obj)
        {
            try
            {
                ViewManager.ShowPageOnMainView<MontagsPostView>();
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Es ist folgender Fehler aufgetreten: {ex.Message}", false);
            }
        }

        private async void SaveExecute(object obj)
        {
            string actionName = "Eintragung Vermerk";
            Task<DBResponse> task = SaveVermerk();
            ViewManager.ActionlistAdd(actionName);
            await task;
            string message = task.Result.Success ? "Der Vermerk wurde eingetragen." : task.Result.Message;
            ViewManager.ShowMainInfoFlyout(message, false);
            ViewManager.ActionlistRemove(actionName);
            ViewManager.ShowPageOnMainView<MontagsPostView>();

        }

        private Task<DBResponse> SaveVermerk()
        {
            Task<DBResponse> task = Task.Run<DBResponse>(() => 
            { 
                DBResponse resp = new DBResponse();
                SelectedDecision.Vermerk = Vermerktext;
                try
                {
                    mPDBContext.MPDecisions.AddOrUpdate(SelectedDecision);
                    mPDBContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    resp.Success = false;
                    resp.Message = $"Der Vermerk konnte nicht eingetragen werden. Es ist folgender Fehler aufgetreten: {ex.Message}";
                }
                if (SelectedDecision.EMailVersenden)
                {
                    resp = CreateEMail();
                    if (!resp.Success) return resp;
                    EMailResponse eMail = (EMailResponse)resp.Data;
                    EMailVersand eMailVersand = new EMailVersand();
                    resp = eMailVersand.Send_Email(eMail.EmailTo, eMail.Subject, eMail.Body, eMail.EMailToBCC);

                }
                else resp.Success = true;
                return resp;        
            });
            return task;
        }

        public DBResponse CreateEMail()
        {

            DBResponse resp = new DBResponse();
            EMailResponse eMail = new EMailResponse();
            try
            {
                MPSetting mpSetting = mPDBContext.MPSettings.FirstOrDefault();
                MPEMail mpEMail = mPDBContext.MPEMails.FirstOrDefault(e => e.MPEMailDescription == "Anschreiben Berichtigung");
                if (mpSetting != null && mpEMail != null)
                {

                    eMail.Subject = Regex.Replace(mpEMail.MPEMailSubject, "'VerfahrenAZ'", $"{SelectedDecision.Aktenzeichen}");

                    string Text = $"{mpSetting.MPSettingEMailAnrede}, <br> <br>{mpEMail.MPEMailBody}<br> <br> {mpSetting.MPSettingEMailSchluss}<br> <br>{mpSetting.MPSettingDatenschutzhinweis}";
                    Text = Regex.Replace(Text, "'VerfahrenAZ'", $"{SelectedDecision.Aktenzeichen}");
                    Text = Regex.Replace(Text, "'KW'", $"{SelectedDecision.MPWeek.MPWeekNumber}/{SelectedDecision.MPWeek.MPWeekYear.ToString().Substring(2)}");
                    eMail.Body = Regex.Replace(Text, "'Vermerk'", $"{Vermerktext}");

                    string strEMailAdresses = string.Empty;

                    UserDBContext userDBContext = new UserDBContext();
                    List<User> MemberList = userDBContext.Users.ToList();

                    foreach (User Member in MemberList)
                    {
                        if (Member.PositionId == 1 || Member.PositionId == 2)
                        {
                            strEMailAdresses = !(strEMailAdresses == "") ? strEMailAdresses + ";" + Member.EMail.ToString() : Member.EMail.ToString();
                        }
                    }
                    eMail.EMailToBCC = strEMailAdresses;
                    eMail.EmailTo = BGHKompaktSystemInfo.EMailDokstelle;
                    resp.Data = eMail;
                    resp.Success = true;
                }
                else
                {
                    resp.Success = false;
                    resp.Message = mpSetting != null ? "Die allgemeinen Einstellungen für die E-Mail konnten nicht gefunden werden." : "Die Vorlage für den Vermerk konnte nicht gefunden werden.";
                }

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = $"Die E-Mail konnte nicht erstellt werden. Es ist folgender Fehler aufgetreten: {ex.Message}";
            }
            return resp;
        }

    }
}
