using System;

namespace Quizleç.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException() { }

        public DatabaseException(string message)
            : base(message) { }

        public DatabaseException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class DatabaseQueryException : DatabaseException
    {
        public DatabaseQueryException() { }

        public DatabaseQueryException(string message)
            : base(message) { }

        public DatabaseQueryException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class DatabaseWriteException : DatabaseException
    {
        public DatabaseWriteException() { }

        public DatabaseWriteException(string message)
            : base(message) { }

        public DatabaseWriteException(string message, Exception inner)
            : base(message, inner) { }
    }
}
