using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKAudioDB
{
    public class Update
    {
        public void InsertUser(long ChatID)
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            { 
                vc.InsertUser(new User { chatID = ChatID });
            }
        }
        public void InsertTrack()
        {

        }
        public void UpdateUser(long chatID, List<int> tracks)
        {
            using (vkAudio_Context vc = new vkAudio_Context())
            {
                vc.UpdateUser(chatID, tracks);
            }
        }
    }
}
