namespace CourseManagament.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrainingStaffTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrainingStaffs",
                c => new
                    {
                        TrainingStaffId = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(nullable: false, maxLength: 255),
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
            DropIndex("dbo.TrainingStaffs", new[] { "TrainingStaffId" });
            DropTable("dbo.TrainingStaffs");
        }
    }
}
