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
                
                var a = new List<ak>();//vc.aks.Find(x => !string.IsNullOrEmpty(x.value)).ToList();
                
                ///Test value to make development without DB available
                var akk = new ak();
                akk._id = MongoDB.Bson.ObjectId.Parse("1a25632897e12f45a213c4de");
                akk.value = "301705994:AAE3BOPfYKRSrLBdLAC8WGk0LrOAnPIfezc";
                a.Add(akk);
                ///

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
    }
}
