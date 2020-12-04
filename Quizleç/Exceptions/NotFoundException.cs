using System;

namespace Quizleç.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() { }

        public UserNotFoundException(string message)
            : base(message) { }

        public UserNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class CollectionNotFoundException : Exception
    {
        public CollectionNotFoundException() { }

        public CollectionNotFoundException(string message)
            : base(message) { }

        public CollectionNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class CardNotFoundException : Exception
    {
        public CardNotFoundException() { }

        public CardNotFoundException(string message)
            : base(message) { }

        public CardNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}
