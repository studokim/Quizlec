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
            Key key = new Key(Options.Namespace, Options.Set.User.SetName, Config.Get.Index());
            Bin[] bins = new[]
            {
                new Bin(Options.Set.User.Id, user.Id),
                new Bin(Options.Set.User.Login, user.Login),
                new Bin(Options.Set.User.PasswordHash, user.PasswordHash),
                new Bin(Options.Set.User.Email, user.Email),
                new Bin(Options.Set.User.Collections, user.Collections),
                new Bin(Options.Set.User.IsActive, true),
            };
            Client.Put((WritePolicy)Policy, key, bins);
        }

        public void PutCollection(Collection collection)
        {
            Key key = new Key(Options.Namespace, Options.Set.Collection.SetName, Config.Get.Index());
            Bin[] bins = new[]
            {
                new Bin(Options.Set.Collection.Id, collection.Id),
                new Bin(Options.Set.Collection.Name, collection.Name),
                new Bin(Options.Set.Collection.Description, collection.Description),
                new Bin(Options.Set.Collection.Owner, collection.Owner),
                new Bin(Options.Set.Collection.Cards, collection.Cards),
                new Bin(Options.Set.Collection.IsActive, true),
            };
            Client.Put((WritePolicy)Policy, key, bins);
        }

        public void PutCard(Card card)
        {
            Key key = new Key(Options.Namespace, Options.Set.Card.SetName, Config.Get.Index());
            Bin[] bins = new[]
            {
                new Bin(Options.Set.Card.Id, card.Id),
                new Bin(Options.Set.Card.FrontSide, card.FrontSide),
                new Bin(Options.Set.Card.BackSide, card.BackSide),
                new Bin(Options.Set.Card.IsActive, true),
            };
            Client.Put((WritePolicy)Policy, key, bins);
        }
    }
}
