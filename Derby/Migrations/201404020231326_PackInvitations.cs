namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PackInvitations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PackInvitations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        ExpiryOffset = c.Int(nullable: false),
                        Code = c.String(maxLength: 25),
                        InvitedEmail = c.String(),
                        Accepted = c.Boolean(nullable: false),
                        AcceptedUserId = c.String(),
                        InvitedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.InvitedBy_Id)
                .Index(t => t.Code, unique: true, name: "InvitationCode")
                .Index(t => t.InvitedBy_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PackInvitations", "InvitedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PackInvitations", new[] { "InvitedBy_Id" });
            DropIndex("dbo.PackInvitations", "InvitationCode");
            DropTable("dbo.PackInvitations");
        }
    }
}
