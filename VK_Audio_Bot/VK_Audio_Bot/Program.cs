using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;
using VK_Audio_Bot;

namespace telbot
{
    class Program
    {
        static TelegramBotClient Bot = new TelegramBotClient("301705994:AAE3BOPfYKRSrLBdLAC8WGk0LrOAnPIfezc");
        static void Main(string[] args)
        {

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnInlineResultChosen += BotOnInlineReceived;

            var me = Bot.GetMeAsync().Result;

            Console.Title = me.Username;

            Bot.StartReceiving();
            while (Bot.IsReceiving) { }
            //Bot.StopReceiving();
        }
        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            Console.WriteLine($"Received: {message.Text} From: {message.Chat.FirstName}");

            if (message == null || message.Type != MessageType.TextMessage) return;
            if (message.Text.StartsWith("/start"))
            {
                TelegramActions.Start(message, Bot);
            }
            else if (message.Text.StartsWith("/t "))
            {
                TelegramActions.Text(message, Bot);
            }
            else if (message.Text.StartsWith("/find "))
            {
                TelegramActions.Find(message, Bot);
            }
            else
            {
                var usage = @"Usage:
/find  - find track with its name
";

                await Bot.SendTextMessageAsync(message.Chat.Id, usage,
                    replyMarkup: new ReplyKeyboardHide());
            }
        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                $"Wait a little, pls");
            if (callbackQueryEventArgs.CallbackQuery.Data != "Save")
            {
                    await Bot.SendAudioAsync(callbackQueryEventArgs.CallbackQuery.From.Id, "https://www.youtube.com/audiolibrary_download?vid=ffc2e9ed58bd5f29", 230, "meh", "meh");
            }
            else
            {

            }
        }
        private static void BotOnInlineReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received choosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }
    }


}
