namespace CourseManagament.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateData : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", new[] { "CourseTrainer_CourseId", "CourseTrainer_TrainerId" }, "dbo.CourseTrainers");
            DropForeignKey("dbo.Trainers", new[] { "CourseTrainer_CourseId", "CourseTrainer_TrainerId" }, "dbo.CourseTrainers");
            DropIndex("dbo.Courses", new[] { "CourseTrainer_CourseId", "CourseTrainer_TrainerId" });
            DropIndex("dbo.Trainers", new[] { "CourseTrainer_CourseId", "CourseTrainer_TrainerId" });
            DropColumn("dbo.Courses", "CourseTrainer_CourseId");
            DropColumn("dbo.Courses", "CourseTrainer_TrainerId");
            DropColumn("dbo.Trainers", "CourseTrainer_CourseId");
            DropColumn("dbo.Trainers", "CourseTrainer_TrainerId");
            DropTable("dbo.CourseTrainers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CourseTrainers",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        TrainerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CourseId, t.TrainerId });
            
            AddColumn("dbo.Trainers", "CourseTrainer_TrainerId", c => c.String(maxLength: 128));
            AddColumn("dbo.Trainers", "CourseTrainer_CourseId", c => c.Int());
            AddColumn("dbo.Courses", "CourseTrainer_TrainerId", c => c.String(maxLength: 128));
            AddColumn("dbo.Courses", "CourseTrainer_CourseId", c => c.Int());
            CreateIndex("dbo.Trainers", new[] { "CourseTrainer_CourseId", "CourseTrainer_TrainerId" });
            CreateIndex("dbo.Courses", new[] { "CourseTrainer_CourseId", "CourseTrainer_TrainerId" });
            AddForeignKey("dbo.Trainers", new[] { "CourseTrainer_CourseId", "CourseTrainer_TrainerId" }, "dbo.CourseTrainers", new[] { "CourseId", "TrainerId" });
            AddForeignKey("dbo.Courses", new[] { "CourseTrainer_CourseId", "CourseTrainer_TrainerId" }, "dbo.CourseTrainers", new[] { "CourseId", "TrainerId" });
        }
    }
}
