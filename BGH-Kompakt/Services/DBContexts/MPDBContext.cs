using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Classes.Test;
using BGH_Kompakt.EntityConfigurations.MP;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BGH_Kompakt.Services.DBContexts
{
    public class MPDBContext : DbContext
    {
        public DbSet<MPDecision> MPDecisions { get; set; }
        public DbSet<MPCategory> MPCategories { get; set; }
        public DbSet<MPWeek> MPWeeks { get; set; }
        public DbSet<MPSenat> MPSenate { get; set; }
        public DbSet<MPSenatAbbreviation> MPSenateAbbreviation { get; set; }
        public DbSet<MPUserDecision> MPUserDecisions { get; set; }
        //public DbSet<MPBE> MPBE { get; set; }
        public DbSet<MPEMailRecipient> MPEMailRecipients { get; set; }
        public DbSet<MPEMail> MPEMails { get; set; }
        public DbSet<MPSetting> MPSettings { get; set; }
        public DbSet<MPVermerk> MPVermerke { get; set; }
        //public DbSet<FileData> FileDatas { get; set; }



        public MPDBContext() : base("name=MP")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Entity<MPCategory>().Property(x => x.MPCategoryText).IsRequired();

                //modelBuilder.Entity<MPSenat>().Property(x => x.MPSenatName).IsRequired();
                
                modelBuilder.Entity<MPWeek>().Property(x => x.MPWeekYear).IsRequired();
                modelBuilder.Entity<MPWeek>().Property(x => x.MPWeekNumber).IsRequired();

                modelBuilder.Entity<MPSenatAbbreviation>().Property(x => x.MPSenatAbbreviationText).IsRequired();

                //modelBuilder.Entity<MPBE>().Property(x => x.MPBEName).IsRequired();
                //modelBuilder.Entity<MPBE>().ToTable("MPBE");

                modelBuilder.Entity<MPEMail>().Property(x => x.MPEMailDescription).IsRequired();

                modelBuilder.Entity<MPVermerk>().Property(x => x.MPVermerkText).IsRequired();
                modelBuilder.Entity<MPVermerk>().ToTable("MPVermerke");

                modelBuilder.Configurations.Add(new MPConfiguration());
                modelBuilder.Configurations.Add(new MPSenatConfiguration());

            }
            catch (Exception)
            {
                MessageBox.Show("Zugriff auf die Datenbank Montagspost nicht möglich");
            }
        }
    }
}
