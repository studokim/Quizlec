using System;
using Aerospike.Client;
using Quizleç.Models;
using User = Quizleç.Models.User;

namespace Quizleç.Database
{
    public class AerospikeWriteClient : AerospikeClient
    {
        public AerospikeWriteClient() : base(true) {}

        public void PutEntity(object obj)
        {
            switch (obj)
            {
                case User user:
                    PutUser(user);
                    break;
                case Collection collection:
                    PutCollection(collection);
                    break;
                case Card card:
                    PutCard(card);
                    break;
                default:
                    throw new ArgumentException
                        ($"Not implemented for this Entity {obj.GetType()}.");
            }
        }

        public void PutUser(User user)
        {
            Key key = MakeKey(Entities.User, user.Id);
            Bin[] bins = new[]
            {
                new Bin("Id", user.Id),
                new Bin("Login", user.Login),
                new Bin("PasswordHash", user.PasswordHash),
                new Bin("Email", user.Email),
                new Bin("Collections", user.Collections),
                new Bin("IsActive", true),
            };
            Client.Put((WritePolicy)Policy, key, bins);
        }

        public void PutCollection(Collection collection)
        {
            Key key = MakeKey(Entities.Collection, collection.Id);
            Bin[] bins = new[]
            {
                new Bin("Id", collection.Id),
                new Bin("Name", collection.Name),
                new Bin("Description", collection.Description),
                new Bin("Owner", collection.Owner),
                new Bin("Cards", collection.Cards),
                new Bin("IsActive", true),
            };
            Client.Put((WritePolicy)Policy, key, bins);
        }

        public void PutCard(Card card)
        {
            Key key = MakeKey(Entities.Card, card.Id);
            Bin[] bins = new[]
            {
                new Bin("Id", card.Id),
                new Bin("FrontSide", card.FrontSide),
                new Bin("BackSide", card.BackSide),
                new Bin("IsActive", true),
            };
            Client.Put((WritePolicy)Policy, key, bins);
        }
    }
}
