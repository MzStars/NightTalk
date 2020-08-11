namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddField_WeCharUserInfo_Phone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WeCharUserInfo", "Phone", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WeCharUserInfo", "Phone");
        }
    }
}
