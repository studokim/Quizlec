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
                Client.Put((WritePolicy) Policy, key, bins);
            }
            catch (Exception e)
            {
                throw new DatabaseWriteException
                    ($"Can't put Entity={entity.GetType().Name}.", e);
            }
        }

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
                new Bin("Owner", collection.Owner),
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

        public int Delete(Entities entity, int id)
        {
            try
            {
                Record r = Client.Get(Policy, MakeKey(entity, id));
                if (r.GetBool("IsActive"))
                    r = Client.Operate((WritePolicy) Policy, MakeKey(entity, id),
                        Operation.Put(new Bin("IsActive", false)));
                return r.GetInt("Id");
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

        public int Update(Object entity, int id)
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
                Record r = Client.Get(Policy, key, "IsActive");
                if (r.GetBool("IsActive"))
                {
                    foreach (var bin in bins)
                    {
                        if (bin.value != Value.NULL && bin.name != "Id" && bin.name != "IsActive")
                            r = Client.Operate((WritePolicy) Policy, key,
                                Operation.Put(bin));
                    }

                    return r.GetInt("Id");
                }
                else
                    throw new CardNotFoundException
                        ($"Can't update deleted {entity.GetType().Name} with id={id}.");
            }
            catch (NullReferenceException e)
            {
                throw new DatabaseWriteException
                    ($"The {entity.GetType().Name} with id={id} never existed.", e);
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
                Record cardRecord = Client.Get(Policy, cardKey, "IsActive");
                Record collectionRecord = Client.Get(Policy, collectionKey,
                    "IsActive", "Cards");
                if (collectionRecord.GetBool("IsActive") &&
                    cardRecord.GetBool("IsActive") &&
                    !collectionRecord.GetList("Cards").Contains((long)cardId))
                {
                    Client.Operate((WritePolicy) Policy, collectionKey,
                        ListOperation.Append("Cards", Value.Get(cardId)));
                }
            }
            catch (NullReferenceException e)
            {
                throw new DatabaseWriteException
                    ($"The Collection with id={collectionId}" +
                     $"or Card with id {cardId} never existed.", e);
            }
            catch (Exception e)
            {
                throw new DatabaseWriteException
                    ($"Error while updating Collection with id={collectionId}", e);
            }
        }
    }
}
