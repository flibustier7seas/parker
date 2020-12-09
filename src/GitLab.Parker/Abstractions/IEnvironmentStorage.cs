using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitLab.Parker.Abstractions
{
    public interface IEnvironmentStorage
    {
        Task<IReadOnlyList<string>> GetAllEnvironmentNamesAsync();
        Task<IReadOnlyList<string>> GetFreeEnvironmentNamesAsync();
        Task<OperationResult> AddAsync(string environmentName);
        Task<OperationResult> SetTakenAsync(string environmentName, User user);
        Task<OperationResult> FreeAsync(string environmentName, long senderId);
    }
}