using System.Collections.Generic;

namespace Quizleç.GraphQL.Models
{
    public class CollectionWithCards
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Quizleç.Models.Card> Cards { get; set; }
        public int CardsCount { get; set; }
    }
}
