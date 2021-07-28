using System;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using StackPoint.Data;
using StackPoint.Domain.Services;
using StackPoint.Service2.AutoMaps;
using StackPoint.Service2.MqMassTransit;
using StackPoint.Service2.Services;

namespace StackPoint.Service2
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
            services.AddControllers();
            services.AddMediatR(typeof(Startup));

            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            services.AddDbContext<DatabaseContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsql(connectionString);
            });

            services.AddAutoMapper(typeof(UserProfile));
            services.AddScoped<IUserService, UserService>();

            var mqHostName = Environment.GetEnvironmentVariable("RABBIT_MQ_HOST_NAME");
            services.AddMassTransit(x =>
            {
                x.AddConsumer<AddUserConsumer>(typeof(AddUserConsumerDefinition));

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(mqHostName);
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
