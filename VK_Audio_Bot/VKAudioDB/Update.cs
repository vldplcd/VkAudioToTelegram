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
        public async void InsertUser(long chatID, List<int> trackIDs)
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                if (vc.Users.Find(u => u.chatID == chatID).Count() == 0)
                    await vc.Users.InsertOneAsync(new User { chatID = chatID, tracks = trackIDs });
                
            }
        }

        public async void InsertTrack(int id, int lyrics_id, string file_id, bool isUploaded, string title, string artist, int owner_id)
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                if (vc.Tracks.Find(t => t.dbID == id).Count() == 0)
                    await vc.Tracks.InsertOneAsync(new Track
                    {
                        dbID = id,
                        lyrics_id = lyrics_id,
                        title = title,
                        artist = artist,
                        file_id = file_id,
                        isUploaded = isUploaded,
                        owner_id = owner_id
                    });
            }
        }

        public async void UpdateUser(long chatID, int track)
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                
                var tr = vc.Users.Find(x => x.chatID == chatID).Single().tracks;
                if (!tr.Any(t => t == track))
                    tr.Add(track);
                var filter = Builders<User>.Filter.Eq((u) => u.chatID, chatID);
                var update = Builders<User>.Update.Set((u) => u.tracks, tr);
                await vc.Users.UpdateManyAsync(filter, update);
            }
        }

        public async void UpdateAk(string akID, string value)
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                var filter = Builders<ak>.Filter.Eq((u) => u.akID, akID);
                var update = Builders<ak>.Update.Set((u) => u.value, value);
                await vc.aks.UpdateManyAsync(filter, update);
            }
        }

        public async void UpdateSInfo(string sID, Dictionary<string, object> info)
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                var filter = Builders<SavedInfo>.Filter.Eq((si) => si.sID, sID);
                var update = Builders<SavedInfo>.Update.Set((si) => si.Info, info);
                await vc.SavedInfo.UpdateOneAsync(filter, update);
            }
        }

        public async void DeleteUserTrack(long chatID, int trID)
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                var tr = vc.Users.Find(x => x.chatID == chatID).Single().tracks;
                if (tr.Any(t => t == trID))
                    tr.Remove(trID);
                var filter = Builders<User>.Filter.Eq((u) => u.chatID, chatID);
                var update = Builders<User>.Update.Set((u) => u.tracks, tr);
                await vc.Users.UpdateOneAsync(filter, update);
            }
        }

    }
}