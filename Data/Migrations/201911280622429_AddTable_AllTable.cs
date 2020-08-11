namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_AllTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        UID = c.Int(nullable: false, identity: true),
                        NikeName = c.String(maxLength: 30),
                        PassWord = c.String(maxLength: 30),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UID);
            
            CreateTable(
                "dbo.Article",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UID = c.Int(nullable: false),
                        ArticleTitle = c.String(maxLength: 50),
                        ArticleContent = c.String(unicode: false, storeType: "text"),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ArticleOperating",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        ActicleID = c.Guid(nullable: false),
                        UnionID = c.String(maxLength: 50),
                        Like = c.Boolean(nullable: false),
                        Favorite = c.Boolean(nullable: false),
                        Forward = c.Boolean(nullable: false),
                        CraeteTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        CommentContent = c.String(maxLength: 500),
                        UnionID = c.String(maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FileInfoes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        FileName = c.String(maxLength: 50),
                        FilePath = c.String(maxLength: 255),
                        FileSuffix = c.String(maxLength: 10),
                        FileType = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.WeCharUserInfo",
                c => new
                    {
                        UnionID = c.String(nullable: false, maxLength: 50),
                        OpenID = c.String(maxLength: 50),
                        SessionID = c.String(maxLength: 200),
                        NikeName = c.String(maxLength: 50),
                        Avater = c.String(maxLength: 255),
                        Gender = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UnionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WeCharUserInfo");
            DropTable("dbo.FileInfoes");
            DropTable("dbo.Comment");
            DropTable("dbo.ArticleOperating");
            DropTable("dbo.Article");
            DropTable("dbo.Account");
        }
    }
}
