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
        public string User { get; set; }
        public string Collection { get; set; }
        public string Card { get; set; }
    }
}
