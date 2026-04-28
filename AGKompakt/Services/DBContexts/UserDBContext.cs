using System;
using System.Data.Entity;
using BGH_Kompakt.EntityConfigurations;
using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Classes.SystemSettings;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.EntityConfigurations.UserDBContext;

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
        public DbSet<AdminStatus> AdminStatus { get; set; }
        public DbSet<ProgrammSetting> ProgrammSettings { get; set; }
        public DbSet<UserDienstbezeichnung> UserDienstbezeichnungen { get; set; }
        public DbSet<Verfahrensbeistand> Verfahrensbeistaende{ get; set; }
       



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

                modelBuilder.Entity<Geschlecht>()
                    .ToTable("Geschlechter")
                    .Property(x => x.GeschlechtText).IsRequired();

                modelBuilder.Entity<Position>()
                    .Property(x => x.PositionText).IsRequired();

                modelBuilder.Entity<Status>()
                    .Property(x => x.StatusText).IsRequired();

                modelBuilder.Entity<Titel>()
                    .Property(x => x.TitelText).IsRequired();

                modelBuilder.Entity<AdminStatus>()
                    .ToTable("AdminStatus")
                    .Property(x => x.AdminStatusText).IsRequired();



                modelBuilder.Entity<UserDienstbezeichnung>()
                    .ToTable("UserDienstbezeichnungen")
                    .HasRequired(x => x.Dienstbezeichnung)
                    .WithMany(x => x.UserDienstbezeichnungen);

                modelBuilder.Configurations.Add(new UserConfiguration());
                modelBuilder.Configurations.Add(new VerfahrensbeistandConfiguration());
                //base.OnModelCreating(modelBuilder);
            }
            catch (Exception) { ErrorMessage.CreateExceptionWithFlyOutMessage("Fehler beim Zugriff auf die Datenbank MontagspostUser", new Exception("Fehler beim Zugriff auf die Datenbank MontagspostUser"));}
        }
    }
}
