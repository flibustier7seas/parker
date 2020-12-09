using Telegram.Bot.Types.ReplyMarkups;

namespace GitLab.Parker.Logic
{
    public static class Keyboards
    {
        public static readonly ReplyKeyboardMarkup KeyboardMarkup = new ReplyKeyboardMarkup
        {
            Keyboard = new[]
            {
                new KeyboardButton[]
                {
                    "/list"
                }
            }
        };
    }
}