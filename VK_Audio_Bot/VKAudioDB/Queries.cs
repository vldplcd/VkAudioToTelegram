using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                {
                    ks.Add(item.value);
                }
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

    }
}
