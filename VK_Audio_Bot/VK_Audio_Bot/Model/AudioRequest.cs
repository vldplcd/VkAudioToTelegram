using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK_Audio_Bot
{
    class AudioRequest
    {
        public string Q { get; set; }

        public int Auto_complete { get; set; }

        public int Lyrics { get; set; }

        public int Performer_only { get; set; }

        public int Sort { get; set; }

        public int Search_own { get; set; }

        public int Offset { get; set; }

        public int Count { get; set; }

        public AudioRequest(string q, int auto_complete, int lyrics, int performer_only,
            int sort, int search_own, int offset, int count)
        {
            Q = q;
            Auto_complete = auto_complete;
            Lyrics = lyrics;
            Performer_only = performer_only;
            Sort = sort;
            Search_own = search_own;
            Offset = offset;
            Count = count;
        }
    }
}
