using BGH_Kompakt.Classes;
using BGH_Kompakt.Classes._LookUp.ActivityRequestLookUps;
using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.ActivityRequestClasses;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BGH_Kompakt.Services
{
    public class ActivityRequestDBContext : DbContext
    {

        //public virtual DbSet<User> Users { get; set; }
        //public virtual DbSet<Geschlecht> Geschlechter{ get; set; }
        //public DbSet<Position> Positions { get; set; }
        //public DbSet<Status> Status { get; set; }
        //public DbSet<Titel> Titel { get; set; }
        //public DbSet<Dienstbezeichnung> enumDienstbezeichnungen { get; set; }
        public DbSet<ActivityClientTyp> ActivityClientTyps { get; set; }
        public DbSet<ActivityClient> ActivityClients { get; set; }
        public DbSet<ActivityRequestTyp> ActivityRequestTyps { get; set; }
        public DbSet<ActivityRequestMeldeArt> ActivityRequestMeldeArten { get; set; }
        public DbSet<ActivityRequestArbitrationClient> ActivityRequestArbitrationClients { get; set; }
        public DbSet<ActivityRequestArbitrationTyp> ActivityRequestArbitrationTyps { get; set; }
        public DbSet<ActivityRequestFrequency> ActivityRequestFrequencies { get; set; }
        public DbSet<ActivityRequestOrtArt> ActivityRequestOrtArten { get; set; }
        public DbSet<ActivityRequestScienceAuthorName> ActivityRequestScienceAuthorNames { get; set; }
        public DbSet<ActivityRequestScienceCategorie> ActivityRequestScienceCategories { get; set; }
        public DbSet<ActivityRequestScienceTyp> ActivityRequestScienceTyps { get; set; }
        public DbSet<ARVerguetungAdventage> ARVerguetungAdventages { get; set; }
        public DbSet<ARVerguetungAdventageTyp> ARVerguetungAdventageTyps { get; set; }
        public virtual DbSet<ActivityRequest> ActivityRequests { get; set; }
        public DbSet<ActivityRequestDataFile> ActivityRequestDataFiles { get; set; }
        public DbSet<ActivityRequestChangeHistory> ActivityRequestChangeHistories { get; set; }
        public DbSet<ActivityRequestStatus> ActivityRequestStatuses { get; set; }
        public DbSet<ActivityRequestConfigurations> ActivityRequestComments { get; set; }

        public ActivityRequestDBContext() :base("name=Main")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            try
            {

                modelBuilder.Entity<ActivityRequestMeldeArt>()
                    .ToTable("ActivityRequestMeldeArten")
                    .Property(x => x.ActivityRequestMeldeArtText).IsRequired();

                modelBuilder.Entity<ActivityRequestOrtArt>()
                    .ToTable("ActivityRequestOrtArten")
                    .Property(x => x.ActivityRequestOrtArtText).IsRequired();
            
                modelBuilder.Entity<ActivityRequestArbitrationClient>()
                    .Property(x => x.ActivityRequestArbitrationClientText).IsRequired();

                modelBuilder.Entity<ActivityRequestScienceAuthorName>()
                    .Property(x => x.ActivityRequestScienceAuthorText).IsRequired();

                modelBuilder.Entity<ActivityRequestScienceCategorie>()
                    .Property(x => x.ActivityRequestScienceCategorieText).IsRequired();

                modelBuilder.Entity<ActivityRequestScienceTyp>()
                    .Property(x => x.ActivityRequestScienceTypText).IsRequired();

                modelBuilder.Entity<ActivityRequestArbitrationTyp>()
                    .Property(x => x.ActivityRequestArbitrationTypText).IsRequired();

                modelBuilder.Entity<ActivityRequestFrequency>()
                    .Property(x => x.ActivityRequestFrequencyText).IsRequired();

                modelBuilder.Entity<ActivityRequestTyp>().Property(x => x.ActivityRequestTypText).IsRequired();
                modelBuilder.Entity<ActivityRequestTyp>().Property(x => x.ActivityRequestTypMeldeArt).IsRequired();

                modelBuilder.Entity<ActivityClientTyp>()
                    .Property(x => x.ActivityClientTypText).IsRequired();

                modelBuilder.Entity<ARVerguetungAdventageTyp>()
                    .Property(x => x.ARVerguetungAdventageTypText).IsRequired();

                modelBuilder.Entity<ARVerguetungAdventage>()
                    .Property(x => x.ARVerguetungAdventageAmount).IsRequired();
                modelBuilder.Entity<ARVerguetungAdventage>()
                    .HasRequired(x => x.ARVerguetungAdventageTyp)
                    .WithMany(a => a.ARVerguetungAdventages)
                    .HasForeignKey(x => x.ARVerguetungAdventageTypId);

                modelBuilder.Configurations.Add(new ActivityClientConfigurations());
                modelBuilder.Configurations.Add(new ActivityRequestConfigurations());
            }
            catch (Exception)
            {
                MessageBox.Show("Zugriff auf die Nebentätigkeiten-DB nicht möglich");
            }
        }
    }
}
