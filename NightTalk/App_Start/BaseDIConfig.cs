using Autofac;
using Data;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NightTalk
{
    public class BaseDIConfig
    {
        public static ContainerBuilder GetBuilder()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<NightTalkContext>();
            builder.RegisterType<Data.EF.DataProvider>().As<Service.IDataProvider>().InstancePerLifetimeScope();

            //builder.RegisterType<RedisHelper>().SingleInstance();
            //if (ConfigurationManager.AppSettings["IsRedis"]?.ToLower() == "1")
            //{
            //    builder.RegisterType<RedisCache>().As<ICache>().SingleInstance();
            //}
            //else
            //{
            //    builder.RegisterType<LocalCache>().As<ICache>().SingleInstance();
            //}
            builder.RegisterType<AccountService>().InstancePerLifetimeScope();
            builder.RegisterType<WeCharUserInfoService>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleService>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleOperatingService>().InstancePerLifetimeScope();
            builder.RegisterType<CommentService>().InstancePerLifetimeScope();

            return builder;
        }
    }
}