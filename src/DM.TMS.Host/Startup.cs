using System;
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
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<TaskApp, TaskApp>();
            services.AddScoped<ITaskRepository, TaskRepository>();//添加依赖注入

            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));//注入连接字符串

            services.AddMvc();

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


            //任务系统初始化
            TaskApp taskApp = app.ApplicationServices.GetService<TaskApp>();
            taskApp.StartTaskHost().Wait();
        }
    }
}