using System.Collections.Generic;
using System.Threading.Tasks;
using VKAudioInfoGetter.Model;

namespace VKAudioInfoGetter
{
    public delegate string GetApiKey(string akID);

    public interface IInfoGetter
    {
        event GetApiKey getApiKeyEvent;
        Task<List<AudioInfo>> GetMusic(string requestText);//, string token)
    }
}
