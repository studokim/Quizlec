using System.Collections.Generic;

namespace Quizleç.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public List<int> Collections { get; set; }
    }
}
