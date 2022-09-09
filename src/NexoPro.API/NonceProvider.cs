using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoPro.API
{
    internal class NonceProvider
    {
        private static readonly object _nonceLock = new();
        private static long? _lastNonce;

        public long GetNonce()
        {
            lock (_nonceLock)
            {
                var nonce = (long)Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds * 1000);
                if (_lastNonce.HasValue && nonce <= _lastNonce.Value)
                {
                    nonce = _lastNonce.Value + 1;
                }
                _lastNonce = nonce;
                return nonce;
            }
        }
    }
}
