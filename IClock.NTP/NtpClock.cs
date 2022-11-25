using GuerrillaNtp;
using System;
using System.Net;
using System.Threading;

namespace IClock.NTP
{
    public class NtpClock : ITimeProvider, IDisposable
    {
        private readonly NtpClient _client;
        private readonly Timer _timer;
        private readonly ITimeProvider _fallback;
        private bool _disposed;

        public NtpClock(TimeSpan? syncInterval = null, TimeSpan? timeout = null, ITimeProvider? fallback = null)
           : this(NtpClient.DefaultEndpoint, syncInterval, timeout, fallback) { }

        public NtpClock(string host, TimeSpan? syncInterval = null, TimeSpan? timeout = null, ITimeProvider? fallback = null)
            : this(host, NtpClient.DefaultPort, syncInterval, timeout, fallback) { }

        public NtpClock(string host, int port, TimeSpan? syncInterval = null, TimeSpan? timeout = null, ITimeProvider? fallback = null)
            : this(new DnsEndPoint(host, port), syncInterval, timeout, fallback) { }

        public NtpClock(EndPoint endPoint, TimeSpan? syncInterval = null, TimeSpan? timeout = null, ITimeProvider? fallback = null)
        {
            _client = new NtpClient(endPoint, timeout);
            _timer = new Timer(Resync);
            _timer.Change(TimeSpan.Zero, syncInterval ?? TimeSpan.FromMinutes(15));
            _fallback = fallback ?? new UtcClock();
        }

        private void Resync(object? stateInfo)
            =>_client.Query();

        public DateTimeOffset GetTime()
            => _client.Last?.UtcNow ?? _fallback.GetTime();

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _timer?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
