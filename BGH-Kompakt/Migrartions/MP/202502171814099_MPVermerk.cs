namespace BGH_Kompakt.Migrartions.MP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MPVermerk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MPDecisions", "Vermerk", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MPDecisions", "Vermerk");
        }
    }
}
