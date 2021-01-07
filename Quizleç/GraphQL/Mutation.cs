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
        public HttpStatusCode PutUser(User user)
        {
            try
            {
                var c = new AerospikeWriteClient();
                c.Put(user);
                c.Close();
                return HttpStatusCode.OK;
            }
            catch (DatabaseWriteException)
            {
                return HttpStatusCode.Conflict;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        public HttpStatusCode PutCard(Card card, int collectionId)
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
                    return HttpStatusCode.NotFound;
                }

                return HttpStatusCode.OK;
            }
            catch (DatabaseWriteException)
            {
                return HttpStatusCode.NotFound;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        public HttpStatusCode PutCollection(Collection collection, int ownerId)
        {
            try
            {
                var queryClient = new AerospikeQueryClient();
                queryClient.GetUserInfo(ownerId);
                var client = new AerospikeWriteClient();
                client.Put(collection);
                client.AddCollectionToUser(ownerId, collection.Id);
                client.Close();
                return HttpStatusCode.OK;
            }
            catch (DatabaseException e)
            {
                if (e.InnerException is AlreadyExistsException)
                    return HttpStatusCode.Conflict;
                return HttpStatusCode.NotFound;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        public HttpStatusCode DeleteUser(int id)
        {
            try
            {
                var c = new AerospikeWriteClient();
                c.Delete(Entities.User, id);
                c.Close();
                return HttpStatusCode.OK;
            }
            catch (DatabaseWriteException)
            {
                return HttpStatusCode.NotFound;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        public HttpStatusCode DeleteCard(int id)
        {
            try
            {
                var c = new AerospikeWriteClient();
                c.Delete(Entities.Card, id);
                c.Close();
                return HttpStatusCode.OK;
            }
            catch (DatabaseWriteException)
            {
                return HttpStatusCode.NotFound;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        public HttpStatusCode DeleteCollection(int id)
        {
            try
            {
                var c = new AerospikeWriteClient();
                c.Delete(Entities.Collection, id);
                c.Close();
                return HttpStatusCode.OK;
            }
            catch (DatabaseWriteException)
            {
                return HttpStatusCode.NotFound;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        public HttpStatusCode UpdateUser(UserInfo user, int id)
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
                return HttpStatusCode.OK;
            }
            catch (DatabaseWriteException)
            {
                return HttpStatusCode.NotFound;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        public HttpStatusCode UpdateCard(CardInfo card, int id)
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
                return HttpStatusCode.OK;
            }
            catch (DatabaseWriteException)
            {
                return HttpStatusCode.NotFound;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        public HttpStatusCode UpdateCollection(CollectionInfo collection, int id)
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
                return HttpStatusCode.OK;
            }
            catch (DatabaseWriteException)
            {
                return HttpStatusCode.NotFound;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
