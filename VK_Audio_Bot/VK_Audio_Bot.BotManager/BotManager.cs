using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VKAudioDB;
using VKAudioInfoGetter;
using VKAudioInfoGetter.Model;
using Newtonsoft.Json;
using VK_Audio_Bot.SpeechRecognition;
using System.Threading;


namespace VK_Audio_Bot.BotManager
{
    public delegate void Log(string log);
    public delegate void BotAction();

    public class BotManager
    {
        public event Log logevent;
        public event BotAction userAddedEvent;
        TelegramBotClient Bot;

        public async Task StartBot()
        {
            if (Bot == null)
            {
                var thread = new Thread(() =>
                {
                    AuthorizationForm vkAuth = new AuthorizationForm();
                    vkAuth.ShowDialog();
                    updateDB.UpdateAk("vk", vkAuth.result);
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                Bot = new TelegramBotClient(dbQueries.GetKey("tg")[0]);
                Bot.OnMessage += BotOnMessageReceived;
                Bot.OnMessageEdited += BotOnMessageReceived;
                Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
                Bot.OnInlineResultChosen += BotOnInlineReceived;
                var thread_sinf = new Thread(async () =>
                {
                    playlists = await GetSavedInfo("pl");
                    tracks = await GetSavedInfo("tr");
                });
                thread_sinf.Start();
                infoGetter.getApiKeyEvent += (akID) => dbQueries.GetKey(akID)[0];
            }
            Bot.StartReceiving();
            logevent?.Invoke($"\nBot connected");            
        }

        private async Task<Dictionary<long, List<AudioInfo>>> GetSavedInfo(string sID)
        {
            var result = new Dictionary<long, List<AudioInfo>>();
            try
            {
                var tracklist = await dbQueries.GetSavedInfo(sID);
                if (tracklist.Count != 0)
                    foreach (var key in tracklist.Keys)
                    {
                        result.Add(key, new List<AudioInfo>());
                        var type = tracklist[key].GetType();
                        var t = Convert.ChangeType(tracklist[key], type);
                        var mehmeh = t as Dictionary<string, object>;
                        var desDictList = mehmeh["_v"] as List<object>;
                        
                        foreach(var innerDict in desDictList)
                        {
                            var objDict = innerDict as Dictionary<string, object>;
                                result[key].Add(new AudioInfo
                                {
                                    Id = (int)objDict["_id"],
                                    Artist = (string)objDict["Artist"],
                                    Title = (string)objDict["Title"],
                                    Duration = (int)objDict["Duration"],
                                    Url = (string)objDict["Url"],
                                    isUploaded = (bool)objDict["isUploaded"],
                                    FileId = (string)objDict["FileId"]

                                });
                        }
                    }
                        
                logevent($"\nPrevious session {sID} loaded");
            }
            catch(Exception ex)
            {
                logevent($"\nFailed to load previous session: {ex.Message}");
            }
            return result;
        }

        public void StopBot()
        {
            Bot.StopReceiving();
            logevent?.Invoke($"\nBot disconnected");
        }

        UpdateDB updateDB = new UpdateDB();
        Queries dbQueries = new Queries();

        IInfoGetter infoGetter;
        public void SetGetter(bool isVk)
        {
            if (isVk)
                infoGetter = GetterFactory.DefaultVk();
            else
                infoGetter = GetterFactory.DefaultMp3CC();
        }

        Dictionary<long, List<AudioInfo>> tracks = new Dictionary<long, List<AudioInfo>>();
        Dictionary<long, int> currentIndexTr = new Dictionary<long, int>();
        Dictionary<long, List<AudioInfo>> playlists = new Dictionary<long, List<AudioInfo>>();
        Dictionary<long, int> currentIndexPl = new Dictionary<long, int>();

        private async Task FindProcess(long chID, string request)
        {
            if (!currentIndexTr.ContainsKey(chID))
                currentIndexTr.Add(chID, 0);
            currentIndexTr[chID] = 0;
            if (!tracks.ContainsKey(chID))
                tracks.Add(chID, new List<AudioInfo>());
            tracks[chID] = await infoGetter.GetMusic(request);

            bool isNextExists;
            var tracks_sublist = NextSublist(chID, tracks, currentIndexTr, out isNextExists);
            TelegramActions.Find(chID, Bot, tracks_sublist, isNextExists, false);
        }
                
        private void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            Task.Run(async () =>
            {
                var message = messageEventArgs.Message;
                var chID = message.Chat.Id;

                if (message.Voice != null)
                {
                    logevent?.Invoke($"\nReceived: Voice Message From: {message.Chat.FirstName}");

                    SpeechRepository sr = new SpeechRepository();
                    string result;
                    try
                    {
                         result = await sr.Result((await Bot.GetFileAsync(message.Voice.FileId)).FileStream, dbQueries.GetKey("ya")[0]);
                    }
                    catch (Exception ex) { await Bot.SendTextMessageAsync(message.Chat.Id, $"{ex.Message}"); return; }

                    await Bot.SendTextMessageAsync(message.Chat.Id, $"I've heard {result}");
                    await FindProcess(chID, result);
                }
                
                if (message == null || message.Type != MessageType.TextMessage) return;
                logevent?.Invoke($"\nReceived: {message.Text} From: {message.Chat.FirstName}");

                if (message.Text.StartsWith("/start"))
                {
                    updateDB.InsertUser(message.Chat.Id);
                    userAddedEvent?.Invoke();
                    TelegramActions.Start(message, Bot);
                }
                else if (message.Text.StartsWith("/find "))
                {
                    await FindProcess(chID, message.Text.Substring(6));
                }
                else if (message.Text.StartsWith("/playlist"))
                {
                    if (!currentIndexPl.ContainsKey(message.Chat.Id))
                        currentIndexPl.Add(chID, 0);
                    currentIndexPl[chID] = 0;
                    var TracksJson = dbQueries.GetUsersTracks(chID);
                    List<AudioInfo> playlist = new List<AudioInfo>();
                    foreach (var track in TracksJson)
                    {
                        playlist.Add(JsonConvert.DeserializeObject<AudioInfo>(track));
                    }
                    if (!playlists.ContainsKey(chID))
                        playlists.Add(chID, new List<AudioInfo>());
                    playlists[chID] = playlist;

                    bool isNextExists;
                    var playlist_sublist = NextSublist(chID, playlists, currentIndexPl, out isNextExists);
                    TelegramActions.ShowPlaylist(chID, Bot, playlist_sublist, isNextExists, false);
                }
                else
                {
                    var usage = "Usage:\n/find  - find track with its name\n/playlist - see your playlist"+
                    "\nSend a voice message to use voice search";

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
                        updateDB.InsertTrack(track.Id, track.Lyrics_id, track.FileId, track.isUploaded, track.Title, track.Artist, track.Owner_id);
                        updateDB.UpdateUser(chID, trID);
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
                    var tracks_sublist = NextSublist(chID, tracks, currentIndexTr, out isNextExists);
                    TelegramActions.Find(chID, Bot, tracks_sublist, isNextExists, true);
                }
                else if (callbackQueryEventArgs.CallbackQuery.Data == "np")
                {
                    bool isNextExists;
                    var tracks_sublist = NextSublist(chID, playlists, currentIndexPl, out isNextExists);
                    TelegramActions.ShowPlaylist(chID, Bot, tracks_sublist, isNextExists, true);
                }
                else if (callbackQueryEventArgs.CallbackQuery.Data == "p")
                {
                    bool isPreviousExists;
                    var tracks_sublist = PreviousSublist(chID, tracks, currentIndexTr, out isPreviousExists);
                    TelegramActions.Find(chID, Bot, tracks_sublist, true, isPreviousExists);
                }
                else if (callbackQueryEventArgs.CallbackQuery.Data == "pp")
                {
                    bool isPreviousExists;
                    var tracks_sublist = PreviousSublist(chID, playlists, currentIndexPl, out isPreviousExists);
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
            var tracks_sublist = tracklist[chID].GetRange(isPreviousExists ? current_index[chID] - 10 : 0, 10);
            if (isPreviousExists)
                currentIndexTr[chID] -= 10;
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
            var ids = dbQueries.GetChatIds();

            foreach (var item in ids)
            {
                var chat = await Bot.GetChatAsync(item);
                res.Add(item, chat.FirstName + " " + chat.LastName);
            }
            return res;
        }

        public void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var tracklist = new Dictionary<string, object>();

            foreach (var key in tracks.Keys)
            {
                tracklist.Add(key.ToString(), tracks[key]);
            }
                
            updateDB.UpdateSInfo("tr", tracklist);
            tracklist = new Dictionary<string, object>();
            foreach (var key in playlists.Keys)
            {
                tracklist.Add(key.ToString(), tracks[key]);
            }
            updateDB.UpdateSInfo("pl", tracklist);

        }

        public void OnClosing()
        {
            var tracklist = new Dictionary<string, object>();

            foreach (var key in tracks.Keys)
            {
                tracklist.Add(key.ToString(), tracks[key]);
            }

            updateDB.UpdateSInfo("tr", tracklist);
            tracklist = new Dictionary<string, object>();
            foreach (var key in playlists.Keys)
            {
                tracklist.Add(key.ToString(), tracks[key]);
            }
            updateDB.UpdateSInfo("pl", tracklist);

        }
    }
}
