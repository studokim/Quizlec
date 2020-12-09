using System.Collections.Generic;

namespace Quizleç.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> Cards { get; set; }
    }
}
