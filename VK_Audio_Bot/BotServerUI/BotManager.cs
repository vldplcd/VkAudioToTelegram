using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VKAudioDB;

namespace BotServerUI
{
    public delegate void Log(string log);

    class BotManager
    {
        public event Log logevent;
        TelegramBotClient Bot;
        string name;

        public async Task StartBot()
        {
            if (Bot == null)
            {
                var q = new Queries();
                Bot = new TelegramBotClient(q.GetKey()[0]);
                Bot.OnMessage += BotOnMessageReceived;
                Bot.OnMessageEdited += BotOnMessageReceived;
                Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
                Bot.OnInlineResultChosen += BotOnInlineReceived;
                name = (await Bot.GetMeAsync()).Username;
            }

            Bot.StartReceiving();
            logevent?.Invoke($"\nBot connected: {name}");
        }

        public void StopBot()
        {
            Bot.StopReceiving();
            logevent?.Invoke($"\nBot disconnected: {name}");
        }

        Update up = new Update();
        Queries q = new Queries();

        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            logevent?.Invoke($"\nReceived: {message.Text} From: {message.Chat.FirstName}");

            if (message == null || message.Type != MessageType.TextMessage) return;

            if (message.Text.StartsWith("/start"))
            {
                up.InsertUser(message.Chat.Id);
                TelegramActions.Start(message, Bot);
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

        public async Task SendMessage(long chatID, string answer)
        {
            await Bot.SendTextMessageAsync(chatID, answer,
                replyMarkup: new ReplyKeyboardHide());
            logevent?.Invoke($"\nSent: {answer}   To: {(await Bot.GetChatAsync(chatID)).FirstName}");
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
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

        private void BotOnInlineReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            logevent?.Invoke($"\nReceived chosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        public async Task<Dictionary<long, string>> Usernames()
        {
            Dictionary<long, string> res = new Dictionary<long, string>();
            res.Add(0, "All");
            var ids = q.GetChatIds();

            foreach (var item in ids)
            {
                var chat = await Bot.GetChatAsync(item);
                res.Add(item, chat.FirstName + " " + chat.LastName);
            }
            return res;
        }
    }
}
