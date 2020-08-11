namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddField_Article_ArticleAuthor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "ArticleAuthor", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Article", "ArticleAuthor");
        }
    }
}
