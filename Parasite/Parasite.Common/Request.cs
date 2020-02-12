using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parasite.Common
{
    public class Request
    {
        [JsonProperty("type")]
        public RequestType Type;

        [JsonProperty("data")]
        public string Data;

        public static Request FromString(string input)
        {
            return JsonConvert.DeserializeObject<Request>(input);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public enum RequestType
    {
        Command,
        Code
    }
}
