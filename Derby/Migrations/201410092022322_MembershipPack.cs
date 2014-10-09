namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MembershipPack : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PackMemberships", name: "UserId", newName: "User_Id");
            RenameIndex(table: "dbo.PackMemberships", name: "IX_UserId", newName: "IX_User_Id");
            AddColumn("dbo.PackMemberships", "Pack_Id", c => c.Int());
            CreateIndex("dbo.PackMemberships", "Pack_Id");
            AddForeignKey("dbo.PackMemberships", "Pack_Id", "dbo.Packs", "Id");
            DropColumn("dbo.PackMemberships", "PackId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PackMemberships", "PackId", c => c.Int(nullable: false));
            DropForeignKey("dbo.PackMemberships", "Pack_Id", "dbo.Packs");
            DropIndex("dbo.PackMemberships", new[] { "Pack_Id" });
            DropColumn("dbo.PackMemberships", "Pack_Id");
            RenameIndex(table: "dbo.PackMemberships", name: "IX_User_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.PackMemberships", name: "User_Id", newName: "UserId");
        }
    }
}
