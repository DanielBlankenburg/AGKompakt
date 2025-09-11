using BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Services.DBContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable enable

namespace BGH_Kompakt.Classes.ActivityRequestClasses
{
    public class ActivityRequest
    {
        public int ActivityRequestId { get; set; }
        public DateTime ARDatum { get; set; }
        public string? ARTitel { get; set; }
        public decimal ARVerguetung { get; set; }
        public Single ARZeitaufwandMain { get; set; }
        public Single? ARZeitaufwandPrep { get; set; }
        public string? ARNote { get; set; }
        public string? ARNoteAdmin { get; set; }
        public string? AROrt {  get; set; }
        public DateTime? ARActivityDate { get; set; }
        public bool? ARAttechments { get; set; }
        public DateTime? ActivityRequestDatePermanentFrom { get; set; }
        public DateTime? ActivityRequestDatePermanentUntil { get; set; }
        public int ActivityRequestDatePermantenDuration { get; set; }
        public string? ActivityRequestArbitrationCaller { get; set; }
        public bool Assurance {  get; set; }
        public int ARUserID { get; set; }
        public int? ActivityRequestTypID { get; set; }
        public virtual ActivityRequestTyp? ActivityRequestTyp { get; set; }
        public int ActivityRequestMeldeArtID { get; set; }
        public virtual ActivityRequestMeldeArt? ActivityRequestMeldeArt { get; set; }
        public int? ActivityClientID { get; set; }
        public virtual ActivityClient? ActivityClient{ get; set; }
        public int? ActivityRequestScienceTypId { get; set; }
        public virtual ActivityRequestScienceTyp? ActivityRequestScienceTyp { get; set; }
        public int? ActivityRequestScienceCategorieId { get; set; }
        public virtual ActivityRequestScienceCategorie? ActivityRequestScienceCategorie { get; set; }
        //public int? ActivityRequestScienceAuthorId { get; set; }
        //public virtual ActivityRequestScienceAuthor? ActivityRequestScienceAuthor { get; set; }
        public int? ActivityRequestOrtArtId { get; set; }
        public virtual ActivityRequestOrtArt? ActivityRequestOrtArt { get; set; }
        public int? ActivityRequestFrequencyId { get; set; }
        public virtual ActivityRequestFrequency? ActivityRequestFrequency { get; set; }
        public int? ActivityRequestArbitrationTypId { get; set; }
        public virtual ActivityRequestArbitrationTyp? ActivityRequestArbitrationTyp { get; set; }
        public int? ActivityRequestVerguetungTypId { get; set; } //1 = erhaltene Vergütung, 2 = Honorarfrei; 3 = Unbekannt
        public int ActivityRequestHourTypId { get; set; } = 1;// 1 = Vorhersage; 2 = Unbekannt
        public bool SciencenAuthorAuthor {  get; set; } = false;
        public bool SciencenAuthorSchriftleitung {  get; set; } = false;
        public bool SciencenAuthorHerausgeber {  get; set; } = false;
        public bool SciencenAuthorWissenschaftlicherBeirat {  get; set; } = false;
        public bool SciencenAuthorSonstiges {  get; set; } = false;

        public  ICollection<ARVerguetungAdventage>? ARVerguetungAdventages { get; set; }
        public  ICollection<ActivityRequestArbitrationClient>? ActivityRequestArbitrationClients {  get; set; }
        public  ICollection<ActivityRequestDataFile>? ActivityRequestDataFiles {  get; set; }
        //public  ICollection<ActivityRequestScienceAuthorName>? ActivityRequestScienceAuthors {  get; set; }
        public int ARZustaendigkeitsbereich { get; set; } = 1; //1 = Zuständigkeit beim Einreichenden; 2 = Zuständigkeit Präsidialrichter; 3 = Zuständigkeit Präsidentin; 4 = Zuständigkeit Vorzimmer; 5 = Archiv; 6 = Vorsitzender
        public bool ARRejected { get; set; } = false;
        [NotMapped]
        public string Status
        {
            get
            {
                switch (ARZustaendigkeitsbereich)
                {
                    case 1:
                        return "Entwurf";
                    case 2:
                        return "In Bearbeitung";
                    case 3:
                        return "In Bearbeitung";
                    case 4:
                        return "In Bearbeitung";
                    case 5:
                        return "Archiv";
                    case 6:
                        return "Beim Vorsitzenden";
                    default:
                        return "Unbekannt";
                }
            }
        }
        [NotMapped]
        public Single? Gesamtzeitaufwand { get {return ARZeitaufwandMain + ARZeitaufwandPrep;}}

        public User? ARUser
        {
            get
            {
                UserDBContext db = new UserDBContext();
                return db.Users.Find(ARUserID) ?? null;
            }
        }

        [NotMapped]
        public int AdventagesCount
        {
            get { return (ARVerguetungAdventages != null) ? ARVerguetungAdventages.Count : 0; }
        }

        [NotMapped]
        public bool ARExclamation
        {
            get { return !string.IsNullOrWhiteSpace(ARNoteAdmin); }
        }

        [NotMapped]
        public bool ARCheck
        {
            get { return (!ARRejected && !ARExclamation && ARZustaendigkeitsbereich > 2); }
        }
    }
}
