namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompetitionCompleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Competitions", "Completed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Competitions", "Completed");
        }
    }
}
