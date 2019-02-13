using Autofac;
using Microsoft.Extensions.Logging;
using Surging.Core.Caching;
using Surging.Core.Caching.Configurations;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Configurations;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.EventBusKafka.Configurations;
using Surging.Core.Nlog;
using Surging.Core.ProxyGenerator;
using Surging.Core.ServiceHosting;
using Surging.Core.ServiceHosting.Internal.Implementation;
using Surging.Core.Zookeeper;
using Surging.Core.Zookeeper.Configurations;
using System;
using System.Text;

namespace mysurging.demo.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var host = new ServiceHostBuilder()
                .RegisterServices(builder =>
                {
                    builder.AddMicroService(option =>
                    {
                        option.AddClient()
                        .AddCache();
                        option.UseZooKeeperManager(new ConfigInfo("127.0.0.1:2181"));
                        builder.Register(p => new CPlatformContainer(ServiceLocator.Current));
                    });
                })
                .UseNLog(LogLevel.Trace, "NLog.config")// 使用NLog 记录日志
                .Configure(build =>
                build.AddEventBusFile("Configs/eventBusSettings.json", optional: false))
                .Configure(build =>
                build.AddCacheFile("Configs/cacheSettings.json", optional: false, reloadOnChange: true))
                .Configure(build =>
                build.AddCPlatformFile("${surgingpath}|Configs/surgingSettings.json", optional: false, reloadOnChange: true))
                .UseClient()
                .UseProxy()
                .UseStartup<Startup>()
                .Build();

            using (host.Run())
            {
                Startup.Test(ServiceLocator.GetService<IServiceProxyFactory>());
                //Startup.TestRabbitMq(ServiceLocator.GetService<IServiceProxyFactory>());
                // Startup.TestForRoutePath(ServiceLocator.GetService<IServiceProxyProvider>());
                /// test Parallel
                //var connectionCount = 200000;
                //StartRequest(connectionCount);
                //Console.ReadLine();
            }
        }
    }
}
