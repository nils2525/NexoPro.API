using NexoPro.API.Enums;
using NexoPro.API.Models;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NexoPro.API
{
    public class Client
    {
        private readonly Uri _baseUri;
        private HttpClient _client;
        private NonceProvider _nonceProvider;
        private byte[] _secret;
        public Client(string key, string secret, string address = "https://pro-api.nexo.io")
        {
            _baseUri = new Uri(address);
            _secret = Encoding.UTF8.GetBytes(secret);
            _nonceProvider = new NonceProvider();
            _client = new HttpClient(new HttpClientHandler());
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("github", "nils2525"));
            _client.DefaultRequestHeaders.Add("X-API-KEY", key);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        private Uri GenerateRequestUri(string path)
        {
            return new Uri("https://pro-api.nexo.io" + path);
        }

        private string SignData(string data)
        {
            using var encryptor = new HMACSHA256(_secret);
            var resultBytes = encryptor.ComputeHash(data != null ? Encoding.UTF8.GetBytes(data) : null);
            return Convert.ToBase64String(resultBytes);//BytesToHexString(resultBytes);
        }

        private string GeneratePath(string path, params (string, object)[] queryParams)
        {
            return path + (queryParams.Count() > 0 ? "?" +
                String.Join("&", queryParams.Where(c => !String.IsNullOrWhiteSpace(c.Item2.ToString()))
                .Select(p => p.Item1 + "=" + HttpUtility.UrlEncode(p.Item2.ToString()))) : String.Empty);
        }

        private async Task<TResult> SendInternalAsync<TResult>(string path)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, GenerateRequestUri(path));
            var nonce = _nonceProvider.GetNonce().ToString();
            message.Headers.Add("X-SIGNATURE", SignData(nonce));
            message.Headers.Add("X-NONCE", nonce);

            var response = await _client.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                var serializer = new JsonSerializer();
                var stream = await response.Content.ReadAsStreamAsync();
                using (var sr = new StreamReader(stream))
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    var resultObj = serializer.Deserialize(jsonTextReader, typeof(TResult));
                    return resultObj != null ? (TResult)resultObj : throw new Exception("Cant parse object.");
                }
            }
            else
            {
                var rawData = await response.Content.ReadAsStringAsync();
                throw new Exception(rawData);
            }
        }

        /// <summary>
        /// Returns a list of all pairs that can be traded on Nexo Pro, along with the minimum and maximum amounts that can be traded per trade.
        /// </summary>
        public async Task<GetPairsResponse> GetPairsAsync()
        {
            return await SendInternalAsync<GetPairsResponse>("/api/v1/pairs");
        }

        /// <summary>
        /// Get an approximate current price for the provided pair, side and amount.
        /// </summary>
        public async Task<GetQuoteResponse> GetQuoteAsync(string pair, double amount, OrderSide side, params string[] exchanges)
        {
            var query = GeneratePath("/api/v1/quote", ("pair", pair), ("amount", amount), ("side", side.ToString().ToLower()), ("exchanges", String.Join(",", exchanges)));
            return await SendInternalAsync<GetQuoteResponse>(query);
        }

        /// <summary>
        /// Retrieves a list of available assets and their corresponding balances.
        /// </summary>
        public async Task<GetAccountBalancesResponse> GetAccountBalancesAsync()
        {
            return await SendInternalAsync<GetAccountBalancesResponse>("/api/v1/accountSummary");
        }

        /// <summary>
        /// Retrieves Nexo platform balances.
        /// </summary>
        /// <returns></returns>
        public async Task<GetPlatformBalancesResponse> GetPlatformBalancesAsync()
        {
            return await SendInternalAsync<GetPlatformBalancesResponse>("/api/v1/platform/balances");
        }

    }
}
