using BGH_Kompakt.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BGH_Kompakt.EntityConfigurations;
using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Classes.Sitzungsunterlagen;
using System.Windows;
using BGH_Kompakt.Migrartions.Users;
using BGH_Kompakt.Classes.SystemSettings;
using BGH_Kompakt.Classes.Helper;

namespace BGH_Kompakt.Services.DBContexts
{
    public class UserDBContext : DbContext
    {

        public  DbSet<User> Users { get; set; }
        public  DbSet<Geschlecht> Geschlechter { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Titel> Titel { get; set; }
        public DbSet<Dienstbezeichnung> Dienstbezeichnungen { get; set; }
        public DbSet<Senat> Senate { get; set; }
        public DbSet<UserFilterMP> FilterMP { get; set; }
        public DbSet<AdminStatus> AdminStatus { get; set; }
        public DbSet<SenatSetting> SenatSettings { get; set; }
        public DbSet<SenatAktenzeichen> SenatAktenzeichen { get; set; }
        public DbSet<SenatSpruchgruppe> SenatSpruchgruppen { get; set; }
        public DbSet<VerfahrenVotenmappe> Votenmappe { get; set; }
        public DbSet<ProgrammSetting> ProgrammSettings { get; set; }
        public DbSet<UserDienstbezeichnung> UserDienstbezeichnungen { get; set; }
        public DbSet<RBesoldung> RBesoldungen { get; set; }
        public DbSet<RBesoldungPayment> RBesoldungPayments{ get; set; }
        



        public UserDBContext() : base("name=User") 
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try
            {

                modelBuilder.Entity<Dienstbezeichnung>()
                    .ToTable("Dienstbezeichnungen")
                    .Property(x => x.DienstbezeichnungText).IsRequired();

                modelBuilder.Entity<Dienstbezeichnung>()
                    .HasRequired(x => x.Besoldungsgruppe)
                    .WithMany(x => x.Dienstbezeichnungen);

                modelBuilder.Entity<Geschlecht>()
                    .ToTable("Geschlechter")
                    .Property(x => x.GeschlechtText).IsRequired();

                modelBuilder.Entity<Position>()
                    .Property(x => x.PositionText).IsRequired();

                modelBuilder.Entity<Status>()
                    .Property(x => x.StatusText).IsRequired();

                modelBuilder.Entity<Titel>()
                    .Property(x => x.TitelText).IsRequired();

                modelBuilder.Entity<Senat>()
                    .ToTable("Senate")
                    .HasRequired(x => x.Senatsetting)
                    .WithRequiredPrincipal(x => x.Senat);

                modelBuilder.Entity<UserFilterMP>()
                    .ToTable("UserFilterMP");

                modelBuilder.Entity<AdminStatus>()
                    .ToTable("AdminStatus")
                    .Property(x => x.AdminStatusText).IsRequired();

                modelBuilder.Entity<SenatAktenzeichen>()
                    .ToTable("SenatAktenzeichen")
                    .HasRequired(x => x.SenatSetting)
                    .WithMany(x => x.Aktenzeichen);

                modelBuilder.Entity<SenatSpruchgruppe>()
                    .ToTable("SenatSpruchgruppen")
                    .HasRequired(x => x.SenatSetting)
                    .WithMany(x => x.Spruchgruppen);

                modelBuilder.Entity<VerfahrenVotenmappe>()
                    .ToTable("Votenmappe");

                modelBuilder.Entity<UserDienstbezeichnung>()
                    .ToTable("UserDienstbezeichnungen")
                    .HasRequired(x => x.Dienstbezeichnung)
                    .WithMany(x => x.UserDienstbezeichnungen);

                modelBuilder.Entity<RBesoldung>()
                    .ToTable("RBesoldungen");

                modelBuilder.Entity<RBesoldungPayment>()
                    .HasRequired(x => x.RBesoldung)
                    .WithMany(x => x.RBesoldungPayments);

                modelBuilder.Configurations.Add(new UserConfiguration());
                //base.OnModelCreating(modelBuilder);
            }
            catch (Exception) { ErrorMessage.CreateExceptionWithFlyOutMessage("Fehler beim Zugriff auf die Datenbank MontagspostUser", new Exception("Fehler beim Zugriff auf die Datenbank MontagspostUser"));}
        }
    }
}
