namespace XSOBack
{
    public class LoggingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(RequestContext context, Func<Task> next)
        {
            XSOBackUtilities.XSOBackLog($"Handling request: {context.Request.Url}", typeof(LoggingMiddleware));
            await next();
            XSOBackUtilities.XSOBackLog($"Response status code: {context.Response.StatusCode}", typeof(LoggingMiddleware));
        }
    }
}