using Aerospike.Client;

namespace Quizleç.Database
{
    public abstract class AerospikeClient : IClient
    {
        protected readonly Config.Aerospike Options;
        protected readonly Policy Policy;
        protected AsyncClient Client;

        protected AerospikeClient(bool write)
        {
            if (write)
                Policy = new WritePolicy();
            else
                Policy = new QueryPolicy();
            // TODO: use Environment variables(?) instead of hardcoding.
            Options = Config.Get.AerospikeOptions("Development");
            Open();
        }

        ~AerospikeClient()
        {
            Close();
        }

        public void Open()
        {
            Client = new AsyncClient(Options.Hostname, Options.Port);
        }

        public void Close()
        {
            Client.Close();
        }
    }
}
