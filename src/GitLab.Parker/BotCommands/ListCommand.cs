using System.Linq;
using System.Threading.Tasks;
using GitLab.Parker.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GitLab.Parker.BotCommands
{
    public class ListCommand : IBotCommand
    {
        private readonly IBotService botService;
        private readonly IEnvironmentStorage environmentStorage;

        public ListCommand(
            IBotService botService,
            IEnvironmentStorage environmentStorage)
        {
            this.botService = botService;
            this.environmentStorage = environmentStorage;
        }

        public bool CanExecute(Message message)
        {
            return message.Type == MessageType.Text && message.Text.StartsWith(@"/list");
        }

        public async Task ExecuteAsync(Message message)
        {
            var envNames = await environmentStorage.GetFreeEnvironmentNamesAsync();

            if (!envNames.Any())
            {
                await message.ReplyAsync(botService, "Нет свободных площадок :(");
                return;
            }

            await message.ReplyAsync(botService, $"Свободные площадки: {string.Join(", ", envNames)}");
        }
    }
}