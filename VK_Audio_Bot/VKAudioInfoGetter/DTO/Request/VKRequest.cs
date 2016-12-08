namespace VKAudioInfoGetter.DTO.Request
{
    class VKRequest
    {
        public string Q { get; set; }

        public short Auto_complete { get; set; }

        public short Lyrics { get; set; }

        public short Performer_only { get; set; }

        public short Sort { get; set; }

        public short Search_own { get; set; }

        public int Offset { get; set; }

        public int Count { get; set; }
    }
}
