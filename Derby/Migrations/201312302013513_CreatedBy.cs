namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedBy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Competitions", "CreatedById", c => c.String());
            AddColumn("dbo.Packs", "CreatedById", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Packs", "CreatedById");
            DropColumn("dbo.Competitions", "CreatedById");
        }
    }
}
