using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parasite.Common
{
    public class Response
    {
        [JsonProperty("type")]
        public ResponseType Type;

        [JsonProperty("data")]
        public string Data;

        public static Response FromString(string input)
        {
            return JsonConvert.DeserializeObject<Response>(input);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public enum ResponseType
    {
        Basic
    }
}
