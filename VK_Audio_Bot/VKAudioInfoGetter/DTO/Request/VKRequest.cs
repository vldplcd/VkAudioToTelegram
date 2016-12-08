namespace VKAudioInfoGetter.DTO.Request
{
    class VKRequest
    {
        public string Q { get; set; }

        public bool Auto_complete { get; set; }

        public bool Lyrics { get; set; }

        public bool Performer_only { get; set; }

        public ushort Sort { get; set; }

        public bool Search_own { get; set; }

        public int Offset { get; set; }

        public int Count { get; set; }
    }
}
