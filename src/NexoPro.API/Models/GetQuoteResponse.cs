using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoPro.API.Models
{
    public class GetQuoteResponse
    {
        public string Pair { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }

        /// <summary>
        /// The timestamp of quote in UTC.
        /// </summary>
        [JsonIgnore]
        public DateTime Timestamp => DateTimeOffset.FromUnixTimeSeconds(TimestampUnixSeconds).DateTime;

        [JsonProperty("timestamp")]
        internal long TimestampUnixSeconds  { get; set; }
    }
}
