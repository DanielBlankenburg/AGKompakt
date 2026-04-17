using BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.EntityConfigurations.ActivityRequestDBContext
{
    internal class ActivityRequestSeeder
    {
        private readonly BGH_Kompakt.Services.ActivityRequestDBContext Context;

        public ActivityRequestSeeder(BGH_Kompakt.Services.ActivityRequestDBContext context)
        {
            Context = context;
        }

        public void ActivityClientTyps()
        {
            Context.ActivityClientTyps.AddOrUpdate(a => a.ActivityClientTypText,
                new ActivityClientTyp { ActivityClientTypText = "Öffentlicher Auftraggeber" },
                new ActivityClientTyp { ActivityClientTypText = "Verlage" },
                new ActivityClientTyp { ActivityClientTypText = "Berufsverbände" },
                new ActivityClientTyp { ActivityClientTypText = "Private Seminarveranstalter" },
                new ActivityClientTyp { ActivityClientTypText = "Sonstige" }
                );
        }

        public void ActivityRequestMeldeArten()
        {
            Context.ActivityRequestMeldeArten.AddOrUpdate(a => a.ActivityRequestMeldeArtText,
                new ActivityRequestMeldeArt { ActivityRequestMeldeArtText = "Anzeigepflichtig" },
                new ActivityRequestMeldeArt { ActivityRequestMeldeArtText = "Genehmigungspflichtig" }
                );
        }

        public void ActivityRequestTyps()
        {
            Context.ActivityRequestTyps.AddOrUpdate(a => a.ActivityRequestTypText,
                new ActivityRequestTyp { ActivityRequestTypText = "Vortragstätigkeit", ActivityRequestTypMeldeArt = 1 },
                new ActivityRequestTyp { ActivityRequestTypText = "Wisseschaftliche Tätigkeit", ActivityRequestTypMeldeArt = 1 },
                new ActivityRequestTyp { ActivityRequestTypText = "Schriftstellerische Tätigkeit", ActivityRequestTypMeldeArt = 1 },
                new ActivityRequestTyp { ActivityRequestTypText = "Künstlerische Tätigkeit", ActivityRequestTypMeldeArt = 1 },
                new ActivityRequestTyp { ActivityRequestTypText = "Referententätigkeit", ActivityRequestTypMeldeArt = 2 },
                new ActivityRequestTyp { ActivityRequestTypText = "Prüfertätigkeit", ActivityRequestTypMeldeArt = 2 },
                new ActivityRequestTyp { ActivityRequestTypText = "Tätigkeit als Schiedsrichter/ Schiedsgutachter", ActivityRequestTypMeldeArt = 2 },
                new ActivityRequestTyp { ActivityRequestTypText = "Sonstige Nebentätigkeit", ActivityRequestTypMeldeArt = 2 }
                );
        }
        public void ActivityRequestScienceCategories()
        {
            Context.ActivityRequestScienceCategories.AddOrUpdate(a => a.ActivityRequestScienceCategorieText,
                new ActivityRequestScienceCategorie { ActivityRequestScienceCategorieText = "Wissenschaftliche Veröffentlichung" },
                new ActivityRequestScienceCategorie { ActivityRequestScienceCategorieText = "Lehr- oder sonstige wissenschaftliche Tätigkeit" }
                );
        }
        public void ActivityRequestScienceTyps()
        {
            Context.ActivityRequestScienceTyps.AddOrUpdate(a => a.ActivityRequestScienceTypText,
                new ActivityRequestScienceTyp { ActivityRequestScienceTypText = "Aufsatz" },
                new ActivityRequestScienceTyp { ActivityRequestScienceTypText = "Monographie" },
                new ActivityRequestScienceTyp { ActivityRequestScienceTypText = "Kommentar" },
                new ActivityRequestScienceTyp { ActivityRequestScienceTypText = "Zeitschrift" },
                new ActivityRequestScienceTyp { ActivityRequestScienceTypText = "Handbuch" },
                new ActivityRequestScienceTyp { ActivityRequestScienceTypText = "Sonstiges" }
                );
        }
        public void ActivityRequestScienceAuthors()
        {
            Context.ActivityRequestScienceAuthorNames.AddOrUpdate(a => a.ActivityRequestScienceAuthorText,
                new ActivityRequestScienceAuthorName { ActivityRequestScienceAuthorText = "Autor/Mitautor" },
                new ActivityRequestScienceAuthorName { ActivityRequestScienceAuthorText = "Schriftleitung" },
                new ActivityRequestScienceAuthorName { ActivityRequestScienceAuthorText = "Herausgaber/in" },
                new ActivityRequestScienceAuthorName { ActivityRequestScienceAuthorText = "wissenschaftlicher Beirat" },
                new ActivityRequestScienceAuthorName { ActivityRequestScienceAuthorText = "sonstige Mitwirkung" }
                );
        }
        public void ActivityRequestOrtArten()
        {
            Context.ActivityRequestOrtArten.AddOrUpdate(a => a.ActivityRequestOrtArtText,
                new ActivityRequestOrtArt { ActivityRequestOrtArtText = "Präsenz" },
                new ActivityRequestOrtArt { ActivityRequestOrtArtText = "Online" }
                );
        }
        public void ActivityRequestArbitrationTyps()
        {
            Context.ActivityRequestArbitrationTyps.AddOrUpdate(a => a.ActivityRequestArbitrationTypText,
                new ActivityRequestArbitrationTyp { ActivityRequestArbitrationTypText = "Parteien gemeinsam" },
                new ActivityRequestArbitrationTyp { ActivityRequestArbitrationTypText = "Unbeteiligte Stelle" }
                );
        }
        public void ActivityRequestFrequencies()
        {
            Context.ActivityRequestFrequencies.AddOrUpdate(a => a.ActivityRequestFrequencyText,
                new ActivityRequestFrequency { ActivityRequestFrequencyText = "Einmalig" },
                new ActivityRequestFrequency { ActivityRequestFrequencyText = "Pro Jahr" }
                );
        }
        public void ARVerguetungAdventageTyps()
        {
            Context.ARVerguetungAdventageTyps.AddOrUpdate(a => a.ARVerguetungAdventageTypText,
                new ARVerguetungAdventageTyp { ARVerguetungAdventageTypText = "Essen" },
                new ARVerguetungAdventageTyp { ARVerguetungAdventageTypText = "Getränke" },
                new ARVerguetungAdventageTyp { ARVerguetungAdventageTypText = "Hotelkosten" },
                new ARVerguetungAdventageTyp { ARVerguetungAdventageTypText = "Überschießende Reisekosten" }
                );
        }

    }
}
