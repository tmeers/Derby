namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomethingSomethingPack : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Packs", "PackInvitation_Id", "dbo.PackInvitations");
            DropIndex("dbo.Packs", new[] { "PackInvitation_Id" });
            DropColumn("dbo.Packs", "PackInvitation_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Packs", "PackInvitation_Id", c => c.Int());
            CreateIndex("dbo.Packs", "PackInvitation_Id");
            AddForeignKey("dbo.Packs", "PackInvitation_Id", "dbo.PackInvitations", "Id");
        }
    }
}
