namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditField_ArticleOperating_articleID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleOperating", "ArticleID", c => c.Guid(nullable: false));
            DropColumn("dbo.ArticleOperating", "ActicleID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ArticleOperating", "ActicleID", c => c.Guid(nullable: false));
            DropColumn("dbo.ArticleOperating", "ArticleID");
        }
    }
}
