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

namespace telbot
{
    class Program
    {
        static TelegramBotClient Bot = new TelegramBotClient("Enter API here");
        static void Main(string[] args)
        {

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnInlineResultChosen += BotOnInlineReceived;

            var me = Bot.GetMeAsync().Result;

            Console.Title = me.Username;

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }
        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            Console.WriteLine($"Received: {message.Text} From: {message.Chat.FirstName}");
            if (message == null || message.Type != MessageType.TextMessage) return;
            if (message.Text.StartsWith("/start "))
            {
                var greeting = $"Hello, {message.Chat.FirstName}!\nThis is VK audio bot. As you will see, it provides you an opportunity to listen to music from vk.com right here.\nType /find Track_name to find track";
                await Bot.SendTextMessageAsync(message.Chat.Id, greeting,
                    replyMarkup: new ReplyKeyboardHide());
            }
            else if (message.Text.StartsWith("/find ")) // send list of first 4 tracks
            {
                await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

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

                        new InlineKeyboardButton(message.Text.Substring(6) + " - 2")
                    },
                    new[]
                    {
                        new InlineKeyboardButton(message.Text.Substring(6) + " - 3"),
                    },
                    new[]
                    {

                        new InlineKeyboardButton(message.Text.Substring(6) + " - 4"),
                    }
                });

                //await Task.Delay(500); // simulate longer running task

                await Bot.SendTextMessageAsync(message.Chat.Id, "Choose",
                    replyMarkup: keyboard);
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
                $"Received {callbackQueryEventArgs.CallbackQuery.Data}");
            if (callbackQueryEventArgs.CallbackQuery.Data != "Save")
            {
                using (WebClient wb = new WebClient())
                {
                    wb.DownloadFile(new Uri("http://cs1-23v4.vk-cdn.net/p24/34a538c4da4f8e.mp3"), "msk1.mp3");
                }
                using (StreamReader str = new StreamReader("msk1.mp3"))
                    await Bot.SendAudioAsync(callbackQueryEventArgs.CallbackQuery.From.Id, new FileToSend("meh", str.BaseStream), 230, "meh", "meh");
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
