using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // https://github.com/NLog/NLog.Web
            Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateWebHostBuilder(args)
                    .Build()
                    .Run();
            }
            catch (Exception exception)
            {
                // Catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers / threads before application-exit 
                // (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }
        
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Warning);
                })
                .UseNLog(); // Tell the logging factory to include NLog
    }
}
