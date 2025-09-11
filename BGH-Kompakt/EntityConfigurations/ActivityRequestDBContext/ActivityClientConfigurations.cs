using BGH_Kompakt.Classes.ActivityRequestClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.EntityConfigurations
{
    public class ActivityClientConfigurations : EntityTypeConfiguration<ActivityClient>
    {
        public ActivityClientConfigurations()
        {
            Property(x => x.ACName).IsRequired();
            HasRequired(x => x.ActivityClientTyp)
            .WithMany(a => a.activityClients)
            .HasForeignKey(x => x.ActivityClientTypID);

        }
    }
}
