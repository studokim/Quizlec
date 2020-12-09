using System.Collections.Generic;

namespace Quizleç.GraphQL.Models
{
    public class UserWithCollections
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public List<Quizleç.Models.Collection> Collections { get; set; }
        public int CollectionsCount { get; set; }
    }
}
