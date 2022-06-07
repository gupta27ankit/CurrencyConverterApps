using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts
{
    public class ExchangeRateDetails
    {
        public bool success { get; set; }
        public string @base { get; set; }
        public DateTime date { get; set; }
        [JsonIgnore] public long timestamp { private get; set; }
        public DateTime downloadTimestamp => DateTimeOffset.FromUnixTimeSeconds(timestamp).LocalDateTime;
        public Dictionary<string, decimal> rates { get; set; }
    }
}
