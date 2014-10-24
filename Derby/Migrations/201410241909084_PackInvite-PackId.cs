namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PackInvitePackId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PackInvitations", "Pack_Id", c => c.Int());
            CreateIndex("dbo.PackInvitations", "Pack_Id");
            AddForeignKey("dbo.PackInvitations", "Pack_Id", "dbo.Packs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PackInvitations", "Pack_Id", "dbo.Packs");
            DropIndex("dbo.PackInvitations", new[] { "Pack_Id" });
            DropColumn("dbo.PackInvitations", "Pack_Id");
        }
    }
}
