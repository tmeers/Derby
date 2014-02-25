namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegLock_HeatComplete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Competitions", "RegistrationLocked", c => c.Boolean(nullable: false));
            AddColumn("dbo.Heats", "IsCompleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Heats", "IsCompleted");
            DropColumn("dbo.Competitions", "RegistrationLocked");
        }
    }
}
