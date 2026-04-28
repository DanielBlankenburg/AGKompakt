using BGH_Kompakt.Classes.UserClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.EntityConfigurations.UserDBContext
{
    public class VerfahrensbeistandConfiguration : EntityTypeConfiguration<Verfahrensbeistand>
    {
        public VerfahrensbeistandConfiguration()
        {
            ToTable("Verfahrensbeistaende");

            Property(x => x.VorName).IsRequired();
            Property(x => x.NachName).IsRequired();
            Property(x => x.EMail).IsRequired();

            HasRequired(x => x.Geschlecht)
            .WithMany(a => a.Verfahrensbeistaende)
            .HasForeignKey(x => x.GeschlechtID);

            HasOptional(x => x.Titel)
            .WithMany(a => a.Verfahrensbeistaende)
            .HasForeignKey(x => x.TitelId);

        }
    }
}
