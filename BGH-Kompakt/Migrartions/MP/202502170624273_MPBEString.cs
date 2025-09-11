namespace BGH_Kompakt.Migrartions.MP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MPBEString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MPBE", "MPBEName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MPBE", "MPBEName", c => c.Int(nullable: false));
        }
    }
}
