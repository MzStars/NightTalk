namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddField_Article_Views_Forward : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "ArticleForward", c => c.Int(nullable: false));
            AddColumn("dbo.Article", "ArticleViews", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Article", "ArticleViews");
            DropColumn("dbo.Article", "ArticleForward");
        }
    }
}
