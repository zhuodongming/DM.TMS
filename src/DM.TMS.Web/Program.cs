using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DM.Infrastructure.Helper;

namespace DM.TMS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();



        private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error($"未处理的异常,Source:{sender},Exception:{e.ExceptionObject}");
        }
    }
}
