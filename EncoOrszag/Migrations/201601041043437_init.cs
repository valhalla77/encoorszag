namespace EncoOrszag.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Egysegs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Tamadas = c.Int(nullable: false),
                        Vedekezes = c.Int(nullable: false),
                        Ar = c.Int(nullable: false),
                        Zsold = c.Int(nullable: false),
                        Ellatmany = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HadseregEgysegs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Darab = c.Int(nullable: false),
                        Egyseg_Id = c.Int(),
                        Hadsereg_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Egysegs", t => t.Egyseg_Id)
                .ForeignKey("dbo.Hadseregs", t => t.Hadsereg_Id)
                .Index(t => t.Egyseg_Id)
                .Index(t => t.Hadsereg_Id);
            
            CreateTable(
                "dbo.Hadseregs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CelOrszag_Id = c.Int(),
                        SajatOrszag_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orszags", t => t.CelOrszag_Id)
                .ForeignKey("dbo.Orszags", t => t.SajatOrszag_Id)
                .Index(t => t.CelOrszag_Id)
                .Index(t => t.SajatOrszag_Id);
            
            CreateTable(
                "dbo.Orszags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Pontszam = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.OrszagEpulets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Darab = c.Int(nullable: false),
                        Epulet_Id = c.Int(),
                        Orszag_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Epulets", t => t.Epulet_Id)
                .ForeignKey("dbo.Orszags", t => t.Orszag_Id)
                .Index(t => t.Epulet_Id)
                .Index(t => t.Orszag_Id);
            
            CreateTable(
                "dbo.Epulets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Ido = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrszagEpuletKeszuls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Hatravan = c.Int(nullable: false),
                        Epulet_Id = c.Int(),
                        Orszag_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Epulets", t => t.Epulet_Id)
                .ForeignKey("dbo.Orszags", t => t.Orszag_Id)
                .Index(t => t.Epulet_Id)
                .Index(t => t.Orszag_Id);
            
            CreateTable(
                "dbo.OrszagFejlesztes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fejlesztes_Id = c.Int(),
                        Orszag_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fejlesztes", t => t.Fejlesztes_Id)
                .ForeignKey("dbo.Orszags", t => t.Orszag_Id)
                .Index(t => t.Fejlesztes_Id)
                .Index(t => t.Orszag_Id);
            
            CreateTable(
                "dbo.Fejlesztes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Ido = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrszagFejlesztesKeszuls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Hatravan = c.Int(nullable: false),
                        Fejlesztes_Id = c.Int(),
                        Orszag_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fejlesztes", t => t.Fejlesztes_Id)
                .ForeignKey("dbo.Orszags", t => t.Orszag_Id)
                .Index(t => t.Fejlesztes_Id)
                .Index(t => t.Orszag_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.OrszagEgysegs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Darab = c.Int(nullable: false),
                        Egyseg_Id = c.Int(),
                        Orszag_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Egysegs", t => t.Egyseg_Id)
                .ForeignKey("dbo.Orszags", t => t.Orszag_Id)
                .Index(t => t.Egyseg_Id)
                .Index(t => t.Orszag_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OrszagEgysegs", "Orszag_Id", "dbo.Orszags");
            DropForeignKey("dbo.OrszagEgysegs", "Egyseg_Id", "dbo.Egysegs");
            DropForeignKey("dbo.Hadseregs", "SajatOrszag_Id", "dbo.Orszags");
            DropForeignKey("dbo.HadseregEgysegs", "Hadsereg_Id", "dbo.Hadseregs");
            DropForeignKey("dbo.Hadseregs", "CelOrszag_Id", "dbo.Orszags");
            DropForeignKey("dbo.Orszags", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrszagFejlesztes", "Orszag_Id", "dbo.Orszags");
            DropForeignKey("dbo.OrszagFejlesztesKeszuls", "Orszag_Id", "dbo.Orszags");
            DropForeignKey("dbo.OrszagFejlesztesKeszuls", "Fejlesztes_Id", "dbo.Fejlesztes");
            DropForeignKey("dbo.OrszagFejlesztes", "Fejlesztes_Id", "dbo.Fejlesztes");
            DropForeignKey("dbo.OrszagEpulets", "Orszag_Id", "dbo.Orszags");
            DropForeignKey("dbo.OrszagEpuletKeszuls", "Orszag_Id", "dbo.Orszags");
            DropForeignKey("dbo.OrszagEpuletKeszuls", "Epulet_Id", "dbo.Epulets");
            DropForeignKey("dbo.OrszagEpulets", "Epulet_Id", "dbo.Epulets");
            DropForeignKey("dbo.HadseregEgysegs", "Egyseg_Id", "dbo.Egysegs");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.OrszagEgysegs", new[] { "Orszag_Id" });
            DropIndex("dbo.OrszagEgysegs", new[] { "Egyseg_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.OrszagFejlesztesKeszuls", new[] { "Orszag_Id" });
            DropIndex("dbo.OrszagFejlesztesKeszuls", new[] { "Fejlesztes_Id" });
            DropIndex("dbo.OrszagFejlesztes", new[] { "Orszag_Id" });
            DropIndex("dbo.OrszagFejlesztes", new[] { "Fejlesztes_Id" });
            DropIndex("dbo.OrszagEpuletKeszuls", new[] { "Orszag_Id" });
            DropIndex("dbo.OrszagEpuletKeszuls", new[] { "Epulet_Id" });
            DropIndex("dbo.OrszagEpulets", new[] { "Orszag_Id" });
            DropIndex("dbo.OrszagEpulets", new[] { "Epulet_Id" });
            DropIndex("dbo.Orszags", new[] { "User_Id" });
            DropIndex("dbo.Hadseregs", new[] { "SajatOrszag_Id" });
            DropIndex("dbo.Hadseregs", new[] { "CelOrszag_Id" });
            DropIndex("dbo.HadseregEgysegs", new[] { "Hadsereg_Id" });
            DropIndex("dbo.HadseregEgysegs", new[] { "Egyseg_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.OrszagEgysegs");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.OrszagFejlesztesKeszuls");
            DropTable("dbo.Fejlesztes");
            DropTable("dbo.OrszagFejlesztes");
            DropTable("dbo.OrszagEpuletKeszuls");
            DropTable("dbo.Epulets");
            DropTable("dbo.OrszagEpulets");
            DropTable("dbo.Orszags");
            DropTable("dbo.Hadseregs");
            DropTable("dbo.HadseregEgysegs");
            DropTable("dbo.Egysegs");
        }
    }
}
