using System.Collections.Generic;

namespace GitLab.Parker.Abstractions
{
    public class Environment
    {
        public string Name { get; set; } = default!;
        public IList<User> TakenBy { get; set; } = new List<User>();
    }
}