namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RacerAlignment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Racers", "CompetitionId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Racers", "CompetitionId");
        }
    }
}
