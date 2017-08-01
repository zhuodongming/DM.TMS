﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DM.TMS.App.TMS;
using DM.TMS.Domain.Interface.TMS;
using DM.TMS.Repository.TMS;
using DM.TMS.Domain;

namespace DM.TMS.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;


            //var builder = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .AddEnvironmentVariables();


        }

        public IConfiguration Configuration { get; }

        public static IServiceCollection Services { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<TaskApp, TaskApp>();
            services.AddScoped<ITaskRepository, TaskRepository>();//添加依赖注入

            services.Configure<DBSettings>(Configuration);

            services.AddMvc();

            Services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            TaskApp taskApp = Services.BuildServiceProvider().GetService<TaskApp>();
            taskApp.StartTaskHost().Wait();
        }
    }
}