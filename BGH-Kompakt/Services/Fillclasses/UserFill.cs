using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Services.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Services.Fillclasses
{
    public static class UserFill
    {
        private static readonly UserDBContext context = new UserDBContext();
        public static void SenatSettingFill()
        {
            var Senate = context.Senate.ToArray();

            foreach (Senat senat in Senate)
            {
                //if (senat.SenatArt == 1)
                //{
                SenatSetting NewSetting = context.SenatSettings.FirstOrDefault(x => x.SenatID == senat.SenatID);
                if (NewSetting == null)
                {
                    NewSetting = new SenatSetting
                    {
                        SenatID = senat.SenatID,
                        Senat = senat,
                        ShowSitzungsplaene = false,
                        ShowVerteilung = false,
                        ShowVotenmappe = false,
                        ShowFormerDays = true,
                        AZPrefix = true,
                        AZPrefixDate = false,
                        ImportAGUrteil = "AG Urteil",
                        ImportAGBeschluss = "AG Beschluss",
                        ImportLGUrteil = "LG Urteil",
                        ImportLGBeschluss = "LG Beschluss",
                        ImportLGHB = "LG HB",
                        ImportLGZB = "LG ZB",
                        ImportOLGUrteil = "OLG Urteil",
                        ImportOLGBeschluss = "OLG Beschluss",
                        ImportOLGHB = "OLG HB",
                        ImportOLGZB = "OLG ZB",
                        ImportEUGHVorlage = "EuGH Vorlage",
                        ImportEUGHURteil = "EUGH Urteil",
                        ImportEntwurf = "Entscheidungsentwurf",
                        ImportVotum = "Votum",
                        ImportVorVotum = "Vorvotum",
                        ImportAnlage = "Anlage",
                        ImportLeitsatz = "L-Karte",
                        ImportRMB = "Rechtsmittelbegründung",
                        ImportRME = "Rechtsmittelerwiderung",
                        ImportSonstiges = "Sonstiges"
                    };
                    context.SenatSettings.Add(NewSetting);
                }
            }
            //}
            context.SaveChanges();

        }
    }
}
