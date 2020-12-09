using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HotChocolate;
using Quizleç.Database;
using Quizleç.Exceptions;
using Quizleç.Models;

namespace Quizleç.GraphQL
{
    public class Query
    {
        public string Hello() => "World!";

        public User GetUserInfoByLogin(string login)
        {
            try
            {
                var client = new AerospikeQueryClient();
                return client.GetUserInfoByLogin(login);
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

        public Collection GetCollectionInfoByName(string name)
        {
            try
            {
                var client = new AerospikeQueryClient();
                return client.GetCollectionInfoByName(name);
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

        public List<Card> GetCardsByCollectionId(int id)
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

        public List<Collection> GetCollectionsByUserId(int id)
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

        public List<Card> GetCardsByUserId(int id)
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

        public User GetUser(int id)
        {
            try
            {
                var client = new AerospikeQueryClient();
                return client.GetUserInfo(id);
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

        public Card GetCard(int id)
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

        public Collection GetCollection(int id)
        {
            try
            {
                var client = new AerospikeQueryClient();
                return client.GetCollectionInfo(id);
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
