namespace BGH_Kompakt.Migrartions.MP
{
    using BGH_Kompakt.EntityConfigurations.MP;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class ConfigurationMP : DbMigrationsConfiguration<BGH_Kompakt.Services.DBContexts.MPDBContext>
    {
        public ConfigurationMP()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Migrartions\MP";
        }

        protected override void Seed(BGH_Kompakt.Services.DBContexts.MPDBContext context)
        {
            //MPSeed Seed = new MPSeed(context);
            //Seed.EnumMPCategories();
            //Seed.MPSenate();
            //Seed.MPSenatAbbreviation();
            //Seed.MPWeeks();
            //Seed.MPDecisions();
        }
    }
}
