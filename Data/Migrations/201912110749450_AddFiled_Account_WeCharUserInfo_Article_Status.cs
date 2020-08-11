namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFiled_Account_WeCharUserInfo_Article_Status : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "AccountStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Article", "ArticleStatus", c => c.Int(nullable: false));
            AddColumn("dbo.WeCharUserInfo", "WeCharUserInfoStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WeCharUserInfo", "WeCharUserInfoStatus");
            DropColumn("dbo.Article", "ArticleStatus");
            DropColumn("dbo.Account", "AccountStatus");
        }
    }
}
