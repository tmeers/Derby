namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Packs", "CreateDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Dens", "CreateDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Contestants", "Lane", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contestants", "Lane");
            DropColumn("dbo.Dens", "CreateDateTime");
            DropColumn("dbo.Packs", "CreateDateTime");
        }
    }
}
