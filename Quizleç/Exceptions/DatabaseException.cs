using System;

namespace Quizleç.Exceptions
{
    public class DatabaseQueryException : Exception
    {
        public DatabaseQueryException() { }

        public DatabaseQueryException(string message)
            : base(message) { }

        public DatabaseQueryException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class DatabaseWriteException : Exception
    {
        public DatabaseWriteException() { }

        public DatabaseWriteException(string message)
            : base(message) { }

        public DatabaseWriteException(string message, Exception inner)
            : base(message, inner) { }
    }
}
