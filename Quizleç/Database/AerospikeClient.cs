using System;
using Aerospike.Client;
using Quizleç.Models;

namespace Quizleç.Database
{
    public abstract class AerospikeClient : IClient
    {
        protected readonly Config.Aerospike Options;
        protected readonly Policy Policy;
        protected AsyncClient Client;

        protected AerospikeClient(bool write)
        {
            if (write)
                Policy = new WritePolicy();
            else
                Policy = new QueryPolicy();
            // TODO: use Environment variables(?) instead of hardcoding.
            Options = Config.Get.AerospikeOptions("Development");
            Open();
        }

        ~AerospikeClient()
        {
            Close();
        }

        public void Open()
        {
            Client = new AsyncClient(Options.Hostname, Options.Port);
        }

        public void Close()
        {
            Client.Close();
        }

        protected Key MakeKey(Entities entity, int id)
        {
            Key key;
            switch (entity)
            {
                case Entities.Card:
                    key = new Key(Options.Namespace, Options.Set.Card, id);
                    break;
                case Entities.Collection:
                    key = new Key(Options.Namespace, Options.Set.Collection, id);
                    break;
                case Entities.User:
                    key = new Key(Options.Namespace, Options.Set.User, id);
                    break;
                default:
                    throw new ArgumentException
                        ($"Not implemented for this Entity {entity}.");
            }

            return key;
        }

        protected bool Exists(Key key)
        {
            return Client.Exists(Policy, key) &&
                   Client.Get(Policy, key, "IsActive").GetBool("IsActive");
        }
    }
}
