using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GitLab.Parker.Abstractions;
using GitLab.Parker.Configuration;
using Microsoft.Extensions.Options;
using Environment = GitLab.Parker.Abstractions.Environment;

namespace GitLab.Parker.Logic
{
    public class EnvironmentStorage : IEnvironmentStorage
    {
        private readonly EnvironmentsOptions environmentsOptions;
        private EnvironmentsData environmentsData;

        public EnvironmentStorage(
            IOptions<EnvironmentsOptions> environmentsOptions,
            IOptionsMonitor<EnvironmentsData> envOptionsMonitor)
        {
            this.environmentsOptions = environmentsOptions.Value;
            this.environmentsData = envOptionsMonitor.CurrentValue;
            envOptionsMonitor.OnChange(OnDataChange);
        }

        public async Task<IReadOnlyList<string>> GetAllEnvironmentNamesAsync()
        {
            return environmentsData.EnvironmentsState.Select(x => x.Name).ToList();
        }

        public async Task<IReadOnlyList<string>> GetFreeEnvironmentNamesAsync()
        {
            return environmentsData.EnvironmentsState.Where(x => !x.TakenBy.Any()).Select(x => x.Name).ToList();
        }

        public async Task<OperationResult> AddAsync(string environmentName)
        {
            var adjustedName = environmentName.Trim().ToLowerInvariant();

            if (IsEnvProhibited(environmentName))
            {
                return OperationResult.Error(ErrorType.EnvironmentProhibited);
            }

            if (IsEnvExist(adjustedName))
            {
                return OperationResult.Error(ErrorType.EnvironmentExistsAlready);
            }

            environmentsData.EnvironmentsState.Add(new Environment
            {
                Name = adjustedName
            });

            await SaveToFile();

            return OperationResult.Ok();
        }

        public async Task<OperationResult> SetTakenAsync(string environmentName, User user)
        {
            var adjustedName = environmentName.Trim().ToLowerInvariant();

            if (!IsEnvExist(adjustedName))
            {
                return OperationResult.Error(ErrorType.EnvironmentDoesNotExist);
            }

            var requestedEnv = environmentsData.EnvironmentsState.First(x => x.Name.Equals(adjustedName, StringComparison.InvariantCultureIgnoreCase));

            if (requestedEnv.TakenBy.Any())
            {
                return requestedEnv.TakenBy.First().ChatId == user.ChatId
                    ? OperationResult.Error(ErrorType.EnvironmentTakenBySelf)
                    : OperationResult.Error(ErrorType.EnvironmentTaken, requestedEnv.TakenBy.First().TelegramNickname);
            }

            requestedEnv.TakenBy = new List<User>
            {
                user
            };

            await SaveToFile();

            return OperationResult.Ok();
        }

        public async Task<OperationResult> FreeAsync(string environmentName, long senderId)
        {
            var adjustedName = environmentName.Trim().ToLowerInvariant();

            if (!IsEnvExist(adjustedName))
            {
                return OperationResult.Error(ErrorType.EnvironmentDoesNotExist);
            }

            var requestedEnv = environmentsData.EnvironmentsState.First(x => x.Name.Equals(adjustedName, StringComparison.InvariantCultureIgnoreCase));

            if (!requestedEnv.TakenBy.Any())
            {
                return OperationResult.Ok();
            }

            if (requestedEnv.TakenBy.First().ChatId != senderId)
            {
                return OperationResult.Error(ErrorType.Forbidden, requestedEnv.TakenBy.First().TelegramNickname);
            }

            requestedEnv.TakenBy = new List<User>();

            await SaveToFile();

            return OperationResult.Ok();
        }

        private bool IsEnvExist(string name)
        {
            return environmentsData.EnvironmentsState.Any(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        private bool IsEnvProhibited(string name)
        {
            return environmentsOptions.Exclusions.Contains(name, StringComparer.InvariantCultureIgnoreCase);
        }

        private async Task SaveToFile()
        {
            var serializedData = JsonSerializer.Serialize(
                environmentsData,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            await File.WriteAllTextAsync("data.json", serializedData);
        }

        private void OnDataChange(EnvironmentsData data)
        {
            this.environmentsData = data;
        }
    }
}