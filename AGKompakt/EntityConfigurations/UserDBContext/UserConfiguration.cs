using BGH_Kompakt.Classes;
using BGH_Kompakt.Classes.UserClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.EntityConfigurations
{
    public class UserConfiguration : EntityTypeConfiguration<User> 
    {
        public UserConfiguration() 
        {
            Property(x => x.VorName).IsRequired();
            Property(x => x.NachName).IsRequired();
            Property(x => x.EMail).IsRequired();
            Property(x => x.ComputerName).IsRequired();
            
            HasOptional(x => x.Dienstbezeichnung)
            .WithMany(a => a.Users)
            .HasForeignKey(x => x.DienstbezeichnungId);

            HasRequired(x => x.Geschlecht)
            .WithMany(a => a.Users)
            .HasForeignKey(x => x.GeschlechtID);

            HasRequired(x => x.Position)
            .WithMany(a => a.Users)
            .HasForeignKey(x => x.PositionId);

            HasRequired(x => x.Status)
            .WithMany(a => a.Users)
            .HasForeignKey(x => x.StatusId);
            
            HasOptional(x => x.Titel)
            .WithMany(a => a.Users)
            .HasForeignKey(x => x.TitelId);

            HasRequired(x => x.FilterMP)
            .WithRequiredPrincipal(x => x.User);

            HasMany(x => x.Senate)
            .WithMany(x => x.Users)
            .Map(m => m.ToTable("Senatsmitglieder"));

            HasMany(x => x.AdminStatus)
            .WithMany(x => x.Users)
            .Map(m => m.ToTable("Rollen"));

            HasMany(x => x.SenateAdmin)
            .WithMany(x => x.AdminUsers)
            .Map(m => m.ToTable("Senatsadministratoren"));

            HasMany(x => x.Spruchgruppen)
            .WithMany(x => x.Members)
            .Map(m => m.ToTable("Spruchgruppenmitglieder"));

            HasMany(x => x.UserDienstbezeichnungen)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}
