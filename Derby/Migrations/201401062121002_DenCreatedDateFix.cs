namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DenCreatedDateFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dens", "CreatedDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Dens", "CreateDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dens", "CreateDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Dens", "CreatedDateTime");
        }
    }
}
