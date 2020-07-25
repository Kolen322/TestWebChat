using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebServer.BLL.Application.Configuration;
using WebServer.DAL.Configuration;
using WebServer.Configuration;
using WebServer.Hub;
using WebServer.Extensions;
using Polly;
using System.Net.Sockets;

namespace WebServer
{
    public class Startup
    {
        private ILogger<Startup> _logger { get; }
        public Startup(ILogger<Startup> logger, IConfiguration configuration)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            var policyOptions = Configuration.GetSection("PolicyOptions").Get<PolicyOptions>();
            services.AddRetryPolicy(policyOptions);
            services.AddControllers();
            services.AddMapper();
            services.AddDal();
            services.AddBll();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "WebServer"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            WaitForDB(app);

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebServer V1");
            });
        }

        private void WaitForDB(IApplicationBuilder app)
        {
            _logger.LogInformation("Enter database configure");
            var retryPolicy = app.ApplicationServices.GetService<ISyncPolicy>();
            var dbHostName = Environment.GetEnvironmentVariable("DB_CONTAINER_NAME");
            using var client = new TcpClient();

            try
            {
                retryPolicy.Execute(
                    () =>
                    {
                        client.Connect(dbHostName, 5432);
                        client.Close();
                    }
                );
                _logger.LogInformation("Connect to database is successful");
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to connect to Database - {nameof(dbHostName)}");
                throw new ApplicationException($"Failed to connect to Database - {nameof(dbHostName)}");
            }
        }
    }
}
