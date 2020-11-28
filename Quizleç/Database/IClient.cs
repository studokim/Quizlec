using System.Collections.Generic;
using Aerospike.Client;

namespace Quizleç.Database
{
    internal interface IClient
    {
        public void Open();
        public void Close();
    }
}
