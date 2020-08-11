namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTable_AllTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "NickName", c => c.String(maxLength: 30));
            AddColumn("dbo.Comment", "ArticleID", c => c.Guid(nullable: false));
            AddColumn("dbo.WeCharUserInfo", "NickName", c => c.String(maxLength: 50));
            DropColumn("dbo.Account", "NikeName");
            DropColumn("dbo.WeCharUserInfo", "NikeName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WeCharUserInfo", "NikeName", c => c.String(maxLength: 50));
            AddColumn("dbo.Account", "NikeName", c => c.String(maxLength: 30));
            DropColumn("dbo.WeCharUserInfo", "NickName");
            DropColumn("dbo.Comment", "ArticleID");
            DropColumn("dbo.Account", "NickName");
        }
    }
}
