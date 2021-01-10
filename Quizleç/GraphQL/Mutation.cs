using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Quizleç.Database;
using Quizleç.Exceptions;
using Quizleç.GraphQL.Models;
using Quizleç.Models;

namespace Quizleç.GraphQL
{
    // TODO: rewrite with exceptions, like Query
    public class Mutation
    {
        public User PutUser(User user)
        {
            try
            {
                var c = new AerospikeWriteClient();
                c.Put(user);
                c.Close();
                return user;
            }
            catch (DatabaseWriteException e)
            {
                throw new GraphQlException("User already exists. " + e.Message, HttpStatusCode.Conflict);
            }
            catch (Exception)
            {
                throw new GraphQlException("Error while saving user.", HttpStatusCode.InternalServerError);
            }
        }

        public Card PutCard(Card card, int collectionId)
        {
            try
            {
                var client = new AerospikeWriteClient();
                if (client.Exists(Entities.Collection, collectionId))
                {
                    client.Put(card);
                    client.AddCardToCollection(collectionId, card.Id);
                    client.Close();
                }
                else
                {
                    client.Close();
                    throw new GraphQlException("Collection was not found.", HttpStatusCode.NotFound);
                }

                return card;
            }
            catch (DatabaseWriteException e)
            {
                throw new GraphQlException(e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Error while saving card.", HttpStatusCode.InternalServerError);
            }
        }

        public Collection PutCollection(Collection collection, int ownerId)
        {
            try
            {
                var queryClient = new AerospikeQueryClient();
                queryClient.GetUserInfo(ownerId);
                var client = new AerospikeWriteClient();
                client.Put(collection);
                client.AddCollectionToUser(ownerId, collection.Id);
                client.Close();
                return collection;
            }
            catch (DatabaseException e)
            {
                if (e.InnerException is AlreadyExistsException)
                    throw new GraphQlException("Collection already exists. " + e.Message, HttpStatusCode.Conflict);
                throw new GraphQlException("User with this id not found. " + e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Error while saving collection.", HttpStatusCode.InternalServerError);
            }
        }

        public User DeleteUser(int id)
        {
            try
            {
                var rc = new AerospikeQueryClient();
                var user = rc.GetUserInfo(id);
                rc.Close();
                var c = new AerospikeWriteClient();
                c.Delete(Entities.User, id);
                c.Close();
                return user;
            }
            catch (DatabaseWriteException e)
            {
                throw new GraphQlException(e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Error while deleting user.", HttpStatusCode.InternalServerError);
            }
        }

        public Card DeleteCard(int id)
        {
            try
            {
                var rc = new AerospikeQueryClient();
                var card = rc.GetCard(id);
                rc.Close();
                var c = new AerospikeWriteClient();
                c.Delete(Entities.Card, id);
                c.Close();
                return card;
            }
            catch (DatabaseWriteException e)
            {
                throw new GraphQlException(e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Error while deleting card.", HttpStatusCode.InternalServerError);
            }
        }

        public Collection DeleteCollection(int id)
        {
            try
            {
                var rc = new AerospikeQueryClient();
                var collection = rc.GetCollectionInfo(id);
                rc.Close();
                var c = new AerospikeWriteClient();
                c.Delete(Entities.Collection, id);
                c.Close();
                return collection;
            }
            catch (DatabaseWriteException e)
            {
                throw new GraphQlException(e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Error while deleting collection.", HttpStatusCode.InternalServerError);
            }
        }

        public UserInfo UpdateUser(UserInfo user, int id)
        {
            try
            {
                var c = new AerospikeWriteClient();
                User u = new User()
                {
                    Login = user.Login,
                    Email = user.Email
                };
                c.Update(u, id);
                c.Close();
                return user;
            }
            catch (DatabaseWriteException e)
            {
                throw new GraphQlException(e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Error while updating user.", HttpStatusCode.InternalServerError);
            }
        }

        public CardInfo UpdateCard(CardInfo card, int id)
        {
            try
            {
                var c = new AerospikeWriteClient();
                Card u = new Card()
                {
                    FrontSide = card.FrontSide,
                    BackSide = card.BackSide
                };
                c.Update(u, id);
                c.Close();
                return card;
            }
            catch (DatabaseWriteException e)
            {
                throw new GraphQlException(e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Error while updating card.", HttpStatusCode.InternalServerError);
            }
        }

        public CollectionInfo UpdateCollection(CollectionInfo collection, int id)
        {
            try
            {
                var c = new AerospikeWriteClient();
                Collection u = new Collection()
                {
                    Name = collection.Name,
                    Description = collection.Description
                };
                c.Update(u, id);
                c.Close();
                return collection;
            }
            catch (DatabaseWriteException e)
            {
                throw new GraphQlException(e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Error while updating collection.", HttpStatusCode.InternalServerError);
            }
        }
    }
}
