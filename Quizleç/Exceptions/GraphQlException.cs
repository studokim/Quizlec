using System;
using System.Net;
using HotChocolate;

namespace Quizleç.Exceptions
{
    public class GraphQlException : HotChocolate.GraphQLException
    {
        public GraphQlException() { }

        public GraphQlException(string message)
            : base(message) { }

        public GraphQlException(string message, HttpStatusCode code, Exception inner = null)
            : base(ErrorBuilder.New()
                .SetMessage(message)
                .SetCode(code.ToString())
                .SetException(inner)
                .Build()) { }
    }
}
