using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace GitLab.Parker.BotCommands
{
    public interface IBotCommand
    {
        bool CanExecute(Message message);
        Task ExecuteAsync(Message message);
    }
}
