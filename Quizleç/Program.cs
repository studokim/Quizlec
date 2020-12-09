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
        private static void Print()
        {
            var c = new AerospikeQueryClient();
            foreach (var card in c.GetCardsByUserId(0))
            {
                Console.WriteLine(card.FrontSide);
            }
        }

        private static void HardcodePut()
        {
            AerospikeWriteClient w = new AerospikeWriteClient();
            w.Put(new Card() { Id = 1, FrontSide = "Cog", BackSide = "Зубец" });
            w.Put(new Card() { Id = 2, FrontSide = "Cab", BackSide = "Такси" });
            w.Put(new Card() { Id = 3, FrontSide = "Can", BackSide = "Банка" });
            w.Put(new Card() { Id = 4, FrontSide = "Cop", BackSide = "Полицейский" });
            w.Put(new Collection()
            {
                Id = 0, Name = "Default", Description = "Basic collection",
                Cards = new List<int>() {2, 4}
            });
            w.Put(new Collection()
            {
                Id = 1, Name = "New", Description = "Second collection",
                Cards = new List<int>() {1, 3}
            });
            w.Put(new User()
            {
                Id = 0, Login = "Johnny", Email = "a@b.c", PasswordHash = "fwejnf",
                Collections = new List<int>() {0, 1}
            });
            w.Put(new User()
            {
                Id = 1, Login = "Smithy", Email = "b@c.d", PasswordHash = "fddgf",
                Collections = new List<int>() {0}
            });
        }

        private static void HardcodeGet()
        {
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
        }

        private static void HardCodeDelete()
        {
            AerospikeWriteClient c = new AerospikeWriteClient();
            Console.WriteLine(c.Delete(Entities.Card, 4));
            Console.WriteLine(c.Delete(Entities.Collection, 4444));
        }

        private static void HardCodeUpdate()
        {
            AerospikeWriteClient w = new AerospikeWriteClient();
            Card r = new Card() {BackSide = "Американский полицейский"};
            w.Update(r, 4);
            User u = new User() { Email = "e@f.g"};
            w.Update(u, 1);
        }
        public static void Main(string[] args)
        {
            //HardcodePut();
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
