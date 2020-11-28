using System;
using System.Collections.Generic;
using Aerospike.Client;
using Quizleç.Models;
using User = Quizleç.Models.User;

namespace Quizleç.Database
{
    public class AerospikeQueryClient : AerospikeClient
    {
        public AerospikeQueryClient() : base(false) {}

        public Record GetRecord(Entities entity, int id)
        {
            Key key;
            switch (entity)
            {
                case Entities.Card:
                    key = new Key(Options.Namespace, Options.Set.Card.SetName, id);
                    break;
                case Entities.Collection:
                    key = new Key(Options.Namespace, Options.Set.Collection.SetName, id);
                    break;
                case Entities.User:
                    key = new Key(Options.Namespace, Options.Set.User.SetName, id);
                    break;
                default:
                    throw new ArgumentException
                        ($"Not implemented for this Entity {entity.ToString()}.");
            }

            return Client.Get(Policy, key);
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
