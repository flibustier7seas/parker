using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace GitLab.Parker.Abstractions
{
    public interface IBotUpdateHandler
    {
        Task HandleAsync(Update update);
    }
}
