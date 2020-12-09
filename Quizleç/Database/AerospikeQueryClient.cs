using System;
using System.Collections;
using System.Collections.Generic;
using Aerospike.Client;
using Quizleç.Exceptions;
using Quizleç.Models;
using User = Quizleç.Models.User;

namespace Quizleç.Database
{
    public class AerospikeQueryClient : AerospikeClient
    {
        public AerospikeQueryClient() : base(false)
        {
        }

        public User GetUserInfo(int id)
        {
            Key key = MakeKey(Entities.User, id);
            if (Exists(key))
            {
                var r = Client.Get(Policy, key, "Id", "Login", "PasswordHash", "Email");
                return new User()
                {
                    Id = id, Login = r.GetString("Login"),
                    PasswordHash = r.GetString("PasswordHash"),
                    Email = r.GetString("Email"),
                };
            }
            else
                throw new UserNotFoundException
                    ($"The user with id={id} does not exist.");
        }

        public Collection GetCollectionInfo(int id)
        {
            Key key = MakeKey(Entities.Collection, id);
            if (Exists(key))
            {
                var r = Client.Get(Policy, key, "Id", "Name", "Description", "Cards");
                return new Collection()
                {
                    Id = id, Name = r.GetString("Name"),
                    Description = r.GetString("Description")
                };
            }
            else
                throw new CollectionNotFoundException
                    ($"The collection with id={id} does not exist.");
        }

        public Card GetCard(int id)
        {
            Key key = MakeKey(Entities.Card, id);
            if (Exists(key))
            {
                var r = Client.Get(Policy, key, "Id", "FrontSide", "BackSide");
                return new Card()
                {
                    Id = id, FrontSide = r.GetString("FrontSide"),
                    BackSide = r.GetString("BackSide"),
                };
            }
            else
                throw new CardNotFoundException
                    ($"The card with id={id} does not exist.");
        }

        public User GetUserInfoByLogin(string login)
        {
            try
            {
                List<User> res = new List<User>();
                Statement stmt = new Statement();
                stmt.SetNamespace(Options.Namespace);
                stmt.SetSetName(Options.Set.User);
                stmt.SetBinNames("Id", "Login", "PasswordHash", "Email", "IsActive");
                stmt.Filter = Filter.Equal("Login", login);
                RecordSet rs = Client.Query((QueryPolicy) Policy, stmt);
                while (rs.Next())
                {
                    Record r = rs.Record;
                    if (r.GetBool("IsActive"))
                        res.Add(new User()
                        {
                            Id = r.GetInt("Id"),
                            Login = login,
                            Email = r.GetString("Email"),
                        });
                }

                rs.Dispose();

                if (res.Count == 1)
                    return res[0];
                if (res.Count == 0)
                    throw new UserNotFoundException
                        ($"The user with login={login} doesn't exist.");
                if (res.Count > 1)
                    throw new DatabaseQueryException
                        ($"Too many users with login={login}: {res.Count}.");
                else
                    throw new DatabaseQueryException();
            }
            catch (AerospikeException e)
            {
                throw new DatabaseQueryException
                    ("Please create User:Login index first.", e);
            }
            catch (Exception e)
            {
                throw new DatabaseQueryException("Can't get user by login", e);
            }
        }

        // TODO: we can't search only by Name, because John can have Default collection and James too
        /*public Collection GetCollectionInfoByName(string name)
        {
            try
            {
                List<Collection> res = new List<Collection>();
                Statement stmt = new Statement();
                stmt.SetNamespace(Options.Namespace);
                stmt.SetSetName(Options.Set.Collection);
                stmt.SetBinNames("Id", "Name", "Description", "Owner", "IsActive");
                stmt.Filter = Filter.Equal("Name", name);
                RecordSet rs = Client.Query((QueryPolicy) Policy, stmt);
                while (rs.Next())
                {
                    Record r = rs.Record;
                    if (r.GetBool("IsActive"))
                        res.Add(new Collection()
                        {
                            Id = r.GetInt("Id"),
                            Name = name,
                            Description = r.GetString("Description"),
                        });
                }

                rs.Dispose();

                if (res.Count <= 0)
                    throw new UserNotFoundException
                        ($"The collection with name={name} doesn't exist.");
                if (res.Count > 1)
                    throw new DatabaseQueryException
                        ($"Too many collections with name={name}: {res.Count}.");
                return res[0];
            }
            catch (AerospikeException e)
            {
                throw new DatabaseQueryException
                    ("Please create Collection:Name index first.", e);
            }
            catch (Exception e)
            {
                throw new DatabaseQueryException("Can't get collection by name", e);
            }
        }*/

        public List<Card> GetCardsByCollectionId(int id)
        {
            Key key = MakeKey(Entities.Collection, id);
            if (Exists(key))
            {
                IList cardIds = Client.Get(Policy, key, "Cards").GetList("Cards");
                if (cardIds.Count <= 0)
                    throw new CardNotFoundException
                        ($"There's no cards in collection with id={id} yet.");
                List<Card> res = new List<Card>();
                foreach (var i in cardIds)
                {
                    try
                    {
                        //  Returned IList consists of long, therefore we need to cast
                        // TODO: fix when Ids are not int
                        res.Add(GetCard((int) ((long) i % int.MaxValue)));
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                return res;
            }
            else
                throw new CollectionNotFoundException
                    ($"The collection with id={id} does not exist.");
        }

        public List<Collection> GetCollectionsByUserId(int id)
        {
            Key key = MakeKey(Entities.User, id);
            if (Exists(key))
            {
                IList collectionIds = Client.Get(Policy, key, "Collections").GetList("Collections");
                if (collectionIds.Count <= 0)
                    throw new CardNotFoundException
                        ($"The user with id={id} has no collections yet.");
                var res = new List<Collection>();
                foreach (var i in collectionIds)
                {
                    try
                    {
                        //  Returned IList consists of long, therefore we need to cast
                        // TODO: fix when Ids are not int
                        res.Add(GetCollectionInfo((int) ((long) i % int.MaxValue)));
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                return res;
            }
            else
                throw new CollectionNotFoundException
                    ($"The user with id={id} does not exist.");
        }

        public List<Card> GetCardsByUserId(int id)
        {
            Key key = MakeKey(Entities.User, id);
            if (Exists(key))
            {
                IList collectionIds = Client.Get(Policy, key, "Collections").GetList("Collections");
                if (collectionIds.Count <= 0)
                    throw new CardNotFoundException
                        ($"The user with id={id} has no collections yet.");
                var res = new List<Card>();
                foreach (var i in collectionIds)
                {
                    try
                    {
                        //  Returned IList consists of long, therefore we need to cast
                        // TODO: fix when Ids are not int
                        res.AddRange(GetCardsByCollectionId((int) ((long) i % int.MaxValue)));
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                return res;
            }
            else
                throw new UserNotFoundException
                    ($"The user with id={id} was deleted.");
        }
    }
}
