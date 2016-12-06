using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace VK_Audio_Bot
{
    public class TelegramActions
    {
        static public async void Find(Message message, TelegramBotClient Bot)
        {

            var keyboard = new InlineKeyboardMarkup(new[]
            {
                    new[]
                    {
                        new InlineKeyboardButton(message.Text.Substring(6) + " - 1"),
                        new InlineKeyboardButton("Save to playlist")
                        {
                            CallbackData = "Save"
                        }
                    },
                    new[]
                    {

                        new InlineKeyboardButton(message.Text.Substring(6) + " - 2"),
                        new InlineKeyboardButton("Save to playlist")
                        {
                            CallbackData = "Save"
                        }
                    },
                    new[]
                    {
                        new InlineKeyboardButton(message.Text.Substring(6) + " - 3"),
                        new InlineKeyboardButton("Save to playlist")
                        {
                            CallbackData = "Save"
                        }
                    },
                    new[]
                    {

                        new InlineKeyboardButton(message.Text.Substring(6) + " - 4"),
                        new InlineKeyboardButton("Save to playlist")
                        {
                            CallbackData = "Save"
                        }
                    },
                    new[]
                    {
                        new InlineKeyboardButton("Next"),
                        new InlineKeyboardButton("Previous")
                        {
                            CallbackData = "Save"
                        }
                    },
                });
            await Bot.SendTextMessageAsync(message.Chat.Id, "Choose",
                replyMarkup: keyboard);
        }

        static public async void Start(Message message, TelegramBotClient Bot)
        {
            var greeting = $"Hello, {message.Chat.FirstName}!\nThis is VK audio bot. As you will see, it provides you an opportunity to listen to music from vk.com right here.\nType /find Track_name to find track";
            await Bot.SendTextMessageAsync(message.Chat.Id, greeting,
                replyMarkup: new ReplyKeyboardHide());
        }

        static public async void Text(Message message, TelegramBotClient Bot)
        {
            var answer = Console.ReadLine();
            await Bot.SendTextMessageAsync(message.Chat.Id, answer,
                replyMarkup: new ReplyKeyboardHide());
        }
    }
}
