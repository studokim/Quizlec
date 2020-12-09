using System;

namespace Quizleç.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() { }

        public NotFoundException(string message)
            : base(message) { }

        public NotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException() { }

        public UserNotFoundException(string message)
            : base(message) { }

        public UserNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class CollectionNotFoundException : NotFoundException
    {
        public CollectionNotFoundException() { }

        public CollectionNotFoundException(string message)
            : base(message) { }

        public CollectionNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class CardNotFoundException : NotFoundException
    {
        public CardNotFoundException() { }

        public CardNotFoundException(string message)
            : base(message) { }

        public CardNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}
