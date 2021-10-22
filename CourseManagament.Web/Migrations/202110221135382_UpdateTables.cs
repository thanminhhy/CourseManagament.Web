namespace CourseManagament.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trainees", "Email", c => c.String());
            AddColumn("dbo.Trainers", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trainers", "Email");
            DropColumn("dbo.Trainees", "Email");
        }
    }
}
