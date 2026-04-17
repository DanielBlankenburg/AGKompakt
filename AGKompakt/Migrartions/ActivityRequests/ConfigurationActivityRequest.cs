namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using BGH_Kompakt.EntityConfigurations.ActivityRequestDBContext;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class ConfigurationActivityRequest : DbMigrationsConfiguration<BGH_Kompakt.Services.ActivityRequestDBContext>
    {
        public ConfigurationActivityRequest()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrartions\ActivityRequests";
        }

        protected override void Seed(BGH_Kompakt.Services.ActivityRequestDBContext context)
        {
            //ActivityRequestSeeder Seed = new ActivityRequestSeeder(context);
            //Seed.ActivityClientTyps();
            //Seed.ActivityRequestMeldeArten();
            //Seed.ActivityRequestTyps();
            //Seed.ActivityRequestScienceCategories();
            //Seed.ActivityRequestScienceAuthors();
            //Seed.ActivityRequestScienceTyps();
            //Seed.ActivityRequestOrtArten();
            //Seed.ActivityRequestArbitrationTyps();
            //Seed.ActivityRequestFrequencies();
            //Seed.ARVerguetungAdventageTyps();
        }
    }
}
