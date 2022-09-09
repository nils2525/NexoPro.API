using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoPro.API.Models
{
    public class GetPairsResponse
    {
        [JsonProperty("pairs")]
        public List<string> Pairs { get; set; } = new();


        [JsonProperty("min_limits")]
        public Dictionary<string, double> MinLimits { get; set; } = new();


        [JsonProperty("max_limits")]
        public Dictionary<string, double?> MaxLimits { get; set; } = new();
    }
}
