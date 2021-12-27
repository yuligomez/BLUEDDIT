using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ServerAdministrativoWebApi
{
    public class Program
    {
        private static ServerAdministrativoManagement serverAdministrativoManagement;
        public static void Main(string[] args)
        {
            serverAdministrativoManagement = new ServerAdministrativoManagement();
            var taskShowMenu = Task.Run(() => serverAdministrativoManagement.Connect());
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
