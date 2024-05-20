using System.Net;

namespace XSOBack
{
    public delegate void RequestHandler(RequestContext context);
    public class Router
    {
        private readonly Dictionary<string, RequestHandler> _routes = new Dictionary<string, RequestHandler>();
        private readonly List<IMiddleware> globalMiddlewares = new List<IMiddleware>();

        public void RegisterHandler(Route route)
        {
            string key = CreateRouteKey(route.Method.ToString(), route.Path);
            _routes[key] = route.Handler;
        }

        public void UseGlobalMiddleware(IMiddleware middleware)
        {
            globalMiddlewares.Add(middleware);
        }

        public async void HandleRequest(HttpListenerContext httpContext)
        {
            string key = CreateRouteKey(httpContext.Request.HttpMethod, httpContext.Request.Url.AbsolutePath);

            var context = new RequestContext(httpContext);

            foreach (var middleware in globalMiddlewares)
            {
                context.Use(middleware);
            }

            RequestHandler handler = _routes.TryGetValue(key, out RequestHandler specificHandler) ? specificHandler : ctx => ctx.NotFound("Endpoint not found");
            context.Use(new FinalHandlerMiddleware(handler));

            await context.InvokeMiddleware();
        }

        private static string CreateRouteKey(string method, string route)
        {
            return $"{method.ToUpper()} {route.ToLower()}";
        }
    }
}