using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quizleç.Database;
using Quizleç.Models;

namespace Quizleç
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var c1 = new AerospikeQueryClient();
            Console.WriteLine(c1.GetCard(0).FrontSide);
            var c2 = new AerospikeWriteClient();
            c2.PutUser(new User() {Login = "Johnny", Email = "a@b.c",
                PasswordHash = "frfrfr", Collections = new List<int>()
                {
                    1, 2, 5, 10
                }
            });
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
