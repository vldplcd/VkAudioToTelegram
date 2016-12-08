using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;
using VK_Audio_Bot;
using VKAudioDB;

namespace telbot
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
            while (Bot.IsReceiving) { GetMusic(); }
            //Bot.StopReceiving();
        }

        static async void GetMusic()
        {
            var r = new Repository();
            var request = new AudioRequest("Thousand Foot Krutch", 1, 0, 0, 2, 1, 0, 10);

            try
            {
                var result = await r.GetAudioList(request);
                foreach (var item in result)
                    Console.WriteLine("\n{0} - {1}, {2}", item.Artist, item.Title, item.Duration);             
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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


