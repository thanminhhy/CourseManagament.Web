namespace CourseManagament.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TrainingStaffs", "Email", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TrainingStaffs", "Email", c => c.String(nullable: false));
        }
    }
}
