namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddField_Article_Type_Date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "ArticleType", c => c.Int(nullable: false));
            AddColumn("dbo.Article", "ArticleDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Article", "ArticleDate");
            DropColumn("dbo.Article", "ArticleType");
        }
    }
}
