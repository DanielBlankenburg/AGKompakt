using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace BGH_Kompakt.Services.UserService
{
    public static class UserManager
    {
        private static readonly UserDBContext dBContext = new UserDBContext();
        private static readonly UserDBContext userDBContext = new UserDBContext();

        public static User RegistratedUser { get; set; }


        public static bool LoginUser (string userName)
        {
            try
            {
                User Ergebnis = dBContext.Users.Include(a => a.AdminStatus).Where(a => a.ComputerName == userName).FirstOrDefault();

                if (Ergebnis != null)
                {
                    RegistratedUser = Ergebnis;
                    RegistratedUser.SetStatus();
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
                                            Titel titel, Geschlecht geschlecht, Status status, Position position, Dienstbezeichnung dienstbezeichnung, bool senatszugehögrigkeit)
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


        public static bool AdminstatusCheck (string AdminText)
        {    
            foreach (AdminStatus Status in UserManager.RegistratedUser.AdminStatus) if (Status.AdminStatusText == AdminText) return true;
            return false;
        }
    }
}

