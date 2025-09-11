namespace BGH_Kompakt.Migrartions.MP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EMail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MPEMailRecipients",
                c => new
                    {
                        MPEMailRecipientID = c.Int(nullable: false, identity: true),
                        MPEMailRecipientAdress = c.String(),
                        MPEMailRecipientTyp = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MPEMailRecipientID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MPEMailRecipients");
        }
    }
}
