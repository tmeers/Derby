namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ownership : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PackMemberships",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PackId = c.Int(nullable: false),
                    UserId = c.String(),
                    AccessLevel = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Packs", t => t.PackId, cascadeDelete: true);
            //.ForeignKey("dbo.AspNetUsers", t => t.UserId, );

        }
        
        public override void Down()
        {
            DropTable("dbo.PackMemberships");
        }
    }
}
