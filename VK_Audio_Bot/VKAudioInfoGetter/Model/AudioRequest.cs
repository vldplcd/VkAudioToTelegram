namespace VKAudioInfoGetter.Model
{
    class AudioRequest
    {
        public string Q { get; set; }

        public short Auto_complete { get; set; }

        public short Lyrics { get; set; }

        public short Performer_only { get; set; }

        public short Sort { get; set; }

        public short Search_own { get; set; }

        public int Offset { get; set; }

        public int Count { get; set; }

        public AudioRequest(string q, short auto_complete, short lyrics, short performer_only,
            short sort, short search_own, int offset, int count)
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
