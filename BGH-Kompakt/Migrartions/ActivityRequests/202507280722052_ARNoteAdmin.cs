namespace BGH_Kompakt.Migrartions.ActivityRequests
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ARNoteAdmin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActivityRequests", "ARNoteAdmin", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ActivityRequests", "ARNoteAdmin");
        }
    }
}
