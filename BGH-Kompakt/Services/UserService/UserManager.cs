using BGH_Kompakt.Classes;
using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Enums;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Views;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace BGH_Kompakt.Services.UserService
{
    public static class UserManager
    {
        private static readonly UserDBContext dBContext = new UserDBContext();
        private static readonly UserDBContext userDBContext = new UserDBContext();

        public static User RegistratedUser { get; set; }
        public static SenatSetting SenatSettings { get; set; } = new SenatSetting();


        public static bool LoginUser (string userName)
        {
            try
            {
                User Ergebnis = dBContext.Users.Include(a => a.AdminStatus).Include(a => a.Senate).Include(x => x.SenateAdmin).Include(x => x.FilterMP).Where(a => a.ComputerName == userName).FirstOrDefault();

                if (Ergebnis != null)
                {
                    RegistratedUser = Ergebnis;
                    RegistratedUser.SetStatus();
                    if (RegistratedUser.Senate.Count > 0) Set_SenatSettings(RegistratedUser.Senate.ToArray());
                    return true;   
                }
                else { return false; }

            }
            catch (Exception ex)
            {
                Logger.WriteLog($"logTime: {DateTime.Now}; Es ist folgender Fehler beim einloggen des Users aufgetreten: {ex.Message}.");
                return false;
            }
        }

        public static DBResponse RegisterUser(string nachname, string vorname, string eMail, string username, 
                                            Titel titel, Geschlecht geschlecht, Status status, Position position, Dienstbezeichnung dienstbezeichnung, List<Senat> Senate, bool senatszugehögrigkeit)
        {
            string errorText = "Der Nutzer konnte nicht eingetragen werden. Es ist folgender Fehler aufgetreten: ";
            DBResponse resp = new DBResponse { Success = true };
            User user = new User
            {
                NachName = nachname,
                VorName = vorname,
                EMail = eMail,
                ComputerName = username,
                TitelId = titel.TitelId,
                GeschlechtID = geschlecht.GeschlechtID,
                StatusId = (status != null) ? status.StatusId : 1,
                PositionId = position.PositionId,              
                Senatszugehörigkeit = senatszugehögrigkeit
            };
            if (dienstbezeichnung != null)
            {
                Dienstbezeichnung addDienstbezeichnung = userDBContext.Dienstbezeichnungen.FirstOrDefault(d => d.DienstbezeichnungId == dienstbezeichnung.DienstbezeichnungId);
                if (addDienstbezeichnung != null) user.Dienstbezeichnung = addDienstbezeichnung;
            }

            if (Senate.Count > 0)
            {
                try
                {
                    Senat SelectedSenat = new Senat();
                    //Senat aus dem aktuellen DBContext holen; Sonst kommt es zu Doppelungen
                    List<Senat> iListSenate = new List<Senat>();
                    foreach(Senat item in Senate)
                    {
                        Senat iSenat = userDBContext.Senate.Include(x => x.AdminUsers).Where(s => s.SenatID == item.SenatID).FirstOrDefault();
                        iListSenate.Add(iSenat);
                        string Adresslist = string.Empty;
                        foreach (User Admin in iSenat.AdminUsers) Adresslist += $"{Admin.EMail};";
                        if (Adresslist != string.Empty)
                        {
                            try
                            {
                                EMailVersand eMailVersand = new EMailVersand();
                                eMailVersand.Send_Email(Adresslist, $"Registerierung Nutzer für den {iSenat.SenatName}", $"Der Nutzer {user.VorName} {user.NachName} hat sich für den {iSenat.SenatName} angemeldet.", immediatSend: true);
                            }
                            catch (Exception) { }
                        }
                        SelectedSenat = iSenat;
                    }
                    user.Senate = iListSenate;
                    Set_SenatSettings(iListSenate);
                }
                catch (Exception ex)
                {
                    resp.Success = false;
                    resp.Message = $"{errorText}{ex.Message}";
                    return resp;
                }
            }

            try
            {
                DbSet<User> _dBSet = userDBContext.Set<User>();
                _dBSet.Add(user);
                userDBContext.SaveChanges();
                resp.Data = user;
                return resp;    
            }
            catch (DbEntityValidationException ex)
            {

                //foreach (var eve in ex.EntityValidationErrors)
                //{
                //    MessageBox.Show($"Entity of type {eve.Entry.Entity.GetType().Name} in state {eve.Entry.State} has the following validation errors:");
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        MessageBox.Show($"- Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                //    }
                //}
                //MessageBox.Show("Es ist folgender Fehler aufgetreten: " + ex.Message);
                resp.Success = false;
                resp.Message = $"{errorText}{ex.Message}";
                return resp;
            }

        }

        public static void Set_SenatSettings(IList<Senat> iListSenate, Senat prefSenat = null)
        {
            
            if (prefSenat != null) if (SenatSettingSet(prefSenat)) return;
            foreach (var senat in iListSenate) if (SenatSettingSet(senat)) return;
        }

        private static bool SenatSettingSet(Senat senat)
        {
            SenatSetting senatsetting = userDBContext.SenatSettings.Include(s => s.Senat).Include(sg => sg.Spruchgruppen).FirstOrDefault(x => x.SenatID == senat.SenatID);
            if (senatsetting != null) 
            {
                //SenatSettings.SenatID = senatsetting.SenatID;
                //SenatSettings.Senat = senatsetting.Senat;
                //SenatSettings.ShowSitzungsplaene = senatsetting.ShowSitzungsplaene;
                //SenatSettings.ShowVerteilung = senatsetting.ShowVerteilung;
                //SenatSettings.ShowVotenmappe = senatsetting.ShowVotenmappe;
                //SenatSettings.ShowSpruchgruppen = senatsetting.ShowSpruchgruppen;
                //SenatSettings.ShowFormerDays = senatsetting.ShowFormerDays;
                //SenatSettings.BSCW_Server_Drive = senatsetting.BSCW_Server_Drive;
                //SenatSettings.AZPrefix = senatsetting.AZPrefix;
                //SenatSettings.AZPrefixDate = senatsetting.AZPrefixDate;
                //SenatSettings.AZPrefixChar = senatsetting.AZPrefixChar;
                //SenatSettings.ImportAGUrteil = senatsetting.ImportAGUrteil;
                //SenatSettings.ImportAGBeschluss = senatsetting.ImportAGBeschluss;
                //SenatSettings.ImportLGUrteil = senatsetting.ImportLGUrteil;
                //SenatSettings.ImportLGBeschluss = senatsetting.ImportLGBeschluss;
                //SenatSettings.ImportLGHB = senatsetting.ImportLGHB;
                //SenatSettings.ImportLGZB = senatsetting.ImportLGZB;
                //SenatSettings.ImportOLGUrteil = senatsetting.ImportOLGUrteil;
                //SenatSettings.ImportOLGBeschluss = senatsetting.ImportOLGBeschluss;
                //SenatSettings.ImportOLGHB = senatsetting.ImportOLGHB;
                //SenatSettings.ImportOLGZB = senatsetting.ImportOLGZB;
                //SenatSettings.ImportEUGHVorlage = senatsetting.ImportEUGHVorlage;
                //SenatSettings.ImportEUGHURteil = senatsetting.ImportEUGHURteil;
                //SenatSettings.ImportEntwurf = senatsetting.ImportEntwurf;
                //SenatSettings.ImportVotum = senatsetting.ImportVotum;
                //SenatSettings.ImportVorVotum = senatsetting.ImportVorVotum;
                //SenatSettings.ImportAnlage = senatsetting.ImportAnlage;
                //SenatSettings.ImportLeitsatz = senatsetting.ImportLeitsatz;
                //SenatSettings.ImportRMB = senatsetting.ImportRMB;
                //SenatSettings.ImportRME = senatsetting.ImportRME;
                //SenatSettings.ImportSonstiges = senatsetting.ImportSonstiges;
                //SenatSettings.Spruchgruppen = senatsetting.Spruchgruppen;
                //SenatSettings.StrafFolderSenat = senatsetting.StrafFolderSenat;


                SenatSettings = senatsetting;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool AdminstatusCheck (string AdminText)
        {    
            foreach (AdminStatus Status in UserManager.RegistratedUser.AdminStatus) if (Status.AdminStatusText == AdminText) return true;
            return false;
        }
    }
}

