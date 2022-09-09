using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoPro.API.Models
{
    public class GetAccountBalancesResponse
    {
        public List<WalletBalanceEntry> Balances { get; set; } = new();
    }

    public class WalletBalanceEntry
    {
        public string AssetName { get; set; }
        public double TotalBalance { get; set; }
        public double AvailableBalance { get; set; }
        public double LockedBalance { get; set; }
    }
}
