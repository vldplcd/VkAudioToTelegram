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
        Dictionary<long, int> current_index_tr = new Dictionary<long, int>();
        Dictionary<long, List<AudioInfo>> playlists = new Dictionary<long, List<AudioInfo>>();
        Dictionary<long, int> current_index_pl = new Dictionary<long, int>();

        private void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            Task.Run(async () =>
            {
                var message = messageEventArgs.Message;
                var chID = message.Chat.Id;
                logevent?.Invoke($"\nReceived: {message.Text} From: {message.Chat.FirstName}");

                if (message == null || message.Type != MessageType.TextMessage) return;

                if (message.Text.StartsWith("/start"))
                {
                    up.InsertUser(message.Chat.Id);
                    TelegramActions.Start(message, Bot);
                }
                else if (message.Text.StartsWith("/find "))
                {
                    
                    if(!current_index_tr.ContainsKey(message.Chat.Id))
                        current_index_tr.Add(chID, 0);
                    current_index_tr[chID] = 0;
                    if (!tracks.ContainsKey(chID))
                        tracks.Add(chID, new List<AudioInfo>());
                    tracks[chID] = await ig.GetMusic(message.Text.Substring(6));

                    bool isNextExists;
                    var tracks_sublist = NextSublist(chID, tracks, current_index_tr, out isNextExists);
                    TelegramActions.Find(chID, Bot, tracks_sublist, isNextExists, false);
                }
                else if (message.Text.StartsWith("/playlist"))
                {
                    if (!current_index_pl.ContainsKey(message.Chat.Id))
                        current_index_pl.Add(chID, 0);
                    current_index_pl[chID] = 0;
                    var TracksJson = q.GetUsersTracks(chID);
                    List<AudioInfo> playlist = new List<AudioInfo>();
                    foreach (var track in TracksJson)
                    {
                        playlist.Add(JsonConvert.DeserializeObject<AudioInfo>(track));
                    }
                    if (!playlists.ContainsKey(chID))
                        playlists.Add(chID, new List<AudioInfo>());
                    playlists[chID] = playlist;

                    bool isNextExists;
                    var playlist_sublist = NextSublist(chID, playlists, current_index_pl, out isNextExists);
                    TelegramActions.ShowPlaylist(chID, Bot, playlist_sublist, isNextExists, false);
                }
                else
                {
                    var usage = @"Usage:
/find  - find track with its name
/playlist - see your playlist";

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
                int trID;
                var chID = callbackQueryEventArgs.CallbackQuery.From.Id;
                if (int.TryParse(callbackQueryEventArgs.CallbackQuery.Data, out trID))
                {
                    try
                    {
                        await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                        $"Wait a little, pls");
                        TelegramActions.SendTrack(chID, trID, tracks[chID], Bot, false);
                    }
                    catch (Exception ex)
                    {
                        await Bot.SendTextMessageAsync(callbackQueryEventArgs.CallbackQuery.Id,
                        $"Failed to show track. Maybe you should try /find again");
                        logevent?.Invoke($"\n{ex.Message}");
                    }
                    
                }
                else if (callbackQueryEventArgs.CallbackQuery.Data.StartsWith("s") &&
                        int.TryParse(callbackQueryEventArgs.CallbackQuery.Data.Substring(1), out trID))
                {
                    try
                    {
                        var track = tracks[chID].Find(x => x.Id == trID);
                        up.InsertTrack(track.Id, track.Lyrics_id, track.FileId, track.isUploaded, track.Title, track.Artist, track.Owner_id);
                        up.UpdateUser(chID, trID);
                        await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                            $"Saved {track.Title} to playlist");
                    }
                    catch (Exception ex)
                    {
                        await Bot.SendTextMessageAsync(callbackQueryEventArgs.CallbackQuery.Id,
                        $"Failed. You cannot save any track from previous requests, sry(9((\nTry find it again");
                        logevent?.Invoke($"\n{ex.Message}");
                    }
                    
                }
                else if (callbackQueryEventArgs.CallbackQuery.Data.StartsWith("p") &&
                        int.TryParse(callbackQueryEventArgs.CallbackQuery.Data.Substring(1), out trID))
                {
                    TelegramActions.SendTrack(chID, trID, playlists[chID], Bot, true);
                }
                else if (callbackQueryEventArgs.CallbackQuery.Data == "n")
                {
                    bool isNextExists;
                    var tracks_sublist = NextSublist(chID, tracks, current_index_tr, out isNextExists);
                    TelegramActions.Find(chID, Bot, tracks_sublist, isNextExists, true);
                }
                else if (callbackQueryEventArgs.CallbackQuery.Data == "np")
                {
                    bool isNextExists;
                    var tracks_sublist = NextSublist(chID, playlists, current_index_pl, out isNextExists);
                    TelegramActions.ShowPlaylist(chID, Bot, tracks_sublist, isNextExists, true);
                }
                else if (callbackQueryEventArgs.CallbackQuery.Data == "p")
                {
                    bool isPreviousExists;
                    var tracks_sublist = PreviousSublist(chID, tracks, current_index_tr, out isPreviousExists);
                    TelegramActions.Find(chID, Bot, tracks_sublist, true, isPreviousExists);
                }
                else if (callbackQueryEventArgs.CallbackQuery.Data == "pp")
                {
                    bool isPreviousExists;
                    var tracks_sublist = PreviousSublist(chID, playlists, current_index_pl, out isPreviousExists);
                    TelegramActions.ShowPlaylist(chID, Bot, tracks_sublist, true, isPreviousExists);
                }
            });

        }

        private List<AudioInfo> NextSublist(long chID, Dictionary<long, List<AudioInfo>> tracklist, Dictionary<long, int> current_index, out bool isNextExists)
        {
            isNextExists = tracklist[chID].Count - current_index[chID] >= 10;
            var tracks_sublist = tracklist[chID].GetRange(current_index[chID],
                !isNextExists ? tracklist[chID].Count - current_index[chID] : 10);
            if (isNextExists)
                current_index[chID] += 10;
            return tracks_sublist;
        }

        private List<AudioInfo> PreviousSublist(long chID, Dictionary<long, List<AudioInfo>> tracklist, Dictionary<long, int> current_index, out bool isPreviousExists)
        {
            isPreviousExists = (current_index[chID] - 10) > 0;
            var tracks_sublist = tracklist[chID].GetRange(isPreviousExists? current_index[chID] - 10 : 0, 10);
            if (isPreviousExists)
                current_index_tr[chID] -= 10;
            return tracks_sublist;
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
