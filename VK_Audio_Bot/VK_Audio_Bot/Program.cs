﻿using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VKAudioDB;
using VK_Audio_Bot.BotManager;

namespace VK_Audio_Bot.ConsoleApp
{
    class Program
    {
        static TelegramBotClient Bot;

        static void Main(string[] args)
        {
            var q = new Queries();
            Bot = new TelegramBotClient(q.GetKey()[0]);
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
            }
            else if (message.Text.StartsWith("/find "))
            {
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


