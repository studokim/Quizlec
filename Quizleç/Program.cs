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
        private static void Hardcode()
        {
            AerospikeWriteClient w = new AerospikeWriteClient();
            w.PutCard(new Card() { Id = 1, FrontSide = "Cog", BackSide = "Зубец" });
            w.PutCard(new Card() { Id = 2, FrontSide = "Cab", BackSide = "Такси" });
            w.PutCard(new Card() { Id = 3, FrontSide = "Can", BackSide = "Банка" });
            w.PutCard(new Card() { Id = 4, FrontSide = "Cop", BackSide = "Полицейский" });
            w.PutCollection(new Collection()
            {
                Id = 0, Name = "Default", Description = "Basic collection",
                Owner = 1, Cards = new List<int>() {2, 4}
            });
            w.PutCollection(new Collection()
            {
                Id = 1, Name = "New", Description = "Second collection",
                Owner = 1, Cards = new List<int>() {1, 3}
            });
            w.PutUser(new User()
            {
                Id = 0, Login = "Johnny", Email = "a@b.c", PasswordHash = "fwejnf",
                Collections = new List<int>() {0, 1}
            });
            w.PutUser(new User()
            {
                Id = 1, Login = "Smithy", Email = "b@c.d", PasswordHash = "fddgf",
                Collections = new List<int>() { }
            });
            //AerospikeManagingClient m = new AerospikeManagingClient();
            //m.MakeIndexes();
        }
        public static void Main(string[] args)
        {
            //Hardcode();
            AerospikeQueryClient c = new AerospikeQueryClient();
            try
            {
                Console.WriteLine(c.GetUserInfo(1).Login);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            try
            {
                Console.WriteLine(c.GetCollectionsByUserId(1).Count);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                Console.WriteLine(c.GetCardsByUserId(1)[0].FrontSide);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
