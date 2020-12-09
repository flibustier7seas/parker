using GitLab.Parker.Abstractions;
using GitLab.Parker.Configuration;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace GitLab.Parker.Logic
{
    public class BotService : IBotService
    {
        public BotService(IOptions<Credentials> config)
        {
            Client = new TelegramBotClient(config.Value.BotToken);
        }

        public TelegramBotClient Client { get; }
    }
}
