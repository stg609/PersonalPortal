using System;

using Consul;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace BuildingBlocks.Services
{
    public static class ConsulExtensions
    {
        public static IApplicationBuilder RegisterToConsul(this IApplicationBuilder builder, ServerOptions serverOptions, string consulUrl, IApplicationLifetime appLifeTime)
        {
            //创建一个 ConsulClient 对象，Consul 服务器地址通过配置文件获取
            ConsulClient consulClient = new ConsulClient(cfg => cfg.Address = new Uri(consulUrl));

            //添加一个健康检查
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                Interval = TimeSpan.FromSeconds(30),
                HTTP = serverOptions.HealthCheckUrl
            };

            var registration = new AgentServiceRegistration()
            {
                //必须唯一
                ID = Guid.NewGuid().ToString(),
                Name = serverOptions.Name,
                Address = serverOptions.Host, //当前这个 service 的host
                Port = serverOptions.Port, //当前这个 service 的端口
                Tags = serverOptions.Tags,
                Check = httpCheck
            };

            //对应用程序的开始结束进行挂钩
            appLifeTime.ApplicationStarted.Register(async () =>
            {
                await consulClient.Agent.ServiceRegister(registration);
            });
            appLifeTime.ApplicationStopped.Register(async () =>
            {
                await consulClient.Agent.ServiceDeregister(registration.ID);
            });

            return builder;
        }
    }
}
