using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VKAudioInfoGetter.Model;

namespace VKAudioInfoGetter
{
    public class InfoGetter
    {
        public async Task<List<AudioInfo>> GetMusic(string requestText)
        {
            var r = new Repository();
            var request = new AudioRequest(requestText, Convert.ToBoolean(1), Convert.ToBoolean(0), Convert.ToBoolean(0), 2, Convert.ToBoolean(0), 0, 30);

            var result = await r.GetAudioList(request);
            return result;
        }

        public async Task<AudioInfo> GetMusicById(int id, int ownerId)
        {
            var r = new Repository();
            string audioIds = ownerId.ToString() + "_" + id.ToString();
            var request = new AudioIdRequest(audioIds);

            var result = await r.GetAudioById(request);
            return result;
        }
    }
}
