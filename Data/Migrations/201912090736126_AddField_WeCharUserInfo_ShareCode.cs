namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddField_WeCharUserInfo_ShareCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WeCharUserInfo", "ShareCode", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WeCharUserInfo", "ShareCode");
        }
    }
}
