using System.Collections.Concurrent;
using XSOBack;

namespace Sample
{
    public class RateLimitMiddleware : IMiddleware
    {
        private readonly int _requestsLimit;
        private readonly TimeSpan _timeSpan;
        private static readonly ConcurrentDictionary<string, RequestCounter> _requests = new ConcurrentDictionary<string, RequestCounter>();

        public RateLimitMiddleware(int requestsLimit, TimeSpan timeSpan)
        {
            _requestsLimit = requestsLimit;
            _timeSpan = timeSpan;
        }

        public async Task InvokeAsync(RequestContext context, Func<Task> next)
        {
            var clientIp = context.Request.RemoteEndPoint.ToString();
            var requestCounter = _requests.GetOrAdd(clientIp, _ => new RequestCounter(DateTime.UtcNow));

            lock (requestCounter)
            {
                if (DateTime.UtcNow - requestCounter.LastRequest < _timeSpan)
                {
                    if (requestCounter.Count >= _requestsLimit)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                        XSOBackUtilities.XSOBackLog($"Too many requests from the same IP {clientIp}", typeof(RateLimitMiddleware));
                        return;
                    }

                    requestCounter.Count++;
                }
                else
                {
                    requestCounter.Count = 1;
                    requestCounter.LastRequest = DateTime.UtcNow;
                }
            }

            await next();
        }

        private class RequestCounter
        {
            public DateTime LastRequest { get; set; }
            public int Count { get; set; }

            public RequestCounter(DateTime lastRequest)
            {
                LastRequest = lastRequest;
                Count = 1;
            }
        }
    }
}
