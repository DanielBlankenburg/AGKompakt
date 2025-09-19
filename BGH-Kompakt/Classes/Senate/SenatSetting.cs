using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static BGH_Kompakt.Enums.SettingEnums;

namespace BGH_Kompakt.Classes.Senate
{
    public class SenatSetting
    {
        public int SenatSettingID { get; set; }
        public int? SenatID { get; set; }
        public virtual Senat Senat { get; set; }
        public bool ShowSitzungsplaene { get; set; } = false;
        public bool ShowVerteilung { get; set; } = false;
        public bool ShowVotenmappe { get; set; } = false;
        public bool ShowBerichterstatter { get; set; } = false;
        public bool ShowSpruchgruppen { get; set; } = false;
        public bool ShowFormerDays { get; set; } = true;
        public Drives? BSCW_Server_Drive { get; set; } = new Drives();
        public ICollection<SenatAktenzeichen> Aktenzeichen {  get; set; }
        public ICollection<SenatSpruchgruppe> Spruchgruppen { get; set; }
        //public string SG1 { get; set; }
        //public string SG2 { get; set; }
        //public string SG3 { get; set; }
        //public string SG4 { get; set; }
        //public string SG5 { get; set; }
        //public string SG6 { get; set; }
        //public string SG7 { get; set; }
        //public string SG8 { get; set; }
        //public string SG9 { get; set; }
        public bool AZPrefix { get; set; } = true;
        public bool AZPrefixDate { get; set; } = false;
        public string AZPrefixChar { get; set; }
        public string ImportAGUrteil { get; set; } = "AG Urteil";
        public string ImportAGBeschluss { get; set; } = "AG Beschluss";
        public string ImportLGUrteil { get; set; } = "LG Urteil";
        public string ImportLGBeschluss { get; set; } = "LG Beschluss";
        public string ImportLGHB { get; set; } = "LG HB";
        public string ImportLGZB { get; set; } = "LG HB";
        public string ImportOLGUrteil { get; set; } = "OLG Urteil";
        public string ImportOLGBeschluss { get; set; } = "OLG Beschluss";
        public string ImportOLGHB { get; set; } = "OLG HB";
        public string ImportOLGZB { get; set; } = "OLG ZB";
        public string ImportEUGHVorlage { get; set; } = "EuGH Vorlage";
        public string ImportEUGHURteil { get; set; } = "EuGH Urteil";
        public string ImportEntwurf { get; set; } = "Entscheidungsentwurf";
        public string ImportVotum { get; set; } = "Votum";
        public string ImportVorVotum { get; set; } = "Vorvotum";
        public string ImportAnlage { get; set; } = "Anlage";
        public string ImportLeitsatz { get; set; } = "L-Karte";
        public string ImportRMB { get; set; } = "Rechtsmittelbegründung";
        public string ImportRME { get; set; } = "Rechtsmittelerwiderung";
        public string ImportSonstiges { get; set; } = "Sonstiges";
        public bool StrafFolderSenat { get; set; } = true;
        public bool StrafFolderSubFolder { get; set; } = true; 
        public string StrafFolderSubFolderText { get; set; } = "Senatshefte";
        public bool StrafFolderBerichterstatter { get; set; } = false;
        public bool StrafFolderYearFirst { get; set; } = false;
        public bool StrafFileAzPrefix { get; set; } = false;
        public bool StrafFileSenat { get; set; } = true;
        public string StrafFileSenatsheftText { get; set; } = "Aktenauszug";
        public string StrafFileWhiteSpaceFill { get; set; } = "_";
    }
}
