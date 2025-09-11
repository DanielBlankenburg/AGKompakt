using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Classes.SystemSettings;
using BGH_Kompakt.Enums;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.UserService;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using static BGH_Kompakt.Enums.SettingEnums;

#nullable enable

namespace BGH_Kompakt.Classes.UserClasses
{
    public class User
    {
        public int UserId { get; set; }
        public string VorName { get; set; } = string.Empty;
        public string NachName { get; set; } = string.Empty;
        public string EMail { get; set; } = string.Empty;   
        public string ComputerName { get; set; } = string.Empty;
        public bool Senatszugehörigkeit { get; set; } = true;

        public int GeschlechtID { get; set; }
        public virtual Geschlecht Geschlecht { get; set; }

        public int? TitelId { get; set; }
        public virtual Titel Titel { get; set; }

        public int PositionId { get; set; }
        public virtual Position Position { get; set; }  

        public int StatusId { get; set; }
        public virtual Status Status { get; set; }

        public int? DienstbezeichnungId { get; set; }
        public virtual Dienstbezeichnung Dienstbezeichnung { get; set; }

        public UserFilterMP? FilterMP { get; set; }

        public IList<Senat> Senate { get; set; }
        public IList<Senat> SenateAdmin { get; set; }
        public IList<AdminStatus> AdminStatus { get; set; }
        public Drives? MPBSCW_Server_Drive { get; set; } = new Drives();
        public bool MPEMailNotification { get; set; } = false;
        public bool MPBSCWSubFolders { get; set; } = false;
        public string Testfield { get; set; } = string.Empty;
        public int Testzahl { get; set; }

        [NotMapped]
        public string Fullname
        {
            get { return $"{SetTitel()}{VorName} {NachName}"; }
        }
        
        [NotMapped]
        public string FullSurname
        {
            get { return $"{SetTitel()}{NachName}"; }
        }

        private string SetTitel()
        {
            string titel = string.Empty;
            if (TitelId > 1)
            {
                if (Titel != null)
                {
                    titel = Titel.TitelText + " ";
                }
                else
                {
                    UserDBContext userDBContext = new UserDBContext();
                    Titel iTitel = userDBContext.Titel.Where(t => t.TitelId == TitelId).FirstOrDefault();
                    if (iTitel != null) titel = iTitel.TitelText + " ";
                }
            }
            return titel;
        }
        [NotMapped]
        public bool IsARAdmin { get; set; } = false;
        [NotMapped]
        public bool IsARVorzimmer { get; set; } = false;
        [NotMapped]
        public bool IsARPraesdialrichter { get; set; } = false;
        [NotMapped]
        public bool IsARPraesident { get; set; } = false;
        [NotMapped]
        public bool IsMPAdmin { get; set; } = false;
        [NotMapped]
        public bool IsVorsitzenderRichter { get; set; } = false;
        [NotMapped]
        public bool ShowSitzungsunterlagen { get; set; }
        [NotMapped]
        public bool ShowActivityRequests { get; set; }
        [NotMapped]
        public bool ShowMontagspost { get; set; }
        [NotMapped]
        public bool ShowMontagspostAdmin { get; set; }

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
            if (AdminStatus != null)
            {
                foreach (AdminStatus adminStatus in AdminStatus)
                {
                    string adminstatustext = adminStatus.AdminStatusText;
                    if (adminstatustext == UserEnums.EnumAdminStatus.NebentätigkeitenAdmin.ToString()) IsARAdmin = true;
                    if (adminstatustext == UserEnums.EnumAdminStatus.Vorzimmer.ToString()) IsARVorzimmer = true;
                    if (adminstatustext == UserEnums.EnumAdminStatus.Präsidialrichter.ToString()) IsARPraesdialrichter = true;
                    if (adminstatustext == UserEnums.EnumAdminStatus.Präsidentin.ToString()) IsARAdmin = true;
                    if (adminstatustext == UserEnums.EnumAdminStatus.MontagspostAdmin.ToString()) IsMPAdmin = true;
                    if (adminstatustext == UserEnums.EnumAdminStatus.MontagspostShow.ToString()) ShowMontagspost = true;
                    if (adminstatustext == UserEnums.EnumAdminStatus.NebentätigkeitenShow.ToString()) ShowActivityRequests = true;
                }

                UserDBContext userDBContext = new UserDBContext();
                ProgrammSetting programmSettings = userDBContext.ProgrammSettings.FirstOrDefault();

                if (programmSettings != null)
                {
                    if (programmSettings.MontagspostActivated == true) if (!ShowMontagspost) ShowMontagspost = PositionId == 1 || PositionId == 2;
                }
                if (!ShowMontagspost) ShowMontagspost = IsMPAdmin;
                if (!ShowMontagspostAdmin) ShowMontagspostAdmin = IsMPAdmin;
                //
                if (programmSettings != null)
                {
                    if (programmSettings.ActivityRequestActivated == true) if (!ShowActivityRequests) ShowActivityRequests = PositionId == 1 || PositionId == 2;
                }
                if (!ShowActivityRequests) ShowActivityRequests = IsARAdmin || IsARPraesdialrichter || IsARPraesident;
                if (Senate != null) ShowSitzungsunterlagen = Senate.Count > 0;
            }
            //Bestimmung, ob User VorsitzenderRichter ist
            if (Dienstbezeichnung != null)
            {
                if (Dienstbezeichnung.DienstbezeichnungText == UserEnums.EnumDienstbezeichnungen.VRiBGH.ToString() ||
                    Dienstbezeichnung.DienstbezeichnungText == UserEnums.EnumDienstbezeichnungen.VRinBGH.ToString()) IsVorsitzenderRichter = true;
            }
        }

        public bool IsSenatAdmin(Senat iSenat)
        {
            if (iSenat == null) return false; 
            foreach(Senat senat in SenateAdmin) if (senat.SenatID == iSenat.SenatID) return true;
            return false;
        }

    }
}
