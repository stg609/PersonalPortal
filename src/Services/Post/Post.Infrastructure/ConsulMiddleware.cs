using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Post.Infrastructure
{
    //public class ConsulMiddleware
    //{
    //    private readonly RequestDelegate _next;

    //    public ConsulMiddleware(RequestDelegate next)
    //    {
    //        _next = next;
    //    }

    //    public async Task InvokeAsync(HttpContext context, IApplicationLifetime appLifeTime)
    //    {
    //        //创建一个 ConsulClient 对象，Consul 服务器地址通过配置文件获取
    //        ConsulClient consulClient = new ConsulClient(cfg => cfg.Address = new Uri(Configuration["ConsulConfig:Address"]));
    //        int port = Convert.ToInt32(Configuration.GetValue<string>("urls").Split(":").Last());

    //        //添加一个健康检查
    //        var httpCheck = new AgentServiceCheck()
    //        {
    //            DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
    //            Interval = TimeSpan.FromSeconds(30),
    //            HTTP = $"http://localhost:{port}/api/HealthCheck" //指向我们自己创建的 healthCheck 节点
    //        };

    //        var registration = new AgentServiceRegistration()
    //        {
    //            //必须唯一
    //            ID = Guid.NewGuid().ToString(),
    //            Name = Configuration.GetValue<string>("Name"),
    //            Address = "localhost", //当前这个 service 的host
    //            Port = port, //当前这个 service 的端口
    //            Tags = new[] { "Tag for Consul agent" },
    //            Check = httpCheck
    //        };

    //        consulClient.Agent.ServiceRegister(registration).ConfigureAwait(false);

    //        //对应用程序的开始结束进行挂钩
    //        appLifeTime.ApplicationStarted.Register(OnStarted);
    //        appLifeTime.ApplicationStopped.Register(OnStopped);

    //        await _next(context);
    //    }
    //}

    //public static class ConsulMiddlewareExtensions
    //{
    //    public static IApplicationBuilder UseConsul(this IApplicationBuilder builder)
    //    {
    //        return builder.UseMiddleware<ConsulMiddleware>()
    //    }
    //}
}
