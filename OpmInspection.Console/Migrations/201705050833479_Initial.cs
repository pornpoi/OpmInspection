namespace OpmInspection.Console.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Backgrounds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(unicode: false),
                        Copyright = c.String(unicode: false),
                        CopyrightLink = c.String(unicode: false),
                        StartedAt = c.DateTime(nullable: false, precision: 0),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)                ;
            
            CreateTable(
                "OfficerStatistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        IpAddress = c.String(unicode: false),
                        Browser = c.String(unicode: false),
                        Platform = c.String(unicode: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)                
                .ForeignKey("Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SkinId = c.Int(nullable: false),
                        PasswordChanged = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(nullable: false, precision: 0),
                        Email = c.String(unicode: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(unicode: false),
                        SecurityStamp = c.String(unicode: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 0),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)                
                .ForeignKey("Skins", t => t.SkinId, cascadeDelete: true)
                .Index(t => t.SkinId);
            
            CreateTable(
                "UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(unicode: false),
                        ClaimValue = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)                
                .ForeignKey("Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "UserLogin",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        LoginProvider = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ProviderKey = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })                
                .ForeignKey("Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "UserRole",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })                
                .ForeignKey("Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(nullable: false, precision: 0),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)                ;
            
            CreateTable(
                "Skins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        ClassName = c.String(unicode: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)                ;
            
            CreateTable(
                "VisitorStatistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SessionID = c.String(unicode: false),
                        IpAddress = c.String(unicode: false),
                        Browser = c.String(unicode: false),
                        Platform = c.String(unicode: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)                ;
            
        }
        
        public override void Down()
        {
            DropForeignKey("OfficerStatistics", "UserId", "Users");
            DropForeignKey("Users", "SkinId", "Skins");
            DropForeignKey("UserRole", "UserId", "Users");
            DropForeignKey("UserRole", "RoleId", "Roles");
            DropForeignKey("UserLogin", "UserId", "Users");
            DropForeignKey("UserClaim", "UserId", "Users");
            DropIndex("UserRole", new[] { "RoleId" });
            DropIndex("UserRole", new[] { "UserId" });
            DropIndex("UserLogin", new[] { "UserId" });
            DropIndex("UserClaim", new[] { "UserId" });
            DropIndex("Users", new[] { "SkinId" });
            DropIndex("OfficerStatistics", new[] { "UserId" });
            DropTable("VisitorStatistics");
            DropTable("Skins");
            DropTable("Roles");
            DropTable("UserRole");
            DropTable("UserLogin");
            DropTable("UserClaim");
            DropTable("Users");
            DropTable("OfficerStatistics");
            DropTable("Backgrounds");
        }
    }
}
