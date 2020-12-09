using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace GitLab.Parker.Logic
{
    internal static class Keyboards
    {
        public static ReplyKeyboardMarkup CreateKeyboard(IEnumerable<string> envs)
        {
            var buttons = envs.Select(x =>
                new KeyboardButton[]
                {
                    $"/take {x}",
                    $"/free {x}",
                });

            var freeAll = new[]
            {
                new KeyboardButton[]
                {
                    "/free all"
                }
            };

            var list = new[]
            {
                new KeyboardButton[]
                {
                    "/list"
                }
            };

            return new ReplyKeyboardMarkup
            {
                ResizeKeyboard = true,
                Keyboard = list.Concat(buttons).Concat(freeAll).ToArray()
            };
        }
    }
}