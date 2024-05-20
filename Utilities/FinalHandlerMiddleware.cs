namespace XSOBack
{
    public class FinalHandlerMiddleware : IMiddleware
    {
        private readonly RequestHandler _handler;

        public FinalHandlerMiddleware(RequestHandler handler)
        {
            _handler = handler;
        }

        public async Task InvokeAsync(RequestContext context, Func<Task> next)
        {
            _handler(context);
            await next();
        }
    }
}