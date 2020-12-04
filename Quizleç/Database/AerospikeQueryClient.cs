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
        public AerospikeQueryClient() : base(false) { }

        public User GetUserInfo(int id)
        {
            try
            {
                var r = Client.Get(Policy, MakeKey(Entities.User, id),
                    "Id", "Login", "PasswordHash", "Email", "IsActive");
                if (r.GetBool("IsActive"))
                    return new User()
                    {
                        Id = id, Login = r.GetString("Login"),
                        PasswordHash = r.GetString("PasswordHash"),
                        Email = r.GetString("Email"),
                    };
                else
                    throw new UserNotFoundException
                        ($"The user with id={id} was deleted.");
            }
            catch (Exception e)
            {
                throw new UserNotFoundException
                    ($"The user with id={id} doesn't exist.", e);
            }
        }

        public Collection GetCollectionInfo(int id)
        {
            try
            {
                var r = Client.Get(Policy, MakeKey(Entities.Collection, id),
                    "Id", "Name", "Description", "Owner", "IsActive");
                if (r.GetBool("IsActive"))
                {
                    return new Collection()
                    {
                        Id = id, Name = r.GetString("Name"),
                        Description = r.GetString("Description"),
                        Owner = r.GetInt("Owner"),
                    };
                }
                else
                    throw new CollectionNotFoundException
                        ($"The collection with id={id} was deleted.");
            }
            catch (Exception e)
            {
                throw new CollectionNotFoundException
                    ($"The collection with id={id} doesn't exist.", e);
            }
        }

        public Card GetCard(int id)
        {
            try
            {
                var r = Client.Get(Policy, MakeKey(Entities.Card, id),
                    "Id", "FrontSide", "BackSide");
                if (r.GetBool("IsActive"))
                    return new Card()
                    {
                        Id = id, FrontSide = r.GetString("FrontSide"),
                        BackSide = r.GetString("BackSide")
                    };
                else
                    throw new CardNotFoundException
                        ($"The card with id={id} was deleted.");
            }
            catch (Exception e)
            {
                throw new CardNotFoundException
                    ($"The card with id={id} doesn't exist.", e);
            }
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

                if (res.Count == 1)
                    return res[0];
                if (res.Count == 0)
                    throw new UserNotFoundException
                        ($"The user with login={login} doesn't exist.");
                if (res.Count > 1)
                    throw new DatabaseException
                        ($"Too many users with login={login}: {res.Count}.");
                else
                    throw new DatabaseException();
            }
            catch (AerospikeException e)
            {
                throw new DatabaseException
                    ("Please create User:Login index first.", e);
            }
            catch (Exception e)
            {
                throw new DatabaseException("Can't get user by login", e);
            }
        }

        public Collection GetCollectionInfoByName(string name)
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
                            Owner = r.GetInt("Owner"),
                        });
                }

                if (res.Count <= 0)
                    throw new UserNotFoundException
                        ($"The collection with name={name} doesn't exist.");
                if (res.Count > 1)
                    throw new DatabaseException
                        ($"Too many collections with name={name}: {res.Count}.");
                return res[0];
            }
            catch (AerospikeException e)
            {
                throw new DatabaseException
                    ("Please create Collection:Name index first.", e);
            }
            catch (Exception e)
            {
                throw new DatabaseException("Can't get collection by name", e);
            }
        }

        public List<Card> GetCardsByCollectionId(int id)
        {
            try
            {
                Key key = MakeKey(Entities.Collection, id);
                Record r = Client.Get(Policy, key, "Cards", "IsActive");
                IList cardIds = r.GetList("Cards");
                if (r.GetBool("IsActive"))
                {
                    if (cardIds.Count <= 0)
                        throw new CardNotFoundException
                            ($"There's no cards in collection with id={id} yet.");
                    List<Card> res = new List<Card>();
                    foreach (var i in cardIds)
                    {
                        //  Returned IList consists of long, therefore we need to cast
                        // TODO: fix when Ids are not int
                        res.Add(GetCard((int)((long)i % int.MaxValue)));
                    }

                    return res;
                }
                else
                    throw new CollectionNotFoundException
                        ($"The collection with id={id} was deleted.");
            }
            catch (Exception e)
            {
                throw new DatabaseException
                    ($"Can't get cards of the collection with id={id}.", e);
            }
        }

        public List<Collection> GetCollectionsByUserId(int id)
        {
            try
            {
                Key key = MakeKey(Entities.User, id);
                Record r = Client.Get(Policy, key, "Collections", "IsActive");
                IList collectionIds = r.GetList("Collections");
                if (r.GetBool("IsActive"))
                {
                    if (collectionIds.Count <= 0)
                        throw new CardNotFoundException
                            ($"The user with id={id} has no collections yet.");
                    var res = new List<Collection>();
                    foreach (var i in collectionIds)
                    {
                        //  Returned IList consists of long, therefore we need to cast
                        // TODO: fix when Ids are not int
                        res.Add(GetCollectionInfo((int)((long)i % int.MaxValue)));
                    }

                    return res;
                }
                else
                    throw new CollectionNotFoundException
                        ($"The user with id={id} was deleted.");
            }
            catch (Exception e)
            {
                throw new DatabaseException
                    ($"Can't get collections of the user with id={id}.", e);
            }
        }

        public List<Card> GetCardsByUserId(int id)
        {
            try
            {
                Key key = MakeKey(Entities.User, id);
                Record r = Client.Get(Policy, key, "Collections", "IsActive");
                IList collectionIds = r.GetList("Collections");
                if (r.GetBool("IsActive"))
                {
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
                            res.AddRange(GetCardsByCollectionId((int)((long)i % int.MaxValue)));
                        }
                        catch (CardNotFoundException) {}
                    }

                    return res;
                }
                else
                    throw new UserNotFoundException
                        ($"The user with id={id} was deleted.");
            }
            catch (Exception e)
            {
                throw new DatabaseException
                    ($"Can't get cards of the user with id={id}.", e);
            }
        }
    }
}
