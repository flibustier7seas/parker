using System.Threading.Tasks;
using GitLab.Parker.Abstractions;
using GitLab.Parker.Logic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GitLab.Parker.BotCommands
{
    public class AddCommand : IBotCommand
    {
        private readonly IBotService botService;
        private readonly IEnvironmentStorage environmentStorage;

        public AddCommand(
            IBotService botService,
            IEnvironmentStorage environmentStorage)
        {
            this.botService = botService;
            this.environmentStorage = environmentStorage;
        }

        public bool CanExecute(Message message)
        {
            return message.Type == MessageType.Text && message.Text.StartsWith(@"/add");
        }

        public async Task ExecuteAsync(Message message)
        {
            var messageParts = message.Text.Split(' ');
            if (messageParts.Length != 2)
            {
                await message.ReplyAsync(botService, @"Я вас не понял! После /add нужно написать пробел и название площадки");
                return;
            }

            var result = await environmentStorage.AddAsync(messageParts[1]);
            if (result.IsSuccess)
            {
                var allEnvs = await environmentStorage.GetAllEnvironmentNamesAsync();
                await botService.Client.SendTextMessageAsync(message.Chat.Id, "Готово!", replyMarkup: Keyboards.CreateKeyboard(allEnvs));
                return;
            }

            switch (result.ErrorType)
            {
                case ErrorType.EnvironmentExistsAlready:
                    await message.ReplyAsync(botService, "Такая площадка уже есть");
                    return;
                case ErrorType.EnvironmentProhibited:
                    await message.ReplyAsync(botService, "Эту площадку нельзя использовать");
                    return;
                default:
                    await message.ReplyAsync(botService, "Что-то пошло не так...");
                    break;
            }
        }
    }
}