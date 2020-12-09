using System.Collections.Generic;
using GitLab.Parker.Abstractions;

namespace GitLab.Parker.Configuration
{
    public class EnvironmentsData
    {
        public IList<Environment> EnvironmentsState { get; } = new List<Environment>();
    }
}