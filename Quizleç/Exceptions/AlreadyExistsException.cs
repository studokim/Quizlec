using System;

namespace Quizleç.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() { }

        public AlreadyExistsException(string message)
            : base(message) { }

        public AlreadyExistsException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class UserAlreadyExistsException : AlreadyExistsException
    {
        public UserAlreadyExistsException() { }

        public UserAlreadyExistsException(string message)
            : base(message) { }

        public UserAlreadyExistsException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class CollectionAlreadyExistsException : AlreadyExistsException
    {
        public CollectionAlreadyExistsException() { }

        public CollectionAlreadyExistsException(string message)
            : base(message) { }

        public CollectionAlreadyExistsException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class CardAlreadyExistsException : AlreadyExistsException
    {
        public CardAlreadyExistsException() { }

        public CardAlreadyExistsException(string message)
            : base(message) { }

        public CardAlreadyExistsException(string message, Exception inner)
            : base(message, inner) { }
    }
}
