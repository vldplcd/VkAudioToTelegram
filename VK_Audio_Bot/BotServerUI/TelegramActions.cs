using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VKAudioInfoGetter.Model;

namespace BotServerUI
{
    public class TelegramActions
    {
        static public async void Find(Message message, TelegramBotClient Bot, List<AudioInfo> tracks)
        {
            InlineKeyboardButton[][] buttons = new InlineKeyboardButton[tracks.Count + 1][];
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
            buttons[i] = new InlineKeyboardButton[]
                    {
                        new InlineKeyboardButton("Next")
                        {
                            CallbackData = "Next"
                        }
                    };
            var keyboard = new InlineKeyboardMarkup(buttons);
            await Bot.SendTextMessageAsync(message.Chat.Id, "Choose",
                replyMarkup: keyboard);
        }

        static public async void Start(Message message, TelegramBotClient Bot)
        {
            var greeting = $"Hello, {message.Chat.FirstName}!\nThis is VK audio bot. As you will see, it provides you an opportunity to listen to music from vk.com right here.\nType /find Track_name to find track";
            await Bot.SendTextMessageAsync(message.Chat.Id, greeting,
                replyMarkup: new ReplyKeyboardHide());
        }

        static public async void SendTrack(long chID, int trID, List<AudioInfo> tracks, TelegramBotClient Bot, bool isPlaylist)
        {
            var track = tracks.Find(x => x.Id == trID);
            if (track != null)
            {
                var keyboard = isPlaylist?
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
                WebRequest request = WebRequest.Create(track.Url);

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        await Bot.SendAudioAsync(chID, new FileToSend("track.mp3", responseStream), track.Duration,
                        track.Title, track.Artist, replyMarkup: keyboard);
                    }
                }
            }
            else
            {
                await Bot.SendTextMessageAsync(chID, "Try to use /find again and then choose necessary track");
            }
        }

        static public async void ShowPlaylist(Message message, TelegramBotClient Bot, List<AudioInfo> tracks)
        {
            InlineKeyboardButton[][] buttons = new InlineKeyboardButton[tracks.Count + 1][];
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
            buttons[i] = new InlineKeyboardButton[]
                    {
                        new InlineKeyboardButton("Next")
                        {
                            CallbackData = "Next"
                        }
                    };
            var keyboard = new InlineKeyboardMarkup(buttons);
            await Bot.SendTextMessageAsync(message.Chat.Id, "Playlist",
                replyMarkup: keyboard);
        }
    }
}
