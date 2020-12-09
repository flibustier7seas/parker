using System;
using System.Threading.Tasks;
using GitLab.Parker.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GitLab.Parker.BotCommands
{
    public class FreeCommand : IBotCommand
    {
        private readonly IEnvironmentStorage environmentStorage;
        private readonly IBotService botService;

        public FreeCommand(
            IEnvironmentStorage environmentStorage,
            IBotService botService)
        {
            this.environmentStorage = environmentStorage;
            this.botService = botService;
        }

        public bool CanExecute(Message message)
        {
            return message.Type == MessageType.Text && message.Text.StartsWith(@"/free");
        }

        public async Task ExecuteAsync(Message message)
        {
            var messageParts = message.Text.Split(' ');
            if (messageParts.Length != 2)
            {
                await message.ReplyAsync(botService, @"Я вас не понял! После /free нужно написать пробел и название площадки");
                return;
            }

            if (messageParts[1].Equals("all", StringComparison.InvariantCultureIgnoreCase))
            {
                await FreeAllAsync(message);
                return;
            }

            var result = await environmentStorage.FreeAsync(messageParts[1], message.Chat.Id);

            if (result.IsSuccess)
            {
                await message.ReplyAsync(botService, @"Площадка освобождена");
            }

            switch (result.ErrorType)
            {
                case ErrorType.Forbidden:
                    await message.ReplyAsync(botService, $"Площадку может освободить только тот, кто ее занял, это был @{result.Message}");
                    break;
                case ErrorType.EnvironmentDoesNotExist:
                    await message.ReplyAsync(botService, "Такой площадки нет");
                    break;
            }
        }

        private async Task FreeAllAsync(Message message)
        {
            var allEnvNames = await environmentStorage.GetAllEnvironmentNamesAsync();

            foreach (var envName in allEnvNames)
            {
                var freeResult = await environmentStorage.FreeAsync(envName, message.Chat.Id);
                if (!freeResult.IsSuccess)
                {
                    await message.ReplyAsync(botService, $"Площадку {envName} не удалось освободить, похоже, ее заняли не вы");
                }
            }

            await message.ReplyAsync(botService, $"Все возможные площадки освобождены");
        }
    }
}