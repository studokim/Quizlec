using System;
using System.Collections.Generic;
using System.Linq;
using Aerospike.Client;
using Quizleç.Exceptions;
using Quizleç.Models;
using User = Quizleç.Models.User;

namespace Quizleç.Database
{
    public class AerospikeWriteClient : AerospikeClient
    {
        public AerospikeWriteClient() : base(true) {}

        private Bin[] MakeUserBins(User user)
        {
            return new[]
            {
                new Bin("Id", user.Id),
                new Bin("Login", user.Login),
                new Bin("PasswordHash", user.PasswordHash),
                new Bin("Email", user.Email),
                new Bin("Collections", user.Collections),
                new Bin("IsActive", true),
            };
        }

        private Bin[] MakeCollectionBins(Collection collection)
        {
            return new[]
            {
                new Bin("Id", collection.Id),
                new Bin("Name", collection.Name),
                new Bin("Description", collection.Description),
                new Bin("Cards", collection.Cards),
                new Bin("IsActive", true),
            };
        }

        private Bin[] MakeCardBins(Card card)
        {
            return new[]
            {
                new Bin("Id", card.Id),
                new Bin("FrontSide", card.FrontSide),
                new Bin("BackSide", card.BackSide),
                new Bin("IsActive", true),
            };
        }
        public void Put(Object entity)
        {
            try
            {
                Key key;
                Bin[] bins;
                switch (entity)
                {
                    case User user:
                        key = MakeKey(Entities.User, user.Id);
                        bins = MakeUserBins(user);
                        break;
                    case Collection collection:
                        key = MakeKey(Entities.Collection, collection.Id);
                        bins = MakeCollectionBins(collection);
                        break;
                    case Card card:
                        key = MakeKey(Entities.Card, card.Id);
                        bins = MakeCardBins(card);
                        break;
                    default:
                        throw new ArgumentException
                            ($"Not implemented for Entity={entity.GetType().Name}.");
                }

                if (!Exists(key))
                    Client.Put((WritePolicy) Policy, key, bins);
                else
                    throw new AlreadyExistsException
                        ($"Entity={entity.GetType().Name} with key={key} already exists.");
            }
            catch (Exception e)
            {
                throw new DatabaseWriteException
                    ($"Can't put Entity={entity.GetType().Name}.", e);
            }
        }

        public void Delete(Entities entity, int id)
        {
            Key key = MakeKey(entity, id);
            if (Exists(key))
                try
                {
                    Record r = Client.Operate((WritePolicy) Policy, key,
                            Operation.Put(new Bin("IsActive", false)));
                }
                catch (Exception e)
                {
                    throw new DatabaseWriteException
                        ($"Error while deleting {entity} with id={id}", e);
                }
            else
            {
                throw new DatabaseWriteException
                    ($"The {entity} with id={id} never existed.");
            }
        }

        public void Update(Object entity, int id)
        {
            try
            {
                Key key;
                Bin[] bins;
                switch (entity)
                {
                    case User user:
                        key = MakeKey(Entities.User, id);
                        bins = MakeUserBins(user);
                        break;
                    case Collection collection:
                        key = MakeKey(Entities.Collection, id);
                        bins = MakeCollectionBins(collection);
                        break;
                    case Card card:
                        key = MakeKey(Entities.Card, id);
                        bins = MakeCardBins(card);
                        break;
                    default:
                        throw new ArgumentException
                            ($"Not implemented for Entity={entity.GetType().Name}.");
                }
                if (Exists(key))
                {
                    foreach (var bin in bins)
                    {
                        if (bin.value != Value.NULL && bin.name != "Id" && bin.name != "IsActive")
                            Client.Operate((WritePolicy) Policy, key, Operation.Put(bin));
                    }
                }
                else
                    throw new CardNotFoundException
                        ($"Can't update deleted {entity.GetType().Name} with id={id}.");
            }
            catch (Exception e)
            {
                throw new DatabaseWriteException
                    ($"Error while updating {entity.GetType().Name} with id={id}", e);
            }
        }

        public void AddCardToCollection(int collectionId, int cardId)
        {
            try
            {
                Key cardKey = MakeKey(Entities.Card, cardId);
                Key collectionKey = MakeKey(Entities.Collection, collectionId);
                Record collectionRecord = Client.Get(Policy, collectionKey, "Cards");
                if (Exists(collectionKey) && Exists(cardKey) &&
                    !collectionRecord.GetList("Cards").Contains((long)cardId))
                {
                    Client.Operate((WritePolicy) Policy, collectionKey,
                        ListOperation.Append("Cards", Value.Get(cardId)));
                }
                else
                    throw new DatabaseWriteException
                        ($"The Collection with id={collectionId}" +
                         $"or Card with id {cardId} never existed.");
            }
            catch (Exception e)
            {
                throw new DatabaseWriteException
                    ($"Error while updating Collection with id={collectionId}", e);
            }
        }

        public void AddCollectionToUser(int userId, int collectionId)
        {
            try
            {
                Key collectionKey = MakeKey(Entities.Collection, collectionId);
                Key userKey = MakeKey(Entities.User, userId);
                Record userRecord = Client.Get(Policy, userKey, "Collections");
                if (Exists(userKey) && Exists(collectionKey) &&
                    !userRecord.GetList("Collections").Contains((long)collectionId))
                {
                    Client.Operate((WritePolicy) Policy, userKey,
                        ListOperation.Append("Collections", Value.Get(collectionId)));
                }
                else
                    throw new DatabaseWriteException
                        ($"The User with id={userId}" +
                         $"or Collection with id {collectionId} never existed.");
            }
            catch (Exception e)
            {
                throw new DatabaseWriteException
                    ($"Error while updating User with id={userId}", e);
            }
        }
    }
}
