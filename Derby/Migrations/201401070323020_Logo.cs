namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Logo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dens", "LogoPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dens", "LogoPath");
        }
    }
}
