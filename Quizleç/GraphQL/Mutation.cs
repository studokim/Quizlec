using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Quizleç.Database;
using Quizleç.Exceptions;
using Quizleç.Models;

namespace Quizleç.GraphQL
{
    public class Mutation
    {
        public User PutUser(User user)
        {
            var c = new AerospikeWriteClient();
            c.Put(user);
            c.Close();
            return user;
        }

        public HttpStatusCode PutCardToCollection(Card card, int collectionId)
        {
            try
            {
                var queryClient = new AerospikeQueryClient();
                queryClient.GetCollectionInfo(collectionId);
                var client = new AerospikeWriteClient();
                client.Put(card);
                client.AddCardToCollection(collectionId, card.Id);
                client.Close();
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
    }
}
