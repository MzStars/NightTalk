using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class NightTalkContext : DbContext
    {
        static NightTalkContext()
        {
            Database.SetInitializer<NightTalkContext>(null);
        }

        public NightTalkContext()
              : this("NightTalkContext")
        {
            //Configuration.UseDatabaseNullSemantics = true;
        }

        public NightTalkContext(string namedConnection)
            : base(namedConnection)
        {
        }

        /// <summary>
        /// 后台账号
        /// </summary>
        public DbSet<Account> Account { set; get; }

        /// <summary>
        /// 文章
        /// </summary>
        public DbSet<Article> Article { set; get; }

        /// <summary>
        /// 文章操作
        /// </summary>
        public DbSet<ArticleOperating> ArticleOperating { set; get; }

        /// <summary>
        /// 评论
        /// </summary>
        public DbSet<Comment> Comment { set; get; }

        /// <summary>
        /// 文件
        /// </summary>
        public DbSet<FileInfo> FileInfo { set; get; }

        /// <summary>
        /// 微信用户
        /// </summary>
        public DbSet<WeCharUserInfo> WeCharUserInfo { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            string tempTableName = string.Format("Ut_DataList_{0}", DateTime.Now.Year);
        }
    }
}
