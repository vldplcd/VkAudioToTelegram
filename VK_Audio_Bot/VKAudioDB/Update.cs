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
                vc.InsertUser(new User { chatID = ChatID, tracks = new List<int>()});
            }
        }

        public void InsertTrack(int id, int lyrics_id, string file_id, bool isUploaded, string title, string artist)
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                vc.InsertTrack(new Track
                {
                    dbID = id,
                    lyrics_id = lyrics_id,
                    title = title,
                    artist = artist,
                    file_id = file_id,
                    isUploaded = isUploaded
                });
            }
        }

        public void UpdateUser(long chatID, int track)
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                var tr = vc.Users.Find(x => x.chatID == chatID).Single().tracks;
                if (!tr.Any(t => t == track))
                    tr.Add(track);
                vc.UpdateUser(chatID, tr);
            }
        }
    }
}