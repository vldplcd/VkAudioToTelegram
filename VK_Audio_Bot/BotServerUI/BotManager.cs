using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VKAudioDB;
using VKAudioInfoGetter;
using VKAudioInfoGetter.Model;
using Newtonsoft.Json;

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

        UpdateDB up = new UpdateDB();
        Queries q = new Queries();
        InfoGetter ig = new InfoGetter();
        Dictionary<long, List<AudioInfo>> tracks = new Dictionary<long, List<AudioInfo>>();
        Dictionary<long, List<AudioInfo>> playlists = new Dictionary<long, List<AudioInfo>>();

        private void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            Task.Run(async () =>
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
                    var chID = message.Chat.Id;
                    if (!tracks.ContainsKey(chID))
                        tracks.Add(chID, new List<AudioInfo>());
                    tracks[chID] = await ig.GetMusic(message.Text.Substring(6));
                    TelegramActions.Find(message, Bot, tracks[chID].GetRange(0,
                        tracks[chID].Count < 10 ? tracks[chID].Count : 10));
                }
                else if (message.Text.StartsWith("/playlist"))
                {
                    var chID = message.Chat.Id;
                    var TracksJson = q.GetUsersTracks(chID);
                    List<AudioInfo> playlist = new List<AudioInfo>();
                    foreach(var track in TracksJson)
                    {
                        playlist.Add(JsonConvert.DeserializeObject<AudioInfo>(track));
                    }
                    if (!playlists.ContainsKey(chID))
                        playlists.Add(chID, new List<AudioInfo>());
                    playlists[chID] = playlist;
                    TelegramActions.ShowPlaylist(message, Bot, playlist);
                }
                else
                {
                    var usage = @"Usage:
/find  - find track with its name";

                    await Bot.SendTextMessageAsync(message.Chat.Id, usage,
                        replyMarkup: new ReplyKeyboardHide());
                }
            });
            
        }

        public async Task SendMessage(long chatID, string answer)
        {
            await Bot.SendTextMessageAsync(chatID, answer,
                replyMarkup: new ReplyKeyboardHide());
            logevent?.Invoke($"\nSent: {answer}   To: {(await Bot.GetChatAsync(chatID)).FirstName}");
        }

        private void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            Task.Run(async () => 
            {
                await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                $"Wait a little, pls");
                int trID;
                var chID = callbackQueryEventArgs.CallbackQuery.From.Id;
                if (int.TryParse(callbackQueryEventArgs.CallbackQuery.Data, out trID))
                {
                    TelegramActions.SendTrack(chID, trID, tracks[chID], Bot, false);
                }
                else if(callbackQueryEventArgs.CallbackQuery.Data.StartsWith("s") && 
                        int.TryParse(callbackQueryEventArgs.CallbackQuery.Data.Substring(1), out trID))
                {
                    var track = tracks[chID].Find(x => x.Id == trID);
                    up.InsertTrack(track.Id, track.Title, track.Artist, track.Duration, track.Lyrics_id, track.Url);
                    up.UpdateUser(chID, trID);
                }
                else if (callbackQueryEventArgs.CallbackQuery.Data.StartsWith("p") &&
                        int.TryParse(callbackQueryEventArgs.CallbackQuery.Data.Substring(1), out trID))
                {
                    TelegramActions.SendTrack(chID, trID, playlists[chID], Bot, true);
                }
            });
            
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
