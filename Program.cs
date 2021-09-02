using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Runtime.InteropServices;
using WorkerServiceTest2.Config;

namespace WorkerServiceTest2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Singleton.Instance.osPlatform = RuntimeInformation.OSDescription;
            if (Singleton.Instance.osPlatform.Contains("Win")){
                Console.WriteLine("WinOS");
                CreateHostBuilderWin(args).Build().Run(); 
            } else
            {
                Console.WriteLine("MacOS");
                CreateHostBuilderMac(args).Build().Run();
            }

        }

        private static void configureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddHostedService<Worker>();
        }

        public static IHostBuilder CreateHostBuilderWin(string[] args) =>
           Host.CreateDefaultBuilder(args)
           .UseWindowsService()
           .ConfigureServices((hostContext, services) =>
           {
               services.AddHostedService<Worker>();
           });

        public static IHostBuilder CreateHostBuilderMac(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices(configureServices);
    }

}
