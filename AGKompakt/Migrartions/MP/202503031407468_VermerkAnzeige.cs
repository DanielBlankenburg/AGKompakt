namespace BGH_Kompakt.Migrartions.MP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VermerkAnzeige : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MPDecisions", "VermerkAnzeige", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MPDecisions", "VermerkAnzeige");
        }
    }
}
