namespace VKAudioInfoGetter
{
    public class GetterFactory
    {
        public static InfoGetter DefaultVk()
        {
            return new InfoGetter();
        }
        public static Mp3CCInfoGetter DefaultMp3CC()
        {
            return new Mp3CCInfoGetter();
        }
    }
}
