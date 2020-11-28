namespace Quizleç.Config
{
    public class AppSettings
    {
        public Logging Logging { get; set; }
        public Aerospike Aerospike { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
        public string MicrosoftHostingLifetime { get; set; }
    }

    public class Aerospike
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Namespace { get; set; }
        public Set Set { get; set; }
    }

    public class Set
    {
        public User User { get; set; }
        public Collection Collection { get; set; }
        public Card Card { get; set; }
    }

    public class User
    {
        public string SetName { get; set; }
        public string Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Collections { get; set; }
        public string IsActive { get; set; }
    }

    public class Collection
    {
        public string SetName { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string Cards { get; set; }
        public string IsActive { get; set; }
    }

    public class Card
    {
        public string SetName { get; set; }
        public string Id { get; set; }
        public string FrontSide { get; set; }
        public string BackSide { get; set; }
        public string IsActive { get; set; }
    }
}
