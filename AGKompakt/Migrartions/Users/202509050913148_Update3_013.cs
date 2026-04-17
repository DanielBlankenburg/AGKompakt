namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update3_013 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProgrammSettings", "MontagspostActivated", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProgrammSettings", "ActivityRequestActivated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProgrammSettings", "ActivityRequestActivated");
            DropColumn("dbo.ProgrammSettings", "MontagspostActivated");
        }
    }
}
