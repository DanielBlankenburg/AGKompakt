namespace BGH_Kompakt.Migrartions.Users
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Verfahrensbeistaende", "Strasse", c => c.String());
            AddColumn("dbo.Verfahrensbeistaende", "Hausnummer", c => c.String());
            AddColumn("dbo.Verfahrensbeistaende", "PLZ", c => c.Int(nullable: false));
            AddColumn("dbo.Verfahrensbeistaende", "Stadt", c => c.String());
            AddColumn("dbo.Verfahrensbeistaende", "Telefon", c => c.String());
            AddColumn("dbo.Verfahrensbeistaende", "Mobiltelefon", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Verfahrensbeistaende", "Mobiltelefon");
            DropColumn("dbo.Verfahrensbeistaende", "Telefon");
            DropColumn("dbo.Verfahrensbeistaende", "Stadt");
            DropColumn("dbo.Verfahrensbeistaende", "PLZ");
            DropColumn("dbo.Verfahrensbeistaende", "Hausnummer");
            DropColumn("dbo.Verfahrensbeistaende", "Strasse");
        }
    }
}
