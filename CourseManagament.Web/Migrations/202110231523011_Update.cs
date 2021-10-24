namespace CourseManagament.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CourseTrainers", newName: "TrainerCourses");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.TrainerCourses", newName: "CourseTrainers");
        }
    }
}
