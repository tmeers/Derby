namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DenSystemFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dens", "InactiveStatus", c => c.Boolean(nullable: false));
            AddColumn("dbo.Dens", "CanCompeteInFinals", c => c.Boolean(nullable: false));
            AddColumn("dbo.Dens", "IsSystemPlaceholder", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dens", "IsSystemPlaceholder");
            DropColumn("dbo.Dens", "CanCompeteInFinals");
            DropColumn("dbo.Dens", "InactiveStatus");
        }
    }
}
