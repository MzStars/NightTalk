namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditField_FileInfo_UID_FileType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileInfoes", "UID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileInfoes", "UID");
        }
    }
}
