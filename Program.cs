using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TestWebAppDotNet
{
    public class Program
    {
        /// <summary>
        /// Use static to create only one instance of simulation,
        /// so each user connecting to the server will have same result.
        /// </summary>
        public static TradeSimulation TradeSimulation = new TradeSimulation();

        public static void Main(string[] args)
        {
            Task.Run(() => SimulateTrades());

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        private static async Task SimulateTrades()
        {
            while (true)
            {
                int waitTime =TradeSimulation.GetRandomWaitTime();
                await Task.Delay(waitTime);
                TradeSimulation.SimulateTrade();
            }
        }
    }
}