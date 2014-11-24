namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InviteStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PackInvitations", "ResentDate", c => c.DateTime());
            AddColumn("dbo.PackInvitations", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PackInvitations", "Status");
            DropColumn("dbo.PackInvitations", "ResentDate");
        }
    }
}
