
namespace NexoPro.API.Test
{
    public class Tests
    {
        private Client _client;
        public Tests()
        {
            _client = new Client("", "");
        }

        [Fact]
        public async Task GetPairsTestAsync()
        {
           var result = await _client.GetPairsAsync();
        }

        [Fact]
        public async Task GetQuoteTestAsync()
        {
            var result = await _client.GetQuoteAsync("BTC/USDT", 1, Enums.OrderSide.Buy);
        }

        [Fact]
        public async Task GetAccountBalancesTestAsync()
        {
            var result = await _client.GetAccountBalancesAsync();
        }

        [Fact]
        public async Task GetPlatformBalancesTestAsync()
        {
            var result = await _client.GetPlatformBalancesAsync();
        }
    }
}