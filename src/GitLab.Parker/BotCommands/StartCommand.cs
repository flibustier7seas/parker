using System.Threading.Tasks;
using GitLab.Parker.Abstractions;
using GitLab.Parker.Logic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GitLab.Parker.BotCommands
{
    public class StartCommand : IBotCommand
    {
        private readonly IBotService botService;

        public StartCommand(IBotService botService)
        {
            this.botService = botService;
        }

        public bool CanExecute(Message message)
        {
            return message.Type == MessageType.Text && message.Text.StartsWith(@"/start");
        }

        public async Task ExecuteAsync(Message message)
        {
            await message.ReplyAsync(
                botService,
                "Привет! Чтобы забронировать площадку, используй /take названиеПлощадки");

            await botService.Client.SendTextMessageAsync(message.Chat.Id, "My keyboard", replyMarkup: Keyboards.KeyboardMarkup);
        }
    }
}
