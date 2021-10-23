namespace CourseManagament.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adding : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseName = c.String(nullable: false, maxLength: 255),
                        CategoryId = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Description = c.String(),
                        UserId = c.String(maxLength: 128),
                        CourseTrainer_CourseId = c.Int(),
                        CourseTrainer_TrainerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.CourseTrainers", t => new { t.CourseTrainer_CourseId, t.CourseTrainer_TrainerId })
                .Index(t => t.CategoryId)
                .Index(t => t.UserId)
                .Index(t => new { t.CourseTrainer_CourseId, t.CourseTrainer_TrainerId });
            
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
                "dbo.CourseTrainers",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        TrainerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CourseId, t.TrainerId });
            
            CreateTable(
                "dbo.Trainers",
                c => new
                    {
                        TrainerId = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(nullable: false, maxLength: 255),
                        Email = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Address = c.String(nullable: false, maxLength: 255),
                        Speciality = c.String(nullable: false),
                        CourseTrainer_CourseId = c.Int(),
                        CourseTrainer_TrainerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TrainerId)
                .ForeignKey("dbo.AspNetUsers", t => t.TrainerId)
                .ForeignKey("dbo.CourseTrainers", t => new { t.CourseTrainer_CourseId, t.CourseTrainer_TrainerId })
                .Index(t => t.TrainerId)
                .Index(t => new { t.CourseTrainer_CourseId, t.CourseTrainer_TrainerId });
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Trainees",
                c => new
                    {
                        TraineeId = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(nullable: false, maxLength: 255),
                        Email = c.String(),
                        Age = c.Int(nullable: false),
                        Address = c.String(nullable: false, maxLength: 255),
                        Education = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TraineeId)
                .ForeignKey("dbo.AspNetUsers", t => t.TraineeId)
                .Index(t => t.TraineeId);
            
            CreateTable(
                "dbo.TrainingStaffs",
                c => new
                    {
                        TrainingStaffId = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(nullable: false, maxLength: 255),
                        Email = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Address = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.TrainingStaffId)
                .ForeignKey("dbo.AspNetUsers", t => t.TrainingStaffId)
                .Index(t => t.TrainingStaffId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrainingStaffs", "TrainingStaffId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Trainees", "TraineeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Trainers", new[] { "CourseTrainer_CourseId", "CourseTrainer_TrainerId" }, "dbo.CourseTrainers");
            DropForeignKey("dbo.Trainers", "TrainerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", new[] { "CourseTrainer_CourseId", "CourseTrainer_TrainerId" }, "dbo.CourseTrainers");
            DropForeignKey("dbo.Courses", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "CategoryId", "dbo.Categories");
            DropIndex("dbo.TrainingStaffs", new[] { "TrainingStaffId" });
            DropIndex("dbo.Trainees", new[] { "TraineeId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Trainers", new[] { "CourseTrainer_CourseId", "CourseTrainer_TrainerId" });
            DropIndex("dbo.Trainers", new[] { "TrainerId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Courses", new[] { "CourseTrainer_CourseId", "CourseTrainer_TrainerId" });
            DropIndex("dbo.Courses", new[] { "UserId" });
            DropIndex("dbo.Courses", new[] { "CategoryId" });
            DropTable("dbo.TrainingStaffs");
            DropTable("dbo.Trainees");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Trainers");
            DropTable("dbo.CourseTrainers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Courses");
            DropTable("dbo.Categories");
        }
    }
}
