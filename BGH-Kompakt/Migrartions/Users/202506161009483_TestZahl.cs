namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestZahl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Testzahl", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Testzahl");
        }
    }
}
