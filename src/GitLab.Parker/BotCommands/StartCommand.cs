using System.Threading.Tasks;
using GitLab.Parker.Abstractions;
using GitLab.Parker.Logic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GitLab.Parker.BotCommands
{
    public class StartCommand : IBotCommand
    {
        private readonly IEnvironmentStorage environmentStorage;
        private readonly IBotService botService;

        public StartCommand(
            IEnvironmentStorage environmentStorage,
            IBotService botService)
        {
            this.environmentStorage = environmentStorage;
            this.botService = botService;
        }

        public bool CanExecute(Message message)
        {
            return message.Type == MessageType.Text && message.Text.StartsWith(@"/start");
        }

        public async Task ExecuteAsync(Message message)
        {
            var allEnvs = await environmentStorage.GetAllEnvironmentNamesAsync();
            var keyboard = Keyboards.CreateKeyboard(allEnvs);

            await botService.Client.SendTextMessageAsync(
                message.Chat.Id,
                "Привет!\n" +
                "Чтобы забронировать площадку, используй /take названиеПлощадки\n" +
                "Чтобы освободить площадку напиши /free названиеПлощадки или /free all чтобы освободить все\n" +
                "Просмотреть свободные площадки - /list\n" +
                "Чтобы добавить новую площадку, напиши /add названиеПлощадки\n",
                replyMarkup: keyboard);
        }
    }
}
