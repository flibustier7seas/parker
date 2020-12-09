using System.Collections.Generic;

namespace GitLab.Parker.Abstractions
{
    public class EnvironmentsOptions
    {
        public IReadOnlyList<string> Exclusions { get; set; } = default!;
    }
}