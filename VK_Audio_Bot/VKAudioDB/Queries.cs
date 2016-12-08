using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VKAudioDB
{
    public class Queries
    {
        public List<string> GetKey()
        {
            List<string> ks = new List<string>();

            using (vkAudio_Context vc = new vkAudio_Context())
            {
                var a = vc.aks.Find(x => !string.IsNullOrEmpty(x.value)).ToList();
                foreach (var item in a)
                    ks.Add(item.value);
            }
            return ks;
        }

        public List<long> GetChatIds()
        {
            List<long> res = new List<long>();

            using (vkAudio_Context vc = new vkAudio_Context())
            {
                vc.Users.Find(u => u.chatID > 0).ToList().ForEach(u => res.Add(u.chatID));
            }
            return res;
        }

        public List<string> GetUsersTracks(long chID)
        {
            List<int> ids = new List<int>();
            List<string> res = new List<string>();

            using (vkAudio_Context vc = new vkAudio_Context())
            {
                ids = vc.Users.Find(u => u.chatID == chID).Single().tracks;
                foreach (int id in ids)
                {
                    res.Add(JsonConvert.SerializeObject(vc.Tracks.Find(t => t.dbID == id).Single()));
                }
            }
            return res;
        }
    }
}
