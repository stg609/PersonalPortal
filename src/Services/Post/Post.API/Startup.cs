using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Post.Infrastructure;

using Swashbuckle.AspNetCore.Swagger;
using BuildingBlocks.Services;
using System.Net;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Post.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Post API",
                    Version = "v1",
                    Description = "The Post Service Http API"
                });

                c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = $"{Configuration.GetValue<string>("IdentityUrl")}/connect/authorize",
                    TokenUrl = $"{Configuration.GetValue<string>("IdentityUrl")}/connect/token",
                    Scopes = new Dictionary<string, string>()
                    {
                        { "post", "Post API" }
                    }
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var identityUrl = Configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "post";
            });

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder => builder.AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials());
            //});
        }

        //private string GetIPv4()
        //{
        //    // order interfaces by speed and filter out down and loopback
        //    // take first of the remaining
        //    var firstUpInterface = NetworkInterface.GetAllNetworkInterfaces()
        //        .OrderByDescending(c => c.Speed)
        //        .FirstOrDefault(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up);
        //    if (firstUpInterface != null)
        //    {
        //        var props = firstUpInterface.GetIPProperties();
        //        // get first IPV4 address assigned to this interface
        //        var firstIpV4Address = props.UnicastAddresses
        //            .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
        //            .Select(c => c.Address)
        //            .FirstOrDefault();

        //        return firstIpV4Address.ToString();
        //    }

        //    return null;
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifeTime)
        {
            int configuredPort = Convert.ToInt32(Configuration.GetValue<string>("urls").Split(":").Last());

            string hostIP = null;
            switch (env.EnvironmentName)
            {
                case "kubernetes":
                    hostIP = Environment.GetEnvironmentVariable("POD_IP");
                    break;
                default:
                    hostIP = "127.0.0.1";
                    break;
            }

            app.RegisterToConsul(new ServerOptions
            {
                Name = Configuration.GetValue<string>("Name"),
                Scheme = "http",
                Host = hostIP,
                Port = configuredPort,
                HealthCheckUrl = $"http://{hostIP}:{configuredPort}/api/HealthCheck",
                Tags = new[] { Environment.MachineName }
            }, Configuration["ConsulConfig:Address"], appLifeTime);

            var pathBase = Configuration["PathBase"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(options =>
            {
                //options.RouteTemplate = "api/post/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{(!String.IsNullOrEmpty(pathBase) ? pathBase : String.Empty) }/swagger/v1/swagger.json", "Post API V1");
                c.OAuthClientId("swagger");
                c.OAuthAppName("swagger");
            });

            //app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            //TODO UnComment in Production
            //app.UseAuthentication();
            app.UseMvc();
        }
    }
}
