using BGH_Kompakt.Classes._LookUp.MP;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.EntityConfigurations.MP
{
    public class MPSenatConfiguration : EntityTypeConfiguration<MPSenat>
    {
        public MPSenatConfiguration()
        {
            ToTable("MPSenate");
            Property(x => x.MPSenatName).IsRequired();

            HasRequired(x => x.MPCategory)
                .WithMany(x => x.MPSenate)
                .HasForeignKey(x => x.MPCategorieID);

            HasMany(x => x.MPSenatAbbreviations)
                 .WithMany(x => x.Senate);

        }
    }
}
