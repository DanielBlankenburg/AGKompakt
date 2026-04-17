using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Classes.Senate;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.EntityConfigurations.MP
{
    public class MPSeed
    {
        private readonly BGH_Kompakt.Services.DBContexts.MPDBContext Context;

        public MPSeed(BGH_Kompakt.Services.DBContexts.MPDBContext context)
        {
            Context = context;      
        }

        public void MPCategories()
        {
            Context.MPCategories.AddOrUpdate(a => a.MPCategoryText,
                
                new MPCategory { MPCategoryID = 0, MPCategoryText = "unbekannt" },
                new MPCategory { MPCategoryID = 1, MPCategoryText = "Zivilsenat" },
                new MPCategory { MPCategoryID = 2, MPCategoryText = "Strafsenat" },
                new MPCategory { MPCategoryID = 3, MPCategoryText = "Sondersenat" }
                );
        }

        public void MPSenatAbbreviation()
        {
            Context.MPSenateAbbreviation.AddOrUpdate(a => a.MPSenatAbbreviationText, 
                new MPSenatAbbreviation { MPSenatAbbreviationID = 1, MPSenatAbbreviationText="I"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 2, MPSenatAbbreviationText="II"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 3, MPSenatAbbreviationText="III"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 4, MPSenatAbbreviationText="IV"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 5, MPSenatAbbreviationText="V"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 6, MPSenatAbbreviationText="VI"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 7, MPSenatAbbreviationText="VIa"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 8, MPSenatAbbreviationText="VII"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 9, MPSenatAbbreviationText="VIII"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 10, MPSenatAbbreviationText="IX"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 11, MPSenatAbbreviationText="X"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 12, MPSenatAbbreviationText="XI"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 13, MPSenatAbbreviationText="XII"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 14, MPSenatAbbreviationText="XIII"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 15, MPSenatAbbreviationText="1"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 16, MPSenatAbbreviationText="2"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 17, MPSenatAbbreviationText="3"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 18, MPSenatAbbreviationText="4"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 19, MPSenatAbbreviationText="5"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 20, MPSenatAbbreviationText="6"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 21, MPSenatAbbreviationText="KVB" },
                new MPSenatAbbreviation { MPSenatAbbreviationID = 22, MPSenatAbbreviationText="EnVR" },
                new MPSenatAbbreviation { MPSenatAbbreviationID = 23, MPSenatAbbreviationText="StB" },
                new MPSenatAbbreviation { MPSenatAbbreviationID = 24, MPSenatAbbreviationText="AnwZ(Brfg)" },
                new MPSenatAbbreviation { MPSenatAbbreviationID = 25, MPSenatAbbreviationText="AnwStr(B)" },
                new MPSenatAbbreviation { MPSenatAbbreviationID = 26, MPSenatAbbreviationText="LwZR"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 27, MPSenatAbbreviationText="NotZ"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 28, MPSenatAbbreviationText="NotStr"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 29, MPSenatAbbreviationText="AK"},
                new MPSenatAbbreviation { MPSenatAbbreviationID = 30, MPSenatAbbreviationText="Unbekannt"}
                );
        }

        public void MPSenate()
        {
            Context.MPSenate.AddOrUpdate(y => y.MPSenatName,
                new MPSenat { MPSenatID = 1, MPSenatName = "unbekannter Senat", MPCategorieID = 0, MPSenatSorting=1 },
                new MPSenat { MPSenatID = 2, MPSenatName = "I. Zivilsenat", MPCategorieID = 1, MPSenatSorting = 1 },
                new MPSenat { MPSenatID = 3, MPSenatName = "II. Zivilsenat", MPCategorieID = 1, MPSenatSorting = 2 },
                new MPSenat { MPSenatID = 4, MPSenatName = "III. Zivilsenat",MPCategorieID = 1 , MPSenatSorting = 3 },
                new MPSenat { MPSenatID = 5, MPSenatName = "IV. Zivilsenat", MPCategorieID = 1 , MPSenatSorting = 4 },
                new MPSenat { MPSenatID = 6, MPSenatName = "V. Zivilsenat", MPCategorieID = 1 , MPSenatSorting = 5 },
                new MPSenat { MPSenatID = 7, MPSenatName = "VI. Zivilsenat", MPCategorieID = 1 , MPSenatSorting = 6 },
                new MPSenat { MPSenatID = 8, MPSenatName = "VIa. Zivilsenat", MPCategorieID = 1 , MPSenatSorting = 7 },
                new MPSenat { MPSenatID = 9, MPSenatName = "VII. Zivilsenat", MPCategorieID = 1 , MPSenatSorting = 8 },
                new MPSenat { MPSenatID = 10, MPSenatName = "VIII. Zivilsenat", MPCategorieID = 1 , MPSenatSorting = 9 },
                new MPSenat { MPSenatID = 11, MPSenatName = "IX. Zivilsenat", MPCategorieID = 1 , MPSenatSorting = 10 },
                new MPSenat { MPSenatID = 12, MPSenatName = "X. Zivilsenat", MPCategorieID = 1 , MPSenatSorting = 11 },
                new MPSenat { MPSenatID = 13, MPSenatName = "XI. Zivilsenat", MPCategorieID = 1 , MPSenatSorting = 12 },
                new MPSenat { MPSenatID = 14, MPSenatName = "XII. Zivilsenat", MPCategorieID = 1 , MPSenatSorting = 13 },
                new MPSenat { MPSenatID = 15, MPSenatName = "XIII. Zivilsenat", MPCategorieID = 1 , MPSenatSorting = 14 },
                new MPSenat { MPSenatID = 16, MPSenatName = "1. Strafsenat", MPCategorieID = 2, MPSenatSorting = 1 },
                new MPSenat { MPSenatID = 17, MPSenatName = "2. Strafsenat", MPCategorieID = 2, MPSenatSorting = 2 },
                new MPSenat { MPSenatID = 18, MPSenatName = "3. Strafsenat", MPCategorieID = 2 , MPSenatSorting = 3 },
                new MPSenat { MPSenatID = 19, MPSenatName = "4. Strafsenat", MPCategorieID = 2 , MPSenatSorting = 4 },
                new MPSenat { MPSenatID = 20, MPSenatName = "5. Strafsenat", MPCategorieID = 2 , MPSenatSorting = 5 },
                new MPSenat { MPSenatID = 21, MPSenatName = "6. Strafsenat", MPCategorieID = 2 , MPSenatSorting = 6 },
                new MPSenat { MPSenatID = 22, MPSenatName = "Anwaltssenat", MPCategorieID = 3, MPSenatSorting = 4 },
                new MPSenat { MPSenatID = 23, MPSenatName = "Notarsenat", MPCategorieID = 3 , MPSenatSorting = 5 },
                new MPSenat { MPSenatID = 24, MPSenatName = "Landwirtschaftssenat", MPCategorieID = 3 , MPSenatSorting = 10 },
                new MPSenat { MPSenatID = 25, MPSenatName = "Patentanwaltsenat", MPCategorieID = 3 , MPSenatSorting = 6 },
                new MPSenat { MPSenatID = 26, MPSenatName = "Steuerberatersenat", MPCategorieID = 3 , MPSenatSorting = 7 },
                new MPSenat { MPSenatID = 27, MPSenatName = "Gemeinsamer Senat der obersten Gerichtshöfe", MPCategorieID = 3 , MPSenatSorting = 1 },
                new MPSenat { MPSenatID = 28, MPSenatName = "Großer Zivilsenat", MPCategorieID = 3 , MPSenatSorting = 2 },
                new MPSenat { MPSenatID = 29, MPSenatName = "Großer Strafsenat", MPCategorieID = 3 , MPSenatSorting = 3 },
                new MPSenat { MPSenatID = 30, MPSenatName = "Dienstgericht", MPCategorieID = 3 , MPSenatSorting = 8 },
                new MPSenat { MPSenatID = 31, MPSenatName = "Kartellsenat", MPCategorieID = 3 , MPSenatSorting = 9 },
                new MPSenat { MPSenatID = 32, MPSenatName = "Ermittlungsrichter", MPCategorieID = 3 , MPSenatSorting = 7 }

                );
        }

        public void MPWeeks()
        {
            //Context.MPWeeks.AddOrUpdate(a => a.MPWeekYear,
            //    new MPWeek { MPWeekID=1, MPWeekYear=2024, MPWeekNumber=31}
            //    );
        }

        public void MPDecisions()
        {
            //Context.MPDecisions.AddOrUpdate(a => a.Rohdaten,
            //    new MPDecision { MPDecisionID=1, Rohdaten = "Title", Date = DateTime.Now, Typ = "Urteil", Aktenzeichen = "I ZB 34/24", SenatID = 2, RegZeichen = "ZB", LaufendeNummer = "34", Jahr = "23", FileName_Fullpath= "I_ZB__34-23", PathName= "C:\\Montagspost\\2024\\KW31\\Zivilsenate\\I. Zivilsenat", MPWeekID=1 });
        }
    }
}
