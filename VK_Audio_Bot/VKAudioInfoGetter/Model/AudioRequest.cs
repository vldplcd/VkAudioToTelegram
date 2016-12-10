namespace VKAudioInfoGetter.Model
{
    public class AudioRequest
    {
        public string Q { get; set; }

        public bool Auto_complete { get; set; }

        public bool Lyrics { get; set; }

        public bool Performer_only { get; set; }

        public ushort Sort { get; set; }

        public bool Search_own { get; set; }

        public int Offset { get; set; }

        public int Count { get; set; }

        public AudioRequest(string q, bool auto_complete, bool lyrics, bool performer_only,
            ushort sort, bool search_own, int offset, int count)
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
