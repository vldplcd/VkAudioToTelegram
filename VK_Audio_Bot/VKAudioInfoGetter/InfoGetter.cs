﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VKAudioInfoGetter.Model;

namespace VKAudioInfoGetter
{
    public class InfoGetter : IInfoGetter
    {
        public event GetApiKey getApiKeyEvent;

        public async Task<List<AudioInfo>> GetMusic(string requestText)
        {
            string token = getApiKeyEvent?.Invoke("vk");
            var r = new Repository();
            var request = new AudioRequest(requestText, Convert.ToBoolean(1), Convert.ToBoolean(0), Convert.ToBoolean(0), 2, Convert.ToBoolean(0), 0, 30);

            var result = await r.GetAudioList(request, token);
            return result;
        }

        public async Task<AudioInfo> GetMusicById(int id, int ownerId, string token)
        {
            var r = new Repository();
            string audioIds = ownerId.ToString() + "_" + id.ToString();
            var request = new AudioIdRequest(audioIds);

            var result = await r.GetAudioById(request, token);
            return result;
        }
    }
}
