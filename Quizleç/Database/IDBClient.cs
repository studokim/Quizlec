using System.Collections.Generic;
using Aerospike.Client;

namespace Quizleç.Database
{
    internal interface IDBClient
    {
        public void Open();
        public void Close();
    }
}
