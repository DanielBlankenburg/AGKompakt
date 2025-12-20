using BGH_Kompakt.Classes;
using BGH_Kompakt.Classes.ActivityRequestClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.EntityConfigurations
{
    public class ActivityRequestConfigurations : EntityTypeConfiguration<ActivityRequest>
    {
        public ActivityRequestConfigurations()
        {
            Property(x => x.ARDatum)
                .IsRequired()
                .HasColumnType("datetime2");

            Property(x => x.ARTitel)
            .IsRequired()
            .HasMaxLength(255);
            Property(x => x.ARVerguetung).IsRequired();
            Property(x => x.ARZeitaufwandMain).IsRequired();
            Property(x => x.ARZustaendigkeitsbereich).IsRequired();

            HasOptional(x => x.ActivityRequestTyp)
            .WithMany(a => a.ActivityRequests)
            .HasForeignKey(x => x.ActivityRequestTypID);

            HasRequired(x => x.ActivityRequestMeldeArt)
            .WithMany(a => a.ActivityRequests)
            .HasForeignKey(x => x.ActivityRequestMeldeArtID);

            HasOptional(x => x.ActivityClient)
            .WithMany(a => a.ActivityRequests)
            .HasForeignKey(x => x.ActivityClientID);

            HasOptional(x => x.ActivityRequestScienceTyp)
            .WithMany(a => a.ActivityRequests)
            .HasForeignKey(x => x.ActivityRequestScienceTypId);

            HasOptional(x => x.ActivityRequestScienceCategorie)
            .WithMany(a => a.ActivityRequests)
            .HasForeignKey(x => x.ActivityRequestScienceCategorieId);

            //HasOptional(x => x.ActivityRequestScienceAuthor)
            //.WithMany(a => a.ActivityRequests)
            //.HasForeignKey(x => x.ActivityRequestScienceAuthorId);

            HasOptional(x => x.ActivityRequestOrtArt)
            .WithMany(a => a.ActivityRequests)
            .HasForeignKey(x => x.ActivityRequestOrtArtId);

            HasOptional(x => x.ActivityRequestFrequency)
            .WithMany(a => a.ActivityRequests)
            .HasForeignKey(x => x.ActivityRequestFrequencyId);

            HasOptional(x => x.ActivityRequestArbitrationTyp)
            .WithMany(a => a.ActivityRequests)
            .HasForeignKey(x => x.ActivityRequestArbitrationTypId);

            HasMany(x => x.ActivityRequestDataFiles)
                .WithRequired(x => x.ActivityRequest)
                .HasForeignKey(x => x.ActivityRequestId);

            HasMany(x => x.ActivityRequestChangeHistories)
                .WithRequired(x => x.ActivityRequest)
                .HasForeignKey(x => x.ActivityRequestId);

            HasMany(x => x.ActivityRequestComments)
                .WithRequired(x => x.ActivityRequest)
                .HasForeignKey(x => x.ActivityRequestID);

            HasMany(x => x.ActivityRequestStatusHistories)
                .WithRequired(x => x.ActivityRequest)
                .HasForeignKey(x => x.ActivityRequestID);

            Property(x => x.ARActivityDate).HasColumnType("datetime2");
            Property(x => x.ActivityRequestDatePermanentFrom).HasColumnType("datetime2");
            Property(x => x.ActivityRequestDatePermanentUntil).HasColumnType("datetime2");

        }
    }
}
