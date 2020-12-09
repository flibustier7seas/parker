using System.Threading.Tasks;
using GitLab.Parker.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = GitLab.Parker.Abstractions.User;

namespace GitLab.Parker.BotCommands
{
    public class TakeCommand : IBotCommand
    {
        private readonly IEnvironmentStorage environmentStorage;
        private readonly IBotService botService;

        public TakeCommand(
            IEnvironmentStorage environmentStorage,
            IBotService botService)
        {
            this.environmentStorage = environmentStorage;
            this.botService = botService;
        }

        public bool CanExecute(Message message)
        {
            return message.Type == MessageType.Text && message.Text.StartsWith(@"/take");
        }

        public async Task ExecuteAsync(Message message)
        {
            var messageParts = message.Text.Split(' ');
            if (messageParts.Length != 2)
            {
                await message.ReplyAsync(botService, @"Я вас не понял! После /take нужно написать пробел и название площадки");
                return;
            }

            var result = await environmentStorage.SetTakenAsync(messageParts[1], new User
            {
                ChatId = message.Chat.Id,
                TelegramNickname = message.Chat.Username
            });

            if (result.IsSuccess)
            {
                await message.ReplyAsync(botService, "Вы заняли площадку, наслаждайтесь!");
            }

            if (result.ErrorType == ErrorType.EnvironmentTakenBySelf)
            {
                await message.ReplyAsync(botService, "Вы уже заняли эту площадку");
            }

            if (result.ErrorType == ErrorType.EnvironmentTaken)
            {
                await message.ReplyAsync(botService, $"Площадка уже занята, ей пользуется @{result.Message}");
            }

            if (result.ErrorType == ErrorType.EnvironmentDoesNotExist)
            {
                await message.ReplyAsync(botService, "Такой площадки нет");
            }
        }
    }
}