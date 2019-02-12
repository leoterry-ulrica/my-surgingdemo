using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Surging.Core.Caching.Configurations;
using Surging.Core.Codec.MessagePack;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Configurations;
using Surging.Core.CPlatform.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation;
using Surging.Core.CPlatform.Runtime.Server;
using Surging.Core.CPlatform.Support;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.EventBusKafka;
using Surging.Core.EventBusKafka.Configurations;
using Surging.Core.Nlog;
using Surging.Core.ProxyGenerator;
using Surging.Core.ServiceHosting;
using Surging.Core.ServiceHosting.Internal.Implementation;
using Surging.Core.Zookeeper;
using Surging.Core.Zookeeper.Configurations;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mysurging.demo.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var host = new ServiceHostBuilder()
                .RegisterServices(builder =>
                {
                    builder.AddMicroService(option =>
                    {
                        option.AddServiceRuntime()
                        .AddRelateService()
                        .AddConfigurationWatch()
                        .UseZooKeeperManager(new ConfigInfo("127.0.0.1:2181"))
                        .AddServiceEngine(typeof(SurgingServiceEngine)); 
                        builder.Register(p => new CPlatformContainer(ServiceLocator.Current));
                    });
                })
                .UseServer(options => { })
                .UseConsoleLifetime()
                .UseNLog(LogLevel.Error, "Configs/nlog.config")// 使用NLog 记录日志
                .Configure(build =>
                build.AddCacheFile("${cachepath}|Configs/cacheSettings.json", basePath: AppContext.BaseDirectory, optional: false, reloadOnChange: true))
                .Configure(build =>
                build.AddCPlatformFile("${surgingpath}|Configs/surgingSettings.json", optional: false, reloadOnChange: true))
                .UseStartup<Startup>()
                .Build();

            using (host.Run())
            {
                Console.WriteLine($"服务端启动成功，{DateTime.Now}。");
            }
        }
    }
}
