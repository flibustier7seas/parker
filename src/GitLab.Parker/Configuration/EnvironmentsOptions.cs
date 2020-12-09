using System.Collections.Generic;

namespace GitLab.Parker.Configuration
{
    public class EnvironmentsOptions
    {
        public IList<string> Exclusions { get; set; } = new List<string>();
    }
}