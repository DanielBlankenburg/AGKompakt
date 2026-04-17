namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SortingMP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserFilterMP", "AscSorting", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserFilterMP", "AscSorting");
        }
    }
}
