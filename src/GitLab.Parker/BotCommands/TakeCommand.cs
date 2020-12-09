using System.Threading.Tasks;
using GitLab.Parker.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GitLab.Parker.BotCommands
{
    public class TakeCommand : IBotCommand
    {
        private readonly IBotService botService;

        public TakeCommand(
            IBotService botService)
        {
            this.botService = botService;
        }

        public bool CanExecute(Message message)
        {
            return message.Type == MessageType.Text && message.Text.StartsWith(@"/take");
        }

        public async Task ExecuteAsync(Message message)
        {
        }
    }
}