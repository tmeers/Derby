namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserMembership : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PackMemberships", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.PackMemberships", "UserId");
            AddForeignKey("dbo.PackMemberships", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PackMemberships", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.PackMemberships", new[] { "UserId" });
            AlterColumn("dbo.PackMemberships", "UserId", c => c.String());
        }
    }
}
