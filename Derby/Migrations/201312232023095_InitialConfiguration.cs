namespace Derby.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialConfiguration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Competitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackId = c.Int(nullable: false),
                        Title = c.String(),
                        Location = c.String(),
                        RaceType = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        EventDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Packs", t => t.PackId, cascadeDelete: true)
                .Index(t => t.PackId);
            
            CreateTable(
                "dbo.Dens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PackId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Packs", t => t.PackId, cascadeDelete: true)
                .Index(t => t.PackId);
            
            CreateTable(
                "dbo.Heats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RaceId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Races", t => t.RaceId, cascadeDelete: true)
                .Index(t => t.RaceId);
            
            CreateTable(
                "dbo.Racers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarNumber = c.String(),
                        DenId = c.Int(nullable: false),
                        ScoutId = c.Int(nullable: false),
                        Heat_Id = c.Int(),
                        Race_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Heats", t => t.Heat_Id)
                .ForeignKey("dbo.Races", t => t.Race_Id)
                .Index(t => t.Heat_Id)
                .Index(t => t.Race_Id);
            
            CreateTable(
                "dbo.Packs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Region = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Races",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompetitionId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CompletedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Scouts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Racers", "Race_Id", "dbo.Races");
            DropForeignKey("dbo.Heats", "RaceId", "dbo.Races");
            DropForeignKey("dbo.Dens", "PackId", "dbo.Packs");
            DropForeignKey("dbo.Competitions", "PackId", "dbo.Packs");
            DropForeignKey("dbo.Racers", "Heat_Id", "dbo.Heats");
            DropIndex("dbo.Racers", new[] { "Race_Id" });
            DropIndex("dbo.Heats", new[] { "RaceId" });
            DropIndex("dbo.Dens", new[] { "PackId" });
            DropIndex("dbo.Competitions", new[] { "PackId" });
            DropIndex("dbo.Racers", new[] { "Heat_Id" });
            DropTable("dbo.Scouts");
            DropTable("dbo.Races");
            DropTable("dbo.Packs");
            DropTable("dbo.Racers");
            DropTable("dbo.Heats");
            DropTable("dbo.Dens");
            DropTable("dbo.Competitions");
        }
    }
}
