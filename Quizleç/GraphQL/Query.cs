using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HotChocolate;
using Quizleç.Database;
using Quizleç.Exceptions;
using Quizleç.GraphQL.Models;

namespace Quizleç.GraphQL
{
    public class Query
    {
        public string Hello() => "World!";

        public UserWithCollections GetUser(string login)
        {
            try
            {
                var client = new AerospikeQueryClient();
                var user = client.GetUserInfoByLogin(login);
                var collections = client.GetCollectionsByUserId(user.Id);
                client.Close();
                var res = new UserWithCollections()
                {
                    Id = user.Id, Login = user.Login, Email = user.Email,
                    Collections = collections, CollectionsCount = collections.Count
                };
                return res;
            }
            catch (NotFoundException e)
            {
                throw new GraphQlException("User not found. " + e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Could not get user.", HttpStatusCode.InternalServerError);
            }
        }

        public List<Quizleç.Models.Card> GetCardsByCollectionId(int id)
        {
            try
            {
                var client = new AerospikeQueryClient();
                return client.GetCardsByCollectionId(id);
            }
            catch (NotFoundException e)
            {
                throw new GraphQlException("Cards not found. " + e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Could not get cards.", HttpStatusCode.InternalServerError);
            }
        }

        public List<Quizleç.Models.Card> GetCardsByUserId(int id)
        {
            try
            {
                var client = new AerospikeQueryClient();
                return client.GetCardsByUserId(id);
            }
            catch (NotFoundException e)
            {
                throw new GraphQlException("Cards or user not found. " + e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Could not get cards.", HttpStatusCode.InternalServerError);
            }
        }

        public List<Quizleç.Models.Collection> GetCollectionsByUserId(int id)
        {
            try
            {
                var client = new AerospikeQueryClient();
                return client.GetCollectionsByUserId(id);
            }
            catch (NotFoundException e)
            {
                throw new GraphQlException("Collections or user not found. " + e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Could not get collections.", HttpStatusCode.InternalServerError);
            }
        }

        public UserWithCollections GetUser(int id)
        {
            try
            {
                var client = new AerospikeQueryClient();
                var user = client.GetUserInfo(id);
                var collections = client.GetCollectionsByUserId(id);
                client.Close();
                var res = new UserWithCollections()
                {
                    Id = user.Id, Login = user.Login, Email = user.Email,
                    Collections = collections, CollectionsCount = collections.Count
                };
                return res;
            }
            catch (NotFoundException e)
            {
                throw new GraphQlException("User not found. " + e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Could not get user.", HttpStatusCode.InternalServerError);
            }
        }

        public Quizleç.Models.Card GetCard(int id)
        {
            try
            {
                var client = new AerospikeQueryClient();
                return client.GetCard(id);
            }
            catch (NotFoundException e)
            {
                throw new GraphQlException("Card not found. " + e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Could not get card.", HttpStatusCode.InternalServerError);
            }
        }

        public CollectionWithCards GetCollection(int id)
        {
            try
            {
                var client = new AerospikeQueryClient();
                var collection = client.GetCollectionInfo(id);
                var cards = client.GetCardsByCollectionId(id);
                client.Close();
                var res = new CollectionWithCards()
                {
                    Id = collection.Id, Name = collection.Name, Description = collection.Description,
                    Cards = cards, CardsCount = cards.Count
                };
                return res;
            }
            catch (NotFoundException e)
            {
                throw new GraphQlException("Collection not found. " + e.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new GraphQlException("Could not get collection.", HttpStatusCode.InternalServerError);
            }
        }
    }
}
