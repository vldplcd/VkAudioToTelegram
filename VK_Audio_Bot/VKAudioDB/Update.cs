using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKAudioDB
{
    public class UpdateDB
    {
        public void InsertUser(long ChatID)
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                vc.InsertUser(new User { chatID = ChatID });
            }
        }

        public void InsertTrack(int id, string title, string artist, int duration, int lyrics_id, string url)
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                vc.InsertTrack(new Track
                {
                    dbID = id,
                    title = title,
                    artist = artist,
                    duration = duration,
                    lyrics_id = lyrics_id,
                    url = url
                });
            }
        }

        public void UpdateUser(long chatID, int track)
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                var tr = vc.Users.Find(x => x.chatID == chatID).Single().tracks;
                if (tr == null)
                    tr = new List<int>();
                if (!tr.Any(t => t == track))
                    tr.Add(track);
                vc.UpdateUser(chatID, tr);
            }
        }
    }
}
