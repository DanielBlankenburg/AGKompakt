namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserInitials : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Initials", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Initials");
        }
    }
}
