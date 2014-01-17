namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RaceDenId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Races", "DenId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Races", "DenId");
        }
    }
}
