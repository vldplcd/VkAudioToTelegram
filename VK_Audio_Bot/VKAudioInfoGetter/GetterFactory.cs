using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
