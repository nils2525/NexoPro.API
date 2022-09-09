using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoPro.API.Models
{
    public class GetPlatformBalancesResponse
    {
        public List<PlatformBalanceEntry> Balances { get; set; } = new();
    }

    public class PlatformBalanceEntry
    {
        public string AssetName { get; set; }
        public double AvailableBalance { get; set; }
    }
}
