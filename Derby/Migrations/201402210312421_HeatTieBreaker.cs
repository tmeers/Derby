namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HeatTieBreaker : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Heats", "TieBreaker", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Heats", "TieBreaker");
        }
    }
}
