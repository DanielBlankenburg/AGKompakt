using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.Base;
using BGH_Kompakt.Classes.SystemSettings;
using BGH_Kompakt.Enums;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using static BGH_Kompakt.Enums.SettingEnums;

#nullable enable

namespace BGH_Kompakt.Classes.UserClasses
{
    public class User : Person
    {
        public int UserId { get; set; }
        //public string VorName { get; set; } = string.Empty;
        //public string NachName { get; set; } = string.Empty;
        //public string EMail { get; set; } = string.Empty;   
        public string ComputerName { get; set; } = string.Empty;

        public int PositionId { get; set; }
        public virtual Position Position { get; set; }  

        public int StatusId { get; set; }
        public virtual Status Status { get; set; }

        public int? DienstbezeichnungId { get; set; }
        public virtual Dienstbezeichnung Dienstbezeichnung { get; set; }

        public IList<AdminStatus> AdminStatus { get; set; }
        public IList<UserDienstbezeichnung> UserDienstbezeichnungen { get; set; }
        public Drives? MPBSCW_Server_Drive { get; set; } = new Drives();
        public string Initials {  get; set; } = string.Empty;
        public string Testfield { get; set; } = string.Empty;
        public int Testzahl { get; set; }



        [NotMapped]
        public bool IsARAdmin { get; set; } = false;

        [NotMapped]
        public bool IsInactiv
        {
            get { return StatusId == 2; }
        }

        public User()
        {
            
        }

        public User(string vorname, string nachname, string eMail, string computerName, int geschlecht, int title, int position, int status, int dienstbezeichnung)
        {
            VorName = vorname;
            NachName = nachname;
            EMail = eMail;
            ComputerName = computerName;
            GeschlechtID = geschlecht;
            PositionId = position;
            StatusId = status;
            TitelId = title;
            DienstbezeichnungId = dienstbezeichnung;   

        }

        public void SetStatus()
        {
            try
            {
                if (AdminStatus != null)
                {
                    foreach (AdminStatus adminStatus in AdminStatus)
                    {
                        string adminstatustext = adminStatus.AdminStatusText;
                        //if (adminstatustext == UserEnums.EnumAdminStatus.NebentätigkeitenAdmin.ToString()) IsARAdmin = true;
                    }

                    UserDBContext userDBContext = new UserDBContext();
                    ProgrammSetting programmSettings = userDBContext.ProgrammSettings.FirstOrDefault();

                }
            }
            catch (System.Exception ex)
            {
                Logger.WriteLog($"logTime: {DateTime.Now}; Es konnte folgender Fehler beim Erstellen des Status aufgetreten: {ex.Message}.");
                throw;
            }
        }
    }
}
