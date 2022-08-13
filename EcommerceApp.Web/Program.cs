using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace EcommerceApp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
           //.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           //.Enrich.FromLogContext()
           //.WriteTo.MongoDB("mongodb://mymongodb/logs")
           // .WriteTo.MongoDB(db,
           //LogEventLevel.Debug,
           //"BackendLog",
           //1,
           //TimeSpan.Zero)
           .WriteTo.File("log.txt")
           .WriteTo.Console()
           .CreateLogger();
            CreateHostBuilder(args).Build().Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog((context, config) =>
                {
                    config.ReadFrom.Configuration(context.Configuration);
                });
    }
}
