using Telegram.Bot;

namespace GitLab.Parker.Abstractions
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
    }
}