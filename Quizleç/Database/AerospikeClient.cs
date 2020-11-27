using System;
using System.Collections.Generic;
using System.Linq;
using Aerospike.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Quizleç.Models;
using User = Quizleç.Models.User;

namespace Quizleç.Database
{
    public class AerospikeClient : IClient
    {
        private readonly Aerospike Options;
        private readonly Policy Policy;
        private AsyncClient Client;
        private Key Key;

        public AerospikeClient(IOptions<Aerospike> optionsAccessor)
        {
            Options = optionsAccessor.Value;
            Policy = new WritePolicy();
            Open();
        }

        public void Open()
        {
            Client = new AsyncClient(Options.Hostname, Options.Port);
        }

        public void Close()
        {
            Client.Close();
        }

        public Record GetRecord(Entities entity, int id)
        {
            switch (entity)
            {
                case Entities.Card:
                    Key = new Key(Options.Namespace, Options.Set.Card, id);
                    break;
                case Entities.Collection:
                    Key = new Key(Options.Namespace, Options.Set.Collection, id);
                    break;
                case Entities.User:
                    Key = new Key(Options.Namespace, Options.Set.User, id);
                    break;
                default:
                    throw new ArgumentException("Not implemented for this Entity.");
            }

            return Client.Get(Policy, Key);
        }

        public User GetUser(int id)
        {
            var r = GetRecord(Entities.User, id);
            return new User()
            {
                Id = id, Login = r.bins["Login"].ToString(),
                PasswordHash = r.bins["PasswordHash"].ToString(),
                Email = r.bins["Email"].ToString(),
                Collections = r.GetList("Collections") as List<int>
            };
        }

        public Collection GetCollection(int id)
        {
            var r = GetRecord(Entities.Collection, id);
            return new Collection()
            {
                Id = id, Name = r.GetString("Name"),
                Description = r.GetString("Description"),
                Owner = r.GetInt("Owner"),
                Cards = r.GetList("Cards") as List<int>
            };
        }

        public Card GetCard(int id)
        {
            var r = GetRecord(Entities.Card, id);
            return new Card()
            {
                Id = id, FrontSide = r.GetString("FrontSide"),
                BackSide = r.GetString("BackSide")
            };
        }
    }
}
