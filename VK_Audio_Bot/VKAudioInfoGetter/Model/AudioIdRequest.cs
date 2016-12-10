using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKAudioInfoGetter.Model
{
    public class AudioIdRequest
    {
        public string Audios { get; set; }

        public AudioIdRequest(string audios)
        {
            Audios = audios;
        }
    }
}
