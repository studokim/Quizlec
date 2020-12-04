using System;
using Quizleç.Database;
using Quizleç.Models;

namespace Quizleç.GraphQL
{
    public class Query
    {
        public string Hello() => "World!";

        public User GetUser(int id)
        {
            var c = new AerospikeQueryClient();
            var u = c.GetUserInfo(id);
            c.Close();
            Console.WriteLine(u.Email);
            return u;
        }
    }

    public class Mutation
    {
        public User PutUser(User user)
        {
            var c = new AerospikeWriteClient();
            c.PutUser(user);
            c.Close();
            return user;
        }
    }

    /*public static class Example
    {

        public static void Run()
        {
            var schema = SchemaBuilder.New()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .Create();

            var executor = schema.MakeExecutable();
            //Console.WriteLine(executor.Execute("{ hello }").ToJson());
        }
    }*/
}
