using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK_Audio_Bot.DTO.Response
{
    class TrackInfo
    {
       [JsonProperty("items")]
        public Item[] Items { get; set; }
    }
}
