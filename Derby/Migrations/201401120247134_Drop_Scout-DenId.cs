namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Drop_ScoutDenId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Scouts", "DenId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Scouts", "DenId", c => c.Int(nullable: false));
        }
    }
}
