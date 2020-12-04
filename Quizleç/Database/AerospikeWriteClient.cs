using System;
using Aerospike.Client;
using Quizleç.Exceptions;
using Quizleç.Models;
using User = Quizleç.Models.User;

namespace Quizleç.Database
{
    public class AerospikeWriteClient : AerospikeClient
    {
        public AerospikeWriteClient() : base(true) {}

        public void PutUser(User user)
        {
            try
            {
                Key key = MakeKey(Entities.User, user.Id);
                Bin[] bins =
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
            catch (Exception e)
            {
                throw new DatabaseWriteException
                    ($"Error while writing user with login={user.Login}.", e);
            }
        }

        public void PutCollection(Collection collection)
        {
            try
            {
                Key key = MakeKey(Entities.Collection, collection.Id);
                Bin[] bins =
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
            catch (Exception e)
            {
                throw new DatabaseWriteException
                    ($"Error while writing collection with name={collection.Name}.", e);
            }
        }

        public void PutCard(Card card)
        {
            try
            {
                Key key = MakeKey(Entities.Card, card.Id);
                Bin[] bins =
                {
                    new Bin("Id", card.Id),
                    new Bin("FrontSide", card.FrontSide),
                    new Bin("BackSide", card.BackSide),
                    new Bin("IsActive", true),
                };
                Client.Put((WritePolicy)Policy, key, bins);
            }
            catch (Exception e)
            {
                throw new DatabaseWriteException
                    ($"Error while writing card with frontside={card.FrontSide}.", e);
            }
        }

        public void Delete(Entities entity, int id)
        {
            try
            {
                Record r = Client.Get(Policy, MakeKey(entity, id));
                if (r.GetBool("IsActive"))
                    Client.Operate((WritePolicy) Policy, MakeKey(entity, id),
                        Operation.Put(new Bin("IsActive", false)));
            }
            catch (NullReferenceException e)
            {
                throw new DatabaseWriteException
                    ($"The {entity} with id={id} never existed.", e);
            }
            catch (Exception e)
            {
                throw new DatabaseWriteException
                    ($"Error while deleting {entity} with id={id}", e);
            }
        }
    }
}
