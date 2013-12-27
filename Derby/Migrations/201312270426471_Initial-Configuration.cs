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
                "dbo.Packs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Region = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.Scouts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Inactive = c.Boolean(nullable: false),
                        PackId = c.Int(nullable: false),
                        DenId = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Packs", t => t.PackId, cascadeDelete: true)
                .Index(t => t.PackId);
            
            CreateTable(
                "dbo.Contestants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RacerId = c.Int(nullable: false),
                        HeatId = c.Int(nullable: false),
                        Place = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Heats", t => t.HeatId, cascadeDelete: true)
                .Index(t => t.HeatId);
            
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
                        Race_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Races", t => t.Race_Id)
                .Index(t => t.Race_Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Racers", "Race_Id", "dbo.Races");
            DropForeignKey("dbo.Heats", "RaceId", "dbo.Races");
            DropForeignKey("dbo.Contestants", "HeatId", "dbo.Heats");
            DropForeignKey("dbo.Scouts", "PackId", "dbo.Packs");
            DropForeignKey("dbo.Dens", "PackId", "dbo.Packs");
            DropForeignKey("dbo.Competitions", "PackId", "dbo.Packs");
            DropIndex("dbo.Racers", new[] { "Race_Id" });
            DropIndex("dbo.Heats", new[] { "RaceId" });
            DropIndex("dbo.Contestants", new[] { "HeatId" });
            DropIndex("dbo.Scouts", new[] { "PackId" });
            DropIndex("dbo.Dens", new[] { "PackId" });
            DropIndex("dbo.Competitions", new[] { "PackId" });
            DropTable("dbo.Races");
            DropTable("dbo.Racers");
            DropTable("dbo.Heats");
            DropTable("dbo.Contestants");
            DropTable("dbo.Scouts");
            DropTable("dbo.Dens");
            DropTable("dbo.Packs");
            DropTable("dbo.Competitions");
        }
    }
}
