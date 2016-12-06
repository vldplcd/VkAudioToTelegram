using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK_Audio_Bot
{
    class Repository
    {
        const string VK_Access_Token = "3c93247fc8645ca6742d14745878c326ba99f0b8c7e0d2037519323d90f03a17aef3354ce11acbd17fb8b";
        const string SearchUrl = "https://api.vk.com/method/audio.search?q=Metallica&auto_complete=1&lyrics=0&performer_only=0&sort=2&search_own=0&count=5&access_token=" + VK_Access_Token;
    }
}
