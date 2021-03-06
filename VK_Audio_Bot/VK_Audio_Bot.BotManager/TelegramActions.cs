﻿using System.Collections.Generic;
using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VKAudioInfoGetter.Model;

namespace VK_Audio_Bot.BotManager
{
    public class TelegramActions
    {
        static public async void Find(long chatID, TelegramBotClient Bot, List<AudioInfo> tracks, bool isNextExists, bool isPreviousExists)
        {
            InlineKeyboardButton[][] buttons = new InlineKeyboardButton[tracks.Count + 2][];
            int i = 0;
            foreach (var item in tracks)
            {
                buttons[i] = new InlineKeyboardButton[]
                {
                        new InlineKeyboardButton(item.Title + " - " + item.Artist)
                        {
                            CallbackData = item.Id.ToString()
                        }
                    };

                i++;
            }
            buttons[i] = isNextExists ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardButton("Next")
                        {
                            CallbackData = "n"
                        }
                    } : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardButton("No more :(")
                        {

                        }
                    };
            buttons[i + 1] = isPreviousExists ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardButton("Previous")
                        {
                            CallbackData = "p"
                        }
                    } : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardButton("No previous :(")
                        {

                        }
                    };


            var keyboard = new InlineKeyboardMarkup(buttons);
            var m = await Bot.SendTextMessageAsync(chatID, "Choose",
                replyMarkup: keyboard);

        }

        static public async void Start(Message message, TelegramBotClient Bot)
        {
            var greeting = $"Hello, {message.Chat.FirstName}!\nThis is VK audio bot. As you will see, it provides you an opportunity to listen to music from vk.com right here."+
                "\n(not from vk actually, as their api was closed)"+
                "\nType /find Track name to find track" +
                "\nType /playlist to see your playlist\nSend voice message to use voice search";
            await Bot.SendTextMessageAsync(message.Chat.Id, greeting,
                replyMarkup: new ReplyKeyboardHide());
        }

        static public async void SendTrack(long chID, int trID, List<AudioInfo> tracks, TelegramBotClient Bot, bool isPlaylist)
        {
            var track = tracks.Find(x => x.Id == trID);
            if (track != null)
            {
                var keyboard = isPlaylist ?
                    new InlineKeyboardMarkup(new[] {
                        new InlineKeyboardButton("Delete from playlist")
                            {
                                CallbackData = $"d{trID}"
                            }
                        }) :
                    new InlineKeyboardMarkup(new[] {
                        new InlineKeyboardButton("Save to playlist")
                            {
                                CallbackData = $"s{trID}"
                            }
                        });
                if (track.isUploaded)
                {
                    await Bot.SendAudioAsync(chID, track.FileId, track.Duration,
                            track.Title, track.Artist, replyMarkup: keyboard);
                }
                else
                {
                    //GetTrack(track, Bot, chID, keyboard);
                    try
                    {
                        var audio_m = await Bot.SendAudioAsync(chID, track.Url, track.Duration, track.Artist, track.Title, replyMarkup: keyboard);
                        track.isUploaded = true;
                        track.FileId = audio_m.Audio.FileId;
                    }
                    catch { await Bot.SendTextMessageAsync(chID, "I can't download this track, sorry"); }
                }

            }
            else
            {
                await Bot.SendTextMessageAsync(chID, "Try to use /find again and then choose necessary track");
            }
        }

        static private async void GetTrack(AudioInfo track, TelegramBotClient Bot, long chID, InlineKeyboardMarkup keyboard)
        {
            WebRequest request = WebRequest.Create(track.Url);
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    try
                    {
                        var audio_m = await Bot.SendAudioAsync(chID, new FileToSend($"{track.Title} - {track.Artist}", responseStream), track.Duration,
                    track.Title, track.Artist, replyMarkup: keyboard);
                        track.isUploaded = true;
                        track.FileId = audio_m.Audio.FileId;
                    }
                    catch
                    {

                    }
                }
            }
        }

        static public async void ShowPlaylist(long chID, TelegramBotClient Bot, List<AudioInfo> tracks, bool isNextExists, bool isPreviousExists)
        {
            InlineKeyboardButton[][] buttons = new InlineKeyboardButton[tracks.Count + 2][];
            int i = 0;

            foreach (var item in tracks)
            {
                buttons[i] = new InlineKeyboardButton[]
                {
                        new InlineKeyboardButton(item.Title + " - " + item.Artist)
                        {
                            CallbackData = "p"+item.Id.ToString()
                        }
                    };

                i++;
            }
            buttons[i] = isNextExists ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardButton("Next")
                        {
                            CallbackData = "np"
                        }
                    } : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardButton("No more :(")
                        {

                        }
                    };
            buttons[i + 1] = isPreviousExists ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardButton("Previous")
                        {
                            CallbackData = "pp"
                        }
                    } : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardButton("No previous :(")
                        {

                        }
                    };
            var keyboard = new InlineKeyboardMarkup(buttons);
            await Bot.SendTextMessageAsync(chID, "Playlist",
                replyMarkup: keyboard);
        }
    }
}
