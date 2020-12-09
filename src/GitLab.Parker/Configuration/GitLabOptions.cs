using System;

namespace GitLab.Parker.Configuration
{
    public class GitLabOptions
    {
        public int ProjectGroupId { get; set; } = default!;
        public string[] ExcludingProjects { get; set; } = Array.Empty<string>();
    }
}