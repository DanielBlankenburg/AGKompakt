namespace BGH_Kompakt.Migrartions.Users
{
    using BGH_Kompakt.EntityConfigurations.UserDBContext;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class ConfigurationUser : DbMigrationsConfiguration<BGH_Kompakt.Services.DBContexts.UserDBContext>
    {
        public ConfigurationUser()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = @"Migrartions\Users";
        }

        protected override void Seed(BGH_Kompakt.Services.DBContexts.UserDBContext context)
        {
            //UserSeeder Seed = new UserSeeder(context);
            //Seed.enumDienstbezeichnungen();
            //Seed.Geschlechter();
            //Seed.Posítions();
            //Seed.Status();
            //Seed.Titel();
            //Seed.Senate();
            //Seed.EnumAdminStatus();
            //Seed.Users();
        }
    }
}
