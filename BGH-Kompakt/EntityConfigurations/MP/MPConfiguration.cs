using BGH_Kompakt.Classes.MP;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.EntityConfigurations.MP
{
    public class MPConfiguration : EntityTypeConfiguration<MPDecision>
    {
        public MPConfiguration() 
        {
            Property(x => x.Rohdaten).IsRequired();
            Property(x => x.Date).IsRequired();
            Property(x => x.Typ).IsRequired();
            Property(x => x.Aktenzeichen).IsRequired();
            Property(x => x.RegZeichen).IsRequired();
            Property(x => x.LaufendeNummer).IsRequired();
            Property(x => x.PathName).IsRequired();
            Property(x => x.FileName).IsRequired();
            Property(x => x.Jahr).IsRequired();

            HasRequired(x => x.Senat)
                .WithMany(x => x.MPDecisions)
                .HasForeignKey(x => x.SenatID);

            HasRequired(x => x.MPWeek)
                .WithMany(x => x.MPDecisions)
                .HasForeignKey (x => x.MPWeekID);
        }
    }
}
