using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK_Audio_Bot.DTO.Request
{
    class VKRequest
    {
        public string Q { get; set; }

        public int Auto_complete { get; set; }

        public int Lyrics { get; set; }

        public int Performer_only { get; set; }

        public int Sort { get; set; }

        public int Search_own { get; set; }

        public int Offset { get; set; }

        public int Count { get; set; }
    }
}
