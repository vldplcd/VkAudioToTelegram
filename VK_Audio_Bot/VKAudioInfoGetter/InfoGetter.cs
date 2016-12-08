using System.Collections.Generic;
using System.Threading.Tasks;
using VKAudioInfoGetter.Model;

namespace VKAudioInfoGetter
{
    class InfoGetter
    {
        public async Task<List<AudioInfo>> GetMusic(string requestText)
        {
            var r = new Repository();
            var request = new AudioRequest(requestText, 1, 0, 0, 2, 0, 0, 10); //2, 3, 4, 6 params are boolean (1 or 0)

            var result = await r.GetAudioList(request);
            return result;
        }
    }
}
