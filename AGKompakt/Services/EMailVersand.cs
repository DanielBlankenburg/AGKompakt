using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.UserService;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;


namespace BGH_Kompakt.Services
{
    public class EMailVersand
    {
        public void EMailAlleAktivenRichterWiMa(string textBody, string subject, string textAnrede = "")
        {
            string Text = string.Empty;
            string strEMailAdresses = string.Empty;

            Text = textAnrede + textBody;
            //Text = "<html><div style='font-size:15px; font-family:Calibri;'>" + "blah blah <a href='file:///G:/Title.xlsx'>blah</a>" + "</div></html>";

            List<User> MemberList = new List<User>();
            UserDBContext userDBContext = new UserDBContext();
            foreach (User Member in MemberList)
            {
                if (Member.PositionId == 1 || Member.PositionId == 2)
                {
                    strEMailAdresses = !(strEMailAdresses == "") ? strEMailAdresses + ";" + Member.EMail.ToString() : Member.EMail.ToString();
                }

            }

            Send_Email(strEMailAdresses, subject, Text);
        }

        public DBResponse Send_Email(string emailTo, string subject, string mailBody, string BCC = "", List<CustomMailAttachment> attachmentList = null, bool immediatSend = false)
        {
            DBResponse resp = new DBResponse();
            try
            {
                Outlook.Application oApp = new Outlook.Application();
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                oMsg.HTMLBody = mailBody;
                oMsg.HTMLBody = "<html><div style='font-size:15.5px; font-family:Calibri;'>" + oMsg.HTMLBody + "</div></html>";
                //Anlagen anfügen
                if (attachmentList != null)
                {
                    foreach (CustomMailAttachment item in attachmentList)
                    {
                        string sDisplayName = item.AttachmentName;
                        int iPosition = (int)oMsg.Body.Length + 1;
                        int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                        oMsg.Attachments.Add(item.AttachmentPath, iAttachType, iPosition, sDisplayName);
                    }
                }
                oMsg.Subject = subject;
                oMsg.To = emailTo;
                if (BCC != "") oMsg.BCC = BCC;
                if (immediatSend) { oMsg.Send(); } else { oMsg.Display(); }
                oMsg = null;
                oApp = null;
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = $"Die Nachricht konnte nicht verschickt werden. Es ist folgender Fehler aufgetreten: {ex.Message};{ex.InnerException}";
            }
            return resp;
        }

    }
}
