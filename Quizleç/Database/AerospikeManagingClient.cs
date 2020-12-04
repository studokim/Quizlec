using Aerospike.Client;
using Quizleç.Models;

namespace Quizleç.Database
{
    public class AerospikeManagingClient : AerospikeClient
    {
        public AerospikeManagingClient() : base(true) {}

        public void MakeIndexes()
        {
            IndexTask task = Client.CreateIndex(Policy, Options.Namespace,
                Options.Set.Collection, "nameCollectionInd",
                "Name", IndexType.STRING);
            task.Wait();
            task = Client.CreateIndex(Policy, Options.Namespace,
                Options.Set.User, "loginUserInd",
                "Login", IndexType.STRING);
            task.Wait();
        }
    }
}
