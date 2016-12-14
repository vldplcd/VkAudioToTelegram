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
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                List<string> ks = new List<string>();
                var a = vc.aks.Find(x => !string.IsNullOrEmpty(x.value)).ToList();
                foreach (var item in a)
                    ks.Add(item.value);
                return ks;
            }            
        }

        public List<long> GetChatIds()
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                List<long> res = new List<long>();
                vc.Users.Find(u => u.chatID > 0).ToList().ForEach(u => res.Add(u.chatID));
                return res;
            }            
        }

        public List<string> GetUsersTracks(long chID)
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                List<int> ids = new List<int>();
                List<string> res = new List<string>();
                ids = vc.Users.Find(u => u.chatID == chID).Single().tracks;
                foreach (int id in ids)
                {
                    res.Add(JsonConvert.SerializeObject(vc.Tracks.Find(t => t.dbID == id).Single()));
                }
                return res;
            }
        }

        public async Task<Dictionary<long, object>> GetSavedInfo(string sID)
        {
            
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                Dictionary<long, object> res = new Dictionary<long, object>();
                Dictionary<string, object> info = (await vc.SavedInfo.FindAsync(s => s.sID == sID)).Single().Info;
                object v;
                if (info.TryGetValue("Info", out v))
                {
                    var value = v as Dictionary<string, object>;
                    foreach (var key in value.Keys)
                        res.Add(long.Parse(key), value[key]);
                }
                    
                return res;
            }

        }

        public List<string> GetUsersTracks(long chID)
        {
            List<int> ids = new List<int>();
            List<string> res = new List<string>();

            using (vkAudio_Context vc = new vkAudio_Context())
            {
                ids = vc.Users.Find(u => u.chatID == chID).Single().tracks;
                foreach(int id in ids)
                {
                    res.Add(JsonConvert.SerializeObject(vc.Tracks.Find(t => t.dbID == id).Single()));
                }
            }
            return res;
        }
    }
}
