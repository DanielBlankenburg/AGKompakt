namespace BGH_Kompakt.Migrartions.MP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MPRedakteureEMailNotice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MPDecisions", "BEEMail", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MPDecisions", "BEEMail");
        }
    }
}
